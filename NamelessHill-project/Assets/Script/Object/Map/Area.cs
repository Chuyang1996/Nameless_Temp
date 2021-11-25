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
        public int id;

        public float costMorale = -1.0f;
        public float costTimeMorale = 1.0f;

        public int Medicine
        {
            set
            {
                if (medicine > 1)
                    medicine = 1;
                else if (medicine < 0)
                    medicine = 0;
                else
                    medicine = value;
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
                if (ammo > 1)
                    ammo = 1;
                else if (ammo < 0)
                    ammo = 0;
                else
                    ammo = value;
            }
            get
            {
                return ammo;
            }
        }
        private int ammo = 0;

        public GameObject centerNode;
        private GameObject matGeneratePoint;
        //[HideInInspector]
        public List<PawnAvatar> pawns = new List<PawnAvatar>();
        public Dictionary<MatType, Mat> mats = new Dictionary<MatType,Mat>();
        //[HideInInspector]
        public AreaType type;


        protected SpriteRenderer areaSprite;
        //[HideInInspector]
        public List<Area> neighboors;
        private void Start()//待修改 等框架搭建完成
        {
            this.matGeneratePoint = transform.Find("MatPos").gameObject;
            this.areaSprite = GetComponent<SpriteRenderer>();
            this.type = AreaType.Normal;
            
        }
        public void Init()//待修改 等框架搭建完成
        {
            
            this.matGeneratePoint = transform.Find("MatPos").gameObject;
            Debug.Log(this.matGeneratePoint);
            this.areaSprite = GetComponent<SpriteRenderer>();
            GameManager.Instance.AddAreaForPlayer(this);
            //if (this.pawns.Count > 0)
            //{
            //    this.areaSprite.color = this.pawns[0].isAI ? new Color(1, 0, 0, 0.2f) : new Color(0, 1, 0, 0.2f);
            //}
            //else
            //{
            //    this.areaSprite.color = new Color(1, 1, 1, 0.2f);
            //}

        }
        public virtual bool AddPawn(PawnAvatar pawn)
        {
            this.pawns.Add(pawn);
            if(this.pawns.Count > 1)
            {
                this.pawns.Remove(pawn);
                return false;
            }


            //this.ResetPawnPos();
            return true;
        }
        public virtual void RemovePawn(PawnAvatar pawn)
        {
            this.pawns.Remove(pawn);
            //if (this.pawns.Count > 0)
            //{
            //    this.areaSprite.color = this.pawns[0].isAI ? new Color(1, 0, 0, 0.2f) : new Color(0, 1, 0, 0.2f);
            //}
            //this.ResetPawnPos();
        }
        public virtual void ChangeColor(bool isAi)
        {
            this.areaSprite.color = isAi? new Color(1, 0, 0, 0.2f) : new Color(0, 1, 0, 0.2f);
            
        }
        public virtual bool IsMatExist(MatType type ,int num)
        {
            if (this.mats.ContainsKey(type))
            {
                this.mats[type].AddMat(num);
                return true;
            }
            else
                return false;
        }
        public virtual void AddMat(Mat mat)
        {
            this.mats.Add(mat.type,mat);
            this.ResetMatPos();
        }
        public virtual void RemoveMat(Mat mat)
        {
            this.mats.Remove(mat.type);
            DestroyImmediate(mat.gameObject);
            this.ResetMatPos();
        }
        public virtual void ResetMatPos()
        {
            float durDis = (this.mats.Count - 1) * 0.5f;
            float halfDis = -durDis / 2;
            foreach(var child in this.mats)
            {
                child.Value.gameObject.transform.parent = this.matGeneratePoint.gameObject.transform;
                child.Value.gameObject.transform.localPosition = Vector3.zero;
                child.Value.gameObject.transform.position = new Vector3(child.Value.gameObject.transform.position.x + halfDis, child.Value.gameObject.transform.position.y, child.Value.gameObject.transform.position.z);
                halfDis += 0.5f;
            }
        }
        public virtual void CostMedicine(PawnAvatar pawn)
        {
            if (pawn.pawnAgent.pawn.curHealth/ pawn.pawnAgent.pawn.maxHealth < 0.2)
            {
                if (this.Medicine > 0)
                {
                    this.Medicine--;
                    pawn.pawnAgent.InitHealth(0.5f * pawn.pawnAgent.pawn.maxHealth);
                }
            }
        }
        public virtual void CostAmmo(PawnAvatar pawn)
        {
            if (pawn.pawnAgent.pawn.curAmmo <= 0)
            {
                if (this.Ammo > 0)
                {
                    this.Ammo--;
                    pawn.pawnAgent.InitAmmo(pawn.pawnAgent.pawn.maxAmmo);
                }
            }
        }

        public virtual void OccupyArea()
        {
            if (this.pawns.Count > 0)
            {
                bool isArealyBelonged = false;
                isArealyBelonged = GameManager.Instance.IsBelongToSameSide(this,this.pawns[0]);
                if (!isArealyBelonged)
                    StartCoroutine(OcuppyProcess(5.0f));
                    
            }
        }

        IEnumerator OcuppyProcess(float waitTime)
        {
            float countTime = 0.0f;
            bool occupySuccess = true;
            while (countTime<waitTime)
            {
                countTime+=Time.deltaTime;
                if (this.pawns.Count <= 0 || this.pawns[0].State != PawnState.Wait)
                {
                    occupySuccess = false;
                    break;
                }
                yield return null;
            }
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