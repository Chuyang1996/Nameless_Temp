using Nameless.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nameless.DataMono
{
    public class CannonExploreBullsEye : MonoBehaviour
    {
        public GameObject canvas;
        public Text fallTimeCountText;

        private Area targetArea = null;
        private List<Area> targetExploreAreas = new List<Area>();


        private Color targetColor;
        private List<Color> targetExploreColor = new List<Color>();

        private CannonAvatar cannonAvatar;
        private float fallCountTime = 0.0f;

        private bool isDoneInit = false;


        public List<GameObject> explores = new List<GameObject>();
        public ExploreAnim exploreAnimTemplate;
        private bool isExplore = false;
        public void Init(Area target, List<Area> targetExploreAreas, CannonAvatar cannonAvatar)
        {
            this.targetArea = target;
            this.targetExploreAreas = targetExploreAreas;
            this.targetColor = target.GetColor();
            for (int i = 0; i < this.targetExploreAreas.Count; i++)
            {
                this.targetExploreColor.Add(this.targetExploreAreas[i].GetColor());
            }
            this.targetArea.SetColor(new Color(1, 1, 0, 0.5f));
            for (int i = 0; i < this.targetExploreAreas.Count; i++)
            {
                this.targetExploreAreas[i].SetColor(new Color(1, 1, 0, 0.5f));
            }
            this.cannonAvatar = cannonAvatar;
            this.fallCountTime = cannonAvatar.cannon.expolreTime;
            this.isDoneInit = true;
            this.isExplore = false;


            for (int i = 0; i < this.targetExploreAreas.Count; i++)
            {
               GameObject exploreTemp = Instantiate(this.exploreAnimTemplate.gameObject, this.targetExploreAreas[i].centerNode.transform) as GameObject;
               exploreTemp.transform.localPosition = new Vector3(0, 0, 0);
               exploreTemp.transform.parent = this.gameObject.transform;
               this.explores.Add(exploreTemp);
            }
        }

        private void Update()
        {
            if (this.isDoneInit)
            {
                ExploreFallTime();
            }
        }

        private void ExploreFallTime()
        {
            if (this.fallCountTime < 0)
            {
                if(!this.isExplore)
                    this.CalucateDamage();
                if(this.explores.Count == 0)
                    this.DestroyExplore();
            }
            else
            {
                this.fallTimeCountText.text = ((int)(this.fallCountTime)).ToString();
                this.fallCountTime -= Time.deltaTime;
            }
        }
        private void CalucateDamage()
        {
            for(int i = 0; i < this.explores.Count; i++)
            {
                this.explores[i].SetActive(true);
            }
            this.SingAreaDamage(this.targetArea, this.cannonAvatar.cannon.exploreDamage);
            for (int i = 0; i < this.targetExploreAreas.Count; i++)
            {
                this.SingAreaDamage(this.targetExploreAreas[i], this.cannonAvatar.cannon.exploreDamage * 0.5f);
            }
            this.isExplore = true;
            this.ResetState();
        }
        private void SingAreaDamage(Area area, float damge)
        {
            if (area.pawns.Count > 0)
            {
                area.pawns[0].pawnAgent.HealthChange(-damge);
                if (area.pawns[0].IsFail())
                {
                    if (area.pawns[0].pawnAgent.opponents.Count == 0)//判断是否可以进行战斗结算
                        area.pawns[0].CheckResult();
                }
            }
            if (area.buildAvatar != null)
            {
                area.buildAvatar.HealthChange(-damge);
                if (area.buildAvatar.IsFail())
                {
                    if (area.buildAvatar.pawnOpponents.Count == 0)//判断是否可以进行战斗结算
                    {
                        area.buildAvatar.CheckResult();
                    }
                }
            }
        }
        private void ResetState()
        {
            this.canvas.SetActive(false);
            this.cannonAvatar.ResetState();
            this.targetArea.SetColor(this.targetColor);
            for (int i = 0; i < this.targetExploreAreas.Count; i++)
            {
                this.targetExploreAreas[i].SetColor(this.targetExploreColor[i]);
            }
            this.targetArea = null;
            this.targetExploreAreas = new List<Area>();
            this.targetExploreColor = new List<Color>();

        }

        public void DestroyExplore()
        {
            DestroyImmediate(this.gameObject);
        }
    }
}