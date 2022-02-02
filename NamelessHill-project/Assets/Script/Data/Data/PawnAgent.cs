using Nameless.Agent;
using Nameless.Data;
using Nameless.DataMono;
using Nameless.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nameless.Data
{
    public enum PropertyState
    {
        Supporting = 0,
        Costing = 1,
    }

    public enum BattleState
    {
        Normal = 0,
        Pinch = 1,
        Surround = 2,
    }
    public enum BattleSide
    {
        Peace = 0,
        Attacker = 1,
        Defender = 2,
    }
    public class PawnAgent
    {
        public FrontPlayer frontPlayer;
        public struct RunningTimeCostProperty
        {
            public float costRMorale;
            public float costBMorale;


            public float costRTimeMorale;
            public float costBTimeMorale;


            public float addMorale;

            public int addAmmo;

            public float addTimeMorale;

            public float addTimeAmmo;
            public RunningTimeCostProperty(float costRMorale, float costBMorale, float costRTimeMorale, float costBTimeMorale,  float addMorale, int addAmmo, float addTimeMorale,float addTimeAmmo)
            {
                this.costRMorale = costRMorale;
                this.costBMorale = costBMorale;

                this.addMorale = addMorale;

                this.addAmmo = addAmmo;

                this.costRTimeMorale = costRTimeMorale;
                this.costBTimeMorale = costBTimeMorale;


                this.addTimeMorale = addTimeMorale;

                this.addTimeAmmo = addTimeAmmo;
            }

            public void Init(float costRMorale, float costBMorale, float costRTimeMorale, float costBTimeMorale,  float addMorale, int addAmmo, float addTimeMorale, float addTimeAmmo)

            {
                this.costRMorale = costRMorale;
                this.costBMorale = costBMorale;


                this.addMorale = addMorale;

                this.addAmmo = addAmmo;

                this.costRTimeMorale = costRTimeMorale;
                this.costBTimeMorale = costBTimeMorale;


                this.addTimeMorale = addTimeMorale;

                this.addTimeAmmo = addTimeAmmo;
            }
        }
        public struct BattleInfo
        {
            public float actualAttack;
            public float actualDefend;
            public float moraleRate;
            public FightSkillType skillType;
            public BattleInfo(float actualAttack, float actualDefend,float moraleRate, FightSkillType skillType)
            {
                this.actualAttack = actualAttack;
                this.actualDefend = actualDefend;
                this.moraleRate = moraleRate;
                this.skillType = skillType;
            }
        }

        public PropertyState state;
        public Slider healthBar;
        public Pawn pawn;
        public string rank = "Soldier";

        #region//��ɫ�¼�
        public Action<float> HealthBarEvent;
        public Action<PawnAgent> MoraleBarEvent;
        public Action<PawnAgent> AmmoBarEvent;
        public Action<float> AttackValueEvent;
        public Action<float> DefendValueEvent;
        public Action<float> SpeedValueEvent;
        public Action<float> HitValueEvent;
        public Action<float> DexValueEvent;
        #endregion

        #region//��ɫ����
        public RunningTimeCostProperty runningTimeProperty;


        //private float countCTimeMorale = 0.0f;
        private float countBTimeMorale = 0.0f;
        private float countConTimeMorale = 0.0f;

        private float countSTimeAmmo = 0.0f;
        #endregion

        #region//��ɫս��״̬
        public Dictionary<PawnAvatar,  BattleSide> battleSideDic = new Dictionary<PawnAvatar, BattleSide>();
        //public BattleState BattleState
        //{
        //    set
        //    {
        //        if(this.battleState == BattleState.Surround)
        //        {
        //            AudioManager.Instance.PlayAudio(this.curOpponent.gameObject.transform, "Surround");
        //        }
        //        else if(this.battleState == BattleState.Pinch)
        //        {
        //            AudioManager.Instance.PlayAudio(this.curOpponent.gameObject.transform, "Pinch");
        //        }
        //        this.battleState = value;
        //    }
        //    get
        //    {
        //        return this.battleState;
        //    }
        //}
        //private BattleState battleState = BattleState.Normal;
        public BattleInfo battleInfo;
        public List<PawnAvatar> opponents = new List<PawnAvatar>();//�������Լ�ս�������н�ɫ
        public PawnAvatar curOpponent = null;//�Լ����ڹ����Ľ�ɫ
        public bool opponentIsInBattle = false;//�Է��Ƿ��ڹ���������ɫ
        public Dictionary<Area, int> aroundOppoNum = new Dictionary<Area, int>();
        public List<PawnAvatar> supporters = new List<PawnAvatar>();

        #endregion

        #region//��ɫ���ܺ�buff
        public List<Buff> buffs;
        #endregion

        #region//��ɫ�Ի�
        public DialogueGroup dialogueGroup;
        #endregion


        public PawnAgent(FrontPlayer frontPlayer, Slider healthBar,Area currentArea, Pawn pawn, int mapId)
        {
            this.frontPlayer = frontPlayer;
            this.healthBar = healthBar;
            this.pawn = pawn;
            this.healthBar.value = this.pawn.curHealth / this.pawn.maxHealth;
            if (currentArea.type == AreaType.Base)
            {
                this.state = PropertyState.Supporting;//��ʱ���ݵ�ǰ���������ȥ�ж��Ƿ����Ļ��߲���
                BaseArea baseArea = (BaseArea)currentArea;
                this.runningTimeProperty = new RunningTimeCostProperty(0, 0, 0, 0, 0, baseArea.supportAmmo,baseArea.supportDeltaTime, baseArea.supportDeltaTime);
            }
            else if (currentArea.type == AreaType.Normal || currentArea.type == AreaType.Spawn)
            {
                this.state = PropertyState.Costing;//��ʱ���ݵ�ǰ���������ȥ�ж��Ƿ����Ļ��߲���
                this.runningTimeProperty = new RunningTimeCostProperty(currentArea.costMorale, -1.0f, currentArea.costTimeMorale, 1.0f, 0, 0, 0, 0);
            }

            this.buffs = new List<Buff>();
            this.supporters = new List<PawnAvatar>();
            if (this.pawn.dialogueDic.ContainsKey(mapId))
                this.dialogueGroup = this.pawn.dialogueDic[mapId];
            else
                this.dialogueGroup = null;
            
            //this.countCTimeMorale = 0.0f;
            this.countBTimeMorale = 0.0f;
            this.countConTimeMorale = 0.0f;
            this.battleSideDic = new Dictionary<PawnAvatar, BattleSide>();
            ResetBattleInfo();
        }

        
        // Start is called before the first frame update

        // Update is called once per frame
        #region//ʵʱ����
        public void RunningTimePropertyUpdate(PawnState state)
        {
            
            if(state == PawnState.Battle)
            {
                if (this.countConTimeMorale > 5.0f)
                {
                    if(this.countBTimeMorale > this.runningTimeProperty.costBTimeMorale)
                    {
                        this.countBTimeMorale = 0.0f;
                        this.MoraleChange(0);
                    }
                    else
                    {
                        this.countBTimeMorale += Time.deltaTime;
                    }
                }
                else
                {
                    this.countConTimeMorale += Time.deltaTime;
                }
            }
            else
            {
                this.countConTimeMorale = 0.0f;
            }
            if (this.state == PropertyState.Costing)
            {

            }
            else if(this.state == PropertyState.Supporting)
            {
                //��ȷ��
                //if (this.countSTimeAmmo > this.runningTimeProperty.addTimeAmmo)
                //{
                //    this.countSTimeAmmo = 0.0f;
                //    if (GameManager.Instance.totalMilitaryRes > 0)//���޸�
                //    {
                //        this.AmmoChange(this.runningTimeProperty.addAmmo);
                //        GameManager.Instance.ChangeMilitaryRes((int)this.runningTimeProperty.addAmmo);//���޸�
                //    }
                //}
                //else
                //{
                //    this.countSTimeAmmo += Time.deltaTime;
                //}
            }
        }

        public void ResetRunningTimeProperty(Area currentArea)
        {
            if (currentArea.type == AreaType.Base)
            {
                this.state = PropertyState.Supporting;//��ʱ���ݵ�ǰ���������ȥ�ж��Ƿ����Ļ��߲���
                BaseArea baseArea = (BaseArea)currentArea;
                this.runningTimeProperty.Init(0, 0, 0, 0, 0,  baseArea.supportAmmo, baseArea.supportDeltaTime, baseArea.supportDeltaTime);
            }
            else if (currentArea.type == AreaType.Normal || currentArea.type == AreaType.Spawn)
            {
                this.state = PropertyState.Costing;//��ʱ���ݵ�ǰ���������ȥ�ж��Ƿ����Ļ��߲���
                this.runningTimeProperty.Init(currentArea.costMorale, -1.0f, currentArea.costTimeMorale, 1.0f, 0, 0, 0, 0);
            }
        }//����ʵʱ�仯�Ľ�ɫ����
        #endregion

        #region//��ɫս��
        public void ChooseMyOpponents(PawnAvatar mine)//ѡ���������ڴ��ҵĵ���ս��
        {
            int index = UnityEngine.Random.Range(0, this.opponents.Count - 1);
            mine.UpdateCurrentOppo(this.opponents[index]);
            this.opponents[index].pawnAgent.opponentIsInBattle = false;
        }
        public void ResetBattleInfo()
        {
            //this.BattleState = BattleState.Normal;
            
            this.opponents = new List<PawnAvatar>();
            this.curOpponent = null;
            this.opponentIsInBattle = false;
            this.aroundOppoNum = new Dictionary<Area, int>();
        }
        public bool CheckIfPinch()
        {
            if (this.aroundOppoNum.Count == 2)
                return true;
            return false;
        }
        public void AddBuff(Buff buff)
        {
            this.buffs.Add(buff);
        }
        public void RemoveBuff(Buff buff)
        {
            this.buffs.Remove(buff);
        }
        #endregion

        #region//��ɫ����
        public void MoraleChange(float valueChange)
        {
            this.pawn.curMorale += valueChange;
            if (this.MoraleBarEvent != null)
                this.MoraleBarEvent(this);
        }
        public float MoralteState()
        {
            float curMorale = this.pawn.curMorale;
            float maxMorale = this.pawn.maxMorale;
            if (curMorale >= maxMorale / 2)
            {
                return 1.5f;
            }
            else if (maxMorale / 4 <= curMorale && curMorale < maxMorale / 2)
            {
                return 1.0f;
            }
            else
            {
                return 0.5f;
            }
        }
        public void HealthChange(float valuechange)
        {
            this.pawn.curHealth += valuechange;
            this.healthBar.value = this.pawn.curHealth / this.pawn.maxHealth;
            if (this.HealthBarEvent != null)
                this.HealthBarEvent(this.healthBar.value);

        }
        public void AmmoChange(int valueChange)
        {
            if(this.pawn.curAmmo == 0 && valueChange < 0)
            {
                this.pawn.curAmmo = 0;
            }
            else if (this.pawn.curAmmo == 0 && valueChange > 0)
            {
                this.pawn.curAmmo += valueChange;
                this.InitAttack(this.pawn.curAttack / 0.2f);

            }
            else
            {
                this.pawn.curAmmo += valueChange;
                if (this.pawn.curAmmo <= 0)
                {
                    this.InitAttack(this.pawn.curAttack * 0.2f);
                }
            }
            if (this.AmmoBarEvent != null)
                this.AmmoBarEvent(this);

        }

        public void InitMorale(float value)
        {
            this.pawn.curMorale  = value;
            if (this.MoraleBarEvent != null)
                this.MoraleBarEvent(this);
        }
        public void InitHealth (float value)
        {
            this.pawn.curHealth = value;
            this.healthBar.value = this.pawn.curHealth / this.pawn.maxHealth;
            if (this.HealthBarEvent != null)
                this.HealthBarEvent(this.healthBar.value);
        }
        public void InitAmmo(float value)
        {
            this.pawn.curAmmo = value;
            if(this.pawn.curAmmo <= 0)
            {
                this.InitAttack(this.pawn.curAttack * 0.2f);
            }
            if (this.AmmoBarEvent != null)
                this.AmmoBarEvent(this);
        }

        public void InitAttack(float value)
        {
            this.pawn.curAttack = value;
            if (this.AttackValueEvent != null)
                this.AttackValueEvent(this.pawn.curAttack);
        }
        #endregion

        //��ɫӪ��
        #region
        public void PushConversation(Conversation conversation)
        {
            this.pawn.conversationsInCamp.Push(conversation);
        }
        #endregion

        public List<Skill> GetSkills()
        {
            return this.pawn.skills;
        }
        public Stack<Conversation> GetConversations()
        {
            return this.pawn.conversationsInCamp;
        }
    }
}