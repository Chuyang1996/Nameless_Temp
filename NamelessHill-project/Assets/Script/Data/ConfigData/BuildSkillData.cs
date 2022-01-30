using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.ConfigData
{
    public class BuildSkillData
    {
        public long Id;
        public string name;
        public string description;
        public string condition;
        public int resCost;
        public float timeCost;
        public int type;
        public string parameter;
        public string prefabName;
        public string iconName;
        public BuildSkillData(
            long Id,
            string name,
            string description,
            string condition,
            int resCost,
            float timeCost,
            int type,
            string parameter, 
            string prefabName, 
            string iconName
            )
        {
            this.Id = Id;
            this.name = name;
            this.description = description;
            this.condition = condition;
            this.resCost = resCost;
            this.timeCost = timeCost;
            this.type = type;
            this.parameter = parameter;
            this.prefabName = prefabName;
            this.iconName = iconName;
        }
    }
}