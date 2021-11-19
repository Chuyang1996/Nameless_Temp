using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nameless.Data
{
    public class EventResult
    {
        public long id;
        public string name;
        public string descrption;
        public long conditionId;
        public List<EventOption> options;
        public Sprite eventImage;
        public EventResult(long id, string name, string descrption, long conditionId, List<EventOption> options,Sprite eventImage)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.conditionId = conditionId;
            this.options = options;
            this.eventImage = eventImage;
        }

    }
}