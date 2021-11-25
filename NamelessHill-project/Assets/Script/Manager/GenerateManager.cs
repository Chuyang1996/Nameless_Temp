using Nameless.DataMono;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.Manager
{

    public class GenerateManager : SingletonMono<GenerateManager>
    {
        public Sprite militarySprite;
        public Sprite ammoSprite;
        public Sprite medicialSprite;
        public Dictionary<MatType, Sprite> MatSprite = new Dictionary<MatType, Sprite>();
        public Dictionary<BuildType, Sprite> BuildSprite = new Dictionary<BuildType, Sprite>();
        public void InitMat()
        {
            this.MatSprite.Add(MatType.MilitryResource, this.militarySprite);

            this.BuildSprite.Add(BuildType.AmmoBuild, this.ammoSprite);
            this.BuildSprite.Add(BuildType.MeidicalBuild, this.medicialSprite);
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

        public void GenerateBuild(Area area, BuildType type)//待修改 看看要不要将addBuild拿出来 可能不止有区域会生成建筑
        {
            if (!area.IsBuildExist(type))
            {
                GameObject build = Instantiate(Resources.Load("Prefabs/Build")) as GameObject;
                build.GetComponent<Build>().Init(type, this.BuildSprite[type]);
                area.AddBuild(build.GetComponent<Build>());
            }
        }

    }
}