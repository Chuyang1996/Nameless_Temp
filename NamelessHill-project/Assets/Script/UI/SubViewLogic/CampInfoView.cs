using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nameless.UI
{
    public class CampInfoView : SelectViewLogic
    {
        public Sprite book;
        public Sprite bookMark;

        public Sprite light;
        public Sprite lightMark;

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
        // Update is called once per frame
        void Update()
        {
            if (Time.timeScale == 0.0f)
                return;

            Vector2 raySelectBtn = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitBtn = Physics2D.Raycast(raySelectBtn, Vector2.zero);
            if (hitBtn.collider != null)
            {
                if (hitBtn.collider.gameObject == CampManager.Instance.lightBtn.gameObject)
                {
                    CampManager.Instance.lightBtn.GetComponent<SpriteRenderer>().sprite = this.lightMark;
                    this.InitInfo("Wait till the next battle");
                }
                else if (hitBtn.collider.gameObject == CampManager.Instance.noteBtn.gameObject)
                {
                    CampManager.Instance.noteBtn.GetComponent<SpriteRenderer>().sprite = this.bookMark;
                    this.InitInfo("My Daily");
                }
                else
                {
                    CampManager.Instance.lightBtn.GetComponent<SpriteRenderer>().sprite = this.light;
                    CampManager.Instance.noteBtn.GetComponent<SpriteRenderer>().sprite = this.book;
                    this.selectPanel.SetActive(false);

                }
            }
            else
            {
                CampManager.Instance.lightBtn.GetComponent<SpriteRenderer>().sprite = this.light;
                CampManager.Instance.noteBtn.GetComponent<SpriteRenderer>().sprite = this.book;
                this.selectPanel.SetActive(false);
            }
            if (Input.GetMouseButtonDown(0) && !GameManager.Instance.noteBookView.gameObject.activeInHierarchy && !GameManager.Instance.conversationView.gameObject.activeInHierarchy)//当这些面板处于关闭状态时才能点击
            {
                //Debug.Log("sssss");
                if (hitBtn.collider != null)
                {

                    if (hitBtn.collider.gameObject == CampManager.Instance.noteBtn.gameObject)
                    {
                        AudioManager.Instance.PlayAudio(CampManager.Instance.noteBtn.transform, AudioConfig.uiRemind);
                        NoteManager.Instance.InitNoteBook();
                    }
                    else
                    {
                        for (int i = 0; i < CampManager.Instance.allCampPawns.Count; i++)
                        {
                            if (hitBtn.collider.gameObject == CampManager.Instance.allCampPawns[i].btnDialogue.gameObject)//待修改.AI
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

        private void LateUpdate()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(this.selectPanel.GetComponent<RectTransform>());
        }
    }
}