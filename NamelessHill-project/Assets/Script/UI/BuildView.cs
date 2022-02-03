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
        public string medicineString;
        public string ammoString;

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
                AudioManager.Instance.PlayAudio(this.transform, AudioConfig.uiRemind);
            });
            this.setBtn.onClick.AddListener(() =>//待修改 改成判断当前建造的东西是啥
            {
                int costMilitaryRes = -this.currentBuild.build.resCost ;
                FrontManager.Instance.localPlayer.ChangeMilitaryRes(costMilitaryRes);

                this.setBtn.interactable = this.IsSetBtnActiveAfterClick();
                //GameManager.Instance.PauseOrPlay(true);
                this.gameObject.SetActive(false);
                
                AudioManager.Instance.PlayAudio(this.transform, AudioConfig.buildEnd);
                PlayerController.Instance.FindAllBuildingArea(this.currentArea, this.currentBuild.build, this.currentPawn);
            });
        }

        public void ResetBuild(PawnAvatar pawn)
        {
            for (int i = 0; i < this.buildList.Count; i++)
                DestroyImmediate(this.buildList[i]);
            this.buildList.Clear();

            this.currentArea = pawn.currentArea;
            this.currentPawn = pawn;
            List<Skill> skills = pawn.pawnAgent.GetSkills();
            for(int i = 0; i < skills.Count; i++)
            {
                if(skills[i] is BuildSkill)
                {
                    BuildSkill buildSkill = (BuildSkill)skills[i];
                    Transform Obj = Instantiate(this.buildTemplate.transform, this.contentBuildSelect.transform);
                    Obj.gameObject.SetActive(true);
                    Obj.GetComponent<BuildSelectUI>().Init(buildSkill.build);
                    this.buildList.Add(Obj.gameObject);
                }
            }

            if (this.buildList.Count > 0)
            {
                this.currentBuild = this.buildList[0].GetComponent<BuildSelectUI>();
                this.currentBuild.SelectBuild();
                this.setBtn.gameObject.SetActive(true);
            }
            else
            {
                this.setBtn.gameObject.SetActive(false);
            }

        }

        public void ResetDescription(string txt, int costMilitary)
        {
            for (int i = 0; i < this.costList.Count; i++)
                DestroyImmediate(this.costList[i]);
            this.costList.Clear();
            this.descTxt.text =  txt;

            if (costMilitary >= 0) {
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
            if (FrontManager.Instance.localPlayer.GetMilitaryRes() < this.currentBuild.build.resCost)
                return false;
            else
                return true;
 
        }

    }
}