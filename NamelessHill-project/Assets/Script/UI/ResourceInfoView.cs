using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nameless.UI {
    public class ResourceInfoView : MonoBehaviour
    {
        public Text ammoTxt;
        public Text medicineTxt;
        // Start is called before the first frame update
        

        // Update is called once per frame
        public void Init(int ammo,int medicine)
        {
            this.ammoTxt.text = ammo.ToString();
            this.medicineTxt.text = medicine.ToString();
        }
    }
}