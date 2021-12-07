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
        public long nextId;
        public string transInfoShowName;
        public string nameBgm;
        public MapData(long id, string name, string descrption,string mapName,long nextId, string transInfoShowName, string nameBgm)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.mapName = mapName;
            this.nextId = nextId;
            this.transInfoShowName = transInfoShowName;
            this.nameBgm = nameBgm;
        }
    }
}