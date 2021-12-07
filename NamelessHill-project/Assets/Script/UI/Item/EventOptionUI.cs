using Nameless.Data;
using Nameless.Manager;
using Nameless.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nameless.DataUI
{
    public class EventOptionUI : MonoBehaviour
    {
        public Button button;
        public Text descTxt;
        private EventOption eventResult;
        // Start is called before the first frame update
        void Start()
        {
            this.button.onClick.AddListener(() => { AudioManager.Instance.PlayAudio(this.transform, AudioConfig.uiRemind); this.OptionResult(); });
        }

        public void InitOption(EventOption eventResult)
        {
            this.eventResult = eventResult;
            this.descTxt.text = eventResult.descrption;
        }

        public void OptionResult()
        {
            this.eventResult.ExecuteEffect();
            GameManager.Instance.eventView.NewEvent();//´ýÐÞ¸Ä
        }
    }
}