#define DEBUG

using Nameless.Data;
using Nameless.DataMono;
using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

namespace Nameless.Controller
{
    public class Win32
    {
        [DllImport("User32.Dll")]
        public static extern long SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(out POINT lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }
    }
    public struct TransitionTarget
    {
        public Vector3 targetPos;
        public float targetZoom;
        public float waitTime;

        public float speedZoom;
        public float speedPos;
        public TransitionTarget(Vector3 targetPos, float targetZoom, float waitTime,float speedPos, float speedZoom)
        {
            this.targetPos = targetPos;
            this.targetZoom = targetZoom;
            this.waitTime = waitTime;

            this.speedPos = speedPos;
            this.speedZoom = speedZoom;
        }
    }
    public class RTSCamera : SingletonMono<RTSCamera>
    {


        public float rangeX = 5;
        public float rangeY = 5;
        [SerializeField] private float orthographicSizeMin = 5;
        [SerializeField] private float orthographicSizeMax = 30;

        [SerializeField] private float speed = 3.0f;
        //[SerializeField] private float dragSpeed = 3.0f;

        private Camera _camera;
        private Win32.POINT lastTimePos;
        private int lastMouseX;
        private int lastMouseY;
        public bool Locked = false;

        //private float _moveToSpeed = 6.0f;
        //private Vector3 _moveToTarget;
        //private bool _moveToCanBeInterupted = true;
        public bool _isTranstionTo = false;
        public bool _clickToNext = false;
        //private bool _lockControlWhileMoving = false;

        //private float _targetZoom;
        //private bool _isZoomingTo = false;
        //private bool _zoomCanBeInterupted = true;
        //private bool _lockZoomWhileZoomingTo = false;
        //private float _zoomToSpeed = 6.0f;

        public void InitCamera()
        {
            Cursor.lockState = CursorLockMode.Confined;
            _camera = GetComponent<Camera>();
        }
        public void StartTransition(Stack<DialoguePawn> transitionTargets)
        {
            this._isTranstionTo = true;
            StartCoroutine(TransitionToCoroutine(transitionTargets));
        }
        public void ResetCameraPos()
        {
            this.transform.position = new Vector3(0,0,-10);
            Camera.main.orthographicSize = 17;
        }
        public void SetCameraPos(Vector2 pos)
        {
            this.transform.position = new Vector3(pos.x, pos.y, -10);
        }
        IEnumerator TransitionToCoroutine(Stack<DialoguePawn> transitionTargets)//??????
        {
            if (transitionTargets.Count > 0)
            {
                DialoguePawn dialoguePawn = transitionTargets.Pop();
                bool GetPosTarget = false;
                bool GetZoomTarget = false;
                Vector3 targetpos = new Vector3(dialoguePawn.pawnAvatar.transform.position.x, dialoguePawn.pawnAvatar.transform.position.y, -10.0f);
                TransitionTarget transitionTarget = new TransitionTarget(targetpos, dialoguePawn.dialogue.zoom, dialoguePawn.dialogue.waitTime, dialoguePawn.dialogue.speedPos, dialoguePawn.dialogue.zoomSpeed);
                while (transform.position != transitionTarget.targetPos || _camera.orthographicSize != transitionTarget.targetZoom)
                {
                    if (!GameManager.Instance.isPlay)
                    {
                        yield return null;
                    }
                    else
                    {
                        if (transform.position == transitionTarget.targetPos)
                            GetPosTarget = true;
                        else
                            transform.position = Vector3.MoveTowards(transform.position, transitionTarget.targetPos, Time.deltaTime * transitionTarget.speedPos);
                        if (_camera.orthographicSize == transitionTarget.targetZoom)
                            GetZoomTarget = true;
                        else
                            _camera.orthographicSize += Mathf.Clamp(-transitionTarget.speedZoom * Time.deltaTime, transitionTarget.targetZoom - _camera.orthographicSize, transitionTarget.speedZoom * Time.deltaTime);
                        if (GetZoomTarget && GetPosTarget)
                            break;
                        yield return null;
                    }
                }
                dialoguePawn.pawnAvatar.ShowDialogue(dialoguePawn.dialogue.dialogueTxt);
                while (!dialoguePawn.dialogue.isAuto)
                {
                    if (!GameManager.Instance.isPlay)
                    {
                        yield return null;
                        continue;
                    }
                    else
                    {
                        if (this._clickToNext)
                        {
                            if (dialoguePawn.dialogue.NextDialogue() != null)
                            {
                                dialoguePawn.dialogue = dialoguePawn.dialogue.NextDialogue();
                                dialoguePawn.pawnAvatar.ShowDialogue(dialoguePawn.dialogue.dialogueTxt);
                            }
                            else
                            {
                                this._clickToNext = false;
                                break;
                            }
                        }
                        yield return null;
                    }
                }
                if (dialoguePawn.dialogue.isAuto)
                {
                    do
                    {
                        dialoguePawn.pawnAvatar.ShowDialogue(dialoguePawn.dialogue.dialogueTxt);
                        yield return new WaitForSecondsRealtime(transitionTarget.waitTime);
                        dialoguePawn.dialogue = dialoguePawn.dialogue.NextDialogue();

                    }
                    while (dialoguePawn.dialogue != null);
                }
                dialoguePawn.pawnAvatar.StopDialogue();
                if (dialoguePawn.dialogue != null && dialoguePawn.dialogue.FindTargetDialoguePawn() != null)
                {
                    DialoguePawn newPawn = new DialoguePawn(dialoguePawn.dialogue.FindTargetDialoguePawn(), new Dialogue(-1, "-1", new ConditionDialogue(ConditionDialogueType.None, 0), true, 4.0f, 20.0f, 5.0f, 1.0f, -1, -1));
                    //DialogueTriggerManager.Instance.PushNewDialoguePawn(newPawn);
                }

                StartCoroutine(TransitionToCoroutine(transitionTargets));
            }
            else
            {
                //DialogueTriggerManager.Instance.isShowDialogue = false;
                this._isTranstionTo = false;
            }
        }

