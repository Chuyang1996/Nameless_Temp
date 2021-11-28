using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.ConfigData
{
    public class PawnGroupData
    {
        public long Id;
        public string group;
        public float waitGenerateTime;
        public float durationTime;

        public PawnGroupData(long id, string group,float waitGenerateTime,float durationTime)
        {
            this.Id = id;
            this.group = group;
            this.waitGenerateTime = waitGenerateTime;
            this.durationTime = durationTime;
        }
    }
}