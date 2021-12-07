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
        Menu =0,
        Battle = 1,
        Camp = 2,
    }
    public class GameManager : SingletonMono<GameManager>
    {
        public bool isPlay = true;
        public GameScene GameScene = GameScene.Menu;
        public bool accessbility = false;
        public Action<string, bool> RESULTEVENT;
        public Action<int> TotalMilitartEvent;
        /// <summary>
        /// 临时数据
        /// </summary>
        /// 
        public const string mainMenuBgmName = "Music_MainBGM_01";
        public int totalTime = 720;//本场战斗总时间//待修改
        public string levelgoalDes = "Hold for 12 hours";//本场战斗总时间//待修改
        public int totalMilitaryRes;//我方的补给数量//待修改

        [HideInInspector]
        public int enemiesDieNum = 0;//敌人死亡数量//待修改

        /// <summary>
        /// 临时数据
        /// </summary>



        public CharacterView characterView;
        public BattleView battleView;
        public CampView campView;
        public BuildView buildView;
        public EventView eventView;
        public ConversationView conversationView;
        public NoteBookView noteBookView;




        private List<Area> playerOccupyAreas = new List<Area>();//待修改 所有玩家占领的区域
        private List<Area> enemyOccupyAreas = new List<Area>();//待修改 所有敌方占领的区域

        // Start is called before the first frame update
        void Start()
        {
            this.isPlay = true;
            this.GameScene = GameScene.Menu;
            RTSCamera.Instance.InitCamera();
            PathManager.Instance.InitPath();
            DataManager.Instance.InitData();
            AudioManager.Instance.InitAudio();
            GenerateManager.Instance.InitMat();
            SpriteManager.Instance.InitTexturePackage();
            PawnManager.Instance.InitPawns();
            NoteManager.Instance.InitNote();
            MapManager.Instance.InitMap(DataManager.Instance.GetMapData(0));
            AudioConfig.Init();

            RTSCamera.Instance.ResetCameraPos();
            AudioManager.Instance.PlayMusic(mainMenuBgmName);
        }

        public void EnterBattleWithTrans()
        {
            MapManager.Instance.GenerateTransInfoShow(MapManager.Instance.currentMapData);
            MapManager.Instance.currentTransInfoShow.StartReader();
        }
        public void InitBattle()
        {
            this.GameScene = GameScene.Battle;
            MapManager.Instance.GenerateMap(MapManager.Instance.currentMapData);
            MapManager.Instance.currentMap.InitArea();
            List<EventResult> eventResults = new List<EventResult>();
            foreach (var child in DataManager.Instance.eventResultData)
            {
                eventResults.Add(EventResultFactory.GetEventResultById(child.Key));
            }
            EventTriggerManager.Instance.InitEventTrigger(eventResults);
            //Debug.Log(DataManager.Instance.GetCharacter(1001).name);
            StartCoroutine(EventTriggerManager.Instance.StartListenEvent());

            this.battleView.InitBattle(this.totalTime, this.levelgoalDes, this.totalMilitaryRes);
            Time.timeScale = 1.0f;
            this.RESULTEVENT += this.Result;

            PawnManager.Instance.AddPawnOnArea(1001, MapManager.Instance.currentMap.FindAreaByLocalId(38), 0, false);
            PawnManager.Instance.AddPawnOnArea(1002, MapManager.Instance.currentMap.FindAreaByLocalId(24), 0, false);
            PawnManager.Instance.AddPawnOnArea(1003, MapManager.Instance.currentMap.FindAreaByLocalId(21), 0, false);
            PawnManager.Instance.AddPawnOnArea(1004, MapManager.Instance.currentMap.FindAreaByLocalId(25), 0, false);
            PawnManager.Instance.AddPawnOnArea(1005, MapManager.Instance.currentMap.FindAreaByLocalId(14), 0, true);
            //PawnManager.Instance.AddPawnOnArea(1005, MapManager.Instance.currentMap.FindAreaByLocalId(22), 0, true);
            //PawnManager.Instance.AddPawnOnArea(1005, MapManager.Instance.currentMap.FindAreaByLocalId(4), 0, true);
            //PawnManager.Instance.AddPawnOnArea(1005, MapManager.Instance.currentMap.FindAreaByLocalId(21), 0, true);
            //PawnManager.Instance.AddPawnOnArea(1005, MapManager.Instance.currentMap.FindAreaByLocalId(2), 0, true);
            #region//获取本次场景里的事件



            //开始监听事件

            StartCoroutine(DialogueTriggerManager.Instance.StartListenEvent());
            #endregion

            this.battleView.gameObject.SetActive(true);

            RTSCamera.Instance.ResetCameraPos();
            AudioManager.Instance.PlayMusic(MapManager.Instance.currentMapData.nameBgm);
        }
        public void RestartBattle()
        {
            this.ClearScene();
            this.InitBattle();
            RTSCamera.Instance.ResetCameraPos();
        }
        public void EnterCamp()
        {

            this.battleView.ExitBattle();//待修改 等UI管理器到位
            this.buildView.gameObject.SetActive(false);//待修改 等UI管理器到位
            this.characterView.gameObject.SetActive(false);//待修改 等UI管理器到位
            this.eventView.gameObject.SetActive(false);//待修改 等UI管理器到位
            this.GameScene = GameScene.Camp;
            RTSCamera.Instance.ResetCameraPos();
            CampManager.Instance.InitCamp(PawnManager.Instance.GetPawnAvatars(false), this.totalMilitaryRes);

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



        public void ClearScene()
        {
            this.enemyOccupyAreas.Clear();
            this.playerOccupyAreas.Clear();
            EventTriggerManager.Instance.ClearEvent();
            DialogueTriggerManager.Instance.ClearEvent();
            StopCoroutine(EventTriggerManager.Instance.StartListenEvent());
            StopCoroutine(DialogueTriggerManager.Instance.StartListenEvent());
            PawnManager.Instance.ClearAllPawn();
            MapManager.Instance.ClearMap();

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