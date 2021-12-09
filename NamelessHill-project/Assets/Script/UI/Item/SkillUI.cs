using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nameless.DataUI
{
    public class SkillUI : MonoBehaviour
    {
        public Image icon;
        public Text descTxt;
        // Start is called before the first frame update
        public void InitSkill(Sprite sprite, string desc)
        {
            this.icon.sprite = sprite;
            this.descTxt.text = desc;
            LayoutRebuilder.ForceRebuildLayoutImmediate(this.GetComponent<RectTransform>());
        }
    }
}