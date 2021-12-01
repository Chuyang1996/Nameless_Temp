using Nameless.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.DataMono
{
    public class PawnCamp : MonoBehaviour
    {
        public GameObject btnDialogue;
        public GameObject leftSide;
        public GameObject rightSide;
        public SpriteRenderer pawnIcon;

        public Pawn pawn;
        public void Init(Pawn pawn)
        {
            if(pawn.leftOrRight < 0)
            {
                this.btnDialogue.transform.parent = this.leftSide.transform;
                this.btnDialogue.transform.localScale = new Vector3(-this.btnDialogue.transform.localScale.x, this.btnDialogue.transform.localScale.y, this.btnDialogue.transform.localScale.z);
            }
            else
            {
                this.btnDialogue.transform.parent = this.rightSide.transform;
            }
            this.btnDialogue.SetActive(true);
            this.pawnIcon.sprite = pawn.campIcon;
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("sssss");
                Vector2 raySelect = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(raySelect, Vector2.zero);
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject == this.btnDialogue.gameObject)//´ýÐÞ¸Ä.AI
                    {
                        

                    }
                }
            }
        }
    }
}