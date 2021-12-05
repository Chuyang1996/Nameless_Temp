using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.Data
{
    public class Conversation
    {
        public long id;
        public string name;
        public string descrption;
        public long[] conversationPawns;
        public List<ConversationOption> options = new List<ConversationOption>();
        public int sideindex;

        public Conversation(long id, string name, string descrption, long[] conversationPawns, List<ConversationOption> options,int side)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.conversationPawns = conversationPawns;
            this.options = options;
            this.sideindex = side;
        }
    }
}