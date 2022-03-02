using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.ConfigData
{
    public class SupportSkillData
    {

        public long Id;
        public string name;
        public string description;
        public string tipDes;
        public string condition;
        public string buff;
        public float attackRate;
        public float defendRate;
        public string iconName;

        public SupportSkillData(long Id, string name, string description, string tipDes, string condition, string buff, float attackRate, float defendRate, string iconName )
        {
            this.Id = Id;
            this.name = name;
            this.description = description;
            this.tipDes = tipDes;
            this.condition = condition;
            this.buff = buff;
            this.attackRate = attackRate;
            this.defendRate = defendRate;
            this.iconName = iconName;
        }
    }
}