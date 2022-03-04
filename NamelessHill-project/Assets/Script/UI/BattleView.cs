using Nameless.Controller;
using Nameless.DataMono;
using Nameless.DataUI;
using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nameless.UI
{
    public class BattleView : MonoBehaviour
    {
        public Button pauseBtn;
        #region
        public ResourceInfoView resourceInfoView;
        public ResourceShow resourceShow;
        public TipInfoView tipInfoView;
        public MouseFollowView mouseFollow;
        #endregion

        #region//关卡结果
        public ResultInfoView resultInfoView;
        #endregion

        #region//区域信息
        public Sprite medicineImg;
        public Sprite ammoImg;
        public Text golaDes;
        //public GameObject contentTxt;
        //public Text areaNameTxt;
        //public BuildInfoUI templateUI;
        //private Area currentArea;
        //private List<GameObject> txtInfoList = new List<GameObject>();
        #endregion

        #region//计时间
        public int totalTime;
        //public Button gamePauseBtn;

        //public Sprite pauseIcon;
        //public Sprite playIcon;

        private bool isPlay = false;

        private float seconds;
        private int minute;
        private int hour;
        #endregion
        // Start is called before the first frame update
        private void Start()
        {
            this.pauseBtn.onClick.AddListener(this.resultInfoView.PausePanelShow);
        }
        public void InitBattle(int totalTime, int militartRes)
        {
            this.gameObject.SetActive(true);
            this.resultInfoView.gameObject.SetActive(false);
            this.tipInfoView.InitTipInfo();
            this.resourceInfoView.Init(militartRes);
            FrontManager.Instance.localPlayer.TotalMilitartEvent += this.resourceShow.ShowResChange;
            this.totalTime = totalTime;
            
            string shour = this.totalTime / 60 > 10 ? (this.totalTime / 60).ToString() : "0" + (this.totalTime / 60).ToString();
            string sminute = this.totalTime % 60 > 10 ? (this.totalTime % 60).ToString() : "0" + (this.totalTime % 60).ToString();
            this.golaDes.text = "for " + shour + " h " + sminute + " m "; 
            this.seconds = 0.0f;
            this.minute = 0;
            this.hour = 0;

            this.isPlay = true;
            //this.gamePauseBtn.image.sprite = pauseIcon;
            //this.gamePauseBtn.onClick.AddListener(() => 
            //{ 
            //    this.isPlay = !this.isPlay;
            //    if (isPlay)
            //    {
            //        GameManager.Instance.PauseOrPlay(true);
            //    }
            //    else
            //    {
            //        this.gamePauseBtn.image.sprite = playIcon;
            //        GameManager.Instance.PauseOrPlay(false);
            //    } 
            //});
        
        }

        // Update is called once per frame
        void Update()
        {
            if (!this.isPlay || RTSCamera.Instance._isTranstionTo)
                return;
            //this.SelectArea();
            this.CountTime();
            
        }
        //void SelectArea()
        //{
        //    Vector2 raySelect = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    RaycastHit2D hit = Physics2D.Raycast(raySelect, Vector2.zero);
        //    if (hit.collider != null && hit.collider.gameObject.tag == "Area")
        //    {
        //        if (hit.collider.gameObject.GetComponent<Area>() != null && hit.collider.gameObject.GetComponent<Area>()!=this.currentArea)
        //        {
        //            this.currentArea = hit.collider.gameObject.GetComponent<Area>();
        //            this.ResetAreaInfo();
        //        }
        //    }
        //}
        void CountTime()
        {
            if (this.seconds > 60.0f)
            {

                this.seconds = 0.0f;
                this.totalTime--;
                EventTriggerManager.Instance.CheckRelateTimeEvent(this.totalTime, FrontManager.Instance.localPlayer);
                //DialogueTriggerManager.Instance.CheckTimeTriggerEvent(this.totalTime);
                this.hour = this.totalTime / 60;
                this.minute = this.totalTime % 60;
                string minTxt = minute > 9 ? minute.ToString() : "0" + minute.ToString();
                string hourTxt = hour > 9 ? hour.ToString() : "0" + hour.ToString();
                this.golaDes.text = "for " + hourTxt + " h " + minTxt + " m ";
                if(this.totalTime  <= 0)
                {
                    Time.timeScale = 0.0f;
                    GameManager.Instance.RESULTEVENT("You Win!!", true);

                }
            }
            else
            {
                this.seconds += (Time.deltaTime * 48);
            }
        }

        public void ExitBattle()
        {
            if(FrontManager.Instance.localPlayer!=null)
                FrontManager.Instance.localPlayer.TotalMilitartEvent -= this.resourceShow.ShowResChange;
            this.gameObject.SetActive(false);
        }
        //void ResetAreaInfo()
        //{
        //    for(int i = 0;i<this.txtInfoList.Count;i++)
        //        DestroyImmediate(this.txtInfoList[i]);
        //    this.txtInfoList.Clear();

        //    if (this.currentArea != null)
        //    {
        //        this.areaNameTxt.text ="Area:"+ this.currentArea.gameObject.name;
        //        Transform txtObj0 = Instantiate(this.templateUI.transform, this.contentTxt.transform);
        //        txtObj0.gameObject.GetComponent<BuildInfoUI>().Init(this.medicineImg, this.currentArea.Medicine);
        //        txtObj0.gameObject.SetActive(true);
        //        Transform txtObj1 = Instantiate(this.templateUI.transform, this.contentTxt.transform);
        //        txtObj1.gameObject.GetComponent<BuildInfoUI>().Init(this.ammoImg, this.currentArea.Ammo);
        //        txtObj1.gameObject.SetActive(true);
        //        this.txtInfoList.Add(txtObj0.gameObject);
        //        this.txtInfoList.Add(txtObj1.gameObject);
        //        //if (this.currentArea.type == AreaType.Normal || this.currentArea.type == AreaType.Spawn)
        //        //{
        //        //    Transform txtObj0 = Instantiate(this.templateUI.transform, this.contentTxt.transform);

        //        //    txtObj0.gameObject.GetComponent<Text>().text = "Morale:     " + this.currentArea.costMorale + "/" + this.currentArea.costTimeMorale + "s";

        //        //    txtObj0.gameObject.SetActive(true);

        //        //    this.txtInfoList.Add(txtObj0.gameObject);

        //        //}
        //        //else if (this.currentArea.type == AreaType.Base)
        //        //{
        //        //    Transform txtObj0 = Instantiate(this.templateUI.transform, this.contentTxt.transform);

        //        //    BaseArea tempBase = (BaseArea)this.currentArea;
        //        //    txtObj0.gameObject.GetComponent<Text>().text = "Ammo:     " + "+"+ tempBase.supportAmmo + "/" + tempBase.supportDeltaTime + "s";

        //        //    txtObj0.gameObject.SetActive(true);

        //        //    this.txtInfoList.Add(txtObj0.gameObject);

        //        //}
        //    }
            
        //}
    }
}