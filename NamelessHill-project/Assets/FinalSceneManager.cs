using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalSceneManager : MonoBehaviour
{

    public bool isStartSceneDone = false;
    public bool allContentShown = false;
    public bool isShownDone = false;
    public SpriteRenderer blackScene;
    public float tempAlpha;
    public float tempContentAlpha;
    public float floatSpeed;
    public float floatSpeedUI;
    public int sceneCount;
    public int itemCount = 0;

    public float readTimer = 0;
    public List<SpriteRenderer> contents;
    public GameObject regularBook;

    public GameObject finalBook;

    public Animator final;

    public bool isFinalShown = false;

    // Start is called before the first frame update
    void Start()
    {
        tempAlpha = blackScene.color.a;
        foreach (SpriteRenderer sprite in contents)
        {
            sprite.color = new Color(255, 255, 255, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStartSceneDone)
        {
            FadeIn();
        }
        if (isStartSceneDone && !allContentShown)
        {
            ShowContent();
        }
        if (allContentShown)
        {

            if (readTimer >= 3)
            {
                HideContent();
            }
            else
            {
                readTimer += Time.deltaTime;
            }
        }
        if (isShownDone)
        {
            ShowFinal();
            //播放动画
            //播放制作人员名单
            //播放完成后loadScene
        }
    }

    public void FadeIn()
    {
        if (tempAlpha > 0)
        {
            tempAlpha -= Time.deltaTime * floatSpeed;
            //Debug.Log(tempAlpha);
            blackScene.color = new Color(0, 0, 0, tempAlpha);
        }
        else
        {
            isStartSceneDone = true;
        }

    }

    public void ShowContent()
    {
        if (!allContentShown)
        {
            tempContentAlpha += Time.deltaTime * floatSpeedUI;
            //Debug.Log(tempContentAlpha);
            contents[itemCount].color = new Color(1, 1, 1, tempContentAlpha);
            //Debug.Log("AAAAAAAA"+contents[itemCount].color.a);
            if (tempContentAlpha >= 1)
            {
                tempContentAlpha = 0;
                itemCount += 1;
                if (itemCount >= contents.Count)
                {
                    allContentShown = true;
                    tempContentAlpha = 1;
                }
            }
        }
    }

    public void HideContent()
    {
        if (!isShownDone)
        {
            if (tempAlpha <= 1)
            {
                tempAlpha += Time.deltaTime * floatSpeed;
                //Debug.Log(tempAlpha);
                blackScene.color = new Color(0, 0, 0, tempAlpha);
            }
            else
            {
                isShownDone = true;
                regularBook.SetActive(false);
                finalBook.SetActive(true);
            }
        }

        // if(!isShownDone)
        // {
        //     if (tempContentAlpha >= 0)
        //     {
        //         tempContentAlpha -= Time.deltaTime * floatSpeedUI;
        //     }
        //     foreach (SpriteRenderer sprite in contents)
        //     {
        //         sprite.color = new Color(1, 1, 1, tempContentAlpha);
        //     }
        //     if (tempContentAlpha < 0)
        //     {
        //         isShownDone = true;
        //     }
        // }
    }

    public void ShowFinal()
    {
        if (!isFinalShown)
        {
            if (tempAlpha > 0)
            {
                tempAlpha -= Time.deltaTime * floatSpeed;
                //Debug.Log(tempAlpha);
                blackScene.color = new Color(0, 0, 0, tempAlpha);
            }
            else
            {
                isFinalShown = true;
                Debug.Log("Shown!");
            }
        }
    }




}
