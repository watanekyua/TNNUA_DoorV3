using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ImageJudger : MonoBehaviour
{
    RectTransform rectTransform;
    
    public float moveSpeed = 100;
    public float scaleSpeed = 1;

    void Awake(){
        rectTransform = transform as RectTransform;
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);

        rectTransform.anchoredPosition = SystemConfig.Instance.GetData<Vector2>("WebcamPos", Vector2.zero);
        rectTransform.localScale = SystemConfig.Instance.GetData<Vector3>("WebcamScale", Vector3.one);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.UpArrow)){
            var p = rectTransform.anchoredPosition;
            rectTransform.anchoredPosition = new Vector2(p.x, p.y + moveSpeed * Time.deltaTime);
            SaveInfos();
        }
        if(Input.GetKey(KeyCode.DownArrow)){
            var p = rectTransform.anchoredPosition;
            rectTransform.anchoredPosition = new Vector2(p.x, p.y - moveSpeed * Time.deltaTime);
            SaveInfos();
        }
        if(Input.GetKey(KeyCode.LeftArrow)){
            var p = rectTransform.anchoredPosition;
            rectTransform.anchoredPosition = new Vector2(p.x - moveSpeed * Time.deltaTime, p.y);
            SaveInfos();
        }
        if(Input.GetKey(KeyCode.RightArrow)){
            var p = rectTransform.anchoredPosition;
            rectTransform.anchoredPosition = new Vector2(p.x + moveSpeed * Time.deltaTime, p.y);
            SaveInfos();
        }
        if(Input.GetKey(KeyCode.Home)){
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.localScale = Vector3.one;
            SaveInfos();
        }
        if(Input.GetKey(KeyCode.KeypadPlus)){
            var p = rectTransform.localScale;
            float scale = 1 + (0.1f * scaleSpeed);
            rectTransform.localScale = new Vector3(p.x * scale, p.y * scale, p.z * scale);
            SaveInfos();
        }
        if(Input.GetKey(KeyCode.KeypadMinus)){
            var p = rectTransform.localScale;
            float scale = 1 - (0.1f * scaleSpeed);
            rectTransform.localScale = new Vector3(p.x * scale, p.y * scale, p.z * scale);
            SaveInfos();
        }
    }

    void SaveInfos(){
        SystemConfig.Instance.SaveData("WebcamPos", rectTransform.anchoredPosition);
        SystemConfig.Instance.SaveData("WebcamScale", rectTransform.localScale);
    }
}
