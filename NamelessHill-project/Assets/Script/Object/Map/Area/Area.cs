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

        public int Medicine
        {
            set
            {
                if(value > 0 && medicine == 0)
                {
                    GenerateManager.Instance.GenerateBuild(this, BuildType.MeidicalBuild);
                }
                medicine = value;
                if (medicine > 1)
                    medicine = 1;
                else if (medicine < 0)
                    medicine = 0;

                if (medicine == 0)
                {
                    if (this.builds.ContainsKey(BuildType.MeidicalBuild))
                    {
                        this.RemoveBuild(this.builds[BuildType.MeidicalBuild]);
                    }
                }
            }
            get
            {
                return medicine;
            }
        }
        private int medicine = 0;

        public int Ammo
        {
            set
            {
                if (value > 0 && ammo == 0)
                {
                    GenerateManager.Instance.GenerateBuild(this, BuildType.AmmoBuild);
                }
                ammo = value;
                if (ammo > 1)
                    ammo = 1;
                else if (ammo < 0)
                    ammo = 0;

                if (ammo == 0) 
                {
                    if (this.builds.ContainsKey(BuildType.AmmoBuild))
                    {
                        this.RemoveBuild(this.builds[BuildType.AmmoBuild]);
                    }
                }
            }
            get
            {
                return ammo;
            }
        }
        private int ammo = 0;

        public GameObject centerNode;
        private GameObject matPoint;
        private GameObject ammoPoint;
        private GameObject meidicalPoint;
        //[HideInInspector]
        public List<PawnAvatar> pawns = new List<PawnAvatar>();
        public Dictionary<MatType, Mat> mats = new Dictionary<MatType,Mat>();
        public Dictionary<BuildType, Build> builds = new Dictionary<BuildType, Build>();
        //[HideInInspector]
        public AreaType type;


        protected SpriteRenderer areaSprite;
        //[HideInInspector]
        public List<Area> neighboors = new List<Area>();

        public virtual void Init(int id, AreaAgent areaAgent)//���޸� �ȿ�ܴ���
        {
            this.localId = id;
            this.centerNode = this.transform.Find("CenterNode").gameObject;
            this.matPoint = this.transform.Find("MatPos").gameObject;
            this.ammoPoint = this.transform.Find("AmmoPos").gameObject;
            this.meidicalPoint = this.transform.Find("MedicialPos").gameObject;
            this.areaSprite = this.GetComponent<SpriteRenderer>();
            this.type = areaAgent.type;
            GameManager.Instance.AddAreaForPlayer(this);

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
        }//���������һ����ɫ
        public virtual void RemovePawn(PawnAvatar pawn)
        {
            this.pawns.Remove(pawn);

        }//�������Ƴ�һ����ɫ
        public virtual void ChangeColor(bool isAi)
        {
            if(GameManager.Instance.accessbility)
                this.areaSprite.color = isAi ? new Color(0.91f, 0.82f, 0.34f, 0.2f) : new Color(0.56f, 0.65f, 0.94f, 0.2f);
            else 
               this.areaSprite.color = isAi ? new Color(1, 0, 0, 0.2f) : new Color(0, 1, 0, 0.2f);

            
        }//������ı���ɫ
        public virtual bool IsMatExist(MatType type ,int num)
        {
            if (this.mats.ContainsKey(type))
            {
                this.mats[type].AddMat(num);
                return true;
            }
            else
                return false;
        }//�������Ƿ���ڸò���
        public virtual bool IsBuildExist(BuildType type)
        {
            if (this.builds.ContainsKey(type))
                return true;
            else
                return false;
        }//�������Ƿ���ڸý���
        public virtual void AddMat(Mat mat)
        {
            if (!this.mats.ContainsKey(mat.type))
            {
                this.mats.Add(mat.type, mat);
                this.ResetMatPos();
            }
        }//��������Ӳ���
        public virtual void RemoveMat(Mat mat)
        {
            if (this.mats.ContainsKey(mat.type))
            {
                this.mats.Remove(mat.type);
                DestroyImmediate(mat.gameObject);
                this.ResetMatPos();
            }
        }//�������Ƴ�����
        public virtual void AddBuild(Build build)
        {
            if (!this.builds.ContainsKey(build.type))
            {
                this.builds.Add(build.type, build);
                if (build.type == BuildType.AmmoBuild)
                {
                    build.gameObject.transform.position = this.ammoPoint.transform.position;
                }
                else if (build.type == BuildType.MeidicalBuild)
                {
                    build.gameObject.transform.position = meidicalPoint.transform.position;
                }
                build.gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
        }//��������ӽ���
        public virtual void RemoveBuild(Build build)
        {
            if (this.builds.ContainsKey(build.type))
            {
                this.builds.Remove(build.type);
                DestroyImmediate(build.gameObject);
            }
        }//�������Ƴ�����
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
        }//���������ò���λ��
        public virtual void CostMedicine(PawnAvatar pawn)
        {
            if (pawn.pawnAgent.pawn.curHealth/ pawn.pawnAgent.pawn.maxHealth <= 0.7)
            {
                if (this.Medicine > 0)
                {
                    this.Medicine--;
                    pawn.pawnAgent.HealthChange(0.3f * pawn.pawnAgent.pawn.maxHealth);
                }
            }
        }
        public virtual void CostAmmo(PawnAvatar pawn)
        {
            if (pawn.pawnAgent.pawn.curAmmo / pawn.pawnAgent.pawn.maxAmmo <= 0.7)
            {
                if (this.Ammo > 0)
                {
                    this.Ammo--;
                    pawn.pawnAgent.AmmoChange((int)0.3f * pawn.pawnAgent.pawn.maxAmmo);
                }
            }
        }
        public virtual void OccupyArea()
        {
            if (this.pawns.Count > 0)
            {
                bool isArealyBelonged = GameManager.Instance.IsBelongToSameSide(this,this.pawns[0]);
                if (!isArealyBelonged)
                    StartCoroutine(OcuppyProcess(5.0f));
                    
            }
        }//ռ�챾����
        IEnumerator OcuppyProcess(float waitTime)
        {
            float countTime = 0.0f;
            bool occupySuccess = true;
            while(this.pawns.Count > 0 && this.pawns[0].State != PawnState.Wait)//��������ִ��˳������
            {
                yield return null;
            }
            if(this.pawns.Count>0)
               this.pawns[0].ocuppyBar.gameObject.SetActive(true);
            while (countTime<waitTime)
            {
                //Debug.LogError("ռ����ing");
                countTime+=Time.deltaTime;
                if (this.pawns.Count <= 0 || this.pawns[0].State != PawnState.Wait)
                {
                    //Debug.LogError(this.pawns.Count + "+ռ��ʧ��+" + this.pawns[0].State);
                    occupySuccess = false;
                    break;
                }
                if (this.pawns.Count > 0)
                    this.pawns[0].OcuppyLoading(countTime / waitTime);
                yield return null;
            }
            if (this.pawns.Count > 0)
                this.pawns[0].ocuppyBar.gameObject.SetActive(false);
            if (occupySuccess)
            {
                if (this.pawns[0].isAI)
                    GameManager.Instance.AddAreaForEnemy(this);
                else
                    GameManager.Instance.AddAreaForPlayer(this);
            }

        }

    }
}