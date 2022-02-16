using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nameless.ConfigData
{
    public class EventTriggerData
    {
        public long Id;
        public string name;
        public string descrption;
        public string condition;
        public int type;
        public string parameter;

        public EventTriggerData(long id, string name, string descrption,string condition, int type, string parameter)
        {
            this.Id = id;
            this.name = name;
            this.descrption = descrption;
            this.condition = condition;
            this.type = type;
            this.parameter = parameter;
        }
    }
}