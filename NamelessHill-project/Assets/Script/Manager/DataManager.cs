using System.Collections.Generic;
using System.Data;
using System.IO;
using Excel;
using OfficeOpenXml;
using UnityEngine;
using Newtonsoft.Json;
using System.Reflection;
using System;
using Nameless.Data;

namespace Nameless.Manager
{
    public class DataManager : Singleton<DataManager>
    {
        ///////////////////////////////数据//////////////////////////
        /// <summary>
        /// 读表数据
        /// </summary>
        #region

        public Dictionary<long, Dictionary<string, string>> characterData;

        #endregion
        ///////////////////////////////数据//////////////////////////




        /////////////////////////////////////方法///////////////////////////////////////////
        /// <summary>
        /// 数据读表周期函数
        /// </summary>
        #region
        /// 初始化读表数据
        public void InitData()
        {
            string data;

            FileStream skillFile = File.Open(Application.streamingAssetsPath + "/" + "Character_Data.txt", FileMode.Open, FileAccess.Read);
            StreamReader skillReader = new StreamReader(skillFile);
            data = skillReader.ReadLine();
            this.characterData = JsonConvert.DeserializeObject<Dictionary<long, Dictionary<string, string>>>(data);

        }
        #endregion


        /// <summary>
        /// 获取接口
        /// </summary>
        /// <param name="NPCname"></param>
        /// <returns></returns>
        #region
        //获取对应装备

        public CharacterData GetCharacter(long id)
        {
            try
            {
                if (this.characterData.ContainsKey(id))
                {
                    CharacterData skill = new CharacterData
                            (id,
                            this.characterData[id]["name"],
                            float.Parse(this.characterData[id]["health"]),
                            float.Parse(this.characterData[id]["crHealth"]),
                            float.Parse(this.characterData[id]["attack"]),
                            float.Parse(this.characterData[id]["crAttack"]),
                            float.Parse(this.characterData[id]["defend"]),
                            float.Parse(this.characterData[id]["crDefend"]),
                            float.Parse(this.characterData[id]["morale"]),
                            float.Parse(this.characterData[id]["crMorale"]),
                            int.Parse(this.characterData[id]["ammo"]),
                            float.Parse(this.characterData[id]["crAmmo"]),
                            float.Parse(this.characterData[id]["food"]),
                            float.Parse(this.characterData[id]["crFood"]),
                            float.Parse(this.characterData[id]["speed"]),
                            float.Parse(this.characterData[id]["crSpeed"]),
                            float.Parse(this.characterData[id]["hit"]),
                            float.Parse(this.characterData[id]["crHit"]),
                            float.Parse(this.characterData[id]["dex"]),
                            float.Parse(this.characterData[id]["crDex"])
                            );
                    return skill;
                }
                else
                {
                    Debug.LogError("楚洋：CharacterData数据转换错误，数据中未能找到此id = " + id + " 的物品，请在逻辑层检查是否对数据进行了初始化,或者Id出了问题，或是否配表出了问题");
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("楚洋:表中的数据名称可能发生改动或者删除，请检查表中的数据title名称是否正确，如果正确请联系楚洋进行核对");
                return null;
            }
        }
        //获取被动技能
    
        //将字符串转换成整型数组
        public int[] StringToIntArray(string stringlist)
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

        //将字符串转换成字典
        #endregion
        /////////////////////////////////////方法///////////////////////////////////////////
    }
}