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
        public bool accessbility = false;
        public Action<string, bool> RESULTEVENT;
        public Action<int> TotalMilitartEvent;
        /// <summary>
        /// ��ʱ����
        /// </summary>
        /// 
        public int totalTime = 720;//����ս����ʱ��//���޸�
        public string levelgoalDes = "Hold for 12 hours";//����ս����ʱ��//���޸�
        public int totalMilitaryRes;//�ҷ��Ĳ�������//���޸�

        [HideInInspector]
        public int enemiesDieNum = 0;//������������//���޸�

        /// <summary>
        /// ��ʱ����
        /// </summary>



        public Map currentMap;
        public CharacterView characterView;
        public BattleView battleView;
        public CampView campView;
        public BuildView buildView;
        public EventView eventView;
        public ConversationView conversationView;
        public NoteBookView noteBookView;

        public InitArea[] areas;//���޸� ������ҽ�ɫ����
        public List<PawnAvatar> curplayerPawns;//���޸� ��ǰ��ҽ�ɫ

        public InitArea[] enemyAreas;//���޸� ���ез���ɫ����
        public List<PawnAvatar> curenemyPawns;//���޸� ��ǰ�з���ɫ


        private List<Area> playerOccupyAreas = new List<Area>();//���޸� �������ռ�������
        private List<Area> enemyOccupyAreas = new List<Area>();//���޸� ���ез�ռ�������

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
            PawnManager.Instance.InitPawns();
            NoteManager.Instance.InitNote();
            //MapManager.Instance.InitMap();



        }
        public void EnterBattle()
        {
            Map.Instance.InitArea();
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

            PawnManager.Instance.AddPawnOnArea(1001, Map.Instance.FindAreaByLocalId(38), 0, false);
            PawnManager.Instance.AddPawnOnArea(1002, Map.Instance.FindAreaByLocalId(24), 0, false);
            PawnManager.Instance.AddPawnOnArea(1003, Map.Instance.FindAreaByLocalId(21), 0, false);
            PawnManager.Instance.AddPawnOnArea(1004, Map.Instance.FindAreaByLocalId(25), 0, false);
            PawnManager.Instance.AddPawnOnArea(1005, Map.Instance.FindAreaByLocalId(14), 0, true);
            PawnManager.Instance.AddPawnOnArea(1005, Map.Instance.FindAreaByLocalId(34), 0, true);
            #region//��ȡ���γ�������¼�



            //��ʼ�����¼�

            StartCoroutine(DialogueTriggerManager.Instance.StartListenEvent());
            #endregion

            this.battleView.gameObject.SetActive(true);
        }
        public void EnterCamp()
        {

            this.battleView.ExitBattle();//���޸� ��UI��������λ
            this.buildView.gameObject.SetActive(false);//���޸� ��UI��������λ
            this.characterView.gameObject.SetActive(false);//���޸� ��UI��������λ
            this.eventView.gameObject.SetActive(false);//���޸� ��UI��������λ
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

        
        public void AddAreaForPlayer(Area area)//���޸� ��д��ܵ�ʱ���
        {
            if(!this.playerOccupyAreas.Contains(area))
                this.playerOccupyAreas.Add(area);
            if (this.enemyOccupyAreas.Contains(area))
                this.enemyOccupyAreas.Remove(area);
            area.ChangeColor(false);

            
        }
        public void AddAreaForEnemy(Area area)//���޸� ��д��ܵ�ʱ���
        {
            if (!this.enemyOccupyAreas.Contains(area))
                this.enemyOccupyAreas.Add(area);
            if (this.playerOccupyAreas.Contains(area))
                this.playerOccupyAreas.Remove(area);
            area.ChangeColor(true);
        }
        public bool IsBelongToSameSide(Area area,PawnAvatar pawn)//���޸� ��д��ܵ�ʱ���
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
        public void EnemiesKillNum(int num)//���޸� ��д��ܵ�ʱ���
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
            PawnManager.Instance.ClearAllPawn();
            Map.Instance.ClearMap();

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