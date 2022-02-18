using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.ConfigData
{
    public class MapData 
    {
        public long id;
        public string name;
        public string descrption;
        public string mapName;
        public int passTime;
        public long nextCampId;
        public string transInfoShowName;
        public string defaultInitPos;
        public Vector2 cameraPos;
        public string nameBgm;
        public MapData(long id, string name, string descrption,string mapName, int passTime,long nextCampId, string transInfoShowName, string defaultInitPos, string cameraPos, string nameBgm)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.mapName = mapName;
            this.nextCampId = nextCampId;
            this.passTime = passTime;
            this.transInfoShowName = transInfoShowName;
            this.defaultInitPos = defaultInitPos;
            float[] pos = StringToFloatCameraPos(cameraPos);
            this.cameraPos = new Vector2(pos[0],pos[1]);
            this.nameBgm = nameBgm;
        }
        private float[] StringToFloatCameraPos(string stringlist)
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
                array = new float[2];
                array[0] = 0;
                array[1] = 0;
            }
            return array;
        }
    }
}