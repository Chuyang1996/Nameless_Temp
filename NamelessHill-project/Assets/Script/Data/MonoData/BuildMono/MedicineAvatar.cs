using Nameless.Data;
using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nameless.DataMono
{
    public class MedicineAvatar : BuildAvatar
    {
        public Medicine medicine;
        public Text useTime;
        public override void Init(PawnAvatar pawnAvatar, Area area, Build build, bool isBuilding)
        {
            this.medicine = (Medicine)build;
            base.Init(pawnAvatar, area, build, isBuilding);
            if (isBuilding)
            {
                this.spriteRenderer.sprite = this.building;
                StartCoroutine(this.Building(pawnAvatar, this.medicine));
            }
            else
            {
                this.spriteRenderer.sprite = this.completed;
            }
        }

        public void SupportPawn()
        {
            if (this.currentPawn != null  && this.medicine.timeUse > 0 && this.buildState == BuildState.Completed)
            {
                if ((this.currentPawn.pawnAgent.pawn.curHealth / this.currentPawn.pawnAgent.pawn.maxHealth) < this.medicine.useConditionValue)
                {
                    float healthAdd = this.medicine.effectValue * this.currentPawn.pawnAgent.pawn.maxHealth;
                    this.currentPawn.pawnAgent.HealthChange(healthAdd);
                    this.medicine.timeUse--;
                    useTime.text = this.medicine.timeUse.ToString();
                }
            }
        }
        private void Update()
        {
            if(this.currentPawn!=null && FactionManager.Instance.RelationFaction(this.currentPawn.GetFaction(),this.faction) == FactionRelation.SameSide)
            {
                this.SupportPawn();
            }
        }
    }
}