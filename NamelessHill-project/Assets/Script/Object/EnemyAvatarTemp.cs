using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Nameless.DataMono
{
    public class EnemyAvatarTemp : MonoBehaviour
    {
        //public global::PawnProperty pawn;
        public Slider healthBar;
        public float currentheatlh;
        // Start is called before the first frame update
        void Start()
        {
            //this.pawn = new Pawn(100, 5);
            //this.currentheatlh = this.pawn.maxHealth;
            this.healthBar.value = 1;
        }

    }
}