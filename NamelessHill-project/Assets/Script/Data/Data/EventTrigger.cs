using Nameless.DataMono;
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
        PawnEnterBattle = 5
    }
    abstract public class EventTrigger
    {
        public long id;
        public string name;
        public string descrption;
        public EventTriggerType type;
        public List<long> parameter;

    }

    public class EventTimePass : EventTrigger
    {
        public int passTime;
        public bool IsTrigger(int passTime)
        {
            if (this.passTime == passTime)
                return true;
            else
                return false;
        }
        public EventTimePass(long id, string name, string descrption, int passTime)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.type = EventTriggerType.TimePass; 
            this.passTime = passTime; 
            
        }
    }

    public class EventMilitaryResLess: EventTrigger
    {
        public int militaryRes;
        public bool IsTrigger(int lastmilitaryRes, int aftermilitaryRes)
        {
            if (aftermilitaryRes < this.militaryRes && this.militaryRes <= lastmilitaryRes)
                return true;
            else
                return false;
        }
        public EventMilitaryResLess(long id, string name, string descrption, int militaryRes)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.type = EventTriggerType.MilitaryResLess;
            this.militaryRes = militaryRes;

        }
    }

    public class EventEnemyKillLess : EventTrigger
    {
        public int enemyKill;
        public bool IsTrigger(int lastEnemyKill, int afterEnemyKill)
        {
            if (lastEnemyKill < this.enemyKill && this.enemyKill <= afterEnemyKill)
                return true;
            else
                return false;
        }
        public EventEnemyKillLess(long id, string name, string descrption, int enemyKill)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.type = EventTriggerType.EnemyKillNum;
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

        public bool IsTrigger(long pawnId, int areaLocalId)
        {
            if(pawnId == this.pawnId && this.areaLocalIds.Contains(areaLocalId))
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
        public EventPawnArrive(long id, string name, string descrption, long pawnId, int limitTime, List<int> areaLocalId)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.type = EventTriggerType.PawnArriveOnArea;
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
        public bool IsTrigger(BuildType buildType)
        {
            if (this.buildType == buildType && this.recordTime == 0)
            {
                this.recordTime++;
                return true;
            }

            return false;
        }

        public EventBuildOnArea(long id, string name, string descrption, BuildType buildType)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.type = EventTriggerType.BuildOnArea;
            this.buildType = buildType;
            this.recordTime = 0;

        }
    }

    public class EventPawnStartBattle : EventTrigger
    {
        public long pawnId;
        public int recordTime = 0;
        public bool IsTrigger(long pawnId)
        {
            if (this.pawnId == pawnId && this.recordTime == 0)
            {
                this.recordTime++;
                return true;
            }

            return false;
        }

        public EventPawnStartBattle(long id, string name, string descrption, long pawnId)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.type = EventTriggerType.PawnEnterBattle;
            this.pawnId = pawnId;
            this.recordTime = 0;
        }
    }
}