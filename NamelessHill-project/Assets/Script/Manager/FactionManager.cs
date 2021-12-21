using Nameless.ConfigData;
using Nameless.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Nameless.Manager
{
    public class Faction
    {
        public long id;
        public Color healthColor;
        public Color pathColor;
        public Color walkColor;
        public Color supportColor;
        public Color areaColor;
        public Color battleColor;
        public string label;

        public Faction(long id, Color healthColor, Color pathColor, Color walkColor, Color supportColor, Color areaColor, Color battleColor, string label)
        {
            this.id = id;
            this.healthColor = healthColor;
            this.pathColor = pathColor;
            this.walkColor = walkColor;
            this.supportColor = supportColor;
            this.areaColor = areaColor;
            this.battleColor = battleColor;
            this.label = label;
        }
    }
    public enum FactionRelation
    {
        SameSide = 0,
        Friend = 1,
        Hostility = 2,
        Error = 3,
    }
    public class FactionManager : Singleton<FactionManager>
    {
        List<Faction> factions = new List<Faction>();
        FactionRelation[][] relations;

        public void InitFaction()
        {
            List<FactionData> factionDatas = DataManager.Instance.GetFactions();
            for(int i = 0;i < factionDatas.Count; i++)
            {
                List<float> healthRGBA = this.StringToFloatArray(factionDatas[i].healthColor);
                List<float> pathRGBA = this.StringToFloatArray(factionDatas[i].pathColor);
                List<float> walkRGBA = this.StringToFloatArray(factionDatas[i].walkColor);
                List<float> supportRGBA = this.StringToFloatArray(factionDatas[i].supportColor);
                List<float> areaRGBA = this.StringToFloatArray(factionDatas[i].areaColor);
                List<float> battleRGBA = this.StringToFloatArray(factionDatas[i].battleColor);
                Color healthColor = new Color(healthRGBA[0], healthRGBA[1], healthRGBA[2], healthRGBA[3]);
                Color pathColor = new Color(pathRGBA[0], pathRGBA[1], pathRGBA[2], pathRGBA[3]);
                Color walkColor = new Color(walkRGBA[0], walkRGBA[1], walkRGBA[2], walkRGBA[3]);
                Color supportColor = new Color(supportRGBA[0], supportRGBA[1], supportRGBA[2], supportRGBA[3]);
                Color areaColor = new Color(areaRGBA[0], areaRGBA[1], areaRGBA[2], areaRGBA[3]);
                Color battleColor = new Color(battleRGBA[0], battleRGBA[1], battleRGBA[2], battleRGBA[3]);
                this.factions.Add(new Faction(factionDatas[i].id, healthColor, pathColor, walkColor, supportColor, areaColor, battleColor, factionDatas[i].name));
            }
            this.relations = new FactionRelation[this.factions.Count][];
            for (int i = 0; i < this.relations.GetLength(0); i++)
            {
                List<long> friends = StringToLongArray(factionDatas[i].friendly_to);
                List<long> hostiles = StringToLongArray(factionDatas[i].hostile_to);

                var tempFactions = new FactionRelation[this.factions.Count];
                for (int j = 0; j < tempFactions.Length; j++) 
                {
                    if(i == j)
                        tempFactions[j] = FactionRelation.SameSide;
                    else if (friends.Contains(this.factions[j].id))
                        tempFactions[j] = FactionRelation.Friend;
                    else if (hostiles.Contains(this.factions[j].id))
                        tempFactions[j] = FactionRelation.Hostility;
                    else
                        tempFactions[j] = FactionRelation.Hostility;
                }
                this.relations[i] = tempFactions;
            }
        }

        public bool IsSameSide(Faction a, Faction b)
        {
            return this.RelationFaction(a,b) == FactionRelation.SameSide;
        }

        public FactionRelation RelationFaction(Faction a, Faction b)
        {
            if (a == null || b == null)
                return FactionRelation.Hostility;

            int indexA = -1;
            int indexB = -1;
            for(int i = 0; i < this.factions.Count; i++)
            {
                if (this.factions[i].id == a.id) indexA = i;
                if (this.factions[i].id == b.id) indexB = i;
            }
            if (indexB == -1 || indexA == -1) 
            {
                Debug.LogError("阵营不存在，请检查阵营填表是否更新导入，或检查相关逻辑是否正确");
                return FactionRelation.Error; 
            }
            return this.relations[indexA][indexB];
        }

        public Faction GetFaction(long id)
        {
            Faction faction = this.factions.Where(_faction => _faction.id == id).FirstOrDefault();
            return faction;
        }


        private List<float> StringToFloatArray(string stringlist)
        {
            float[] array;
            List<float> result = new List<float>();
            if (stringlist.Contains("]") && stringlist.Contains("["))
            {
                stringlist = stringlist.Remove(0, 1);
                stringlist = stringlist.Remove(stringlist.Length - 1, 1);
                array = stringlist.Contains(",") ? Array.ConvertAll<string, float>(stringlist.Split(new char[] { ',' }), s => float.Parse(s)) : new float[1] { float.Parse(stringlist) };
                for (int i = 0; i < array.Length; i++)
                {
                    result.Add(array[i]);
                }
            }
            return result;
        }
        private List<long> StringToLongArray(string stringlist)
        {
            long[] array;
            List<long> result = new List<long>();
            if (stringlist.Contains("]") && stringlist.Contains("["))
            {
                stringlist = stringlist.Remove(0, 1);
                stringlist = stringlist.Remove(stringlist.Length - 1, 1);
                array = stringlist.Contains(",") ? Array.ConvertAll<string, long>(stringlist.Split(new char[] { ',' }), s => long.Parse(s)) : new long[1] { long.Parse(stringlist) };
                for(int i = 0; i < array.Length; i++)
                {
                    result.Add(array[i]);
                }
            }
            return result;
        }
    }
}