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
        public SpriteRenderer sprite;
        public SpriteRenderer arrowSprite;
        private PawnAvatar attacker;
        private PawnAvatar defender;
        private bool forceEnd = false;
        public void Init(PawnAvatar attacker, PawnAvatar defender)
        {

            this.attacker = attacker;
            this.defender = defender;
            this.name = this.attacker.gameObject.name + " vs " + this.defender.gameObject.name;
            this.arrowSprite.color = this.attacker.isAI ? Color.red : Color.green;
            this.CalculatePosition();
            this.attacker.pawnAgent.battleSideDic.Add(this.defender,BattleSide.Attacker);
            this.defender.pawnAgent.battleSideDic.Add(this.attacker, BattleSide.Defender);

            this.attacker.pawnAgent.opponents.Add(this.defender);
            this.defender.pawnAgent.opponents.Add(this.attacker);

            this.battle = new Battle(this.attacker, this.defender);
            this.forceEnd = false;
        }

        public IEnumerator ProcessBattle(bool defenderisInBattle)
        {
            this.attacker.pawnAgent.opponentIsInBattle = defenderisInBattle;
            //this.defender.pawnAgent.opponentIsInBattle = true;
            bool attackerTurn = true;


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

                    if (this.IsTheBattleEnd())
                        break;
                    this.attacker.CalcuateBattleInfo();//计算本次战斗实际的角色数据
                    this.defender.CalcuateBattleInfo();

                    if (attackerTurn || this.attacker.pawnAgent.opponentIsInBattle)
                    {
                        this.attacker.pawnAgent.AmmoChange(-1);
                        this.attacker.currentArea.CostAmmo(this.attacker);
                        float attackerAtk = this.attacker.pawnAgent.battleInfo.actualAttack;
                        float defenderDef = this.defender.pawnAgent.battleInfo.actualDefend;

                        float damage = (attackerAtk - defenderDef)/* * this.attacker.pawnAgent.pawn.curMorale / this.attacker.pawnAgent.pawn.maxMorale*/;
                        if (damage < 0)
                            damage = 0;
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
                        if (damage < 0)
                            damage = 0;
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
                this.defender.pawnAgent.battleSideDic.Remove(this.attacker);
                this.attacker.pawnAgent.opponents.Remove(this.defender);
                this.attacker.pawnAgent.battleSideDic.Remove(this.defender);

                if (this.defender.pawnAgent.opponents.Count == 0)//判断是否可以进行战斗结算
                {
                    this.defender.CheckResult(true);
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
                    this.attacker.CheckResult(false);
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

                this.defender.pawnAgent.MoraleChange(-10.0f);
                this.attacker.CheckIfBattleResult();//检查周围是否还有其他的敌人正在攻击自己
                this.forceEnd = true;
            }
            else if (this.attacker.State == PawnState.Walk || this.attacker.State == PawnState.Draw || this.attacker.State == PawnState.Wait)
            {
                this.defender.pawnAgent.opponents.Remove(this.attacker);
                this.defender.pawnAgent.battleSideDic.Remove(this.attacker);
                this.attacker.pawnAgent.opponents.Remove(this.defender);
                this.attacker.pawnAgent.battleSideDic.Remove(this.defender);


                this.attacker.pawnAgent.MoraleChange(-10.0f);
                this.defender.CheckIfBattleResult();//检查周围是否还有其他的敌人正在攻击自己
                this.forceEnd = true;
                //this.attacker.gameObject.SetActive(false); 
            }
            return isEnd || this.forceEnd;
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