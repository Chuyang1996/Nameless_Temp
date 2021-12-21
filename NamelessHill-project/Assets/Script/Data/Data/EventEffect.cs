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
        UnlockConversation = 106,
        
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
        private FrontPlayer frontPlayer;
        public MoraleEventEffect(long Id, int levelChange, FrontPlayer frontPlayer)
        {
            this.type = EventEffectType.MoraleChange;
            this.pawnId = Id;
            this.levelChange = levelChange;
            this.frontPlayer = frontPlayer;
        }

        public override void Execute()
        {
            List<PawnAvatar> pawnAvatars = FrontManager.Instance.GetPawnAvatars(this.frontPlayer);
            for (int i = 0; i < pawnAvatars.Count; i++)
            {
                if(pawnAvatars[i].pawnAgent.pawn.id == this.pawnId)
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
        private FrontPlayer frontPlayer;
        public AllMoraleEventEffect(int levelChange, FrontPlayer frontPlayer)
        {
            this.type = EventEffectType.AllMoraleChange;
            this.levelChange = levelChange;
            this.frontPlayer = frontPlayer;
        }

        public override void Execute()
        {
            List<PawnAvatar> pawnAvatars = FrontManager.Instance.GetPawnAvatars(this.frontPlayer);
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
        private FrontPlayer frontPlayer;
        public MilitaryResEventEffect(float ammoChange, FrontPlayer frontPlayer)
        {
            this.type = EventEffectType.MilitaryResourceChange;
            this.ammoChange = ammoChange;
            this.frontPlayer = frontPlayer;
        }

        public override void Execute()
        {
            this.frontPlayer.ChangeMilitaryRes((int)this.ammoChange);
        }
    }

    public class NextEventEffect : EventEffect
    {
        public long eventId;
        private FrontPlayer frontPlayer;
        public NextEventEffect(long eventId, FrontPlayer frontPlayer)
        {
            this.type = EventEffectType.NextEvent;
            this.eventId = eventId;
            this.frontPlayer = frontPlayer;
        }
        public override void Execute()
        {
            EventTriggerManager.Instance.AddNewEvent(this.eventId, this.frontPlayer);
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
        private FrontPlayer frontPlayer;
        public UnlockConversationEffect(long pawnId, Conversation conversation, FrontPlayer frontPlayer)
        {
            this.type = EventEffectType.UnlockConversation;
            this.pawnId = pawnId;
            this.conversation = conversation;
            this.frontPlayer = frontPlayer;
        }
        public override void Execute()
        {
            if (FrontManager.Instance.GetPawnAvatarByPlayer(this.pawnId, frontPlayer) !=null)
            {
                FrontManager.Instance.GetPawnAvatarByPlayer(this.pawnId, frontPlayer).pawnAgent.PushConversation(this.conversation);
            }
        }
    }
}