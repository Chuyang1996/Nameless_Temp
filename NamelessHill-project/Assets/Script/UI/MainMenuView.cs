using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nameless.UI
{
    public class MainMenuView : MonoBehaviour
    {
        public GameObject mainMenuPanel;
        public GameObject optionPanel;

        public Button startBtn;
        public Button optionBtn;
        public Button exitBtn;
        public Button backBtn;

        public Toggle accessbilityToggle;

        private void Start()
        {
            this.startBtn.onClick.AddListener(this.NewStart);
            this.optionBtn.onClick.AddListener(this.Option);
            this.backBtn.onClick.AddListener(this.MainMenu);
            this.exitBtn.onClick.AddListener(this.Exit);
            this.accessbilityToggle.onValueChanged.AddListener(this.ActiveAccessbility);
        }

        public void NewStart()
        {
            AudioManager.Instance.PlayAudio(this.transform, AudioConfig.uiRemind);
            GameManager.Instance.EnterBattleWithTrans();
            this.gameObject.SetActive(false);
        }
        public void MainMenu()
        {
            AudioManager.Instance.PlayAudio(this.transform, AudioConfig.uiRemind);
            this.mainMenuPanel.SetActive(true);
            this.optionPanel.SetActive(false);
        }
        public void Option()
        {
            AudioManager.Instance.PlayAudio(this.transform, AudioConfig.uiRemind);
            this.mainMenuPanel.SetActive(false);
            this.optionPanel.SetActive(true);

        }
        public void ActiveAccessbility(bool value)
        {
            AudioManager.Instance.PlayAudio(this.transform, AudioConfig.uiRemind);
            Debug.Log(value);
            GameManager.Instance.accessbility = value;
        }
        public void Exit()
        {
            AudioManager.Instance.PlayAudio(this.transform, AudioConfig.uiRemind);
            Application.Quit();
        }
    }
}