using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nameless.UI
{
    public class CampView :  SelectViewLogic
    {

        public Animation animation;

        public AnimationClip enterAnim;
        public AnimationClip exitAnim;



        public Text resourceText;
        public Text pawnNum;


        public Button pauseBtn;

        public Button resumeBtn;
        public Button optionBtn;
        public Button mainBtn;
        public Button exitBtn;


        public Button backBtn;
        public Slider musicSlider;
        public Slider soundSlider;

        public GameObject pausePanel;
        public GameObject resultPanel;
        public GameObject optionPanel;

        private void Start()
        {
            this.pauseBtn.onClick.AddListener(this.PausePanelShow);
            this.resumeBtn.onClick.AddListener(this.BackToGame);
            this.optionBtn.onClick.AddListener(this.OptionPanel);
            this.mainBtn.onClick.AddListener(this.MainMenuPanel);
            this.exitBtn.onClick.AddListener(this.ExitGame);


            this.backBtn.onClick.AddListener(this.BackToResultPanel);
            this.musicSlider.value = AudioManager.Instance.MusicVolume;
            this.soundSlider.value = AudioManager.Instance.SoundVolume;
            this.musicSlider.onValueChanged.AddListener((float value) => { AudioManager.Instance.MusicVolume = value; });
            this.soundSlider.onValueChanged.AddListener((float value) => { AudioManager.Instance.SoundVolume = value; });
        }
        public void InitCamp(int militartRes, int pawnNum)
        {
            this.gameObject.SetActive(true);
            this.InitMilitRes(militartRes);
            GameManager.Instance.TotalMilitartEvent += this.InitMilitRes;
            this.InitPawnInfo(pawnNum);
            this.animation.clip = enterAnim;
            this.animation.Play();
        }

        public void ExitCamp()
        {
            GameManager.Instance.TotalMilitartEvent -= this.InitMilitRes;
        }

        public void EnterCampEvent()
        {
            CampManager.Instance.ActiveCamp();
        }

        public void ExitCampEvent()
        {

        }
        public void InitPawnInfo(int value)
        {
            this.pawnNum.text = value.ToString() + " " + "Members Left";
        }
        public void InitMilitRes(int value)
        {
            this.resourceText.text = value.ToString();
        }



        private void PausePanelShow()
        {
            AudioManager.Instance.PlayAudio(this.transform, AudioConfig.uiRemind);
            Time.timeScale = 0.0f;
            this.pausePanel.gameObject.SetActive(true);
        }
        private void BackToGame()
        {
            AudioManager.Instance.PlayAudio(this.transform, AudioConfig.uiRemind);
            Time.timeScale = 1.0f;
            this.pausePanel.gameObject.SetActive(false);
        }
        private void OptionPanel()
        {
            AudioManager.Instance.PlayAudio(this.transform, AudioConfig.uiRemind);
            this.resultPanel.SetActive(false);
            this.optionPanel.SetActive(true);
        }
        private void MainMenuPanel()
        {
            AudioManager.Instance.PlayAudio(this.transform, AudioConfig.uiRemind);
            Time.timeScale = 1.0f;
            this.gameObject.SetActive(false);
            GameManager.Instance.BackToMainMenu();
        }

        private void ExitGame()
        {
            AudioManager.Instance.PlayAudio(this.transform, AudioConfig.uiRemind);
            Application.Quit();
        }

        private void BackToResultPanel()
        {
            AudioManager.Instance.PlayAudio(this.transform, AudioConfig.uiRemind);
            this.resultPanel.SetActive(true);
            this.optionPanel.SetActive(false);
        }


    } 
}