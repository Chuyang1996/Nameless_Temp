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
        /// 临时数据
        /// </summary>
        /// 
        public int totalTime = 720;//本场战斗总时间//待修改
        public int totalAmmo;//我方的弹药数量//待修改
        public int totalMedicine;//我方的药品数量//待修改
        [HideInInspector]
        public int enemiesDieNum = 0;//敌人死亡数量//待修改

        /// <summary>
        /// 临时数据
        /// </summary>



        public AreasManager currentMap;
        public CharacterView characterView;
        public BattleView battleView;
        public BuildView buildView;
        public EventView eventView;

        public Area[] areas;//待修改 所有玩家角色区域
        public List<PawnAvatar> playerPawns;//待修改 所有玩家角色
        public Area[] enemyAreas;//待修改 所有敌方角色区域
        public List<PawnAvatar> enemyPawns;//待修改 所有敌方角色
 
        // Start is called before the first frame update
        void Start()
        {
            this.isPlay = true;
            RTSCamera.Instance.InitCamera();
            DataManager.Instance.InitData();
            AudioManager.Instance.InitAudio();
            MatManager.Instance.InitMat();
            //Debug.Log(DataManager.Instance.GetCharacter(1001).name);



            this.battleView.Init(this.totalTime);
            this.battleView.resourceInfoView.Init(this.totalAmmo, this.totalMedicine);
            Time.timeScale = 1.0f;
            this.RESULTEVENT += this.Result;
            for(int i = 0; i < this.playerPawns.Count; i++)
            {
                this.playerPawns[i].characterView = this.characterView;
                this.playerPawns[i].currentArea = this.areas[i];
                this.areas[i].Init();
                this.areas[i].AddPawn(this.playerPawns[i]);
                this.playerPawns[i].transform.position = this.areas[i].centerNode.transform.position;
                this.playerPawns[i].Init(0);
                DialogueTriggerManager.Instance.TimeTriggerEvent += this.playerPawns[i].ReceiveCurrentTime;
                DialogueTriggerManager.Instance.CheckGameStartEvent(this.playerPawns[i]);
            }

            for(int i = 0; i < this.enemyPawns.Count; i++)
            {
                this.enemyPawns[i].characterView = this.characterView;
                this.enemyPawns[i].currentArea = this.enemyAreas[i];
                this.enemyAreas[i].Init();
                this.enemyAreas[i].AddPawn(this.enemyPawns[i]);
                this.enemyPawns[i].transform.position = this.enemyAreas[i].centerNode.transform.position;
                this.enemyPawns[i].Init(0);
                DialogueTriggerManager.Instance.TimeTriggerEvent += this.enemyPawns[i].ReceiveCurrentTime;
                DialogueTriggerManager.Instance.CheckGameStartEvent(this.enemyPawns[i]);
            }
            #region//获取本次场景里的事件
            List<EventResult> eventResults = new List<EventResult>();
            eventResults.Add(EventResultFactory.GetEventResultById(101));
            eventResults.Add(EventResultFactory.GetEventResultById(102));
            eventResults.Add(EventResultFactory.GetEventResultById(103));
            eventResults.Add(EventResultFactory.GetEventResultById(104));
            eventResults.Add(EventResultFactory.GetEventResultById(105));
            EventTriggerManager.Instance.InitEventTrigger(eventResults);

            //开始监听事件
            StartCoroutine(EventTriggerManager.Instance.StartListenEvent());
            StartCoroutine(DialogueTriggerManager.Instance.StartListenEvent());
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
        public void EnemiesKillNum(int num)
        {
            EventTriggerManager.Instance.CheckRelateEnemyKillEvent(num);
            this.enemiesDieNum += num;
        }

        public void PlayCamera(Stack<DialoguePawn> pawns)
        {
            //TransitionTarget[] transitionTargets = new TransitionTarget[pawns.Count + 1];
            //for (int i = 0; i < pawns.Count; i++)
            //{
            //    Vector3 targetpos = new Vector3(pawns[i].transform.position.x, pawns[i].transform.position.y, -10.0f);
            //    transitionTargets[i] = new TransitionTarget(targetpos, 1.5f, 2.0f, 2.0f, 2.0f);
            //}
            //Vector3 pos = new Vector3(0, 0, -10.0f);
            //transitionTargets[pawns.Count] = new TransitionTarget(pos, 5.0f, 2.0f, 2.0f, 0.5f);
            RTSCamera.Instance.StartTransition(pawns);
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