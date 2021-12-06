using Nameless.ConfigData;
using Nameless.DataMono;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.Manager {
    public class MapManager : SingletonMono<MapManager>
    {

        private const string loadPath = "Prefabs/Map/";
        public MapData currentMapData;
        [HideInInspector]
        public Map currentMap;


        public MouseFollower mouseFollower;
        // Start is called before the first frame update
        public void InitMap()
        {
            this.currentMapData = DataManager.Instance.GetMapData(0);
        }
        public void UpdateNewMap(MapData mapData)
        {
            this.currentMapData = mapData;
        }
        public void NewMap(MapData mapData)
        {
            GameObject map = Instantiate(Resources.Load(loadPath + mapData.mapName) as GameObject, this.gameObject.transform);
            map.transform.localPosition = new Vector3(0, 0, 0);
            map.GetComponent<Map>().id = mapData.id;
            this.currentMap = map.GetComponent<Map>();
        }
        public void ClearMap()
        {
            Destroy(this.currentMap.gameObject);
        }
    }
}