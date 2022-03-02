using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nameless.DataUI
{

    public class SupportItemUI : MonoBehaviour
    {
        public Text textDesc;
        public Image skillIcon;
        // Start is called before the first frame update
        public void Init(string text, Sprite icon)
        {
            this.textDesc.text = text;
            this.skillIcon.sprite = icon;
        }

        
    }
}