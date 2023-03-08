using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugText : MonoBehaviour
{
    public Text _t;
    void Start()
    {
        _t = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        _t.text = $"{Time.realtimeSinceStartup}";
    }
}
