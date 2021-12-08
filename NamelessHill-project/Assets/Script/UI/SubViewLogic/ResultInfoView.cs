using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Nameless.UI {
    public class ResultInfoView : MonoBehaviour
    {
        public Text resultTxt;
        public Button resumeBtn;
        public Button enterCampBtn;
        public Button restartBtn;
        public Button optionBtn;
        public Button mainBtn;
        public Button exitBtn;


        public Button backBtn;
        public Slider musicSlider;
        public Slider soundSlider;

        public GameObject resultPanel;
        public GameObject optionPanel;
        // Start is called before the first frame update
        void Start()
        {
            this.restartBtn.onClick.AddListener(this.RestartBattle);
            this.resumeBtn.onClick.AddListener(this.BackToGame);
            this.enterCampBtn.onClick.AddListener(this.EnterCamp);
            this.optionBtn.onClick.AddListener(this.OptionPanel);
            this.mainBtn.onClick.AddListener(this.MainMenuPanel);
            this.exitBtn.onClick.AddListener(this.ExitGame);


            this.backBtn.onClick.AddListener(this.BackToResultPanel);
            this.musicSlider.value = AudioManager.Instance.MusicVolume;
            this.soundSlider.value = AudioManager.Instance.SoundVolume;
            this.musicSlider.onValueChanged.AddListener((float value) => { AudioManager.Instance.MusicVolume = value; });
            this.soundSlider.onValueChanged.AddListener((float value) => { AudioManager.Instance.SoundVolume = value; });
        }

        public void SetResultTxt(string result, bool isWin)
        {
            StopAllCoroutines();
            this.gameObject.SetActive(true);
            this.resultTxt.text = result;
            this.resultTxt.color = isWin ? Color.green : Color.red;
            string audioFile = isWin ? "Win" : "Lose";
            AudioManager.Instance.PlayAudio(transform, audioFile);
            if (isWin)
                this.WinPanelShow();
            else
                this.LosePanelShow();
        }
        public void PausePanelShow()
        {
            AudioManager.Instance.PlayAudio(this.transform, AudioConfig.uiRemind);
            Time.timeScale = 0.0f; 
            this.gameObject.SetActive(true);
            this.resultTxt.text = "Pause";
            this.resultTxt.color = Color.black;
            this.restartBtn.gameObject.SetActive(false);
            this.resumeBtn.gameObject.SetActive(true);
            this.enterCampBtn.gameObject.SetActive(false);
            this.optionBtn.gameObject.SetActive(true);
            this.mainBtn.gameObject.SetActive(true);
            this.exitBtn.gameObject.SetActive(true);
        }
        private void WinPanelShow()
        {
            Time.timeScale = 0.0f;
            this.restartBtn.gameObject.SetActive(false);
            this.resumeBtn.gameObject.SetActive(false);
            this.enterCampBtn.gameObject.SetActive(true);
            this.optionBtn.gameObject.SetActive(false);
            this.mainBtn.gameObject.SetActive(false);
            this.exitBtn.gameObject.SetActive(false);
        }


        private void LosePanelShow()
        {
            Time.timeScale = 0.0f;
            this.restartBtn.gameObject.SetActive(true);
            this.resumeBtn.gameObject.SetActive(false);
            this.enterCampBtn.gameObject.SetActive(false);
            this.optionBtn.gameObject.SetActive(true);
            this.mainBtn.gameObject.SetActive(true);
            this.exitBtn.gameObject.SetActive(true);
        }



        private void BackToGame()
        {
            AudioManager.Instance.PlayAudio(this.transform, AudioConfig.uiRemind);
            Time.timeScale = 1.0f;
            this.gameObject.SetActive(false);
        }
        private void RestartBattle()
        {
            AudioManager.Instance.PlayAudio(this.transform, AudioConfig.uiRemind); 
            Time.timeScale = 1.0f; 
            GameManager.Instance.RestartBattle();
        }
        private void EnterCamp()
        {
            AudioManager.Instance.PlayAudio(this.transform, AudioConfig.uiRemind); 
            Time.timeScale = 1.0f; 
            GameManager.Instance.EnterCamp();
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