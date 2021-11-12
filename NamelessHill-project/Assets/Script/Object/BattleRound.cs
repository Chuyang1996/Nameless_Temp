using Nameless.Data;
using Nameless.DataMono;
using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.DataMono
{
    
    public class BattleRound : MonoBehaviour
    {
        public Battle battle;
        private PawnAvatar attacker;
        private PawnAvatar defender;
        private bool forceEnd = false;
        public void Init(PawnAvatar attacker, PawnAvatar defender)
        {
            this.attacker = attacker;
            this.defender = defender;

            this.attacker.pawnAgent.battleSide = BattleSide.Attacker;
            this.defender.pawnAgent.battleSide = BattleSide.Defender;

            this.attacker.pawnAgent.opponents.Add(this.defender);
            this.defender.pawnAgent.opponents.Add(this.attacker);

            this.battle = new Battle(this.attacker, this.defender);
            this.forceEnd = false;
        }

        public IEnumerator ProcessBattle()
        {
            this.attacker.pawnAgent.opponentIsInBattle = true;
            this.defender.pawnAgent.opponentIsInBattle = true;
            bool attackerTurn = true;
            this.attacker.ShowBattleHint(true);
            this.defender.ShowBattleHint(true);

            while (true)
            {
                if (!GameManager.Instance.isPlay)
                {
                    yield return null;
                }
                else
                {
                    if (this.attacker == null || this.defender == null)
                        break;

                    if (this.IsTheBattleEnd())
                        break;
                    this.attacker.CalcuateBattleInfo();//计算本次战斗实际的角色数据
                    this.defender.CalcuateBattleInfo();

                    if (attackerTurn)
                    {
                        this.attacker.pawnAgent.AmmoChange(-1);
                        this.attacker.currentArea.CostAmmo(this.attacker);
                        float attackerAtk = this.attacker.pawnAgent.battleInfo.actualAttack;
                        float defenderDef = this.defender.pawnAgent.battleInfo.actualDefend;

                        float damage = (attackerAtk - defenderDef)/* * this.attacker.pawnAgent.pawn.curMorale / this.attacker.pawnAgent.pawn.maxMorale*/;
                        this.defender.pawnAgent.HealthChange(-damage);
                        this.defender.currentArea.CostMedicine(this.defender);
                    }
                    else
                    {
                        this.defender.pawnAgent.AmmoChange(-1);
                        this.defender.currentArea.CostAmmo(this.defender);
                        float attackerAtk = this.defender.pawnAgent.battleInfo.actualAttack;
                        float defenderDef = this.attacker.pawnAgent.battleInfo.actualDefend;

                        float damage = (attackerAtk - defenderDef) /** this.defender.pawnAgent.pawn.curMorale / this.defender.pawnAgent.pawn.maxMorale*/;
                        this.attacker.pawnAgent.HealthChange(-damage);
                        this.attacker.currentArea.CostMedicine(this.attacker);
                    }
                    attackerTurn = !attackerTurn;
                    yield return new WaitForSecondsRealtime(1.0f);
                }
            }
            BattleManager.Instance.battleDic.Remove(this.battle);

            DestroyImmediate(this.gameObject);
        }

        bool IsTheBattleEnd()
        {
            bool isEnd = false;
            if (this.defender.IsFail())
            {
                this.defender.pawnAgent.opponents.Remove(this.attacker);
                this.attacker.pawnAgent.opponents.Remove(this.defender);
                this.attacker.ShowBattleHint(false);
                this.defender.ShowBattleHint(false);

                if (this.defender.pawnAgent.opponents.Count == 0)//判断是否可以进行战斗结算
                {
                    this.defender.CheckResult(true);
                }
                this.attacker.CheckIfBattleResult(this.defender,true);//检查周围是否还有其他的敌人正在攻击自己

                isEnd = true;
                //this.defender.gameObject.SetActive(false);
            }
            else if (this.attacker.IsFail())
            {
                this.defender.pawnAgent.opponents.Remove(this.attacker);
                this.attacker.pawnAgent.opponents.Remove(this.defender);
                this.attacker.ShowBattleHint(false);
                this.defender.ShowBattleHint(false);

                if (this.attacker.pawnAgent.opponents.Count == 0)//判断是否可以进行战斗结算
                {
                    this.attacker.CheckResult(false);
                }
                this.defender.CheckIfBattleResult(this.attacker,false);//检查周围是否还有其他的敌人正在攻击自己
                isEnd = true;
                //this.attacker.gameObject.SetActive(false); 
            }
            else if(this.defender.State == PawnState.Walk)
            {
                this.defender.pawnAgent.opponents.Remove(this.attacker);
                this.attacker.pawnAgent.opponents.Remove(this.defender);
                this.attacker.ShowBattleHint(false);
                this.defender.ShowBattleHint(false);

                this.defender.pawnAgent.MoraleChange(-10.0f);
                this.attacker.CheckIfBattleResult(this.defender, true);//检查周围是否还有其他的敌人正在攻击自己
                this.forceEnd = true;
            }
            else if (this.attacker.State == PawnState.Walk)
            {
                this.defender.pawnAgent.opponents.Remove(this.attacker);
                this.attacker.pawnAgent.opponents.Remove(this.defender);
                this.attacker.ShowBattleHint(false);
                this.defender.ShowBattleHint(false);

                this.attacker.pawnAgent.MoraleChange(-10.0f);
                this.defender.CheckIfBattleResult(this.attacker, false);//检查周围是否还有其他的敌人正在攻击自己
                this.forceEnd = true;
                //this.attacker.gameObject.SetActive(false); 
            }
            return isEnd || this.forceEnd;
        }

    }
}