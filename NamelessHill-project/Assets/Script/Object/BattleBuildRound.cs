using Nameless.Data;
using Nameless.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.DataMono
{
    public class BattleBuildRound : MonoBehaviour
    {
        public BattleBuild battle;
        public SpriteRenderer sprite;
        public SpriteRenderer arrowSprite;
        private PawnAvatar pawnAttacker;
        private BuildAvatar buildDefender;

        public void Init(PawnAvatar pawnAttacker, BuildAvatar buildDefender)
        {
            this.pawnAttacker = pawnAttacker;
            this.buildDefender = buildDefender;
            this.name = this.pawnAttacker.gameObject.name + " vs " + this.buildDefender.gameObject.name;
            this.arrowSprite.color = this.pawnAttacker.pawnAgent.frontPlayer.faction.battleColor;
            this.CalculatePosition();

            this.buildDefender.pawnOpponents.Add(pawnAttacker);
            
            this.battle = new BattleBuild(this.pawnAttacker, this.buildDefender);
        }

        public IEnumerator ProcessBattle()
        {
            //this.defender.pawnAgent.opponentIsInBattle = true;
            //if (this.buildDefender.currentPawn != null 
            //    && FactionManager.Instance.RelationFaction(this.buildDefender.currentPawn.GetFaction(), this.pawnAttacker.GetFaction()) == FactionRelation.Hostility)
            //{
            //    BunkerAvatar bunkerAvatar = (BunkerAvatar)this.buildDefender;
            //    bunkerAvatar.DefendBunker(this.pawnAttacker);
            //}
            float attackerTC = 0.0f;
            EventTriggerManager.Instance.CheckPawnStartBattle(this.pawnAttacker.pawnAgent.pawn.id);
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

                    if (this.pawnAttacker == null || this.buildDefender == null || this.IsTheBattleEnd())
                        break;
                    this.pawnAttacker.CalcuateBattleInfo();//计算本次战斗实际的角色数据

                    attackerTC += Time.deltaTime;

                    if (attackerTC >= this.pawnAttacker.pawnAgent.pawn.curAtkSpeed)
                    {
                        attackerTC = 0.0f;
                        this.CalculateBattle(this.pawnAttacker, this.buildDefender);
                    }
                    yield return null;
                }
            }
            BattleManager.Instance.battleBuildDic.Remove(this.battle);


            if (this != null)
                DestroyImmediate(this.gameObject);
        }

        private void CalculateBattle(PawnAvatar attacker, BuildAvatar attackRecever)
        {
            attacker.pawnAgent.AmmoChange(-1);
            //attcker.currentArea.CostAmmo(this.attacker);
            float attackerAtk = attacker.pawnAgent.battleInfo.actualAttack;
            float moraleRate = attacker.pawnAgent.battleInfo.moraleRate;

            //float hitRate = 50.0f + attacker.pawnAgent.pawn.curHit - attackRecever.pawnAgent.pawn.curDex;
            //float finalHit = UnityEngine.Random.Range(0, 100);

            float damage = attackerAtk * moraleRate; /* * this.attacker.pawnAgent.pawn.curMorale / this.attacker.pawnAgent.pawn.maxMorale*/;
            //if (damage < 0 || finalHit > hitRate)
            //    damage = 0;
            attackRecever.HealthChange(-damage);
        }

        private bool IsTheBattleEnd()
        {
            bool isEnd = false;

            if (this.buildDefender.IsFail())
            {
                this.buildDefender.pawnOpponents.Remove(this.pawnAttacker);

                if (this.buildDefender.pawnOpponents.Count == 0)//判断是否可以进行战斗结算
                {
                    this.buildDefender.CheckResult();
                }
                this.pawnAttacker.CheckIfBattleResult();//检查周围是否还有其他的敌人正在攻击自己

                isEnd = true;
                //this.defender.gameObject.SetActive(false);
            }
            else if (this.pawnAttacker.IsFail())
            {
                this.buildDefender.pawnOpponents.Remove(this.pawnAttacker);
                if (this.pawnAttacker.pawnAgent.opponents.Count == 0)//判断是否可以进行战斗结算
                {
                    this.pawnAttacker.CheckResult();
                }
                isEnd = true;
            }
            else if (this.pawnAttacker.State == PawnState.Walk || this.pawnAttacker.State == PawnState.Draw || this.pawnAttacker.State == PawnState.Wait)
            {
                this.buildDefender.pawnOpponents.Remove(this.pawnAttacker);
                this.pawnAttacker.pawnAgent.MoraleChange(0);
                this.pawnAttacker.CheckIfBattleResult();//检查周围是否还有其他的敌人正在攻击自己
                isEnd = true;
                //this.attacker.gameObject.SetActive(false); 
            }
            else if (this.buildDefender.currentPawn != null)
            {
                this.buildDefender.pawnOpponents.Remove(this.pawnAttacker);
                this.pawnAttacker.UpdatePawnState();
                isEnd = true;
            }
            return isEnd;
        }

        void CalculatePosition()
        {
            Vector3 dir3 = this.buildDefender.transform.position - this.pawnAttacker.transform.position;
            Vector3 cross = Vector3.Cross(Vector3.up, dir3);
            float angle = cross.z > 0 ? Vector2.Angle(Vector3.up, dir3) : -Vector2.Angle(Vector3.up, dir3);
            this.arrowSprite.transform.localEulerAngles = new Vector3(0, 0, angle);
            this.transform.position = (this.pawnAttacker.transform.position + this.buildDefender.transform.position) / 2;
            //float dot = Vector2.Dot(dir3,)
        }
    }
}