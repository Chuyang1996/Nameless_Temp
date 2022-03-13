using Nameless.Agent;
using Nameless.ConfigData;
using Nameless.Controller;
using Nameless.Data;
using Nameless.DataMono;
using Nameless.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Nameless.Manager {
    public enum GameScene
    {
        Menu =0,
        Battle = 1,
        Camp = 2,
    }
    public class EventCollections
    {
        private List<long> pawnKilledIds = new List<long>();
        private List<long> eventOptionId = new List<long>();
        private List<Pawn> leavePawn = new List<Pawn>();
        public EventCollections()
        {
            this.pawnKilledIds  = new List<long>();
            this.eventOptionId  = new List<long>();
            this.leavePawn      = new List<Pawn>();
        }
        public EventCollections(EventCollectionSaveData eventCollectionSaveData)
        {
            this.pawnKilledIds = eventCollectionSaveData.pawnKilledIds;
            this.eventOptionId = eventCollectionSaveData.eventOptionId;
            this.leavePawn = new List<Pawn>();
            for (int i = 0; i > eventCollectionSaveData.leavePawn.Count; i++)
            {
                this.leavePawn.Add(new Pawn(eventCollectionSaveData.leavePawn[i]));
            }
        }
        public List<long> AllPawnSkilledIds()
        {
            return this.pawnKilledIds;
        }
        public List<long> AllEventOptionIds()
        {
            return this.eventOptionId;
        }
        public List<Pawn> AllLeavePawns()
        {
            return this.leavePawn;
        }
        public void AddDeathPawnId(long id)
        {
            this.pawnKilledIds.Add(id);
        }

        public void AddEventOptionId(long id)
        {
            this.eventOptionId.Add(id);
        }

        public void AddLeavePawn(Pawn pawn)
        {
            this.leavePawn.Add(pawn);
        }

        public bool IsPawnKilled(long id)
        {
            if (this.pawnKilledIds.Contains(id))
                return true;
            return false;
        }

        public bool IsEventOptionChoosed(long id)
        {
            if (this.eventOptionId.Contains(id))
                return true;
            return false;
        }

        public Pawn GetLeavePawn(long id)
        {
            return this.leavePawn.Where(_pawn => _pawn.id == id).FirstOrDefault();
        }
        
    }
    public class Player
    {
        public List<Pawn> pawns = new List<Pawn>();
        public EventCollections eventCollections = new EventCollections();
        public int totalMilitaryRes;//我方的补给数量

        public Player()
        {
            this.pawns = new List<Pawn>();
            this.eventCollections = new EventCollections();
        }
        public Player(List<Pawn> pawns, int totalMilitaryRes, EventCollections eventCollections)
        {
            this.pawns = pawns;
            this.totalMilitaryRes = totalMilitaryRes;
            this.eventCollections = eventCollections;
        }
        public Player(List<PawnAvatar> pawnAvatars, int totalMilitaryRes, EventCollections eventCollections)
        {
            List<Pawn> pawns = new List<Pawn>();
            for (int i = 0; i < pawnAvatars.Count; i++)
            {
                pawns.Add(pawnAvatars[i].pawnAgent.pawn);
            }
            this.pawns = pawns;
            this.eventCollections = eventCollections;
            this.totalMilitaryRes = totalMilitaryRes;
        }
        public Player(List<PawnCamp> pawnCamps,int totalMilitaryRes, EventCollections eventCollections)
        {
            List<Pawn> pawns = new List<Pawn>();
            for(int i = 0; i < pawnCamps.Count; i++)
            {
                pawns.Add(pawnCamps[i].pawn);
            }
            this.pawns = pawns;
            this.eventCollections = eventCollections;
            this.totalMilitaryRes = totalMilitaryRes;
        }

        public Player(PlayerSaveData playerSaveData)
        {
            for(int i = 0; i < playerSaveData.pawnSaveDatas.Count; i++)
            {
                this.pawns.Add(new Pawn(playerSaveData.pawnSaveDatas[i]));
            }
            this.eventCollections = new EventCollections(playerSaveData.eventCollectionSaveData);
            this.totalMilitaryRes = playerSaveData.totalMilitaryRes;
        }
    }
    public class GameManager : SingletonMono<GameManager>
    {
        public Player localPlayer;
        public bool isPlay = true;
        public GameScene GameScene = GameScene.Menu;
        public bool accessbility = false;
        public Action<string, bool> RESULTEVENT;
        /// <summary>
        /// 临时数据
        /// </summary>
        /// 
        public const string mainMenuBgmName = "Music_MainBGM_01";
        public int totalTime = 0;//本场战斗总时间//待修改



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
        public MainMenuView mainMenuView;



        //资源包
        public AssetBundle gameDataAsset;
        public AssetBundle atlasAsset;
        public AssetBundle mapAsset;
        public AssetBundle transInfoShowAsset;
        public AssetBundle campAsset;
        public AssetBundle characterAsset;
        public AssetBundle buildAsset;
        public AssetBundle notesAsset;
        public AssetBundle audioAsset;
        // Start is called before the first frame update
        void Start()
        {
            this.LoadAssetBundle();
            this.isPlay = true;
            this.GameScene = GameScene.Menu;
            RTSCamera.Instance.InitCamera();
            PathManager.Instance.InitPath();
            DataManager.Instance.InitData();
            AudioManager.Instance.InitAudio();
            StaticObjGenManager.Instance.InitMat();
            SpriteManager.Instance.InitTexturePackage();
            NoteManager.Instance.InitNote();
            MapManager.Instance.InitMap(DataManager.Instance.GetMapData(0));
            FactionManager.Instance.InitFaction();
            FrontManager.Instance.InitFront();
            PlayerController.Instance.InitBattlePlayer();
            AudioConfig.Init();
            SaveManager.Instance.Init();

            RTSCamera.Instance.ResetCameraPos();
            AudioManager.Instance.PlayMusic(mainMenuBgmName);

            this.localPlayer = new Player();
        }
        public void LoadAssetBundle()
        {
            if(this.gameDataAsset==null)
                this.gameDataAsset = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/AssetBundle/gamedata.nameless");
            if (this.atlasAsset == null)
                this.atlasAsset = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/AssetBundle/atlas.nameless");
            if (this.mapAsset == null)
                this.mapAsset = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/AssetBundle/map.nameless");
            if (this.transInfoShowAsset == null)
                this.transInfoShowAsset = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/AssetBundle/transInfoShow.nameless");
            if (this.campAsset == null)
                this.campAsset = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/AssetBundle/camp.nameless");
            if (this.characterAsset == null)
                this.characterAsset = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/AssetBundle/character.nameless");
            if (this.buildAsset == null)
                this.buildAsset = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/AssetBundle/build.nameless");
            if (this.notesAsset == null)
                this.notesAsset = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/AssetBundle/note.nameless");
            if (this.audioAsset == null)
                this.audioAsset = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/AssetBundle/audio.nameless");
        }
        public void StartNewGame()
        {
            //默认出场的几个角色
            List<Pawn> pawns = new List<Pawn>();
            Pawn pawn1 = PawnFactory.GetPawnById(1001);
            Pawn pawn2 = PawnFactory.GetPawnById(1002);
            Pawn pawn3 = PawnFactory.GetPawnById(1003);
            Pawn pawn4 = PawnFactory.GetPawnById(1004);
            Pawn pawn5 = PawnFactory.GetPawnById(1005);
            Pawn pawn6 = PawnFactory.GetPawnById(1006);
            pawns.Add(pawn1);
            pawns.Add(pawn2);
            pawns.Add(pawn3);
            pawns.Add(pawn4);
            pawns.Add(pawn5);
            pawns.Add(pawn6);
            EventCollections eventCollections = new EventCollections();
            this.localPlayer = new Player(pawns,300, eventCollections);

            NoteManager.Instance.InitNote();
            MapManager.Instance.InitMap(DataManager.Instance.GetMapData(0));

            this.EnterBattle();
        }
        public bool LoadOldGame()
        {
            GameData gameData = SaveManager.Instance.LoadData();
            if(gameData.mapId!=-1 && gameData.campId == -1)
            {
                MapManager.Instance.InitMap(DataManager.Instance.GetMapData(gameData.mapId));
                NoteManager.Instance.notePageDic = gameData.notePageSaveData.NotePageDictionary();
                this.localPlayer = gameData.playerSaveData.GetPlayer();
                this.EnterBattle();
                return true;

            }
            else if (gameData.mapId == -1 && gameData.campId != -1)
            {
                NoteManager.Instance.notePageDic = gameData.notePageSaveData.NotePageDictionary();
                this.localPlayer = gameData.playerSaveData.GetPlayer();
                this.EnterCamp(gameData.campId, this.localPlayer);
                return true;
            }
            else
            {
                return false;
            }
        }
        public void EnterBattle()//进入战场  //并开始播放的动画
        {
            SaveManager.Instance.SaveData(this.localPlayer, NoteManager.Instance.notePageDic, MapManager.Instance.currentMapData.id, -1);
            GameManager.Instance.ClearBattle();
            GameManager.Instance.ClearCamp();
            RTSCamera.Instance.ResetCameraPos();
            MapManager.Instance.GenerateTransInfoShow(MapManager.Instance.currentMapData);
            MapManager.Instance.currentTransInfoShow.StartReader();
        }
        public void InitBattle()//初始化战斗场景里的所有对象 用于进入战场战斗 和 重新开始战斗(仅用于本地单人模式)
        {
            this.GameScene = GameScene.Battle;
            var Localplayer = FrontManager.Instance.GenFactionPlayer(FactionManager.Instance.GetFaction(1), true, false, false, this.localPlayer.totalMilitaryRes, this.localPlayer.eventCollections);
            var enemyComputer = FrontManager.Instance.GenFactionPlayer(FactionManager.Instance.GetFaction(3), false, true, false, 0, new EventCollections());
            FrontManager.Instance.localPlayer = Localplayer;
            
            MapManager.Instance.GenerateMap(MapManager.Instance.currentMapData);


            //Dictionary<InitArea, FrontPlayer> frontPlayerInAreas = new Dictionary<InitArea, FrontPlayer>();//设置初始区域划分(单人模式下默认都是本机玩家)
            //for(int i = 0; i < MapManager.instance.currentMap.initAreas.Count; i++)
            //{
            //    frontPlayerInAreas.Add(MapManager.instance.currentMap.initAreas[i], FrontManager.Instance.localPlayer);
            //}

            MapManager.Instance.currentMap.InitArea();
            List<EventResult> eventResults = new List<EventResult>();
            foreach (var child in MapManager.Instance.currentMapData.eventIds)
            {
                eventResults.Add(EventResultFactory.GetEventResultById(child, Localplayer));
            }
            EventTriggerManager.Instance.InitEventTrigger(eventResults);
            //Debug.Log(DataManager.Instance.GetCharacter(1001).name);
            StartCoroutine(EventTriggerManager.Instance.StartListenEvent());
            this.totalTime = MapManager.Instance.currentMapData.passTime;
            this.battleView.InitBattle(this.totalTime, FrontManager.Instance.localPlayer.GetMilitaryRes());
            Time.timeScale = 1.0f;
            this.RESULTEVENT += this.Result;

            //将角色设置到地图默认的点上
            List<int> defaultPos = MapManager.Instance.currentMap.GetDefaultPos();
            for(int i = 0; i < defaultPos.Count; i++)
            {
                if (i < this.localPlayer.pawns.Count)
                {
                    FrontManager.Instance.AddPawnOnArea(this.localPlayer.pawns[i], MapManager.Instance.currentMap.FindAreaByLocalId(defaultPos[i]), MapManager.Instance.currentMapData.id, FrontManager.Instance.localPlayer);
                }
            }

            //待修改 现在先默认设置一个敌人角色
            //Pawn pawn5 = PawnFactory.GetPawnById(1005);
            //FrontManager.Instance.AddPawnOnArea(pawn5, MapManager.Instance.currentMap.FindAreaByLocalId(14), MapManager.Instance.currentMapData.id, enemyComputer);
            #region//获取本次场景里的事件



            //开始监听事件

            //StartCoroutine(DialogueTriggerManager.Instance.StartListenEvent());
            #endregion

            //打开战场界面
            this.battleView.gameObject.SetActive(true);

            //RTSCamera.Instance.ResetCameraPos();
            AudioManager.Instance.PlayMusic(MapManager.Instance.currentMapData.nameBgm);

            //this.UpdateBattleToPlayer();
            PlayerController.Instance.UpdateBattlePlayer(FrontManager.Instance.localPlayer);
        }
        public void RestartBattle()//重新进入战场开始游戏
        {
            RTSCamera.Instance.ResetCameraPos();
            this.battleView.gameObject.SetActive(false);
            this.GameScene = GameScene.Menu;
            AudioManager.Instance.PlayMusic(mainMenuBgmName);
            this.ClearBattle();
            this.LoadOldGame();

        }
        public void EnterCamp(long campId,Player player)//进入营地
        {

            this.battleView.ExitBattle();//待修改 等UI管理器到位
            this.buildView.gameObject.SetActive(false);//待修改 等UI管理器到位
            this.characterView.gameObject.SetActive(false);//待修改 等UI管理器到位
            this.eventView.gameObject.SetActive(false);//待修改 等UI管理器到位
            this.GameScene = GameScene.Camp;
            RTSCamera.Instance.ResetCameraPos();


            
            CampData campData = DataManager.Instance.GetCampData(campId/*MapManager.Instance.currentMapData.nextCampId*/);
            CampManager.Instance.InitCamp(campData, player.pawns, player.eventCollections, this.localPlayer.totalMilitaryRes);
            //this.UpdateCampToPlayer();
            SaveManager.Instance.SaveData(player, NoteManager.Instance.notePageDic, -1, campId);
            
            MapManager.Instance.UpdateNewMap(DataManager.Instance.GetMapData(CampManager.Instance.currentCampData.nextBattleId));//更新下一场战斗的地图

        }
        public void BackToMainMenu()//返回主界面
        {
            this.GameScene = GameScene.Menu;
            GameManager.Instance.ClearBattle();
            GameManager.Instance.ClearCamp();
            RTSCamera.Instance.ResetCameraPos();
            AudioManager.Instance.PlayMusic(mainMenuBgmName);
            this.battleView.gameObject.SetActive(false);
            this.campView.gameObject.SetActive(false);
            this.mainMenuView.gameObject.SetActive(true);
            this.mainMenuView.MainMenuFromOtherScene();
        }
        public void ClearBattle()//清空战场的对象
        {
            PlayerController.Instance.ResetBattlePlayer();
            EventTriggerManager.Instance.ClearEvent();
            //DialogueTriggerManager.Instance.ClearEvent();
            StopCoroutine(EventTriggerManager.Instance.StartListenEvent());
           // StopCoroutine(DialogueTriggerManager.Instance.StartListenEvent());
            FrontManager.Instance.ClearFront();
            BattleManager.Instance.ClearBattle();
            MapManager.Instance.ClearMap();

        }
        public void ClearCamp()//清空营地
        {
            CampManager.Instance.ClearCamp();

        }

        public void UpdateBattleToPlayer()
        {
            this.localPlayer = new Player(FrontManager.Instance.localPlayer.GetPawnAvatars(), FrontManager.Instance.localPlayer.GetMilitaryRes(), FrontManager.Instance.localPlayer.eventCollections);
        }
        public void UpdateCampToPlayer()
        {
            this.localPlayer = new Player(CampManager.Instance.GetPawnCamps(), CampManager.Instance.GetMilitaryRes(), CampManager.Instance.eventCollections);
        }
        private void Result(string title, bool isWin)
        {
            this.battleView.resultInfoView.SetResultTxt(title, isWin);
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