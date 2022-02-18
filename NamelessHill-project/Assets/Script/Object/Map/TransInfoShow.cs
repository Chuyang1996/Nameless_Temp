using Nameless.DataUI;
using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.DataMono
{
    public class TransInfoShow : MonoBehaviour
    {
        // Start is called before the first frame update
        public Animation animation;
        public TextReaderUI[] textReaderUIs;
        private int index;
        public void StartReader()
        {
            this.index = 0;
            StartCoroutine(this.ReadText());
        }

        IEnumerator ReadText()
        {
            string charS = "";
            int i = 0;
            float countTime = 0.0f;
            bool isSkip = false;
            bool hasSkip = true;
            while(i < this.textReaderUIs[index].contentTxt.Length)
            {
                while (countTime < 0.03f)
                {
                    countTime += Time.deltaTime;
                    if (Input.GetMouseButtonDown(0) && !hasSkip)
                    {
                        isSkip = true;
                        break;
                    }
                    hasSkip = false;
                    yield return null;
                }
                countTime = 0.0f;
                charS += this.textReaderUIs[index].contentTxt[i].ToString();
                this.textReaderUIs[index].descTxt.text = charS;
                i ++;
                if (isSkip)
                    break;
                yield return null;
            }
            this.textReaderUIs[index].descTxt.text = this.textReaderUIs[index].contentTxt;
            this.textReaderUIs[index].icon.SetActive(true);
            hasSkip = true;
            while (true)
            {
                if (Input.GetMouseButtonDown(0)&& !hasSkip)
                    break;
                hasSkip = false;
                yield return null;
            }

            this.index++;
            if (this.index < this.textReaderUIs.Length)
                StartCoroutine(this.ReadText());
            else
                this.animation.Play();
        }

        public void InitBattle()
        {
            GameManager.Instance.InitBattle();
        }

    }
}