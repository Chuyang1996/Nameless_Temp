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
    public enum GameScene
    {
        Battle =0,
        Camp = 1,
    }
    public class GameManager : SingletonMono<GameManager>
    {
        public bool isPlay = true;
        public GameScene GameScene;
        public Action<string, bool> RESULTEVENT;

        /// <summary>
        /// 临时数据
        /// </summary>
        /// 
        public int totalTime = 720;//本场战斗总时间//待修改
        public string levelgoalDes = "Hold for 12 hours";//本场战斗总时间//待修改
        public int totalMilitaryRes;//我方的补给数量//待修改
        public Action<int> TotalMilitartEvent;
        [HideInInspector]
        public int enemiesDieNum = 0;//敌人死亡数量//待修改

        /// <summary>
        /// 临时数据
        /// </summary>



        public AreasManager currentMap;
        public CharacterView characterView;
        public BattleView battleView;
        public CampView campView;
        public BuildView buildView;
        public EventView eventView;
        public ConversationView conversationView;
        public NoteBookView noteBookView;

        public InitArea[] areas;//待修改 所有玩家角色区域
        public List<PawnAvatar> curplayerPawns;//待修改 当前玩家角色

        public InitArea[] enemyAreas;//待修改 所有敌方角色区域
        public List<PawnAvatar> curenemyPawns;//待修改 当前敌方角色


        private List<Area> playerOccupyAreas = new List<Area>();//待修改 所有玩家占领的区域
        private List<Area> enemyOccupyAreas = new List<Area>();//待修改 所有敌方占领的区域

        // Start is called before the first frame update
        void Start()
        {
            this.isPlay = true;
            this.GameScene = GameScene.Battle;
            RTSCamera.Instance.InitCamera();
            PathManager.Instance.InitPath();
            DataManager.Instance.InitData();
            AudioManager.Instance.InitAudio();
            GenerateManager.Instance.InitMat();
            SpriteManager.Instance.InitTexturePackage();
            AreasManager.Instance.InitArea();
            PawnManager.Instance.InitPawns();
            NoteManager.Instance.InitNote();
            //Debug.Log(DataManager.Instance.GetCharacter(1001).name);


            this.battleView.InitBattle(this.totalTime, this.levelgoalDes, this.totalMilitaryRes);
            Time.timeScale = 1.0f;
            this.RESULTEVENT += this.Result;
            for(int i = 0; i < this.curplayerPawns.Count; i++)
            {
                this.curplayerPawns[i].characterView = this.characterView;
                this.curplayerPawns[i].currentArea = this.areas[i].GetArea();
                //this.areas[i].Init();
                this.areas[i].GetArea().AddPawn(this.curplayerPawns[i]);
                this.curplayerPawns[i].transform.position = this.areas[i].GetArea().centerNode.transform.position;
                this.curplayerPawns[i].Init(0);
                DialogueTriggerManager.Instance.TimeTriggerEvent += this.curplayerPawns[i].ReceiveCurrentTime;
                DialogueTriggerManager.Instance.CheckGameStartEvent(this.curplayerPawns[i]);
                PawnManager.Instance.AddPawnForFaction(this.curplayerPawns[i], false);
            }

            for(int i = 0; i < this.curenemyPawns.Count; i++)
            {
                this.curenemyPawns[i].characterView = this.characterView;
                this.curenemyPawns[i].currentArea = this.enemyAreas[i].GetArea();
                //this.enemyAreas[i].Init();
                this.enemyAreas[i].GetArea().AddPawn(this.curenemyPawns[i]);
                this.curenemyPawns[i].transform.position = this.enemyAreas[i].GetArea().centerNode.transform.position;
                this.curenemyPawns[i].Init(0);
                DialogueTriggerManager.Instance.TimeTriggerEvent += this.curenemyPawns[i].ReceiveCurrentTime;
                DialogueTriggerManager.Instance.CheckGameStartEvent(this.curenemyPawns[i]);
                PawnManager.Instance.AddPawnForFaction(this.curenemyPawns[i], true);
            }
            #region//获取本次场景里的事件
            List<EventResult> eventResults = new List<EventResult>();
            foreach (var child in DataManager.Instance.eventResultData)
            {
                eventResults.Add(EventResultFactory.GetEventResultById(child.Key));
            }
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

        public void ChangeMilitaryRes(int cost)
        {
            EventTriggerManager.Instance.CheckRelateMilitaryResEvent(cost);
            this.totalMilitaryRes += cost;
            if (this.TotalMilitartEvent != null)
                this.TotalMilitartEvent(this.totalMilitaryRes);
            //this.battleView.resourceInfoView.Init(this.totalMilitaryRes);
        }

        
        public void AddAreaForPlayer(Area area)//待修改 等写框架的时候改
        {
            if(!this.playerOccupyAreas.Contains(area))
                this.playerOccupyAreas.Add(area);
            if (this.enemyOccupyAreas.Contains(area))
                this.enemyOccupyAreas.Remove(area);
            area.ChangeColor(false);

            
        }
        public void AddAreaForEnemy(Area area)//待修改 等写框架的时候改
        {
            if (!this.enemyOccupyAreas.Contains(area))
                this.enemyOccupyAreas.Add(area);
            if (this.playerOccupyAreas.Contains(area))
                this.playerOccupyAreas.Remove(area);
            area.ChangeColor(true);
        }
        public bool IsBelongToSameSide(Area area,PawnAvatar pawn)//待修改 等写框架的时候改
        {
            if (pawn.isAI)
            {
                if (this.enemyOccupyAreas.Contains(area))
                    return true;
                else
                    return false;
            }
            else
            {
                if (this.playerOccupyAreas.Contains(area))
                    return true;
                else
                    return false;
            }
        }
        public void EnemiesKillNum(int num)//待修改 等写框架的时候改
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

        public void EnterCamp()
        {
           
            this.battleView.ExitBattle();//待修改 等UI管理器到位
            this.buildView.gameObject.SetActive(false);//待修改 等UI管理器到位
            this.characterView.gameObject.SetActive(false);//待修改 等UI管理器到位
            this.eventView.gameObject.SetActive(false);//待修改 等UI管理器到位
            this.GameScene = GameScene.Camp;
            RTSCamera.Instance.ResetCameraPos();
            CampManager.Instance.InitCamp(PawnManager.Instance.GetPawnAvatars(false),this.totalMilitaryRes);
            PawnManager.Instance.ClearAllPawn();
            
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