using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.ConfigData
{
    public class AreaData 
    {
        public long Id;
        public string name;
        public string description;
        public int type;
        public int rule;
        public string parameter;

        public AreaData(long id, string name, string description,int type, int rule, string parameter)
        {
            this.Id = id;
            this.name = name;
            this.description = description;
            this.type = type;
            this.rule = rule;
            this.parameter = parameter;
        }
    }
}