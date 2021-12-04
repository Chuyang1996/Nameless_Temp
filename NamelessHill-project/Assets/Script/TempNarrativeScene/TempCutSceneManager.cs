using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempCutSceneManager : MonoBehaviour
{
    public bool showMap = false;
    public bool isMapShown = false;
    public bool isArrowShown = false;
    public bool isSceneFinished = false;
    public SpriteRenderer map;
    public List<SpriteRenderer> arrows;
    private float tempAlpha = 0;
    public float colorChangeSpeed = 2;
    public Transform targetZoomInPoint;
    public float cameraZoomSpeed = 5;
    public float cameraMoveSpeed = 3;
    public SpriteRenderer BG;

    private float waitTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        map.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
        BG.color = new Color(1, 1, 1, 0);
        BG.gameObject.SetActive(false);
        map.gameObject.SetActive(false);
        foreach (var arrow in arrows)
        {
            arrow.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
            arrow.gameObject.SetActive(false);
        }
        map.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (showMap)
        {
            map.gameObject.SetActive(true);
            ShowMap();
        }

        if (isSceneFinished)
        {
            FadeIntoScene();
        }
    }

    public void ShowMap()
    {
        if (!isMapShown)
        {
            if (map.GetComponent<SpriteRenderer>().color.a < 1)
            {
                tempAlpha += Time.deltaTime * colorChangeSpeed;
                map.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, tempAlpha);
            }
            else
            {
                isMapShown = true;
                tempAlpha = 0;
            }
        }
        else
        {
            if (!isArrowShown)
            {
                foreach (var arrow in arrows)
                {
                    arrow.gameObject.SetActive(true);
                }
                tempAlpha += Time.deltaTime * colorChangeSpeed;
                foreach (var arrow in arrows)
                {
                    arrow.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, tempAlpha);
                }

                if (tempAlpha >= 1)
                {
                    isArrowShown = true;
                    isSceneFinished = true;
                    tempAlpha = 0;
                }
            }
        }
    }

    public void FadeIntoScene()
    {
        Vector3 tempDir = (targetZoomInPoint.position - Camera.main.transform.position);
        if(tempDir.magnitude>=0.5)
        {
            Camera.main.transform.Translate(tempDir.normalized * Time.deltaTime*cameraMoveSpeed);
        }
        if(Camera.main.orthographicSize>=1.3)
        {
            Camera.main.orthographicSize -= Time.deltaTime * cameraZoomSpeed;
            tempAlpha = 0;
        }
        else
        {
            waitTimer+=Time.deltaTime;
            if(waitTimer>=0.7)
            {
                BG.gameObject.SetActive(true);
                tempAlpha += Time.deltaTime * colorChangeSpeed;
                if (tempAlpha <= 1)
                {
                    BG.color = new Color(0, 0, 0, tempAlpha);
                }
                else
                {
                    //NextLevel
                }
            }

        }
    }
}
