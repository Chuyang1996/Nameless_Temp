using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.ConfigData
{
    public class ConversationEffectData 
    {
        public long id;
        public string name;
        public string descrption;
        public int type;
        public string parameter;
        // Start is called before the first frame update
        public ConversationEffectData(long id, string name, string descrption,int type,string parameter)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.type = type;
            this.parameter = parameter;
        }
    }
}