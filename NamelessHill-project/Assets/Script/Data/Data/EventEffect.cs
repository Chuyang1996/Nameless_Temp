using Nameless.ConfigData;
using Nameless.DataMono;
using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nameless.Data
{

    public enum EventEffectType
    {
        MoraleChange = 100,
        AllMoraleChange = 101,
        MilitaryResourceChange = 102,
        NextEvent = 104,
        UnlockNote = 105,
        UnlockConversation = 106
    }

    abstract public class EventEffect 
    {
        public EventEffectType type;

       abstract public void Execute();
    }

    public class MoraleEventEffect : EventEffect
    {
        public long pawnId;
        public int levelChange;
        public MoraleEventEffect(long Id, int levelChange)
        {
            this.type = EventEffectType.MoraleChange;
            this.pawnId = Id;
            this.levelChange = levelChange;
        }

        public override void Execute()
        {
            List<PawnAvatar> pawnAvatars = PawnManager.Instance.GetPawnAvatars(false);
            for (int i = 0; i < pawnAvatars.Count; i++)
            {
                if(pawnAvatars[i].Id == this.pawnId)
                {
                    float curMorale = pawnAvatars[i].pawnAgent.pawn.curMorale;
                    float maxMorale = pawnAvatars[i].pawnAgent.pawn.maxMorale;
                    int curLevel;
                    if(curMorale/maxMorale >= maxMorale / 2)
                        curLevel = 3;
                    else if(maxMorale / 4 <= curMorale/maxMorale && curMorale / maxMorale < maxMorale / 2)
                        curLevel = 2;
                    else
                        curLevel = 1;
                    
                    curLevel += this.levelChange;

                    if(curLevel >= 3)
                        pawnAvatars[i].pawnAgent.InitMorale((maxMorale + maxMorale / 2) / 2);
                    else if (curLevel == 2)
                        pawnAvatars[i].pawnAgent.InitMorale((maxMorale/4 + maxMorale / 2) / 2);
                    else if (curLevel <= 1)
                        pawnAvatars[i].pawnAgent.InitMorale((maxMorale / 4 ) / 2);
                    
                }
            }

        }
    }
    
    public class AllMoraleEventEffect : EventEffect
    {
        public int levelChange;

        public AllMoraleEventEffect(int levelChange)
        {
            this.type = EventEffectType.AllMoraleChange;
            this.levelChange = levelChange;
        }

        public override void Execute()
        {
            List<PawnAvatar> pawnAvatars = PawnManager.Instance.GetPawnAvatars(false);
            for (int i = 0; i < pawnAvatars.Count; i++)
            {
                float curMorale = pawnAvatars[i].pawnAgent.pawn.curMorale;
                float maxMorale = pawnAvatars[i].pawnAgent.pawn.maxMorale;
                int curLevel;
                if (curMorale / maxMorale >= maxMorale / 2)
                    curLevel = 3;
                else if (maxMorale / 4 <= curMorale / maxMorale && curMorale / maxMorale < maxMorale / 2)
                    curLevel = 2;
                else
                    curLevel = 1;

                curLevel += this.levelChange;

                if (curLevel >= 3)
                    pawnAvatars[i].pawnAgent.InitMorale((maxMorale + maxMorale / 2) / 2);
                else if (curLevel == 2)
                    pawnAvatars[i].pawnAgent.InitMorale((maxMorale / 4 + maxMorale / 2) / 2);
                else if (curLevel <= 1)
                    pawnAvatars[i].pawnAgent.InitMorale((maxMorale / 4) / 2);


            }
        }
    }

    public class MilitaryResEventEffect : EventEffect
    {
        public float ammoChange;

        public MilitaryResEventEffect(float ammoChange)
        {
            this.type = EventEffectType.MilitaryResourceChange;
            this.ammoChange = ammoChange;
        }

        public override void Execute()
        {
            GameManager.Instance.ChangeMilitaryRes((int)this.ammoChange);
        }
    }

    public class NextEventEffect : EventEffect
    {
        public long eventId;
        public NextEventEffect(long eventId)
        {
            this.type = EventEffectType.NextEvent;
            this.eventId = eventId;
        }
        public override void Execute()
        {
            EventTriggerManager.Instance.AddNewEvent(this.eventId);
        }
    }

    public class UnlockNoteEffect : EventEffect
    {
        public long notePageId;
        public List<NoteInfo> noteIds = new List<NoteInfo>();

        public UnlockNoteEffect(long notePageId, List<NoteInfo> noteIds)
        {
            this.type = EventEffectType.UnlockNote;
            this.notePageId = notePageId;
            this.noteIds = noteIds;
        }
        public override void Execute()
        {
            NoteManager.Instance.AddNote(this.notePageId, this.noteIds);
        }
    }

    public class UnlockConversationEffect : EventEffect
    {
        public long pawnId;
        public Conversation conversation;
        public UnlockConversationEffect(long pawnId, Conversation conversation)
        {
            this.type = EventEffectType.UnlockConversation;
            this.pawnId = pawnId;
            this.conversation = conversation;
        }
        public override void Execute()
        {
            if (PawnManager.Instance.GetPawnAvatarById(this.pawnId, false)!=null)
            {
                PawnManager.Instance.GetPawnAvatarById(this.pawnId, false).pawnAgent.PushConversation(this.conversation);
            }
        }
    }
}