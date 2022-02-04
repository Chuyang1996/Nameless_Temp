using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.DataMono
{
    public class ExploreAnim : MonoBehaviour
    {
        public CannonExploreBullsEye cannonExploreBullsEye;

        public void ExploreEvent()
        {
            this.cannonExploreBullsEye.explores.Remove(this.gameObject);
        }
    }
}