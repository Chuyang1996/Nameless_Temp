using Nameless.Data;
using Nameless.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nameless.DataMono
{
    public enum BuildState
    {
        Building = 0,
        Completed = 1,
        Destory = 2,

    }
    public class BuildAvatar : MonoBehaviour
    {
        public Sprite building;
        public Sprite completed;

        public SpriteRenderer spriteRenderer;
        public BuildType buildType;
        public BuildState buildState;
        public Faction faction;
        public List<PawnAvatar> pawnOpponents = new List<PawnAvatar>();

        public PawnAvatar currentPawn = null;
        public Area currentArea = null;
        public float MaxHealth { private set; get; }
        public float CurHealth
        {
            set
            {
                if (value > MaxHealth)
                    curHealth = MaxHealth;
                else if (value < 0)
                    curHealth = 0;
                else
                    curHealth = value;
                this.healthBar.value = curHealth / MaxHealth;
            }
            get
            {
                return curHealth;
            }
        }
        
        private float curHealth;


        public Slider healthBar;
        public virtual void Init(PawnAvatar pawnAvatar, Area area, Build build,bool isBuilding)
        {
            this.buildType = build.type;
            this.pawnOpponents = new List<PawnAvatar>();
            area.AddBuild(this);
            area.ResetBuildPos(this);
            this.currentArea = area;
            if (isBuilding)
            {
                this.MaxHealth = build.healthBuild;
                this.CurHealth = build.healthBuild;
                this.buildState = BuildState.Building;
                this.faction = pawnAvatar.GetFaction();
            }
            else
            {
                this.MaxHealth = build.health;
                this.CurHealth = build.health;
                this.buildState = BuildState.Completed;
                this.faction = area.playerBelong.faction;
            }

        }

        public virtual void ExecuteBuildEffect()
        {

        }
        public virtual void HealthChange(float value)
        {
            this.CurHealth += value;
        }
        public bool IsFail()
        {
            if (this.CurHealth <= 0)
            {
                return true;
            }
            return false;
        }
        public void DestoryBuilding()
        {
            Destroy(this.gameObject);
        }
        public IEnumerator Building(PawnAvatar pawnAvatar, Build build)
        {
            pawnAvatar.isBuild = true;
            float timeCount = 0;
            pawnAvatar.ocuppyBar.gameObject.SetActive(true);
            Area pawnArea = pawnAvatar.currentArea;
            while (this.buildState == BuildState.Building)
            {
                pawnAvatar.ocuppyBar.value = timeCount / build.timeCost;
                timeCount += Time.deltaTime;
                if(timeCount > build.timeCost)
                {
                    this.buildState = BuildState.Completed;
                }
                else if(pawnAvatar== null || pawnArea != pawnAvatar.currentArea)
                {
                    this.buildState = BuildState.Destory;
                }
                yield return null;
            }
            pawnAvatar.isBuild = false;
            pawnAvatar.ocuppyBar.gameObject.SetActive(false);
            if (this.buildState == BuildState.Completed)
            {
                this.MaxHealth = build.health;
                this.CurHealth = build.health;
                if(this.spriteRenderer!=null)
                    this.spriteRenderer.sprite = this.completed;
            }
            else
            {
                this.DestoryBuilding();
            }
            
        }
        public void UpdateCurrentPawn(PawnAvatar pawnAvatar)
        {
           this.currentPawn = pawnAvatar;
        }
        public bool CheckIfBattle(PawnAvatar pawnAvatar)
        {
            if (FactionManager.Instance.RelationFaction(this.faction, pawnAvatar.GetFaction()) == FactionRelation.Hostility || this is ObstacleAvatar)
                return true;
            return false;
        }
        public virtual void CheckResult()
        {
            this.DestoryBuilding();
        }
    }
}