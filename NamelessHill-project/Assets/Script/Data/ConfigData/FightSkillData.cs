using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.ConfigData
{
    public class FightSkillData
    {
        public long Id;
        public string name;
        public string description;
        public string condition;
        public float attackRate;
        public float defendRate;
        public string parameter;
        public string iconName;
        public FightSkillData(
            long Id,
            string name,
            string description,
            string condition,
            float attackRate,
            float defendRate,
            string parameter,
            string iconName
            )
        {
            this.Id = Id;
            this.name = name;
            this.description = description;
            this.condition = condition;
            this.attackRate = attackRate;
            this.defendRate = defendRate;
            this.parameter = parameter;
            this.iconName = iconName;
        }
    }
}
