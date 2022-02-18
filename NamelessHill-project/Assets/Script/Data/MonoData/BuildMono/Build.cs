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
        Obstacle = 1,//ÕÏ°­
        Bunker = 2,//ÑÚÌå
        Cannon = 3,//ÆÈ»÷ÅÚ
        Ammo = 4,//µ¯Ò©
        Medicine = 5,//Ò©Æ·
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

        public Obstacle(long id, string name, string description, float health , float healthBuild, int resCost,string prefabName,  float timeCost,Sprite sprite)
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
        public override void ExecuteBuild(PawnAvatar pawnAvatar, Area area)
        {

        }
    }
    public class Medicine : Build
    {
        public int timeUse;
        public float useConditionValue;
        public float effectValue;
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