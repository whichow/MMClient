// ***********************************************************************
// Company          : 
// Author           : KimCh
// Copyright(c)     : KimCh
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Init The UI Root
/// </summary>
public class KUIRoot : MonoBehaviour
{
    private const int kScreenWidth = 1334;
    private const int kScreenHeight = 750;

    #region Field

    /// <summary>
    /// 
    /// </summary>
    private Transform _backgroundLayer;
    /// <summary>
    /// 
    /// </summary>
    private Transform _normalLayer;
    /// <summary>
    /// 
    /// </summary>
    public Transform _popupLayer { get;private set; }
    /// <summary>
    /// 
    /// </summary>
    private Transform _tutorialLayer;

    #endregion

    #region Property  

    private Camera _uiCamera;
    public Camera uiCamera
    {
        get
        {
            return _uiCamera;
        }
        private set
        {
            _uiCamera = value;
            uiRootCamera = _uiCamera;
        }
    }

    private Canvas _uiCanvas;
    public Canvas uiCanvas
    {
        get
        {
            return _uiCanvas;
        }
        private set
        {
            _uiCanvas = value;
            uiRootCanvas = _uiCanvas;
        }
    }

    public static Camera uiRootCamera
    {
        get;
        private set;
    }

    public static Canvas uiRootCanvas
    {
        get;
        private set;
    }

    #endregion

    #region Public API

    /// <summary>
    /// 屏幕坐标转换到UI坐标
    /// </summary>
    /// <param name="screenPoint"></param>
    /// <returns></returns>
    public static Vector2 ScreenPointToLocalPointInRectangle(Vector2 screenPoint)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)uiRootCanvas.transform, screenPoint, uiRootCamera, out localPoint);
        return localPoint;
    }

    /// <summary>
    /// 鼠标坐标转换的屏幕坐标
    /// </summary>
    /// <returns></returns>
    public static Vector2 MousePointToLocalPointInRectangle()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)uiRootCanvas.transform, Input.mousePosition, uiRootCamera, out localPoint);
        return localPoint;
    }

    #endregion

    #region Internal Methods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="window"></param>
    internal void AnchorToBackground(GameObject window)
    {
        AnchorUI(window, _backgroundLayer);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="window"></param>
    internal void AnchorToNormal(GameObject window)
    {
        AnchorUI(window, _normalLayer);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="window"></param>
    internal void AnchorToPopup(GameObject window)
    {
        AnchorUI(window, _popupLayer);
    }

    internal void AnchorToTutorial(GameObject window)
    {
        AnchorUI(window, _tutorialLayer);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="window"></param>
    /// <param name="parent"></param>
    private void AnchorUI(GameObject window, Transform parent)
    {
        var windowTransform = window.transform;
        var windowRectTransform = windowTransform as RectTransform;

        Vector3 anchoredPosition = Vector3.zero;
        Vector2 sizeDelta = Vector2.zero;
        Vector3 scale = Vector3.one;

        if (windowRectTransform)
        {
            anchoredPosition = windowRectTransform.anchoredPosition;
            sizeDelta = windowRectTransform.sizeDelta;
            scale = windowRectTransform.localScale;
        }
        else
        {
            anchoredPosition = windowTransform.localPosition;
            scale = windowTransform.localScale;
        }

        windowTransform.SetParent(parent, false);

        if (windowRectTransform)
        {
            windowRectTransform.anchoredPosition = anchoredPosition;
            windowRectTransform.sizeDelta = sizeDelta;
            windowRectTransform.localScale = scale;
        }
        else
        {
            windowTransform.localPosition = anchoredPosition;
            windowTransform.localScale = scale;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void CreateRoot()
    {
        var go = this.gameObject;

        go.name = "UIRoot";
        go.layer = LayerMask.NameToLayer("UI");
        go.AddComponent<RectTransform>();

        ///Camera
        var cameraObj = new GameObject("UICamera")
        {
            layer = go.layer
        };
        cameraObj.transform.SetParent(go.transform, false);
        cameraObj.transform.localPosition = new Vector3(0, 0, -1000f);

        ////Camera
        var camera = cameraObj.AddComponent<Camera>();
        this.uiCamera = camera;
        camera.clearFlags = CameraClearFlags.Depth;
        camera.depth = 10;
        camera.orthographic = true;

        camera.cullingMask = 1 << go.layer;
        camera.nearClipPlane = 0f;
        camera.farClipPlane = 100f;

        ////Canvas
        var canvas = go.AddComponent<Canvas>();
        this.uiCanvas = canvas;
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.pixelPerfect = false;
        canvas.worldCamera = camera;

        int sw = Screen.width;
        int sh = Screen.height;
        float sr = sw / (float)sh;
        int rw = (int)(sr * kScreenHeight);
        int rh = kScreenHeight;
        ////Canvas Scaler
        var canvasScaler = go.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        //canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
        canvasScaler.referenceResolution = new Vector2(rw, rh);

        //int ManualWidth = 1334;
        //int ManualHeight = 750;
        //if (Convert.ToSingle(Screen.height) / Screen.width > Convert.ToSingle(ManualHeight) / ManualWidth)
        //{
        //    int manualHeight;
        //    manualHeight = Mathf.RoundToInt(Convert.ToSingle(ManualWidth) / Screen.width * Screen.height);
        //    blPadMode = true;
        //    float scale = Convert.ToSingle(manualHeight / 750f);
        //    mUICamera.fieldOfView *= scale;
        //}
        //else
        //{
        //    blPadMode = false;
        //    //Canvas Scaler
        //    var canvasScaler = _uiCanvas.GetComponent<CanvasScaler>();
        //    canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        //    int sw = Screen.width;
        //    int sh = Screen.height;
        //    float sr = sw / (float)sh;
        //    int rw = (int)(sr * ManualHeight);
        //    int rh = ManualHeight;
        //    canvasScaler.referenceResolution = new Vector2(rw, rh);
        //}

        ////Sub
        _backgroundLayer = this.CreateSubRoot("Background", 100).transform;
        _normalLayer = this.CreateSubRoot("Normal", 200).transform;
        _popupLayer = this.CreateSubRoot("Popup", 300).transform;
        _tutorialLayer = this.CreateSubRoot("Tutorial", 400).transform;

        //Add Event System
        if (!EventSystem.current)
        {
            var eventObj = new GameObject(typeof(EventSystem).Name)
            {
                layer = go.layer
            };
            eventObj.transform.SetParent(go.transform, false);
            eventObj.AddComponent<EventSystem>();
            eventObj.AddComponent<StandaloneInputModule>();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="sort"></param>
    /// <returns></returns>
    private GameObject CreateSubRoot(string name, int sort)
    {
        var go = new GameObject(name)
        {
            layer = this.gameObject.layer
        };

        go.transform.SetParent(this.transform, false);
        go.transform.localPosition = new Vector3(0, 0, -sort);

        var canvas = go.AddComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingOrder = sort;
        if (name == "Normal")
        {
            canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;
        }

        var rectTransform = go.GetComponent<RectTransform>();
        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;

        go.AddComponent<GraphicRaycasterEx>();

        return go;
    }

    #endregion

    #region Unity

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        _Instance = this;
        this.CreateRoot();
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnDestroy()
    {
        _Instance = null;
    }

    #endregion

    #region Static

    private static KUIRoot _Instance;

    public static KUIRoot Instance
    {
        get
        {
            if (!_Instance)
            {
                _Instance = new GameObject("UIRoot").AddComponent<KUIRoot>();
            }
            return _Instance;
        }
    }

    #endregion
}
