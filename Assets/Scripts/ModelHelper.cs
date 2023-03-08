using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TriLibCore.Extensions;
using UnityEngine.UI;

public class ModelHelper : MonoBehaviour
{
    public Dropdown PlaybackAnimation;
    public GameObject ModelGameObject;
    public ModelPosCon modelPosCon;
    Animation _animation;

    void Start()
    {
        PlaybackAnimation.onValueChanged.AddListener(OnAnimationSelected);
    }

    public void SetupModel(string filePath){
        var assetLoaderOptions = TriLibCore.AssetLoader.CreateDefaultLoaderOptions();
        TriLibCore.AssetLoader.LoadModelFromFile(filePath, OnLoad, OnMaterialsLoad, OnProgress, OnError, null, assetLoaderOptions);
    }

    public void CloseModel(){
        ModelGameObject.SetActive(false);
    }

    void OnAnimationSelected(int i){
        if (_animation == null)
        {
            return;
        }
        _animation.Play(PlaybackAnimation.options[i].text, PlayMode.StopAll);

        SystemConfig.Instance.SaveData("LastAnim", PlaybackAnimation.options[i].text);
    }

    /// <summary>
    /// Called when any error occurs.
    /// </summary>
    /// <param name="obj">The contextualized error, containing the original exception and the context passed to the method where the error was thrown.</param>
    private void OnError(TriLibCore.IContextualizedError obj)
    {
        Debug.LogError($"An error occurred while loading your Model: {obj.GetInnerException()}");
    }

    /// <summary>
    /// Called when the Model loading progress changes.
    /// </summary>
    /// <param name="assetLoaderContext">The context used to load the Model.</param>
    /// <param name="progress">The loading progress.</param>
    private void OnProgress(TriLibCore.AssetLoaderContext assetLoaderContext, float progress)
    {
        Debug.Log($"Loading Model. Progress: {progress:P}");
    }

    /// <summary>
    /// Called when the Model (including Textures and Materials) has been fully loaded.
    /// </summary>
    /// <remarks>The loaded GameObject is available on the assetLoaderContext.RootGameObject field.</remarks>
    /// <param name="assetLoaderContext">The context used to load the Model.</param>
    private void OnMaterialsLoad(TriLibCore.AssetLoaderContext assetLoaderContext)
    {
        Debug.Log("Materials loaded. Model fully loaded.");

        foreach (Transform item in ModelGameObject.transform)
        {
            Destroy(item.gameObject);
        }
        ModelGameObject.SetActive(true);

        assetLoaderContext.RootGameObject.transform.parent = ModelGameObject.transform;
        assetLoaderContext.RootGameObject.transform.localEulerAngles = new Vector3(0, 180, 0);
        assetLoaderContext.RootGameObject.transform.localPosition = Vector3.zero;

        _animation = assetLoaderContext.RootGameObject.GetComponent<Animation>();
        PlaybackAnimation.ClearOptions();
        if (_animation != null)
        {
            var _animations = _animation.GetAllAnimationClips();
            if (_animations.Count > 0)
            {
                PlaybackAnimation.interactable = true;
                for (var i = 0; i < _animations.Count; i++)
                {
                    var animationClip = _animations[i];
                    PlaybackAnimation.options.Add(new Dropdown.OptionData(animationClip.name));
                }
                PlaybackAnimation.captionText.text = _animations[0].name;
            }
            else
            {
                _animation = null;
            }
        }

        string lastAnim = SystemConfig.Instance.GetData<string>("LastAnim");
        if(!string.IsNullOrEmpty(lastAnim) && _animation != null){
            _animation.Play(lastAnim, PlayMode.StopAll);
        }
    }

    /// <summary>
    /// Called when the Model Meshes and hierarchy are loaded.
    /// </summary>
    /// <remarks>The loaded GameObject is available on the assetLoaderContext.RootGameObject field.</remarks>
    /// <param name="assetLoaderContext">The context used to load the Model.</param>
    private void OnLoad(TriLibCore.AssetLoaderContext assetLoaderContext)
    {
        Debug.Log("Model loaded. Loading materials.");
    }

    public bool IsActive(){
        return ModelGameObject.activeSelf;
    }

    public void ResetPos(){
        if(IsActive()){
            modelPosCon.ResetPos();
        }
    }
}
