using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nameless.ConfigData
{
    public class ConversationData
    {
        public long id;
        public string name;
        public string descrption;
        public string conversationPawns;
        public string options;
        public int side;
        public ConversationData(long id, string name, string descrption,string conversationPawns, string options,int side)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.conversationPawns = conversationPawns;
            this.options = options;
            this.side = side;
        }
    }
}