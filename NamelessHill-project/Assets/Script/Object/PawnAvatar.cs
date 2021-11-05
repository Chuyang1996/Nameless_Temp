using Nameless.Data;
using Nameless.Manager;
using Nameless.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nameless.DataMono
{

    public enum PawnState
    {
        Wait = 0,
        Draw = 1,
        Walk = 2,
        Battle = 3,
    }
    public class PawnAvatar : MonoBehaviour
    {
        #region//AI
        public int goal;
        public bool isAI;
        #endregion

        public CharacterView characterView;
        private List<Vector3> nodePath;
        private Area startPoint;
        private Area endPoint;
        private Dictionary<GameObject, bool> areaDic = new Dictionary<GameObject, bool>();
        private Dictionary<GameObject, float> distanceDic = new Dictionary<GameObject, float>();
        private List<Area> endAreaList = new List<Area>();
        private List<Path> pathList = new List<Path>();
        private int nodeCount = 0;
        private int walkedNodes = 0;

        private float durationTime = 0;
        private float walkTime = 0;
        private int currentNode = 0;
        private int currentPhysicsNode = 0;

        private GameObject Wire;
        private GameObject walkWire;
        private LineRenderer renderWire;
        private LineRenderer walkRenderWire;
        private Dictionary<Area, LineRenderer> supportRenderWireDic = new Dictionary<Area, LineRenderer>();
        private List<LineRenderer> supportRenderWires = new List<LineRenderer>();
        private Area targetArea;
        public PawnState state = PawnState.Wait;

        public Area currentArea;
        public Area CurrentArea
        {
            set
            {
                this.currentArea = value;
                this.pawnAgent.ResetRunningTimeProperty(value);
            }
            get
            {
                return this.currentArea;
            }
           
        }
        public float totalDistance = 0.0f;
        public PawnState lastState = PawnState.Wait;
        public PawnState State
        {
            get
            {
                return state;
            }
            set
            {
                this.ResetAllSupport();
                this.fixbtn.gameObject.SetActive(false);
                if (value == PawnState.Wait)
                {
                    this.fixbtn.gameObject.SetActive(true);
                    this.PlayCharacterAnim(0);
                }
                else if (value == PawnState.Walk)
                    this.PlayCharacterAnim(1);
                else if (value == PawnState.Battle)
                    this.PlayCharacterAnim(2);
                lastState = state;
                state = value;
            }
        }

        public bool isPlay = false;
        public int currentIndex = 0;

        public Material WireMaterial;
        public float animationDuration;

        #region//����
        public PawnAgent pawnAgent;
        private AnimatorOverrideController animOverride;
        public AnimationClip[] animationClips;
        public Animator characterAnim;
        public Animation dialogueAnim;
        #endregion

        #region//UI
        public Image battleHint;
        public Slider healthBar;
        public Image healthBarColor;
        public Text nameTxt;
        public Text dialogueTxt;
        public GameObject fixbtn;
        #endregion
        //100, 
        //1.0f, 
        //20, 
        //1.0f,
        // 200,
        // 0.5f,
        // 100,
        // 0.5f,
        // 100, 
        //0.5f,
        // 1,
        // 1.0f,
        // 100,
        // 1.0f, 
        //100,
        // 1.0f,
        //10,
        //1.0f
        public float health = 100;
        public float crHealth = 1.0f;
        public float attack = 20.0f;
        public float crAttack = 1.0f;
        public float morale = 200.0f;
        public float crMorale = 0.5f;
        public int ammo = 100;
        public float crAmmo = 0.5f;
        public float speed = 1.0f;
        public float crSpeed = 1.0f;
        public float hit = 100.0f;
        public float crHit = 1.0f;
        public float dex = 100.0f;
        public float crDex = 1.0f;
        public float defend = 10;
        public float crDefend = 1.0f;
        // Start is called before the first frame update
        void Start()
        {
            
            //this.Init();

        }
        public void Init()
        {
            this.healthBar.value = 1;
            this.characterView = GameManager.Instance.characterView;
            this.pawnAgent = new PawnAgent(this.healthBar, this.CurrentArea,this.health, this.crHealth, this.attack, this.crAttack, this.morale, this.crMorale, this.ammo, this.crAmmo,  this.speed, this.crSpeed, this.hit, this.crHit, this.dex, this.crDex, this.defend, this.crDefend);
            this.animOverride = new AnimatorOverrideController(this.characterAnim.runtimeAnimatorController);
            this.characterAnim.runtimeAnimatorController = this.animOverride;
            this.State = PawnState.Wait;
            //this.InitLine();
        }
        // Update is called once per frame
        void Update()
        {
            if (this.currentArea == null || this.pawnAgent == null)
                return;

            if (this.State == PawnState.Wait)
                this.CheckSupport();
            else if (this.State == PawnState.Draw && !this.isAI)
                this.MouseDrawPath();
            else if (this.State == PawnState.Walk)
                this.WalkPath();
            this.pawnAgent.RunningTimePropertyUpdate(this.State);
            //this.CheckBattleState();

            if (this.isAI && this.State == PawnState.Wait)
            {
                this.targetArea = this.CurrentArea;
                this.startPoint = this.CurrentArea;
                this.InitLine();
                this.AIDrawPath();
                return;
            }
            if (this.characterView.gameObject.activeInHierarchy)
                return;
            if (Input.GetMouseButtonDown(1))
            {
                Ray targetray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit TargetHit;
                if (Physics.Raycast(targetray, out TargetHit))
                {
                    if (TargetHit.transform.gameObject == this.gameObject)
                    {
                        this.characterView.SetNewPawn(this);
                    }
                }
            }
            if (Input.GetMouseButtonDown(0) && this.State != PawnState.Battle)
            {
                //Debug.Log("sssss");
                Ray targetray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit TargetHit;
                if (Physics.Raycast(targetray, out TargetHit))
                {
                    if (TargetHit.transform.gameObject == this.gameObject)
                    {
                        this.targetArea = this.CurrentArea;
                        this.startPoint = this.CurrentArea;
                        this.State = PawnState.Draw;
                        AreasManager.Instance.mouseFollower.gameObject.GetComponent<SpriteRenderer>().sprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
                        this.InitLine();
                    }
                    else if (TargetHit.transform.gameObject == this.fixbtn)
                    {
                        GameManager.Instance.buildView.gameObject.SetActive(true);
                        GameManager.Instance.buildView.ResetBuild(this);
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (this.State == PawnState.Draw)
                {
                    AreasManager.Instance.mouseFollower.gameObject.GetComponent<SpriteRenderer>().sprite = null;
                    AreasManager.Instance.mouseFollower.ResetState();
                    if (this.endAreaList.Count > 0 && this.distanceDic.Count > 0)
                    {
                        this.walkTime = this.distanceDic[this.endAreaList[this.currentNode].centerNode] / this.pawnAgent.pawn.curSpeed;
                        this.State = PawnState.Walk;
                        float t = this.walkTime / (float)(this.pathList[this.currentNode].nodes.Length - 1);
                        int start = 0;
                        //this.InitWalkLine();
                        //StartCoroutine(ShowWalkPath(start, this.pathList[0].nodes.Length - 1, t));
                    }
                    else
                    {
                        this.State = PawnState.Wait;
                    }

                }
            }


        }

        #region//·��
        void InitLine()
        {
            Destroy(Wire);
            this.durationTime = 0;
            this.walkTime = 0;
            this.currentNode = 0;
            this.currentPhysicsNode = 0;
            this.walkedNodes = 0;
            this.nodePath = new List<Vector3>();
            this.Wire = new GameObject();
            this.areaDic = new Dictionary<GameObject, bool>();
            this.distanceDic = new Dictionary<GameObject, float>();
            this.endAreaList = new List<Area>();
            this.pathList = new List<Path>();
            this.Wire.name = "Wire";
            this.renderWire = Wire.AddComponent<LineRenderer>();
            this.renderWire.material = WireMaterial;
            //renderWire.material.SetTextureScale("_MainTex", new Vector2(2f, 2f));
            this.renderWire.SetWidth(0.4f, 0.4f);
            this.renderWire.SetColors(this.isAI?Color.yellow: Color.blue, this.isAI ? Color.yellow : Color.blue);
            this.renderWire.sortingOrder = 1;
            this.currentIndex = 0;
            this.nodePath.Add(this.startPoint.centerNode.transform.position);
            this.renderWire.sortingOrder = 0;
            this.renderWire.positionCount = 1;
            this.renderWire.SetPosition(0, this.startPoint.centerNode.transform.position);
            this.areaDic.Add(this.targetArea.gameObject, true);

        }
        //void InitWalkLine()
        //{
        //    Destroy(this.walkWire);
        //    this.walkWire = new GameObject();
        //    this.walkWire.name = "walkWire";
        //    this.walkRenderWire = walkWire.AddComponent<LineRenderer>();
        //    this.walkRenderWire.material = WireMaterial;
        //    this.walkRenderWire.SetWidth(0.4f, 0.4f);
        //    this.walkRenderWire.SetColors(Color.yellow, Color.yellow);
        //    this.walkRenderWire.sortingOrder = 1;
        //    this.walkRenderWire.positionCount = 1;
        //    this.walkRenderWire.SetPosition(0, this.CurrentArea.centerNode.transform.position);
        //}//���޸�
        void MouseDrawPath()
        {

            if (Input.GetMouseButton(0) && nodePath != null && this.State == PawnState.Draw)
            {

                Vector2 raySelect = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                AreasManager.Instance.mouseFollower.gameObject.transform.position = new Vector3(raySelect.x, raySelect.y, 0);
                RaycastHit2D hit = Physics2D.Raycast(raySelect, Vector2.zero);

                if (hit.collider != null && hit.collider.gameObject.tag == "Area" && !this.areaDic.ContainsKey(hit.collider.gameObject))
                {
                    if (hit.collider.gameObject.GetComponent<Area>() != null)
                    {
                        if (hit.collider.gameObject.GetComponent<Area>() != this.targetArea)
                        {
                            this.DrawPath(hit.collider.gameObject.GetComponent<Area>(),false);
                        }
                    }
                }
            }
        }//����껭·��
        void AIDrawPath(/*int goal*/)
        {
            if (this.currentArea.id == goal)
                return;
            this.State = PawnState.Draw;
            //this.goal = goal;
            List<int>[] tempPath = AreasManager.Instance.Dijkstra(AreasManager.Instance.areaMatrix, this.currentArea.id);
            List<int> targetPath = new List<int>();
            for (int i = 0; i < tempPath.Length; i++)
            {
                if (tempPath[i][tempPath[i].Count - 1] == this.goal)
                {
                    targetPath = tempPath[i];
                }
            }
            for (int i = 0; i < targetPath.Count; i++)
            {
                this.DrawPath(AreasManager.Instance.areas[targetPath[i]], true);
            }
        }//AI�Զ���·��
        void DrawPath(Area targetArea,bool isAuto)
        {
            this.targetArea = targetArea;
            this.endPoint = targetArea;
            NodeToNode temp1 = new NodeToNode(this.startPoint.centerNode, this.endPoint.centerNode);
            NodeToNode temp2 = new NodeToNode(this.endPoint.centerNode, this.startPoint.centerNode);
            if (AreasManager.Instance.pathDic.ContainsKey(temp1))
            {

                this.nodePath.RemoveAt(this.nodePath.Count - 1);
                for (int i = 0; i < AreasManager.Instance.pathDic[temp1].nodes.Length; i++)
                {
                    this.nodePath.Add(AreasManager.Instance.pathDic[temp1].nodes[i].transform.position);
                }
                this.nodeCount = AreasManager.Instance.pathDic[temp1].nodes.Length;
                Debug.Log(AreasManager.Instance.pathDic[temp1].name + "������AAAAAAAAAA");

                this.startPoint = this.targetArea;

                this.areaDic.Add(this.targetArea.gameObject, true);
                this.pathList.Add(AreasManager.Instance.pathDic[temp1]);
                this.distanceDic.Add(this.endPoint.centerNode, AreasManager.Instance.pathDic[temp1].Distance());
                this.endAreaList.Add(this.endPoint);

                this.totalDistance += AreasManager.Instance.pathDic[temp1].Distance();
                if (!isAuto)
                {
                    if (this.targetArea.pawns.Count > 0 && this.targetArea.pawns[0].isAI)
                        AreasManager.Instance.mouseFollower.LabelChange(this.totalDistance / this.pawnAgent.pawn.curSpeed, TipState.Battle);
                    else
                        AreasManager.Instance.mouseFollower.LabelChange(this.totalDistance / this.pawnAgent.pawn.curSpeed, TipState.Walk);
                }
                if (!this.isPlay)
                    StartCoroutine(this.DrawLineByNode(nodePath.Count,isAuto));
            }
            else if (AreasManager.Instance.pathDic.ContainsKey(temp2))
            {
                this.nodePath.RemoveAt(this.nodePath.Count - 1);
                for (int i = AreasManager.Instance.pathDic[temp2].nodes.Length - 1; i >= 0; i--)
                {
                    this.nodePath.Add(AreasManager.Instance.pathDic[temp2].nodes[i].transform.position);
                }

                this.nodeCount = AreasManager.Instance.pathDic[temp2].nodes.Length;
                Debug.Log(AreasManager.Instance.pathDic[temp2].name + "������BBBBBBBBB");

                this.startPoint = this.targetArea;

                this.areaDic.Add(this.targetArea.gameObject, true);
                this.pathList.Add(AreasManager.Instance.pathDic[temp2]);
                this.distanceDic.Add(this.endPoint.centerNode, AreasManager.Instance.pathDic[temp2].Distance());
                this.endAreaList.Add(this.endPoint);

                this.totalDistance += AreasManager.Instance.pathDic[temp2].Distance();
                if (!isAuto)
                {
                    if (this.targetArea.pawns.Count > 0 && this.targetArea.pawns[0].isAI)
                        AreasManager.Instance.mouseFollower.LabelChange(this.totalDistance / this.pawnAgent.pawn.curSpeed, TipState.Battle);
                    else
                        AreasManager.Instance.mouseFollower.LabelChange(this.totalDistance / this.pawnAgent.pawn.curSpeed, TipState.Walk);
                }
                if (!this.isPlay)
                    StartCoroutine(this.DrawLineByNode(nodePath.Count, isAuto));
            }
            else
            {
                if (!isAuto)
                    AreasManager.Instance.mouseFollower.LabelChange(0.0f, TipState.UnWalk);
            }
        }//·������
        void WalkPath()
        {
            if (this.durationTime >= this.walkTime)
            {
                if (this.endAreaList[this.currentNode].pawns.Count > 0 && this.isAI != this.endAreaList[this.currentNode].pawns[0].isAI )//���޸� �ж��Ƿ�ս��
                {
                    if (this.endAreaList[this.currentNode].pawns[0].State != PawnState.Battle)
                    {
                        this.State = PawnState.Battle;
                        this.endAreaList[this.currentNode].pawns[0].State = PawnState.Battle;
                        BattleManager.Instance.GenerateBattle(this, this.endAreaList[this.currentNode].pawns[0]);
                    }
                    return;
                }
                int nodes = (this.pathList[this.currentNode].nodes.Length - 1);
                this.walkedNodes += nodes;
                this.ReDrawLine(this.walkedNodes - 1);
                this.gameObject.transform.position = this.endAreaList[this.currentNode].centerNode.gameObject.transform.position;
                this.CurrentArea.RemovePawn(this);
                this.UpdateCurrentArea(this.endAreaList[this.currentNode]);
                this.CurrentArea.AddPawn(this);
                if (!this.isAI)
                {
                    this.TryGetMat();//������������ͼ��ȡ���ز���
                }
                this.currentNode++;
                if (this.currentNode < this.endAreaList.Count)
                {
                    this.durationTime = 0.0f;
                    this.walkTime = this.distanceDic[this.endAreaList[this.currentNode].centerNode] / this.pawnAgent.pawn.curSpeed;
                    float t = this.walkTime / (float)(this.pathList[this.currentNode].nodes.Length - 1);
                    int start = this.walkedNodes - 1;
                    int end = this.walkedNodes + this.pathList[this.currentNode].nodes.Length - 1;
                    //this.InitWalkLine();
                    //StartCoroutine(ShowWalkPath(start, end, t));    
                }
                else
                {
                    this.InitLine();
                    this.State = PawnState.Wait;
                }
            }
            else
            {
                this.durationTime += Time.deltaTime;
            }
        }//·��������Ϻ��������
        void ReDrawLine(int startInt)
        {
            if (nodePath != null)
            {
                if (0 < nodePath.Count)
                {
                    renderWire.positionCount = nodePath.Count - startInt;

                    for (int i = startInt; i < nodePath.Count; i++)
                    {
                        renderWire.SetPosition(i - startInt, nodePath[i]);

                    }
                }
            }
        }
        IEnumerator DrawLineByNode(int currentPath,bool isAuto)
        {
            if (nodePath != null)
            {
                if (0 < currentPath)
                {
                    renderWire.positionCount = currentPath;
                    Debug.Log("������������������this.currentIndex: " + this.currentIndex);
                    float segmentDuration = this.animationDuration / this.nodeCount;
                    this.isPlay = true;
                    for (int i = this.currentIndex; i < renderWire.positionCount - 1; i++)
                    {
                        float startTime = Time.time;
                        Debug.Log("i:" + i);
                        Vector3 startPos = nodePath[i];
                        Vector3 endPos = nodePath[i + 1];

                        Vector3 pos = startPos;
                        while (pos != endPos)
                        {
                            float t = (Time.time - startTime) / segmentDuration;
                            pos = Vector3.Lerp(startPos, endPos, t);

                            for (int j = i + 1; j < renderWire.positionCount; j++)
                            {
                                try
                                {
                                    renderWire.SetPosition(j, pos);
                                }
                                catch (Exception e)
                                {
                                    Debug.LogError(renderWire.positionCount + " : " + "j" + " : " + j);
                                }
                            }
                            yield return null;
                        }
                        //Debug.Log("��������");
                    }
                    this.currentIndex = renderWire.positionCount - 1;
                    this.isPlay = false;
                    Debug.Log("��������");
                    if (this.nodePath.Count != renderWire.positionCount)
                    {
                        Debug.Log("���ֻأ�����");
                        StartCoroutine(DrawLineByNode(this.nodePath.Count,isAuto));
                    }
                    else if (isAuto)
                    {
                        this.State = PawnState.Walk;
                    }
                }
            }
        }
        //IEnumerator ShowWalkPath(int start, int end, float t)
        //{
        //    this.walkRenderWire.positionCount = end - start + 1;
        //    Debug.Log("��·��������������" );
        //    float segmentDuration = this.animationDuration / this.nodeCount;
        //    this.isPlay = true;
        //    for (int i = start; i <= end; i++)
        //    {
        //        float startTime = Time.time;
        //        Debug.Log("i:" + i);
        //        Vector3 startPos = nodePath[i];
        //        Vector3 endPos = nodePath[i + 1];

        //        Vector3 pos = startPos;
        //        while (pos != endPos)
        //        {
        //            float ft = (Time.time - startTime)/ segmentDuration;
        //            pos = Vector3.Lerp(startPos, endPos, ft);

        //            for (int j = i + 1; j <= 5; j++)
        //            {
        //                this.walkRenderWire.SetPosition(j, pos);
        //            }
        //            yield return null;
        //        }
        //        //Debug.Log("��������");
        //    }
        //}//���޸�
        #endregion

        #region//ս��
        public bool IsFail()
        {
            if (this.pawnAgent.pawn.curHealth <= 0)
            {
                return true;
            }
            return false;
        }//����Ƿ�����
        public void CheckResult(bool ifRetreat)
        {
            this.pawnAgent.MoraleChange(-10.0f);//���޸�
            this.ClearPawn();
            //if (this.pawnAgent.pawn.curMorale <= 0 || this.CheckIfSurround())
            //{
            //    this.ClearPawn();
            //}
            //else
            //{
            //    this.pawnAgent.InitHealth(this.pawnAgent.pawn.maxHealth * 0.2f);
            //    if (this.Wire != null)
            //        DestroyImmediate(this.Wire.gameObject);
            //    if (ifRetreat)
            //    {
            //        this.PlayDialogue(this.pawnAgent.pawn.fallBackTxt);
            //        this.targetArea = this.CurrentArea;
            //        this.startPoint = this.CurrentArea;
            //        this.InitLine();
            //        this.Retreat();
            //    }
            //}
        }//���ս�ܺ�Ľ����������
        public void CheckIfBattleResult(PawnAvatar opponent, bool ifForward)
        {
            this.pawnAgent.MoraleChange(10.0f);//���޸�
            this.PlayDialogue(this.pawnAgent.pawn.winTxt);
            this.pawnAgent.ResetBattleInfo();
            this.State = this.lastState;
            //if (this.pawnAgent.opponents.Count > 0)
            //{
            //    this.pawnAgent.ChooseMyOpponents();
            //}
            //else
            //{
            //    this.pawnAgent.ResetBattleInfo();
            //    if (ifForward)
            //        StartCoroutine(CheckFrontArea(opponent));//���ǰ�������Ƿ��е���
            //    else
            //        this.State = this.lastState;
            //}
        }//���սʤ��Ľ�����Ƿ�ս����


        private void CheckSupport()//���֧Ԯ
        {
            for(int i = 0; i < this.CurrentArea.neighboors.Count; i++)
            {
                if (!this.supportRenderWireDic.ContainsKey(this.CurrentArea.neighboors[i]))
                {
                    if(this.CurrentArea.neighboors[i].pawns.Count > 0 )
                    {
                        if (this.CurrentArea.neighboors[i].pawns[0].isAI == this.isAI && this.CurrentArea.neighboors[i].pawns[0].State == PawnState.Battle)
                        {
                            GameObject newLine = new GameObject();
                            newLine.name = "SupportLine";
                            newLine.AddComponent<Support>();
                            newLine.GetComponent<Support>().InitSupport(this, this.CurrentArea.neighboors[i].pawns[0]); 
                            LineRenderer supportLine = newLine.AddComponent<LineRenderer>();
                            supportLine.material = this.WireMaterial;
                            supportLine.SetWidth(0.2f, 0.2f);
                            Color color1 = new Color(0,0.5f, 0, 255);
                            Color color2 = new Color(0.5f, 0, 0, 255);
                            supportLine.SetColors(this.isAI ? color2 : color1, this.isAI ? color2 : color1);
                            supportLine.sortingOrder = 1;
                            supportLine.positionCount = 2;
                            supportLine.SetPosition(0, this.transform.position);
                            supportLine.SetPosition(1, this.CurrentArea.neighboors[i].pawns[0].transform.position);
                            this.supportRenderWireDic.Add(this.CurrentArea.neighboors[i], supportLine);
                            this.supportRenderWires.Add(supportLine);
                            
                        }
                    }
                }
                else
                {
                    if(this.CurrentArea.neighboors[i].pawns.Count <= 0 || this.CurrentArea.neighboors[i].pawns[0].State != PawnState.Battle )
                    {
                        this.supportRenderWireDic[this.CurrentArea.neighboors[i]].GetComponent<Support>().RemoveSupport();
                        LineRenderer tempLine = this.supportRenderWireDic[this.CurrentArea.neighboors[i]];
                        this.supportRenderWireDic.Remove(this.CurrentArea.neighboors[i]);
                        this.supportRenderWires.Remove(tempLine);
                        DestroyImmediate(tempLine.gameObject);
                    }
                }
            }
        }
        private void ResetAllSupport()//����֧Ԯ
        {
            for (int i = 0; i < this.supportRenderWires.Count; i++)
            {
                this.supportRenderWires[i].GetComponent<Support>().RemoveSupport();
                DestroyImmediate(this.supportRenderWires[i].gameObject);
            }
            this.supportRenderWires.Clear();
            this.supportRenderWireDic.Clear();
            
        }
        //public bool CheckIfSurround()
        //{
        //    int index = 0;
        //    for (int i = 0; i < this.currentArea.neighboors.Count; i++)
        //    {
        //        if (this.currentArea.neighboors[i].pawns.Count > 0 && this.isAI != this.currentArea.neighboors[i].pawns[0].isAI)
        //        {
        //            index++;
        //        }
        //    }
        //    if (this.currentArea.neighboors.Count == index)
        //        return true;

        //    return false;
        //}//����Ƿ���Χ���ǵ���
        //public void CheckBattleState()
        //{
        //    if (this.pawnAgent.BattleState == BattleState.Normal)
        //    {
        //        if (this.pawnAgent.CheckIfPinch())
        //        {

        //            this.PlayDialogue(this.pawnAgent.pawn.pinchTxt);
        //            this.pawnAgent.InitMorale(this.pawnAgent.pawn.curMorale * 0.8f);
        //            this.pawnAgent.BattleState = BattleState.Pinch;
        //        }
        //    }
        //    else if (this.pawnAgent.BattleState == BattleState.Pinch)
        //    {
        //        if (this.pawnAgent.aroundOppoNum.Count < 2)
        //        {
        //            this.pawnAgent.InitMorale(this.pawnAgent.pawn.curMorale * 1.2f);
        //            this.pawnAgent.BattleState = BattleState.Normal;
        //        }
        //        else if (this.pawnAgent.aroundOppoNum.Count == this.CurrentArea.neighboors.Count)
        //        {
        //            this.PlayDialogue(this.pawnAgent.pawn.surroundTxt);
        //            this.pawnAgent.InitMorale(this.pawnAgent.pawn.curMorale * 0.5f);
        //            this.pawnAgent.BattleState = BattleState.Surround;
        //        }
        //    }else if (this.pawnAgent.BattleState == BattleState.Surround)
        //    {
        //        if (this.pawnAgent.aroundOppoNum.Count < this.CurrentArea.neighboors.Count)
        //        {
        //            this.PlayDialogue(this.pawnAgent.pawn.pinchTxt);
        //            this.pawnAgent.InitMorale(this.pawnAgent.pawn.curMorale * 2.0f);
        //            this.pawnAgent.InitMorale(this.pawnAgent.pawn.curMorale * 0.8f);
        //            this.pawnAgent.BattleState = BattleState.Pinch;
        //        }
        //    }
        //}//��鵱ǰ��ս��״̬
        //public void Retreat()
        //{
        //    List<Area> tempAreaList = new List<Area>();
        //    for (int i = 0; i < this.currentArea.neighboors.Count; i++)
        //    {
        //        if (this.currentArea.neighboors[i].pawns.Count <= 0 || this.isAI == this.currentArea.neighboors[i].pawns[0].isAI)
        //        {
        //            tempAreaList.Add(this.currentArea.neighboors[i]);
        //        }
        //    }
        //    int index =UnityEngine.Random.Range(0, tempAreaList.Count - 1);
        //    this.InitLine();
        //    this.DrawPath(tempAreaList[index], true);


        //}//����
        //IEnumerator CheckFrontArea(PawnAvatar opponent)
        //{
        //    while ((opponent!=null && opponent.currentArea == this.endAreaList[this.currentNode]) || this.pawnAgent.opponents.Count > 0)//����Χû�е��˹����ҵ�����ҵȴ�ǰ�����˳���
        //    {
        //        yield return new WaitForSecondsRealtime(0.1f);
        //    }

        //    if(this.pawnAgent.opponents.Count<=0)//ȷ��ǰ��û�е��˺� ��ȷ��һ����Χ��û�����ڴ���
        //        this.State = this.lastState;
        //}//���ǰ�������Ƿ��е���
        #endregion

        #region//Ч��
        public void UpdateCurrentArea(Area area)
        {
            this.CurrentArea = area;
            if(this.isAI && this.CurrentArea.type == AreaType.Base)
            {
                GameManager.Instance.RESULTEVENT("You Lose!!", false);
            }
        }
        public void PlayCharacterAnim(int index)
        {
            this.animOverride["Idle"]= this.animationClips[index];
            this.characterAnim.runtimeAnimatorController = this.animOverride;
        }
        public void PlayDialogue(string txt)//���ŶԻ�
        {
            this.dialogueTxt.text = txt;
            this.dialogueAnim.Play();
        }
        public void ShowBattleHint(bool isShow)
        {
            this.battleHint.gameObject.SetActive(isShow);
        }
        public void TryGetMat()
        {
            int index = 0;
            int lenght = this.currentArea.mats.Count;
            while(index < lenght)
            {
                if(this.currentArea.mats[this.currentArea.mats.Count - 1].type == MatType.AMMO)
                {
                    GameManager.Instance.ChangeAmmo(this.currentArea.mats[this.currentArea.mats.Count - 1].num);
                }
                else if (this.currentArea.mats[this.currentArea.mats.Count - 1].type == MatType.MEDICINE)
                {
                    GameManager.Instance.ChangeMedicine(this.currentArea.mats[this.currentArea.mats.Count - 1].num);
                }
                this.currentArea.RemoveMat(this.currentArea.mats[this.currentArea.mats.Count - 1]);
                index++;
                
            }
        }
        public void ClearPawn()
        {
            if (this.isAI)
            {
                MatManager.Instance.GenerateMat(this.CurrentArea, MatType.AMMO, 100);
                MatManager.Instance.GenerateMat(this.CurrentArea, MatType.MEDICINE, 100);
            }
            if (this.Wire != null)
            {
                DestroyImmediate(this.Wire.gameObject);
                if (this.walkWire != null)
                    DestroyImmediate(this.walkWire.gameObject);
            }
            this.CurrentArea.RemovePawn(this);
            this.CurrentArea.Init();

            DestroyImmediate(this.gameObject);
        }//���������
        #endregion
    }

}

