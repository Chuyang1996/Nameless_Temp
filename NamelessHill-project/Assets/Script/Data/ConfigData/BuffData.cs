using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.ConfigData
{
    public class BuffData
    {
        // Start is called before the first frame update
        public long Id;
        public string name;
        public string description;
        public string conditions;
        public int type;
        public string parameter;

        public BuffData(
            long Id,
            string name,
            string description,
            string conditions,
            int type,
            string parameter
            )
        {
            this.Id = Id;
            this.name = name;
            this.description = description;
            this.conditions = conditions;
            this.type = type;
            this.parameter = parameter;
        }
    }
}
