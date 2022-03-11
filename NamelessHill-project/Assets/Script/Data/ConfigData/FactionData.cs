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
        public string areaColor;
        public string battleColor;
        public int pathMaterialIndex;
        public string battleIcon;

        public FactionData(long id, string name,string txt, string friendly_to,string hostile_to,string healthColor, string areaColor, string battleColor, int pathMaterialIndex, string battleIcon)
        {
            this.id = id;
            this.name = name;
            this.txt = txt;
            this.friendly_to = friendly_to;
            this.hostile_to = hostile_to;
            this.healthColor = healthColor;

            this.areaColor = areaColor;
            this.battleColor = battleColor;
            this.pathMaterialIndex = pathMaterialIndex;
            this.battleIcon = battleIcon;
        }
    }
}