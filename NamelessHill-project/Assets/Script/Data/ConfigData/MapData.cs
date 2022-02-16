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
        public string nameBgm;
        public MapData(long id, string name, string descrption,string mapName, int passTime,long nextCampId, string transInfoShowName, string defaultInitPos, string nameBgm)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.mapName = mapName;
            this.nextCampId = nextCampId;
            this.passTime = passTime;
            this.transInfoShowName = transInfoShowName;
            this.defaultInitPos = defaultInitPos;
            this.nameBgm = nameBgm;
        }
    }
}