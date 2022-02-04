using Nameless.Data;
using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Nameless.DataMono
{
    public class CannonAvatar : BuildAvatar
    {
        public Cannon cannon;
        public Animator animator;
        public CannonAnim cannonAnimEvent;

        private Dictionary<Area, List<Area>> exploreAreas = new Dictionary<Area, List<Area>>();

        private bool isAreaConfirm = false;

        private float cdCountTime = 0.0f;

        private CannonExploreBullsEye exploreBullsEye = null;

        private Area targetArea;
        private List<Area> targetExplore;

        

        public override void Init(PawnAvatar pawnAvatar, Area area, Build build, bool isBuilding)
        {
            this.cannon = (Cannon)build;
            base.Init(pawnAvatar, area, build, isBuilding);
            if (isBuilding)
                StartCoroutine(this.Building(pawnAvatar, this.cannon));
            this.isAreaConfirm = false;
            this.exploreAreas = new Dictionary<Area, List<Area>>();
            if (this.buildState == BuildState.Completed)
            {
                List<Area> areas = MapManager.Instance.currentMap.areas;
                for (int i = 0; i < areas.Count; i++)
                {
                    float distanceArea = Vector2.Distance(areas[i].centerNode.transform.position, this.currentArea.centerNode.transform.position);
                    if (this.cannon.minRange < distanceArea && distanceArea < this.cannon.maxRange)
                    {
                        List<Area> areaRange = new List<Area>();
                        for (int j = 0; j < areas.Count; j++)
                        {
                            float distanceExploreArea = Vector2.Distance(areas[j].centerNode.transform.position, areas[i].centerNode.transform.position);
                            if (distanceExploreArea < this.cannon.exploreRange)
                                areaRange.Add(areas[j]);
                        }
                        this.exploreAreas.Add(areas[i],areaRange);
                    }
                }
            }
            this.isAreaConfirm = true;
            this.cdCountTime = 0.0f;
            this.targetArea = null;
            this.targetExplore = new List<Area>();
        }

        private void Update()
        {
            if(this.isAreaConfirm && this.buildState == BuildState.Completed)
            {
                this.CdCountTime();
            }
        }

        private void CdCountTime()
        {
            if(this.cdCountTime > this.cannon.cdTime)
            {
                this.LookForTarget();
            }
            else
            {
                this.cdCountTime += Time.deltaTime;
            }
        }

        private void LookForTarget()
        {
            if (this.targetArea == null)
            {
                foreach (var child in this.exploreAreas)
                {
                    if (child.Key.pawns.Count > 0 && FactionManager.Instance.RelationFaction(child.Key.pawns[0].GetFaction(), this.faction) == FactionRelation.Hostility && child.Key.buildAvatar != null)
                    {
                        this.InitTarget(child.Key, child.Value);
                        this.animator.SetTrigger("Fire");
                        break;
                    }
                    else if (child.Key.pawns.Count > 0 && FactionManager.Instance.RelationFaction(child.Key.pawns[0].GetFaction(), this.faction) == FactionRelation.Hostility)
                    {
                        this.InitTarget(child.Key, child.Value);
                        this.animator.SetTrigger("Fire");
                        break;
                    }
                    else if (child.Key.buildAvatar != null && child.Key.buildAvatar is BunkerAvatar && FactionManager.Instance.RelationFaction(child.Key.buildAvatar.faction, this.faction) == FactionRelation.Hostility)
                    {
                        this.InitTarget(child.Key, child.Value);
                        this.animator.SetTrigger("Fire");
                        break;
                    }
                    else if (child.Key.buildAvatar != null && child.Key.buildAvatar is ObstacleAvatar && FactionManager.Instance.RelationFaction(child.Key.buildAvatar.faction, this.faction) == FactionRelation.Hostility)
                    {
                        this.InitTarget(child.Key, child.Value);
                        this.animator.SetTrigger("Fire");
                        break;
                    }
                }
            }
        }

        private void InitTarget(Area target, List<Area> targetExplore)
        {
            this.targetArea = target;
            this.targetExplore = targetExplore;
        }

        public void GenTarget()
        {
            this.exploreBullsEye = StaticObjGenManager.Instance.GenerateBuildIcon(this.targetArea, BuildIconType.BullEyes).GetComponent<CannonExploreBullsEye>();
            this.exploreBullsEye.GetComponent<CannonExploreBullsEye>().Init(this.targetArea, this.targetExplore, this);
        }

        public void ResetState()
        {
            this.cdCountTime = 0.0f;
            this.targetArea = null;
            this.targetExplore = new List<Area>();
            this.exploreBullsEye = null;

        }

        public override void CheckResult()
        {
            animator.SetTrigger("Death");
        }
    }
}