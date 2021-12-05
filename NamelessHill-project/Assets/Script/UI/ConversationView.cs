using Nameless.Data;
using Nameless.DataUI;
using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nameless.UI
{
    public class ConversationView : MonoBehaviour
    {
        // Start is called before the first frame update
        public Text descTxt;

        public Image templatePawnIm;
        public GameObject pawnContent;
        private List<GameObject> pawnsImage = new List<GameObject>();

        public ConversationOptionUI templateOption;
        public GameObject optionContent;
        private List<GameObject> options = new List<GameObject>();
        
        

        public void ResetConversation(Conversation conversation)
        {
            for (int i = 0; i < this.pawnsImage.Count; i++)
                DestroyImmediate(this.pawnsImage[i].gameObject);
            this.pawnsImage.Clear();
            for (int i = 0; i < this.options.Count; i++)
                DestroyImmediate(this.options[i].gameObject);
            this.options.Clear();

            for(int i = 0; i < conversation.conversationPawns.Length; i++)
            {
                GameObject imageObj = Instantiate(this.templatePawnIm.gameObject, this.pawnContent.transform) as GameObject;
                imageObj.gameObject.SetActive(true);
                imageObj.GetComponent<Image>().sprite = CampManager.Instance.FindPawnInCamp(conversation.conversationPawns[i]).pawn.selectIcon;
                this.pawnsImage.Add(imageObj);
            }

            for(int i = 0; i < conversation.options.Count; i++)
            {
                GameObject optionObj = Instantiate(this.templateOption.gameObject, this.optionContent.transform) as GameObject;
                optionObj.gameObject.SetActive(true);
                optionObj.GetComponent<ConversationOptionUI>().InitOption(conversation.options[i]);
                this.options.Add(optionObj);
            }

            this.descTxt.text = conversation.descrption;
        }
    }
}