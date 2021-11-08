using Nameless.DataMono;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.Data
{
    
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


        public virtual void Execute(PawnAvatar receiver)
        {

        }

    }

    public class TimelyBuff : Buff
    {
        public BuffAffectProperty property;
        public float second;
        public float valueChange;
        
        public TimelyBuff(long id, string name, string descrption,  int[] speedEffect)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.property = (BuffAffectProperty)speedEffect[0];
            this.second = speedEffect[1];
            this.valueChange = speedEffect[2];

        }

        public IEnumerator ActiveEffect(PawnAvatar pawnAvatar)
        {
            while (true)
            {
                if (pawnAvatar.pawnAgent.buffs.Contains(this))
                {
                    if(this.property == BuffAffectProperty.Health)
                        pawnAvatar.pawnAgent.HealthChange(valueChange);
                }
                else
                    break;
                yield return new WaitForSecondsRealtime(this.second);
            }
        }
    }
}