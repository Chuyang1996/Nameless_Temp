using Nameless.DataMono;
using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Nameless.Data
{
    public enum GenerateRuleType
    {
        None = 0,
        WaitToGen = 1,
        CoupleGroupGen = 2,

    }
    public class AreaAgent
    {
        public long Id;
        public string name;
        public string descrption;
        public AreaType type;
        public long eventOptionId;
        public PawnGenRule pawnRule;
        public FrontPlayer frontPlayer;
        public AreaAgent(long id, string name, string descrption, AreaType type, long eventOptionId, GenerateRuleType genType, List<PawnGroup> pawnGroups, FrontPlayer frontPlayer)
        {
            this.Id = id;
            this.name = name;
            this.descrption = descrption;
            this.type = type;
            this.eventOptionId = eventOptionId;
            this.frontPlayer = frontPlayer;
            if(type  == AreaType.Spawn)
            {
                if(genType == GenerateRuleType.WaitToGen)
                {
                    WaitGen waitGen = new WaitGen(pawnGroups, this.eventOptionId, frontPlayer);
                    this.pawnRule = waitGen;
                }
                else if(genType == GenerateRuleType.CoupleGroupGen)
                {
                    CoupleGroupGen coupleGen = new CoupleGroupGen(pawnGroups, this.eventOptionId, frontPlayer);
                    this.pawnRule = coupleGen;
                }
            }
        }

    }

    abstract public class PawnGenRule
    {
        public GenerateRuleType generateRuleType;
        public List<PawnGroup> pawnGroups = new List<PawnGroup>();
        public PawnGroup currentGroup;
        public FrontPlayer frontPlayer;
        public long eventOptionId;
        abstract protected bool Active();
        abstract public IEnumerator Execute(SpawnArea area);

    }

    public class WaitGen : PawnGenRule
    {
        bool isProcess = false;
        public WaitGen(List<PawnGroup> pawnGroup, long eventOptionId, FrontPlayer frontPlayer)
        {
            this.pawnGroups = pawnGroup;
            this.currentGroup = pawnGroup[0];
            this.eventOptionId = eventOptionId;
            this.frontPlayer = frontPlayer;
            this.isProcess = false;
        }

        protected override bool Active()
        {
            return !this.isProcess;
        }

        public override IEnumerator Execute(SpawnArea area)
        {
            while (true)
            {
                if (this.Active() && area.neighboors.Count > 0)
                    break;
                yield return null;
            }
            this.isProcess = true;
            while (this.eventOptionId != -1 && !this.frontPlayer.eventCollections.IsEventOptionChoosed(this.eventOptionId))
            {
                yield return null;
            }
            yield return new WaitForSecondsRealtime(this.currentGroup.waitGenerateTime * 1.25f);
            for (int i = 0; i < currentGroup.pawns.Count; i++)
            {
                while (area.pawns.Count > 0)
                {
                    yield return null;
                }
                while (area.pawns.Count > 0)
                {
                    yield return null;
                }
                while (!GameManager.Instance.isPlay)
                {
                    yield return null;
                }

                yield return new WaitForSecondsRealtime(this.currentGroup.durationTime * 1.25f);
                while (!GameManager.Instance.isPlay)
                {
                    yield return null;
                }
                area.GenPawn(currentGroup.pawns[i].id);
            }
        }

    }

    public class CoupleGroupGen : PawnGenRule
    {
        private bool isProcess = false;
        private List<PawnAvatar> pawnAvatars = new List<PawnAvatar>();
        private int groupIndex = 0;
        public CoupleGroupGen(List<PawnGroup> pawnGroups, long eventOptionId, FrontPlayer frontPlayer)
        {
            this.pawnGroups = pawnGroups;
            this.currentGroup = pawnGroups[0];

            this.frontPlayer = frontPlayer;
            this.eventOptionId = eventOptionId;
            this.groupIndex = 0;
            this.pawnAvatars = new List<PawnAvatar>();
            this.isProcess = false;
        }

        protected override bool Active()
        {
            if (this.groupIndex < this.pawnGroups.Count)
                return true;
            return false;
        }
        public override IEnumerator Execute(SpawnArea area)
        {
            while(area.neighboors.Count <= 0)
            {
                yield return null;
            } 
            while (this.eventOptionId != -1 && !this.frontPlayer.eventCollections.IsEventOptionChoosed(this.eventOptionId))
            {
                yield return null;
            }
            while (this.Active())
            {
                if (!this.isProcess && this.IfAllPawnsDeath())
                {
                    this.isProcess = true;
                    this.currentGroup = this.pawnGroups[this.groupIndex];
                    yield return new WaitForSecondsRealtime(this.currentGroup.waitGenerateTime * 1.25f);

                    for (int i = 0; i < currentGroup.pawns.Count; i++)
                    {
                        while (area.pawns.Count > 0)
                        {
                            yield return null;
                        } 

                        
                        while (area.pawns.Count > 0)
                        {
                            yield return null;
                        }
                        while (!GameManager.Instance.isPlay)
                        {
                            yield return null;
                        }
                        this.pawnAvatars.Add(area.GenPawn(currentGroup.pawns[i].id));
                        yield return new WaitForSecondsRealtime(this.currentGroup.durationTime * 1.25f);
                        while (!GameManager.Instance.isPlay)
                        {
                            yield return null;
                        }
                    }
                    this.isProcess = false;
                    this.groupIndex++;
                }
                yield return null;
            }
        }
        private bool IfAllPawnsDeath()
        {
            int indexNum = 0;
            for (int i = 0; i < this.pawnAvatars.Count; i++)
            {
                if (this.pawnAvatars[i] == null)
                    indexNum++;
            }
            if (indexNum == this.pawnAvatars.Count) 
            {
                this.pawnAvatars.Clear();
                
            }
            if(this.pawnAvatars.Count == 0)
            {
                return true;
            }
            return false;
        }



    }
}