using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nameless.DataMono
{
    public enum BuildType
    {
        AmmoBuild = 0,
        MeidicalBuild = 1,
    }
    public class Build : MonoBehaviour
    {

        public BuildType type;
        public SpriteRenderer icon;
        // Start is called before the first frame update
        public void Init(BuildType type, Sprite sprite)
        {

            this.type = type;
            this.icon.sprite = sprite;
        }
    }
}