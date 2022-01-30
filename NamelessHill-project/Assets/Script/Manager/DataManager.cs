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

        public Dictionary<long, Dictionary<string, string>> dialogueData;
        public Dictionary<long, Dictionary<string, string>> dialogueGroupData;

        public Dictionary<long, Dictionary<string, string>> areaData;
        public Dictionary<long, Dictionary<string, string>> pawnGroupData;

        public Dictionary<long, Dictionary<string, string>> conversationData;
        public Dictionary<long, Dictionary<string, string>> conversationOptionData;
        public Dictionary<long, Dictionary<string, string>> conversationEffectData;

        public Dictionary<long, Dictionary<string, string>> notePageData;
        public Dictionary<long, Dictionary<string, string>> noteData;

        public Dictionary<long, Dictionary<string, string>> mapData;
        public Dictionary<long, Dictionary<string, string>> campData;
        public Dictionary<long, Dictionary<string, string>> factionData;

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
            this.characterData = this.ReadFile("Character_Data.txt");
            this.fightSkillData = this.ReadFile("FightSkill_Data.txt");
            this.supportSkillData = this.ReadFile("SupportSkill_Data.txt");
            this.buildSkillData = this.ReadFile("BuildSkill_Data.txt");
            this.buffSkillData = this.ReadFile("Buff_Data.txt");
            this.eventTriggerData = this.ReadFile("EventTrigger_Data.txt");
            this.eventResultData = this.ReadFile("EventResult_Data.txt");
            this.eventOptionData = this.ReadFile("EventOption_Data.txt");
            this.dialogueData = this.ReadFile("Dialogue_Data.txt");
            this.dialogueGroupData = this.ReadFile("DialogueGroup_Data.txt");
            this.areaData = this.ReadFile("Area_Data.txt");
            this.pawnGroupData = this.ReadFile("PawnGroup_Data.txt");
            this.conversationData = this.ReadFile("Conversation_Data.txt");
            this.conversationOptionData = this.ReadFile("ConversationOption_Data.txt");
            this.conversationEffectData = this.ReadFile("ConversationEffect_Data.txt");
            this.notePageData = this.ReadFile("NotePage_Data.txt");
            this.noteData = this.ReadFile("Note_Data.txt");
            this.mapData = this.ReadFile("Map_Data.txt");
            this.campData = this.ReadFile("Camp_Data.txt");
            this.factionData = this.ReadFile("Faction_Data.txt");
            //string data;



        }

        private Dictionary<long, Dictionary<string, string>> ReadFile(string fileName)
        {
            string data;
            FileStream file = File.Open(Application.streamingAssetsPath + "/" + fileName, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(file);
            data = reader.ReadLine();
            return JsonConvert.DeserializeObject<Dictionary<long, Dictionary<string, string>>>(data);
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
                            float.Parse(this.characterData[id]["atkSpeed"]),
                            float.Parse(this.characterData[id]["crAtkSpeed"]),
                            float.Parse(this.characterData[id]["speed"]),
                            float.Parse(this.characterData[id]["crSpeed"]),
                            float.Parse(this.characterData[id]["hit"]),
                            float.Parse(this.characterData[id]["crHit"]),
                            float.Parse(this.characterData[id]["dex"]),
                            float.Parse(this.characterData[id]["crDex"]),
                            int.Parse(this.characterData[id]["leftResNum"]),
                            this.characterData[id]["fightSkill"],
                            this.characterData[id]["supportSkill"],
                            this.characterData[id]["buildSkill"],
                            this.characterData[id]["dialogue"],
                            this.characterData[id]["animPrefab"],
                            this.characterData[id]["selectIcon"],
                            this.characterData[id]["campIcon"],
                            int.Parse(this.characterData[id]["campPosIndex"]),
                            int.Parse(this.characterData[id]["BtnLRpos"]),
                            this.characterData[id]["converIds"]
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
                            float.Parse(this.fightSkillData[id]["defendRate"]),
                            this.fightSkillData[id]["iconName"]
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
                            float.Parse(this.supportSkillData[id]["defendRate"]),
                            this.supportSkillData[id]["iconName"]
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
                            int.Parse(this.buildSkillData[id]["resCost"]),
                            float.Parse(this.buildSkillData[id]["timeCost"]),
                            int.Parse(this.buildSkillData[id]["type"]),
                            this.buildSkillData[id]["parameter"],
                            this.buildSkillData[id]["prefabName"],
                            this.buildSkillData[id]["iconName"]
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
                            this.eventTriggerData[id]["parameter"]
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
                            this.eventResultData[id]["option"],
                            this.eventResultData[id]["Image"]
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
        public DialogueData GetDialogueData(long id)
        {
            try
            {
                if (this.dialogueData.ContainsKey(id))
                {
                    DialogueData skill = new DialogueData
                            (id,
                            this.dialogueData[id]["dialogue"],
                            this.dialogueData[id]["condition"],
                            bool.Parse(this.dialogueData[id]["isAuto"]),
                            float.Parse(this.dialogueData[id]["speedPos"]),
                            float.Parse(this.dialogueData[id]["zoom"]),
                            float.Parse(this.dialogueData[id]["zoomSpeed"]),
                            float.Parse(this.dialogueData[id]["waitTime"]),
                            long.Parse(this.dialogueData[id]["nextId"] == "null" ? "-1" : this.dialogueData[id]["nextId"]),
                            long.Parse(this.dialogueData[id]["nextPawn"] == "null" ? "-1" : this.dialogueData[id]["nextPawn"])
                            );
                    return skill;
                }
                else
                {
                    Debug.LogError("楚洋：DialogueData数据转换错误，数据中未能找到此id = " + id + " 的物品，请在逻辑层检查是否对数据进行了初始化,或者Id出了问题，或是否配表出了问题");
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("楚洋:表中的数据名称可能发生改动或者删除，请检查表中的数据title名称是否正确，如果正确请联系楚洋进行核对");
                return null;
            }
        }
        public DialogueGroupData GetDialogueGroupData(long id)
        {
            try
            {
                if (this.dialogueGroupData.ContainsKey(id))
                {
                    DialogueGroupData skill = new DialogueGroupData
                            (id,
                            this.dialogueGroupData[id]["dialogueIds"]
                            );
                    return skill;
                }
                else
                {
                    Debug.LogError("楚洋：DialogueData数据转换错误，数据中未能找到此id = " + id + " 的物品，请在逻辑层检查是否对数据进行了初始化,或者Id出了问题，或是否配表出了问题");
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("楚洋:表中的数据名称可能发生改动或者删除，请检查表中的数据title名称是否正确，如果正确请联系楚洋进行核对");
                return null;
            }
        }
        public AreaData GetAreaData(long id)
        {
            try
            {
                if (this.areaData.ContainsKey(id))
                {
                    AreaData skill = new AreaData
                            (id,
                            this.areaData[id]["name"],
                            this.areaData[id]["descrption"],
                            int.Parse(this.areaData[id]["type"]),
                            int.Parse(this.areaData[id]["rule"] == "null" ? "0" : this.areaData[id]["rule"]),
                            this.areaData[id]["parameter"]
                            );
                    return skill;
                }
                else
                {
                    Debug.LogError("楚洋：AreaData数据转换错误，数据中未能找到此id = " + id + " 的物品，请在逻辑层检查是否对数据进行了初始化,或者Id出了问题，或是否配表出了问题");
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("楚洋:表中的数据名称可能发生改动或者删除，请检查表中的数据title名称是否正确，如果正确请联系楚洋进行核对");
                return null;
            }
        }
        public PawnGroupData GetPawnGroupData(long id)
        {
            try
            {
                if (this.pawnGroupData.ContainsKey(id))
                {
                    PawnGroupData skill = new PawnGroupData
                            (id,
                            this.pawnGroupData[id]["group"],
                            int.Parse(this.pawnGroupData[id]["waitGenerateTime"]),
                            int.Parse(this.pawnGroupData[id]["durationTime"])
                            );
                    return skill;
                }
                else
                {
                    Debug.LogError("楚洋：AreaData数据转换错误，数据中未能找到此id = " + id + " 的物品，请在逻辑层检查是否对数据进行了初始化,或者Id出了问题，或是否配表出了问题");
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("楚洋:表中的数据名称可能发生改动或者删除，请检查表中的数据title名称是否正确，如果正确请联系楚洋进行核对");
                return null;
            }
        }
        public ConversationData GetConversationData(long id)
        {
            try
            {
                if (this.conversationData.ContainsKey(id))
                {
                    ConversationData skill = new ConversationData
                            (id,
                            this.conversationData[id]["name"],
                            this.conversationData[id]["descrption"],
                            this.conversationData[id]["conversationPawns"],
                            this.conversationData[id]["options"],
                            int.Parse(this.conversationData[id]["side"])
                            );
                    return skill;
                }
                else
                {
                    Debug.LogError("楚洋：ConversationData数据转换错误，数据中未能找到此id = " + id + " 的物品，请在逻辑层检查是否对数据进行了初始化,或者Id出了问题，或是否配表出了问题");
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("楚洋:表中的数据名称可能发生改动或者删除，请检查表中的数据title名称是否正确，如果正确请联系楚洋进行核对");
                return null;
            }
        }
        public ConversationOptionData GetConversationOptionData(long id)
        {
            try
            {
                if (this.conversationOptionData.ContainsKey(id))
                {
                    ConversationOptionData skill = new ConversationOptionData
                            (id,
                            this.conversationOptionData[id]["name"],
                            this.conversationOptionData[id]["descrption"],
                            this.conversationOptionData[id]["effects"]
                            );
                    return skill;
                }
                else
                {
                    Debug.LogError("楚洋：ConversationOptionData数据转换错误，数据中未能找到此id = " + id + " 的物品，请在逻辑层检查是否对数据进行了初始化,或者Id出了问题，或是否配表出了问题");
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("楚洋:表中的数据名称可能发生改动或者删除，请检查表中的数据title名称是否正确，如果正确请联系楚洋进行核对");
                return null;
            }
        }
        public ConversationEffectData GetConversationEffectData(long id)
        {
            try
            {
                if (this.conversationEffectData.ContainsKey(id))
                {
                    ConversationEffectData skill = new ConversationEffectData
                            (id,
                            this.conversationEffectData[id]["name"],
                            this.conversationEffectData[id]["descrption"],
                            int.Parse(this.conversationEffectData[id]["type"]),
                            this.conversationEffectData[id]["parameter"]
                            );
                    return skill;
                }
                else
                {
                    Debug.LogError("楚洋：ConversationEffectData数据转换错误，数据中未能找到此id = " + id + " 的物品，请在逻辑层检查是否对数据进行了初始化,或者Id出了问题，或是否配表出了问题");
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("楚洋:表中的数据名称可能发生改动或者删除，请检查表中的数据title名称是否正确，如果正确请联系楚洋进行核对");
                return null;
            }
        }
        public NotePageData GetNotePageData(long id)
        {
            try
            {
                if (this.notePageData.ContainsKey(id))
                {
                    NotePageData skill = new NotePageData
                            (id,
                            this.notePageData[id]["name"],
                            this.notePageData[id]["descrption"],
                            this.notePageData[id]["noteTemplate"],
                            this.notePageData[id]["noteIds"]
                            );
                    return skill;
                }
                else
                {
                    Debug.LogError("楚洋：NotePageData数据转换错误，数据中未能找到此id = " + id + " 的物品，请在逻辑层检查是否对数据进行了初始化,或者Id出了问题，或是否配表出了问题");
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("楚洋:表中的数据名称可能发生改动或者删除，请检查表中的数据title名称是否正确，如果正确请联系楚洋进行核对");
                return null;
            }
        }
        public NoteData GetNoteData(long id)
        {
            try
            {
                if (this.noteData.ContainsKey(id))
                {
                    NoteData skill = new NoteData
                            (id,
                            this.noteData[id]["name"],
                            this.noteData[id]["descrption"],
                            this.noteData[id]["noteImage"]
                            );
                    return skill;
                }
                else
                {
                    Debug.LogError("楚洋：NoteData数据转换错误，数据中未能找到此id = " + id + " 的物品，请在逻辑层检查是否对数据进行了初始化,或者Id出了问题，或是否配表出了问题");
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("楚洋:表中的数据名称可能发生改动或者删除，请检查表中的数据title名称是否正确，如果正确请联系楚洋进行核对");
                return null;
            }
        }
        public MapData GetMapData(long id)
        {
            try
            {
                if (this.mapData.ContainsKey(id))
                {
                    MapData skill = new MapData
                            (id,
                            this.mapData[id]["name"],
                            this.mapData[id]["descrption"],
                            this.mapData[id]["mapName"],
                            long.Parse(this.mapData[id]["nextCampId"]),
                            this.mapData[id]["transInfoShowName"],
                            this.mapData[id]["defaultInitPos"],
                            this.mapData[id]["mapBgm"]
                            );
                    return skill;
                }
                else
                {
                    Debug.LogError("楚洋：MapData数据转换错误，数据中未能找到此id = " + id + " 的物品，请在逻辑层检查是否对数据进行了初始化,或者Id出了问题，或是否配表出了问题");
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("楚洋:表中的数据名称可能发生改动或者删除，请检查表中的数据title名称是否正确，如果正确请联系楚洋进行核对");
                return null;
            }
        }
        public CampData GetCampData(long id)
        {
            try
            {
                if (this.campData.ContainsKey(id))
                {
                    CampData skill = new CampData
                            (id,
                            this.campData[id]["name"],
                            this.campData[id]["descrption"],
                            this.campData[id]["campName"],
                            long.Parse(this.campData[id]["nextBattleId"]),
                            this.campData[id]["campBgm"]
                            );
                    return skill;
                }
                else
                {
                    Debug.LogError("楚洋：MapData数据转换错误，数据中未能找到此id = " + id + " 的物品，请在逻辑层检查是否对数据进行了初始化,或者Id出了问题，或是否配表出了问题");
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("楚洋:表中的数据名称可能发生改动或者删除，请检查表中的数据title名称是否正确，如果正确请联系楚洋进行核对");
                return null;
            }
        }
        public FactionData GetFactionData(long id)
        {
            try
            {
                if (this.factionData.ContainsKey(id))
                {
                    FactionData skill = new FactionData
                            (id,
                            this.factionData[id]["name"],
                            this.factionData[id]["txt"],
                            this.factionData[id]["friendly_to"],
                            this.factionData[id]["hostile_to"],
                            this.factionData[id]["healthColor"],
                            this.factionData[id]["pathColor"],
                            this.factionData[id]["walkColor"],
                            this.factionData[id]["supportColor"],
                            this.factionData[id]["areaColor"],
                            this.factionData[id]["battleColor"]
                            );
                    return skill;
                }
                else
                {
                    Debug.LogError("楚洋：FactionData数据转换错误，数据中未能找到此id = " + id + " 的物品，请在逻辑层检查是否对数据进行了初始化,或者Id出了问题，或是否配表出了问题");
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("楚洋:表中的数据名称可能发生改动或者删除，请检查表中的数据title名称是否正确，如果正确请联系楚洋进行核对");
                return null;
            }
        }

        public List<FactionData> GetFactions()
        {
            List<FactionData> factionDatas = new List<FactionData>();
            foreach(var child in this.factionData)
            {
                factionDatas.Add(this.GetFactionData(child.Key));
            }
            return factionDatas;
        }
        #endregion
        /////////////////////////////////////方法///////////////////////////////////////////

    }
}