using Nameless.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.DataMono
{
    public class Support : MonoBehaviour
    {
        private PawnAvatar supporter;
        private PawnAvatar receiver;
        private List<Buff> receiverBuffs;
        public void InitSupport(PawnAvatar supporter, PawnAvatar receiver)
        {
            this.receiverBuffs = new List<Buff>();
            this.supporter = supporter;
            this.receiver = receiver;
            this.receiver.pawnAgent.supporters.Add(supporter);


            for(int i = 0; i < supporter.pawnAgent.skills.Count; i++)
            {
                if(supporter.pawnAgent.skills[i] is SupportSkill)
                {
                    PropertySkillEffect skillEffect = supporter.pawnAgent.skills[i].Execute(receiver, receiver);
                    List<Buff> buffs = skillEffect.buffs;
                    for(int j = 0; j < buffs.Count; j++)
                    {
                        if(buffs[j] is TimelyBuff)
                        {
                            TimelyBuff timelyBuff = (TimelyBuff)buffs[i];
                            this.receiver.pawnAgent.AddBuff(buffs[i]);
                            this.receiverBuffs.Add(buffs[i]);
                            StartCoroutine(timelyBuff.ActiveEffect(receiver));
                        }
                    }
                }
            }

            this.receiver.RefreshSupportIcon();
           // Debug.Log(this.receiver.pawnAgent.pawn.curAttack);

        }

        public void RemoveSupport()
        {
            this.receiver.pawnAgent.supporters.Remove(supporter);

            for(int i = 0; i < this.receiverBuffs.Count; i++)
            {
                if (this.receiver.pawnAgent.buffs.Contains(this.receiverBuffs[i]))
                {
                    this.receiver.pawnAgent.RemoveBuff(this.receiverBuffs[i]);
                }
            }

            
            this.receiver.RefreshSupportIcon();
        }
    }
}