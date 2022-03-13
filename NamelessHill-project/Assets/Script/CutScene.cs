using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutScene : MonoBehaviour
{
    public Button button;
    // Start is called before the first frame update
    void Start()
    {
        this.button.onClick.AddListener(() =>
        {
            Application.LoadLevel(0);
        });
    }

}
