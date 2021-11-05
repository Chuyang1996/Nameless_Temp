using Nameless.DataMono;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.Manager
{

    public class MatManager : SingletonMono<MatManager>
    {
        public Sprite ammoSprite;
        public Sprite medicineSprite;
        public Dictionary<MatType, Sprite> MatSprite = new Dictionary<MatType, Sprite>();
        public void InitMat()
        {
            this.MatSprite.Add(MatType.AMMO, this.ammoSprite);
            this.MatSprite.Add(MatType.MEDICINE, this.medicineSprite);
        }

        public void GenerateMat(Area area, MatType type, int num)
        {
            GameObject mat = Instantiate(Resources.Load("Prefabs/Mat")) as GameObject;
            mat.GetComponent<Mat>().Init(num, type, this.MatSprite[type]);
            area.AddMat(mat.GetComponent<Mat>());
        } 

    }
}