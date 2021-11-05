using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.DataMono
{
    public class UnPassArea : Area
    {
        // Start is called before the first frame update
        void Start()
        {
            this.areaSprite = GetComponent<SpriteRenderer>();
            this.areaSprite.color = Color.grey;
            this.type = AreaType.UnPass;
        }

    }
}