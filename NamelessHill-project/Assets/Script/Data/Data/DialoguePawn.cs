using Nameless.DataMono;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.Data {
    public class DialoguePawn
    {
        public PawnAvatar pawnAvatar;
        public Dialogue dialogue;

        public DialoguePawn(PawnAvatar pawnAvatar, Dialogue dialogue)
        {
            this.pawnAvatar = pawnAvatar;
            this.dialogue = dialogue;
        }
    }
}