using Nameless.ConfigData;
using Nameless.Data;
using Nameless.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.Agent
{
    public static class PawnGroupFactory
    {
        public static PawnGroup GetPawnGroupById(long id)
        {
            return Get(DataManager.Instance.GetPawnGroupData(id));
        }

        public static PawnGroup Get(PawnGroupData pawnGroupData)
        {
            long[] pawnsId = StringToIntArray(pawnGroupData.group);
            List<Pawn> pawns = new List<Pawn>();
            for(int i = 0; i < pawnsId.Length; i++)
            {
                pawns.Add(PawnFactory.GetPawnById(pawnsId[i]));
            }

            return new PawnGroup(pawnGroupData.Id, pawns, pawnGroupData.waitGenerateTime, pawnGroupData.durationTime);
            // Start is called before the first frame update
        }


        private static long[] StringToIntArray(string stringlist)
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
                array[0] = 0;
            }
            return array;
        }
    }
}