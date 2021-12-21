using Nameless.ConfigData;
using Nameless.DataMono;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.Manager
{
    public class MapManager : SingletonMono<MapManager>
    {

        private const string loadPathMap = "Prefabs/Map/";
        private const string loadPathTransInfoShow = "Prefabs/TransShow/";
        public MapData currentMapData;
        public TransInfoShow currentTransInfoShow;
        [HideInInspector]
        public Map currentMap;


        public MouseFollower mouseFollower;
        // Start is called before the first frame update
        public void InitMap(MapData mapData)
        {
            this.currentMapData = mapData;

        }
        public void UpdateNewMap(MapData mapData)
        {
            
            this.currentMapData = mapData;

        }
        public void GenerateTransInfoShow(MapData mapData)
        {
            if (this.currentTransInfoShow != null)
                DestroyImmediate(this.currentTransInfoShow.gameObject);
            this.currentTransInfoShow = Instantiate(Resources.Load(loadPathTransInfoShow + mapData.transInfoShowName) as GameObject, this.gameObject.transform).GetComponent<TransInfoShow>();
            this.currentTransInfoShow.transform.localPosition = new Vector3(0, 0, 0);
        }
        public void GenerateMap(MapData mapData)
        {
            GameObject map = Instantiate(Resources.Load(loadPathMap + mapData.mapName) as GameObject, this.gameObject.transform);
            map.transform.localPosition = new Vector3(0, 0, 0);
            map.GetComponent<Map>().id = mapData.id;
            this.currentMap = map.GetComponent<Map>();
            List<int> defaultPos = new List<int>();
            int[] tempPos = this.StringToIntArray(mapData.defaultInitPos);
            for (int i = 0; i < tempPos.Length; i++)
                defaultPos.Add(tempPos[i]);
            this.currentMap.InitMap(defaultPos);
        }
        public void ClearMap()
        {
            if(this.currentTransInfoShow != null)
                Destroy(this.currentTransInfoShow.gameObject);
            if(this.currentMap != null)
                Destroy(this.currentMap.gameObject);
        }
        private int[] StringToIntArray(string stringlist)
        {
            int[] array;
            if (stringlist.Contains("]") && stringlist.Contains("["))
            {
                stringlist = stringlist.Remove(0, 1);
                stringlist = stringlist.Remove(stringlist.Length - 1, 1);
                array = stringlist.Contains(",") ? Array.ConvertAll<string, int>(stringlist.Split(new char[] { ',' }), s => int.Parse(s)) : new int[1] { int.Parse(stringlist) };
            }
            else
            {
                array = new int[1];
                array[0] = 0;
            }
            return array;
        }
    }
}