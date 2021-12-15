using Nameless.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nameless.Manager {
    public class ConversationManager : Singleton<ConversationManager>
    {
        public void GoToConversation(Conversation conversation)
        {
            if (this.CanGoConversation(conversation))
            {
                GameManager.Instance.conversationView.gameObject.SetActive(true);
                GameManager.Instance.conversationView.ResetConversation(conversation);
            }
            else
                this.EndConversation();
        }

        public void EndConversation()
        {
            GameManager.Instance.conversationView.gameObject.SetActive(false);
        }

        public bool CanGoConversation(Conversation conversation)
        {
            if (conversation == null)
                return false;
            for(int i = 0; i < conversation.conversationPawns.Length; i++)
            {
                if (CampManager.Instance.campScene.FindPawnInCamp(conversation.conversationPawns[i]) == null)
                    return false;
            }
            return true;
        }

        
    }
}