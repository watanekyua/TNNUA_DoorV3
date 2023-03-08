using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;


public class VideoHelper : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Transform VideoCanvas;
    public GameObject VideoGameObject;
    public VideoPosCon videoPosCon;

    public void SetupVideo(string filePath){
        videoPlayer.url = filePath;
        videoPlayer.Prepare();
        videoPlayer.loopPointReached += delegate {
            
        };
        videoPlayer.prepareCompleted += delegate {
            Debug.Log($"Get video size: {videoPlayer.texture.width}x{videoPlayer.texture.height}");
            VideoCanvas.localScale = new Vector3(videoPlayer.texture.width/5000f, 1, videoPlayer.texture.height/5000f);
            VideoGameObject.SetActive(true);
        };
            
        videoPlayer.Play();
    }

    public void CloseVideo(){
        videoPlayer.Stop();
        VideoGameObject.SetActive(false);
    }

    public bool IsActive(){
        return VideoGameObject.activeSelf;
    }

    public void ResetPos(){
        if(IsActive()){
            videoPosCon.ResetPos();
        }
    }
}
