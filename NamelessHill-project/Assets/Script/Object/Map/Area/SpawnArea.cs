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

        public override void Init(int id, AreaAgent areaAgent)
        {
            base.Init(id,areaAgent);
            this.areaAgent = areaAgent;
            StartCoroutine(this.areaAgent.pawnRule.Execute(this));
        }
        public PawnAvatar GenPawn(long id)
        {
            return PawnManager.Instance.GeneratePawn(id, this, spawnAI, 0);//待修改 确定了地图的配表之后
            //GameObject pawnAvatar = Instantiate(Resources.Load(pawnPath)) as GameObject;
            //pawnAvatar.GetComponent<PawnAvatar>().currentArea = this;
            //pawnAvatar.GetComponent<PawnAvatar>().Id = id;
            //pawnAvatar.gameObject.transform.position = this.centerNode.transform.position;
            //if (spawnAI)
            //{
            //    pawnAvatar.GetComponent<PawnAvatar>().isAI = true;
            //    pawnAvatar.GetComponent<PawnAvatar>().nameTxt.color = Color.red;
            //    pawnAvatar.GetComponent<PawnAvatar>().healthBarColor.color = Color.red;
            //}
            //this.AddPawn(pawnAvatar.GetComponent<PawnAvatar>());
            //pawnAvatar.GetComponent<PawnAvatar>().Init(0);
            //DialogueTriggerManager.Instance.TimeTriggerEvent += pawnAvatar.GetComponent<PawnAvatar>().ReceiveCurrentTime;
            //GameManager.Instance.enemyPawns.Add(pawnAvatar.GetComponent<PawnAvatar>());
            //return pawnAvatar.GetComponent<PawnAvatar>();
        }

    }
}