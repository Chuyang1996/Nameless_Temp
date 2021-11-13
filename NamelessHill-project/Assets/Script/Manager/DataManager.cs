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
using Nameless.ConfigData;

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
        public Dictionary<long, Dictionary<string, string>> fightSkillData;
        public Dictionary<long, Dictionary<string, string>> supportSkillData;
        public Dictionary<long, Dictionary<string, string>> buildSkillData;
        public Dictionary<long, Dictionary<string, string>> buffSkillData;
        public Dictionary<long, Dictionary<string, string>> eventTriggerData;
        public Dictionary<long, Dictionary<string, string>> eventResultData;
        public Dictionary<long, Dictionary<string, string>> eventOptionData;

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

            FileStream characterFile = File.Open(Application.streamingAssetsPath + "/" + "Character_Data.txt", FileMode.Open, FileAccess.Read);
            StreamReader characterReader = new StreamReader(characterFile);
            data = characterReader.ReadLine();
            this.characterData = JsonConvert.DeserializeObject<Dictionary<long, Dictionary<string, string>>>(data);

            FileStream fightSkillFile = File.Open(Application.streamingAssetsPath + "/" + "FightSkill_Data.txt", FileMode.Open, FileAccess.Read);
            StreamReader fightSkillReader = new StreamReader(fightSkillFile);
            data = fightSkillReader.ReadLine();
            this.fightSkillData = JsonConvert.DeserializeObject<Dictionary<long, Dictionary<string, string>>>(data);

            FileStream supportSkillFile = File.Open(Application.streamingAssetsPath + "/" + "SupportSkill_Data.txt", FileMode.Open, FileAccess.Read);
            StreamReader supportSkillReader = new StreamReader(supportSkillFile);
            data = supportSkillReader.ReadLine();
            this.supportSkillData = JsonConvert.DeserializeObject<Dictionary<long, Dictionary<string, string>>>(data);

            FileStream buildSkillFile = File.Open(Application.streamingAssetsPath + "/" + "BuildSkill_Data.txt", FileMode.Open, FileAccess.Read);
            StreamReader buildSkillReader = new StreamReader(buildSkillFile);
            data = buildSkillReader.ReadLine();
            this.buildSkillData = JsonConvert.DeserializeObject<Dictionary<long, Dictionary<string, string>>>(data);

            FileStream buffSkillFile = File.Open(Application.streamingAssetsPath + "/" + "Buff_Data.txt", FileMode.Open, FileAccess.Read);
            StreamReader buffSkillReader = new StreamReader(buffSkillFile);
            data = buffSkillReader.ReadLine();
            this.buffSkillData = JsonConvert.DeserializeObject<Dictionary<long, Dictionary<string, string>>>(data);

            FileStream eventTriggerFile = File.Open(Application.streamingAssetsPath + "/" + "EventTrigger_Data.txt", FileMode.Open, FileAccess.Read);
            StreamReader eventTriggerReader = new StreamReader(eventTriggerFile);
            data = eventTriggerReader.ReadLine();
            this.eventTriggerData = JsonConvert.DeserializeObject<Dictionary<long, Dictionary<string, string>>>(data);

            FileStream eventResultFile = File.Open(Application.streamingAssetsPath + "/" + "EventResult_Data.txt", FileMode.Open, FileAccess.Read);
            StreamReader eventResultReader = new StreamReader(eventResultFile);
            data = eventResultReader.ReadLine();
            this.eventResultData = JsonConvert.DeserializeObject<Dictionary<long, Dictionary<string, string>>>(data);

            FileStream eventOptionFile = File.Open(Application.streamingAssetsPath + "/" + "EventOption_Data.txt", FileMode.Open, FileAccess.Read);
            StreamReader eventOptionReader = new StreamReader(eventOptionFile);
            data = eventOptionReader.ReadLine();
            this.eventOptionData = JsonConvert.DeserializeObject<Dictionary<long, Dictionary<string, string>>>(data);

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
                            this.characterData[id]["descrption"],
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
                            float.Parse(this.characterData[id]["crDex"]),
                            this.characterData[id]["fightSkill"],
                            this.characterData[id]["supportSkill"],
                            this.characterData[id]["buildSkill"]
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
        public FightSkillData GetFightSkillData(long id)
        {
            try
            {
                if (this.fightSkillData.ContainsKey(id))
                {
                    FightSkillData skill = new FightSkillData
                            (id,
                            this.fightSkillData[id]["name"],
                            this.fightSkillData[id]["descrption"],
                            this.fightSkillData[id]["condition"],
                            float.Parse(this.fightSkillData[id]["attackRate"]),
                            float.Parse(this.fightSkillData[id]["defendRate"])
                            );
                    return skill;
                }
                else
                {
                    Debug.LogError("楚洋：FightSkillData数据转换错误，数据中未能找到此id = " + id + " 的物品，请在逻辑层检查是否对数据进行了初始化,或者Id出了问题，或是否配表出了问题");
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("楚洋:表中的数据名称可能发生改动或者删除，请检查表中的数据title名称是否正确，如果正确请联系楚洋进行核对");
                return null;
            }
        }
        public SupportSkillData GetSupportSkillData(long id)
        {
            try
            {
                if (this.supportSkillData.ContainsKey(id))
                {
                    SupportSkillData skill = new SupportSkillData
                            (id,
                            this.supportSkillData[id]["name"],
                            this.supportSkillData[id]["descrption"],
                            this.supportSkillData[id]["condition"],
                            this.supportSkillData[id]["buff"],
                            float.Parse(this.supportSkillData[id]["attackRate"]),
                            float.Parse(this.supportSkillData[id]["defendRate"])
                            );
                    return skill;
                }
                else
                {
                    Debug.LogError("楚洋：SupportSkillData数据转换错误，数据中未能找到此id = " + id + " 的物品，请在逻辑层检查是否对数据进行了初始化,或者Id出了问题，或是否配表出了问题");
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("楚洋:表中的数据名称可能发生改动或者删除，请检查表中的数据title名称是否正确，如果正确请联系楚洋进行核对");
                return null;
            }
        }
        public BuildSkillData GetBuildSkillData(long id)
        {
            try
            {
                if (this.buildSkillData.ContainsKey(id))
                {
                    BuildSkillData skill = new BuildSkillData
                            (id,
                            this.buildSkillData[id]["name"],
                            this.buildSkillData[id]["descrption"],
                            this.buildSkillData[id]["condition"],
                            float.Parse(this.buildSkillData[id]["costMedicineRate"]),
                            float.Parse(this.buildSkillData[id]["costAmmoRate"])
                            );
                    return skill;
                }
                else
                {
                    Debug.LogError("楚洋：BuildSkillData数据转换错误，数据中未能找到此id = " + id + " 的物品，请在逻辑层检查是否对数据进行了初始化,或者Id出了问题，或是否配表出了问题");
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("楚洋:表中的数据名称可能发生改动或者删除，请检查表中的数据title名称是否正确，如果正确请联系楚洋进行核对");
                return null;
            }
        }
        public BuffData GetBuffData(long id)
        {
            try
            {
                if (this.buffSkillData.ContainsKey(id))
                {
                    BuffData skill = new BuffData
                            (id,
                            this.buffSkillData[id]["name"],
                            this.buffSkillData[id]["descrption"],
                            this.buffSkillData[id]["condition"],
                            int.Parse(this.buffSkillData[id]["type"]),
                            this.buffSkillData[id]["parameter"]
                            );
                    return skill;
                }
                else
                {
                    Debug.LogError("楚洋：BuildSkillData数据转换错误，数据中未能找到此id = " + id + " 的物品，请在逻辑层检查是否对数据进行了初始化,或者Id出了问题，或是否配表出了问题");
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("楚洋:表中的数据名称可能发生改动或者删除，请检查表中的数据title名称是否正确，如果正确请联系楚洋进行核对");
                return null;
            }
        }
        public EventTriggerData GetEventTriggerData(long id)
        {
            try
            {
                if (this.eventTriggerData.ContainsKey(id))
                {
                    EventTriggerData skill = new EventTriggerData
                            (id,
                            this.eventTriggerData[id]["name"],
                            this.eventTriggerData[id]["descrption"],
                            int.Parse(this.eventTriggerData[id]["type"]),
                            float.Parse(this.eventTriggerData[id]["parameter"])
                            );
                    return skill;
                }
                else
                {
                    Debug.LogError("楚洋：EventTriggerData数据转换错误，数据中未能找到此id = " + id + " 的物品，请在逻辑层检查是否对数据进行了初始化,或者Id出了问题，或是否配表出了问题");
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("楚洋:表中的数据名称可能发生改动或者删除，请检查表中的数据title名称是否正确，如果正确请联系楚洋进行核对");
                return null;
            }
        }
        public EventResultData GetEventResultData(long id)
        {
            try
            {
                if (this.eventResultData.ContainsKey(id))
                {
                    EventResultData skill = new EventResultData
                            (id,
                            this.eventResultData[id]["name"],
                            this.eventResultData[id]["descrption"],
                            long.Parse(this.eventResultData[id]["condition"]),
                            this.eventResultData[id]["option"]
                            );
                    return skill;
                }
                else
                {
                    Debug.LogError("楚洋：EventResultData数据转换错误，数据中未能找到此id = " + id + " 的物品，请在逻辑层检查是否对数据进行了初始化,或者Id出了问题，或是否配表出了问题");
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("楚洋:表中的数据名称可能发生改动或者删除，请检查表中的数据title名称是否正确，如果正确请联系楚洋进行核对");
                return null;
            }
        }
        public EventOptionData GetEventOptionData(long id)
        {
            try
            {
                if (this.eventOptionData.ContainsKey(id))
                {
                    EventOptionData skill = new EventOptionData
                            (id,
                            this.eventOptionData[id]["name"],
                            this.eventOptionData[id]["descrption"],
                            this.eventOptionData[id]["effect"]
                            );
                    return skill;
                }
                else
                {
                    Debug.LogError("楚洋：EventOptionData数据转换错误，数据中未能找到此id = " + id + " 的物品，请在逻辑层检查是否对数据进行了初始化,或者Id出了问题，或是否配表出了问题");
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("楚洋:表中的数据名称可能发生改动或者删除，请检查表中的数据title名称是否正确，如果正确请联系楚洋进行核对");
                return null;
            }
        }
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