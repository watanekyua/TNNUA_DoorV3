using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JudgeDepth : MonoBehaviour
{
    public InputField INP_JudgeLine;
    public float judgeLine = 12;

    public System.Action<float> OnNearPsersonCome;
    public GameObject ForgroundImageBehide;
    public GameObject ForgroundImageFront;

    void Start()
    {
        INP_JudgeLine.onValueChanged.AddListener(x => {
            float f = 12;
            if(float.TryParse(x, out f)){
                judgeLine = f;
                SystemConfig.Instance.SaveData("DepthJudge", x);
            }
        });
        INP_JudgeLine.text = SystemConfig.Instance.GetData<string>("DepthJudge", "12");


        OnNearPsersonCome += WhenNearPersionCome;
        WhenNearPersionCome(0);
    }

    void WhenNearPersionCome(float near){
        Debug.Log($"come : {near}");
        if(near < judgeLine){
            ForgroundImageFront.SetActive(true);
            ForgroundImageBehide.SetActive(false);
        } else {
            ForgroundImageFront.SetActive(false);
            ForgroundImageBehide.SetActive(true);
        }
    }

    public float emuNearValue;
    [EasyButtons.Button]
    void DoEmuNearPersion(){
        OnNearPsersonCome.Invoke(emuNearValue);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
