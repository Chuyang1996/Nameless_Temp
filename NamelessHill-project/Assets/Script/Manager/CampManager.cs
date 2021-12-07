using Nameless.Data;
using Nameless.DataMono;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Nameless.Manager
{
    public class CampManager : SingletonMono<CampManager>
    {

        public const string  campBgmName = "AmbienceLoop_Shelter_01";
        public GameObject campScene;
        public GameObject[] pawnsPos;
        public List<PawnCamp> allCampPawns = new List<PawnCamp>();


        public GameObject noteBtn;
        public GameObject lightBtn;



        private string pawnPath = "Prefabs/PawnCamp";
        public void InitCamp(List<PawnAvatar> pawnAvatars,int militaryRes)
        {
            this.ReceivePawnFromBattle(pawnAvatars);
            GameManager.Instance.campView.InitCamp(militaryRes, pawnAvatars.Count);
        }
        public void ActiveCamp()
        {
            GameManager.Instance.ClearBattle();
            this.campScene.SetActive(true);
            AudioManager.Instance.PlayMusic(campBgmName);
        }
        public void ReceivePawnFromBattle(List<PawnAvatar> pawnAvatars)
        {
            for(int i = 0; i < pawnAvatars.Count; i++)
            {
                this.allCampPawns.Add(this.GenerateCampPawn(pawnAvatars[i]));
            }
        }

        public PawnCamp GenerateCampPawn(PawnAvatar pawn)
        {
            GameObject pawnCamp = Instantiate(Resources.Load(pawnPath)) as GameObject;
            pawnCamp.GetComponent<PawnCamp>().Init(pawn.pawnAgent.pawn, pawn.pawnAgent.conversations);
            if (pawn.pawnAgent.pawn.campPosIndex < this.pawnsPos.Length)
                pawnCamp.transform.parent = pawnsPos[pawn.pawnAgent.pawn.campPosIndex].transform;
            else
                pawnCamp.transform.parent = pawnsPos[0].transform;
            pawnCamp.transform.localPosition = new Vector3(0, 0, 0);

            return pawnCamp.GetComponent<PawnCamp>();
        }

        public PawnCamp FindPawnInCamp(long id)
        {
            PawnCamp pawnCamp = this.allCampPawns.Where(_pawn => _pawn.pawn.id == id).FirstOrDefault();
            return pawnCamp;
        }

        public void ClearCamp()
        {
            for (int i = 0; i < this.allCampPawns.Count; i++)
                DestroyImmediate(allCampPawns[i].gameObject);
            this.allCampPawns.Clear();

        }

    }
}