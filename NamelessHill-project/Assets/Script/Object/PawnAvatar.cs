using Nameless.Agent;
using Nameless.Controller;
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
        public long Id;

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
        private int currentWalkNode = 0;

        private GameObject Wire;
        private GameObject walkWire;
        private LineRenderer renderWire;
        private LineRenderer walkRenderWire;
        private Dictionary<Area, LineRenderer> supportRenderWireDic = new Dictionary<Area, LineRenderer>();
        private List<LineRenderer> supportRenderWires = new List<LineRenderer>();
        public Area targetArea;
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
                this.StateTriggerEvent(value);
                //if (value == PawnState.Wait)
                //    this.PlayCharacterAnim(value);
                //else if (value == PawnState.Walk)
                //    this.PlayCharacterAnim(1);
                //else if (value == PawnState.Battle)
                //    this.PlayCharacterAnim(2);
                lastState = state;
                state = value;
            }
        }
        public bool isPlay = false;
        public int currentIndex = 0;

        public Material WireMaterial;
        public float animationDuration;

        public PawnAgent pawnAgent;
        #region//动画
        private const string pathFindAnim = "Prefabs/CharacterAnim/";

        public GameObject _root;
        public Action _attackEvent;
        public Action _walkEvent;
        public Action _waitEvent;
        public Action _deathEvent;

        public Animation dialogueAnim;
        #endregion

        #region//UI
        public Slider healthBar;
        public Image healthBarColor;
        public Text nameTxt;
        public GameObject dialogueIm;
        public Text dialogueTxt;


        public GameObject fixbtn;
        private bool FixBtnActive
        {
            set
            {
                fixbtn.gameObject.SetActive(value);
                fixBtnActive = value;
            }
            get
            {
                return fixBtnActive;
            }
        }
        private bool fixBtnActive = false;
        #endregion
        // Start is called before the first frame update
        public void Init(int mapId)
        {
            this.healthBar.value = 1;
            this.characterView = GameManager.Instance.characterView;
            this.pawnAgent = new PawnAgent(this.healthBar, this.CurrentArea,PawnFactory.GetPawnById(Id),mapId);
            this.State = PawnState.Wait;
            this.nameTxt.text = DataManager.Instance.GetCharacter(Id).name;
            this.fixbtn.gameObject.SetActive(false);
            GameObject animObj = Instantiate(Resources.Load(pathFindAnim + this.pawnAgent.pawn.animName), this._root.transform) as GameObject;
            animObj.transform.localPosition = new Vector3(0, -9 ,0);
            animObj.transform.localScale = new Vector3(1, 1, 1);
            animObj.GetComponent<CharacterAnim>().Init(this);
            //this.InitLine();
        }
        // Update is called once per frame
        void Update()
        {
            if (this.currentArea == null || this.pawnAgent == null || RTSCamera.Instance._isTranstionTo)
                return;



            Ray targetRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit targetHit;
            if (Physics.Raycast(targetRay, out targetHit))
            {
                if (targetHit.transform.gameObject == this.gameObject)
                {
                    PathManager.Instance.ShowPath(this);
                }
            }


            if (this.State == PawnState.Wait)
                this.CheckSupport();
            else if (this.State == PawnState.Draw && !this.isAI)
                this.MouseDrawPath();
            //else if (this.State == PawnState.Walk)
            //    this.WalkPath();
            this.pawnAgent.RunningTimePropertyUpdate(this.State);
            //this.CheckBattleState();

            if (this.isAI/* && this.State == PawnState.Wait*/)
            {
                //this.targetArea = this.CurrentArea;
                //this.startPoint = this.CurrentArea;
                //this.InitLine();
                this.AIBehavior();
                return;
            }
            if (this.characterView.gameObject.activeInHierarchy || this.isAI)
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
            if (Input.GetMouseButtonDown(0) /*&& this.State != PawnState.Battle*/)
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
                        AreasManager.Instance.mouseFollower.gameObject.GetComponent<SpriteRenderer>().sprite = this.pawnAgent.pawn.selectIcon;
                        this.InitLine();
                        this.ShowPath(true);
                    }
                    else if (TargetHit.transform.gameObject == this.fixbtn && !this.isAI)//待修改.AI
                    {
                        GameManager.Instance.buildView.gameObject.SetActive(true);
                        GameManager.Instance.buildView.ResetBuild(this);
                        GameManager.Instance.PauseOrPlay(false);
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
                        
                        this.ShowPath(false);
                        PathManager.Instance.AddPath(this);
                        this.State = PawnState.Walk;
                        List<Vector3> tempNode = this.nodePath;
                        StartCoroutine(WalkLineByNode(tempNode));
                        //this.InitWalkLine();
                        //StartCoroutine(ShowWalkPath(start, this.pathList[0].nodes.Length - 1, t));
                    }
                    else
                    {
                        this.State = PawnState.Wait;
                    }

                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("sssss");
                Vector2 raySelect = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(raySelect, Vector2.zero);
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject == this.currentArea.gameObject && !this.isAI)//待修改.AI
                    {
                        this.FixBtnActive = !this.FixBtnActive;

                    }
                }
            }

        }

        #region//路径
        void InitLine()
        {
            Destroy(Wire);
            Destroy(walkWire);
            this.currentWalkNode = 0;
            this.nodePath = new List<Vector3>();
            this.Wire = new GameObject();
            this.walkWire = new GameObject();
            this.areaDic = new Dictionary<GameObject, bool>();
            this.distanceDic = new Dictionary<GameObject, float>();
            this.endAreaList = new List<Area>();
            this.pathList = new List<Path>();
            this.Wire.name = "Wire: " + this.pawnAgent.pawn.name;
            this.walkWire.name = "walkWire";
            this.renderWire = Wire.AddComponent<LineRenderer>();
            this.walkRenderWire = walkWire.AddComponent<LineRenderer>();
            this.renderWire.material = WireMaterial;
            this.walkRenderWire.material = WireMaterial;
            //renderWire.material.SetTextureScale("_MainTex", new Vector2(2f, 2f));
            this.renderWire.SetWidth(0.4f, 0.4f);
            this.walkRenderWire.SetWidth(0.4f, 0.4f);
            Color playerPath = new Color(0, 0, 1, 1);
            Color aiPath = new Color(1, 1, 0, 0.3f);
            this.renderWire.SetColors(this.isAI ? aiPath : playerPath, this.isAI ? aiPath : playerPath);
            Color playerWalk = new Color(0, 1, 1, 1);
            Color aiWalk = new Color(1, 0.5f, 0, 0.3f);
            this.walkRenderWire.SetColors(this.isAI? aiWalk : playerWalk, this.isAI ? aiWalk : playerWalk);
            
            this.renderWire.sortingOrder = 0;
            this.walkRenderWire.sortingOrder = 1;
            this.currentIndex = 0;
            this.nodePath.Add(this.startPoint.centerNode.transform.position);
            this.renderWire.sortingOrder = 1;
            this.walkRenderWire.sortingOrder = 2;
            this.renderWire.positionCount = 1;
            this.walkRenderWire.positionCount = 1;
            this.renderWire.SetPosition(0, this.startPoint.centerNode.transform.position);
            this.walkRenderWire.SetPosition(0, this.startPoint.centerNode.transform.position);
            this.areaDic.Add(this.targetArea.gameObject, true);

        }
        public void ShowPath(bool isShow)
        {
            if(this.renderWire!=null && this.walkRenderWire != null)
            {
                float alpha = isShow ? 1.0f : 0.3f;
                Color playerPath = new Color(0, 0, 1, alpha);
                Color aiPath = new Color(1, 1, 0, alpha);
                Color playerWalk = new Color(0, 1, 1, alpha);
                Color aiWalk = new Color(1, 0.5f, 0, alpha);

                this.renderWire.SetColors(this.isAI ? aiPath : playerPath, this.isAI ? aiPath : playerPath);
                this.walkRenderWire.SetColors(this.isAI ? aiWalk : playerWalk, this.isAI ? aiWalk : playerWalk);

            }
        }
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
        }//用鼠标画路径
        void AIBehavior(/*int goal*/)
        {
            if(this.State == PawnState.Wait)
            {
                if (!GameManager.Instance.IsBelongToSameSide(this.currentArea, this))//如果当前区域还不是AI的 则继续等待
                    return;

                List<Area> oppoArea = new List<Area>();
                List<Area> occupyArea = new List<Area>();
                int samePawnNum = 0;
                for (int i = 0; i < this.currentArea.neighboors.Count; i++)
                {
                    if (this.currentArea.neighboors[i].pawns.Count > 0 && !this.currentArea.neighboors[i].pawns[0].isAI)//待修改 等框架搭建确定阵营
                    {
                        oppoArea.Add(this.currentArea.neighboors[i]);
                    }
                    if (this.currentArea.neighboors[i].pawns.Count <= 0 && !GameManager.Instance.IsBelongToSameSide( this.currentArea.neighboors[i],this))//待修改 等框架搭建确定阵营
                    {
                        occupyArea.Add(this.currentArea.neighboors[i]);
                    }
                    if(this.currentArea.neighboors[i].pawns.Count > 0 && GameManager.Instance.IsBelongToSameSide(this.currentArea.neighboors[i], this))
                    {
                        samePawnNum++;
                    }
                }
                if (this.currentArea.neighboors.Count == samePawnNum)
                    return;
                Area tagArea;
                if (oppoArea.Count > 0)
                    tagArea = oppoArea[UnityEngine.Random.Range(0, oppoArea.Count - 1)];
                else if (occupyArea.Count > 0)
                    tagArea = occupyArea[UnityEngine.Random.Range(0, occupyArea.Count - 1)];
                else
                {
                    List<int>[] tempPath = AreasManager.Instance.Dijkstra(AreasManager.Instance.areaMatrix, this.currentArea.id);
                    List<int> targetPath = new List<int>();
                    for (int i = 0; i < tempPath.Length; i++)
                    {
                        if (tempPath[i][tempPath[i].Count - 1] == this.goal)
                        {
                            targetPath = tempPath[i];
                        }
                    }
                    tagArea = AreasManager.Instance.areas[targetPath[1]];
                }
                this.targetArea = this.CurrentArea;
                this.startPoint = this.CurrentArea;
                this.InitLine();
                this.State = PawnState.Draw;
                this.DrawPath(tagArea, true);
            }

            //if (this.currentArea.id == goal)
            //    return;
            //this.State = PawnState.Draw;
            ////this.goal = goal;
            //List<int>[] tempPath = AreasManager.Instance.Dijkstra(AreasManager.Instance.areaMatrix, this.currentArea.id);
            //List<int> targetPath = new List<int>();
            //for (int i = 0; i < tempPath.Length; i++)
            //{
            //    if (tempPath[i][tempPath[i].Count - 1] == this.goal)
            //    {
            //        targetPath = tempPath[i];
            //    }
            //}
            //for (int i = 0; i < targetPath.Count; i++)
            //{
            //    this.DrawPath(AreasManager.Instance.areas[targetPath[i]], true);
            //}
        }//AI自动画路径
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
                //Debug.Log(AreasManager.Instance.pathDic[temp1].name + "新区域AAAAAAAAAA");

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
                //Debug.Log(AreasManager.Instance.pathDic[temp2].name + "新区域BBBBBBBBB");

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
        }//路径绘制
        //void WalkPath()
        //{
        //    if (this.durationTime >= this.walkTime)
        //    {
        //        if (this.endAreaList[this.currentWalkNode].pawns.Count > 0 && this.isAI != this.endAreaList[this.currentWalkNode].pawns[0].isAI)//待修改 判断是否战斗
        //        {
        //            if (this.endAreaList[this.currentWalkNode].pawns[0].State != PawnState.Battle)
        //            {
        //                this.State = PawnState.Battle;
        //                this.endAreaList[this.currentWalkNode].pawns[0].State = PawnState.Battle;
        //                BattleManager.Instance.GenerateBattle(this, this.endAreaList[this.currentWalkNode].pawns[0]);
        //            }
        //            return;
        //        }
        //        int nodes = (this.pathList[this.currentWalkNode].nodes.Length - 1);
        //        this.walkedNodes += nodes;
        //        this.ReDrawLine(this.walkedNodes);
        //        this.gameObject.transform.position = this.endAreaList[this.currentWalkNode].centerNode.gameObject.transform.position;
        //        this.CurrentArea.RemovePawn(this);
        //        this.UpdateCurrentArea(this.endAreaList[this.currentWalkNode]);
        //        this.CurrentArea.AddPawn(this);
        //        if (!this.isAI)
        //        {
        //            this.TryGetMat();//到达该区域后试图获取当地材料
        //        }
        //        this.currentWalkNode++;
        //        if (this.currentWalkNode < this.endAreaList.Count)
        //        {
        //            this.durationTime = 0.0f;
        //            this.walkTime = this.distanceDic[this.endAreaList[this.currentWalkNode].centerNode] / this.pawnAgent.pawn.curSpeed;
        //            //this.InitWalkLine();
        //            //StartCoroutine(ShowWalkPath(start, end, t));    
        //        }
        //        else
        //        {
        //            this.InitLine();
        //            this.State = PawnState.Wait;
        //        }
        //    }
        //    else
        //    {
        //        this.durationTime += Time.deltaTime;
        //    }
        //}//路径绘制完毕后进行行走
        void ReDrawWalkLine(int startInt)
        {
            for (int i = startInt; i < walkRenderWire.positionCount; i++)
            {
                walkRenderWire.SetPosition(i, walkRenderWire.GetPosition(startInt));

            }

        }

        void RemoveBehindLine(int startInt)
        {
            for (int i = 0; i <= startInt; i++)
            {
                this.walkRenderWire.SetPosition(i, walkRenderWire.GetPosition(startInt));
                this.renderWire.SetPosition(i, walkRenderWire.GetPosition(startInt));

            }
        }
        IEnumerator DrawLineByNode(int currentPath,bool isAuto)
        {
            if (nodePath != null)
            {
                if (0 < currentPath)
                {
                    renderWire.positionCount = currentPath;
                    //Debug.Log("开画！！！！！！！this.currentIndex: " + this.currentIndex);
                    float segmentDuration = this.animationDuration / this.nodeCount;
                    this.isPlay = true;
                    for (int i = this.currentIndex; i < renderWire.positionCount - 1; i++)
                    {
                        float startTime = Time.time;
                        //Debug.Log("i:" + i);
                        Vector3 startPos = nodePath[i];
                        Vector3 endPos = nodePath[i + 1];

                        Vector3 pos = startPos;
                        while (pos != endPos)
                        {
                            if (!GameManager.Instance.isPlay)
                            {
                                yield return null;
                            }
                            else
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
                        }
                        //Debug.Log("结束播放");
                    }
                    this.currentIndex = renderWire.positionCount - 1;
                    this.isPlay = false;
                    //Debug.Log("结束播放");
                    if (this.nodePath.Count != renderWire.positionCount)
                    {
                        //Debug.Log("新轮回！！！");
                        StartCoroutine(DrawLineByNode(this.nodePath.Count,isAuto));
                    }
                    else if (isAuto)//用于AI自动走路
                    {
                        this.State = PawnState.Walk;
                        List<Vector3> tempNode = this.nodePath;
                        StartCoroutine(WalkLineByNode(tempNode));
                    }
                }
            }
        }
        IEnumerator WalkLineByNode(List<Vector3> nodeWalk)
        {
            if (nodeWalk != null)
            {
                if (0 < nodeWalk.Count)
                {
                    int lastNode = -1;
                    this.walkRenderWire.positionCount = nodeWalk.Count;
                    //Debug.Log("开画走路！！！！！！！");
                    this.ChangeDirection(this.endAreaList[this.currentWalkNode].centerNode.gameObject);
                    for (int i = 0; i < this.walkRenderWire.positionCount - 1; i++)
                    {
                        float startTime = Time.time;
                        //Debug.Log("i:" + i);
                        Vector3 startPos = nodeWalk[i];
                        Vector3 endPos = nodeWalk[i + 1];

                        Vector3 pos = startPos;
                        float distance = Vector2.Distance(new Vector2(startPos.x,startPos.y), new Vector2(endPos.x,endPos.y));
                        float segmentTime = distance / this.pawnAgent.pawn.curSpeed;
                        segmentTime = GameManager.Instance.IsBelongToSameSide(this.currentArea, this) ? segmentTime : segmentTime/2;
                        while (pos != endPos)
                        {
                            if (!GameManager.Instance.isPlay)
                            {
                                yield return null;
                            }
                            else
                            {
                                float t = (Time.time - startTime) / segmentTime;
                                pos = Vector3.Lerp(startPos, endPos, t);

                                for (int j = i + 1; j < this.walkRenderWire.positionCount; j++)
                                {
                                    while (this.State != PawnState.Walk)
                                    {
                                        if (this.State == PawnState.Draw)
                                        {
                                            yield break;
                                        }
                                        yield return null;
                                    }

                                    try
                                    {
                                        this.walkRenderWire.SetPosition(j, pos);
                                    }
                                    catch (Exception e)
                                    {
                                        Debug.LogError(renderWire.positionCount + " : " + "j" + " : " + j);
                                    }

                                }
                                yield return null;
                            }

                            while(this.State != PawnState.Walk)
                            {
                                if(this.State == PawnState.Draw)
                                {
                                    yield break;
                                }
                                yield return null;
                            }
                        }
                        if (endPos == this.endAreaList[this.currentWalkNode].centerNode.gameObject.transform.position)
                        {
                            bool ifBlock = false;
                            if (this.endAreaList[this.currentWalkNode].pawns.Count > 0 )
                            {
                                ifBlock = true;
                                this.ReDrawWalkLine(lastNode+1);
                                i = lastNode;
                                if ( this.isAI != this.endAreaList[this.currentWalkNode].pawns[0].isAI)//待修改 判断是否战斗
                                {
                                    this.State = PawnState.Battle;
                                    bool defenderisInBattle = false;
                                    if (this.endAreaList[this.currentWalkNode].pawns[0].State == PawnState.Battle)
                                        defenderisInBattle = true;
                                    else
                                        this.endAreaList[this.currentWalkNode].pawns[0].State = PawnState.Battle;
                                    BattleManager.Instance.GenerateBattle(this, this.endAreaList[this.currentWalkNode].pawns[0],defenderisInBattle);
                                }

                                while (this.State != PawnState.Draw && this.endAreaList[this.currentWalkNode].pawns.Count > 0)
                                {
                                    if (this.State != PawnState.Battle && this.pawnAgent.curOpponent != null)
                                        this.State = PawnState.Battle;
                                    if (this.State == PawnState.Walk)
                                    {
                                        this.StateTriggerEvent(PawnState.Wait);
                                        this.CheckSupport();
                                    }
                                    if (this.endAreaList[this.currentWalkNode].pawns[0].isAI != this.isAI && this.endAreaList[this.currentWalkNode].pawns[0].State != PawnState.Battle)
                                        break;
                                    yield return null;
                                }

                                this.StateTriggerEvent(PawnState.Walk);
                                this.CheckSupport();
                                if (this.State == PawnState.Draw)
                                {
                                    yield break;
                                }

                            }
                            if (!ifBlock)
                            {
                                if (this.endAreaList[this.currentWalkNode].AddPawn(this))//如果两个角色正好同时到达 则让最后添加的人回去等着
                                {
                                    this.gameObject.transform.position = this.endAreaList[this.currentWalkNode].centerNode.gameObject.transform.position;
                                    this.CurrentArea.RemovePawn(this);
                                    this.UpdateCurrentArea(this.endAreaList[this.currentWalkNode]);
                                    this.RemoveBehindLine(lastNode+2);
                                    if (!this.isAI)
                                    {
                                        this.TryGetMat();//到达该区域后试图获取当地材料
                                    }
                                    this.currentWalkNode++;
                                    if (this.currentWalkNode < this.endAreaList.Count)
                                        this.ChangeDirection(this.endAreaList[this.currentWalkNode].centerNode.gameObject);
                                    lastNode = i;
                                }
                                else
                                {
                                    this.ReDrawWalkLine(lastNode + 1);
                                    i = lastNode;
                                }
                            }
                        }
                        //Debug.Log("结束播放");
                    }

                    this.InitLine();
                    if (this.currentWalkNode >= this.endAreaList.Count)
                        this.State = PawnState.Wait;
                    //}
                }
            }
        }
        #endregion

        #region//战斗
        public void CalcuateBattleInfo()
        {
            float attack = this.pawnAgent.pawn.curAttack;
            float defend = this.pawnAgent.pawn.curDefend;
            for (int i = 0; i < this.pawnAgent.skills.Count; i++)
            {
                if (this.pawnAgent.skills[i] is FightSkill)
                {
                    PropertySkillEffect propertySkillEffect = this.pawnAgent.skills[i].Execute(this, this);
                    attack += propertySkillEffect.changeAttack;
                    defend += propertySkillEffect.changeDefend;
                }
            }
            for(int i = 0; i < this.pawnAgent.supporters.Count; i++)
            {
                List<Skill> skills = this.pawnAgent.supporters[i].pawnAgent.skills;
                for(int j = 0; j < skills.Count; j++)
                {
                    if(skills[j] is SupportSkill)
                    {
                        PropertySkillEffect propertySkillEffect = skills[j].Execute(this, this);
                        attack += propertySkillEffect.changeAttack;
                        defend += propertySkillEffect.changeDefend;
                    }
                }
            }
            this.pawnAgent.battleInfo = new PawnAgent.BattleInfo(attack, defend);
        }
        public void UpdateCurrentOppo(PawnAvatar oppo)
        {
            this.pawnAgent.curOpponent = oppo;
            this.ChangeDirection(oppo.gameObject);
        }
        public bool IsFail()
        {
            if (this.pawnAgent.pawn.curHealth <= 0)
            {
                return true;
            }
            return false;
        }//检查是否输了
        public void CheckResult(bool ifRetreat)
        {
            this.pawnAgent.MoraleChange(-10.0f);//待修改
            this.pawnAgent.battleSideDic = new Dictionary<PawnAvatar, BattleSide>();//待修改
            //this.ClearPawn();
            this.PlayDeathAnim();
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
        }//检查战败后的结果（死亡）
        public void CheckIfBattleResult()
        {
            this.pawnAgent.MoraleChange(10.0f);//待修改
            this.PlayDialogue(this.pawnAgent.pawn.winTxt);
            //this.pawnAgent.ResetBattleInfo();
            //this.State = this.lastState;
            if (this.pawnAgent.opponents.Count > 0)
            {
                this.pawnAgent.ChooseMyOpponents(this);
            }
            else
            {
                this.pawnAgent.ResetBattleInfo();
                this.State = this.lastState;
                Debug.Log(this.name+":"+this.State);
                //if (ifForward)
                //    StartCoroutine(CheckFrontArea(opponent));//检查前方区域是否还有敌人
                //else
                //    this.State = this.lastState;
            }
        }//检查战胜后的结果（是否还战斗）
        private void CheckSupport()//检查支援
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
                    if(this.CurrentArea.neighboors[i].pawns.Count <= 0 || this.CurrentArea.neighboors[i].pawns[0].State != PawnState.Battle || this.CurrentArea.neighboors[i].pawns[0].isAI != this.isAI)
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
        private void ResetAllSupport()//重置支援
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
        //}//检查是否周围都是敌人
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
        //}//检查当前的战斗状态
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
        //    int index = UnityEngine.Random.Range(0, tempAreaList.Count - 1);
        //    this.InitLine();
        //    this.DrawPath(tempAreaList[index], true);


        //}//撤退


        //IEnumerator CheckFrontArea(PawnAvatar opponent)
        //{
        //    while ((opponent != null && opponent.currentArea == this.endAreaList[this.currentWalkNode]) || this.pawnAgent.opponents.Count > 0)//在周围没有敌人攻击我的情况我等待前方敌人撤退
        //    {
        //        yield return new WaitForSecondsRealtime(0.1f);
        //    }

        //    if (this.pawnAgent.opponents.Count <= 0)//确定前方没有敌人后 再确定一下周围有没有人在打我
        //        this.State = this.lastState;
        //}//检查前方区域是否还有敌人
        #endregion

        #region//动画事件
        private void PlayWaitAnim()
        {
            if (this._waitEvent != null)
                this._waitEvent();
        }
        private void PlayWalkAnim()
        {
            if (this._walkEvent != null)
                this._walkEvent();
        }
        private void PlayAttackAnim()
        {
            if (this._attackEvent != null)
                this._attackEvent();
        }
        private void PlayDeathAnim()
        {
            if (this._deathEvent != null)
                this._deathEvent();
        }
        #endregion

        #region//动画效果
        public void UpdateCurrentArea(Area area)
        {
            this.CurrentArea = area;
            if(this.isAI && this.CurrentArea.type == AreaType.Base)
            {
                GameManager.Instance.RESULTEVENT("You Lose!!", false);
            }
        }
        public void PlayDialogue(string txt)//播放对话
        {
            this.dialogueTxt.text = txt;
            this.dialogueAnim.Play();
        }
        public void ShowDialogue(string txt)
        {
            if (txt == "-1")
            {
                this.dialogueIm.SetActive(false);
            }
            else
            {
                this.dialogueIm.SetActive(true);
                this.dialogueTxt.text = txt;
            }
        }
        public void StopDialogue()
        {
            this.dialogueIm.SetActive(false);
        }
        #endregion
        private void ChangeDirection(GameObject tag)
        {
            Vector2 dir = tag.transform.position - this.transform.position;
            if(dir.x < 0)
                this._root.transform.localScale = new Vector3(1, 1, 1);
            else if(dir.x > 0)
                this._root.transform.localScale = new Vector3(-1, 1, 1);
            
        }
        private void StateTriggerEvent(PawnState pawnState)
        {
            if (pawnState == PawnState.Wait)
            {
                //Debug.LogError("PawnState.Wait");
                this.currentArea.OccupyArea();
                this.PlayWaitAnim();
            }
            else if (pawnState == PawnState.Walk)
                this.PlayWalkAnim();
            else if (pawnState == PawnState.Battle)
                this.PlayAttackAnim();

        }
        public void TryGetMat()
        {

            List<Mat> mats = new List<Mat>();
            foreach(var child in this.currentArea.mats)
            {
                if(child.Key == MatType.MilitryResource)
                {
                    GameManager.Instance.ChangeMilitaryRes(child.Value.Nums());
                }
                mats.Add(child.Value);

                
            }
            for (int i = 0; i < mats.Count; i++) {
                this.currentArea.RemoveMat(mats[0]);
            }
        }
        public void ReceiveCurrentTime(int time)//用于检查时间流逝对该角色的影响
        {
            int passTime = GameManager.Instance.totalTime - time;
            DialogueTriggerManager.Instance.CheckTimeflowEvent(this, passTime);
        }
        public void ClearPawn()
        {
            if (this.isAI)
            {
                GenerateManager.Instance.GenerateMat(this.CurrentArea, MatType.MilitryResource, 100);
                GameManager.Instance.EnemiesKillNum(1);
                PawnManager.Instance.RemovePawn(this);
            }
            else
            {
                PawnManager.Instance.RemovePawn(this);
            }

            if (this.Wire != null)
            {
                Destroy(this.Wire.gameObject);
                if (this.walkWire != null)
                    Destroy(this.walkWire.gameObject);
            }
            this.CurrentArea.RemovePawn(this);
            //this.CurrentArea.Init();
            DialogueTriggerManager.Instance.TimeTriggerEvent -= this.ReceiveCurrentTime;
            Destroy(this.gameObject);
        }//清除掉棋子
    }

}

