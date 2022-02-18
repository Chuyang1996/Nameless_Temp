using Nameless.ConfigData;
using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.Data
{
    public enum ConverEffectType
    {
        MoraleChange = 100,
        ResourceChange = 101,
        UnlockNore = 102,
        NextConversation = 103,
    }
    abstract public class ConversationEffect
    {
        public long id;
        public string name;
        public string descrption;
        public ConverEffectType type;

        abstract public void Execute();
    }

    public class ConversationMoraleChange : ConversationEffect
    {
        public long pawnId;
        public int moraleLevel;

        public ConversationMoraleChange(long id, string name, string descrption, long pawnId, int moraleChange)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.type = ConverEffectType.MoraleChange;
            this.pawnId = pawnId;
            this.moraleLevel = moraleChange;
        }

        public override void Execute()
        {
            if (CampManager.Instance.campScene.FindPawnInCamp(pawnId)!=null)
            {
                float curMorale = CampManager.Instance.campScene.FindPawnInCamp(pawnId).pawn.curMorale;
                float maxMorale = CampManager.Instance.campScene.FindPawnInCamp(pawnId).pawn.maxMorale;
                int curLevel;
                if (curMorale / maxMorale >= maxMorale / 2)
                    curLevel = 3;
                else if (maxMorale / 4 <= curMorale / maxMorale && curMorale / maxMorale < maxMorale / 2)
                    curLevel = 2;
                else
                    curLevel = 1;

                curLevel += this.moraleLevel;

                if (curLevel >= 3)
                {
                    CampManager.Instance.campScene.FindPawnInCamp(pawnId).InitMorale((maxMorale + maxMorale / 2) / 2);
                    CampManager.Instance.campScene.FindPawnInCamp(pawnId).tipIcon.sprite = CampManager.Instance.campScene.moraleUp;
                }
                else if (curLevel == 2)
                {
                    CampManager.Instance.campScene.FindPawnInCamp(pawnId).InitMorale((maxMorale / 4 + maxMorale / 2) / 2);
                    CampManager.Instance.campScene.FindPawnInCamp(pawnId).tipIcon.sprite = CampManager.Instance.campScene.moraleMiddle;
                }
                else if (curLevel <= 1)
                {
                    CampManager.Instance.campScene.FindPawnInCamp(pawnId).InitMorale((maxMorale / 4) / 2);
                    CampManager.Instance.campScene.FindPawnInCamp(pawnId).tipIcon.sprite = CampManager.Instance.campScene.moraleDown;
                }
                CampManager.Instance.campScene.FindPawnInCamp(pawnId).tipEffectResultAnim.Play();
            }
        }
    }

    public class ConversationResourceChange : ConversationEffect
    {
        public int resourceChange;
        public ConversationResourceChange(long id, string name, string descrption,int resourceChange)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.type = ConverEffectType.ResourceChange;
            this.resourceChange = resourceChange;
        }
        public override void Execute()
        {
            CampManager.Instance.ChangeMilitaryRes(resourceChange);//因为对话现在只在营地进行 所以用CampManager里的接口

        }
    }

    public class ConversationUnlockNote: ConversationEffect
    {
        public long noteId;
        public List<NoteInfo> noteInfos = new List<NoteInfo>();
        public ConversationUnlockNote(long id, string name, string descrption, long noteId, List<NoteInfo> noteInfos)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.type = ConverEffectType.UnlockNore;
            this.noteId = noteId;
            this.noteInfos = noteInfos;
        }
        public override void Execute()
        {
            NoteManager.Instance.AddNote(this.noteId, this.noteInfos);
        }
    }

    public class ConversationNext : ConversationEffect
    {
        public Conversation conversation;
        public ConversationNext(long id, string name, string descrption, Conversation conversation)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.type = ConverEffectType.NextConversation;
            this.conversation = conversation;
        }
        public bool IsActive()
        {
            return ConversationManager.Instance.CanGoConversation(conversation);
        }
        public override void Execute()
        {
            ConversationManager.Instance.GoToConversation(conversation);
        }

    }
}