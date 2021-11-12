using Nameless.Agent;
using Nameless.Controller;
using Nameless.Data;
using Nameless.DataMono;
using Nameless.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.Manager {
    public class GameManager : SingletonMono<GameManager>
    {
        public bool isPlay = true;

        public Action<string, bool> RESULTEVENT;

        /// <summary>
        /// ��ʱ����
        /// </summary>
        /// 
        public int totalTime = 720;//���޸�
        public int totalAmmo;//���޸�
        public int totalMedicine;//���޸�


        /// <summary>
        /// ��ʱ����
        /// </summary>



        public AreasManager currentMap;
        public CharacterView characterView;
        public BattleView battleView;
        public BuildView buildView;
        public EventView eventView;
        public Area[] areas;
        public PawnAvatar[] pawns;
        // Start is called before the first frame update
        void Start()
        {
            this.isPlay = true;
            DataManager.Instance.InitData();
            //Debug.Log(DataManager.Instance.GetCharacter(1001).name);
            AudioManager.Instance.InitAudio();
            MatManager.Instance.InitMat();


            #region//��ȡ���γ�������¼�
            List<EventResult> eventResults = new List<EventResult>();
            eventResults.Add( EventResultFactory.GetEventResultById(101));
            eventResults.Add( EventResultFactory.GetEventResultById(102));
            eventResults.Add( EventResultFactory.GetEventResultById(103));
            eventResults.Add( EventResultFactory.GetEventResultById(104));
            eventResults.Add( EventResultFactory.GetEventResultById(105));
            EventTriggerManager.Instance.InitEventTrigger(eventResults);

            //��ʼ�����¼�
            StartCoroutine(EventTriggerManager.Instance.StartListenEvent());
            #endregion

            this.battleView.Init(this.totalTime);
            this.battleView.resourceInfoView.Init(this.totalAmmo, this.totalMedicine);
            Time.timeScale = 1.0f;
            this.RESULTEVENT += this.Result;
            for(int i = 0; i < this.pawns.Length; i++)
            {
                this.pawns[i].characterView = this.characterView;
                this.pawns[i].currentArea = this.areas[i];
                this.areas[i].Init();
                this.areas[i].AddPawn(this.pawns[i]);
                this.pawns[i].transform.position = this.areas[i].centerNode.transform.position;
                this.pawns[i].Init();
            }


            #region//�����������ʱ
            RTSCamera.Instance.InitCamera();
            TransitionTarget[] transitionTargets = new TransitionTarget[pawns.Length+1];
            for(int i = 0; i < pawns.Length; i++)
            {
                Vector3 targetpos = new Vector3(pawns[i].transform.position.x, pawns[i].transform.position.y, - 10.0f);
                transitionTargets[i] = new TransitionTarget(targetpos, 1.5f, 2.0f, 2.0f, 2.0f);
            }
            Vector3 pos = new Vector3(0, 0, -10.0f);
            transitionTargets[pawns.Length] = new TransitionTarget(pos, 5.0f, 2.0f, 2.0f, 0.5f);
            RTSCamera.Instance.StartTransition(transitionTargets,false);
            #endregion
        }

        private void Result(string title, bool isWin)
        {
            this.battleView.resultInfoView.SetResultTxt(title, isWin);
        }

        public void ChangeAmmo(int cost)
        {
            EventTriggerManager.Instance.CheckRelateAmmoEvent(cost);
            this.totalAmmo += cost;
            this.battleView.resourceInfoView.Init(this.totalAmmo, this.totalMedicine);
        }
        public void ChangeMedicine(int cost)
        {
            EventTriggerManager.Instance.CheckRelateMedicineEvent(cost);
            this.totalMedicine += cost;
            this.battleView.resourceInfoView.Init(this.totalAmmo, this.totalMedicine);
        }

        public void PauseOrPlay(bool isPlay)
        {
            this.isPlay = isPlay;
            if (this.isPlay)
                Time.timeScale = 1.0f;
            else
                Time.timeScale = 0.0f;
        }
    }
}