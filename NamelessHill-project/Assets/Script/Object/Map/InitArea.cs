using Nameless.Agent;
using Nameless.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.DataMono
{
    public class InitArea : MonoBehaviour
    {
        public int localId;
        public long areaId;
        private Area area;
        public void InitAreaInfo()
        {
            AreaAgent areaAgent = AreaFactory.GetAreaById(this.areaId);
            if (areaAgent.type == AreaType.Normal)
            {
                this.gameObject.AddComponent<Area>().Init(this.localId, areaAgent);
                this.area = this.GetComponent<Area>();
            }
            else if (areaAgent.type == AreaType.Base)
            {
                this.gameObject.AddComponent<BaseArea>().Init(this.localId, areaAgent);
                this.area = this.GetComponent<BaseArea>();
            }
            else if (areaAgent.type == AreaType.UnPass)
            {
                this.gameObject.AddComponent<UnPassArea>().Init(this.localId, areaAgent);
                this.area = this.GetComponent<UnPassArea>();

            }
            else if (areaAgent.type == AreaType.Spawn)
            {
                this.gameObject.AddComponent<SpawnArea>().Init(this.localId, areaAgent);
                this.area = this.GetComponent<SpawnArea>();
            }
        }

        public Area GetArea()
        {
            return this.area;
        }
    }
}