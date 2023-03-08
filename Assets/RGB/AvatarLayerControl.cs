using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;

public class AvatarLayerControl : MonoBehaviour
{
    public float loopDelay = 0.5f;
    public int BW_Threshold = 75;
    public int depthThreshold = 25;
    public RenderTexture SignalInput;
    public NNCam.Compositor nnCam;
    public JudgeDepth judgeDepth;
    public AvatarLayer Prefab_Layer;
    public List<Transform> CanvasLayer;
    public List<AvatarLayer> OutputLayer;

    public Transform debug_Mats;
    public bool useDebug = false;
    public List<AvatarLayer> debugLayers;

    public RawImage BW_Image;
    Texture2D lastBW_tex;
    int sizePam = 1;

    //public List<RenderTexture> 
    void Start()
    {
        StartCoroutine(LateLoop());
    }

    [EasyButtons.Button]
    void ImmediateProccessImage(){
        ProccessImage();
    }

    IEnumerator LateLoop(){
        while(true){
            yield return new WaitForSeconds(loopDelay);

            ProccessImage();
        }
    }

    void ProccessImage(){
        //轉成 Texture2D
        SignalInput = nnCam.GetOutputTex;
        Texture2D tempTex = new Texture2D(SignalInput.width, SignalInput.height, TextureFormat.RGBA32, false);
        RenderTexture.active = SignalInput;
        tempTex.ReadPixels(new UnityEngine.Rect(0, 0, SignalInput.width, SignalInput.height), 0, 0);
        RenderTexture.active = null;
        tempTex.Apply();

        //轉成 Mat
        Mat srcMat = new Mat (tempTex.height, tempTex.width, CvType.CV_8UC1);
        Utils.texture2DToMat (tempTex, srcMat);
        Destroy(tempTex);
        
        //BW 位元組化
        int thresh = BW_Threshold;
        var im_bw = Imgproc.threshold(srcMat, srcMat, thresh, 255, Imgproc.THRESH_BINARY);

        var originSize = srcMat.size();
        Imgproc.resize(srcMat, srcMat, srcMat.size()/sizePam, 0, 0, 0);

        //看BW圖
        // Mat im_bw_flip = new Mat();
        // srcMat.copyTo(im_bw_flip);
        // if(lastBW_tex != null) Destroy(lastBW_tex);
        // lastBW_tex = new Texture2D (im_bw_flip.cols (), im_bw_flip.rows (), TextureFormat.RGBA32, false);
        // Utils.matToTexture2D(im_bw_flip, lastBW_tex);
        // BW_Image.texture = lastBW_tex;


        //給定輸出圖
        //Mat dstMat = new Mat (srcMat.size (), CvType.CV_8UC3);

        //執行編號
        Mat labels = new Mat ();
        Mat stats = new Mat ();
        Mat centroids = new Mat ();
        int total = Imgproc.connectedComponentsWithStats (srcMat, labels, stats, centroids);

        //Debug.Log ("total : " + total);

        //為每個Blob建立Mat
        //List<Mat> blobs = new List<Mat>();
        //List<Point> bottomPoint = new List<Point>(); bottomPoint.Add(new Point());
        // for (int i = 0; i < total; ++i)
        //     blobs.Add(new Mat(srcMat.size (), CvType.CV_8UC3));

        //Blob 繪製
        // for (int i = 0; i < srcMat.rows (); ++i) {
        //     for (int j = 0; j < srcMat.cols (); ++j) {
        //         int lab = (int)labels.get (i, j) [0];   //該點的編碼
        //         blobs[lab].put (i, j, 255, 255, 255);   //是幾號就丟幾號
        //     }
        // }

        double largest_y = 0;
        for (int i = 1; i < total; ++i) {
            int x = (int)stats.get (i, Imgproc.CC_STAT_LEFT) [0];
            int y = (int)stats.get (i, Imgproc.CC_STAT_TOP) [0];
            int height = (int)stats.get (i, Imgproc.CC_STAT_HEIGHT) [0];
            int width = (int)stats.get (i, Imgproc.CC_STAT_WIDTH) [0];

            //Imgproc.circle (dstMat, new Point (x + width/2, y + height), 30, new Scalar (255, 0, 0), -1);
            Point p = new Point (x + width/2, y + height);
            //bottomPoint.Add(p);

            if(p.y > largest_y){
                largest_y = p.y;
            }
        }

        judgeDepth.OnNearPsersonCome.Invoke(SignalInput.height - (float)largest_y);

        return;

        //清除上一個Layer
        // foreach (var item in OutputLayer)
        //     Destroy(item.gameObject);
        // OutputLayer.Clear();

        // if(useDebug){
        //     foreach (var item in debugLayers)
        //         Destroy(item.gameObject);
        //     debugLayers.Clear();
        // }

        //Imgproc.resize(srcMat, srcMat, originSize, 0, 0, 0);

        // for (int i = 1; i < total; i++)
        // {
        //     Texture2D outTex = new Texture2D (srcMat.cols (), srcMat.rows (), TextureFormat.RGBA32, false);
        //     Texture2D outTexMask = new Texture2D (srcMat.cols (), srcMat.rows (), TextureFormat.RGBA32, false);
        //     Imgproc.resize(blobs[i], blobs[i], originSize, 0, 0, 0);
        //     Utils.matToTexture2D (blobs[i], outTex);
        //     nnCam.ProccessSingle(outTex, outTexMask);
        //     Destroy(outTex);

        //     var temp = Instantiate(Prefab_Layer, CanvasLayer[StayLayer(bottomPoint[i])]);
        //     temp.SetDisplay(outTexMask);
        //     OutputLayer.Add(temp);

        //     if(useDebug){
        //         var temp2 = Instantiate(Prefab_Layer, debug_Mats);
        //         temp2.SetDisplay(outTexMask);
        //         debugLayers.Add(temp2);
        //     }
        // }
    }

    int StayLayer(Point p){
        if(p.y > SignalInput.height/sizePam - depthThreshold){
            //Debug.Log($"{p.y} , {virtualInput.height - threshold} : 0");
            return 0;
        }
        if(p.y > SignalInput.height/sizePam - depthThreshold * 2){
            //Debug.Log($"{p.y} , {virtualInput.height - threshold} : 1");
            return 1;
        }
        if(p.y > SignalInput.height/sizePam - depthThreshold * 3){
            //Debug.Log($"{p.y} , {virtualInput.height - threshold} : 2");
            return 2;
        }
        if(p.y > SignalInput.height/sizePam - depthThreshold * 4){
            //Debug.Log($"{p.y} , {virtualInput.height - threshold} : 3");
            return 3;
        }
        if(p.y > SignalInput.height/sizePam - depthThreshold * 5){
            //Debug.Log($"{p.y} , {virtualInput.height - threshold} : 4");
            return 4;
        }
        Debug.Log($"{p.y} , {SignalInput.height/sizePam - depthThreshold} : 5");
        return 4;
    }
}
