using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.Data
{
    public class PawnGroup
    {
        public long id;
        public List<Pawn> pawns = new List<Pawn>();
        public float waitGenerateTime;
        public float durationTime;

        public PawnGroup(long id, List<Pawn> pawns, float waitGenerateTime, float durationTime)
        {
            this.id = id;
            this.pawns = pawns;
            this.waitGenerateTime = waitGenerateTime;
            this.durationTime = durationTime;
        }
    }
}