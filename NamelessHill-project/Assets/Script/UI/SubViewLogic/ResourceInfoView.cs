using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nameless.UI {
    public class ResourceInfoView : MonoBehaviour
    {
        public Text militartResTxt;
        // Start is called before the first frame update
        

        // Update is called once per frame
        public void Init(int ammo)
        {
            this.militartResTxt.text = ammo.ToString();
        }
    }
}