using Nameless.Data;
using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.DataMono
{
    public class PawnCamp : MonoBehaviour
    {
        public Sprite dialogueIm;
        public Sprite dialogueImMark;

        public GameObject btnDialogue;
        public GameObject leftSide;
        public GameObject rightSide;
        public SpriteRenderer pawnIcon;

        Stack<Conversation> conversationsInCamp = new Stack<Conversation>();
        public Pawn pawn;
        public void Init(Pawn pawn, Stack<Conversation> conversationsInCamp)
        {
            if(pawn.leftOrRight < 0)
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
            this.pawn = pawn;
            this.conversationsInCamp = conversationsInCamp;
            if (!this.pawn.conversationMapDic.ContainsKey(0) && this.conversationsInCamp.Count == 0)//待修改 根据地图ID和其他相关的条件去判断是否有对话
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
            if(this.conversationsInCamp.Count > 0) 
            { 
                ConversationManager.Instance.GoToConversation(this.conversationsInCamp.Pop());
            }
            else if (this.pawn.conversationMapDic.ContainsKey(0))//待修改 根据地图ID去索引
            {
                ConversationManager.Instance.GoToConversation(this.pawn.conversationMapDic[0]);
            }

            if(this.conversationsInCamp.Count == 0 && !this.pawn.conversationMapDic.ContainsKey(0))
                this.btnDialogue.SetActive(false);
        }


    }
}