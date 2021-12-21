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
            this.pawn = pawn;

        }
        public void RefreshPawnCamp()
        {
            if ((!this.pawn.conversationMapDic.ContainsKey(0) || !ConversationManager.Instance.CanGoConversation(this.pawn.conversationMapDic[0])) 
                && 
                (this.pawn.conversationsInCamp.Count == 0 || !ConversationManager.Instance.CanGoConversation(this.pawn.conversationsInCamp.Peek())))//���޸� ���ݵ�ͼID��������ص�����ȥ�ж��Ƿ��жԻ�//���޸� ���ݵ�ͼIDȥ����
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
            else if (this.pawn.conversationMapDic.ContainsKey(0))//���޸� ���ݵ�ͼIDȥ����
            {
                ConversationManager.Instance.GoToConversation(this.pawn.conversationMapDic[0]);
            }

            if(this.pawn.conversationsInCamp.Count == 0 && !this.pawn.conversationMapDic.ContainsKey(0))
                this.btnDialogue.SetActive(false);
        }


    }
}