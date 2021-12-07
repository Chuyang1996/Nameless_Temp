using Nameless.ConfigData;
using Nameless.DataMono;
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
        }
        public void ClearMap()
        {
            Destroy(this.currentTransInfoShow.gameObject);
            Destroy(this.currentMap.gameObject);
        }
    }
}