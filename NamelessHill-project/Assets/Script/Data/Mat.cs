using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.DataMono
{
    public enum MatType
    {
        AMMO = 0,
        MEDICINE =1,
    }
    public class Mat : MonoBehaviour
    {
        public MatType type;
        public int num;
        public SpriteRenderer icon;

        public void Init(int num, MatType type, Sprite sprite)
        {
            this.num = num;
            this.type = type;
            this.icon.sprite = sprite;
        }
    }
}