using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppQuit : MonoBehaviour
{
    public Button BTN_Quit;

    void Start(){
        BTN_Quit.onClick.AddListener(() => {
            Application.Quit();
        });
    }
}
