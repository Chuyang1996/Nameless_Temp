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
            public BattleInfo(float actualAttack, float actualDefend)
            {
                this.actualAttack = actualAttack;
                this.actualDefend = actualDefend;
            }
        }

        public PropertyState state;
        public Slider healthBar;
        public Pawn pawn;
        public string rank = "Soldier";

        #region//角色事件
        public Action<float> HealthBarEvent;
        public Action<PawnAgent> MoraleBarEvent;
        public Action<PawnAgent> AmmoBarEvent;
        public Action<float> AttackValueEvent;
        public Action<float> DefendValueEvent;
        public Action<float> SpeedValueEvent;
        public Action<float> HitValueEvent;
        public Action<float> DexValueEvent;
        #endregion

        #region//角色属性
        public RunningTimeCostProperty runningTimeProperty;


        //private float countCTimeMorale = 0.0f;
        private float countBTimeMorale = 0.0f;
        private float countConTimeMorale = 0.0f;

        private float countSTimeAmmo = 0.0f;
        #endregion

        #region//角色战斗状态
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
        public List<PawnAvatar> opponents = new List<PawnAvatar>();//正在与自己战斗的所有角色
        public PawnAvatar curOpponent = null;//自己正在攻击的角色
        public bool opponentIsInBattle = false;//对方是否在攻击其他角色
        public Dictionary<Area, int> aroundOppoNum = new Dictionary<Area, int>();
        public List<PawnAvatar> supporters = new List<PawnAvatar>();

        #endregion

        #region//角色技能和buff
        public List<Buff> buffs;
        public List<Skill> skills;
        #endregion

        #region//角色对话
        public DialogueGroup dialogueGroup;
        #endregion

        public PawnAgent(Slider healthBar,Area currentArea, Pawn pawn, int mapId)
        {
            this.healthBar = healthBar;
            this.pawn = pawn;
            this.healthBar.value = this.pawn.curHealth / this.pawn.maxHealth;
            if (currentArea.type == AreaType.Base)
            {
                this.state = PropertyState.Supporting;//暂时根据当前地面的类型去判断是否消耗或者补充
                BaseArea baseArea = (BaseArea)currentArea;
                this.runningTimeProperty = new RunningTimeCostProperty(0, 0, 0, 0, 0, baseArea.supportAmmo,baseArea.supportDeltaTime, baseArea.supportDeltaTime);
            }
            else if (currentArea.type == AreaType.Normal || currentArea.type == AreaType.Spawn)
            {
                this.state = PropertyState.Costing;//暂时根据当前地面的类型去判断是否消耗或者补充
                this.runningTimeProperty = new RunningTimeCostProperty(currentArea.costMorale, -1.0f, currentArea.costTimeMorale, 1.0f, 0, 0, 0, 0);
            }

            this.buffs = new List<Buff>();
            this.skills = new List<Skill>();
            this.supporters = new List<PawnAvatar>();
            if (this.pawn.dialogueDic.ContainsKey(mapId))
                this.dialogueGroup = this.pawn.dialogueDic[mapId];
            else
                this.dialogueGroup = null;
            
            for (int i = 0; i < pawn.fightSkillIds.Count; i++)
            {
                this.skills.Add(SkillFactory.GetSkillById(SkillFactoryType.FightSkill, pawn.fightSkillIds[i]));
            }
            for (int i = 0; i < pawn.supportSkillIds.Count; i++)
            {
                this.skills.Add(SkillFactory.GetSkillById(SkillFactoryType.SupportSkill, pawn.supportSkillIds[i]));
            }
            for (int i = 0; i < pawn.buildSkillIds.Count; i++)
            {
                this.skills.Add(SkillFactory.GetSkillById(SkillFactoryType.BuildSkill, pawn.buildSkillIds[i]));
            }
            //this.countCTimeMorale = 0.0f;
            this.countBTimeMorale = 0.0f;
            this.countConTimeMorale = 0.0f;
            this.battleSideDic = new Dictionary<PawnAvatar, BattleSide>();

            ResetBattleInfo();
        }

        
        // Start is called before the first frame update

        // Update is called once per frame
        #region//实时属性
        public void RunningTimePropertyUpdate(PawnState state)
        {
            
            if(state == PawnState.Battle)
            {
                if (this.countConTimeMorale > 5.0f)
                {
                    if(this.countBTimeMorale > this.runningTimeProperty.costBTimeMorale)
                    {
                        this.countBTimeMorale = 0.0f;
                        this.MoraleChange(this.runningTimeProperty.costBMorale);
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

                if (this.countSTimeAmmo > this.runningTimeProperty.addTimeAmmo)
                {
                    this.countSTimeAmmo = 0.0f;
                    if (GameManager.Instance.totalMilitaryRes > 0)//待修改
                    {
                        this.AmmoChange(this.runningTimeProperty.addAmmo);
                        GameManager.Instance.ChangeMilitaryRes((int)this.runningTimeProperty.addAmmo);//待修改
                    }
                }
                else
                {
                    this.countSTimeAmmo += Time.deltaTime;
                }
            }
        }

        public void ResetRunningTimeProperty(Area currentArea)
        {
            if (currentArea.type == AreaType.Base)
            {
                this.state = PropertyState.Supporting;//暂时根据当前地面的类型去判断是否消耗或者补充
                BaseArea baseArea = (BaseArea)currentArea;
                this.runningTimeProperty.Init(0, 0, 0, 0, 0,  baseArea.supportAmmo, baseArea.supportDeltaTime, baseArea.supportDeltaTime);
            }
            else if (currentArea.type == AreaType.Normal || currentArea.type == AreaType.Spawn)
            {
                this.state = PropertyState.Costing;//暂时根据当前地面的类型去判断是否消耗或者补充
                this.runningTimeProperty.Init(currentArea.costMorale, -1.0f, currentArea.costTimeMorale, 1.0f, 0, 0, 0, 0);
            }
        }//重置实时变化的角色数据
        #endregion

        #region//角色战斗
        public void ChooseMyOpponents(PawnAvatar mine)//选择其他正在打我的敌人战斗
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
        //public void AddSurroundArea(PawnAvatar opponent)
        //{
        //    if (this.aroundOppoNum.ContainsKey(opponent.currentArea))
        //        this.aroundOppoNum[opponent.currentArea]++;
        //    else
        //        this.aroundOppoNum.Add(opponent.currentArea, 1);
        //}
        //public void RemoveSurroundArea(PawnAvatar opponent)
        //{
        //    if (this.aroundOppoNum.ContainsKey(opponent.currentArea))
        //    {
        //        this.aroundOppoNum[opponent.currentArea]--;
        //        if (this.aroundOppoNum[opponent.currentArea] == 0)
        //            this.aroundOppoNum.Remove(opponent.currentArea);
        //    }
        //}
        #endregion

        #region//角色属性
        public void MoraleChange(float valueChange)
        {
            this.pawn.curMorale += valueChange;
            if (this.MoraleBarEvent != null)
                this.MoraleBarEvent(this);

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
    }
}