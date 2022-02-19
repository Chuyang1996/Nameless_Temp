using Nameless.DataMono;
using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Nameless.Data
{
    public enum EventTriggerType
    {
        TimePass = 0,
        MilitaryResLess = 1,
        EnemyKillNum = 2,
        PawnArriveOnArea = 3,
        BuildOnArea = 4,
        PawnEnterBattle = 5,
    }
    abstract public class EventTrigger
    {
        public long id;
        public string name;
        public string descrption;
        public EventTriggerType type;
        public ConditionCollection conditionCollection;
        public List<long> parameter;


    }

    public class EventTimePass : EventTrigger
    {
        public int passTime;
        public bool IsTrigger(int passTime,FrontPlayer frontPlayer)
        {
            if (this.passTime == passTime && this.conditionCollection.CanPass(frontPlayer))
                return true;
            else
                return false;
        }
        public EventTimePass(long id, string name, string descrption, int passTime, ConditionCollection conditionCollection)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.type = EventTriggerType.TimePass;
            this.conditionCollection = conditionCollection;
            this.passTime = passTime; 
            
        }
    }

    public class EventMilitaryResLess: EventTrigger
    {
        public int militaryRes;
        public bool IsTrigger(int lastmilitaryRes, int aftermilitaryRes, FrontPlayer frontPlayer)
        {
            if (aftermilitaryRes < this.militaryRes && this.militaryRes <= lastmilitaryRes && this.conditionCollection.CanPass(frontPlayer))
                return true;
            else
                return false;
        }
        public EventMilitaryResLess(long id, string name, string descrption, int militaryRes, ConditionCollection conditionCollection)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.type = EventTriggerType.MilitaryResLess;
            this.conditionCollection = conditionCollection;
            this.militaryRes = militaryRes;

        }
    }

    public class EventEnemyKillLess : EventTrigger
    {
        public int enemyKill;
        public bool IsTrigger(int lastEnemyKill, int afterEnemyKill, FrontPlayer frontPlayer)
        {
            if (lastEnemyKill < this.enemyKill && this.enemyKill <= afterEnemyKill && this.conditionCollection.CanPass(frontPlayer))
                return true;
            else
                return false;
        }
        public EventEnemyKillLess(long id, string name, string descrption, int enemyKill, ConditionCollection conditionCollection)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.type = EventTriggerType.EnemyKillNum;
            this.conditionCollection = conditionCollection;
            this.enemyKill = enemyKill;

        }
    }

    public class EventPawnArrive : EventTrigger
    {
        public long pawnId;
        public int limitTimes;
        public int recordTime = 0;
        public bool always = false;
        public List<int> areaLocalIds;

        public bool IsTrigger(long pawnId, int areaLocalId, FrontPlayer frontPlayer)
        {
            if(pawnId == this.pawnId && this.areaLocalIds.Contains(areaLocalId) && this.conditionCollection.CanPass(frontPlayer))
            {
                if (this.always)
                {
                    return true;
                }
                else if(this.recordTime < this.limitTimes)
                {
                    this.recordTime++;
                    return true;
                }
            }
            return false;
        }
        public EventPawnArrive(long id, string name, string descrption, long pawnId, int limitTime, List<int> areaLocalId, ConditionCollection conditionCollection)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.type = EventTriggerType.PawnArriveOnArea;
            this.conditionCollection = conditionCollection;
            this.recordTime = 0;
            this.pawnId = pawnId;
            this.limitTimes = limitTime;
            this.always = limitTime <= 0 ? true : false;
            this.areaLocalIds = areaLocalId;
        }
    }


    public class EventBuildOnArea : EventTrigger
    {
        public BuildType buildType;
        public int recordTime = 0;
        public bool IsTrigger(BuildType buildType, FrontPlayer frontPlayer)
        {
            if (this.buildType == buildType && this.recordTime == 0 && this.conditionCollection.CanPass(frontPlayer))
            {
                this.recordTime++;
                return true;
            }

            return false;
        }

        public EventBuildOnArea(long id, string name, string descrption, BuildType buildType, ConditionCollection conditionCollection)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.type = EventTriggerType.BuildOnArea;
            this.conditionCollection = conditionCollection;
            this.buildType = buildType;
            this.recordTime = 0;

        }
    }

    public class EventPawnStartBattle : EventTrigger
    {
        public long pawnId;
        public int recordTime = 0;
        public bool IsTrigger(long pawnId, FrontPlayer frontPlayer)
        {
            if (this.pawnId == pawnId && this.recordTime == 0 && this.conditionCollection.CanPass(frontPlayer))
            {
                this.recordTime++;
                return true;
            }

            return false;
        }

        public EventPawnStartBattle(long id, string name, string descrption, long pawnId, ConditionCollection conditionCollection)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.type = EventTriggerType.PawnEnterBattle;
            this.conditionCollection = conditionCollection;
            this.pawnId = pawnId;
            this.recordTime = 0;
        }
    }


}