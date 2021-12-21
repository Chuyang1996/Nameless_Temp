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
        public bool spawnAI = true;//���޸� ����ȷ����Ӫ��ϵ���޸�
        public string pawnPath = "Prefabs/Pawn";
        public AreaAgent areaAgent;
        private FrontPlayer areaPlayer;
        public override void Init(int localId, AreaAgent areaAgent, FrontPlayer frontPlayer,long factionId)
        {
            base.Init(localId,areaAgent, frontPlayer, factionId);
            this.areaPlayer = FrontManager.Instance.GenFactionPlayer(FactionManager.Instance.GetFaction(factionId), false, true, false, 0);
            this.areaAgent = areaAgent;
            StartCoroutine(this.areaAgent.pawnRule.Execute(this));
        }
        public PawnAvatar GenPawn(long id)
        {

            return FrontManager.Instance.AddPawnOnArea(PawnFactory.GetPawnById(id), this, 0, this.areaPlayer);//���޸� ȷ���˵�ͼ�����֮��
        }

    }
}