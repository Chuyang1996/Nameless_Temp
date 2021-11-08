using Nameless.ConfigData;
using Nameless.Data;
using Nameless.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Nameless.Agent
{
    public static class BuffFactory
    {
        // Start is called before the first frame update
        public static Buff GetBuffById(long id)
        {
            return Get(DataManager.Instance.GetBuffData(id));
        }

        public static Buff Get(BuffData buffData)
        {
            return new TimelyBuff(buffData.Id, buffData.name, buffData.description, StringToLongArray(buffData.parameter));
        }
        private static int[] StringToLongArray(string stringlist)
        {
            int[] array;
            if (stringlist.Contains("]") && stringlist.Contains("["))
            {
                stringlist = stringlist.Remove(0, 1);
                stringlist = stringlist.Remove(stringlist.Length - 1, 1);
                array = stringlist.Contains(",") ? Array.ConvertAll<string, int>(stringlist.Split(new char[] { ',' }), s => int.Parse(s)) : new int[1] { int.Parse(stringlist) };
            }
            else
            {
                array = new int[1];
                array[0] = 0;
            }
            return array;
        }
    }
}