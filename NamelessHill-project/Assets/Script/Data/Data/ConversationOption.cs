using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.Data
{
    public class ConversationOption 
    {
        public long id;
        public string name;
        public string descrption;
        public List<ConversationEffect> conversationEffects = new List<ConversationEffect>();

        public ConversationOption(long id, string name, string descrption, List<ConversationEffect> conversationEffects)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.conversationEffects = conversationEffects;
        }

        public void Execute()
        {
            List<ConversationNext> conversationNexts = new List<ConversationNext>();
            for (int i = 0; i < this.conversationEffects.Count; i++)
            {
                if (this.conversationEffects[i] is ConversationNext)
                {
                    conversationNexts.Add((ConversationNext)this.conversationEffects[i]);
                }
                else
                {
                    this.conversationEffects[i].Execute();//先将进入下一个对话之外的效果执行完毕，然后判断哪个对话是下一个对话
                }
            }

            bool isEndConversation = true;
            for (int i = 0; i < conversationNexts.Count; i++)
            {
                if (conversationNexts[i].IsActive()) {
                    conversationNexts[i].Execute();
                    isEndConversation = false;
                    break;
                } 
            }

            if (isEndConversation)
                ConversationManager.Instance.EndConversation();
        }
    }
}