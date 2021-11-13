using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCameraController : MonoBehaviour
{
    public float moveSpeed = 1;
    public float triggerDistance = 1f;
    public Transform leftMost;
    public Transform rightMost;
    public Vector3 cameraScreenPos;

    public float leftX;
    public float rightX;
    // Start is called before the first frame update
    void Start()
    {
        leftX = Camera.main.WorldToScreenPoint(leftMost.position).x;
        rightX = Camera.main.WorldToScreenPoint(rightMost.position).x;
    }

    // Update is called once per frame
    void Update()
    {
        cameraScreenPos = Camera.main.WorldToScreenPoint(this.transform.position);
        Debug.Log(cameraScreenPos);
        MoveCamera();
    }

    public void MoveCamera()
    {
        if(Input.GetKey(KeyCode.A))
        {
            if(cameraScreenPos.x > 0)
            this.transform.Translate(Vector3.left*moveSpeed*Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            if(cameraScreenPos.x < Screen.width)
            this.transform.Translate(Vector3.right*moveSpeed*Time.deltaTime);
        }
        
    }
}
