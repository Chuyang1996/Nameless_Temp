using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nameless.DataMono
{
    public enum MatType
    {
        MilitryResource = 0,
    }
    public class Mat : MonoBehaviour
    {
        public MatType type;
        public Text numtxt;
        public SpriteRenderer icon;

        private int num;
        public void Init(int num, MatType type, Sprite sprite)
        {
            this.num = num;
            this.numtxt.text = num.ToString();
            this.type = type;
            this.icon.sprite = sprite;
        }

        public int Nums()
        {
            return num;
        }
        public void AddMat(int num)
        {
            this.num += num;
            this.numtxt.text = this.num.ToString();
        }
    }
}