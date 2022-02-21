using Nameless.ConfigData;
using Nameless.Data;
using Nameless.DataMono;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Nameless.Manager
{
    public class CampManager : SingletonMono<CampManager>
    {

        public const string  campBgmName = "AmbienceLoop_Shelter_01";
        public CampData currentCampData;
        public Camp campScene;
        private List<PawnCamp> allCampPawns = new List<PawnCamp>();

        public Action<int> TotalMilitartEvent;
        public int totalMilitaryRes;

        private string campPath = "Prefabs/Camp/";
        public void InitCamp(CampData campData, List<Pawn> pawnAvatars,int militaryRes)
        {
            this.UpdateCampData(campData);
            GameObject camp = Instantiate(Resources.Load(this.campPath + campData.campName) as GameObject, this.transform);
            camp.transform.localPosition = new Vector3(0, 0, 0);
            this.campScene = camp.GetComponent<Camp>();
            this.campScene.InitCamp(pawnAvatars);
            this.ReceivePawnFromBattle(this.campScene.CampPawns());


            //¿ªÆôUI
            GameManager.Instance.campView.InitCamp(campData.descrption, militaryRes, pawnAvatars.Count);
        }
        public void UpdateCampData(CampData campData)
        {
            this.currentCampData = campData;
        }
        public void ActiveCamp()
        {
            GameManager.Instance.ClearBattle();
            this.campScene.gameObject.SetActive(true);
            this.campScene.ResetAllBtnState();
            AudioManager.Instance.PlayMusic(this.currentCampData.nameBgm);
        }
        public void ReceivePawnFromBattle(List<PawnCamp> pawnCamp)
        {
            this.allCampPawns.Clear();
            this.allCampPawns = pawnCamp;
        }

        public void ChangeMilitaryRes(int cost)
        {
            this.totalMilitaryRes += cost;
            if (this.TotalMilitartEvent != null)
                this.TotalMilitartEvent(this.totalMilitaryRes);
            //this.battleView.resourceInfoView.Init(this.totalMilitaryRes);
        }

        public List<PawnCamp> GetPawnCamps()
        {
            return this.allCampPawns;
        }
        public int GetMilitaryRes()
        {
            return this.totalMilitaryRes;
        }
        public void ClearCamp()
        {
            for (int i = 0; i < this.allCampPawns.Count; i++)
                DestroyImmediate(this.allCampPawns[i].gameObject);
            this.allCampPawns.Clear();

            if(this.campScene!=null)
                DestroyImmediate(this.campScene.gameObject);

        }

    }
}