using Nameless.ConfigData;
using Nameless.Data;
using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nameless.Agent
{
    public static class EventTriggerFactory
    {
        // Start is called before the first frame update
        public static EventTrigger GetEventTriggerById(long id)
        {
            return Get(DataManager.Instance.GetEventTriggerData(id));
        }

        public static EventTrigger Get(EventTriggerData buffData)
        {
            return new EventTrigger(buffData.Id, buffData.name, buffData.descrption, (EventTriggerType)buffData.type, buffData.parameter);
        }
    }
}