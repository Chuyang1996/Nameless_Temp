using Nameless.Data;
using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.DataMono
{
    public enum AreaType
    {
        Normal = 0,
        Base = 1,
        UnPass = 2,
        Spawn = 3
    }
    public class Area : MonoBehaviour
    {
        public int localId;
        public float costMorale = -1.0f;
        public float costTimeMorale = 1.0f;

        //public int Medicine
        //{
        //    set
        //    {
        //        if(value > 0 && medicine == 0)
        //        {
        //            StaticObjGenManager.Instance.GenerateBuild(this, BuildType.MeidicalBuild);
        //        }
        //        medicine = value;
        //        if (medicine > 1)
        //            medicine = 1;
        //        else if (medicine < 0)
        //            medicine = 0;

        //        if (medicine == 0)
        //        {
        //            if (this.builds.ContainsKey(BuildType.MeidicalBuild))
        //            {
        //                this.RemoveBuild(this.builds[BuildType.MeidicalBuild]);
        //            }
        //        }
        //    }
        //    get
        //    {
        //        return medicine;
        //    }
        //}
        //private int medicine = 0;

        //public int Ammo
        //{
        //    set
        //    {
        //        if (value > 0 && ammo == 0)
        //        {
        //            StaticObjGenManager.Instance.GenerateBuild(this, BuildType.AmmoBuild);
        //        }
        //        ammo = value;
        //        if (ammo > 1)
        //            ammo = 1;
        //        else if (ammo < 0)
        //            ammo = 0;

        //        if (ammo == 0) 
        //        {
        //            if (this.builds.ContainsKey(BuildType.AmmoBuild))
        //            {
        //                this.RemoveBuild(this.builds[BuildType.AmmoBuild]);
        //            }
        //        }
        //    }
        //    get
        //    {
        //        return ammo;
        //    }
        //}
        //private int ammo = 0;

        public GameObject centerNode;
        private GameObject matPoint;
        private GameObject bunkerPos;
        private GameObject obstaclePos;
        private GameObject cannonPos;
        //[HideInInspector]
        public FrontPlayer playerBelong;
        public List<PawnAvatar> pawns = new List<PawnAvatar>();
        public Dictionary<MatType, Mat> mats = new Dictionary<MatType,Mat>();
        public BuildAvatar buildAvatar;
        //[HideInInspector]
        public AreaType type;


        protected SpriteRenderer areaSprite;
        //[HideInInspector]
        public List<Area> neighboors = new List<Area>();

        public virtual void Init(int id, AreaAgent areaAgent, FrontPlayer frontPlayer, long factionId)//待修改 等框架搭建完成
        {
            this.localId = id;
            this.centerNode = this.transform.Find("CenterNode").gameObject;
            this.matPoint = this.transform.Find("MatPos").gameObject;
            this.bunkerPos = this.transform.Find("BunkerPos").gameObject;
            this.obstaclePos = this.transform.Find("ObstaclePos").gameObject;
            this.cannonPos = this.transform.Find("CannonPos").gameObject;
            this.areaSprite = this.GetComponent<SpriteRenderer>();
            this.type = areaAgent.type;
            this.playerBelong = frontPlayer;
            FrontManager.Instance.AddAreaForPlayer(this, frontPlayer);

        }
        public virtual bool AddPawn(PawnAvatar pawn)
        {
            this.pawns.Add(pawn);
            if(this.pawns.Count > 1)
            {
                this.pawns.Remove(pawn);
                return false;
            }

            EventTriggerManager.Instance.CheckPawnArriveArea(pawn.pawnAgent.pawn.id, this.localId);
            //this.ResetPawnPos();
            return true;
        }//本区域添加一个角色
        public virtual void RemovePawn(PawnAvatar pawn)
        {
            this.pawns.Remove(pawn);

        }//本区域移除一个角色
        public virtual void ChangeColor(Color color)
        {
            this.areaSprite.color = color;
        }//本区域改变颜色
        public virtual bool IsMatExist(MatType type ,int num)
        {
            if (this.mats.ContainsKey(type))
            {
                this.mats[type].AddMat(num);
                return true;
            }
            else
                return false;
        }//本区域是否存在该材料
        public virtual bool IsBuildExist()
        {
            if (this.buildAvatar!=null)
                return true;
            else
                return false;
        }//本区域是否存在该建筑
        public virtual void AddMat(Mat mat)
        {
            if (!this.mats.ContainsKey(mat.type))
            {
                this.mats.Add(mat.type, mat);
                this.ResetMatPos();
            }
        }//本区域添加材料
        public virtual void RemoveMat(Mat mat)
        {
            if (this.mats.ContainsKey(mat.type))
            {
                this.mats.Remove(mat.type);
                DestroyImmediate(mat.gameObject);
                this.ResetMatPos();
            }
        }//本区域移除材料
        public virtual void AddBuild(BuildAvatar build)
        {
            if (this.buildAvatar == null)
            {
                this.buildAvatar = build;
                if (build.buildType == BuildType.Obstacle)
                {
                    build.gameObject.transform.parent = this.obstaclePos.transform;
                }
                else if (build.buildType == BuildType.Bunker)
                {
                    build.gameObject.transform.parent = this.bunkerPos.transform;
                }
                else if (build.buildType == BuildType.Cannon)
                {
                    build.gameObject.transform.parent = this.cannonPos.transform;
                }
                build.gameObject.transform.localPosition = new Vector3(0, 0, 0);
                build.gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
        }//本区域添加建筑
        public virtual void RemoveBuild(BuildAvatar build)
        {
            if (this.buildAvatar!= null)
            {
                DestroyImmediate(build.gameObject);
            }
        }//本区域移除建筑
        public virtual void ResetMatPos()
        {
            float durDis = (this.mats.Count - 1) * 0.5f;
            float halfDis = -durDis / 2;
            foreach(var child in this.mats)
            {
                child.Value.gameObject.transform.parent = this.matPoint.gameObject.transform;
                child.Value.gameObject.transform.localPosition = Vector3.zero;
                child.Value.gameObject.transform.position = new Vector3(child.Value.gameObject.transform.position.x + halfDis, child.Value.gameObject.transform.position.y, child.Value.gameObject.transform.position.z);
                halfDis += 0.5f;
            }
        }//本区域重置材料位置
        public virtual void ResetBuildPos(BuildAvatar buildAvatar)
        {
            if (buildAvatar is ObstacleAvatar)
            {
                buildAvatar.transform.parent = this.obstaclePos.transform;
                this.buildAvatar.transform.localPosition = new Vector3(0, 0, 0);
            }
            else if (buildAvatar is BunkerAvatar)
            {
                buildAvatar.transform.parent = this.bunkerPos.transform;
                this.buildAvatar.transform.localPosition = new Vector3(0, 0, 0);
            }
            else if (buildAvatar is CannonAvatar)
            {
                buildAvatar.transform.parent = this.cannonPos.transform;
                this.buildAvatar.transform.localPosition = new Vector3(0, 0, 0);
            }
            
        }
        public virtual void OccupyArea()
        {
            if (this.pawns.Count > 0)
            {
                bool isArealyBelonged = FactionManager.Instance.IsSameSide(this.playerBelong.faction,this.pawns[0].pawnAgent.frontPlayer.faction);// GameManager.Instance.IsBelongToSameSide(this,this.pawns[0]);
                if (!isArealyBelonged)
                    StartCoroutine(OcuppyProcess(5.0f));
                    
            }
        }//占领本区域
        public bool CanBuild(Faction faction)
        {
            if (this.pawns.Count == 0 && this.buildAvatar == null && FactionManager.Instance.IsSameSide(this.playerBelong.faction, faction))
                return true;
            return false;
        }
        public Color GetColor()
        {
            return this.areaSprite.color;
        }
        public void SetColor(Color color)
        {
            this.areaSprite.color = color;
        }
        IEnumerator OcuppyProcess(float waitTime)
        {
            float countTime = 0.0f;
            bool occupySuccess = true;
            while(this.pawns.Count > 0 && this.pawns[0].State != PawnState.Wait)//这里由于执行顺序问题
            {
                yield return null;
            }
            if(this.pawns.Count>0)
               this.pawns[0].ocuppyBar.gameObject.SetActive(true);
            while (countTime<waitTime)
            {
                //Debug.LogError("占领中ing");
                countTime+=Time.deltaTime;
                if (this.pawns.Count <= 0 || this.pawns[0].State != PawnState.Wait)
                {
                    //Debug.LogError(this.pawns.Count + "+占领失败+" + this.pawns[0].State);
                    occupySuccess = false;
                    break;
                }
                if (this.pawns.Count > 0)
                    this.pawns[0].BehaviorLoading(countTime / waitTime);
                yield return null;
            }
            if (this.pawns.Count > 0)
                this.pawns[0].ocuppyBar.gameObject.SetActive(false);
            if (occupySuccess)
            {
                FrontManager.Instance.AddAreaForPlayer(this, this.pawns[0].pawnAgent.frontPlayer);
            }

        }

    }
}