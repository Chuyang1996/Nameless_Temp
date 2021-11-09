#define DEBUG

using System.Collections;
using UnityEngine;

namespace Nameless.Controller
{
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
        public float x = 5;
        public float y = 5;
        [SerializeField] private float orthographicSizeMin = 5;
        [SerializeField] private float orthographicSizeMax = 30;

        [SerializeField] private float speed = 3.0f;

        private Camera _camera;

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
        public void StartTransition(TransitionTarget[] transitionTargets, bool isAuto)
        {
            this._isTranstionTo = true;
            StartCoroutine(TransitionToCoroutine(transitionTargets, 0, isAuto));
        }

        IEnumerator TransitionToCoroutine(TransitionTarget[] transitionTargets,int index,bool isAuto)
        {
            if (index < transitionTargets.Length)
            {
                bool GetPosTarget = false;
                bool GetZoomTarget = false;
                while (transform.position != transitionTargets[index].targetPos || _camera.orthographicSize != transitionTargets[index].targetZoom)
                {
                    if (transform.position == transitionTargets[index].targetPos)
                        GetPosTarget = true;
                    else
                        transform.position = Vector3.MoveTowards(transform.position, transitionTargets[index].targetPos, Time.deltaTime * transitionTargets[index].speedPos);
                    if (_camera.orthographicSize == transitionTargets[index].targetZoom)
                        GetZoomTarget = true;
                    else
                        _camera.orthographicSize += Mathf.Clamp(-transitionTargets[index].speedZoom * Time.deltaTime, transitionTargets[index].targetZoom - _camera.orthographicSize, transitionTargets[index].speedZoom * Time.deltaTime);
                    if (GetZoomTarget && GetPosTarget)
                        break;
                    yield return null;
                }

                while (!isAuto)
                {
                    if (this._clickToNext)
                    {
                        this._clickToNext = false;
                        break;
                    }
                    yield return null;
                }
                if(isAuto)
                    yield return new WaitForSecondsRealtime(transitionTargets[index].waitTime);

                index++;
                StartCoroutine(TransitionToCoroutine(transitionTargets, index, isAuto));
            }
            else
            {
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


        private void Start()
        {

        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (Locked)
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
            if (Input.mousePosition.x < 5) move_x -= speed;
            else if (Input.mousePosition.x > Screen.width - 5) move_x += speed;

            if (Input.mousePosition.y < 5) move_y -= speed;
            else if (Input.mousePosition.y > Screen.height - 5) move_y += speed;

            //if (_moveToCanBeInterupted && (move_x != 0 || move_y != 0))
            //    StopMoveTo();

            Vector3 newPosition = transform.position + new Vector3(move_x, move_y, 0) * Time.fixedDeltaTime;

            newPosition.x = Mathf.Clamp(newPosition.x, -x, x);
            newPosition.y = Mathf.Clamp(newPosition.y, -y, y);


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