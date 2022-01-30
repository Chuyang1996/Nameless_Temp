using Nameless.Data;
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
        public Build build;

        // Start is called before the first frame update
        void Start()
        {
            this.button.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayAudio(this.transform, AudioConfig.uiRemind);
                this.SelectBuild();
            });
        }

        public void Init(Build build)
        {
            this.icon.sprite = build.sprite;
            this.build = build;
        }

        public void SelectBuild()
        {
            this.buildView.ResetBuildSelect();
            this.buildView.currentBuild = this;
            this.buildView.ResetDescription(this.build.description, this.build.resCost);
            this.selectIcon.SetActive(true);
            if (FrontManager.Instance.localPlayer.GetMilitaryRes() < this.build.resCost)
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