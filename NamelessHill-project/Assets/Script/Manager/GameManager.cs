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
        public int totalTime = 720;//����ս����ʱ��//���޸�
        public string levelgoalDes = "Hold for 12 hours";//����ս����ʱ��//���޸�
        public int totalMilitaryRes;//�ҷ��Ĳ�������//���޸�
        [HideInInspector]
        public int enemiesDieNum = 0;//������������//���޸�

        /// <summary>
        /// ��ʱ����
        /// </summary>



        public AreasManager currentMap;
        public CharacterView characterView;
        public BattleView battleView;
        public BuildView buildView;
        public EventView eventView;

        public Area[] areas;//���޸� ������ҽ�ɫ����
        public List<PawnAvatar> playerPawns;//���޸� ������ҽ�ɫ

        public Area[] enemyAreas;//���޸� ���ез���ɫ����
        public List<PawnAvatar> enemyPawns;//���޸� ���ез���ɫ


        private List<Area> playerOccupyAreas = new List<Area>();//���޸� �������ռ�������
        private List<Area> enemyOccupyAreas = new List<Area>();//���޸� ���ез�ռ�������

        // Start is called before the first frame update
        void Start()
        {
            this.isPlay = true;
            RTSCamera.Instance.InitCamera();
            PathManager.Instance.InitPath();
            DataManager.Instance.InitData();
            AudioManager.Instance.InitAudio();
            GenerateManager.Instance.InitMat();
            SpriteManager.Instance.InitTexturePackage();
            //Debug.Log(DataManager.Instance.GetCharacter(1001).name);


            this.battleView.Init(this.totalTime, this.levelgoalDes);
            this.battleView.resourceInfoView.Init(this.totalMilitaryRes);
            Time.timeScale = 1.0f;
            this.RESULTEVENT += this.Result;
            for(int i = 0; i < this.playerPawns.Count; i++)
            {
                this.playerPawns[i].characterView = this.characterView;
                this.playerPawns[i].currentArea = this.areas[i];
                //this.areas[i].Init();
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
                //this.enemyAreas[i].Init();
                this.enemyAreas[i].AddPawn(this.enemyPawns[i]);
                this.enemyPawns[i].transform.position = this.enemyAreas[i].centerNode.transform.position;
                this.enemyPawns[i].Init(0);
                DialogueTriggerManager.Instance.TimeTriggerEvent += this.enemyPawns[i].ReceiveCurrentTime;
                DialogueTriggerManager.Instance.CheckGameStartEvent(this.enemyPawns[i]);
            }
            #region//��ȡ���γ�������¼�
            List<EventResult> eventResults = new List<EventResult>();
            foreach (var child in DataManager.Instance.eventResultData)
            {
                eventResults.Add(EventResultFactory.GetEventResultById(child.Key));
            }
            EventTriggerManager.Instance.InitEventTrigger(eventResults);

            //��ʼ�����¼�
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
            this.battleView.resourceInfoView.Init(this.totalMilitaryRes);
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