using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nameless.UI
{
    public class CampInfoView : SelectViewLogic
    {

        // Start is called before the first frame update
        public GameObject selectPanel;
        public Text infoTxt;
        private void InitInfo(string value)
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

        private void MouseOnItem()
        {
            Vector2 raySelectBtn = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitBtn = Physics2D.Raycast(raySelectBtn, Vector2.zero);
            if (hitBtn.collider != null)
            {
                CampManager.Instance.campScene.ResetAllBtnState();
                for (int i = 0; i < CampManager.Instance.allCampPawns.Count; i++)
                {
                    if (hitBtn.collider.gameObject == CampManager.Instance.allCampPawns[i].btnDialogue.gameObject)
                        CampManager.Instance.allCampPawns[i].btnDialogue.GetComponent<SpriteRenderer>().sprite = CampManager.Instance.allCampPawns[i].dialogueImMark;

                }
                if (hitBtn.collider.gameObject == CampManager.Instance.campScene.lightBtn.gameObject)
                {
                    CampManager.Instance.campScene.lightBtn.GetComponent<SpriteRenderer>().sprite = CampManager.Instance.campScene.lightMark;
                    this.InitInfo("Wait till the next battle");
                }
                else if (hitBtn.collider.gameObject == CampManager.Instance.campScene.noteBtn.gameObject)
                {
                    CampManager.Instance.campScene.noteBtn.GetComponent<SpriteRenderer>().sprite = CampManager.Instance.campScene.bookMark;
                    this.InitInfo("My Daily");
                }
                else
                {
                    this.selectPanel.SetActive(false);

                }
            }//�����������Ʒ��ʱ��Ч��
            else
            {
                this.selectPanel.SetActive(false);
            }
        }
        private void MouseClickOnItem()
        {
            Vector2 raySelectBtn = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitBtn = Physics2D.Raycast(raySelectBtn, Vector2.zero);
            if (Input.GetMouseButtonDown(0) 
                && !GameManager.Instance.noteBookView.gameObject.activeInHierarchy 
                && !GameManager.Instance.conversationView.gameObject.activeInHierarchy)//����Щ��崦�ڹر�״̬ʱ���ܵ��
            {
                //Debug.Log("sssss");
                if (hitBtn.collider != null)
                {

                    if (hitBtn.collider.gameObject == CampManager.Instance.campScene.noteBtn.gameObject)
                    {
                        AudioManager.Instance.PlayAudio(CampManager.Instance.campScene.noteBtn.transform, AudioConfig.uiRemind);
                        NoteManager.Instance.InitNoteBook();
                    }
                    else if (hitBtn.collider.gameObject == CampManager.Instance.campScene.lightBtn.gameObject)
                    {
                        GameManager.Instance.campView.OpenConfirm();
                    }
                    {
                        for (int i = 0; i < CampManager.Instance.allCampPawns.Count; i++)
                        {
                            if (hitBtn.collider.gameObject == CampManager.Instance.allCampPawns[i].btnDialogue.gameObject)
                            {
                                AudioManager.Instance.PlayAudio(CampManager.Instance.allCampPawns[i].btnDialogue.transform, AudioConfig.uiRemind);
                                CampManager.Instance.allCampPawns[i].ClickToConversation();
                                break;
                            }
                        }
                    }
                }
            }
        }
        // Update is called once per frame
        void Update()
        {
            if (Time.timeScale == 0.0f)
                return;
            this.MouseOnItem();
            this.MouseClickOnItem();

        }

        private void LateUpdate()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(this.selectPanel.GetComponent<RectTransform>());
        }
    }
}