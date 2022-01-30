using Nameless.Data;
using Nameless.DataMono;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.Manager
{

    public class StaticObjGenManager : SingletonMono<StaticObjGenManager>
    {
        public Sprite militarySprite;
        public Dictionary<MatType, Sprite> MatSprite = new Dictionary<MatType, Sprite>();
        public void InitMat()
        {
            this.MatSprite.Add(MatType.MilitryResource, this.militarySprite);

        }

        public void GenerateMat(Area area, MatType type, int num)//待修改 看看要不要将AddMat拿出来 可能不止有区域会生成材料
        {
            if (!area.IsMatExist(type, num))
            { 
                GameObject mat =Instantiate( Resources.Load("Prefabs/Mat") )as GameObject;
                mat.GetComponent<Mat>().Init(num, type, this.MatSprite[type]);
                area.AddMat(mat.GetComponent<Mat>());
            }
        }

        public void GenerateBuild(PawnAvatar pawnAvatar, Area area, Build build)
        {

            if(build is Obstacle)
            {
                GameObject buildObj = Instantiate(Resources.Load("Prefabs/Build/" + build.prefabName)) as GameObject;
                area.AddBuild(buildObj.GetComponent<ObstacleAvatar>());
                buildObj.GetComponent<ObstacleAvatar>().Init(pawnAvatar, area,build);
            }
            else if(build is Bunker)
            {
                GameObject buildObj = Instantiate(Resources.Load("Prefabs/Build/" + build.prefabName)) as GameObject;
                area.AddBuild(buildObj.GetComponent<BunkerAvatar>());
                buildObj.GetComponent<BunkerAvatar>().Init(pawnAvatar, area, build);
            }
            else if(build is Cannon)
            {
                GameObject buildObj = Instantiate(Resources.Load("Prefabs/Build/" + build.prefabName)) as GameObject;
                area.AddBuild(buildObj.GetComponent<CannonAvatar>());
                buildObj.GetComponent<CannonAvatar>().Init(pawnAvatar, area, build);
            }
            EventTriggerManager.Instance.CheckEventBuildOnArea(build.type);
        }

        public GameObject GenerateBuildIcon(Area area)
        {
            GameObject buildObj = Instantiate(Resources.Load("Prefabs/Build/BuildIcon")) as GameObject;
            buildObj.transform.parent = area.centerNode.transform;
            buildObj.transform.localPosition = new Vector3(0, 0, 0);
            return buildObj;
        }

    }
}