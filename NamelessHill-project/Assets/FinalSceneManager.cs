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
    public int itemCount=0;

    public float readTimer =0;
    public List<SpriteRenderer> contents;
    public GameObject regularBook;

    public GameObject finalBook;

    // Start is called before the first frame update
    void Start()
    {
        tempAlpha = blackScene.color.a;
        foreach(SpriteRenderer sprite in contents)
        {
            sprite.color = new Color(255,255,255,0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        FadeIn();
        if(isStartSceneDone&&!allContentShown)
        {
            ShowContent();
        }
        if(allContentShown)
        {
            readTimer+=Time.deltaTime;
            if(readTimer>=5)
            {
                HideContent();
            }
        }
        if(isShownDone)
        {
            ShowFinal();
            //播放动画
            //播放制作人员名单
            //播放完成后loadScene
        }
    }

    public void FadeIn()
    {
        if(tempAlpha>0)
        {
            tempAlpha-=Time.deltaTime*floatSpeed;
            //Debug.Log(tempAlpha);
            blackScene.color = new Color(0,0,0,tempAlpha);
        }
        else
        {
            isStartSceneDone = true;
        }
        
    }

    public void ShowContent()
    {
        tempContentAlpha+=Time.deltaTime*floatSpeedUI;
        Debug.Log(tempContentAlpha);
        contents[itemCount].color = new Color(1,1,1,tempContentAlpha);
        Debug.Log("AAAAAAAA"+contents[itemCount].color.a);
        if(tempContentAlpha>=1)
        {
            tempContentAlpha=0;
            itemCount+=1;
            if(itemCount>=contents.Count)
            {
                allContentShown=true;
                tempContentAlpha = 1;
            }
        }
    }

    public void HideContent()
    {
        tempContentAlpha-=Time.deltaTime*floatSpeedUI;
        foreach(SpriteRenderer sprite in contents)
        {
            sprite.color = new Color(1,1,1,tempContentAlpha);
        }
        if(tempContentAlpha<0)
        {
            isShownDone=true;
        }
    }

    public void ShowFinal()
    {
        finalBook.SetActive(true);
        regularBook.SetActive(false);
    }




}
