using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace Nameless.UI
{
    public class CampResourceShow : MonoBehaviour
    {
        public Animation animation;
        public Text changeTxt;
        public Text resTxt;

        private int resChange;
        private int resTotal;
        public void InitRes(int res)
        {
            this.resTxt.text = res.ToString();
        }
        public void ShowResChange(int totalValue,int changeValue)
        {
            if (changeValue >= 0)
            {
                this.changeTxt.color = Color.green;
                this.changeTxt.text = "+" + changeValue.ToString();
            }
            else
            {
                this.changeTxt.color = Color.red;
                this.changeTxt.text = changeValue.ToString();
            }
            this.resChange = changeValue;
            this.resTotal = totalValue;

            this.animation.Play();
            
        }


        public void AddResourceEvent()
        {
            this.resTxt.text = this.resTotal.ToString();
        }
    }
}