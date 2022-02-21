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

        public Button contiuneBtn;
        public Button startBtn;
        public Button optionBtn;
        public Button exitBtn;
        public Button backBtn;

        public Toggle accessbilityToggle;
        public Slider musicSlider;
        public Slider soundSlider;

        private void Start()
        {
            this.contiuneBtn.onClick.AddListener(this.LoadGame);
            this.startBtn.onClick.AddListener(this.NewStart);
            this.optionBtn.onClick.AddListener(this.Option);
            this.backBtn.onClick.AddListener(this.MainMenuForSubMenu);
            this.exitBtn.onClick.AddListener(this.Exit);
            this.accessbilityToggle.onValueChanged.AddListener(this.ActiveAccessbility);
            this.musicSlider.value = AudioManager.Instance.MusicVolume;
            this.soundSlider.value = AudioManager.Instance.SoundVolume;
            this.musicSlider.onValueChanged.AddListener((float value) => { AudioManager.Instance.MusicVolume = value; });
            this.soundSlider.onValueChanged.AddListener((float value) => { AudioManager.Instance.SoundVolume = value; });

            this.contiuneBtn.interactable = SaveManager.Instance.IfSaveExist();
        }
        public void LoadGame()
        {
            AudioManager.Instance.PlayAudio(this.transform, AudioConfig.uiRemind);
            GameManager.Instance.LoadOldGame();
            this.gameObject.SetActive(false);
        }
        public void NewStart()
        {
            AudioManager.Instance.PlayAudio(this.transform, AudioConfig.uiRemind);
            GameManager.Instance.StartNewGame();
            this.gameObject.SetActive(false);
        }
        public void MainMenuForSubMenu()
        {
            AudioManager.Instance.PlayAudio(this.transform, AudioConfig.uiRemind);
            this.contiuneBtn.interactable = SaveManager.Instance.IfSaveExist();
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

        public void MainMenuFromOtherScene()
        {
            this.contiuneBtn.interactable = SaveManager.Instance.IfSaveExist();
        }
    }
}