using Nameless.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.Agent
{
    public static class ConditionFactory 
    {
        public static ConditionCollection GetConditionById(string conditionStr)
        {
            List<Condition> conditions = new List<Condition>();
            string[] conditionStrArray = StringToStringArray(conditionStr);
            if (conditionStrArray[0] != "null")
            {
                for (int i = 0; i < conditionStrArray.Length; i++)
                {
                    string[] tempConodition = StringToCondition(conditionStrArray[i]);
                    if ((ConditionType)int.Parse(tempConodition[0]) == ConditionType.Killed)
                    {
                        PawnIfKilledCondition pawnIfKilledCondition = new PawnIfKilledCondition(long.Parse(tempConodition[1]), bool.Parse(tempConodition[2]));
                        conditions.Add(pawnIfKilledCondition);
                    }
                    else if ((ConditionType)int.Parse(tempConodition[0]) == ConditionType.EventOption)
                    {
                        PlayerEventOptionChooseCondition playerEventHappenedCondition = new PlayerEventOptionChooseCondition(long.Parse(tempConodition[1]), bool.Parse(tempConodition[2]));
                        conditions.Add(playerEventHappenedCondition);
                    }
                }
            }
            return new ConditionCollection(conditions);
        }

        private static string[] StringToStringArray(string stringlist)
        {
            string[] array;
            if (stringlist.Contains("]") && stringlist.Contains("["))
            {
                stringlist = stringlist.Remove(0, 1);
                stringlist = stringlist.Remove(stringlist.Length - 1, 1);
                array = stringlist.Contains(",") ? stringlist.Split(new char[] { ',' }) : new string[1] { stringlist };
            }
            else
            {
                array = new string[1];
                array[0] = "null";
            }
            return array;
        }

        private static string[] StringToCondition(string stringlist)
        {
            string[] array;
            if (stringlist.Contains("]") && stringlist.Contains("["))
            {
                stringlist = stringlist.Remove(0, 1);
                stringlist = stringlist.Remove(stringlist.Length - 1, 1);
                array = stringlist.Contains(":") ? stringlist.Split(new char[] { ':' }) : new string[1] { stringlist };
            }
            else
            {
                array = new string[1];
                array[0] = "null";
            }
            return array;
        }
    }
}