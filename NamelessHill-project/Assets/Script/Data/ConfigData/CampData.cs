using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.ConfigData
{
    public class CampData
    {
        public long id;
        public string name;
        public string descrption;
        public string campName;
        public long nextBattleId;
        public string nameBgm;
        public CampData(long id, string name, string descrption, string campName, long nextBattleId,  string nameBgm)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.campName = campName;
            this.nextBattleId = nextBattleId;
            this.nameBgm = nameBgm;
        }
    }
}