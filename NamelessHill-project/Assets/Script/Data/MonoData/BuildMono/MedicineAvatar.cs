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
        public Button buttonUse;
        public override void Init(PawnAvatar pawnAvatar, Area area, Build build, bool isBuilding)
        {
            Medicine medicine = (Medicine)build;
            this.medicine = new Medicine(medicine);
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
            this.buttonUse.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayAudio(this.transform, AudioConfig.uiRemind);
                SupportPawn();
            });
        }

        public void SupportPawn()
        {
            if (this.currentPawn != null  && this.medicine.timeUse > 0 && this.buildState == BuildState.Completed)
            {

                float healthAdd = this.medicine.effectValue * this.currentPawn.pawnAgent.pawn.maxHealth;
                this.currentPawn.pawnAgent.HealthChange(healthAdd);
                this.medicine.timeUse--;
                useTime.text = this.medicine.timeUse.ToString();
                if (this.medicine.timeUse == 0)
                    this.DestoryBuilding();

            }
        }
        private void Update()
        {
            if(this.currentPawn!=null && FactionManager.Instance.RelationFaction(this.currentPawn.GetFaction(),this.faction) == FactionRelation.SameSide)
            {
                this.buttonUse.gameObject.SetActive(true);
            }
            else
            {
                this.buttonUse.gameObject.SetActive(false);

            }
        }
    }
}