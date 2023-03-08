using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

// 幹他媽 Canvas 上的 全版 Panel 的 Raycast Target 會吃掉Camera的Raycaster
// 要把全版Raycast Target 關掉

// OnMouseDown 當你場上不只一個攝影機時，所有攝影機都會觸發
// IPointerDown 可以限制只有哪個攝影機可以按, 需要的有 1.攝影機掛Raycaster / 2.要發生事件的物體掛上 Interface 與 Collider / 3.實作上述Interface / 4.記得Raycaster 上的Mask有沒有設定

public class VideoPosCon : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public TextMeshProUGUI TXT_Tips;
    bool isLeftMouseDown = false;
    bool isRightMouseDown = false;
    Vector2 lastPointDown;
    Vector3 lastObjectPosition;

    public float ampZ = 0.05f;

    void Start()
    {
        Vector3 p = SystemConfig.Instance.GetData<Vector3>("VideoCanvasPos", new Vector3(0, 0, 0));
        transform.position = p;

        Vector3 v = SystemConfig.Instance.GetData<Vector3>("VideoCanvasScale", new Vector3(1, 1, 1));
        transform.localScale = v;

        TXT_Tips.gameObject.SetActive(false);

        //Debug.Log($"Get data:{v}");
    }

    public void ResetPos(){
        transform.position = Vector3.zero;
        transform.localScale = Vector3.one;
        SystemConfig.Instance.SaveData("VideoCanvasScale", transform.localScale);
        SystemConfig.Instance.SaveData("VideoCanvasPos", transform.position);
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
        //Debug.Log(this.gameObject.name + " Was Point.");
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
            //Debug.Log(Input.mouseScrollDelta.y);

            var v = transform.localScale;
            float scale = (1 + Input.mouseScrollDelta.y * 0.1f);
            transform.localScale = new Vector3(v.x * scale, v.y * scale, v.z * scale);

            //Debug.Log($"Save data:{transform.localScale}");
            SystemConfig.Instance.SaveData("VideoCanvasScale", transform.localScale);
        }

        if(isLeftMouseDown){
            //Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //transform.localPosition = new Vector3(Input.mousePosition.y - isMouseDown.y);

            // Get the mouse position from Event.
            // Note that the y position from Event is inverted.
            //mousePos.x = currentEvent.mousePosition.x;
            //mousePos.y = cam.pixelHeight - currentEvent.mousePosition.y;
            float planeZpos = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
            //float planeZpos = Vector3.Distance(Camera.main.transform.position , transform.position);
            Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, planeZpos));

            transform.position = new Vector3(point.x, point.y, transform.position.z);

            SystemConfig.Instance.SaveData("VideoCanvasPos", transform.position);

            TXT_Tips.text = $"Pos {transform.position}\nScale:{transform.localScale}";
            
            //Debug.Log(point);

            return;
        }

        if(isRightMouseDown){
            float disY = Input.mousePosition.y - lastPointDown.y;

            transform.position = new Vector3(lastObjectPosition.x, lastObjectPosition.y, lastObjectPosition.z + disY * ampZ);

            SystemConfig.Instance.SaveData("VideoCanvasPos", transform.position);

            TXT_Tips.text = $"Pos {transform.position}\nScale:{transform.localScale}";
        }
    }
}
