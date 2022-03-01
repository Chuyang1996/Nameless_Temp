using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nameless.DataMono {
    public enum TipState
    {
        Walk = 0,
        Battle = 1,
        UnWalk = 2,
    }
    public class MouseFollower : MonoBehaviour
    {
        public Sprite walkIcon;
        public Sprite battleIcon;
        public Sprite unWalkIcon;

        public Text labelTxt;
        public Image stateIcon;


        public void LabelChange(float content, TipState tipState)
        {
            this.labelTxt.gameObject.SetActive(true);
            this.stateIcon.gameObject.SetActive(true);
            if (tipState == TipState.Battle)
            {
                this.stateIcon.sprite = this.battleIcon;
                this.labelTxt.text = "";
            }
            else if(tipState == TipState.Walk)
            {
                this.stateIcon.sprite = this.walkIcon;
                string minute = (int)(content / 60.0f) < 10 ? "0" + ((int)(content / 60.0f)).ToString() : ((int)(content / 60.0f)).ToString();
                string seconds = (int)(content % 60.0f) < 10 ? "0" + ((int)(content % 60.0f)).ToString() : ((int)(content % 60.0f)).ToString();
                this.labelTxt.text = minute.ToString() + ":" + seconds.ToString(); ;
            }
            else if(tipState == TipState.UnWalk)
            {
                this.stateIcon.sprite = unWalkIcon;
                this.labelTxt.text = "Unpass";
            }
        }


        public void ResetState()
        {
            this.labelTxt.gameObject.SetActive(false);
            this.stateIcon.gameObject.SetActive(false);
        }
    }
}