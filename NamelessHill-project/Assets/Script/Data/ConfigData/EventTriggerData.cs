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
        public int type;
        public float parameter;

        public EventTriggerData(long id, string name, string descrption,int type, float parameter)
        {
            this.Id = id;
            this.name = name;
            this.descrption = descrption;
            this.type = type;
            this.parameter = parameter;
        }
    }
}