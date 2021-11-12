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
    


    // Start is called before the first frame update
    void Start()
    {
        text = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetMouseButtonDown(0))
        {
            if(!isShowingText)
            {
                StartCoroutine(ShowText(showChar.Length));
                isShowingText = true;
            }
            else
            {
                ShowAllText();
            }
        }

    }

    public IEnumerator ShowText(int strLength)
    {
        int i =0;
        while(i<content.Length)
        {
            yield return new WaitForSeconds(0.05f);
            showChar += content[i].ToString();
            text.text = showChar;
            i+=1;
        }
    }

    public void ShowAllText()
    {
        if(showChar == content)
        {
            isShowingText = false;
            //展示完成
        }
        else
        {
            StopAllCoroutines();
        }

        showChar = content;
        text.text = content;
    }
}
