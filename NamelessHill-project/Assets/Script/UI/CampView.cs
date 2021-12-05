using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nameless.UI
{
    public class CampView :  SelectViewLogic
    {
        public Animation animation;

        public AnimationClip enterAnim;
        public AnimationClip exitAnim;

        public GameObject selectPanel;

        public Text resourceText;
        public Text pawnNum;
        public Text infoTxt;

        public void InitCamp(int militartRes, int pawnNum)
        {
            this.gameObject.SetActive(true);
            this.InitMilitRes(militartRes);
            GameManager.Instance.TotalMilitartEvent += this.InitMilitRes;
            this.InitPawnInfo(pawnNum);
            this.animation.clip = enterAnim;
            this.animation.Play();
        }

        public void ExitCamp()
        {
            GameManager.Instance.TotalMilitartEvent -= this.InitMilitRes;
        }

        public void EnterCampEvent()
        {
            CampManager.Instance.ActiveCamp();
        }

        public void ExitCampEvent()
        {

        }
        public void InitPawnInfo(int value)
        {
            this.pawnNum.text = value.ToString() + " " + "Members Left";
        }
        public void InitMilitRes(int value)
        {
            this.resourceText.text = value.ToString();
        }

        public void InitInfo(string value)
        {
            this.FollowMouseMove(this.selectPanel);
            this.selectPanel.SetActive(true);
            this.infoTxt.text = value;
        }
        public override void FollowMouseMove(GameObject item)
        {
            base.FollowMouseMove(item);
            item.gameObject.SetActive(true);
            RectTransform rectTransform = item.transform as RectTransform;
            item.GetComponent<RectTransform>().anchoredPosition = new Vector2(pos.x + (item.GetComponent<RectTransform>().sizeDelta.x / 2), pos.y - (item.GetComponent<RectTransform>().sizeDelta.y / 2));

        }
        private void Update()
        {
            Vector2 raySelectBtn = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitBtn = Physics2D.Raycast(raySelectBtn, Vector2.zero);
            if (hitBtn.collider != null)
            {

                if (hitBtn.collider.gameObject == CampManager.Instance.noteBtn.gameObject)
                {
                    this.InitInfo("My Daily");
                }
                else
                {
                    this.selectPanel.SetActive(false);

                }
            }
            else
            {
                this.selectPanel.SetActive(false);
            }
            if (Input.GetMouseButtonDown(0) && !GameManager.Instance.noteBookView.gameObject.activeInHierarchy && !GameManager.Instance.conversationView.gameObject.activeInHierarchy)//����Щ��崦�ڹر�״̬ʱ���ܵ��
            {
                //Debug.Log("sssss");
                if (hitBtn.collider != null)
                {

                    if (hitBtn.collider.gameObject == CampManager.Instance.noteBtn.gameObject)
                    {
                        NoteManager.Instance.InitNoteBook();
                    }
                    else
                    {
                        for (int i = 0; i < CampManager.Instance.allCampPawns.Count; i++)
                        {
                            if (hitBtn.collider.gameObject == CampManager.Instance.allCampPawns[i].btnDialogue.gameObject)//���޸�.AI
                            {
                                CampManager.Instance.allCampPawns[i].ClickToConversation();
                                Debug.Log("������Ի�");
                                break;
                            }
                        }
                    }
                }
            }

        }
    } 
}