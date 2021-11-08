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
            Dictionary<BuffConditionType, float> tempDic = new Dictionary<BuffConditionType, float>();
            if (buffData.conditions != "null")
            {
                tempDic = GetConditions(buffData.conditions);
            }
            return new TimelyBuff(buffData.Id, buffData.name, buffData.description, tempDic, StringToLongArray(buffData.parameter));
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
        public static Dictionary<BuffConditionType, float> GetConditions(string conditionString)
        {
            Dictionary<BuffConditionType, float> tempCondition = new Dictionary<BuffConditionType, float>();
            List<float[]> tempList = StringToListArray(conditionString);
            for (int i = 0; i < tempList.Count; i++)
            {
                tempCondition.Add((BuffConditionType)tempList[i][0], tempList[i][1]);
            }
            return tempCondition;

        }
        private static List<float[]> StringToListArray(string stringlist)
        {
            try
            {
                List<float[]> listArray = new List<float[]>();
                if (stringlist.Contains("]") && stringlist.Contains("["))
                {
                    string[] tempList;
                    stringlist = stringlist.Remove(0, 1);
                    stringlist = stringlist.Remove(stringlist.Length - 1, 1);
                    if (stringlist != "")
                    {
                        tempList = stringlist.Contains(",") ? stringlist.Split(',') : new string[1] { stringlist };
                        for (int i = 0; i < tempList.Length; i++)
                        {
                            tempList[i] = tempList[i].Remove(0, 1);
                            tempList[i] = tempList[i].Remove(tempList[i].Length - 1, 1);
                            float[] tempArray = Array.ConvertAll<string, float>(tempList[i].Split(new char[] { ':' }), s => float.Parse(s));
                            listArray.Add(tempArray);
                        }

                    }
                    else
                    {
                        listArray.Add(new float[1] { 0 });
                    }
                }
                else
                {
                    listArray.Add(new float[1] { 0 });
                }
                return listArray;

            }
            catch (Exception e)
            {
                Debug.LogError("转换错误 可能是配表格式错误导致 请检查表格的填写格式是否正确");
                return null;
            }
        }
    }
}