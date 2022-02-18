using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Nameless.DataMono
{
    public class Camp : MonoBehaviour
    {
        public Sprite book;
        public Sprite bookMark;

        public Sprite light;
        public Sprite lightMark;

        public Sprite dialogueIm;
        public Sprite moraleUp;
        public Sprite moraleMiddle;
        public Sprite moraleDown;

        public GameObject[] pawnsPos;
        public GameObject noteBtn;
        public GameObject lightBtn;

        private List<PawnCamp> allCampPawns = new List<PawnCamp>();
        private string pawnPath = "Prefabs/PawnCamp";
        public void InitCamp(List<PawnAvatar> pawnAvatars)
        {
            for (int i = 0; i < pawnAvatars.Count; i++)
            {
                this.allCampPawns.Add(this.GenerateCampPawn(pawnAvatars[i]));
            }
            for (int i = 0; i < this.allCampPawns.Count; i++)
            {
                this.allCampPawns[i].RefreshPawnCamp();
            }
        }
        public PawnCamp FindPawnInCamp(long id)
        {
            PawnCamp pawnCamp = this.allCampPawns.Where(_pawn => _pawn.pawn.id == id).FirstOrDefault();
            return pawnCamp;
        }
        public List<PawnCamp> CampPawns()
        {
            return this.allCampPawns;
        }

        public void ResetAllBtnState()
        {
            for (int i = 0; i < this.allCampPawns.Count; i++)
            {
                this.allCampPawns[i].pawnIcon.sprite = this.allCampPawns[i].pawnSpriteMark;
            }
            this.noteBtn.GetComponent<SpriteRenderer>().sprite = this.book;
            this.lightBtn.GetComponent<SpriteRenderer>().sprite = this.light;
    }
        private PawnCamp GenerateCampPawn(PawnAvatar pawn)
        {
            GameObject pawnCamp = Instantiate(Resources.Load(this.pawnPath)) as GameObject;
            pawnCamp.GetComponent<PawnCamp>().Init(pawn.pawnAgent.pawn);
            if (pawn.pawnAgent.pawn.campPosIndex < this.pawnsPos.Length)
                pawnCamp.transform.parent = pawnsPos[pawn.pawnAgent.pawn.campPosIndex].transform;
            else
                pawnCamp.transform.parent = pawnsPos[0].transform;
            pawnCamp.transform.localPosition = new Vector3(0, 0, 0);

            return pawnCamp.GetComponent<PawnCamp>();
        }
    }
}