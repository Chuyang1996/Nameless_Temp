using Nameless.Agent;
using Nameless.Data;
using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.DataMono
{
    public class InitArea : MonoBehaviour
    {
        public long factionId;//仅用于生成角色的区域使用 其他地方默认是1
        public int localId;
        public long areaId;
        public long buildId;
        private Area area;
        public void InitAreaInfo()
        {
            AreaAgent areaAgent = AreaFactory.GetAreaById(this.areaId);
            FrontPlayer frontPlayer = FrontManager.Instance.GetFrontPlayer(this.factionId);
            if (areaAgent.type == AreaType.Normal)
            {
                this.gameObject.AddComponent<Area>().Init(this.localId, areaAgent, frontPlayer);
                this.area = this.GetComponent<Area>();
            }
            else if (areaAgent.type == AreaType.Base)
            {
                this.gameObject.AddComponent<BaseArea>().Init(this.localId, areaAgent, frontPlayer);
                this.area = this.GetComponent<BaseArea>();
               
            }
            else if (areaAgent.type == AreaType.UnPass)
            {
                this.gameObject.AddComponent<UnPassArea>().Init(this.localId, areaAgent, frontPlayer);
                this.area = this.GetComponent<UnPassArea>();

            }
            else if (areaAgent.type == AreaType.Spawn)
            {
                this.gameObject.AddComponent<SpawnArea>().Init(this.localId, areaAgent, frontPlayer);
                this.area = this.GetComponent<SpawnArea>();
            }
        }
        public void InitBuildInfo()
        {
            if (area.type == AreaType.Normal)
            {
                if (this.buildId != 0)
                    StaticObjGenManager.Instance.GenerateBuild(null, this.area, ((BuildSkill)SkillFactory.GetSkillById(SkillFactoryType.BuildSkill, this.buildId)).build, false);
            }
            else if (area.type == AreaType.Base)
            {
                if (this.buildId != 0)
                    StaticObjGenManager.Instance.GenerateBuild(null, this.area, ((BuildSkill)SkillFactory.GetSkillById(SkillFactoryType.BuildSkill, this.buildId)).build, false);
            }
        }
        public Area GetArea()
        {
            return this.area;
        }
    }
}