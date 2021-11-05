using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nameless.DataUI
{
    public class BuildInfoUI : MonoBehaviour
    {
        public Image buildIcon;
        public Text num;
        // Start is called before the first frame update
        public void Init(Sprite sprite, int num)
        {
            this.buildIcon.sprite = sprite;
            this.num.text = "X" + num.ToString();
        }
    }
}