using Nameless.DataMono;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Nameless.Manager
{
    public class PawnManager : SingletonMono<PawnManager>
    {
        private string pawnPath = "Prefabs/Pawn";
        private List<PawnAvatar> playerPawns = new List<PawnAvatar>();//待修改 所有玩家角色
        private List<PawnAvatar> enemyPawns = new List<PawnAvatar>();//待修改 所有敌方角色

        public void InitPawns()
        {
            this.playerPawns = new List<PawnAvatar>();//待修改 所有玩家角色
            this.enemyPawns = new List<PawnAvatar>();//待修改 所有敌方角色
        }
        public void AddPawnForFaction(PawnAvatar pawnAvatar, bool isAi)
        {
            if (isAi)
            {
                if (!this.enemyPawns.Contains(pawnAvatar))
                    this.enemyPawns.Add(pawnAvatar);
            }
            else
            {
                if (!this.playerPawns.Contains(pawnAvatar))
                    this.playerPawns.Add(pawnAvatar);
            }
        }

        public void RemovePawn(PawnAvatar pawnAvatar)
        {

            if (this.enemyPawns.Contains(pawnAvatar))
                this.enemyPawns.Remove(pawnAvatar);
            else if (this.playerPawns.Contains(pawnAvatar))
                this.playerPawns.Remove(pawnAvatar);

        }
        public PawnAvatar GetPawnAvatarById(long id, bool isAi)//待修改 阵营确定之后
        {
            PawnAvatar pawnAvatar = isAi?this.enemyPawns.Where(_pawn => _pawn.pawnAgent.pawn.id == id).FirstOrDefault() : this.playerPawns.Where(_pawn => _pawn.pawnAgent.pawn.id == id).FirstOrDefault();
            return pawnAvatar;
        }
        public List<PawnAvatar> GetPawnAvatars(bool isAi)
        {
            return isAi ? this.enemyPawns : this.playerPawns;
        }

        public PawnAvatar AddPawnOnArea(long id, Area area, int mapId,bool spawnAI = true)//待修改 阵营确定之后
        {
            if (area == null)
                return null;
            GameObject pawnAvatar = Instantiate(Resources.Load(pawnPath)) as GameObject;
            pawnAvatar.GetComponent<PawnAvatar>().Id = id;
            pawnAvatar.GetComponent<PawnAvatar>().isAI = spawnAI;
            pawnAvatar.GetComponent<PawnAvatar>().Init(mapId, area);
            DialogueTriggerManager.Instance.TimeTriggerEvent += pawnAvatar.GetComponent<PawnAvatar>().ReceiveCurrentTime;

            if (spawnAI)
                this.enemyPawns.Add(pawnAvatar.GetComponent<PawnAvatar>());
            else
            {
                this.playerPawns.Add(pawnAvatar.GetComponent<PawnAvatar>());
            }
            pawnAvatar.gameObject.transform.parent = this.gameObject.transform;
            return pawnAvatar.GetComponent<PawnAvatar>();
        }

        public void ClearAllPawn()
        {
            for (int i = 0; i < this.playerPawns.Count; i++)
            {
                Destroy(this.playerPawns[i].gameObject);
            }
            this.playerPawns.Clear();

            for (int i = 0; i < this.enemyPawns.Count; i++)
            {
                Destroy(this.enemyPawns[i].gameObject);
            }
            this.enemyPawns.Clear();
        }
    }
}