        //IEnumerator MoveToCoroutine()
        //{
        //    while (_isMovingTo && transform.position != _moveToTarget)
        //    {
        //        transform.position = Vector3.MoveTowards(transform.position, _moveToTarget, Time.deltaTime * _moveToSpeed);
        //        if (transform.position == _moveToTarget)
        //            break;
        //        yield return null;
        //    }
        //    _isMovingTo = false;
        //}

        //IEnumerator ZoomToCoroutine()
        //{
        //    while (_isZoomingTo && _camera.orthographicSize != _targetZoom)
        //    {
        //        _camera.orthographicSize += Mathf.Clamp(-_zoomToSpeed * Time.deltaTime, _targetZoom - _camera.orthographicSize, _zoomToSpeed * Time.deltaTime);
        //        if (_camera.orthographicSize == _targetZoom)
        //            break;
        //        yield return null;
        //    }
        //    _isZoomingTo = false;
        //}

        //public void MoveToWorldPoint(Vector3 pos, float speed = 6.0f, bool canBeInterupted = true, bool lockControlWhileMoving = false)
        //{
        //    StopCoroutine("MoveToCoroutine");
        //    _moveToTarget = pos;
        //    _moveToTarget.z = _camera.transform.position.z;
        //    _moveToSpeed = speed;
        //    _isMovingTo = true;
        //    _moveToCanBeInterupted = canBeInterupted;
        //    _lockControlWhileMoving = lockControlWhileMoving;

        //    if (canBeInterupted && lockControlWhileMoving)
        //        Debug.Log("Warning: You cannot interupt moving while it is locked");

        //    StartCoroutine("MoveToCoroutine");
        //}

