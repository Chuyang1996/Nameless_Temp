using Nameless.Data;
using Nameless.DataMono;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.Manager
{
    public class CampManager : SingletonMono<CampManager>
    {
        public GameObject campScene;
        public GameObject[] pawnsPos;
        public List<PawnCamp> allCampPawns;

        private string pawnPath = "Prefabs/PawnCamp";
        public void InitCamp(List<PawnAvatar> pawnAvatars)
        {
            this.campScene.SetActive(true);
            this.ReceivePawnFromBattle(pawnAvatars);
        }
        public void ReceivePawnFromBattle(List<PawnAvatar> pawnAvatars)
        {
            for(int i = 0; i < pawnAvatars.Count; i++)
            {
                this.allCampPawns.Add(this.GenerateCampPawn(pawnAvatars[i].pawnAgent.pawn));
            }
        }

        public PawnCamp GenerateCampPawn(Pawn pawn)
        {
            GameObject pawnCamp = Instantiate(Resources.Load(pawnPath)) as GameObject;
            pawnCamp.GetComponent<PawnCamp>().Init(pawn);
            if (pawn.campPosIndex < pawnsPos.Length)
                pawnCamp.transform.parent = pawnsPos[pawn.campPosIndex].transform;
            else
                pawnCamp.transform.parent = pawnsPos[0].transform;
            pawnCamp.transform.localPosition = new Vector3(0, 0, 0);

            return pawnCamp.GetComponent<PawnCamp>();
        }

        public void ResetPawnAvatars()
        {
            for (int i = 0; i < this.allCampPawns.Count; i++)
                DestroyImmediate(allCampPawns[i].gameObject);
            this.allCampPawns.Clear();
        }

    }
}