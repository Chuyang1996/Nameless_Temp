using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.Data
{
    public enum EventTriggerType
    {
        TimePass = 0,
        AmmoLess = 1,
        MedicineLess = 2,
        EnemyKillNum = 3,
    }
    public class EventTrigger
    {
        public long id;
        public string name;
        public string descrption;
        public EventTriggerType type;
        public float parameter;

        public EventTrigger(long id, string name, string descrption, EventTriggerType type, float parameter)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.type = type;
            this.parameter = parameter;
        }
    }
}