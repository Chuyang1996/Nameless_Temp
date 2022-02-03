using Nameless.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nameless.DataMono
{
    public class CannonExploreBullsEye : MonoBehaviour
    {
        public Text fallTimeCountText;

        private Area targetArea = null;
        private List<Area> targetExploreAreas = new List<Area>();


        private Color targetColor;
        private List<Color> targetExploreColor = new List<Color>();

        private CannonAvatar cannonAvatar;
        private float fallCountTime = 0.0f;

        private bool isDoneInit = false;
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
            isDoneInit = true;
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
                this.CalucateDamage();
                this.ResetState();
            }
            else
            {
                this.fallTimeCountText.text = ((int)(this.fallCountTime)).ToString();
                this.fallCountTime -= Time.deltaTime;
            }
        }
        private void CalucateDamage()
        {
            this.SingAreaDamage(this.targetArea, this.cannonAvatar.cannon.exploreDamage);
            for (int i = 0; i < this.targetExploreAreas.Count; i++)
            {
                this.SingAreaDamage(this.targetExploreAreas[i], this.cannonAvatar.cannon.exploreDamage * 0.5f);
            }
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
            this.cannonAvatar.ResetState();
            this.targetArea.SetColor(this.targetColor);
            for (int i = 0; i < this.targetExploreAreas.Count; i++)
            {
                this.targetExploreAreas[i].SetColor(this.targetExploreColor[i]);
            }
            this.targetArea = null;
            this.targetExploreAreas = new List<Area>();
            this.targetExploreColor = new List<Color>();

            DestroyImmediate(this.gameObject);
        }
    }
}