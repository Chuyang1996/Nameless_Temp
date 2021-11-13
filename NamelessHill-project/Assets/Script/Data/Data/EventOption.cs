using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.Data
{
    public class EventOption
    {
        public long id;
        public string name;
        public string descrption;
        public List<EventEffect> effects;


        public EventOption(long id, string name, string descrption, List<EventEffect> effects)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.effects = effects;
        }


        public void ExecuteEffect()
        {
            for(int i = 0; i < this.effects.Count; i++)
            {
                this.effects[i].Execute();
            }
        }

    }
}