using Nameless.DataMono;
using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.Data
{
    public enum BuffConditionType
    {
        LowHealth = 0,
        HighHealth = 1,
        LowAttack = 2,
        HightAttack = 3,
        LowDefend = 4,
        HighDefend = 5,
        LowMorale = 6,
        HighMorale = 7,
        LowAmmo = 8,
        HighAmmo = 9,
        IsAttacker = 10,
        IsDefender = 11,
        //LowMedicine = 10,
        //HighMedicine = 11,
    }
    public enum BuffAffectProperty
    {
        Health = 0,
        Attack = 1,
        Defend = 2,
    }
    public class Buff
    {
        public long id;
        public string name;
        public string descrption;
        public Dictionary<BuffConditionType, float> conditions = new Dictionary<BuffConditionType, float>();
        protected bool IsActive(PawnAvatar pawnAvatar)
        {
            foreach (var child in this.conditions)
            {
                if (child.Key == BuffConditionType.LowHealth)
                {
                    if (pawnAvatar.pawnAgent.pawn.curHealth <= child.Value * pawnAvatar.pawnAgent.pawn.maxHealth)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (child.Key == BuffConditionType.HighHealth)
                {
                    if (pawnAvatar.pawnAgent.pawn.curHealth > child.Value * pawnAvatar.pawnAgent.pawn.maxHealth)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (child.Key == BuffConditionType.LowAttack)
                {
                    if (pawnAvatar.pawnAgent.pawn.curAttack <= child.Value * pawnAvatar.pawnAgent.pawn.maxAttack)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (child.Key == BuffConditionType.HightAttack)
                {
                    if (pawnAvatar.pawnAgent.pawn.curAttack > child.Value * pawnAvatar.pawnAgent.pawn.maxAttack)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (child.Key == BuffConditionType.LowDefend)
                {
                    if (pawnAvatar.pawnAgent.pawn.curDefend <= child.Value * pawnAvatar.pawnAgent.pawn.maxDefend)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (child.Key == BuffConditionType.HighDefend)
                {
                    if (pawnAvatar.pawnAgent.pawn.curDefend > child.Value * pawnAvatar.pawnAgent.pawn.maxDefend)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (child.Key == BuffConditionType.LowMorale)
                {
                    if (pawnAvatar.pawnAgent.pawn.curMorale <= child.Value * pawnAvatar.pawnAgent.pawn.maxMorale)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (child.Key == BuffConditionType.HighMorale)
                {
                    if (pawnAvatar.pawnAgent.pawn.curMorale > child.Value * pawnAvatar.pawnAgent.pawn.maxMorale)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (child.Key == BuffConditionType.LowAmmo)
                {
                    if (pawnAvatar.pawnAgent.pawn.curAmmo <= child.Value * pawnAvatar.pawnAgent.pawn.maxAmmo)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (child.Key == BuffConditionType.HighAmmo)
                {
                    if (pawnAvatar.pawnAgent.pawn.curAmmo > child.Value * pawnAvatar.pawnAgent.pawn.maxAmmo)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (child.Key == BuffConditionType.IsAttacker)
                {
                    if (pawnAvatar.pawnAgent.battleSide == BattleSide.Attacker)
                    {
                        continue;
                    }
                    else
                        return false;
                }
                else if (child.Key == BuffConditionType.IsDefender)
                {
                    if (pawnAvatar.pawnAgent.battleSide == BattleSide.Defender)
                    {
                        continue;
                    }
                    else
                        return false;
                }

            }
            return true;
        }
    }

    public class TimelyBuff : Buff
    {
        public BuffAffectProperty property;
        public float second;
        public float valueChange;
        
        public TimelyBuff(long id, string name, string descrption, Dictionary<BuffConditionType, float> conditions,  int[] speedEffect)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.conditions = conditions;
            this.property = (BuffAffectProperty)speedEffect[0];
            this.second = speedEffect[1];
            this.valueChange = speedEffect[2];

        }

        public IEnumerator ActiveEffect(PawnAvatar pawnAvatar)
        {
            while (true)
            {
                if (!GameManager.Instance.isPlay)
                {
                    yield return null;
                }
                else
                {
                    if (pawnAvatar.pawnAgent.buffs.Contains(this))
                    {
                        if (this.IsActive(pawnAvatar))
                        {
                            if (this.property == BuffAffectProperty.Health)
                                pawnAvatar.pawnAgent.HealthChange(valueChange);
                        }
                    }
                    else
                        break;
                    yield return new WaitForSecondsRealtime(this.second);
                }
            }
        }
    }
}