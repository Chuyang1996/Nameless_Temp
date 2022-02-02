using Nameless.Data;
using Nameless.DataMono;
using Nameless.Manager;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Nameless.Data
{
    public class NodeToNode
    {

        public GameObject start;
        public GameObject end;

        public NodeToNode(GameObject start, GameObject end)
        {
            this.start = start;
            this.end = end;
        }
        public override bool Equals(object obj)
        {
            NodeToNode temp = (NodeToNode)obj;
            if (this.start == temp.start && this.end == temp.end)
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return 1;

        }

    }
}
namespace Nameless.DataMono
{
    public class Map : MonoBehaviour
    {
        public long id;

        public Sprite sprite;
        public Sprite spriteAccessbility;
        public SpriteRenderer bg;
        public GameObject PathLine;
        public GameObject SupportLine;
        public GameObject BattleCollect;
        public GameObject PawnCollect;
        public float[,] areaMatrix;
        public List<InitArea> initAreas;
        public List<Area> areas;
        public List<Path> paths;
        public Dictionary<NodeToNode, Path> pathDic = new Dictionary<NodeToNode, Path>();

        private List<int> defaultPoss;
        public void InitMap(List<int> defaultPoss)
        {
            this.defaultPoss = defaultPoss;
        }
        // Start is called before the first frame update
        public void InitArea()
        {
            this.bg.sprite = GameManager.Instance.accessbility ? spriteAccessbility : sprite;
            for(int i = 0;i<this.initAreas.Count;i++)
            {
                this.initAreas[i].InitAreaInfo();
                this.areas.Add(this.initAreas[i].GetArea());
            }
            for (int i = 0; i < this.initAreas.Count; i++)
            {
                this.initAreas[i].InitBuildInfo();
            }
            for (int i = 0; i < paths.Count; i++)
            {
                NodeToNode temp = new NodeToNode(paths[i].nodes[0], paths[i].nodes[paths[i].nodes.Length - 1]);
                this.pathDic.Add(temp, paths[i]);

            }
            Dictionary<GameObject, Area> tempDic = new Dictionary<GameObject, Area>();
            for(int i = 0;i< this.areas.Count; i++)
            {
                tempDic.Add(this.areas[i].centerNode, this.areas[i]);
            }
            for(int i = 0; i < this.paths.Count; i++)
            {
                tempDic[this.paths[i].nodes[0]].neighboors.Add(tempDic[this.paths[i].nodes[this.paths[i].nodes.Length - 1]]);
                tempDic[this.paths[i].nodes[this.paths[i].nodes.Length - 1]].neighboors.Add(tempDic[this.paths[i].nodes[0]]);
            }
            for(int i = 0;i< this.areas.Count; i++)
            {
                this.areas[i].localId = i + 1;
            }
            this.areaMatrix = new float[areas.Count, areas.Count];
            for(int i = 0; i < this.areaMatrix.GetLength(0); i++)
            {
                
                for(int j = 0; j < this.areaMatrix.GetLength(1); j++)
                {
                    NodeToNode temp1 = new NodeToNode(this.areas[i].centerNode, this.areas[j].centerNode);
                    NodeToNode temp2 = new NodeToNode(this.areas[j].centerNode, this.areas[i].centerNode);
                    if (i == j)
                    {
                        this.areaMatrix[i, j] = 0.0f;
                    }
                    else if (this.pathDic.ContainsKey(temp1))
                    {
                        this.areaMatrix[i,j] = this.pathDic[temp1].Distance();
                    }
                    else if(this.pathDic.ContainsKey(temp2))
                    {
                        this.areaMatrix[i, j] = this.pathDic[temp2].Distance();
                    }
                    else
                    {
                        this.areaMatrix[i, j] = float.MaxValue;
                    }
                }
            }
             
        }

        // Update is called once per frame
        public Area FindAreaByLocalId(int id)
        {
            return this.areas.Where(_area => _area.localId == id).FirstOrDefault();
        }

        public List<int> GetDefaultPos()
        {
            return this.defaultPoss;
        }
        public  List<int>[] Dijkstra(float[,] graphic, int start)//打印结果为以start为起始点到达其他位置的所有最短路径
        {
            int n = graphic.GetLength(0);
            int[] visit = new int[n];
            float[] dist = new float[n];
            List<int>[] path = new List<int>[n];
            for (int i = 0; i < n; i++)
            {
                dist[i] = graphic[start, i];
                List<int> temp = new List<int>();
                temp.Add(start);
                if (graphic[start, i] != -1)
                {
                    temp.Add(i);
                }
                path[i] = temp;
            }
            visit[start] = 1;
            for (int i = 0; i < n; i++)
            {
                float min_dist = float.MaxValue;
                int middle = 0;

                for (int j = 0; j < n; j++)
                {
                    if (visit[j] == 0 && ((min_dist > dist[j])))
                    {
                        min_dist = dist[j];
                        middle = j;
                    }
                }

                for (int j = 0; j < n; j++)
                {
                    if (visit[j] == 0 && (dist[j] > dist[middle] + graphic[middle, j]))
                    {

                        dist[j] = dist[middle] + graphic[middle, j];
                        List<int> temp = new List<int>();
                        for (int m = 0; m < path[middle].Count; m++)
                        {
                            temp.Add(path[middle][m]);
                        }
                        temp.Add(j);
                        path[j] = temp;

                    }
                }

                visit[middle] = 1;
            }

            return path;
        }



    }
}
