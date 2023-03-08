using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LogInfos : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {   
        Debug.Log($"origin width:{Screen.width} height:{Screen.height}");
        await Task.Delay(1000);
        Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, true);
        Debug.Log($"new width:{Screen.width} height:{Screen.height}");
    }
}
