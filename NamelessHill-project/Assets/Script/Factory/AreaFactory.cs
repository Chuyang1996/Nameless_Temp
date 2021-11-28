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
    public static class AreaFactory
    {
        public static AreaAgent GetAreaById(long id)
        {
            return Get(DataManager.Instance.GetAreaData(id));
        }

        public static AreaAgent Get(AreaData areaData)
        {
            long[] pawnsGroupId = StringToLongArray(areaData.parameter);
            List<PawnGroup> pawnGroups = new List<PawnGroup>();
            if (pawnsGroupId[0] != -1)
            {
                for (int i = 0; i < pawnsGroupId.Length; i++)
                {
                    pawnGroups.Add(PawnGroupFactory.GetPawnGroupById(pawnsGroupId[i]));
                }
            }

            return new AreaAgent(areaData.Id, areaData.name, areaData.description, (AreaType)areaData.type, (GenerateRuleType)areaData.rule, pawnGroups);
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