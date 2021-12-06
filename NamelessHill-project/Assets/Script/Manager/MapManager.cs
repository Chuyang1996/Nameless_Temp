using Nameless.ConfigData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.Manager {
    public class MapManager : SingletonMono<MapManager>
    {
        private const string loadPath = "Prefabs/Map/";
        public Map currentMap;
        // Start is called before the first frame update
        public void InitMap()
        {
            this.currentMap = this.NewMap(DataManager.Instance.GetMapData(0)); 
        }
        public Map NewMap(MapData mapData)
        {
            GameObject map = Instantiate(Resources.Load(loadPath + mapData.mapName) as GameObject);
            return map.GetComponent<Map>();
        }
        public void ClearMap()
        {
            Destroy(this.currentMap.gameObject);
        }
    }
}