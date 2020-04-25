using UnityEngine;

public class ScreenCoordinateTransform
{
    #region Instance
    private static ScreenCoordinateTransform _instance;
    public static ScreenCoordinateTransform Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ScreenCoordinateTransform();
            }
            if (_sceneCamera == null || _gameCamera == null || _uiCamera == null)           //防止对象被销毁
            {
                Init();
            }
            return _instance;
        }

    }
    #endregion

    private static Camera _sceneCamera;
    public Camera sceneCamera
    {
        get { return _sceneCamera; }
    }

    private static Camera _gameCamera;
    public Camera gameCamera
    {
        get { return _gameCamera; }
    }

    private static Camera _uiCamera;
    public Camera uiCamera
    {
        get { return _uiCamera; }
    }

    private static RectTransform _gameCanvas;
    public RectTransform gameCanvas
    {
        get { return _gameCanvas; }
    }

    private static RectTransform _uiCanvas;
    public RectTransform uiCanvas
    {
        get { return _uiCanvas; }
    }

    #region C&D
    public ScreenCoordinateTransform()
    {
        _sceneCamera = Camera.main;

        var gameUI = GameObject.Find("GameUI");
        _gameCamera = gameUI.GetComponentInChildren<Camera>();
        _gameCanvas = gameUI.GetComponentInChildren<Canvas>().transform as RectTransform;
        //_gameCamera = KUIManager.Instance.GameCamera;
        //_gameCanvas = KUIManager.Instance.GameCanvas.transform as RectTransform;

        _uiCamera = KUIRoot.uiRootCamera;
        _uiCanvas = KUIRoot.uiRootCanvas.transform as RectTransform;
    }
    #endregion

    private static void Init()
    {
        _sceneCamera = Camera.main;

        var gameUI = GameObject.Find("GameUI");
        _gameCamera = gameUI.GetComponentInChildren<Camera>();
        _gameCanvas = gameUI.GetComponentInChildren<Canvas>().transform as RectTransform;
        //_gameCamera = KUIManager.Instance.GameCamera;
        //_gameCanvas = KUIManager.Instance.GameCanvas.transform as RectTransform;

        _uiCamera = KUIRoot.uiRootCamera;
        _uiCanvas = KUIRoot.uiRootCanvas.transform as RectTransform;
    }

    public Vector2 LocalPointToUIPixelPoint(RectTransform rectTransform, Camera original, Camera target, Vector3 pos)
    {
        Vector2 pixel;
        Vector3 worldPoint = RectTransformUtility.WorldToScreenPoint(original, pos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, worldPoint, target, out pixel);
        return pixel;
    }

    public Vector3 WorldPointToWorldUIPoint(RectTransform rectTransform, Camera original, Camera target, Vector3 pos)
    {
        Vector3 worldPos;
        Vector3 worldPoint = RectTransformUtility.WorldToScreenPoint(original, pos);
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, worldPoint, target, out worldPos);
        return worldPos;
    }

    public Vector3 WorldScenePointToWorldUIPoint( Camera original, Camera target, Vector3 pos)
    {
        Vector3 worldPos;
        Vector3 worldPoint = RectTransformUtility.WorldToScreenPoint(original, pos);
        worldPos = target.ScreenToWorldPoint(worldPoint);
        ///RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, worldPoint, target, out worldPos);
        return worldPos;
    }

    public Vector3 WorldScreenPointToWorldScenePiontGet(Vector3 pos, Camera camera)
    {
        return camera.ScreenToWorldPoint(pos);
    }

    #region 城建坐标  <====> UGUI坐标
    public void LocalGameUIPointToUIPointSet(RectTransform rectObject, Vector3 pos, RectTransform target = null)
    {
        rectObject.anchoredPosition = LocalGameUIPointToUIPointGet(  pos,target);

    }
    public Vector2 LocalGameUIPointToUIPointGet(Vector3 pos, RectTransform target = null)
    {
        if (target)
            return LocalPointToUIPixelPoint(target, _gameCamera, uiCamera, pos);
        else
            return LocalPointToUIPixelPoint(_gameCanvas, _gameCamera, uiCamera, pos);
    }

    /// <summary>
    /// 设置物体 UI坐标到城建坐标
    /// </summary>
    /// <param name="rectObject"></param>
    /// <param name="pos"></param>
    /// <param name="target"></param>
    public void WorldUIPointToGameUIPointSet(RectTransform rectObject, Vector3 pos, RectTransform target = null)
    {
        rectObject.transform.position = WorldUIPointToGameUIPointGet(pos, target);
    }
    /// <summary>
    /// UI坐标到城建坐标
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public Vector3 WorldUIPointToGameUIPointGet(Vector3 pos, RectTransform target = null)
    {
        if (target)
            return WorldPointToWorldUIPoint(target, _uiCamera, _gameCamera, pos);
        else
            return WorldPointToWorldUIPoint(_gameCanvas, _uiCamera, _gameCamera, pos);
    }

    /// <summary>
    /// 城建坐标 到 UI坐标
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public Vector3 WorldGameUIPointToUIPointtGet(Vector3 pos, RectTransform target = null)
    {
        if (target)
            return WorldPointToWorldUIPoint(target, _gameCamera, _uiCamera, pos);
        else
            return WorldPointToWorldUIPoint(_gameCanvas, _gameCamera, _uiCamera, pos);
    }

    #endregion

    #region 城建坐标  <====> 场景坐标
    public void LocalScenePointToGameUIPointSet(RectTransform rectObject, Vector3 pos, RectTransform target = null)
    {
            rectObject.anchoredPosition = LocalScenePointToGameUIPointGet( pos,target);

    }
    public Vector2 LocalScenePointToGameUIPointGet( Vector3 pos, RectTransform target = null)
    {
        if (target)
            return LocalPointToUIPixelPoint(target, _sceneCamera, _gameCamera, pos);
        else
            return  LocalPointToUIPixelPoint(_gameCanvas, _sceneCamera, _gameCamera, pos);
    }


    /// <summary>
    /// 设置 城建坐标 到场景坐标
    /// </summary>
    /// <param name="rectObject"></param>
    /// <param name="pos"></param>
    /// <param name="target"></param>
    public void WorldScenePointToGameUIPointSet(RectTransform rectObject, Vector3 pos, RectTransform target = null)
    {

        rectObject.position = WorldScenePointToGameUIPointGet(pos, target);
    }

    public Vector3 WorldScenePointToGameUIPointGet(Vector3 pos, RectTransform target = null)
    {
        if (target)
            return WorldPointToWorldUIPoint(target, _sceneCamera, _gameCamera, pos);
        else
            return WorldPointToWorldUIPoint(_gameCanvas, _sceneCamera, _gameCamera, pos);
    }

    #endregion

    #region 场景坐标<====> UGUI坐标
    public void UIPiontToScenePoint(RectTransform rectObject, Vector3 pos, RectTransform target = null)
    {
        rectObject.anchoredPosition = LocalUIPiontToScenePointGet(pos, target);
    }
    public Vector2 LocalUIPiontToScenePointGet(Vector3 pos, RectTransform target = null)
    {
        if (target)
            return LocalPointToUIPixelPoint(target, _sceneCamera, _gameCamera, pos);
        else
            return LocalPointToUIPixelPoint(_gameCanvas, _sceneCamera, _gameCamera, pos);
    }
    /// <summary>
    /// 设置 屏幕坐标  到 UI坐标
    /// </summary>
    /// <param name="rectObject"></param>
    /// <param name="pos"></param>
    /// <param name="target"></param>
    public void WorldScenePointToUIPiontSet(RectTransform rectObject, Vector3 pos, RectTransform target = null)
    {
        rectObject.position = WorldScenePointToUIPiontGet(pos, target);
    }
    /// <summary>
    /// 获取 屏幕坐标 到 ui坐标
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public Vector3 WorldScenePointToUIPiontGet(Vector3 pos, RectTransform target = null)
    {
            return WorldScenePointToWorldUIPoint( _sceneCamera, uiCamera, pos);
    }

    public Vector3 WorldUIScreenPointToWorldScenePiontGet(Vector3 pos)
    {
       return WorldScreenPointToWorldScenePiontGet(pos,uiCamera);
    }
    #endregion

    #region 判断坐标点 是否在指定的摄像机内
    public bool IsGameScreenCoord(Vector3 pos)
    {
        return IsSceneScreenCoord(gameCamera,pos);
    }
    public bool IsSceneScreenCoord(Vector3 pos)
    {
        return IsSceneScreenCoord(sceneCamera, pos);
    }
    public bool IsUISceneScreenCoord(Vector3 pos)
    {
        return IsSceneScreenCoord(uiCamera, pos);
    }
    public bool IsSceneScreenCoord(Camera camera, Vector3 pos)
    {
        Vector3 screenPos = camera.WorldToScreenPoint(pos);
        if (screenPos.x > 0 && screenPos.x < camera.pixelWidth && screenPos.y > 0  && screenPos.y < camera.pixelHeight)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    #endregion
}

