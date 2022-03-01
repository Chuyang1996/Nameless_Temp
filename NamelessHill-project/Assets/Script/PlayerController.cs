using Nameless.Data;
using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.DataMono
{
    public class PlayerController : SingletonMono<PlayerController>
    {
        private bool isInBattleScene = false;
        private FrontPlayer localplayer;


        private List<PawnAvatar> pawnAvatars;

        private PawnAvatar currentPawnAvatar;
        private Build currentbuild;
        private int resCost = 0;
        private List<GameObject> buildIcon = new List<GameObject>();
        private List<GameObject> buildArea = new List<GameObject>();
        private List<Color> preColor = new List<Color>();
        private bool isBuild = false;


        private Area currentArea;
        public void InitBattlePlayer()
        {
            this.isInBattleScene = false;
            this.isBuild = false;
            this.localplayer = null;
            this.pawnAvatars = new List<PawnAvatar>();
            this.currentPawnAvatar = null;
            this.currentbuild = null;
            this.buildIcon = new List<GameObject>();
            this.buildArea = new List<GameObject>();
            this.preColor = new List<Color>();
            this.currentArea = null;
        }
        public void UpdateBattlePlayer(FrontPlayer frontPlayer)
        {
            this.localplayer = frontPlayer;
            this.pawnAvatars = this.localplayer.GetPawnAvatars();
            this.isInBattleScene = true;
            this.isBuild = false;
            this.currentArea = null;

        }
        public void ResetBattlePlayer()
        {
            this.isInBattleScene = false;
            this.isBuild = false;
            this.localplayer = null;
            this.pawnAvatars = new List<PawnAvatar>();
            this.currentPawnAvatar = null;
            this.currentbuild = null;
            this.buildIcon = new List<GameObject>();
            this.buildArea = new List<GameObject>();
            this.preColor = new List<Color>();
        }

        public void FindAllBuildingArea(Area area, Build build, PawnAvatar builder, int costRes)
        {
            this.isBuild = true;
            this.currentbuild = build;
            this.currentPawnAvatar = builder;
            this.resCost = costRes;

            List<Area> areas = new List<Area>();
            for(int i = 0; i < area.neighboors.Count; i++)
            {
                if (area.neighboors[i].CanBuild(builder.GetFaction()))
                {
                    areas.Add(area.neighboors[i]);
                    this.buildArea.Add(area.neighboors[i].gameObject);
                }
            }

            for(int i = 0; i < areas.Count; i++)
            {
                this.buildIcon.Add(StaticObjGenManager.Instance.GenerateBuildIcon(areas[i], BuildIconType.Building));
                this.preColor.Add(areas[i].GetColor());
                this.buildArea[i].GetComponent<Area>().SetColor(new Color(1, 1, 1, 0.3f), true, false);

            }
        }

        public void ResetAllBuildingBtn()
        {
            for(int i = 0; i < this.buildIcon.Count; i++)
                DestroyImmediate(this.buildIcon[i]);
            this.buildIcon.Clear();
            for (int i = 0; i < this.buildArea.Count; i++)
                this.buildArea[i].GetComponent<Area>().SetColor(this.preColor[i], true, false);
            this.buildArea.Clear();
            this.currentbuild = null;

        }
        // Update is called once per frame
        void Update()
        {
            if (this.isInBattleScene && !this.isBuild)
            {
                Vector2 raySelect = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(raySelect, Vector2.zero);
                if (hit.collider != null && hit.collider.gameObject.GetComponent<Area>() != null && FrontManager.Instance.localPlayer.ContainArea(hit.collider.gameObject.GetComponent<Area>()))
                {
                    if (this.currentArea == null)
                    {
                        this.currentArea = hit.collider.gameObject.GetComponent<Area>();
                        this.currentArea.SetColor(new Color(1, 1, 1, 0.3f), false, false);
                    }
                    else
                    {
                        if (hit.collider.gameObject.GetComponent<Area>() != this.currentArea)
                        {
                            this.currentArea.SetColor(this.currentArea.recordColor, false, false);
                            this.currentArea = hit.collider.gameObject.GetComponent<Area>();
                            this.currentArea.SetColor(new Color(1, 1, 1, 0.3f), false, false);
                        }
                    }
                }
                else
                {
                    if (this.currentArea != null)
                    {
                        this.currentArea.SetColor(this.currentArea.recordColor, false, false);
                    }
                }
                Ray targetray1 = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit TargetHit1;
                //Debug.Log("sssss");
                if (Physics.Raycast(targetray1, out TargetHit1))
                {

                    if (TargetHit1.transform.gameObject.GetComponent<PawnAvatar>() != null
                        && TargetHit1.transform.gameObject.GetComponent<PawnAvatar>().pawnAgent.frontPlayer == FrontManager.Instance.localPlayer)//���޸�.AI
                    {
                        PawnAvatar pawnAvatar = TargetHit1.collider.gameObject.GetComponent<PawnAvatar>();
                        if (Input.GetMouseButtonDown(1))//
                        {
                            this.currentPawnAvatar = pawnAvatar;

                            GameManager.Instance.characterView.SetNewPawn(this.currentPawnAvatar);

                        }else if (Input.GetMouseButtonDown(0))
                        {
                            this.currentPawnAvatar = pawnAvatar;
                        }
                    }


                }

                if (Input.GetMouseButton(0) /*&& this.State != PawnState.Battle*/)
                {
                    Ray targetray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit TargetHit3;
                    //Debug.Log("sssss");
                    if (this.currentPawnAvatar != null && Physics.Raycast(targetray, out TargetHit3))
                    {
                        if (TargetHit3.transform.gameObject == this.currentPawnAvatar.gameObject)
                        {

                            this.currentPawnAvatar.targetArea = this.currentPawnAvatar.CurrentArea;
                            this.currentPawnAvatar.SetStartPoint(this.currentPawnAvatar.CurrentArea);
                            this.currentPawnAvatar.State = PawnState.Draw;
                            MapManager.Instance.mouseFollower.gameObject.GetComponent<SpriteRenderer>().sprite = this.currentPawnAvatar.pawnAgent.pawn.selectIcon;
                            this.currentPawnAvatar.InitLine(true);
                            this.currentPawnAvatar.ShowPath(true);

                        }

                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    if (this.currentPawnAvatar != null && this.currentPawnAvatar.State == PawnState.Draw)
                    {
                        MapManager.Instance.mouseFollower.gameObject.GetComponent<SpriteRenderer>().sprite = null;
                        MapManager.Instance.mouseFollower.ResetState();
                        if (this.currentPawnAvatar.GetEndAreaList().Count > 0 && this.currentPawnAvatar.GetDistanceDic().Count > 0)
                        {

                            this.currentPawnAvatar.ShowPath(false);
                            PathManager.Instance.AddPath(this.currentPawnAvatar);
                            this.currentPawnAvatar.State = PawnState.Walk;
                            List<Vector3> tempNode = this.currentPawnAvatar.GetNodePath();
                            this.currentPawnAvatar.StartDrawWalkLine(tempNode);
                            //this.InitWalkLine();
                            //StartCoroutine(ShowWalkPath(start, this.pathList[0].nodes.Length - 1, t));
                        }
                        else
                        {
                            this.currentPawnAvatar.State = PawnState.Wait;
                        }

                    }
                }
            }
            else if (this.isBuild)
            {
                Vector2 raySelect = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(raySelect, Vector2.zero);
                if (Input.GetMouseButtonDown(0))
                {
                    GameManager.Instance.PauseOrPlay(true);
                    if(hit.collider!=null && this.buildArea.Contains(hit.collider.gameObject))
                    {
                        FrontManager.Instance.localPlayer.ChangeMilitaryRes(this.resCost);
                        StaticObjGenManager.Instance.GenerateBuild(this.currentPawnAvatar, hit.collider.gameObject.GetComponent<Area>(), this.currentbuild, true);

                    }
                    this.resCost = 0;
                    this.ResetAllBuildingBtn();

                    this.isBuild = false;

                }
                if (hit.collider != null && this.buildArea.Contains(hit.collider.gameObject))
                {
                    for(int i = 0; i < this.buildArea.Count; i++)
                    {
                        this.buildArea[i].GetComponent<Area>().SetColor(new Color(1, 1, 1, 0.3f),true, false);
                    }
                    hit.collider.gameObject.GetComponent<Area>().SetColor(new Color(1, 1, 1, 0.5f), true, false);
                }
            }
        }
    }
}