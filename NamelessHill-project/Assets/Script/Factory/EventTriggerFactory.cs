using Nameless.ConfigData;
using Nameless.Data;
using Nameless.DataMono;
using Nameless.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nameless.Agent
{
    public static class EventTriggerFactory
    {
        // Start is called before the first frame update
        public static EventTrigger GetEventTriggerById(long id)
        {
            return Get(DataManager.Instance.GetEventTriggerData(id));
        }

        public static EventTrigger Get(EventTriggerData buffData)
        {
            if((EventTriggerType)buffData.type == EventTriggerType.TimePass)
            {
                long[] temp = StringToLongArray(buffData.parameter);

                return new EventTimePass(buffData.Id, buffData.name, buffData.descrption, (int)temp[0], ConditionFactory.GetConditionById(buffData.condition));
            }
            else if ((EventTriggerType)buffData.type == EventTriggerType.MilitaryResLess)
            {
                long[] temp = StringToLongArray(buffData.parameter);
                return new EventMilitaryResLess(buffData.Id, buffData.name, buffData.descrption, (int)temp[0], ConditionFactory.GetConditionById(buffData.condition));
            }
            else if ((EventTriggerType)buffData.type == EventTriggerType.EnemyKillNum)
            {
                long[] temp = StringToLongArray(buffData.parameter);
                return new EventEnemyKillLess(buffData.Id, buffData.name, buffData.descrption, (int)temp[0], ConditionFactory.GetConditionById(buffData.condition));
            }
            else if ((EventTriggerType)buffData.type == EventTriggerType.PawnArriveOnArea)
            {
                long[] temp = StringToLongArray(buffData.parameter);
                List<int> areaLocalIds = new List<int>();
                for(int i = 2;i < temp.Length; i++)
                {
                    areaLocalIds.Add((int)temp[i]);
                }
                return new EventPawnArrive(buffData.Id, buffData.name, buffData.descrption, (long)temp[0], (int)temp[1], areaLocalIds, ConditionFactory.GetConditionById(buffData.condition));
            }
            else if ((EventTriggerType)buffData.type == EventTriggerType.BuildOnArea)
            {
                long[] temp = StringToLongArray(buffData.parameter);
                return new EventBuildOnArea(buffData.Id, buffData.name, buffData.descrption, (BuildType)temp[0], ConditionFactory.GetConditionById(buffData.condition));
            }
            else if ((EventTriggerType)buffData.type == EventTriggerType.PawnEnterBattle)
            {
                long[] temp = StringToLongArray(buffData.parameter);
                return new EventPawnStartBattle(buffData.Id, buffData.name, buffData.descrption, temp[0], ConditionFactory.GetConditionById(buffData.condition));
            }
            return null;
        }

        private static long[] StringToLongArray(string stringlist)
        {
            long[] array;
            if (stringlist.Contains("]") && stringlist.Contains("["))
            {
                stringlist = stringlist.Remove(0, 1);
                stringlist = stringlist.Remove(stringlist.Length - 1, 1);
                array = stringlist.Contains(",") ? Array.ConvertAll<string, long>(stringlist.Split(new char[] { ',' }), s => long.Parse(s)) : new long[1] { long.Parse(stringlist) };
            }
            else
            {
                array = new long[1];
                array[0] = -1;
            }
            return array;
        }
    }
}