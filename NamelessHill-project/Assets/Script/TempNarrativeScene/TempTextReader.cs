using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempTextReader : MonoBehaviour
{
    public Text text;
    public string content;
    private string showChar = "";

    public bool isShowingText = false;

    public bool isAllowToShow = false;

    public TempTextReader nextLine;

    public Image nextIcon;

    public bool isDone = false;

    public TempCutSceneManager tcsm;



    // Start is called before the first frame update
    void Start()
    {
        text = this.GetComponent<Text>();
        content = text.text;
        text.text = " ";
        nextIcon.gameObject.SetActive(false);
        tcsm = GameObject.Find("CutSceneManager").GetComponent<TempCutSceneManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAllowToShow && !isDone)
        {
            if (showChar == content)
            {
                nextIcon.gameObject.SetActive(true);
                //展示完成
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (!isShowingText)
                {
                    Debug.Log("StartProcess!!");
                    StartCoroutine(ShowText(showChar.Length));
                    isShowingText = true;
                }
                else
                {
                    ShowAllText();
                    Debug.Log("isDOne!!");
                    if (nextLine != null)
                    {
                        nextLine.isAllowToShow = true;
                    }
                    else
                    {
                        //ShowPic
                        tcsm.showMap = true;
                    }
                    nextIcon.gameObject.SetActive(false);
                }

                if (isDone && !isShowingText)
                {

                }
            }
        }


    }

    public IEnumerator ShowText(int strLength)
    {
        int i = 0;
        while (i < content.Length)
        {
            yield return new WaitForSeconds(0.05f);
            showChar += content[i].ToString();
            text.text = showChar;
            i += 1;
        }
    }

    public void ShowAllText()
    {
        if (showChar == content)
        {
            isShowingText = false;
            isDone = true;
            nextIcon.gameObject.SetActive(true);
            //展示完成
        }
        else
        {
            StopAllCoroutines();
        }

        showChar = content;
        text.text = content;
        nextIcon.gameObject.SetActive(true);
    }
}
