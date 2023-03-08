using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ModelPosCon : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public TextMeshProUGUI TXT_Tips;
    bool isLeftMouseDown = false;
    bool isRightMouseDown = false;
    Vector2 lastPointDown;
    Vector3 lastObjectPosition;

    public float ampZ = 0.05f;
    
    void Start()
    {
        Vector3 p = SystemConfig.Instance.GetData<Vector3>("ModelPos", new Vector3(0, 0, 0));
        transform.position = p;

        Vector3 v = SystemConfig.Instance.GetData<Vector3>("ModelScale", new Vector3(1, 1, 1));
        transform.localScale = v;

        TXT_Tips.gameObject.SetActive(false);
    }

    public void ResetPos(){
        transform.position = Vector3.zero;
        transform.localScale = Vector3.one;
        SystemConfig.Instance.SaveData("ModelScale", transform.localScale);
        SystemConfig.Instance.SaveData("ModelPos", transform.position);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        lastPointDown = Input.mousePosition;
        lastObjectPosition = transform.position;
        if(eventData.button == PointerEventData.InputButton.Left){
            isLeftMouseDown = true;
        }
        if(eventData.button == PointerEventData.InputButton.Right){
            isRightMouseDown = true;
        }

        TXT_Tips.gameObject.SetActive(true);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        isLeftMouseDown = false;
        isRightMouseDown = false;

        TXT_Tips.gameObject.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.LeftAlt)){
            return;
        }
        
        if(Input.mouseScrollDelta.y != 0 && isLeftMouseDown){
            var v = transform.localScale;
            float scale = (1 + Input.mouseScrollDelta.y * 0.1f);
            transform.localScale = new Vector3(v.x * scale, v.y * scale, v.z * scale);

            SystemConfig.Instance.SaveData("ModelScale", transform.localScale);
        }

        if(isLeftMouseDown){
            
            float planeZpos = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);

            Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, planeZpos));

            transform.position = new Vector3(point.x, point.y, transform.position.z);

            SystemConfig.Instance.SaveData("ModelPos", transform.position);

            TXT_Tips.text = $"Pos {transform.position}\nScale:{transform.localScale}";

            return;
        }

        if(isRightMouseDown){
            float disY = Input.mousePosition.y - lastPointDown.y;

            transform.position = new Vector3(lastObjectPosition.x, lastObjectPosition.y, lastObjectPosition.z + disY * ampZ);

            SystemConfig.Instance.SaveData("ModelPos", transform.position);

            TXT_Tips.text = $"Pos {transform.position}\nScale:{transform.localScale}";
        }
    }
}
