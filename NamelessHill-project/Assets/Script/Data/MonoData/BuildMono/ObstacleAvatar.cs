using Nameless.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Nameless.DataMono
{
    public class ObstacleAvatar : BuildAvatar
    {
        public Obstacle obstacle;

        public override void Init(PawnAvatar pawnAvatar, Area area, Build build, bool isBuilding)
        {
            this.obstacle = (Obstacle)build;
            base.Init(pawnAvatar, area,build, isBuilding);
            if (isBuilding)
                StartCoroutine(this.Building(pawnAvatar, this.obstacle));
        }
    }
}