using Nameless.Data;
using Nameless.DataMono;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nameless.UI {
    public class CharacterView : MonoBehaviour
    {
        public Button closeBtn;
        public Sprite[] stateSprite;

        public Text name;
        public Text rank;

        public Slider healthBar;
        public Image moraleIm;
        public Text ammoText;

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
            this.MoraleChange(pawnAvatar.pawnAgent);
            this.AmmoChange(pawnAvatar.pawnAgent);


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
        public void MoraleChange(PawnAgent value)
        {
            float curMorale = (float)value.pawn.curMorale;
            float maxMorale = (float)value.pawn.maxMorale;
            if (curMorale >= maxMorale / 2)
            {
                this.moraleIm.sprite = stateSprite[0];
            }
            else if (maxMorale / 4 <= curMorale && curMorale < maxMorale / 2)
            {
                this.moraleIm.sprite = stateSprite[1];
            }
            else
            {
                this.moraleIm.sprite = stateSprite[2];
            }
        }
        public void AmmoChange(PawnAgent value)
        {
            this.ammoText.text = value.pawn.curAmmo.ToString() + "/" + value.pawn.maxAmmo.ToString(); 
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