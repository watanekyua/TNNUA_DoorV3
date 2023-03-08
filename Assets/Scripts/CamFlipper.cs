using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CamFlipper : MonoBehaviour
{
    public Toggle TG_Flip;
    
    public List<Transform> ObjectToFlips;

    void Start()
    {
        TG_Flip.onValueChanged.AddListener(FlipToggle);

        var res = SystemConfig.Instance.GetData<bool>("IsFlip", false);
        TG_Flip.isOn = res;
    }

    void FlipToggle(bool res){
        foreach (var item in ObjectToFlips)
        {
            float flipValue = res ? 1 : -1;
            item.localScale = new Vector3(flipValue, 1, 1);
        }

        SystemConfig.Instance.SaveData("IsFlip", res);
    }
}
