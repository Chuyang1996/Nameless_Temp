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
        public static AreaAgent GetAreaById(long id,FrontPlayer frontPlayer)
        {
            return Get(DataManager.Instance.GetAreaData(id), frontPlayer);
        }

        public static AreaAgent Get(AreaData areaData, FrontPlayer frontPlayer)
        {
            long[] pawnsGroupId = StringToLongArray(areaData.parameter);
            List<PawnGroup> pawnGroups = new List<PawnGroup>();
            long eventOptionId = -1;
            if (pawnsGroupId.Length != 0)
            {
                eventOptionId = pawnsGroupId[0];
                for (int i = 1; i < pawnsGroupId.Length; i++)
                {
                    pawnGroups.Add(PawnGroupFactory.GetPawnGroupById(pawnsGroupId[i]));
                }
            }

            return new AreaAgent(areaData.Id, areaData.name, areaData.description, (AreaType)areaData.type, eventOptionId, (GenerateRuleType)areaData.rule, pawnGroups, frontPlayer);
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