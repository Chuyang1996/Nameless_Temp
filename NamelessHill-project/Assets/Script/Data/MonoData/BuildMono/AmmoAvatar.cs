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
        public Button buttonUse;
        public override void Init(PawnAvatar pawnAvatar, Area area, Build build, bool isBuilding)
        {
            Ammo ammo = (Ammo)build;
            this.ammo = new Ammo(ammo);
            this.useTime.text = this.ammo.timeUse.ToString();
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
            this.buttonUse.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayAudio(this.transform, AudioConfig.uiRemind);
                SupportPawn();
            });
        }

        public void SupportPawn()
        {
            if (this.currentPawn != null && this.ammo.timeUse > 0 && this.buildState == BuildState.Completed)
            {

                this.currentPawn.pawnAgent.AmmoChange((int)(this.ammo.effectValue * this.currentPawn.pawnAgent.pawn.maxAmmo));
                this.ammo.timeUse--;
                this.useTime.text = this.ammo.timeUse.ToString();
                if (this.ammo.timeUse == 0)
                    this.DestoryBuilding();

            }
        }
        private void Update()
        {
            if (this.currentPawn != null && FactionManager.Instance.RelationFaction(this.currentPawn.GetFaction(), this.faction) == FactionRelation.SameSide)
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