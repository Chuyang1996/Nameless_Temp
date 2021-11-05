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
        //[HideInInspector]
        public List<PawnAvatar> pawns = new List<PawnAvatar>();
        public List<Mat> mats = new List<Mat>();
        //[HideInInspector]
        public AreaType type;


        protected SpriteRenderer areaSprite;
        //[HideInInspector]
        public List<Area> neighboors;
        private void Start()
        {
            this.areaSprite = GetComponent<SpriteRenderer>();
            this.type = AreaType.Normal;
        }
        public void Init()
        {
            this.areaSprite = GetComponent<SpriteRenderer>();
            if (this.pawns.Count > 0)
            {
                if (this.pawns[0].isAI)
                    this.areaSprite.color = Color.red;
                else
                    this.areaSprite.color = Color.green;
            }
            else
            {
                this.areaSprite.color = Color.white;
            }
        }


        public virtual void AddPawn(PawnAvatar pawn)
        {
            this.pawns.Add(pawn);
            if (this.pawns.Count > 0)
            {
                if (this.pawns[0].isAI)
                    this.areaSprite.color = Color.red;
                else
                    this.areaSprite.color = Color.green;
            }
            else
            {
                this.areaSprite.color = Color.white;
            }
            this.ResetPawnPos();
        }
        public virtual void RemovePawn(PawnAvatar pawn)
        {
            this.pawns.Remove(pawn);
            if (this.pawns.Count > 0)
            {
                if (this.pawns[0].isAI)
                    this.areaSprite.color = Color.red;
                else
                    this.areaSprite.color = Color.green;
            }
            else
            {
                this.areaSprite.color = Color.white;
            }
            this.ResetPawnPos();
        }
        public virtual void AddMat(Mat mat)
        {
            this.mats.Add(mat);
            this.ResetMatPos();
        }
        public virtual void RemoveMat(Mat mat)
        {
            this.mats.Remove(mat);
            DestroyImmediate(mat.gameObject);
            this.ResetMatPos();
        }
        public virtual void ResetMatPos()
        {
            float durDis = (this.mats.Count - 1) * 0.5f;
            float halfDis = -durDis / 2;
            for (int i = 0; i < this.mats.Count; i++)
            {
                this.mats[i].transform.parent = this.centerNode.gameObject.transform;
                this.mats[i].transform.localPosition = Vector3.zero;
                this.mats[i].transform.position = new Vector3(this.mats[i].transform.position.x + halfDis, this.mats[i].transform.position.y, this.mats[i].transform.position.z);
                halfDis += 0.5f;
            }
        }
        public virtual void ResetPawnPos()
        {
            float durDis = (this.pawns.Count - 1) * 0.5f;
            float halfDis = - durDis / 2;
            for(int i = 0; i < this.pawns.Count; i++)
            {
                this.pawns[i].transform.position = new Vector3(this.pawns[i].transform.position.x + halfDis, this.pawns[i].transform.position.y, this.pawns[i].transform.position.z);
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

    }
}