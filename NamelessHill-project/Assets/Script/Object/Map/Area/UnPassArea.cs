using Nameless.Data;
using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.DataMono
{
    public class UnPassArea : Area
    {
        // Start is called before the first frame update

        public override void Init(int id, AreaAgent areaAgent, FrontPlayer frontPlayer, long factionId)
        {
            base.Init(id, areaAgent, frontPlayer, factionId);
            this.areaSprite.color = Color.grey;
        }
        //void Start()
        //{
        //    this.areaSprite = GetComponent<SpriteRenderer>();
        //    this.areaSprite.color = Color.grey;
        //    this.type = AreaType.UnPass;
        //}

    }
}