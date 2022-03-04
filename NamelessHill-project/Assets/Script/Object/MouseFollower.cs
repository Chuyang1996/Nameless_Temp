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


        public Image stateIcon;


        public void LabelChange(float content, TipState tipState)
        {

            this.stateIcon.gameObject.SetActive(true);
            if (tipState == TipState.Battle)
            {
                this.stateIcon.sprite = this.battleIcon;

            }
            else if(tipState == TipState.Walk)
            {
                this.stateIcon.sprite = this.walkIcon;

            }
            else if(tipState == TipState.UnWalk)
            {
                this.stateIcon.sprite = unWalkIcon;

            }
        }


        public void ResetState()
        {
            this.stateIcon.gameObject.SetActive(false);
        }
    }
}