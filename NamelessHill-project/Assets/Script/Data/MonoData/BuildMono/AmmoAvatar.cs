using Nameless.Data;
using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nameless.DataMono
{
    public class AmmoAvatar : BuildAvatar
    {
        public Ammo ammo;
        public Text useTime;
        public override void Init(PawnAvatar pawnAvatar, Area area, Build build, bool isBuilding)
        {
            this.ammo = (Ammo)build;
            base.Init(pawnAvatar, area, build, isBuilding);
            if (isBuilding)
            {
                this.spriteRenderer.sprite = this.building;
                StartCoroutine(this.Building(pawnAvatar, this.ammo));
            }
            else
            {
                this.spriteRenderer.sprite = this.completed;
            }
        }

        public void SupportPawn()
        {
            if (this.currentPawn != null && this.ammo.timeUse > 0 && this.buildState == BuildState.Completed)
            {
              if((this.currentPawn.pawnAgent.pawn.curAmmo / this.currentPawn.pawnAgent.pawn.maxAmmo) < this.ammo.useConditionValue)
                {
                    this.currentPawn.pawnAgent.AmmoChange((int)(this.ammo.effectValue * this.currentPawn.pawnAgent.pawn.maxAmmo));
                    this.ammo.timeUse--;
                    this.useTime.text = this.ammo.timeUse.ToString();
                                    }
            }
        }
        private void Update()
        {
            if (this.currentPawn != null && FactionManager.Instance.RelationFaction(this.currentPawn.GetFaction(), this.faction) == FactionRelation.SameSide)
            {
                this.SupportPawn();
            }
        }
    }
}