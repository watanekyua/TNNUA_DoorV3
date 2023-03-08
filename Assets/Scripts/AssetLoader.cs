using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TriLibCore.SFB;

public class AssetLoader : MonoBehaviour
{
    public Button LoadAsset;
    public Button RemoveAsset;
    public Button ResetAssetPos;
    public VideoHelper videoHelper;
    public ModelHelper modelHelper;
    public List<BasePresent> allPresent;
    public BasePresent mPresent;
    public Dropdown DefaultPresent;

    void Start()
    {
        LoadAsset.onClick.AddListener(OpenAsset);
        RemoveAsset.onClick.AddListener(ResetAsset);
        ResetAssetPos.onClick.AddListener(ResetAssetPosition);
        DefaultPresent.onValueChanged.AddListener(SetDpIndex);

        string path = SystemConfig.Instance.GetData<string>("LastPath");
        Debug.Log("last path: " + path);
        if(!string.IsNullOrWhiteSpace(path)){
            LoadAssetAction(path);
        }

        int dp_index = SystemConfig.Instance.GetData<int>("DefaultPresent", 0);
        //Debug.Log($"DV{dp_index}");
        DefaultPresent.value = dp_index;
        
        if(string.IsNullOrEmpty(path)){
            OpenPresent();
        }
    }

    void SetDpIndex(int index){
        //Debug.Log($"Drop Change");
        if(index >= allPresent.Count)
            return;

        if(mPresent != allPresent[index]){
            ClosePresent();
        }
        mPresent = allPresent[index];
        SystemConfig.Instance.SaveData("DefaultPresent", index);
    }

    void LoadAssetAction(string filePath){
        Debug.Log($"Read File:{filePath}");

        System.IO.FileInfo fi = new System.IO.FileInfo(filePath);  
        
        Debug.Log("File Type " + fi.Extension);

        if(fi.Extension == ".webm" || fi.Extension == ".mp4"){
            SystemConfig.Instance.SaveData("LastPath", filePath);
            Debug.Log("save path: " + filePath);

            modelHelper.CloseModel();

            videoHelper.SetupVideo(filePath);
        }

        if(fi.Extension == ".fbx"){
            SystemConfig.Instance.SaveData("LastPath", filePath);

            videoHelper.CloseVideo();

            modelHelper.SetupModel(filePath);
        }
    }

    void OpenAsset(){
        var extensions = new [] {
                new ExtensionFilter("supported video", "webm", "mp4"),
                new ExtensionFilter("3d model", "fbx"),
            };
        var result = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);

        if (result != null)
        {
            var hasFiles = result != null && result.Count > 0 && result[0].HasData;

            if(result.Count > 0){
                string filePath = result[0].Name;
                
                LoadAssetAction(filePath);

                ClosePresent();
            }
        }
    }

    void OpenPresent(){
        mPresent.StartPresent();
    }

    void ClosePresent(){
        //buddhaMove.StopMove();
        mPresent.StopPresent();
    }

    void ResetAsset(){
        OpenPresent();
        modelHelper.CloseModel();
        videoHelper.CloseVideo();

        SystemConfig.Instance.SaveData("LastPath", "");
    }

    void ResetAssetPosition(){
        videoHelper.ResetPos();
        modelHelper.ResetPos();
    }
}
