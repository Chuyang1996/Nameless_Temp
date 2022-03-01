using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Nameless.UI
{
    public class MouseFollowView : SelectViewLogic
    {
        public GameObject buildIcon;
        // Update is called once per frame
        void Update()
        {
            this.FollowMouseMove(buildIcon);
        }

        public override void FollowMouseMove(GameObject item)
        {
            base.FollowMouseMove(item);
            RectTransform rectTransform = item.transform as RectTransform;
            item.GetComponent<RectTransform>().anchoredPosition = pos;

        }
    }
}