using Nameless.ConfigData;
using Nameless.Data;
using Nameless.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nameless.Agent
{
    public enum SkillFactoryType
    {
        FightSkill = 0,
        SupportSkill = 1,
        BuildSkill = 2,
    }
    public static class SkillFactory 
    {
        public static Skill GetSkillById(SkillFactoryType skillType, long id)
        {
            if (skillType == SkillFactoryType.SupportSkill)
                return Get(DataManager.Instance.GetSupportSkillData(id));
            else if (skillType == SkillFactoryType.FightSkill)
                return Get(DataManager.Instance.GetFightSkillData(id));
            else
                return Get(DataManager.Instance.GetBuildSkillData(id));
        }
        public static Skill Get(FightSkillData fightSkillData)
        {
            Dictionary<SkillConditionType, float> tempDic = new Dictionary<SkillConditionType, float>();
            if(fightSkillData.condition != "null")
            {
                tempDic = GetConditions(fightSkillData.condition);
            }
            return new FightSkill(fightSkillData.Id, fightSkillData.name, fightSkillData.description, tempDic,fightSkillData.attackRate, fightSkillData.defendRate,SpriteManager.Instance.FindSpriteByName(AtlasType.IconImage, fightSkillData.iconName));
        }
        public static Skill Get(SupportSkillData supportSkillData)
        {
            Dictionary<SkillConditionType, float> tempDic = new Dictionary<SkillConditionType, float>();
            if (supportSkillData.condition != "null")
            {
                tempDic = GetConditions(supportSkillData.condition);
            }
            List<Buff> buffs = new List<Buff>();
            long[] ids = StringToLongArray(supportSkillData.buff);
            if (ids.Length > 0 && ids[0] != 0)//待修改 感觉这种不好
            {
                for (int i = 0; i < ids.Length; i++)
                {
                    buffs.Add(BuffFactory.GetBuffById(ids[i]));
                }
            }
            return new SupportSkill(supportSkillData.Id, supportSkillData.name, supportSkillData.description, tempDic, buffs, supportSkillData.attackRate, supportSkillData.defendRate, SpriteManager.Instance.FindSpriteByName(AtlasType.IconImage, supportSkillData.iconName));
        }
        public static Skill Get(BuildSkillData buildSkillData)
        {
            Dictionary<SkillConditionType, float> tempDic = new Dictionary<SkillConditionType, float>();
            if (buildSkillData.condition != "null")
            {
                tempDic = GetConditions(buildSkillData.condition);
            }
            if((BuildType)buildSkillData.type == BuildType.Obstacle)
            {
                float healthObstacle =StringToFloatArray(buildSkillData.parameter)[0];
                float healthBuildingObstacle = healthObstacle* StringToFloatArray(buildSkillData.parameter)[1];
                Obstacle obstacle = new Obstacle(buildSkillData.Id, buildSkillData.name, buildSkillData.description, healthObstacle, healthBuildingObstacle,buildSkillData.resCost, buildSkillData.prefabName,buildSkillData.timeCost, SpriteManager.Instance.FindSpriteByName(AtlasType.IconImage, buildSkillData.iconName));
                return new BuildSkill(buildSkillData.Id, buildSkillData.name, buildSkillData.description, tempDic, BuildType.Obstacle, obstacle, SpriteManager.Instance.FindSpriteByName(AtlasType.IconImage,buildSkillData.iconName));
            }
            else if ((BuildType)buildSkillData.type == BuildType.Bunker)
            {
                float healthBunker = StringToFloatArray(buildSkillData.parameter)[0];
                float healthBuildingBunker = healthBunker* StringToFloatArray(buildSkillData.parameter)[1];
                Bunker bunker = new Bunker(buildSkillData.Id, buildSkillData.name, buildSkillData.description, healthBunker, healthBuildingBunker, buildSkillData.resCost, buildSkillData.prefabName, buildSkillData.timeCost, SpriteManager.Instance.FindSpriteByName(AtlasType.IconImage, buildSkillData.iconName));
                return new BuildSkill(buildSkillData.Id, buildSkillData.name, buildSkillData.description, tempDic, BuildType.Bunker, bunker, SpriteManager.Instance.FindSpriteByName(AtlasType.IconImage, buildSkillData.iconName));
            }
            else if ((BuildType)buildSkillData.type == BuildType.Cannon)
            {
                float healthCannon = StringToFloatArray(buildSkillData.parameter)[0];
                float healthBuildingCannon = healthCannon * StringToFloatArray(buildSkillData.parameter)[1];
                int minRange = (int)(StringToFloatArray(buildSkillData.parameter)[2]);
                int maxRange = (int)(StringToFloatArray(buildSkillData.parameter)[3]);
                int exploreRange = (int)(StringToFloatArray(buildSkillData.parameter)[4]);
                int exploreTime = (int)(StringToFloatArray(buildSkillData.parameter)[5]);
                float exploreDamage = StringToFloatArray(buildSkillData.parameter)[6];
                float cdTime = StringToFloatArray(buildSkillData.parameter)[7];
                Cannon cannon = new Cannon(buildSkillData.Id, buildSkillData.name, buildSkillData.description, healthCannon, healthBuildingCannon, minRange, maxRange, exploreRange, exploreTime, exploreDamage, cdTime, buildSkillData.resCost, buildSkillData.prefabName, buildSkillData.timeCost, SpriteManager.Instance.FindSpriteByName(AtlasType.IconImage, buildSkillData.iconName));
                return new BuildSkill(buildSkillData.Id, buildSkillData.name, buildSkillData.description, tempDic, BuildType.Cannon, cannon, SpriteManager.Instance.FindSpriteByName(AtlasType.IconImage, buildSkillData.iconName));
            }
            else if ((BuildType)buildSkillData.type == BuildType.Ammo)
            {
                float healthAmmo= StringToFloatArray(buildSkillData.parameter)[0];
                float healthBuildingAmmo = healthAmmo * StringToFloatArray(buildSkillData.parameter)[1];
                int timeUse = (int)(StringToFloatArray(buildSkillData.parameter)[2]);
                float useConditionValue = (StringToFloatArray(buildSkillData.parameter)[3]);
                float useEffectValue = (StringToFloatArray(buildSkillData.parameter)[4]);
                Ammo cannon = new Ammo(buildSkillData.Id, buildSkillData.name, buildSkillData.description, healthAmmo, healthBuildingAmmo, timeUse, useConditionValue, useEffectValue, buildSkillData.resCost, buildSkillData.prefabName, buildSkillData.timeCost, SpriteManager.Instance.FindSpriteByName(AtlasType.IconImage, buildSkillData.iconName));
                return new BuildSkill(buildSkillData.Id, buildSkillData.name, buildSkillData.description, tempDic, BuildType.Cannon, cannon, SpriteManager.Instance.FindSpriteByName(AtlasType.IconImage, buildSkillData.iconName));
            }
            else if ((BuildType)buildSkillData.type == BuildType.Medicine)
            {
                float healthMedicine = StringToFloatArray(buildSkillData.parameter)[0];
                float healthBuildingMedicine = healthMedicine * StringToFloatArray(buildSkillData.parameter)[1];
                int timeUse = (int)(StringToFloatArray(buildSkillData.parameter)[2]);
                float useConditionValue = (StringToFloatArray(buildSkillData.parameter)[3]);
                float useEffectValue = (StringToFloatArray(buildSkillData.parameter)[4]);
                Medicine cannon = new Medicine(buildSkillData.Id, buildSkillData.name, buildSkillData.description, healthMedicine, healthBuildingMedicine, timeUse, useConditionValue, useEffectValue, buildSkillData.resCost, buildSkillData.prefabName, buildSkillData.timeCost, SpriteManager.Instance.FindSpriteByName(AtlasType.IconImage, buildSkillData.iconName));
                return new BuildSkill(buildSkillData.Id, buildSkillData.name, buildSkillData.description, tempDic, BuildType.Cannon, cannon, SpriteManager.Instance.FindSpriteByName(AtlasType.IconImage, buildSkillData.iconName));
            }
            else
            {
                return null;
            }


        }


        public static Dictionary<SkillConditionType, float> GetConditions(string conditionString)
        {
            Dictionary<SkillConditionType, float> tempCondition = new Dictionary<SkillConditionType, float>();
            List<float[]> tempList = StringToListArray(conditionString);
            for(int i = 0; i < tempList.Count; i++)
            {
                tempCondition.Add((SkillConditionType)tempList[i][0], tempList[i][1]);
            }
            return tempCondition;

        }
        private static float[] StringToFloatArray(string stringlist)
        {
            float[] array;
            if (stringlist.Contains("]") && stringlist.Contains("["))
            {
                stringlist = stringlist.Remove(0, 1);
                stringlist = stringlist.Remove(stringlist.Length - 1, 1);
                array = stringlist.Contains(",") ? Array.ConvertAll<string, float>(stringlist.Split(new char[] { ',' }), s => float.Parse(s)) : new float[1] { float.Parse(stringlist) };
            }
            else
            {
                array = new float[1];
                array[0] = 0;
            }
            return array;
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
                array[0] = 0;
            }
            return array;
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