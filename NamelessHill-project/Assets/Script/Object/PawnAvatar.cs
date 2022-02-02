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

        #region//AI
        public int goal;
        #endregion

        private List<Vector3> nodePath;
        private Area startPoint;
        private Area endPoint;
        private Dictionary<GameObject, bool> areaDic = new Dictionary<GameObject, bool>();
        private Dictionary<GameObject, float> distanceDic = new Dictionary<GameObject, float>();
        private List<Area> endAreaList = new List<Area>();
        private List<Path> pathList = new List<Path>();
        private int nodeCount = 0;
        private int currentWalkNode = 0;

        private GameObject wire;
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
                this.FixBtnActive = false;
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
        public bool isBuild = false;
        public bool isPlay = false;
        public int currentIndex = 0;

        public Material WireMaterial;
        public float animationDuration;

        public PawnAgent pawnAgent;
        #region//����
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
        public Slider ocuppyBar;
        public Image healthBarColor;
        public GameObject dialogueIm;
        public Text dialogueTxt;
        public GameObject buffContent;
        private List<GameObject> buffs = new List<GameObject>();
        public Image bufficon;
        

        public Button fixbtn;
        private bool FixBtnActive
        {
            set
            {
                this.fixbtn.gameObject.SetActive(value);
                this.fixBtnActive = value;
            }
            get
            {
                return this.fixBtnActive;
            }
        }
        private bool fixBtnActive = false;
        #endregion
        // Start is called before the first frame update
        public void Init(Pawn pawn, FrontPlayer frontPlayer, int mapId, Area initArea)
        {
            
            this.healthBar.value = 1;
            this.healthBar.gameObject.transform.Find("Fill Area/Fill").gameObject.GetComponent<Image>().color = frontPlayer.faction.healthColor;
            this.ocuppyBar.gameObject.transform.Find("Fill Area/Fill").gameObject.GetComponent<Image>().color = new Color(0, 1, 1, 1);
            this.currentArea = initArea;
            this.pawnAgent = new PawnAgent(frontPlayer,this.healthBar, this.CurrentArea, pawn, mapId);
            initArea.AddPawn(this);
            this.transform.position = initArea.centerNode.transform.position; 
            this.State = PawnState.Wait;
            this.fixbtn.gameObject.SetActive(false);
            GameObject animObj = Instantiate(Resources.Load(pathFindAnim + this.pawnAgent.pawn.animPrefab), this._root.transform) as GameObject;
            animObj.transform.localPosition = new Vector3(0, -9 ,0);
            animObj.transform.localScale = new Vector3(1, 1, 1);
            animObj.GetComponent<CharacterAnim>().Init(this);

            DialogueTriggerManager.Instance.TimeTriggerEvent += this.ReceiveCurrentTime;
            DialogueTriggerManager.Instance.CheckGameStartEvent(this);

            this.fixbtn.onClick.AddListener(() =>
            {
                if (this.State != PawnState.Wait)
                    return;
                AudioManager.Instance.PlayAudio(this.transform, AudioConfig.uiRemind);
                GameManager.Instance.buildView.gameObject.SetActive(true);
                GameManager.Instance.buildView.ResetBuild(this);
                GameManager.Instance.PauseOrPlay(false);
                this.FixBtnActive = !this.FixBtnActive;
            });
            //this.InitLine();
        }
        // Update is called once per frame
        void Update()
        {
            if (this.currentArea == null || this.pawnAgent == null || RTSCamera.Instance._isTranstionTo)
                return;

            Vector2 raySelect = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(raySelect, Vector2.zero);

            Ray targetray1 = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit TargetHit1;
            if (Physics.Raycast(targetray1, out TargetHit1))
            {
                if (TargetHit1.transform.gameObject == this.gameObject)
                {
                    PathManager.Instance.ShowPath(this);
                }
            }


            if (this.State == PawnState.Wait)
                this.CheckSupport();
            else if (this.State == PawnState.Draw && this.pawnAgent.frontPlayer.CanControl())
                this.MouseDrawPath();

            this.pawnAgent.RunningTimePropertyUpdate(this.State);
            if (this.pawnAgent.frontPlayer.IsBot()/* && this.State == PawnState.Wait*/)
            {
                this.AIBehavior();
                return;
            }
            if (!GameManager.Instance.characterView.gameObject.activeInHierarchy && this.pawnAgent.frontPlayer.CanControl())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (hit.collider != null && !this.isBuild)
                    {
                        if ((hit.collider.gameObject == this.currentArea.gameObject || hit.collider.gameObject == this.gameObject)
                            && FactionManager.Instance.IsSameSide(this.currentArea.playerBelong.faction, this.GetFaction())
                            && this.State == PawnState.Wait)//���޸�.AI
                        {
                            this.FixBtnActive = !this.FixBtnActive;

                        }
                    }
                }
            }

        }

        #region//角色路径
        public void InitLine(bool byDraw)
        {
            Destroy(wire);
            Destroy(walkWire);
            this.currentWalkNode = 0;
            this.nodePath = new List<Vector3>();
            this.wire = new GameObject();
            this.walkWire = new GameObject();
            this.areaDic = new Dictionary<GameObject, bool>();
            this.distanceDic = new Dictionary<GameObject, float>();
            this.endAreaList = new List<Area>();
            this.pathList = new List<Path>();
            this.wire.name = "Wire: " + this.pawnAgent.pawn.name;
            this.walkWire.name = "walkWire";
            this.renderWire = wire.AddComponent<LineRenderer>();
            this.walkRenderWire = walkWire.AddComponent<LineRenderer>();
            this.renderWire.material = WireMaterial;
            this.walkRenderWire.material = WireMaterial;
            //renderWire.material.SetTextureScale("_MainTex", new Vector2(2f, 2f));
            this.renderWire.SetWidth(0.4f, 0.4f);
            this.walkRenderWire.SetWidth(0.4f, 0.4f);

            this.ShowPath(byDraw);

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
            this.wire.transform.parent = MapManager.Instance.currentMap.PathLine.transform;
            this.walkWire.transform.parent = MapManager.Instance.currentMap.PathLine.transform;

        }
        public void ShowPath(bool isShow)
        {
            if(this.renderWire!=null && this.walkRenderWire != null)
            {
                float alpha = isShow ? 1.0f : 0.3f;
                Color pathColor = new Color(this.pawnAgent.frontPlayer.faction.pathColor.r, this.pawnAgent.frontPlayer.faction.pathColor.g, this.pawnAgent.frontPlayer.faction.pathColor.b, alpha);
                Color walkColor = new Color(this.pawnAgent.frontPlayer.faction.walkColor.r, this.pawnAgent.frontPlayer.faction.walkColor.g, this.pawnAgent.frontPlayer.faction.walkColor.b, alpha);

                this.renderWire.SetColors(pathColor, pathColor);
                this.walkRenderWire.SetColors(walkColor, walkColor);

            }
        }
        void MouseDrawPath()
        {

            if (Input.GetMouseButton(0) && nodePath != null && this.State == PawnState.Draw)
            {

                Vector2 raySelect = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                MapManager.Instance.mouseFollower.gameObject.transform.position = new Vector3(raySelect.x, raySelect.y, 0);
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
        void AIBehavior(/*int goal*/)
        {
            if(this.State == PawnState.Wait)
            {
                if (!FactionManager.Instance.IsSameSide(this.currentArea.playerBelong.faction, this.GetFaction()))//�����ǰ���򻹲���AI�� ������ȴ�
                    return;

                List<Area> oppoArea = new List<Area>();
                List<Area> occupyArea = new List<Area>();
                int samePawnNum = 0;
                for (int i = 0; i < this.currentArea.neighboors.Count; i++)
                {
                    if (this.currentArea.neighboors[i].pawns.Count > 0
                        && FactionManager.Instance.RelationFaction(this.GetFaction(), this.currentArea.neighboors[i].pawns[0].GetFaction()) == FactionRelation.Hostility)//���޸� �ȿ�ܴȷ����Ӫ
                    {
                        oppoArea.Add(this.currentArea.neighboors[i]);
                    }
                    if (this.currentArea.neighboors[i].pawns.Count <= 0 
                        && FactionManager.Instance.RelationFaction(this.GetFaction(), this.currentArea.neighboors[i].playerBelong.faction) == FactionRelation.Hostility)//���޸� �ȿ�ܴȷ����Ӫ
                    {
                        occupyArea.Add(this.currentArea.neighboors[i]);
                    }
                    if(this.currentArea.neighboors[i].pawns.Count > 0 
                        && (FactionManager.Instance.RelationFaction(this.GetFaction(), this.currentArea.neighboors[i].pawns[0].GetFaction()) == FactionRelation.SameSide
                        ||  FactionManager.Instance.RelationFaction(this.GetFaction(), this.currentArea.neighboors[i].pawns[0].GetFaction()) == FactionRelation.Friend)
                        )
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
                    List<int>[] tempPath = MapManager.Instance.currentMap.Dijkstra(MapManager.Instance.currentMap.areaMatrix, this.currentArea.localId - 1);
                    List<int> targetPath = new List<int>();
                    for (int i = 0; i < tempPath.Length; i++)
                    {
                        if (tempPath[i][tempPath[i].Count - 1] == this.goal)
                        {
                            targetPath = tempPath[i];
                        }
                    }
                    tagArea = MapManager.Instance.currentMap.areas[targetPath[1]];
                }
                this.targetArea = this.CurrentArea;
                this.startPoint = this.CurrentArea;
                this.InitLine(false);
                this.State = PawnState.Draw;
                this.DrawPath(tagArea, true);
            }
        }//AI�Զ���·��
        void DrawPath(Area targetArea,bool isAuto)
        {
            this.targetArea = targetArea;
            this.endPoint = targetArea;
            NodeToNode temp1 = new NodeToNode(this.startPoint.centerNode, this.endPoint.centerNode);
            NodeToNode temp2 = new NodeToNode(this.endPoint.centerNode, this.startPoint.centerNode);
            if (MapManager.Instance.currentMap.pathDic.ContainsKey(temp1))
            {

                this.nodePath.RemoveAt(this.nodePath.Count - 1);
                for (int i = 0; i < MapManager.Instance.currentMap.pathDic[temp1].nodes.Length; i++)
                {
                    this.nodePath.Add(MapManager.Instance.currentMap.pathDic[temp1].nodes[i].transform.position);
                }
                this.nodeCount = MapManager.Instance.currentMap.pathDic[temp1].nodes.Length;
                //Debug.Log(AreasManager.Instance.pathDic[temp1].name + "������AAAAAAAAAA");

                this.startPoint = this.targetArea;

                this.areaDic.Add(this.targetArea.gameObject, true);
                this.pathList.Add(MapManager.Instance.currentMap.pathDic[temp1]);
                this.distanceDic.Add(this.endPoint.centerNode, MapManager.Instance.currentMap.pathDic[temp1].Distance());
                this.endAreaList.Add(this.endPoint);

                this.totalDistance += MapManager.Instance.currentMap.pathDic[temp1].Distance();
                if (!isAuto)
                {
                    if (this.targetArea.pawns.Count > 0 
                        && FactionManager.Instance.RelationFaction(this.GetFaction(), this.targetArea.pawns[0].GetFaction()) == FactionRelation.Hostility)
                        MapManager.Instance.mouseFollower.LabelChange(this.totalDistance / this.pawnAgent.pawn.curSpeed, TipState.Battle);
                    else
                        MapManager.Instance.mouseFollower.LabelChange(this.totalDistance / this.pawnAgent.pawn.curSpeed, TipState.Walk);
                }
                if (!this.isPlay)
                    StartCoroutine(this.DrawLineByNode(nodePath.Count,isAuto));
            }
            else if (MapManager.Instance.currentMap.pathDic.ContainsKey(temp2))
            {
                this.nodePath.RemoveAt(this.nodePath.Count - 1);
                for (int i = MapManager.Instance.currentMap.pathDic[temp2].nodes.Length - 1; i >= 0; i--)
                {
                    this.nodePath.Add(MapManager.Instance.currentMap.pathDic[temp2].nodes[i].transform.position);
                }

                this.nodeCount = MapManager.Instance.currentMap.pathDic[temp2].nodes.Length;
                //Debug.Log(AreasManager.Instance.pathDic[temp2].name + "������BBBBBBBBB");

                this.startPoint = this.targetArea;

                this.areaDic.Add(this.targetArea.gameObject, true);
                this.pathList.Add(MapManager.Instance.currentMap.pathDic[temp2]);
                this.distanceDic.Add(this.endPoint.centerNode, MapManager.Instance.currentMap.pathDic[temp2].Distance());
                this.endAreaList.Add(this.endPoint);

                this.totalDistance += MapManager.Instance.currentMap.pathDic[temp2].Distance();
                if (!isAuto)
                {
                    if (this.targetArea.pawns.Count > 0
                        && FactionManager.Instance.RelationFaction(this.GetFaction(), this.targetArea.pawns[0].GetFaction()) == FactionRelation.Hostility)
                        MapManager.Instance.mouseFollower.LabelChange(this.totalDistance / this.pawnAgent.pawn.curSpeed, TipState.Battle);
                    else
                        MapManager.Instance.mouseFollower.LabelChange(this.totalDistance / this.pawnAgent.pawn.curSpeed, TipState.Walk);
                }
                if (!this.isPlay)
                    StartCoroutine(this.DrawLineByNode(nodePath.Count, isAuto));
            }
            else
            {
                if (!isAuto)
                    MapManager.Instance.mouseFollower.LabelChange(0.0f, TipState.UnWalk);
            }
        }//·������
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
                    //Debug.Log("������������������this.currentIndex: " + this.currentIndex);
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
                        //Debug.Log("��������");
                    }
                    this.currentIndex = renderWire.positionCount - 1;
                    this.isPlay = false;
                    //Debug.Log("��������");
                    if (this.nodePath.Count != renderWire.positionCount)
                    {
                        //Debug.Log("���ֻأ�����");
                        StartCoroutine(DrawLineByNode(this.nodePath.Count,isAuto));
                    }
                    else if (isAuto)//����AI�Զ���·
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
                        segmentTime = FactionManager.Instance.IsSameSide(this.currentArea.playerBelong.faction, this.GetFaction()) ? segmentTime : 2 * segmentTime;
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
                            if (this.endAreaList[this.currentWalkNode].pawns.Count > 0)
                            {

                                this.ReDrawWalkLine(lastNode + 1);
                                i = lastNode;
                                if (FactionManager.Instance.RelationFaction(this.endAreaList[this.currentWalkNode].pawns[0].GetFaction(), this.GetFaction()) == FactionRelation.Hostility
                                    && !this.pawnAgent.battleSideDic.ContainsKey(this.endAreaList[this.currentWalkNode].pawns[0]))
                                {
                                    StartBattle(this.endAreaList[this.currentWalkNode].pawns[0]);
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
                                    if (FactionManager.Instance.RelationFaction(this.endAreaList[this.currentWalkNode].pawns[0].GetFaction(), this.GetFaction()) == FactionRelation.Hostility
                                        && this.endAreaList[this.currentWalkNode].pawns[0].State != PawnState.Battle)
                                        break;
                                    yield return null;
                                }

                                this.ResetAllSupport();
                                if (this.State == PawnState.Walk)
                                    this.StateTriggerEvent(PawnState.Walk);
                                if (this.State == PawnState.Draw)
                                    yield break;

                            }
                            else if (this.endAreaList[this.currentWalkNode].buildAvatar != null && this.endAreaList[this.currentWalkNode].buildAvatar.CheckIfBattle(this))
                            {
                                BuildAvatar buildAvatar = this.endAreaList[this.currentWalkNode].buildAvatar;
                                this.ReDrawWalkLine(lastNode + 1);
                                i = lastNode;
                                this.State = PawnState.Battle;
                                BattleManager.Instance.GenerateBattleBuild(this, buildAvatar);

                                while (this.State == PawnState.Battle && this.endAreaList.Count > 0 && this.endAreaList[this.currentWalkNode].buildAvatar != null)
                                {
                                    yield return null;
                                }

                                if (this.State == PawnState.Walk)
                                    this.StateTriggerEvent(PawnState.Walk);
                                if (this.State == PawnState.Draw)
                                    yield break;

                                if (this.endAreaList.Count > 0 && this.endAreaList[this.currentWalkNode].pawns.Count > 0)//检查建筑内是否有敌人
                                {

                                    this.ReDrawWalkLine(lastNode + 1);
                                    i = lastNode;
                                    if (FactionManager.Instance.RelationFaction(this.endAreaList[this.currentWalkNode].pawns[0].GetFaction(), this.GetFaction()) == FactionRelation.Hostility
                                        && !this.pawnAgent.battleSideDic.ContainsKey(this.endAreaList[this.currentWalkNode].pawns[0]))
                                    {
                                        StartBattle(this.endAreaList[this.currentWalkNode].pawns[0]);
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
                                        if (FactionManager.Instance.RelationFaction(this.endAreaList[this.currentWalkNode].pawns[0].GetFaction(), this.GetFaction()) == FactionRelation.Hostility
                                            && this.endAreaList[this.currentWalkNode].pawns[0].State != PawnState.Battle)
                                            break;
                                        yield return null;
                                    }

                                    this.ResetAllSupport();
                                    if (this.State == PawnState.Walk)
                                        this.StateTriggerEvent(PawnState.Walk);
                                    if (this.State == PawnState.Draw)
                                        yield break;


                                }
                            }
                            else
                            {
                                if (this.endAreaList[this.currentWalkNode].AddPawn(this))//���������ɫ����ͬʱ���� ���������ӵ��˻�ȥ����
                                {
                                    this.gameObject.transform.position = this.endAreaList[this.currentWalkNode].centerNode.gameObject.transform.position;
                                    this.CurrentArea.RemovePawn(this);
                                    this.UpdateCurrentArea(this.endAreaList[this.currentWalkNode]);
                                    this.RemoveBehindLine(lastNode+2);
                                    if (this.pawnAgent.frontPlayer.IsLocalPlayer())
                                    {
                                        this.TryGetMat();//������������ͼ��ȡ���ز���
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
                        //Debug.Log("��������");
                    }

                    this.InitLine(false);
                    if (this.currentWalkNode >= this.endAreaList.Count)
                    {
                        if (this.pawnAgent.frontPlayer.CanControl())
                            AudioManager.Instance.PlayAudio(this.transform, AudioConfig.moveEnd);
                        this.State = PawnState.Wait;
                    }
                    //}
                }
            }
        }
        public void StartDrawWalkLine(List<Vector3> nodeWalk)
        {
            StartCoroutine(WalkLineByNode(nodeWalk));
        }
        #endregion

        #region//角色战斗
        public void StartBattle(PawnAvatar defender)
        {
            this.State = PawnState.Battle;
            bool defenderisInBattle = false;
            if (defender.State == PawnState.Battle)
                defenderisInBattle = true;
            else
                this.endAreaList[this.currentWalkNode].pawns[0].State = PawnState.Battle;
            BattleManager.Instance.GenerateBattlePawn(this, defender, defenderisInBattle);
        }
        public void CalcuateBattleInfo()
        {
            float attack = this.pawnAgent.pawn.curAttack;
            float defend = this.pawnAgent.pawn.curDefend;
            FightSkillType skillType = FightSkillType.None;
            List<Skill> skills = this.pawnAgent.GetSkills();
            for (int i = 0; i < skills.Count; i++)
            {
                if (skills[i] is FightSkill)
                {
                    PropertySkillEffect propertySkillEffect = skills[i].Execute(this, this);
                    attack += propertySkillEffect.changeAttack;
                    defend += propertySkillEffect.changeDefend;
                    skillType = FightSkillType.IngoreBuild;
                }
            }
            for(int i = 0; i < this.pawnAgent.supporters.Count; i++)
            {
                List<Skill> supportSkills = this.pawnAgent.supporters[i].pawnAgent.GetSkills();
                for(int j = 0; j < supportSkills.Count; j++)
                {
                    if(supportSkills[j] is SupportSkill)
                    {
                        PropertySkillEffect propertySkillEffect = supportSkills[j].Execute(this, this);
                        attack += propertySkillEffect.changeAttack;
                        defend += propertySkillEffect.changeDefend;
                    }
                }
            }
            this.pawnAgent.battleInfo = new PawnAgent.BattleInfo(attack, defend, this.pawnAgent.MoralteState(), skillType);
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
        }//����Ƿ�����
        public void CheckResult()
        {
            this.pawnAgent.MoraleChange(0);//���޸�
            this.pawnAgent.battleSideDic = new Dictionary<PawnAvatar, BattleSide>();//���޸�
            //this.ClearPawn();
            this.ResetAllSupport();
            this.PlayDeathAnim();
            if(this.pawnAgent.frontPlayer.CanControl())
                AudioManager.Instance.PlayAudio(this.transform, AudioConfig.deathCharacter);
        }//���ս�ܺ�Ľ����������
        public void CheckIfBattleResult()
        {
            this.pawnAgent.MoraleChange(0);//���޸�
            this.PlayDialogue(this.pawnAgent.pawn.winTxt);
            if (this.pawnAgent.opponents.Count > 0)
            {
                this.pawnAgent.ChooseMyOpponents(this);
            }
            else
            {
                this.pawnAgent.ResetBattleInfo();
                this.UpdatePawnState();
                Debug.Log(this.name+":"+this.State);
            }
        }//���սʤ��Ľ�����Ƿ�ս����
        private void CheckSupport()//���֧Ԯ
        {
            for(int i = 0; i < this.CurrentArea.neighboors.Count; i++)
            {
                if (!this.supportRenderWireDic.ContainsKey(this.CurrentArea.neighboors[i]))
                {
                    if(this.CurrentArea.neighboors[i].pawns.Count > 0 )
                    {
                        if (FactionManager.Instance.IsSameSide( this.CurrentArea.neighboors[i].pawns[0].GetFaction(), this.GetFaction())
                            && this.CurrentArea.neighboors[i].pawns[0].State == PawnState.Battle)
                        {
                            GameObject newLine = new GameObject();
                            newLine.name = "SupportLine";
                            newLine.AddComponent<Support>();
                            newLine.GetComponent<Support>().InitSupport(this, this.CurrentArea.neighboors[i].pawns[0]);
                            newLine.transform.parent = MapManager.Instance.currentMap.SupportLine.transform;
                            LineRenderer supportLine = newLine.AddComponent<LineRenderer>();
                            supportLine.material = this.WireMaterial;
                            supportLine.SetWidth(0.2f, 0.2f);
                            Color color1 = new Color(0,0.5f, 0, 255);
                            Color color2 = new Color(0.5f, 0, 0, 255);
                            supportLine.SetColors(this.pawnAgent.frontPlayer.faction.supportColor, this.pawnAgent.frontPlayer.faction.supportColor);
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
                    if(this.CurrentArea.neighboors[i].pawns.Count <= 0 || this.CurrentArea.neighboors[i].pawns[0].State != PawnState.Battle 
                        || !FactionManager.Instance.IsSameSide(this.CurrentArea.neighboors[i].pawns[0].GetFaction(), this.GetFaction()))
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
        public void RefreshSupportIcon()
        {
            if (this != null)
            {
                for (int i = 0; i < this.buffs.Count; i++)
                    DestroyImmediate(this.buffs[i]);
                this.buffs.Clear();

                for (int i = 0; i < this.pawnAgent.supporters.Count; i++)
                {
                    List<Skill> skills = this.pawnAgent.supporters[i].pawnAgent.GetSkills();
                    for (int j = 0; j < skills.Count; j++)
                    {
                        if (skills[j] is SupportSkill)
                        {
                            GameObject icon = Instantiate(this.bufficon.gameObject, this.buffContent.transform);
                            icon.GetComponent<Image>().sprite = skills[j].icon;
                            icon.SetActive(true);
                            this.buffs.Add(icon);
                        }
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
            for (int i = 0; i < this.buffs.Count; i++)
                DestroyImmediate(this.buffs[i]);
            this.buffs.Clear();

        }
        #endregion

        #region//角色动画
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

        #region//角色对话
        public void UpdateCurrentArea(Area area)
        {
            this.CurrentArea = area;
            if(FactionManager.Instance.RelationFaction(this.GetFaction(),this.CurrentArea.playerBelong.faction) == FactionRelation.Hostility && this.CurrentArea.type == AreaType.Base)//待修改 等胜负条件确定
            {
                GameManager.Instance.RESULTEVENT("You Lose!!", false);
            }
        }
        public void PlayDialogue(string txt)//���ŶԻ�
        {
            this.dialogueTxt.text = txt;
            if(this.dialogueAnim!=null)
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

        #region//角色属性
        public void SetStartPoint(Area area)
        {
            this.startPoint = area;
        }
        public void SetFixBtnActive(bool isActive)
        {
            this.FixBtnActive = isActive;
        }
        public Faction GetFaction()
        {
            return this.pawnAgent.frontPlayer.faction;
        }
        public bool GetFixBtnActive()
        {
           return this.FixBtnActive;
        }
        public Dictionary<GameObject, float> GetDistanceDic()
        {
            return this.distanceDic;
        }
        public List<Area> GetEndAreaList()
        {
            return this.endAreaList;
        }
        public List<Vector3> GetNodePath()
        {
            return this.nodePath;
        }
        #endregion
        public void UpdatePawnState()
        {
            this.State = this.lastState;
        }
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
            {
                if (this.pawnAgent.frontPlayer.CanControl())
                    AudioManager.Instance.PlayAudio(this.transform,AudioConfig.moveStart);
                this.PlayWalkAnim();
            }
            else if (pawnState == PawnState.Battle)
            {
                if (this.pawnAgent.frontPlayer.CanControl())
                    AudioManager.Instance.PlayAudio(this.transform, AudioConfig.battleStart);
                this.PlayAttackAnim();
            }

        }
        public void BehaviorLoading(float value)
        {
            this.ocuppyBar.value = value;
        }
        public void TryGetMat()
        {

            List<Mat> mats = new List<Mat>();
            foreach(var child in this.currentArea.mats)
            {
                if(child.Key == MatType.MilitryResource)
                {
                    this.pawnAgent.frontPlayer.ChangeMilitaryRes(child.Value.Nums());
                }
                mats.Add(child.Value);

                
            }
            for (int i = 0; i < mats.Count; i++) {
                this.currentArea.RemoveMat(mats[0]);
            }
        }
        public void ReceiveCurrentTime(int time)
        {
            int passTime = GameManager.Instance.totalTime - time;
            DialogueTriggerManager.Instance.CheckTimeflowEvent(this, passTime);
        }
        public void ClearPawn()
        {
            if (this.pawnAgent.frontPlayer.IsBot() || !this.pawnAgent.frontPlayer.IsLocalPlayer())
            {
                StaticObjGenManager.Instance.GenerateMat(this.CurrentArea, MatType.MilitryResource, this.pawnAgent.pawn.leftResNum);
                FrontManager.Instance.localPlayer.EnemiesKillNum(1);//待修改
            }
            FrontManager.Instance.RemovePawn(this.pawnAgent.frontPlayer, this);
            if (this.wire != null)
            {
                Destroy(this.wire.gameObject);
                if (this.walkWire != null)
                    Destroy(this.walkWire.gameObject);
            }
            
            this.CurrentArea.RemovePawn(this);
            //this.CurrentArea.Init();
            DialogueTriggerManager.Instance.TimeTriggerEvent -= this.ReceiveCurrentTime;
            Destroy(this.gameObject);
        }
    }

}

