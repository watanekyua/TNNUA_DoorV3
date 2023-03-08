using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using com.rfilkov.kinect;

public class BackgroundCatcher : MonoBehaviour
{
    [Tooltip("Depth sensor index - 0 is the 1st one, 1 - the 2nd one, etc.")]
    public int sensorIndex = 0;

    [Tooltip("RawImage used to display the color camera feed.")]
    public UnityEngine.UI.RawImage backgroundImage;

    [Tooltip("Camera used to display the background image. Set it, if you'd like to allow background image to resize, to match the color image's aspect ratio.")]
    public Camera backgroundCamera;

    public NNCam.Compositor nncamer;


    // last camera rect width & height
    private float lastCamRectW = 0;
    private float lastCamRectH = 0;

    // references
    //private KinectManager kinectManager = null;
    //private KinectInterop.SensorData sensorData = null;
    private Vector2 initialAnchorPos = Vector2.zero;

    Texture2D SavedTexture;

    public float catchTime = 5;
    float timeElapse = 0;
    int noHumanCount = 0;

    bool firstCatch = true;

    void Start()
    {
        if (backgroundImage == null)
        {
            backgroundImage = GetComponent<UnityEngine.UI.RawImage>();
        }

        StartCoroutine(CoFirstCatch());
        StartCoroutine(CheckLoop());

        // kinectManager = KinectManager.Instance;
        // sensorData = kinectManager != null ? kinectManager.GetSensorData(sensorIndex) : null;
    }

    IEnumerator CoFirstCatch(){
        yield return new WaitForSeconds(3);
        if(firstCatch){
            firstCatch = false;
            CatchImage();
        }
    }

    IEnumerator CheckLoop(){
        while(true){
            if (nncamer.IsNoPersion())
            {
                timeElapse += 1;

                if(timeElapse > catchTime){
                    timeElapse = 0;
                    CatchImage();
                    Debug.Log("Catch Background Image!");
                }

            } else {
                timeElapse = 0;
            }
            yield return new WaitForSeconds(1);
        }
    }


    void CatchImage()
    {
        nncamer.CatchBackTex();

        // if (kinectManager && kinectManager.IsInitialized())
        // {
        //     float cameraWidth = backgroundCamera ? backgroundCamera.pixelRect.width : 0f;
        //     float cameraHeight = backgroundCamera ? backgroundCamera.pixelRect.height : 0f;

        //     Texture imageTex = kinectManager.GetColorImageTex(sensorIndex);
        //     if (backgroundImage && imageTex != null && (backgroundImage.texture == null || 
        //         backgroundImage.texture.width != imageTex.width || backgroundImage.texture.height != imageTex.height ||
        //         lastCamRectW != cameraWidth || lastCamRectH != cameraHeight))
        //     {
        //         lastCamRectW = cameraWidth;
        //         lastCamRectH = cameraHeight;

        //         backgroundImage.texture = imageTex;
        //         backgroundImage.rectTransform.localScale = sensorData.colorImageScale;  // kinectManager.GetColorImageScale(sensorIndex);
        //         backgroundImage.color = Color.white;
        //     }

        //     if(imageTex != null)
        //     {
        //         if(SavedTexture != null)
        //         {
        //             Destroy(SavedTexture);
        //         }
        //         SavedTexture = new Texture2D(imageTex.width, imageTex.height, TextureFormat.BGRA32, false);
        //         Graphics.CopyTexture(imageTex, SavedTexture);
        //         SavedTexture.Apply();

        //         backgroundImage.texture = SavedTexture;
        //     }
        // }
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Insert)){
            CatchImage();
        }

        // if (kinectManager && kinectManager.IsInitialized())
        // {
        //     if(firstCatch){
        //         firstCatch = false;
        //         CatchImage();
        //     }

        //     if (!kinectManager.IsUserDetected(0) &&
        //         !kinectManager.IsUserDetected(1) &&
        //         !kinectManager.IsUserDetected(2) &&
        //         !kinectManager.IsUserDetected(3) &&
        //         !kinectManager.IsUserDetected(4))
        //     {
        //         timeElapse += Time.deltaTime;

        //         if(timeElapse > catchTime){
        //             timeElapse = 0;
        //             CatchImage();
        //             Debug.Log("Catch Image!");
        //         }

        //     } else {
        //         timeElapse = 0;
        //     }
        // }
    }

    // public RenderTexture GetBackground(){

    //     if(foregroundTexture)
    //     {
    //         foregroundTexture.Release();
    //         foregroundTexture = null;
    //     }

    //     if (sensorData != null && sensorData.sensorInterface != null && KinectInterop.IsDirectX11Available()){
    //         sensorInt = (DepthSensorBase)sensorData.sensorInterface;

    //         // set the texture resolution
    //         if (sensorInt.pointCloudColorTexture == null && sensorInt.pointCloudVertexTexture == null)
    //         {
    //             sensorInt.pointCloudResolution = DepthSensorBase.PointCloudResolution.ColorCameraResolution;
    //         }

    //         textureRes = sensorInt.GetPointCloudTexResolution(sensorData);

    //         foregroundTexture = KinectInterop.CreateRenderTexture(foregroundTexture, textureRes.x, textureRes.y, RenderTextureFormat.ARGB32);

    //         return foregroundTexture;
    //     }

    //     return null;
    // }
}
