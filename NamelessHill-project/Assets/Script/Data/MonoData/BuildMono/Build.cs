using Nameless.DataMono;
using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nameless.Data
{
    public enum BuildType
    {
        //AmmoBuild = 0,
        //MeidicalBuild = 1,
        None = 0,
        Obstacle = 1,//?ϰ?
        Bunker = 2,//????
        Cannon = 3,//?Ȼ???
        Ammo = 4,//??ҩ
        Medicine = 5,//ҩƷ
    }
    abstract public class Build
    {
        public long Id;
        public string name;
        public string description;
        public BuildType type;
        public float health;
        public float healthBuild;
        public int resCost;
        public float timeCost;
        public string prefabName;
        public Sprite sprite;
        abstract public void ExecuteBuild(PawnAvatar pawnAvatar,Area area);
        // Start is called before the first frame update

    }

    public class Obstacle : Build
    {

        public Obstacle(Obstacle obstacle)
        {
            this.Id = obstacle.Id;
            this.name = obstacle.name;
            this.description = obstacle.description;
            this.health = obstacle.health;
            this.healthBuild = obstacle.healthBuild;
            this.resCost = obstacle.resCost;
            this.timeCost = obstacle.timeCost;
            this.prefabName = obstacle.prefabName;
            this.type = BuildType.Obstacle;
            this.sprite = obstacle.sprite;
        }
        public Obstacle(long id, string name, string description, float health, float healthBuild, int resCost, string prefabName, float timeCost, Sprite sprite)
        {
            this.Id = id;
            this.name = name;
            this.description = description;
            this.health = health;
            this.healthBuild = healthBuild;
            this.resCost = resCost;
            this.timeCost = timeCost;
            this.prefabName = prefabName;
            this.type = BuildType.Obstacle;
            this.sprite = sprite;
        }
        public override void ExecuteBuild(PawnAvatar pawnAvatar, Area area)
        {
            StaticObjGenManager.Instance.GenerateBuild(pawnAvatar, area, this,true);
        }
    }
    public class Bunker : Build
    {

        public Bunker(Bunker bunker)
        {
            this.Id = bunker.Id;
            this.name = bunker.name;
            this.description = bunker.description;
            this.health = bunker.health;
            this.healthBuild = bunker.healthBuild;
            this.resCost = bunker.resCost;
            this.timeCost = bunker.timeCost; 
            this.prefabName = bunker.prefabName;
            this.type = BuildType.Bunker;
            this.sprite = bunker.sprite;
        }
        public Bunker(long id, string name, string description, float health, float healthBuild, int resCost, string prefabName, float timeCost, Sprite sprite)
        {
            this.Id = id;
            this.name = name;
            this.description = description;
            this.health = health;
            this.healthBuild = healthBuild;
            this.resCost = resCost;
            this.timeCost = timeCost;
            this.prefabName = prefabName;
            this.type = BuildType.Bunker;
            this.sprite = sprite;
        }
        public override void ExecuteBuild(PawnAvatar pawnAvatar, Area area)
        {
            StaticObjGenManager.Instance.GenerateBuild(pawnAvatar, area, this, true);
        }
    }
    public class Cannon : Build
    {

        public int minRange;
        public int maxRange;
        public int exploreRange;
        public float expolreTime;
        public float exploreDamage;
        public float cdTime;

        public Cannon(long id, string name, string description, float health, float healthBuild, int minRange,int maxRange,int exploreRange,float expolreTime, float exploreDamage,float cdTime, int resCost, string prefabName, float timeCost, Sprite sprite)
        {
            this.Id = id;
            this.name = name;
            this.description = description;
            this.health = health;
            this.healthBuild = healthBuild;
            this.resCost = resCost;
            this.timeCost = timeCost;
            this.minRange = minRange;
            this.maxRange = maxRange;
            this.exploreRange = exploreRange;
            this.expolreTime = expolreTime;
            this.exploreDamage = exploreDamage;
            this.cdTime = cdTime;
            this.prefabName = prefabName;
            this.type = BuildType.Cannon;
            this.sprite = sprite;
        }
        public Cannon(Cannon cannon)
        {
            this.Id = cannon.Id;
            this.name = cannon.name;
            this.description = cannon.description;
            this.health = cannon.health;
            this.healthBuild = cannon.healthBuild;
            this.resCost = cannon.resCost;
            this.timeCost = cannon.timeCost;
            this.minRange = cannon.minRange;
            this.maxRange = cannon.maxRange;
            this.exploreRange = cannon.exploreRange;
            this.expolreTime = cannon.expolreTime;
            this.exploreDamage = cannon.exploreDamage;
            this.cdTime = cannon.cdTime;
            this.prefabName = cannon.prefabName;
            this.type = BuildType.Cannon;
            this.sprite = cannon.sprite;
        }
        public override void ExecuteBuild(PawnAvatar pawnAvatar, Area area)
        {
            StaticObjGenManager.Instance.GenerateBuild(pawnAvatar, area, this, true);
        }
    }

    public class Ammo : Build
    {
        public int timeUse;
        public float useConditionValue;
        public float effectValue;
        public Ammo(long id, string name, string description, float health, float healthBuild, int timeUse, float useConditionValue, float effectValue, int resCost, string prefabName, float timeCost, Sprite sprite)
        {
            this.Id = id;
            this.name = name;
            this.description = description;
            this.health = health;
            this.healthBuild = healthBuild;
            this.resCost = resCost;
            this.timeUse = timeUse;
            this.useConditionValue = useConditionValue;
            this.effectValue = effectValue;
            this.prefabName = prefabName;
            this.timeCost = timeCost;
            this.type = BuildType.Ammo;
            this.sprite = sprite;
        }
        public Ammo(Ammo ammo)
        {
            this.Id = ammo.Id;
            this.name = ammo.name;
            this.description = ammo.description;
            this.health = ammo.health;
            this.healthBuild = ammo.healthBuild;
            this.resCost = ammo.resCost;
            this.timeUse = ammo.timeUse;
            this.useConditionValue = ammo.useConditionValue;
            this.effectValue = ammo.effectValue;
            this.prefabName = ammo.prefabName;
            this.timeCost = ammo.timeCost;
            this.type = BuildType.Ammo;
            this.sprite = ammo.sprite;
        }
        public override void ExecuteBuild(PawnAvatar pawnAvatar, Area area)
        {

        }
    }
    public class Medicine : Build
    {
        public int timeUse;
        public float useConditionValue;
        public float effectValue;
        public Medicine(Medicine medicine)
        {
            this.Id = medicine.Id;
            this.name = medicine.name;
            this.description = medicine.description;
            this.health = medicine.health;
            this.healthBuild = medicine.healthBuild;
            this.resCost = medicine.resCost;
            this.timeUse = medicine.timeUse;
            this.useConditionValue = medicine.useConditionValue;
            this.effectValue = medicine.effectValue;
            this.prefabName = medicine.prefabName;
            this.timeCost = medicine.timeCost;
            this.type = BuildType.Medicine;
            this.sprite = medicine.sprite;
        }
        public Medicine(long id, string name, string description, float health, float healthBuild, int timeUse, float useConditionValue, float effectValue, int resCost, string prefabName, float timeCost, Sprite sprite)
        {
            this.Id = id;
            this.name = name;
            this.description = description;
            this.health = health;
            this.healthBuild = healthBuild;
            this.resCost = resCost;
            this.timeUse = timeUse;
            this.useConditionValue = useConditionValue;
            this.effectValue = effectValue;
            this.prefabName = prefabName;
            this.timeCost = timeCost;
            this.type = BuildType.Medicine;
            this.sprite = sprite;
        }
        public override void ExecuteBuild(PawnAvatar pawnAvatar, Area area)
        {

        }
    }
}