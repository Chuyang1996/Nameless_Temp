using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Nameless.UI {
    public class ResultInfoView : MonoBehaviour
    {
        public Text resultTxt;
        public Button restartBtn;
        // Start is called before the first frame update
        void Start()
        {
            this.restartBtn.onClick.AddListener(() => { Time.timeScale = 1.0f;  });
        }

        public void SetResultTxt(string result, bool isWin)
        {
            StopAllCoroutines();
            this.restartBtn.onClick.RemoveAllListeners();
            Time.timeScale = 0.0f;
            this.gameObject.SetActive(true);
            this.resultTxt.text = result;
            this.resultTxt.color = isWin ? Color.green : Color.red;
            string audioFile = isWin ? "Win" : "Lose";
            AudioManager.Instance.PlayAudio(transform, audioFile);
            if (isWin)
                this.restartBtn.onClick.AddListener(() => { Time.timeScale = 1.0f; GameManager.Instance.EnterCamp(); });
            else
                this.restartBtn.onClick.AddListener(() => { Time.timeScale = 1.0f; GameManager.Instance.RestartBattle(); });
        } 
    }
}