using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.Data
{
    public enum ConditionType
    {
        Killed = 1,
        EventOption = 2,
    }
    public class ConditionCollection
    {
        private List<Condition> conditions = new List<Condition>();
        public ConditionCollection(List<Condition> conditions)
        {
            this.conditions = conditions;
        }
        public bool CanPass(FrontPlayer frontPlayer)
        {
            for(int i = 0; i < this.conditions.Count; i++)
            {
                if (!this.conditions[i].CanPass(frontPlayer))
                    return false;
            }

            return true;
        }
    }
    abstract public class Condition
    {
        public ConditionType conditionType;
        public abstract bool CanPass(FrontPlayer frontPlayer);

    }

    public class PawnIfKilledCondition : Condition
    {
        private long pawnId;
        private bool isKilled;
        public PawnIfKilledCondition(long pawnId, bool isKilled)
        {
            this.conditionType = ConditionType.Killed;
            this.pawnId = pawnId;
            this.isKilled = isKilled;
        }
        public override bool CanPass(FrontPlayer frontPlayer)
        {
            return this.isKilled == frontPlayer.eventCollections.IsPawnKilled(this.pawnId);
            
        }
    }

    public class PlayerEventOptionChooseCondition : Condition
    {
        private long eventId;
        private bool isChoosed;
        public PlayerEventOptionChooseCondition(long pawnId, bool isChoosed)
        {
            this.conditionType = ConditionType.EventOption;
            this.eventId = pawnId;
            this.isChoosed = isChoosed;
        }
        public override bool CanPass(FrontPlayer frontPlayer)
        {
            return this.isChoosed == frontPlayer.eventCollections.IsEventOptionChoosed(this.eventId);

        }
    }

}