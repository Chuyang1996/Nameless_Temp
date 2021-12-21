using Nameless.Manager;
using Nameless.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nameless.DataUI
{
    public class BuildSelectUI : MonoBehaviour
    {
        public Image icon;
        public Button button;
        public BuildView buildView;
        public GameObject selectIcon;

        public int addAmmoBuild;
        public int addMedicineBuild;

        public int costMilitartRes;

        private string description;
        // Start is called before the first frame update
        void Start()
        {
            this.button.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayAudio(this.transform, AudioConfig.uiRemind);
                this.SelectBuild();
            });
        }

        public void Init(Sprite sprite, int costMilitartRes, int addAmmoBuild, int addMedicineBuild,string desc)
        {
            this.icon.sprite = sprite;
            this.description = desc;
            this.costMilitartRes = costMilitartRes;
            this.addAmmoBuild = addAmmoBuild;
            this.addMedicineBuild = addMedicineBuild;
        }

        public void SelectBuild()
        {
            this.buildView.ResetBuildSelect();
            this.buildView.currentBuild = this;
            this.buildView.ResetDescription(description, costMilitartRes);
            this.selectIcon.SetActive(true);
            if (FrontManager.Instance.localPlayer.GetMilitaryRes() < this.costMilitartRes)
            {
                this.buildView.setBtn.interactable = false;
            }
            else if ((this.buildView.currentArea.Ammo >= 1 && this.addAmmoBuild > 0))
            {
                this.buildView.setBtn.interactable = false;
            }
            else if ((this.buildView.currentArea.Medicine >= 1 && this.addMedicineBuild > 0))
            {
                this.buildView.setBtn.interactable = false;
            }
            else
            {
                this.buildView.setBtn.interactable = true;
            }
        }
        // Update is called once per frame
        
    }
}