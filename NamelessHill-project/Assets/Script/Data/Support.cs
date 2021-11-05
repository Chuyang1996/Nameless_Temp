using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.DataMono
{
    public class Support : MonoBehaviour
    {
        private PawnAvatar supporter;
        private PawnAvatar receiver;
        public void InitSupport(PawnAvatar supporter, PawnAvatar receiver)
        {
            this.supporter = supporter;
            this.receiver = receiver;
            this.receiver.pawnAgent.InitAttack(this.receiver.pawnAgent.pawn.curAttack + this.receiver.pawnAgent.pawn.maxAttack * 0.2f);
            Debug.Log(this.receiver.pawnAgent.pawn.curAttack);

        }

        public void RemoveSupport()
        {
            this.receiver.pawnAgent.InitAttack(this.receiver.pawnAgent.pawn.curAttack - this.receiver.pawnAgent.pawn.maxAttack * 0.2f);
        }
    }
}