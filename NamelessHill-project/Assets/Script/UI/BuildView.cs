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
                this.gameObject.SetActive(false);
            });
            this.setBtn.onClick.AddListener(() =>
            {
                this.currentArea.Ammo += this.currentBuild.addAmmoBuild;
                this.currentArea.Medicine += this.currentBuild.addMedicineBuild;
                int costAmmo = -this.currentBuild.costAmmo;
                int costMedicine = -this.currentBuild.costMedicine;
                List<Skill> skills = this.currentPawn.pawnAgent.skills;
                for(int i = 0;i< skills.Count; i++)
                {
                    PropertySkillEffect propertySkillEffect = skills[i].Execute(this.currentPawn,this.currentPawn);
                    costAmmo -= (int)propertySkillEffect.changeAmmo;
                    costMedicine -= (int)propertySkillEffect.changeMedicine;
                }
                GameManager.Instance.ChangeAmmo(costAmmo);
                GameManager.Instance.ChangeMedicine(costMedicine);
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
            Obj0.GetComponent<BuildSelectUI>().Init(this.medicineSprite,0, 0, 1, 50, "When your own unit is fighting in the area, if its health is lower than 20%, it will restore 50% of its maximum health");
            Obj1.GetComponent<BuildSelectUI>().Init(this.ammoSprite, 1, 100, 0, 0, "When your own unit is fighting in the area, when the ammunition is 0, it will automatically restore full ammunition");
            this.buildList.Add(Obj0.gameObject);
            this.buildList.Add(Obj1.gameObject);

            this.currentBuild = Obj0.GetComponent<BuildSelectUI>();
            this.currentBuild.SelectBuild();

        }

        public void ResetDescription(string txt, int costAmmo, int costMedicine)
        {
            for (int i = 0; i < this.costList.Count; i++)
                DestroyImmediate(this.costList[i]);
            this.costList.Clear();
            this.descTxt.text = "Med Kit: " + txt;

            if (costAmmo > 0) {
                Transform Obj0 = Instantiate(this.infoTemplate.transform, this.contentInfoSelect.transform);
                Obj0.gameObject.SetActive(true);
                Obj0.GetComponent<BuildInfoUI>().Init(ammoSprite,costAmmo);
                this.costList.Add(Obj0.gameObject);
            }
            if (costMedicine > 0)
            {
                Transform Obj0 = Instantiate(this.infoTemplate.transform, this.contentInfoSelect.transform);
                Obj0.gameObject.SetActive(true);
                Obj0.GetComponent<BuildInfoUI>().Init(medicineSprite, costMedicine);
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
            if (GameManager.Instance.totalAmmo < this.currentBuild.costAmmo || GameManager.Instance.totalAmmo < this.currentBuild.costMedicine)
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