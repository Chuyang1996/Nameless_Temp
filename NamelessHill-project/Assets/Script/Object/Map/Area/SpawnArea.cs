using Nameless.Agent;
using Nameless.Data;
using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.DataMono
{
    public class SpawnArea : Area
    {
        public bool spawnAI = true;//待修改 后面确定阵营关系后修改
        public string pawnPath = "Prefabs/Pawn";
        public AreaAgent areaAgent;
        private FrontPlayer areaPlayer;
        public override void Init(int localId, AreaAgent areaAgent, FrontPlayer frontPlayer)
        {
            base.Init(localId,areaAgent, frontPlayer);
            this.areaPlayer = FrontManager.Instance.GenFactionPlayer(frontPlayer.faction, false, true, false, 0, frontPlayer.eventCollections);
            this.areaAgent = areaAgent;
            StartCoroutine(this.areaAgent.pawnRule.Execute(this));
        }
        public PawnAvatar GenPawn(long id)
        {

            return FrontManager.Instance.AddPawnOnArea(PawnFactory.GetPawnById(id), this, 0, this.areaPlayer);//待修改 确定了地图的配表之后
        }

    }
}