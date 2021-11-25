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

        private void Start()//待修改 等框架搭建完成
        {
            this.areaSprite = GetComponent<SpriteRenderer>();
            this.areaSprite.color = Color.Lerp(Color.blue,Color.green,0.5F);
            this.type = AreaType.Base;
        }
        public override bool AddPawn(PawnAvatar pawn)
        {
            this.pawns.Add(pawn);
            if(this.pawns.Count>1)
            {
                this.pawns.Remove(pawn);
                return false;
            }
            this.areaSprite.color = Color.Lerp(Color.blue, Color.green, 0.5F);
            return true;
        }

        public override void RemovePawn(PawnAvatar pawn)
        {
            this.pawns.Remove(pawn);
            this.areaSprite.color = Color.Lerp(Color.blue, Color.green, 0.5F);
        }

        public override void ChangeColor(bool isAi)
        {
            
        }
    }
}