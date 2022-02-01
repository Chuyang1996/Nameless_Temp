using Nameless.Data;
using Nameless.DataMono;
using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.DataMono
{
    
    public class BattlePawnRound : MonoBehaviour
    {
        public BattlePawn battle;
        public SpriteRenderer sprite;
        public SpriteRenderer arrowSprite;
        private PawnAvatar attacker;
        private PawnAvatar defender;
        public void Init(PawnAvatar attacker, PawnAvatar defender)
        {
            
            this.attacker = attacker;
            this.defender = defender;
            this.name = this.attacker.gameObject.name + " vs " + this.defender.gameObject.name;
            this.arrowSprite.color = attacker.pawnAgent.frontPlayer.faction.battleColor;
            this.CalculatePosition();
            this.attacker.pawnAgent.battleSideDic.Add(this.defender,BattleSide.Attacker);
            this.defender.pawnAgent.battleSideDic.Add(this.attacker, BattleSide.Defender);

            this.attacker.pawnAgent.opponents.Add(this.defender);
            this.defender.pawnAgent.opponents.Add(this.attacker);

            this.battle = new BattlePawn(this.attacker, this.defender);
            BattleManager.Instance.battlePawnDic.Add(this.battle, this);
        }

        public IEnumerator ProcessBattle(bool defenderisInBattle)
        {
            this.attacker.pawnAgent.opponentIsInBattle = defenderisInBattle;
            //this.defender.pawnAgent.opponentIsInBattle = true;
            float attackerTC = 0.0f;
            float defenderTC = 0.0f;
            EventTriggerManager.Instance.CheckPawnStartBattle(this.attacker.pawnAgent.pawn.id);
            EventTriggerManager.Instance.CheckPawnStartBattle(this.defender.pawnAgent.pawn.id);
            while (true)
            {
                if (!GameManager.Instance.isPlay)
                {
                    yield return null;
                }
                else
                {
                    //if (this.attacker == null || this.defender == null)
                    //    break;

                    if (this.attacker == null || this.defender == null || this.IsTheBattleEnd())
                        break;
                    this.attacker.CalcuateBattleInfo();//计算本次战斗实际的角色数据
                    this.defender.CalcuateBattleInfo();

                    attackerTC += Time.deltaTime;
                    if (!this.attacker.pawnAgent.opponentIsInBattle)
                        defenderTC += Time.deltaTime;
                    
                    if (attackerTC >= this.attacker.pawnAgent.pawn.curAtkSpeed)
                    {
                        attackerTC = 0.0f;
                        this.CalculateBattle(this.attacker, this.defender);
                    }

                    if (defenderTC >= this.defender.pawnAgent.pawn.curAtkSpeed)
                    {
                        defenderTC = 0.0f;
                        this.CalculateBattle(this.defender, this.attacker);
                    }
                    yield return null;
                }
            }
            BattleManager.Instance.battlePawnDic.Remove(this.battle);


            if (this != null)
                DestroyImmediate(this.gameObject);
        }

        void CalculateBattle(PawnAvatar attcker, PawnAvatar attackRecever)
        {
            if (attackRecever.currentArea.buildAvatar == null)//被攻击方没有建筑的时候
            {
                attcker.pawnAgent.AmmoChange(-1);
                //attcker.currentArea.CostAmmo(this.attacker);
                float attackerAtk = attcker.pawnAgent.battleInfo.actualAttack;
                float defenderDef = attackRecever.pawnAgent.battleInfo.actualDefend;
                float moraleRate = attcker.pawnAgent.battleInfo.moraleRate;

                float hitRate = 50.0f + attacker.pawnAgent.pawn.curHit - attackRecever.pawnAgent.pawn.curDex;
                float finalHit = Random.Range(0, 100);

                float damage = (attackerAtk - defenderDef) * moraleRate; /* * this.attacker.pawnAgent.pawn.curMorale / this.attacker.pawnAgent.pawn.maxMorale*/;
                if (damage < 0 || finalHit > hitRate)
                    damage = 0;
                attackRecever.pawnAgent.HealthChange(-damage);
            }
            else//被攻击方有建筑的时候
            {
                attacker.pawnAgent.AmmoChange(-1);
                float attackerAtk = attacker.pawnAgent.battleInfo.actualAttack;
                float moraleRate = attacker.pawnAgent.battleInfo.moraleRate;
                float damage = attackerAtk * moraleRate;
                attackRecever.currentArea.buildAvatar.HealthChange(-damage);
                this.CheckIfBuildDestory(attackRecever.currentArea.buildAvatar);
            }
            //attackRecever.currentArea.CostMedicine(this.defender);
        }
        void CheckIfBuildDestory(BuildAvatar buildAvatar)
        {
            if (buildAvatar.IsFail())
            {
                if (buildAvatar.pawnOpponents.Count == 0)
                    buildAvatar.CheckResult();
            } 
        }
        bool IsTheBattleEnd()
        {
            bool isEnd = false;
            if (this.defender.IsFail())
            {
                this.defender.pawnAgent.opponents.Remove(this.attacker);
                this.defender.pawnAgent.battleSideDic.Remove(this.attacker);
                this.attacker.pawnAgent.opponents.Remove(this.defender);
                this.attacker.pawnAgent.battleSideDic.Remove(this.defender);

                if (this.defender.pawnAgent.opponents.Count == 0)//判断是否可以进行战斗结算
                {
                    this.defender.CheckResult();
                }
                this.attacker.CheckIfBattleResult();//检查周围是否还有其他的敌人正在攻击自己

                isEnd = true;
                //this.defender.gameObject.SetActive(false);
            }
            else if (this.attacker.IsFail())
            {
                this.defender.pawnAgent.opponents.Remove(this.attacker);
                this.defender.pawnAgent.battleSideDic.Remove(this.attacker);
                this.attacker.pawnAgent.opponents.Remove(this.defender);
                this.attacker.pawnAgent.battleSideDic.Remove(this.defender);

                if (this.attacker.pawnAgent.opponents.Count == 0)//判断是否可以进行战斗结算
                {
                    this.attacker.CheckResult();
                }
                this.defender.CheckIfBattleResult();//检查周围是否还有其他的敌人正在攻击自己
                isEnd = true;
                //this.attacker.gameObject.SetActive(false); 
            }
            else if(this.defender.State == PawnState.Walk || this.defender.State == PawnState.Draw || this.defender.State == PawnState.Wait)
            {
                this.defender.pawnAgent.opponents.Remove(this.attacker);
                this.defender.pawnAgent.battleSideDic.Remove(this.attacker);
                this.attacker.pawnAgent.opponents.Remove(this.defender);
                this.attacker.pawnAgent.battleSideDic.Remove(this.defender);

                this.defender.pawnAgent.MoraleChange(0);
                this.attacker.CheckIfBattleResult();//检查周围是否还有其他的敌人正在攻击自己
                isEnd = true;
            }
            else if (this.attacker.State == PawnState.Walk || this.attacker.State == PawnState.Draw || this.attacker.State == PawnState.Wait)
            {
                this.defender.pawnAgent.opponents.Remove(this.attacker);
                this.defender.pawnAgent.battleSideDic.Remove(this.attacker);
                this.attacker.pawnAgent.opponents.Remove(this.defender);
                this.attacker.pawnAgent.battleSideDic.Remove(this.defender);


                this.attacker.pawnAgent.MoraleChange(0);
                this.defender.CheckIfBattleResult();//检查周围是否还有其他的敌人正在攻击自己
                isEnd = true;
                //this.attacker.gameObject.SetActive(false); 
            }
            return isEnd ;
        }


        void CalculatePosition()
        {
            Vector3 dir3 =   this.defender.transform.position - this.attacker.transform.position ;
            Vector3 cross = Vector3.Cross(Vector3.up, dir3);
            float angle = cross.z > 0 ? Vector2.Angle(Vector3.up, dir3) : -Vector2.Angle(Vector3.up, dir3);
            this.arrowSprite.transform.localEulerAngles = new Vector3(0, 0, angle);
            this.transform.position = (this.attacker.transform.position + this.defender.transform.position) / 2;
            //float dot = Vector2.Dot(dir3,)
        }

    }
}