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
        public float costMedicineRate;
        public float costAmmoRate;

        public BuildSkillData(
            long Id,
            string name,
            string description,
            string condition,
            float costMedicineRate,
            float costAmmoRate
            )
        {
            this.Id = Id;
            this.name = name;
            this.description = description;
            this.condition = condition;
            this.costMedicineRate = costMedicineRate;
            this.costAmmoRate = costAmmoRate;
        }
    }
}