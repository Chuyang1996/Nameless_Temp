using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nameless.DataUI
{

    public class SupportItemUI : MonoBehaviour
    {
        public Text textDesc;
        // Start is called before the first frame update
        public void Init(string text)
        {
            this.textDesc.text = text;
        }

        
    }
}