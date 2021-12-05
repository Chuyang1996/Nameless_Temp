using Nameless.Data;
using Nameless.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nameless.DataUI
{
    public class ConversationOptionUI : MonoBehaviour
    {
        public Button btnOption;
        public Text optionTxt;
        private ConversationOption conversationOption;

        // Start is called before the first frame update
        void Start()
        {
            this.btnOption.onClick.AddListener(this.OptionClick);
        }
        
        public void InitOption(ConversationOption conversationOption)
        {
            this.conversationOption = conversationOption;
            this.optionTxt.text = conversationOption.name;

        }
        private void OptionClick()
        {
            this.conversationOption.Execute();
        }
    }
}