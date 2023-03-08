using UnityEngine;
using UnityEngine.UI;

namespace NNCam {

public sealed class Compositor : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] WebcamInput _input = null;
    [SerializeField] Texture2D _background = null;
    [SerializeField, Range(0.01f, 0.99f)] float _threshold = .5f;
    [SerializeField] ResourceSet _resources = null;
    [SerializeField] Shader _shader = null;
    [SerializeField] RenderTexture BackgroundTex;
    [SerializeField] RenderTexture OriginImg;
    [SerializeField] RenderTexture OutputTex;
    [SerializeField] RenderTexture MaskImg;
    [SerializeField] RenderTexture TempBuff;
    [SerializeField] int noHumanValue = 3000;
    public Slider TH_Slider;

    public RenderTexture GetOutputTex => OutputTex;

    #endregion

    #region Internal objects

    SegmentationFilter _filter;
    Material _material;

    #endregion

    #region MonoBehaviour implementation

    void Start()
    {
        _filter = new SegmentationFilter(_resources);
        _material = new Material(_shader);

        TH_Slider.onValueChanged.AddListener(x => {
            _threshold = x;
            SystemConfig.Instance.SaveData("NN_TH", x);
        });
        TH_Slider.value = SystemConfig.Instance.GetData<float>("NN_TH");
    }

    void OnDestroy()
    {
        _filter.Dispose();
        Destroy(_material);
    }

    void Update(){
      _filter.ProcessImage(_input.Texture);
      
        _material.SetTexture("_Background", _background);
        _material.SetTexture("_CameraFeed", _input.Texture);
        _material.SetTexture("_Mask", _filter.MaskTexture);
        _material.SetFloat("_Threshold", _threshold);
        Graphics.Blit(null, OutputTex, _material, 0);

        Graphics.Blit(_filter.MaskTexture, MaskImg);
        Graphics.Blit(_input.Texture, OriginImg);
    }

    public void ProccessSingle(Texture2D tex, Texture2D opt){
        _material.SetTexture("_CameraFeed", _input.Texture);
        _material.SetTexture("_Mask", tex);
        _material.SetFloat("_Threshold", _threshold);

        Graphics.Blit(null, TempBuff, _material, 0);
        Graphics.CopyTexture(TempBuff, opt);
    }

    public void CatchBackTex(){
        Graphics.Blit(_input.Texture, BackgroundTex);
    }

    Texture2D SavedTexture;
    public bool IsNoPersion(){
        if(_filter != null && _filter.MaskTexture != null){
            if(SavedTexture != null)
            {
                Destroy(SavedTexture);
            }

            SavedTexture = new Texture2D(_filter.MaskTexture.width, _filter.MaskTexture.height, TextureFormat.RGBA32, false);
            RenderTexture.active = (RenderTexture) _filter.MaskTexture;
            SavedTexture.ReadPixels(new Rect(0, 0, _filter.MaskTexture.width, _filter.MaskTexture.height), 0, 0);
            RenderTexture.active = null;
            SavedTexture.Apply();

            Color[] pixels = SavedTexture.GetPixels();

            ///MaskImg = SavedTexture;

            float totalPixel = 0;
            foreach (var p in pixels)
            {
                totalPixel += p.r;
            }
            
            // Debug.Log(pixels[0].r);
            // Debug.Log(pixels[0].g);
            // Debug.Log(pixels[0].b);
            // Debug.Log(pixels[0].a);
            // Debug.Log(pixels[0].grayscale);
            //Debug.Log($"Total Himan Pix val : {totalPixel}");
            if(totalPixel < noHumanValue) return true;
        }

        return false;
    }

    // void OnRenderImage(RenderTexture source, RenderTexture destination)
    // {
    //     _material.SetTexture("_Background", _background);
    //     _material.SetTexture("_CameraFeed", _input.Texture);
    //     _material.SetTexture("_Mask", _filter.MaskTexture);
    //     _material.SetFloat("_Threshold", _threshold);
    //     Graphics.Blit(null, destination, _material, 0);
    //     Graphics.Blit(destination, OutputTex);
    // }

    #endregion
}

} // namespace NNCam
