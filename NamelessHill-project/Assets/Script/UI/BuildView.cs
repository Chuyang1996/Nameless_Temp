using Nameless.Data;
using Nameless.DataMono;
using Nameless.DataUI;
using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nameless.UI
{
    public class BuildView : MonoBehaviour
    {
        public Sprite ammoSprite;
        public Sprite medicineSprite;

        public Sprite ammoCostSprite;
        public Sprite medicineCostSprite;


        public GameObject contentBuildSelect;
        public GameObject contentInfoSelect;

        public Button setBtn;
        public Button closeBtn;

        public BuildSelectUI buildTemplate;
        public BuildInfoUI infoTemplate;

        private List<GameObject> buildList = new List<GameObject>();
        private List<GameObject> costList = new List<GameObject>();
        private PawnAvatar currentPawn;

        public Area currentArea;
        public BuildSelectUI currentBuild;
        public Text descTxt;
        // Start is called before the first frame update
        void Start()
        {
            this.closeBtn.onClick.AddListener(() => 
            {
                GameManager.Instance.PauseOrPlay(true);
                this.gameObject.SetActive(false);
            });
            this.setBtn.onClick.AddListener(() =>
            {
                this.currentArea.Ammo += this.currentBuild.addAmmoBuild;
                this.currentArea.Medicine += this.currentBuild.addMedicineBuild;
                int costMilitaryRes = -this.currentBuild.costMilitartRes;
                List<Skill> skills = this.currentPawn.pawnAgent.skills;
                for(int i = 0;i< skills.Count; i++)
                {
                    PropertySkillEffect propertySkillEffect = skills[i].Execute(this.currentPawn,this.currentPawn);
                    costMilitaryRes -= (int)propertySkillEffect.changeAmmo;
                }
                GameManager.Instance.ChangeMilitaryRes(costMilitaryRes);
                this.setBtn.interactable = this.IsSetBtnActiveAfterClick();
            });
        }

        public void ResetBuild(PawnAvatar pawn)
        {
            for (int i = 0; i < this.buildList.Count; i++)
                DestroyImmediate(this.buildList[i]);
            this.buildList.Clear();

            this.currentArea = pawn.currentArea;
            this.currentPawn = pawn;
            Transform Obj0 = Instantiate(this.buildTemplate.transform, this.contentBuildSelect.transform);
            Transform Obj1 = Instantiate(this.buildTemplate.transform, this.contentBuildSelect.transform);
            Obj0.gameObject.SetActive(true);
            Obj1.gameObject.SetActive(true);
            Obj0.GetComponent<BuildSelectUI>().Init(this.medicineSprite,50,0, 1,  "When your own unit is fighting in the area, if its health is lower than 20%, it will restore 50% of its maximum health");
            Obj1.GetComponent<BuildSelectUI>().Init(this.ammoSprite, 100,1, 0,  "When your own unit is fighting in the area, when the ammunition is 0, it will automatically restore full ammunition");
            this.buildList.Add(Obj0.gameObject);
            this.buildList.Add(Obj1.gameObject);

            this.currentBuild = Obj0.GetComponent<BuildSelectUI>();
            this.currentBuild.SelectBuild();

        }

        public void ResetDescription(string txt, int costMilitary)
        {
            for (int i = 0; i < this.costList.Count; i++)
                DestroyImmediate(this.costList[i]);
            this.costList.Clear();
            this.descTxt.text = "Med Kit: " + txt;

            if (costMilitary > 0) {
                Transform Obj0 = Instantiate(this.infoTemplate.transform, this.contentInfoSelect.transform);
                Obj0.gameObject.SetActive(true);
                Obj0.GetComponent<BuildInfoUI>().Init(this.ammoCostSprite,costMilitary);
                this.costList.Add(Obj0.gameObject);
            }
        }

        public void ResetBuildSelect()
        {
            for (int i = 0; i < this.buildList.Count; i++)
            {
                this.buildList[i].GetComponent<BuildSelectUI>().selectIcon.gameObject.SetActive(false);
            }
        }


        private bool IsSetBtnActiveAfterClick()
        {
            if (GameManager.Instance.totalMilitaryRes < this.currentBuild.costMilitartRes)
            {
                return false;
            }
            else if ((this.currentArea.Ammo >= 1 && this.currentBuild.addAmmoBuild > 0) || (this.currentArea.Medicine >= 1 && this.currentBuild.addMedicineBuild > 0))
            {
                return false;
            }
            else
                return true;
 
        }

    }
}