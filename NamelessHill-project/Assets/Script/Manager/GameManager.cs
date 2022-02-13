using Nameless.Agent;
using Nameless.ConfigData;
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

    public class Player
    {
        public List<Pawn> pawns;
        public int totalMilitaryRes;//�ҷ��Ĳ�������

        public Player()
        {

        }
        public Player(List<Pawn> pawns, int totalMilitaryRes)
        {
            this.pawns = pawns;
            this.totalMilitaryRes = totalMilitaryRes;
        }
        public Player(List<PawnAvatar> pawnAvatars, int totalMilitaryRes)
        {
            List<Pawn> pawns = new List<Pawn>();
            for (int i = 0; i < pawnAvatars.Count; i++)
            {
                pawns.Add(pawnAvatars[i].pawnAgent.pawn);
            }
            this.pawns = pawns;
            this.totalMilitaryRes = totalMilitaryRes;
        }
        public Player(List<PawnCamp> pawnCamps,int totalMilitaryRes)
        {
            List<Pawn> pawns = new List<Pawn>();
            for(int i = 0; i < pawnCamps.Count; i++)
            {
                pawns.Add(pawnCamps[i].pawn);
            }
            this.pawns = pawns;
            this.totalMilitaryRes = totalMilitaryRes;
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
        /// ��ʱ����
        /// </summary>
        /// 
        public const string mainMenuBgmName = "Music_MainBGM_01";
        public int totalTime = 720;//����ս����ʱ��//���޸�
        public string levelgoalDes = "Hold for 12 hours";//����ս����ʱ��//���޸�


        /// <summary>
        /// ��ʱ����
        /// </summary>



        public CharacterView characterView;
        public BattleView battleView;
        public CampView campView;
        public BuildView buildView;
        public EventView eventView;
        public ConversationView conversationView;
        public NoteBookView noteBookView;
        public MainMenuView mainMenuView;


        // Start is called before the first frame update
        void Start()
        {
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

            RTSCamera.Instance.ResetCameraPos();
            AudioManager.Instance.PlayMusic(mainMenuBgmName);

            this.localPlayer = new Player();
        }
        public void StartNewGame()
        {
            //Ĭ�ϳ����ļ�����ɫ
            List<Pawn> pawns = new List<Pawn>();
            Pawn pawn1 = PawnFactory.GetPawnById(1001);
            Pawn pawn2 = PawnFactory.GetPawnById(1002);
            Pawn pawn3 = PawnFactory.GetPawnById(1003);
            Pawn pawn4 = PawnFactory.GetPawnById(1004);
            Pawn pawn9 = PawnFactory.GetPawnById(1009);
            Pawn pawn10 = PawnFactory.GetPawnById(1010);
            pawns.Add(pawn1);
            pawns.Add(pawn2);
            pawns.Add(pawn3);
            pawns.Add(pawn4);
            pawns.Add(pawn9);
            pawns.Add(pawn10);
            this.localPlayer = new Player(pawns,1000);
            this.EnterBattle();
        }
        public void EnterBattle()//����ս��  //����ʼ���ŵĶ���
        {
            GameManager.Instance.ClearBattle();
            GameManager.Instance.ClearCamp();
            RTSCamera.Instance.ResetCameraPos();
            MapManager.Instance.GenerateTransInfoShow(MapManager.Instance.currentMapData);
            MapManager.Instance.currentTransInfoShow.StartReader();
        }
        public void InitBattle()//��ʼ��ս������������ж��� ���ڽ���ս��ս�� �� ���¿�ʼս��(�����ڱ��ص���ģʽ)
        {
            this.GameScene = GameScene.Battle;
            var Localplayer = FrontManager.Instance.GenFactionPlayer(FactionManager.Instance.GetFaction(1), true, false, false, this.localPlayer.totalMilitaryRes);
            var enemyComputer = FrontManager.Instance.GenFactionPlayer(FactionManager.Instance.GetFaction(3), false, true, false, 0);
            FrontManager.Instance.localPlayer = Localplayer;

            MapManager.Instance.GenerateMap(MapManager.Instance.currentMapData);


            //Dictionary<InitArea, FrontPlayer> frontPlayerInAreas = new Dictionary<InitArea, FrontPlayer>();//���ó�ʼ���򻮷�(����ģʽ��Ĭ�϶��Ǳ������)
            //for(int i = 0; i < MapManager.instance.currentMap.initAreas.Count; i++)
            //{
            //    frontPlayerInAreas.Add(MapManager.instance.currentMap.initAreas[i], FrontManager.Instance.localPlayer);
            //}

            MapManager.Instance.currentMap.InitArea();
            List<EventResult> eventResults = new List<EventResult>();
            foreach (var child in DataManager.Instance.eventResultData)
            {
                eventResults.Add(EventResultFactory.GetEventResultById(child.Key, Localplayer));
            }
            EventTriggerManager.Instance.InitEventTrigger(eventResults);
            //Debug.Log(DataManager.Instance.GetCharacter(1001).name);
            StartCoroutine(EventTriggerManager.Instance.StartListenEvent());

            this.battleView.InitBattle(this.totalTime, this.levelgoalDes, FrontManager.Instance.localPlayer.GetMilitaryRes());
            Time.timeScale = 1.0f;
            this.RESULTEVENT += this.Result;

            //����ɫ���õ���ͼĬ�ϵĵ���
            List<int> defaultPos = MapManager.Instance.currentMap.GetDefaultPos();
            for(int i = 0; i < defaultPos.Count; i++)
            {
                if (i < this.localPlayer.pawns.Count)
                {
                    FrontManager.Instance.AddPawnOnArea(this.localPlayer.pawns[i], MapManager.Instance.currentMap.FindAreaByLocalId(defaultPos[i]), 0, FrontManager.Instance.localPlayer);
                }
            }

            //���޸� ������Ĭ������һ�����˽�ɫ
            Pawn pawn5 = PawnFactory.GetPawnById(1005);
            FrontManager.Instance.AddPawnOnArea(pawn5, MapManager.Instance.currentMap.FindAreaByLocalId(14), 0, enemyComputer);
            #region//��ȡ���γ�������¼�



            //��ʼ�����¼�

            StartCoroutine(DialogueTriggerManager.Instance.StartListenEvent());
            #endregion

            //��ս������
            this.battleView.gameObject.SetActive(true);

            RTSCamera.Instance.ResetCameraPos();
            AudioManager.Instance.PlayMusic(MapManager.Instance.currentMapData.nameBgm);

            this.UpdateBattleToPlayer();
            PlayerController.Instance.UpdateBattlePlayer(FrontManager.Instance.localPlayer);
        }
        public void RestartBattle()//���½���ս����ʼ��Ϸ
        {
            RTSCamera.Instance.ResetCameraPos();
            this.battleView.gameObject.SetActive(false);
            this.GameScene = GameScene.Menu;
            AudioManager.Instance.PlayMusic(mainMenuBgmName);
            this.ClearBattle();
            this.EnterBattle();

        }
        public void EnterCamp()//����Ӫ��
        {

            this.battleView.ExitBattle();//���޸� ��UI��������λ
            this.buildView.gameObject.SetActive(false);//���޸� ��UI��������λ
            this.characterView.gameObject.SetActive(false);//���޸� ��UI��������λ
            this.eventView.gameObject.SetActive(false);//���޸� ��UI��������λ
            this.GameScene = GameScene.Camp;
            RTSCamera.Instance.ResetCameraPos();

            CampData campData = DataManager.Instance.GetCampData(MapManager.Instance.currentMapData.nextCampId);
            CampManager.Instance.InitCamp(campData,FrontManager.Instance.GetPawnAvatars(FrontManager.Instance.localPlayer), this.localPlayer.totalMilitaryRes);
            this.UpdateCampToPlayer();

        }
        public void BackToMainMenu()//����������
        {
            this.GameScene = GameScene.Menu;
            GameManager.Instance.ClearBattle();
            GameManager.Instance.ClearCamp();
            RTSCamera.Instance.ResetCameraPos();
            AudioManager.Instance.PlayMusic(mainMenuBgmName);
            this.battleView.gameObject.SetActive(false);
            this.campView.gameObject.SetActive(false);
            this.mainMenuView.gameObject.SetActive(true);
        }
        public void ClearBattle()//���ս���Ķ���
        {
            PlayerController.Instance.ResetBattlePlayer();
            EventTriggerManager.Instance.ClearEvent();
            DialogueTriggerManager.Instance.ClearEvent();
            StopCoroutine(EventTriggerManager.Instance.StartListenEvent());
            StopCoroutine(DialogueTriggerManager.Instance.StartListenEvent());
            FrontManager.Instance.ClearFront();
            BattleManager.Instance.ClearBattle();
            MapManager.Instance.ClearMap();

        }
        public void ClearCamp()//���Ӫ��
        {
            CampManager.Instance.ClearCamp();

        }

        private void UpdateBattleToPlayer()
        {
            this.localPlayer = new Player(FrontManager.Instance.localPlayer.GetPawnAvatars(), FrontManager.Instance.localPlayer.GetMilitaryRes());
        }
        private void UpdateCampToPlayer()
        {
            this.localPlayer = new Player(CampManager.Instance.GetPawnCamps(), CampManager.Instance.GetMilitaryRes());
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