using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUI : MonoBehaviour
{
    public Transform targetObject;
    public RectTransform targetCanvas;

    RectTransform rectTransform;
    Vector2 deltaPos;

    void Awake(){
        rectTransform = transform as RectTransform;
    }

    void Start()
    {
        deltaPos = rectTransform.anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 screenPoint = WorldPointToUIPoint(Camera.main, targetCanvas, targetObject.position);
        rectTransform.anchoredPosition = screenPoint + deltaPos;

        //Debug.Log("rtc "+RectTransformUtility.WorldToScreenPoint(Camera.main, targetObject.position));
        //Debug.Log("m "+Input.mousePosition);

        //Vector2 vp;
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(targetCanvas, Input.mousePosition, Camera.main, out vp);

        //Debug.Log("vp " + vp);
    }
    
    //這是畫面座標
    Vector2 WorldToScreenPoint(Camera cam, Vector3 worldPoint)
    {
        return RectTransformUtility.WorldToScreenPoint(cam, worldPoint);
    }

    //畫面座標轉UI座標
    bool ScreenPointToLocalPointInRectangle(RectTransform rect, Vector2 screenPoint, Camera cam, out Vector2 localPoint)
    {
        return RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPoint, cam, out localPoint);
    }

    //世界座標轉UI座標, Canvas的 RenderMode 一定要是Camera
    Vector2 WorldPointToUIPoint(Camera cam, RectTransform rect, Vector3 worldPoint)
    {
        Vector2 sp = WorldToScreenPoint(cam, worldPoint);
        Vector2 output;
        ScreenPointToLocalPointInRectangle(rect, sp, cam, out output);

        return output;
    }
}
