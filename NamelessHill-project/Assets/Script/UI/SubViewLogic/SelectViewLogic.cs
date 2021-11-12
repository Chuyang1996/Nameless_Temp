using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Nameless.UI
{
    public abstract class SelectViewLogic : MonoBehaviour
    {
        private GameObject canvasObj;
        private RectTransform canvasRectTransform;
        private Canvas canvas;
        private Camera canvasCam;

        public Vector2 pos;
        // Start is called before the first frame update
        void Start()
        {
            this.canvasObj = FindCanvas();
            this.canvasRectTransform = this.canvasObj.transform as RectTransform;
            this.canvas = this.canvasObj.GetComponent<Canvas>();
            this.canvasCam = this.canvasObj.GetComponent<Camera>();
        }


        public virtual GameObject GetOverUI(int index)
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;

            GraphicRaycaster gr = this.canvas.GetComponent<GraphicRaycaster>();
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(pointerEventData, results);
            if (results.Count != 0)
            {
                if (results.Count <= index)
                    return null;
                return results[index].gameObject;
            }

            return null;
        }


        //贴士鼠标跟踪
        public virtual void FollowMouseMove(GameObject item)
        {
            if (RenderMode.ScreenSpaceCamera == canvas.renderMode)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, Input.mousePosition, canvas.worldCamera, out pos);
            }
            else if (RenderMode.ScreenSpaceOverlay == canvas.renderMode)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, Input.mousePosition, canvasCam, out pos);


            }
            else
            {
                Debug.Log("请选择正确的相机模式!");
            }
        }

        private GameObject FindCanvas()
        {
            Transform temp = this.transform;
            while (true)
            {
                if (temp.GetComponent<Canvas>())
                {
                    return temp.gameObject;
                }
                else
                {
                    temp = temp.parent;
                }
            }
        }
    }
}