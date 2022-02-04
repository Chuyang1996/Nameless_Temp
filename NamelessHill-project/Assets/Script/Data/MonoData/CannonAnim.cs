using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.DataMono {
    public class CannonAnim : MonoBehaviour
    {
        public CannonAvatar cannonAvatar;

        public void FireEvent()
        {
            this.cannonAvatar.GenTarget();
        }

        public void DeathEvent()
        {
            this.cannonAvatar.DestoryBuilding();
        }

        // Start is called before the first frame update
    }
}