        //public void ZoomTo(float zoomLevel, float zoomSpeed = 3.0f, bool canBeInterupted = true, bool lockZoomWhileZoomingTo = false)
        //{
        //    StopCoroutine("ZoomToCoroutine");
        //    _targetZoom = Mathf.Clamp(orthographicSizeMin, zoomLevel, orthographicSizeMax);
        //    _zoomToSpeed = zoomSpeed;
        //    _isZoomingTo = true;
        //    _zoomCanBeInterupted = canBeInterupted;
        //    _lockZoomWhileZoomingTo = lockZoomWhileZoomingTo;

        //    if (canBeInterupted && lockZoomWhileZoomingTo)
        //        Debug.Log("Warning: You cannot interupt zooming while it is locked");

        //    StartCoroutine("ZoomToCoroutine");
        //}



        //public void StopZoomTo()
        //{
        //    _isMovingTo = false;
        //    StopCoroutine("ZoomToCoroutine");
        //}

        //public void StopMoveTo()
        //{
        //    _isMovingTo = false;
        //    StopCoroutine("MoveToCoroutine");
        //}

        public void StopTransition()
        {
            _isTranstionTo = false;
            StopCoroutine("TransitionToCoroutine");
        }

#if DEBUG
        void Update()
        {
            //if (Input.GetKeyDown(KeyCode.F))
            //    MoveToWorldPoint(_camera.ScreenToWorldPoint(Input.mousePosition), 12.0f, true);

            //if (Input.GetKeyDown(KeyCode.G))
            //    ZoomTo(5.0f, 6.0f, true);

        }
#endif


        // Update is called once per frame
        void LateUpdate()
        {
            if (GameManager.Instance.GameScene != GameScene.Battle)
                return;
            //if (Input.GetMouseButton(2))
            //{
            //    Cursor.visible = false;
            //}
            //else if (Input.GetMouseButtonUp(2))
            //{
            //    Cursor.visible = true;
            //}
            if (Locked || !GameManager.Instance.isPlay)
                return;
            if (this._isTranstionTo)
            {
                if (Input.GetMouseButtonDown(0))
                    this._clickToNext = true;
                return;
            }

            float move_x, move_y;

            move_x = Input.GetAxis("Horizontal") * speed;
            move_y = Input.GetAxis("Vertical") * speed;
            if (Input.GetMouseButton(2))
            {
                Cursor.lockState = CursorLockMode.None;
                //Cursor.visible = false;
                move_x = (Vector3.left * Input.GetAxis("Mouse X") * speed).x;
                move_y = (Vector3.up * Input.GetAxis("Mouse Y") * -speed).y;
                //move_x = (Camera.main.ScreenToWorldPoint(Input.mousePosition).x - this.lastTimePos.x) * speed;
                //move_y = (Camera.main.ScreenToWorldPoint(Input.mousePosition).y - this.lastTimePos.y) * speed;
            }
            else if (Input.GetMouseButtonUp(2))
            {
                Win32.SetCursorPos(this.lastMouseX, this.lastMouseY);
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
                //Cursor.visible = true;
                if (Input.mousePosition.x < 5) move_x -= speed;
                else if (Input.mousePosition.x > Screen.width - 5) move_x += speed;

                if (Input.mousePosition.y < 5) move_y -= speed;
                else if (Input.mousePosition.y > Screen.height - 5) move_y += speed;
                Win32.POINT pt = new Win32.POINT();
                Win32.GetCursorPos(out pt);
                this.lastMouseX = pt.X;
                this.lastMouseY = pt.Y;
            }

            //if (_moveToCanBeInterupted && (move_x != 0 || move_y != 0))
            //    StopMoveTo();

            Vector3 newPosition = transform.position + new Vector3(move_x, move_y, 0) * Time.fixedDeltaTime;

            newPosition.x = Mathf.Clamp(newPosition.x, -rangeX, rangeX);
            newPosition.y = Mathf.Clamp(newPosition.y, -rangeY, rangeY);


            transform.position = newPosition;

            if (Input.GetAxis("Mouse ScrollWheel") > 0)
                Camera.main.orthographicSize--;
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
                Camera.main.orthographicSize++;
            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, orthographicSizeMin, orthographicSizeMax);


            //PlayerControl.Instance.RefreshOpUIPos();
        }

    }
}