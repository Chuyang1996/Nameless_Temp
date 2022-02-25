using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum CurrentScene
{
    VictoryScene,
    RetreatScene,
    SurrenderScene
}
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

    public CurrentScene cs;
    public int itemCount = 0;

    public float readTimer = 0;
    public List<SpriteRenderer> contents;
    public GameObject regularBook;

    public GameObject finalBook;

    public Animator final;

    public bool isFinalShown = false;

    public SpriteRenderer credits;
    public float creditAlpha;
    public float creditShowSpeed;

    public SpriteRenderer note;

    public float hintTimer;
    public bool isEverythingDone = false;
    public bool isReadyBack = false;

    // Start is called before the first frame update
    void Start()
    {
        tempAlpha = blackScene.color.a;
        foreach (SpriteRenderer sprite in contents)
        {
            sprite.color = new Color(255, 255, 255, 0);
        }
        switch (cs)
        {
            case CurrentScene.RetreatScene:
                break;
            case CurrentScene.VictoryScene:
                //Victory
                tempAlpha = blackScene.color.a;
                note.color = new Color(1, 1, 1, 0);
                credits.gameObject.SetActive(false);
                foreach (SpriteRenderer sprite in contents)
                {
                    sprite.color = new Color(255, 255, 255, 0);
                }
                break;
            case CurrentScene.SurrenderScene:
                //Surrender
                break;
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
        switch (cs)
        {
            case CurrentScene.RetreatScene:
                //Retreat
                if (allContentShown)
                {
                    readTimer += Time.deltaTime;
                }
                if (readTimer >= 3)
                {
                    tempAlpha += Time.deltaTime * floatSpeed;
                    //Debug.Log(tempAlpha);
                    blackScene.color = new Color(0, 0, 0, tempAlpha);
                    if (tempAlpha >= 1 && readTimer >= 6)
                    {

                        Debug.Log("ASDASDASDASD");
                        SceneManager.LoadScene(0);

                    }


                }
                break;
            case CurrentScene.VictoryScene:
                //Victory
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
                }

                if (isEverythingDone)
                {
                    ShowNote();
                }
                if (isReadyBack)
                {
                    Debug.Log("assss");
                    if (Input.GetMouseButtonDown(0))
                    {
                        SceneManager.LoadScene(0);
                    }
                }
                break;
            case CurrentScene.SurrenderScene:
                if (readTimer >= 3)
                {
                    tempAlpha += Time.deltaTime * floatSpeed;
                    //Debug.Log(tempAlpha);
                    blackScene.color = new Color(0, 0, 0, tempAlpha);

                }
                else if (readTimer < 3)
                {
                    readTimer += Time.deltaTime;
                }
                else if (readTimer >= 5)
                {
                    SceneManager.LoadScene(0);
                }
                //Surrender
                break;
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
                foreach (SpriteRenderer sprite in contents)
                {
                    sprite.gameObject.SetActive(false);
                }
                finalBook.SetActive(true);
                credits.gameObject.SetActive(true);
                isEverythingDone = true;
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

    public void ShowNote()
    {
        if (creditAlpha < 1)
        {
            creditAlpha += Time.deltaTime * creditShowSpeed;
            note.color = new Color(1, 1, 1, creditAlpha);
        }
        else
        {
            isReadyBack = true;
        }
    }


}
