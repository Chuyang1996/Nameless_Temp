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

        public int costAmmo;
        public int costMedicine;

        private string description;
        // Start is called before the first frame update
        void Start()
        {
            this.button.onClick.AddListener(() => 
            {
                this.SelectBuild();
            });
        }

        public void Init(Sprite sprite, int addAmmoBuild, int costAmmo, int addMedicineBuild,int costMedicine,string desc)
        {
            this.icon.sprite = sprite;
            this.description = desc;
            this.costAmmo = costAmmo;
            this.costMedicine = costMedicine;
            this.addAmmoBuild = addAmmoBuild;
            this.addMedicineBuild = addMedicineBuild;
        }

        public void SelectBuild()
        {
            this.buildView.ResetBuildSelect();
            this.buildView.currentBuild = this;
            this.buildView.ResetDescription(description, costAmmo, costMedicine);
            this.selectIcon.SetActive(true);
            if (GameManager.Instance.totalAmmo < this.costAmmo
            || GameManager.Instance.totalMedicine < this.costMedicine)
            {
                this.buildView.setBtn.interactable = false;
            }
            else if ((this.buildView.currentArea.Ammo >= 1 && this.addAmmoBuild > 0)
                || (this.buildView.currentArea.Medicine >= 1 && this.addMedicineBuild > 0))
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