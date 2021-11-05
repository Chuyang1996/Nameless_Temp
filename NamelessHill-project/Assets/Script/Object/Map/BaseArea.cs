using Nameless.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.DataMono
{
    public class BaseArea : Area
    {
        public BattleView battleView;//´ýÐÞ¸Ä

        public int supportAmmo;
        public float supportDeltaTime;

        private void Start()
        {
            this.areaSprite = GetComponent<SpriteRenderer>();
            this.areaSprite.color = Color.Lerp(Color.blue,Color.green,0.5F);
            this.type = AreaType.Base;
        }

        private void Update()
        {
            //this.battleView.ammoTxt.text = totalAmmo.ToString();
            //this.battleView.foodTxt.text = totalFood.ToString();
        }
        public override void AddPawn(PawnAvatar pawn)
        {
            this.pawns.Add(pawn);
            this.areaSprite.color = Color.Lerp(Color.blue, Color.green, 0.5F);
            this.ResetPawnPos(); 
        }

        public override void RemovePawn(PawnAvatar pawn)
        {
            this.pawns.Remove(pawn);
            this.areaSprite.color = Color.Lerp(Color.blue, Color.green, 0.5F);
            this.ResetPawnPos();
        }
    }
}