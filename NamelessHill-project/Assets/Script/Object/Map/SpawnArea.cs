using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.DataMono
{
    public class SpawnArea : Area
    {
        public bool spawnAI;
        public int limiltedNum = 5;
        public float durationTimeSpawn = 0.0f;
        public string pawnPath = "Prefabs/Pawn";
        // Start is called before the first frame update


        private float countTimeSpawn = 0.0f;
        private int countNumSpawn = 0;

        private void Start()//待修改 等框架搭建完成
        {
            this.areaSprite = GetComponent<SpriteRenderer>();
            //this.areaSprite.color = Color.white;
            this.type = AreaType.Spawn;
        }
        // Update is called once per frame
        void Update()
        {
            if (this.pawns.Count <= 0)
            {
                if (this.countTimeSpawn > this.durationTimeSpawn && this.countNumSpawn < this.limiltedNum)
                {
                    this.countNumSpawn++;
                    this.countTimeSpawn = 0.0f;
                    GameObject pawnAvatar = Instantiate(Resources.Load(pawnPath)) as GameObject;
                    pawnAvatar.GetComponent<PawnAvatar>().currentArea = this;
                    pawnAvatar.gameObject.transform.position = this.centerNode.transform.position;
                    if (spawnAI)
                    {
                        pawnAvatar.GetComponent<PawnAvatar>().isAI = true;
                        pawnAvatar.GetComponent<PawnAvatar>().nameTxt.color = Color.red;
                        pawnAvatar.GetComponent<PawnAvatar>().healthBarColor.color = Color.red;
                    }
                    this.AddPawn(pawnAvatar.GetComponent<PawnAvatar>());
                    pawnAvatar.GetComponent<PawnAvatar>().Init(0);
                    DialogueTriggerManager.Instance.TimeTriggerEvent += pawnAvatar.GetComponent<PawnAvatar>().ReceiveCurrentTime;
                    GameManager.Instance.enemyPawns.Add(pawnAvatar.GetComponent<PawnAvatar>());
                }
                else
                {
                    this.countTimeSpawn += Time.deltaTime;
                }
            }
        }
    }
}