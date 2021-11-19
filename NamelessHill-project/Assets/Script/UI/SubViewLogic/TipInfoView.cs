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
        public GameObject opponentTip;


        public Sprite[] stateSprite;
        public Image stateMorale;
        public Slider ownAmmoSlider;


        public Slider oppoMoraleSlider;

        public GameObject content;
        public GameObject supportTemplate;
        public List<GameObject> RreshPanels = new List<GameObject>();
        private List<GameObject> supportsItem = new List<GameObject>();
        private PawnAvatar currentPawn;
        private bool isShowSupport = false;
        // Start is called before the first frame update

        // Update is called once per frame
        void Update()
        {
            Ray targetray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit TargetHit;
            if (Physics.Raycast(targetray, out TargetHit))
            {
                if (TargetHit.transform.gameObject.GetComponent<PawnAvatar>() != null && !TargetHit.transform.gameObject.GetComponent<PawnAvatar>().isAI)//待修改.AI
                {
                    this.ownTip.SetActive(true);
                    this.opponentTip.SetActive(false);
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
                    this.currentPawn = TargetHit.transform.gameObject.GetComponent<PawnAvatar>();
                    this.RreshPanel();//待修改 不要让其反复调用
                    float curMorale = (float)currentPawn.pawnAgent.pawn.curMorale;
                    float maxMorale = (float)currentPawn.pawnAgent.pawn.maxMorale;
                    if (curMorale >= maxMorale / 2)
                    {
                        stateMorale.sprite = stateSprite[0];
                    }
                    else if(maxMorale / 4 <= curMorale && curMorale < maxMorale / 2)
                    {
                        stateMorale.sprite = stateSprite[1];
                    }
                    else
                    {
                        stateMorale.sprite = stateSprite[2];
                    }
                    this.ownAmmoSlider.value = (float)currentPawn.pawnAgent.pawn.curAmmo / (float)currentPawn.pawnAgent.pawn.maxAmmo;
                }
                else if (TargetHit.transform.gameObject.GetComponent<PawnAvatar>() != null && TargetHit.transform.gameObject.GetComponent<PawnAvatar>().isAI)//待修改.AI
                {
                    this.currentPawn = TargetHit.transform.gameObject.GetComponent<PawnAvatar>();
                    this.opponentTip.SetActive(true);
                    this.ownTip.SetActive(false);
                    this.FollowMouseMove(opponentTip);
                    this.oppoMoraleSlider.value = currentPawn.pawnAgent.pawn.curMorale / currentPawn.pawnAgent.pawn.maxMorale;

                }
                else
                {
                    this.ownTip.SetActive(false);
                    this.opponentTip.SetActive(false);
                }
            }
            else
            {
                this.ownTip.SetActive(false);
                this.opponentTip.SetActive(false);
            }
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
            foreach (GameObject child in this.RreshPanels)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(child.GetComponent<RectTransform>());
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(this.GetComponent<RectTransform>());
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
                    List<Skill> tempSkills = supporters[i].pawnAgent.skills;
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
            foreach (GameObject child in this.RreshPanels)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(child.GetComponent<RectTransform>());
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(this.GetComponent<RectTransform>());
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