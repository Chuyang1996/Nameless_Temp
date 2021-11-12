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
        // Start is called before the first frame update
        void Start()
        {
            this.button.onClick.AddListener(() => this.OptionResult());
        }

        public void InitOption()
        {

        }

        public void OptionResult()
        {
            GameManager.Instance.eventView.NewEvent();
        }
    }
}