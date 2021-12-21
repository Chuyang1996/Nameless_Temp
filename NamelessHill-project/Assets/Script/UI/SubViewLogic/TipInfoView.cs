using Nameless.Data;
using Nameless.DataMono;
using Nameless.DataUI;
using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nameless.UI
{
    public class TipInfoView : SelectViewLogic
    {
        public GameObject ownTip;

        public Text nameTxt;
        public Sprite[] stateSprite;
        public Image stateMorale;
        public Slider ownAmmoSlider;
        


        public GameObject content;
        public GameObject supportTemplate;
        public List<GameObject> RreshPanels = new List<GameObject>();
        private List<GameObject> supportsItem = new List<GameObject>();
        private PawnAvatar currentPawn;
        //private bool isShowSupport = false;
        // Start is called before the first frame update

        // Update is called once per frame
        
        void Update()
        {
            Ray targetray1 = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit TargetHit1;
                //Debug.Log("sssss");
            if (Physics.Raycast(targetray1, out TargetHit1))
            {
                if (TargetHit1.transform.gameObject.GetComponent<PawnAvatar>() != null 
                    && TargetHit1.transform.gameObject.GetComponent<PawnAvatar>().pawnAgent.frontPlayer.IsLocalPlayer())//���޸�.AI
                {
                    this.ownTip.SetActive(true);
                    this.FollowMouseMove(ownTip);
                    //if (this.currentPawn != TargetHit.transform.gameObject.GetComponent<PawnAvatar>() || TargetHit.transform.gameObject.GetComponent<PawnAvatar>().pawnAgent.supporters.Count == 0)
                    //{
                    //    this.ClearPanel();
                    //    this.isShowSupport = false;
                    //}
                    //if(this.currentPawn != TargetHit.transform.gameObject.GetComponent<PawnAvatar>() || !this.isShowSupport)
                    //{
                    //    this.RreshPanel();
                    //    this.isShowSupport = true;
                    //}
                    this.currentPawn = TargetHit1.transform.gameObject.GetComponent<PawnAvatar>();
                    this.RreshPanel();
                    if (currentPawn.pawnAgent.MoralteState() == 1.5f)
                    {
                        stateMorale.sprite = stateSprite[0];
                    }
                    else if(currentPawn.pawnAgent.MoralteState() ==  1.0f)
                    {
                        stateMorale.sprite = stateSprite[1];
                    }
                    else
                    {
                        stateMorale.sprite = stateSprite[2];
                    }
                    this.nameTxt.text = currentPawn.pawnAgent.pawn.name;
                    this.ownAmmoSlider.value = (float)currentPawn.pawnAgent.pawn.curAmmo / (float)currentPawn.pawnAgent.pawn.maxAmmo;
                }
                else
                {
                    this.ownTip.SetActive(false);

                }
            }
            else
            {
                this.ownTip.SetActive(false);
            }
        }

        public void InitTipInfo()
        {
            this.ownAmmoSlider.gameObject.transform.Find("Background").gameObject.GetComponent<Image>().color = GameManager.Instance.accessbility ? new Color(0.33f, 0.33f, 0.33f, 1) : new Color(0, 1, 1, 1);
        }
        public override void FollowMouseMove(GameObject item)
        {
            base.FollowMouseMove(item);
            item.gameObject.SetActive(true);
            RectTransform rectTransform = item.transform as RectTransform;
            item.GetComponent<RectTransform>().anchoredPosition = new Vector2(pos.x + (item.GetComponent<RectTransform>().sizeDelta.x / 2), pos.y - (item.GetComponent<RectTransform>().sizeDelta.y/2));

        }
        private void RreshPanel()
        {
            StartCoroutine(RreshEnumerator());
        }
        private void ClearPanel()
        {
            StartCoroutine(ClearEnumerator());
        }

        private IEnumerator ClearEnumerator()
        {
            yield return new WaitForSecondsRealtime(1.0f);
            this.RemoveAllTips();
            //foreach (GameObject child in this.RreshPanels)
            //{
            //    LayoutRebuilder.ForceRebuildLayoutImmediate(child.GetComponent<RectTransform>());
            //}
            //LayoutRebuilder.ForceRebuildLayoutImmediate(this.GetComponent<RectTransform>());
        }
        private IEnumerator RreshEnumerator()
        {
            yield return new WaitForSecondsRealtime(1.0f);
            this.RreshAllTips();
            
        }
        private void RemoveAllTips()
        {
            for (int i = 0; i < supportsItem.Count; i++)
                DestroyImmediate(this.supportsItem[i]);
            this.supportsItem.Clear();
        }

        private void RreshAllTips()
        {
            this.RemoveAllTips();

            if (this.currentPawn)
            {
                List<SupportSkill> supportSkills = new List<SupportSkill>();
                List<PawnAvatar> supporters = this.currentPawn.pawnAgent.supporters;
                for(int i = 0; i < supporters.Count; i++)
                {
                    List<Skill> tempSkills = supporters[i].pawnAgent.GetSkills();
                    for(int j = 0; j < tempSkills.Count; j++)
                    {
                        if (tempSkills[j] is SupportSkill)
                        {
                            supportSkills.Add((SupportSkill)tempSkills[j]);
                        }
                    }
                }

                List<Buff> buffs = new List<Buff>();
                for(int i = 0; i < supportSkills.Count; i++)
                {
                    if(supportSkills[i].attackRate != 0.0f || supportSkills[i].defendRate != 0.0f)//���޸�
                        this.GenerateSupportTip(supportSkills[i].descrption);
                    for(int j = 0; j < supportSkills[i].buffs.Count; j++)
                    {
                        buffs.Add(supportSkills[i].buffs[j]);
                    }
                }

                for(int i = 0; i < buffs.Count; i++)
                {
                    this.GenerateSupportTip(buffs[i].descrption);
                }
            }
            //foreach (GameObject child in this.RreshPanels)
            //{
            //    LayoutRebuilder.ForceRebuildLayoutImmediate(child.GetComponent<RectTransform>());
            //}
            //LayoutRebuilder.ForceRebuildLayoutImmediate(this.GetComponent<RectTransform>());
        }


        private void GenerateSupportTip(string text)
        {
            GameObject gameObjectUI = Instantiate(this.supportTemplate.gameObject, this.content.transform) as GameObject;
            gameObjectUI.GetComponent<SupportItemUI>().Init(text);
            gameObjectUI.SetActive(true);
            this.supportsItem.Add(gameObjectUI);
        }
    }
}