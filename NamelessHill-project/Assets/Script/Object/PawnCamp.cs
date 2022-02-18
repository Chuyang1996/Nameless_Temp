using Nameless.Data;
using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.DataMono
{
    public class PawnCamp : MonoBehaviour
    {
        public Sprite pawnSprite;
        public Sprite pawnSpriteMark;

        public GameObject btnDialogue;
        public GameObject leftSide;
        public GameObject rightSide;
        public SpriteRenderer pawnIcon;
        public SpriteRenderer tipIcon;
        public Animation tipEffectResultAnim;


        public Pawn pawn;
        public void Init(Pawn pawn)
        {
            if (pawn.leftOrRight < 0)
            {
                this.btnDialogue.transform.parent = this.leftSide.transform;
                this.btnDialogue.transform.localScale = new Vector3(-this.btnDialogue.transform.localScale.x, this.btnDialogue.transform.localScale.y, this.btnDialogue.transform.localScale.z);
            }
            else
            {
                this.btnDialogue.transform.parent = this.rightSide.transform;
            }
            this.btnDialogue.transform.localPosition = new Vector3(0, 0, 0);
            this.btnDialogue.SetActive(true);
            this.pawnIcon.sprite = pawn.campIcon;

            this.pawnSprite = pawn.campIcon;
            this.pawnSpriteMark = pawn.campMarkIcon;

            this.pawn = pawn;

        }
        public void RefreshPawnCamp()
        {
            if (/*(!this.pawn.conversationMapDic.ContainsKey(CampManager.Instance.currentCampData.id) || !ConversationManager.Instance.CanGoConversation(this.pawn.conversationMapDic[CampManager.Instance.currentCampData.id])) &&*/ 
                (this.pawn.conversationsInCamp.Count == 0 || !ConversationManager.Instance.CanGoConversation(this.pawn.conversationsInCamp.Peek())))//待修改 根据地图ID和其他相关的条件去判断是否有对话//待修改 根据地图ID去索引
                this.btnDialogue.SetActive(false);
            else
                this.btnDialogue.SetActive(true);
        }

        public void InitMorale(float value)
        {
            this.pawn.curMorale = value;
        }

        public void ClickToConversation()
        {
            if(this.pawn.conversationsInCamp.Count > 0) 
            { 
                ConversationManager.Instance.GoToConversation(this.pawn.conversationsInCamp.Pop());
            }
            else if (this.pawn.conversationMapDic.ContainsKey(CampManager.Instance.currentCampData.id))//待修改 根据地图ID去索引
            {
                ConversationManager.Instance.GoToConversation(this.pawn.conversationMapDic[CampManager.Instance.currentCampData.id]);
            }

            if(this.pawn.conversationsInCamp.Count == 0 /*&& !this.pawn.conversationMapDic.ContainsKey(CampManager.Instance.currentCampData.id)*/)
                this.btnDialogue.SetActive(false);
        }


    }
}