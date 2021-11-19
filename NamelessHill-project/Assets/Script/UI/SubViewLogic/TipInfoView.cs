using Nameless.DataMono;
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
        public 
        // Start is called before the first frame update

        // Update is called once per frame
        void Update()
        {
            Ray targetray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit TargetHit;
            if (Physics.Raycast(targetray, out TargetHit))
            {
                if (TargetHit.transform.gameObject.GetComponent<PawnAvatar>() != null && !TargetHit.transform.gameObject.GetComponent<PawnAvatar>().isAI)//´ýÐÞ¸Ä
                {
                    this.ownTip.SetActive(true);
                    this.opponentTip.SetActive(false);
                    this.FollowMouseMove(ownTip);
                    float curMorale = (float)TargetHit.transform.gameObject.GetComponent<PawnAvatar>().pawnAgent.pawn.curMorale;
                    float maxMorale = (float)TargetHit.transform.gameObject.GetComponent<PawnAvatar>().pawnAgent.pawn.maxMorale;
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
                    this.ownAmmoSlider.value = (float)TargetHit.transform.gameObject.GetComponent<PawnAvatar>().pawnAgent.pawn.curAmmo / (float)TargetHit.transform.gameObject.GetComponent<PawnAvatar>().pawnAgent.pawn.maxAmmo;
                }
                else if (TargetHit.transform.gameObject.GetComponent<PawnAvatar>() != null && TargetHit.transform.gameObject.GetComponent<PawnAvatar>().isAI)//´ýÐÞ¸Ä
                {
                    this.opponentTip.SetActive(true);
                    this.ownTip.SetActive(false);
                    this.FollowMouseMove(opponentTip);
                    this.oppoMoraleSlider.value = TargetHit.transform.gameObject.GetComponent<PawnAvatar>().pawnAgent.pawn.curMorale / TargetHit.transform.gameObject.GetComponent<PawnAvatar>().pawnAgent.pawn.maxMorale;

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
            item.GetComponent<RectTransform>().anchoredPosition = new Vector2(pos.x + (item.GetComponent<RectTransform>().sizeDelta.x / 2), pos.y - (item.GetComponent<RectTransform>().sizeDelta.y));

        }
    }
}