using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.ConfigData
{
    public class FactionData
    {
        public long id;
        public string name;
        public string txt;
        public string friendly_to;
        public string hostile_to;
        public string healthColor;
        public string pathColor;
        public string walkColor;
        public string supportColor;
        public string areaColor;
        public string battleColor;
        public string battleIcon;

        public FactionData(long id, string name,string txt, string friendly_to,string hostile_to,string healthColor, string pathColor, string walkColor, string supportColor, string areaColor, string battleColor, string battleIcon)
        {
            this.id = id;
            this.name = name;
            this.txt = txt;
            this.friendly_to = friendly_to;
            this.hostile_to = hostile_to;
            this.healthColor = healthColor;
            this.pathColor = pathColor;
            this.walkColor = walkColor;
            this.supportColor = supportColor;
            this.areaColor = areaColor;
            this.battleColor = battleColor;
            this.battleIcon = battleIcon;
        }
    }
}