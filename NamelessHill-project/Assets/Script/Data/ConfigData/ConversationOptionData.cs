using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.ConfigData
{
    public class ConversationOptionData
    {
        public long id;
        public string name;
        public string descrption;
        public string effects;
        // Start is called before the first frame update
        public ConversationOptionData(long id, string name, string descrption,string effects)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.effects = effects;
        }
    }
}