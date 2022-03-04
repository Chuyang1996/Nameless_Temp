using Nameless.Data;
using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Nameless.DataMono
{
    public class BunkerAvatar : BuildAvatar
    {
        public Bunker bunker;
        public override void Init(PawnAvatar pawnAvatar, Area area, Build build, bool isBuilding)
        {
            Bunker bunker = (Bunker)build;
            this.bunker = new Bunker(bunker);
            base.Init(pawnAvatar, area, build, isBuilding);
            if (isBuilding)
            {
                this.spriteRenderer.sprite = this.building;
                StartCoroutine(this.Building(pawnAvatar, this.bunker));
            }
            else
            {
                this.spriteRenderer.sprite = this.completed;
            }
        }

        public void DefendBunker(PawnAvatar attacker)
        {
            if(this.currentPawn!= null)
            {
                this.currentPawn.StartBattle(attacker);
            }
        }
    }
}