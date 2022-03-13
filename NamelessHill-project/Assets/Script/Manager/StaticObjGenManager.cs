using Nameless.Data;
using Nameless.DataMono;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.Manager
{
    public enum BuildIconType
    {
        None = 0,
        Building = 1,
        BullEyes = 2,
    }
    public class StaticObjGenManager : SingletonMono<StaticObjGenManager>
    {
        public Sprite militarySprite;
        public Material[] lineMaterials;
        public Material[] supportMaterials;
        public GameObject[] arrowObjs;
        public Dictionary<MatType, Sprite> MatSprite = new Dictionary<MatType, Sprite>();
        public void InitMat()
        {
            this.MatSprite.Add(MatType.MilitryResource, this.militarySprite);

        }

        public void GenerateMat(Area area, MatType type, int num)//���޸� ����Ҫ��Ҫ��AddMat�ó��� ���ܲ�ֹ����������ɲ���
        {
            if (!area.IsMatExist(type, num))
            { 
                GameObject mat =Instantiate( Resources.Load("Prefabs/Mat") )as GameObject;
                mat.GetComponent<Mat>().Init(num, type, this.MatSprite[type]);
                area.AddMat(mat.GetComponent<Mat>());
            }
        }

        public void GenerateBuild(PawnAvatar pawnAvatar, Area area, Build build, bool isBuilding)
        {
            GameManager.Instance.PauseOrPlay(true);
            if (build is Obstacle)
            {
                GameObject buildObj = Instantiate(GameManager.Instance.buildAsset.LoadAsset(build.prefabName)) as GameObject;
                buildObj.GetComponent<ObstacleAvatar>().Init(pawnAvatar, area,build,isBuilding);
            }
            else if(build is Bunker)
            {
                GameObject buildObj = Instantiate(GameManager.Instance.buildAsset.LoadAsset(build.prefabName)) as GameObject;
                buildObj.GetComponent<BunkerAvatar>().Init(pawnAvatar, area, build, isBuilding);
            }
            else if(build is Cannon)
            {
                GameObject buildObj = Instantiate(GameManager.Instance.buildAsset.LoadAsset(build.prefabName)) as GameObject;
                buildObj.GetComponent<CannonAvatar>().Init(pawnAvatar, area, build, isBuilding);
            }
            else if (build is Ammo)
            {
                GameObject buildObj = Instantiate(GameManager.Instance.buildAsset.LoadAsset(build.prefabName)) as GameObject;
                buildObj.GetComponent<AmmoAvatar>().Init(pawnAvatar, area, build, isBuilding);
            }
            else if (build is Medicine)
            {
                GameObject buildObj = Instantiate(GameManager.Instance.buildAsset.LoadAsset(build.prefabName)) as GameObject;
                buildObj.GetComponent<MedicineAvatar>().Init(pawnAvatar, area, build, isBuilding);
            }
            EventTriggerManager.Instance.CheckEventBuildOnArea(build.type,FrontManager.Instance.localPlayer);
        }

        public GameObject GenerateBuildIcon(Area area, BuildIconType buildIconType)
        {
            if (buildIconType == BuildIconType.Building)
            {
                GameObject buildObj = Instantiate(GameManager.Instance.buildAsset.LoadAsset("BuildIcon")) as GameObject;
                buildObj.transform.parent = area.centerNode.transform;
                buildObj.transform.localPosition = new Vector3(0, 0, 0);
                return buildObj;
            }
            else if (buildIconType == BuildIconType.BullEyes)
            {
                GameObject buildObj = Instantiate(GameManager.Instance.buildAsset.LoadAsset("Bullseyes")) as GameObject;
                buildObj.transform.parent = area.centerNode.transform;
                buildObj.transform.localPosition = new Vector3(0, 0, 0);
                return buildObj;
            }
            else
                return null;
        }

    }
}