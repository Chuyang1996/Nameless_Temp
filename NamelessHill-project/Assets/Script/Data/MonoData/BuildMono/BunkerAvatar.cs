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
        public override void Init(PawnAvatar pawnAvatar,Area area, Build build, bool isBuilding)
        {
            this.bunker = (Bunker)build;
            base.Init(pawnAvatar, area, build, isBuilding);
            if(isBuilding)
                StartCoroutine(this.Building(pawnAvatar, this.bunker));
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