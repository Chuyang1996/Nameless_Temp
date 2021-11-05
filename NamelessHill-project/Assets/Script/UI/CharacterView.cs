using Nameless.DataMono;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nameless.UI {
    public class CharacterView : MonoBehaviour
    {
        public Button closeBtn;

        public Text name;
        public Text rank;

        public Slider healthBar;
        public Slider moraleBar;
        public Slider ammoBar;
        public Slider foodBar;

        public Text attack;
        public Text defend;
        public Text speed;
        public Text hit;
        public Text dex;

        private PawnAvatar currentPawn;
        // Start is called before the first frame update
        void Start()
        {
            this.closeBtn.onClick.AddListener(() => { this.ResetPanel(); this.gameObject.SetActive(false); });
        }
        public void SetNewPawn(PawnAvatar pawnAvatar)
        {
            this.currentPawn = pawnAvatar;

            this.name.text = this.currentPawn.name;
            this.rank.text = this.currentPawn.pawnAgent.rank;

            this.healthBar.value = this.currentPawn.pawnAgent.pawn.curHealth/this.currentPawn.pawnAgent.pawn.maxHealth;
            this.moraleBar.value = this.currentPawn.pawnAgent.pawn.curMorale/this.currentPawn.pawnAgent.pawn.maxMorale;
            this.ammoBar.value = this.currentPawn.pawnAgent.pawn.curAmmo/this.currentPawn.pawnAgent.pawn.maxAmmo;


            this.attack.text = ((int)this.currentPawn.pawnAgent.pawn.curAttack).ToString();
            this.defend.text = ((int)this.currentPawn.pawnAgent.pawn.curDefend).ToString();
            this.speed.text = ((int)this.currentPawn.pawnAgent.pawn.curSpeed).ToString();
            this.hit.text = ((int)this.currentPawn.pawnAgent.pawn.curHit).ToString();
            this.dex.text = ((int)this.currentPawn.pawnAgent.pawn.curDex).ToString();

            this.currentPawn.pawnAgent.HealthBarEvent += HealthChange;
            this.currentPawn.pawnAgent.MoraleBarEvent += MoraleChange;
            this.currentPawn.pawnAgent.AmmoBarEvent += AmmoChange;


            this.currentPawn.pawnAgent.AttackValueEvent += AttackChange;
            this.currentPawn.pawnAgent.DefendValueEvent += DefendChange;
            this.currentPawn.pawnAgent.SpeedValueEvent += SpeedChange;
            this.currentPawn.pawnAgent.HitValueEvent += HitChange;
            this.currentPawn.pawnAgent.DexValueEvent += DexChange;

            this.gameObject.SetActive(true);
        }

        private void ResetPanel()
        {
            this.currentPawn.pawnAgent.HealthBarEvent -= HealthChange;
            this.currentPawn.pawnAgent.MoraleBarEvent -= MoraleChange;
            this.currentPawn.pawnAgent.AmmoBarEvent -= AmmoChange;


            this.currentPawn.pawnAgent.AttackValueEvent -= AttackChange;
            this.currentPawn.pawnAgent.DefendValueEvent -= DefendChange;
            this.currentPawn.pawnAgent.SpeedValueEvent -= SpeedChange;
            this.currentPawn.pawnAgent.HitValueEvent -= HitChange;
            this.currentPawn.pawnAgent.DexValueEvent -= DexChange;
            this.currentPawn = null;
        }
        public void HealthChange(float value)
        {
            this.healthBar.value = value;
        }
        public void MoraleChange(float value)
        {
            this.moraleBar.value = value;
        }
        public void AmmoChange(float value)
        {
            this.ammoBar.value = value;
        }
        public void FoodChange(float value)
        {
            this.foodBar.value = value;
        }

        public void AttackChange(float value)
        {
            this.attack.text = value.ToString();
        }

        public void DefendChange(float value)
        {
            this.defend.text = ((int)value).ToString();
        }

        public void SpeedChange(float value)
        {
            this.speed.text = ((int)value).ToString();
        }

        public void HitChange(float value)
        {
            this.hit.text = ((int)value).ToString();
        }

        public void DexChange(float value)
        {
            this.dex.text = ((int)value).ToString();
        }
    }
}