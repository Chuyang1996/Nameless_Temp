using Nameless.Data;
using Nameless.DataMono;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Nameless.Manager
{

    public class FrontPlayer
    {
        public Faction faction { get; private set; }

        public EventCollections eventCollections;

        private bool isLocalPlayer;
        private bool isBot;
        private List<PawnAvatar> pawnAvatars = new List<PawnAvatar>();
        private List<Area> occupyAreas = new List<Area>();


        public Action<int,int> TotalMilitartEvent;
        private int totalMilitaryRes;
        private int enemiesDieNum = 0;
        public FrontPlayer(Faction faction, bool isLocalPlayer, bool isBot, int totalMilitaryRes, EventCollections eventCollections)
        {
            this.faction = faction;
            this.isLocalPlayer = isLocalPlayer;
            this.isBot = isBot;
            this.pawnAvatars = new List<PawnAvatar>();
            this.eventCollections = eventCollections;
            this.totalMilitaryRes = totalMilitaryRes;
            this.enemiesDieNum = 0;
        }
        public Player GetPlayer()
        {
            return new Player(this.GetPawns(), this.totalMilitaryRes, this.eventCollections);
        }
        public bool IsLocalPlayer()
        {
            return this.isLocalPlayer;
        }
        public bool IsBot()
        {
            return this.isBot;
        }
        public bool CanControl()
        {
            if (!this.isBot)
            {
                if (this.isLocalPlayer)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        public List<PawnAvatar> GetPawnAvatars()
        {
            return pawnAvatars;
        }
        public void AddPawnAvatar(PawnAvatar pawnAvatar)
        {
            if (!this.pawnAvatars.Contains(pawnAvatar) && pawnAvatar != null)
                this.pawnAvatars.Add(pawnAvatar);
        }
        public List<Pawn> GetPawns()
        {
            List<Pawn> pawns = new List<Pawn>();
            for (int i = 0; i < this.GetPawnAvatars().Count; i++)
            {
                pawns.Add(this.GetPawnAvatars()[i].pawnAgent.pawn);
            }
            return pawns;
        }
        public void RemovePawnAvatar(PawnAvatar pawnAvatar)
        {
            if (this.pawnAvatars.Contains(pawnAvatar) && pawnAvatar != null)
                this.pawnAvatars.Remove(pawnAvatar);
        }
        public bool ContainArea(Area area)
        {
            return this.occupyAreas.Contains(area);
        }
        public void AddArea(Area area)
        {
            if (!this.occupyAreas.Contains(area))
                this.occupyAreas.Add(area);
        }
        public void RemoveArea(Area area)
        {
            if (this.occupyAreas.Contains(area))
                this.occupyAreas.Remove(area);
        }
        public void ClearPlayer()
        {
            this.pawnAvatars.Clear();
            this.occupyAreas.Clear();
        }

        public void ChangeMilitaryRes(int cost)
        {
            EventTriggerManager.Instance.CheckRelateMilitaryResEvent(cost,this);
            this.totalMilitaryRes += cost;
            if (this.TotalMilitartEvent != null)
                this.TotalMilitartEvent(this.totalMilitaryRes, cost);
            //this.battleView.resourceInfoView.Init(this.totalMilitaryRes);
        }

        public int GetMilitaryRes()
        {
            return this.totalMilitaryRes;
        }

        public void EnemiesKillNum(int num)//?????? ????????????????
        {
            EventTriggerManager.Instance.CheckRelateEnemyKillEvent(num,this);
            this.enemiesDieNum += num;
        }
        public int GetEnemiesDieNum()
        {
            return this.enemiesDieNum;
        }
    }
    public class FrontManager : SingletonMono<FrontManager>
    {
        private string pawnPath = "Prefabs/Pawn";
        public FrontPlayer localPlayer = null;
        private List<FrontPlayer> frontPlayersDic = new List<FrontPlayer>();


        public void InitFront()
        {
            this.frontPlayersDic = new List<FrontPlayer>();
            this.localPlayer = null;

        }

        public FrontPlayer GetFrontPlayer(long factionId)
        {
            FrontPlayer frontPlayer = this.frontPlayersDic.Where(_pawn => _pawn.faction.id == factionId).FirstOrDefault();
            return frontPlayer;
        }

        public void RemovePawn(FrontPlayer frontPlayer,PawnAvatar pawnAvatar)
        {
            frontPlayer.RemovePawnAvatar(pawnAvatar);
        }

        public PawnAvatar GetPawnAvatarByPlayer(long id, FrontPlayer frontPlayer)
        {
            PawnAvatar pawnAvatar = frontPlayer.GetPawnAvatars().Where(_pawn => _pawn.pawnAgent.pawn.id == id).FirstOrDefault();
            return pawnAvatar;
        }

        public List<PawnAvatar> GetPawnAvatars(FrontPlayer frontPlayer)
        {
            return frontPlayer.GetPawnAvatars();
        }
        
        public List<Pawn> GetPawns(FrontPlayer frontPlayer)
        {
            List<Pawn> pawns = new List<Pawn>();
            for(int i = 0; i < frontPlayer.GetPawnAvatars().Count; i++)
            {
                pawns.Add(frontPlayer.GetPawnAvatars()[i].pawnAgent.pawn);
            }
            return pawns;
        }
        public FrontPlayer GenFactionPlayer(Faction faction, bool isLocalPlayer, bool isBot, bool forceGenNewPlayer, int totalMilitaryRes, EventCollections eventCollections)//forceGenNewPlayer????????????????????????????????????????????????????????????????player
        {
            FrontPlayer frontPlayer = new FrontPlayer(faction, isLocalPlayer, isBot, totalMilitaryRes,eventCollections);
            if (forceGenNewPlayer)
            {
                this.frontPlayersDic.Add(frontPlayer);
            }
            else
            {
                FrontPlayer frontPlayertemp = this.frontPlayersDic.Where(_player => _player.faction == faction).FirstOrDefault();
                if (frontPlayertemp != null)
                    frontPlayer = frontPlayertemp;
                else
                    this.frontPlayersDic.Add(frontPlayer);

            }
            return frontPlayer;

        }
        
        public PawnAvatar AddPawnOnArea(Pawn pawn, Area area, long mapId, FrontPlayer frontPlayer)
        {
            if (area == null)
            {
                return null;
            }
            
            GameObject pawnAvatar = Instantiate(Resources.Load(pawnPath)) as GameObject;
            pawnAvatar.GetComponent<PawnAvatar>().Init(pawn, frontPlayer, mapId, area);
            //DialogueTriggerManager.Instance.TimeTriggerEvent += pawnAvatar.GetComponent<PawnAvatar>().ReceiveCurrentTime;
            frontPlayer.AddPawnAvatar(pawnAvatar.GetComponent<PawnAvatar>());
            pawnAvatar.gameObject.transform.parent = MapManager.Instance.currentMap.PawnCollect.transform;
            return pawnAvatar.GetComponent<PawnAvatar>();
        }

        public void AddAreaForPlayer(Area area, FrontPlayer frontPlayer)
        {
            area.playerBelong.RemoveArea(area);
            area.playerBelong = frontPlayer;
            area.playerBelong.AddArea(area);
            area.SetColor(frontPlayer.faction.areaColor, false, true);
        }

        public void UpdateEventOptionForAllPlayers(long eventOptionId)
        {
            for(int i = 0; i < this.frontPlayersDic.Count; i++)
            {
                this.frontPlayersDic[i].eventCollections.AddEventOptionId(eventOptionId);
            }
        }

        public void ClearFront()
        {
            for (int i = 0; i < this.frontPlayersDic.Count; i++)
            {
                this.frontPlayersDic[i].ClearPlayer();
            }
        }
    }
}