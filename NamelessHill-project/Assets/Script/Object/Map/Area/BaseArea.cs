using Nameless.Data;
using Nameless.Manager;
using Nameless.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.DataMono
{
    public class BaseArea : Area
    {
        public BattleView battleView;//待修改

        public int supportAmmo;
        public float supportDeltaTime;

        public override void Init(int id,AreaAgent areaAgent, FrontPlayer frontPlayer)
        {
            base.Init(id, areaAgent, frontPlayer);
            this.recordColor = new Color(1, 1, 1, 0.19f);
            //Color colorb = new Color(0, 0, 1, 0.4f); 
            //Color colorg = new Color(0, 1, 0, 0.4f); 
            //this.areaSprite.color = Color.Lerp(colorb, colorg, 0.5F);
            
        }
        //private void Start()//待修改 等框架搭建完成
        //{
        //    this.areaSprite = GetComponent<SpriteRenderer>();
        //    this.areaSprite.color = Color.Lerp(Color.blue,Color.green,0.5F);
        //    this.type = AreaType.Base;
        //}


        public override void RemovePawn(PawnAvatar pawn)
        {
            this.pawns.Remove(pawn);
        }

        public override void SetColor(Color color, bool isForce, bool isRecord)
        {
            if(isForce)
                this.areaSprite.color = color;
            else
                this.areaSprite.color = new Color(1, 1, 1, 0.19f);

        }
    }
}