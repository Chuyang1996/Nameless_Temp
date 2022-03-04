using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Nameless.Manager;

namespace Nameless.UI
{
    public class ResourceShow : MonoBehaviour
    {
        public Animation animation;
        public Text minusTxt;
        public Text plusTxt;
        public Text resTxt;

        private int resChange;
        private int resTotal;
        public void InitRes(int res)
        {
            this.resTxt.text = res.ToString();
        }
        public void ShowResChange(int totalValue,int changeValue)
        {
            AudioManager.Instance.PlayAudio(this.transform,"SFX_SupplyAdd_01");
            this.minusTxt.gameObject.SetActive(false);
            this.plusTxt.gameObject.SetActive(false);
            if (changeValue >= 0)
            {
                this.plusTxt.gameObject.SetActive(true);
                this.plusTxt.text = "+" + changeValue.ToString();
            }
            else
            {
                this.minusTxt.gameObject.SetActive(true);
                this.minusTxt.text = changeValue.ToString();
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