using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Playables;
using UnityEngine.UI;

public class BasePresent : MonoBehaviour
{
    public PlayableDirector playable;
    public Vector3 fpos;
    public float speed = 3;
    public float delayMove = 5;

    public InputField INP_Size;

    protected Vector3 orinPos;

    void Awake(){
        orinPos = transform.position;

        // Component[] components = playable.GetComponents(typeof(Component));
        // foreach(Component component in components) {
        //     Debug.Log(component.ToString());
        // }
    }

    void Start(){
        string key = gameObject.name+"_scale";
        var scale = SystemConfig.Instance.GetData<Vector3>(key, transform.localScale);
        Debug.Log("last Scale :" + scale);
        transform.localScale = scale;

        ApplicationDelegate.instance.ToDoOnQuit += SaveInfos;

        if(INP_Size != null){
            INP_Size.onValueChanged.AddListener(x => {
                float f = 1f;
                if(float.TryParse(x, out f)){
                    transform.localScale = new Vector3(f, f, f);
                    SaveInfos();
                }
            });
            INP_Size.text = scale.x.ToString("0.00");
        }
    }

    public virtual void StartPresent(){

    }

    public virtual void StopPresent(){

    }

    void SaveInfos() {
        //Debug.Log("savw des");
        string key = gameObject.name+"_scale";
        SystemConfig.Instance.SaveData(key, transform.localScale);
    }
}
