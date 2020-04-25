// ***********************************************************************
// Company          : 
// Author           : KimCh
// Created          : 
//
// Last Modified By : KimCh
// Last Modified On : 
// ***********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;

using Game.UI;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Build
{
    public class GameCamera : MonoBehaviour
    {
        #region Delegate

        public delegate bool ShowEventHandler(out Vector3 position);

        #endregion

        #region Event

        /// <summary>
        /// 
        /// </summary>
        public static Action<Transform> OnLongPressStart;
        /// <summary>
        /// 
        /// </summary>
        public static Action<Transform, float> OnLongPressProgress;
        /// <summary>
        /// 
        /// </summary>
        public static Action OnLongPressBreak;
        /// <summary>
        /// 点击非UGUI 元素时 ，回调MapObjectOn.EmptySpaceClick
        /// </summary>
        public static Action OnEmptySpaceClick;
        /// <summary>
        /// 
        /// </summary>
        public static event Func<Transform, bool, Transform> OnClickEntity;
        /// <summary>
        /// 城建任意位置点击事件（在城建被选中时注册，建筑物被选中后，点击其他地方（失去焦点））
        /// </summary>
        public static event Action<Transform> OnClick;
        /// <summary>
        /// 
        /// </summary>
        public static event Action OnScrollBegin;
        /// <summary>
        /// 
        /// </summary>
        public static Action OnScroll;
        /// <summary>
        /// 
        /// </summary>
        public static Action OnScrollEnd;
        /// <summary>
        /// param is drag Object
        /// </summary>
        public static Action<Transform> OnDragObjectStart;
        /// <summary>
        /// return is allow
        /// </summary>
        public static Func<bool> OnDragObjectProgress;
        /// <summary>
        /// 拖拽过程完成(拖拽时 鼠标松开)事件
        /// </summary>
        public static Action OnDragObjectFinish;

        public static Action OnDragProcess;

        /// <summary>
        /// 刷地板
        /// </summary>
        public static Action<Int2> OnSurfaceDragProgress;

        /// <summary>
        /// 
        /// </summary>
        public static event Action OnCameraReach;

        public Action OnCameraReachCallBack;
        /// <summary>
        /// 
        /// </summary>
        public Action preClick;

        public Func<Transform> onTryObjectDrag;

        /// <summary>
        /// 点击城建元素回调
        /// </summary>
        public static Action OnBuildingEnum;

        /// <summary>
        /// 点击非城建元素回调
        /// </summary>
        public static Action OnNoBuildingEnum;
        /// <summary>
        /// 鼠标点击UGUI事件
        /// </summary>
        public static event Action OnClickUGUI;
        #endregion

        #region Static

        /// <summary>
        /// 
        /// </summary>
        public static GameCamera Instance;

        public static GameObject CurrentUICommponent;

        #endregion

        #region Field 

        /// <summary>
        /// 
        /// </summary>
        public Vector2 hardBorder = new Vector2(2f, 2f);
        /// <summary>
        /// 
        /// </summary>
        public Vector2 borderModify = new Vector2(-2f, -2f);

        public Vector3 center = Vector3.zero;
        public float height = 50f;
        public float width = 50f;

        private bool _chargingClick;
        private bool _scrollDragFailed;
        /// <summary>
        /// 
        /// </summary>
        private Vector3 _scrollOffset;
        /// <summary>
        /// 
        /// </summary>
        private Vector3 _cameraOffset;
        /// <summary>
        /// 修正值
        /// </summary>
        private Vector3 _scrollModification;

        /// <summary>
        /// 当前焦点
        /// </summary>
        private Transform _focusObject;

        #endregion

        #region Property


        public MapObject curClickMapObject
        {
            get;
            private set;
        }
        /// <summary>
        /// 主相机
        /// </summary>
        public Camera mainCamera
        {
            get;
            private set;
        }
        /// <summary>
        /// 地面平面
        /// </summary>
        public Plane groundPlane
        {
            get { return Map.Instance.MapPlane; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Vector3 mouseScreenPoint
        {
            get { return Input.mousePosition; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Vector3 mouseViewportPoint
        {
            get { return mainCamera.ScreenToViewportPoint(Input.mousePosition); }
        }
        /// <summary>
        /// 
        /// </summary>
        public Vector3 mouseWorldPoint
        {
            get { return mainCamera.ScreenToWorldPoint(Input.mousePosition); }
        }

        private PointerEventData eventData;
        private List<RaycastResult> results;

        #endregion 

        #region Method

        /// <summary>
        /// 别人家建筑不支持操作
        /// </summary>
        /// <returns></returns>
        private bool IsSelfPlayer()
        {
            return true;
        }
        /// <summary>
        /// 点击点是否在UGUI上面
        /// </summary>
        /// <returns></returns>
        private bool IsPointerOverUIObject()
        {
            if (!EventSystem.current)
            {
                return false;
            }

            var eventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            var raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raycastResults);
            return raycastResults.Count > 0;
        }

        #region 获取屏幕当前鼠标点位置的物体

        /// <summary>
        /// 获取屏幕当前鼠标点位置的物体
        /// </summary>
        /// <returns></returns>
        public Transform GetScreenPointObject()
        {
            var touchPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var hit2D = Physics2D.Raycast(touchPoint, Vector2.zero, 1000f, LayerManager.BuildingLayerMask);
            return hit2D.transform;
        }

        /// <summary>
        /// 获取屏幕当前鼠标点位置的指定层级的物体
        /// </summary>
        /// <param name="layerMask"></param>
        /// <returns></returns>
        public Transform GetScreenPointObject(LayerMask layerMask)
        {
            var touchPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(touchPoint, Vector2.zero, 1000f, layerMask);
            return hit.transform;
        }

        /// <summary>
        /// 获取屏幕当前鼠标点位置的所有物体
        /// </summary>
        /// <returns></returns>
        public GameObject[] GetScreenPointObjects()
        {
            var touchPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var hits = Physics2D.RaycastAll(touchPoint, Vector2.zero, 1000f, LayerManager.BuildingLayerMask);

            var objArray = new GameObject[hits.Length];
            for (int i = objArray.Length - 1; i >= 0; i--)
            {
                objArray[i] = hits[i].transform.gameObject;
            }
            return objArray;
        }

        /// <summary>
        /// 获取屏幕当前鼠标点位置的所有物体
        /// </summary>
        /// <returns></returns>
        public Transform[] GetScreenPointObjects(LayerMask layerMask)
        {
            var touchPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var hits = Physics2D.RaycastAll(touchPoint, Vector2.zero, 1000f, layerMask);

            var objArray = new Transform[hits.Length];
            for (int i = objArray.Length - 1; i >= 0; i--)
            {
                objArray[i] = hits[i].transform;
            }
            return objArray;
        }

        #endregion

        #region 获取坐标点

        /// <summary>
        /// 获取坐标点（世界屏幕坐标点）
        /// </summary>
        /// <returns></returns>
        public Vector3 ScreenPointToNavCoord()
        {
            return ScreenPointToNavCoord(Input.mousePosition);
        }

        /// <summary>
        /// 获取坐标点（世界屏幕坐标点）
        /// </summary>
        /// <returns></returns>
        public Vector3 ScreenPointToNavCoord(Vector3 screenPos)
        {
            var ray = mainCamera.ScreenPointToRay(screenPos);
            float enter;
            Map.Instance.MapPlane.Raycast(ray, out enter);
            var point = ray.GetPoint(enter);
            point.z = 0f;
            return point;
        }

        /// <summary>
        /// 获取地板坐标点
        /// </summary>
        /// <returns></returns>
        public Vector3 GetGroundPlaneHit()
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            float enter;
            Map.Instance.MapPlane.Raycast(ray, out enter);
            return ray.GetPoint(enter);
        }

        /// <summary>
        /// 获取屏幕坐标点
        /// </summary>
        /// <param name="mapObject"></param>
        /// <returns></returns>
        public Vector2 GetSceneCoord(GameObject mapObject)
        {
            Vector3 pos = BubbleManager.Instance.WorldPointToLocalPointInRectangle(mapObject.transform.position);
            return new Vector2(pos.x, pos.y);
        }

        //public Vector2 GetSceneCoord(GameObject mapObject)
        //{
        //    Vector3 vector3 =  mainCamera.WorldToScreenPoint(mapObject.transform.position);
        //    Vector2 vector2 =new Vector2(vector3.x, vector3.y);
        //    Debug.Log("屏幕+"+ Screen.width+"---"+ vector2+"--"+ mapObject.name);
        //    vector2 = vector2- new Vector2(ScreenWidth, KScreenHeight)/2;
        //    Debug.Log("屏幕+"+ ScreenWidth + "---"+ KScreenHeight+"---"+ new Vector2(ScreenWidth, KScreenHeight) / 2);
        //    return vector2;
        //}

        #endregion

        /// <summary>
        /// 刷地板
        /// </summary>
        public void SurfaceEditor(bool b)
        {
            if (b)
            {
                SwitchState(OpState.kSurface);
            }
            else
            {
                SwitchState(OpState.kControl);
            }
        }

        #region 设置当前拖拽的城建元素

        /// <summary>
        /// 设置城建元素 当前拖拽的物体
        /// </summary>
        /// <param name="mapObject"></param>
        public void GrabMapObject(MapObject mapObject)
        {
            this.GrabMapObject(mapObject.transform);
        }

        /// <summary>
        /// 创建元素时 设置城建元素 当前拖拽的物体
        /// </summary>
        /// <param name="target"></param>
        private void GrabMapObject(Transform target)
        {
            _draggedObject = target;
            SwitchState(OpState.kDrag);
        }

        #endregion

        /// <summary>
        /// 场景成键元素 开始拖拽
        /// </summary>
        private void StartLongPress()
        {
            //Debug.Log("StartLongPress");
            //IsBuildingMoveState = true;
            _touchDuration = 1;
            _touchObject = GetScreenPointObject();
            if (!_touchObject)
            {
                return;
            }

            if (!Map.Instance.IsDragMode && this.CheckClickedObject(ref _touchObject, true))
            {
                if (OnLongPressStart != null)
                {
                    OnLongPressStart(_touchObject);
                }
            }
        }

        /// <summary>
        /// 长按取消 回调（鼠标在长安过程中 松开鼠标）
        /// </summary>
        private void BreakLongPress()
        {
            _touchObject = null;
            if (OnLongPressBreak != null)
            {
                OnLongPressBreak();

            }
        }

        /// <summary>
        /// 城建元素被长时间按住 事件
        /// </summary>
        /// <param name="progress"></param>
        private void ProgressLongPress(float progress)
        {
            //Debug.Log("OnLongPressProgress");
            if (OnLongPressProgress != null)
            {
                OnLongPressProgress(_touchObject, progress);
            }
        }

        /// <summary>
        /// 获取当前拖拽物体
        /// </summary>
        /// <returns></returns>
        private bool TryGetDragObject()
        {
            if (this.onTryObjectDrag != null)
            {
                _draggedObject = this.onTryObjectDrag();

                if (_draggedObject && this.CheckClickedObject(ref _draggedObject, true))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="longPress"></param>
        /// <returns></returns>
        private bool CheckClickedObject(ref Transform target, bool longPress)
        {
            if (OnClickEntity != null)
            {
                foreach (var invocation in OnClickEntity.GetInvocationList())
                {
                    target = invocation.DynamicInvoke(target, longPress) as Transform;
                    if (!target)
                    {
                        return false;
                    }
                }
            }

            if (IsRestriction(Restrictions.Click))
            {
                if (_AllowedObjects.Count == 0)
                {
                    return ((_AllowedUIObjects.Count > 0) && _AllowedUIObjects.Contains(target.gameObject));
                }

                if (!_AllowedObjects.Contains(target.gameObject))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 所有点击事件前(通知)
        /// </summary>
        private void PreClick()
        {
            if (preClick != null)
            {
                preClick();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void Click()
        {
            //if (IsRestriction(Restrictions.Click))
            //{
            //    return;
            //}
            bool flag = IsPermission(Restrictions.Click);
            Debuger.Log("是否允许点击：" + flag);

            var clickObject = GetScreenPointObject(LayerManager.BuildingLayerMask);
            if (clickObject)                    //判断点击的物体是场景物体还是城建元素层物体
            {
                Debuger.Log("当前点击为建筑层");

                if (!this.CheckClickedObject(ref clickObject, false))
                {
                    return;
                }

                if (Map.Instance.IsBuildingMoveState)
                {
                    return;
                }

                if (_focusObject != clickObject)
                {
                    if (_focusObject)
                    {
                        _focusObject.SendMessage("OnFocus", false, SendMessageOptions.DontRequireReceiver);
                    }
                    _focusObject = clickObject;
                    clickObject.SendMessage("OnFocus", true, SendMessageOptions.DontRequireReceiver);
                }

                clickObject.SendMessage("OnTap", SendMessageOptions.DontRequireReceiver);
                Instance.curClickMapObject = clickObject.GetComponent<MapObject>();
                if (OnBuildingEnum != null)
                {
                    OnBuildingEnum();
                }
            }
            else
            {
                Debuger.Log("当前点击为非建筑层！");
                var clickObjects = GetScreenPointObjects(LayerManager.AreaLayerMask | LayerManager.ClickerLayerMask);
                foreach (var item in clickObjects)
                {
                    clickObject = item;
                    if (item && this.CheckClickedObject(ref clickObject, false))
                    {
                        item.SendMessage("OnTap", SendMessageOptions.DontRequireReceiver);
                    }
                }

                if (flag && _focusObject)
                {
                    _focusObject.SendMessage("OnFocus", false, SendMessageOptions.DontRequireReceiver);
                    _focusObject = null;
                }

                if (flag && OnEmptySpaceClick != null)
                {
                    if (Map.Instance.IsBuildingMoveState)
                    {
                        Debuger.Log("当前城建状态，需要对将要律建造的建筑移动到当前位置XXX");

                    }
                    else
                    {
                        OnEmptySpaceClick();
                    }
                }
            }

            //todo: 此处要判断是否要取消建筑
            if (flag && OnClick != null)
            {
                if (Map.Instance.IsBuildingMoveState)
                {
                    Debuger.Log("当前城建状态，需要对将要律建造的建筑移动到当前位置");

                }
                else
                {
                    OnClick(clickObject);
                }
            }

            if (Map.Instance.IsBuildingMoveState)
            {
                Debuger.Log("需要对将要律建造的建筑移动到当前位置");
                Map.Instance.CurBuildingClickMove(Input.mousePosition);
            }

            //if (Map.DragCancelCallBack != null)
            //{
            //    Map.DragCancelCallBack();
            //}
        }

        private void ClickTarget()
        {
            //OnStopFollow -= this.ClickTarget;
            //if (this.showOwner == null)
            //{
            //    this.showOwner = this;
            //}
            //OnClick -= this.OnClickAnywhere;
            //Disallow(this.showOwner);
            //if (this.zoomBeforeShow >= 0f)
            //{
            //    KUIManager.Instanse.CallTimeout(() =>
            //    {
            //        Debug.Log("Zooming  back...");
            //        GameCamera.Instance.Zoom(this.zoomBeforeShow, false);
            //    }, 0.15f);
            //}
            //this.zoomBeforeShow = -1f;
            //this.target = null;
        }

        private void OnClickAnywhere(Transform target)
        {
            this.ClickTarget();
        }

        /// <summary>
        /// Scroll 范围
        /// </summary>
        private void ClampScroll()
        {
            float hard = Mathf.Lerp(this.hardBorder.x, this.hardBorder.y, this.zoom);
            float modify = Mathf.Lerp(this.borderModify.x, this.borderModify.y, this.zoom);

            Vector3 zero = Vector3.zero;
            zero.x = Mathf.Clamp(this._scrollOffset.x, ((this.center.x - this.width) - hard) + modify, ((this.center.x + this.width) + hard) - modify);
            zero.y = Mathf.Clamp(this._scrollOffset.y, ((this.center.y - this.height) - hard) + modify, ((this.center.y + this.height) + hard) - modify);

            this._scrollOffset = zero;
            if (hard != 0f)
            {
                Vector3 b = Vector3.zero;
                b.x = Mathf.Clamp(this._scrollOffset.x, (this.center.x - this.width) + modify, (this.center.x + this.width) - modify);
                b.y = Mathf.Clamp(this._scrollOffset.y, (this.center.y - this.height) + modify, (this.center.y + this.height) - modify);
                this._scrollOffset = Vector3.Lerp(this._scrollOffset, b, Time.deltaTime * 2f);
            }
        }

        #endregion

        #region 将ui界面移动到指定位置，使城建物体居中

        private bool _showing;

        [SerializeField]
        private float _showDuration = 0.33f;

        /// <summary>
        /// 将ui界面移动到指定位置，使城建物体居中
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="canvas"></param>
        public void Show(RectTransform rect, Canvas canvas)
        {
            float wRatio = rect.rect.width / (canvas.transform as RectTransform).rect.width;
            float hRatio = rect.rect.height / (canvas.transform as RectTransform).rect.height;

            var xBorder = (Screen.width * wRatio) * 0.5f;
            var yBorder = (Screen.height * hRatio) * 0.5f;

            if (this.mainCamera.orthographic)
            {
                Vector2 vector = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, rect.position);
                Vector2 vector2 = vector;
                vector2.x = Mathf.Clamp(vector2.x, xBorder, Screen.width - xBorder);
                vector2.y = Mathf.Clamp(vector2.y, yBorder, Screen.height - yBorder);
                if (vector2 != vector)
                {
                    Vector2 vector3 = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
                    vector3 += vector - vector2;
                    var ray = mainCamera.ScreenPointToRay(vector3);
                    float dist;
                    groundPlane.Raycast(ray, out dist);
                    this.Show(ray.GetPoint(dist), null);
                }
            }
            else
            {
                this.Show(Vector3.zero, (out Vector3 position) =>
                {
                    Vector2 vector = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, rect.position);
                    Vector2 vector2 = vector;
                    vector2.x = Mathf.Clamp(vector2.x, xBorder, Screen.width - xBorder);
                    vector2.y = Mathf.Clamp(vector2.y, yBorder, Screen.height - yBorder);
                    if (vector2 == vector)
                    {
                        position = Vector3.zero;
                        return false;
                    }
                    Vector2 vector3 = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
                    vector3 += vector - vector2;
                    var ray = mainCamera.ScreenPointToRay(vector3);
                    float dist;
                    groundPlane.Raycast(ray, out dist);
                    position = ray.GetPoint(dist);
                    return true;
                });
            }
        }
        /// <summary>
        /// 将ui界面移动到指定位置，使城建物体居中
        /// </summary>
        /// <param name="point"></param>
        /// <param name="onShowPositionCalculate"></param>
        public void Show(Vector3 point, ShowEventHandler onShowPositionCalculate = null)
        {
            Unblock();
            SwitchState(OpState.kShow);

            StopAllCoroutines();
            if (this._showing)
            {
                _scrollOffset = (this.transform.position - _cameraOffset) - this.shakeOffset;
            }
            StartCoroutine(this.ShowRoutine(point, onShowPositionCalculate));
        }

        ///// <summary>
        ///// 将ui界面移动到指定位置(瞬间)，使城建物体居中
        ///// </summary>
        ///// <param name="point"></param>
        ///// <param name="onShowPositionCalculate"></param>
        //public void ShowMoment(Vector3 point, ShowEventHandler onShowPositionCalculate = null)
        //{
        //    Unblock();
        //    SwitchState(OpState.kShow);

        //    StopAllCoroutines();
        //    if (this._showing)
        //    {
        //        _scrollOffset = (this.transform.position - _cameraOffset) - this.shakeOffset;
        //    }
        //    StartCoroutine(this.ShowRoutine(point, onShowPositionCalculate));
        //}

        ///// <summary>
        ///// 将ui界面移动到指定位置，使城建物体居中
        ///// </summary>
        ///// <param name="point"></param>
        ///// <param name="onShowPositionCalculate"></param>
        //public void Show(Vector3 point, ShowEventHandler onShowPositionCalculate = null)
        //{
        //    Unblock();
        //    SwitchState(OpState.kShow);

        //    StopAllCoroutines();
        //    if (this._showing)
        //    {
        //        _scrollOffset = (this.transform.position - _cameraOffset) - this.shakeOffset;
        //    }
        //    StartCoroutine(this.ShowRoutine(point, onShowPositionCalculate));
        //}

        private IEnumerator ShowRoutine(Vector3 point, ShowEventHandler onShowPositionCalculate = null)
        {
            Block(GetRestrictions() | Restrictions.Move);
            var curveProgress = 0f;
            this._showing = true;
            yield return null;

            while (curveProgress < 1f)
            {
                Vector3 vector = (point + _cameraOffset) - this.transform.position;

                if (vector.sqrMagnitude > 0.25f)
                {
                    yield return null;
                    if ((onShowPositionCalculate == null) || onShowPositionCalculate(out point))
                    {
                        point.z = 0f;
                        curveProgress += Time.deltaTime / this._showDuration;

                        this.transform.position = Vector3.Lerp(this.transform.position, point + _cameraOffset, curveProgress) + this.shakeOffset;
                        _scrollOffset = (this.transform.position - _cameraOffset) - this.shakeOffset;
                        continue;
                    }
                }
                break;
            }

            _scrollOffset = (this.transform.position - _cameraOffset) - this.shakeOffset;
            Block(GetRestrictions() & ~Restrictions.Move);
            this._showing = false;
            //this.OnEnable();
            if (OnCameraReach != null)
            {
                OnCameraReach();
            }
            // if(OnCameraReachCallBack != null)
            SwitchState(OpState.kControl);
        }

        #endregion

        #region Zoom

        [Serializable]
        public class ZoomInfo
        {
            [Tooltip("使用最接近的缩放参数.")]
            public bool snapping = true;
            [Tooltip("缩放值.")]
            public float value;
        }

        public enum ZoomLevel : byte
        {
            Normal = 0,
            Near = 1,
            Far = 2,
            Limit = 3//极限
        }

        public static event Action<float> OnZoom;

        [Header("Zoom")]
        public ZoomInfo[] zoomLevels;
        public ZoomLevel defaultZoomLevel = ZoomLevel.Normal;
        public float maxZoom = -270f;
        public float minZoom = -80f;
        public float zoomSpeed = 2f;

        private float _maxZoomValue;
        private float _minZoomValue;

        private float _zoom;
        private float[] _snapZooms;
        private float _zoomPrevious;
        private float _defaultZoomValue;

        /// <summary>
        /// 
        /// </summary>
        public float zoom
        {
            get { return _zoom; }
            private set
            {
                if (IsPermission(Restrictions.Zoom))
                {
                    this.SetZoom(value);
                }
            }
        }

        private void SetZoom(float value)
        {
            _zoomPrevious = _zoom;
            _zoom = value;
            if (mainCamera.orthographic)
            {
                mainCamera.orthographicSize = Mathf.Lerp(minZoom, maxZoom, _zoom);
            }
            else
            {
                this.mainCamera.fieldOfView = 5f;
                _cameraOffset = this.transform.forward * Mathf.LerpUnclamped(this.minZoom, this.maxZoom, (_zoom * _zoom) * Mathf.Sign(_zoom));
            }

            if (OnZoom != null)
            {
                OnZoom(value);
            }
        }



        public float GetZoomValue(ZoomLevel level)
        {
            return this.zoomLevels[(int)level].value;
        }

        public void Zoom(ZoomLevel level, bool instant = false)
        {
            this.Zoom(this.GetZoomValue(level), instant);
        }

        public void Zoom(float value, bool instant = false)
        {
            value = Mathf.Clamp01(value);
            StopCoroutine("ZoomRoutine");
            if (!instant)
            {
                StartCoroutine("ZoomRoutine", value);
            }
            else
            {
                this.SetZoom(value);
            }
        }

        public void zoomFocus(System.Action zoomFinishBack = null)
        {
            StartCoroutine(ienumZoomFocus(zoomFinishBack));
        }
        private IEnumerator ienumZoomFocus(System.Action zoomFinishBack = null)
        {

            float from = 2.35f;
            float to = 5.27f;
            float zoomValue = 0;
            //Block(Restrictions.All);
            while (true)
            {
                zoomValue += Time.deltaTime;
                zoomValue = Mathf.Clamp01(zoomValue);
                SetZoom(zoomValue);

                if (zoomValue == 1)
                {
                    if (zoomFinishBack != null)
                    {
                        yield return new WaitForSeconds(0.5f);
                        zoomFinishBack();
                    }
                    yield break;

                }
                yield return 0;
            }
        }
        private IEnumerator ZoomRoutine(float value)
        {
            Block(GetRestrictions() | Restrictions.Zoom);

            var time = 0f;
            var curve = AnimationCurve.EaseInOut(0f, this.zoom, 1f, value);
            while (time < 1f)
            {
                this.SetZoom(curve.Evaluate(time));
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            this.SetZoom(value);

            Block(GetRestrictions() & ~Restrictions.Zoom);
        }

        private int GetNearestZoomLevelIndex(float value)
        {
            float max = float.MaxValue;
            int index = -1;
            for (int i = _snapZooms.Length - 1; i >= 0; i--)
            {
                float min = Mathf.Abs(_snapZooms[i] - value);
                if (min < max)
                {
                    max = min;
                    index = i;
                }
            }
            return index;
        }

        private void ClampZoom()
        {
            StopCoroutine("ClampZoomRoutine");
            StartCoroutine("ClampZoomRoutine");
        }

        private IEnumerator ClampZoomRoutine()
        {
            var targetZoom = 0f;
            var zoomStart = _zoomPrevious;
            this.zoom = Mathf.Clamp01(this.zoom);
            yield return null;
            if (_snapZooms.Length > 0)
            {
                int nearestZoomLevelIndex = this.GetNearestZoomLevelIndex(zoomStart);
                int sI = (int)Mathf.Sign(this.zoom - zoomStart);
                if (sI == 0)
                {
                    sI = 1;
                }
                int nearestI = nearestZoomLevelIndex + sI;
                targetZoom = nearestI < _snapZooms.Length ? _snapZooms[nearestI] : _snapZooms[nearestZoomLevelIndex];
            }
            else
            {
                targetZoom = Mathf.Clamp(this.zoom, _minZoomValue, _maxZoomValue);
            }

            while (Mathf.Abs(targetZoom - this.zoom) >= 0.001f)
            {
                this.zoom = Mathf.Lerp(this.zoom, targetZoom, Time.deltaTime * this.zoomSpeed);
                yield return null;
            }

            this.zoom = targetZoom;
        }

        private IEnumerator PitchZoomRoutine()
        {
            this.StopCoroutine("ClampZoomRoutine");

            Vector2[] oldTouchPoints = null;
            float oldTouchDistance = 0f;

            var touchIDs = new int[] { Input.GetTouch(0).fingerId, Input.GetTouch(1).fingerId };
            while ((Input.touchCount > 1) && ((touchIDs[0] == Input.GetTouch(0).fingerId) && (touchIDs[1] == Input.GetTouch(1).fingerId)))
            {
                if (oldTouchPoints == null)
                {
                    oldTouchPoints = new Vector2[2];
                    oldTouchPoints[0] = Input.GetTouch(0).position;
                    oldTouchPoints[1] = Input.GetTouch(1).position;
                    oldTouchDistance = Vector2.Distance(oldTouchPoints[0], oldTouchPoints[1]);
                }
                else
                {
                    Vector2 vector = new Vector2(this.mainCamera.pixelWidth, this.mainCamera.pixelHeight);

                    var touchPoints = new Vector2[] { Input.GetTouch(0).position, Input.GetTouch(1).position };
                    float magnitude = Vector2.Distance(touchPoints[0], touchPoints[1]);

                    Vector3 vector3 = this.transform.TransformDirection(((oldTouchPoints[0] + oldTouchPoints[1] - vector) * this.mainCamera.orthographicSize) / vector.y);

                    _scrollOffset += vector3 + (vector3.z * _scrollModification);
                    this.zoom = Mathf.Clamp01((this.zoom * oldTouchDistance) / magnitude);

                    vector3 = this.transform.TransformDirection((((touchPoints[0] + touchPoints[1]) - vector) * this.mainCamera.orthographicSize) / vector.y);
                    _scrollOffset -= vector3 + (vector3.z * _scrollModification);

                    oldTouchPoints[0] = touchPoints[0];
                    oldTouchPoints[1] = touchPoints[1];
                    oldTouchDistance = magnitude;
                }
                yield return null;
            }
            this.ClampZoom();
            yield return null;
            SwitchState(OpState.kControl);
        }

        #endregion

        #region Follow Method

        public void Follow(Transform target)
        {
            Unblock();
            StopAllCoroutines();
            if (this._showing)
            {
                _scrollOffset = (this.transform.position - _cameraOffset) - this.shakeOffset;
            }
            StartCoroutine(this.FollowRoutine(target));
        }

        public void StopFollow()
        {
            Unblock();
            base.StopAllCoroutines();
            //OnStopFollow.Execute();
            this._showing = false;
            this.OnEnable();
        }

        private IEnumerator FollowRoutine(Transform target)
        {
            Debug.Log("FollowRoutine");
            if (target != null)
            {
                Block(GetRestrictions() | Restrictions.Move);
                var curveProgress = 0f;
                this._showing = true;
                yield return null;
                var transTarget = target.transform;
                var transSelf = this.transform;
                var reached = false;

                while (transTarget != null)
                {
                    var progressCondition = (curveProgress >= 1f) || (((transTarget.position + this._cameraOffset) - transSelf.position).sqrMagnitude < 0.25f);
                    if (progressCondition && !reached)
                    {
                        reached = true;
                        Block(GetRestrictions() & ~Restrictions.Move);
                        this.OnEnable();
                        if (OnCameraReach != null)
                        {
                            OnCameraReach();
                        }
                    }

                    //Debug.Log("progressCondition");

                    if ((!progressCondition || !Input.GetMouseButton(0)) || IsRestriction(Restrictions.Map))
                    {
                        yield return new WaitForEndOfFrame();
                        if (transTarget != null)
                        {
                            curveProgress = Mathf.Min(1f, (float)(curveProgress + (Time.deltaTime / this._showDuration)));
                            var tPos = transTarget.position;
                            tPos.z = 0;
                            this.transform.position = Vector3.Lerp(this.transform.position, tPos + this._cameraOffset, curveProgress) + this.shakeOffset;
                            continue;
                        }
                    }
                    break;
                }

                this._scrollOffset = (this.transform.position - this._cameraOffset) - this.shakeOffset;
                //GameCamera.OnStopFollow.Execute();
                this._showing = false;
            }
        }

        #endregion

        #region Shake

        private Vector3 prevShake;
        private Vector3 shakeOffset;

        private bool shakeFadeDone = true;
        private static float shakeEndTime;

        public void Shake()
        {
            StopCoroutine("ShakeRoutine");
            StartCoroutine("ShakeRoutine");
        }

        private void InterruptShakeRoutines()
        {
            StopCoroutine("CameraShakeFadeAway");
            StopCoroutine("CameraShake");
            this.shakeFadeDone = true;
        }

        private IEnumerator ShakeRoutine()
        {
            var shakeEndTime = Time.time + 0.33f;

            while (Time.time < shakeEndTime)
            {
                this.shakeOffset = (Vector3)(UnityEngine.Random.insideUnitCircle.normalized * 0.25f);
                yield return new WaitForFixedUpdate();
                yield return new WaitForFixedUpdate();
                yield return new WaitForFixedUpdate();
            }

            yield return new WaitForFixedUpdate();
            this.shakeOffset = Vector3.zero;
            yield return null;
        }

        private void ApplyCurrentShake()
        {
            if (this.prevShake != Vector3.zero)
            {
                Transform transform = base.transform;
                transform.position -= this.prevShake;
                this.prevShake = Vector3.zero;
            }
            if (this.shakeOffset != Vector3.zero)
            {
                Transform transform2 = base.transform;
                transform2.position += this.shakeOffset;
                this.prevShake = this.shakeOffset;
            }
        }

        private IEnumerator CameraShake(float force)
        {
            while (Time.time < GameCamera.shakeEndTime)
            {
                this.shakeOffset = Vector3.Lerp(this.shakeOffset, new Vector3((Mathf.PerlinNoise(0f, Time.time * force) * 0.1f) - 0.05f, 0f, (Mathf.PerlinNoise(Time.time * force, Time.time * force) * 0.1f) - 0.05f), Time.deltaTime * force);

                this.ApplyCurrentShake();
                yield return null;
            }

            if (this.shakeFadeDone)
            {
                this.StartCoroutine(CameraShakeFadeAway());
            }
        }

        private IEnumerator CameraShakeFadeAway()
        {
            this.shakeFadeDone = false;
            while (this.shakeOffset.sqrMagnitude > 0.001f)
            {
                this.shakeOffset = Vector3.Lerp(this.shakeOffset, Vector3.zero, Time.deltaTime);
                this.ApplyCurrentShake();
                yield return null;
            }
            this.shakeOffset = Vector3.zero;
            this.shakeFadeDone = true;
        }

        #endregion

        #region Update

        private enum OpState
        {
            kNone = 0,
            kControl = 1,
            kScroll = 2,
            kFollow = 3,
            kShake = 4,
            kDrag = 5,
            kShow = 6,
            kZoom = 7,
            /// <summary>
            /// 刷地板
            /// </summary>
            kSurface = 8,
        }

        private OpState _opState;

        /// <summary>
        /// 鼠标是否被按下
        /// </summary>
        private bool _touched;
        /// <summary>
        /// 鼠标是否点击 城建元素
        /// </summary>
        private bool _draggable;
        private Transform _draggedObject;

        /// <summary>
        /// 地图位置
        /// </summary>
        private Vector3 _touchOrigin;
        /// <summary>
        /// 鼠标位置
        /// </summary>
        private Vector3 _touchPosition;
        /// <summary>
        /// 多点
        /// </summary>
        private int _touchFinger;
        /// <summary>
        /// 处理长按
        /// </summary>
        private Transform _touchObject;
        /// <summary>
        /// 长按事件触发时间
        /// </summary>
        private float _touchOriginalDuration = 20;
        private float _touchDuration;
        private float longPressDuration = 1f;

        private Vector3 _dragOffset;
        private Vector3 _dragTimedOffset;
        private Vector3 _lastDragPosition;
        private float _dragStartTime;

        /// <summary>
        /// 屏幕事件处理状态切换
        /// </summary>
        /// <param name="state"></param>
        private void SwitchState(OpState state)
        {
            //Debuger.Log("GameCamera.SwitchState: " + state);
            _opState = state;
            switch (state)
            {
                case OpState.kControl:
                    break;
                case OpState.kScroll:
                    _touchOrigin = GetGroundPlaneHit();
                    _touchPosition = Input.mousePosition;
                    _touchFinger = -1;
                    if (Input.touchCount > 0)
                    {
                        _touchFinger = Input.GetTouch(0).fingerId;
                    }

                    _scrollDragFailed = false;
                    _chargingClick = false;

                    //var scrollVelocity = Vector3.zero;

                    if (Input.GetMouseButton(0))
                    {
                        if (IsSelfPlayer())
                        {
                            this.StartLongPress();
                            _touchDuration = 1;
                        }
                        this._chargingClick = true;
                    }

                    if (OnScrollBegin != null)
                    {
                        OnScrollBegin();
                    }

                    if (OnScroll != null)
                    {
                        OnScroll();
                    }
                    break;
                case OpState.kDrag:
                    if (_draggedObject)
                    {
                        if (OnDragObjectStart != null)
                        {
                            OnDragObjectStart(_draggedObject);
                        }

                        _lastDragPosition = MapHelper.AlignToGrid(_draggedObject.position);
                        _draggedObject.position = _lastDragPosition;

                        _dragOffset = _draggedObject.position - this.GetGroundPlaneHit();
                        _dragTimedOffset = Vector3.zero;// (GameCamera.timedOffset * 1f) - ((GameCamera.timedOffset * Vector3.Dot(mapSize, GameCamera.timedOffset)) * 0.5f);

                        _dragStartTime = Time.time;
                    }
                    break;
                case OpState.kZoom:
                    StartCoroutine(PitchZoomRoutine());
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delta"></param>
        private void UpdateState(float delta)
        {
            switch (_opState)
            {
                case OpState.kControl:
                    UpdateControl();
                    break;
                case OpState.kScroll:
                    UpdateScroll();
                    break;
                case OpState.kDrag:
                    UpdateDrag();
                    break;
                case OpState.kSurface:
                    UpdateSurface();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 鼠标状态 时时 检测  状态切换
        /// </summary>
        /// <param name="delta"></param>
        private void UpdateControl()
        {
            var touching = Input.GetMouseButton(0);
            if (!_touched && touching)
            {
                _draggable = !this.IsPointerOverUIObject();
            }
            _touched = touching;

#if DEBUG_MY
            if (IsPermission(Restrictions.Zoom))
            {
                var wheel = Input.GetAxis("Mouse ScrollWheel");
                if (wheel > 0.1f)
                {
                    this.Zoom(ZoomLevel.Far);
                }
                else if (wheel > 0.3f)
                {
                    this.Zoom(ZoomLevel.Limit);
                }
                else if (wheel < -0.1f)
                {
                    this.Zoom(ZoomLevel.Near);
                }
            }
#endif

            if (_touched && _draggable)  //_touched
            {
                if (IsPermission(Restrictions.Click))
                {
                    PreClick();
                }

                if (IsPermission(Restrictions.Drag) && TryGetDragObject())
                {
                    SwitchState(OpState.kDrag);
                    return;
                }

                if (_opState != OpState.kSurface)
                {
                    SwitchState(OpState.kScroll);
                }
            }
        }

        /// <summary>
        /// 主要处理场景拖拽事件 缩放和鼠标按下事件
        /// </summary>
        /// <param name="delta"></param>
        private void UpdateScroll()
        {
            //多点缩放
            // Debug.Log("滑动场景");
            if (Input.touchCount > 1 && IsPermission(Restrictions.Zoom))
            {
                this.BreakLongPress();
                SwitchState(OpState.kZoom);
                return;
            }
            if (Input.GetMouseButton(0))        //鼠标一直按住时
            {
                if (_touchObject)
                {
                    if (IsPermission(Restrictions.LongPress))
                    {
                        var longPressDuration = 0.6f;
                        _touchDuration = Mathf.Clamp(_touchDuration - Time.deltaTime, 0f, longPressDuration);
                        var progress = (longPressDuration - _touchDuration) / longPressDuration;
                        ProgressLongPress(progress);
                        if (progress > Mathf.Max((0.2f * Time.smoothDeltaTime * 60f), 0.2f))
                        {
                            _chargingClick = false;
                        }

                        if (_touchDuration <= 0f)
                        {
                            _touchObject.SendMessage("OnLongPress", SendMessageOptions.DontRequireReceiver);    //通知所用挂载MapObject组件的物体，调用OnLongPress方法
                            SwitchState(OpState.kControl);
                            return;
                        }
                    }
                }

                if (_chargingClick)
                {
                    var vector = Input.mousePosition - _touchPosition;
                    if (vector.sqrMagnitude > Screen.dpi)
                    {
                        this.BreakLongPress();
                        _chargingClick = false;
                    }
                }

                if (IsPermission(Restrictions.Move))
                {
                    var currPos = GetGroundPlaneHit();
                    var movePos = _touchOrigin - currPos;

                    Vector3 offset = _scrollOffset + movePos + (_scrollModification * movePos.y);
                    //scrollVelocity = Vector3.ClampMagnitude(Vector3.Lerp(scrollVelocity, ((offset - this._scrollOffset) / Time.deltaTime), Time.deltaTime * 30f), 150f);
                    _scrollOffset = offset;
                }

                if ((Input.touchCount > 0) && (_touchFinger != Input.GetTouch(0).fingerId))
                {
                    this.BreakLongPress();
                }
            }
            else                    //鼠标点击事件
            {
                this.BreakLongPress();
                if (_chargingClick)
                {
                    this.Click();
                }

                SwitchState(OpState.kControl);
            }
        }

        /// <summary>
        /// 处理 城建元素拖拽事件
        /// </summary>
        /// <param name="delta"></param>
        private void UpdateDrag()
        {
            if (_draggedObject && Input.GetMouseButton(0))  //拖拽过程
            {
                var hitPoint = this.GetGroundPlaneHit();
                var offset = _dragOffset + (_dragTimedOffset * Math.Min(Time.time - _dragStartTime, 0.15f));

                var dragPosition = MapHelper.AlignToGrid(hitPoint + offset);

                if ((dragPosition - _lastDragPosition).sqrMagnitude > 0.0001f)
                {
                    _lastDragPosition = dragPosition;
                    _draggedObject.position = dragPosition;

                    if (OnDragObjectProgress != null)
                    {
                        OnDragObjectProgress();
                    }
                }

                var sClamp = (_touchOrigin - hitPoint).magnitude * 0.5f;
                var scrollSpeed = Mathf.Clamp(sClamp, 5f, 10f);

                var vPoint = this.mouseViewportPoint;
                if (vPoint.y > 0.9f)
                {
                    _scrollOffset += this.transform.up * (Time.deltaTime * scrollSpeed);// ((new Vector3(this.transform.up.x, this.transform.up.y, 0f) * Time.deltaTime) * scrollSpeed);
                }
                if (vPoint.y < 0.1f)
                {
                    _scrollOffset -= this.transform.up * (Time.deltaTime * scrollSpeed); //((new Vector3(this.transform.up.x, this.transform.up.y, 0f) * Time.deltaTime) * scrollSpeed);
                }
                if (vPoint.x > 0.9f)
                {
                    _scrollOffset += this.transform.right * (Time.deltaTime * scrollSpeed);
                }
                if (vPoint.x < 0.1f)
                {
                    _scrollOffset -= this.transform.right * (Time.deltaTime * scrollSpeed);
                }
            }
            else                //拖拽完成
            {
                if (OnDragObjectFinish != null)
                {
                    Debuger.Log("OnItemDragFinish");
                    OnDragObjectFinish();
                }
                SwitchState(OpState.kControl);
            }
        }

        /// <summary>
        /// 处理刷地板
        /// </summary>
        private void UpdateSurface()
        {
            if (Input.GetMouseButton(0))
            {
                var hitPoint = this.GetGroundPlaneHit();
                var dragPosition = MapHelper.AlignToGrid(hitPoint);
                Int2 pos = MapHelper.PositionToGrid(dragPosition);
                if (OnSurfaceDragProgress != null)
                {
                    OnSurfaceDragProgress(pos);
                }

                if (!this.IsPointerOverUIObject())
                {
                    var sClamp = (_touchOrigin - hitPoint).magnitude * 0.5f;
                    var scrollSpeed = Mathf.Clamp(sClamp, 5f, 10f);

                    var vPoint = this.mouseViewportPoint;
                    if (vPoint.y > 0.9f)
                    {
                        _scrollOffset += this.transform.up * (Time.deltaTime * scrollSpeed);// ((new Vector3(this.transform.up.x, this.transform.up.y, 0f) * Time.deltaTime) * scrollSpeed);
                    }
                    if (vPoint.y < 0.1f)
                    {
                        _scrollOffset -= this.transform.up * (Time.deltaTime * scrollSpeed); //((new Vector3(this.transform.up.x, this.transform.up.y, 0f) * Time.deltaTime) * scrollSpeed);
                    }
                    if (vPoint.x > 0.9f)
                    {
                        _scrollOffset += this.transform.right * (Time.deltaTime * scrollSpeed);
                    }
                    if (vPoint.x < 0.1f)
                    {
                        _scrollOffset -= this.transform.right * (Time.deltaTime * scrollSpeed);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateShow()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateZoom()
        {

        }

        #endregion

        #region Unity

        private void Awake()
        {
            Instance = this;

            if (KQuality.Instance.deviceLevel < 2)
            {
                GetComponent<UnityEngine.Rendering.PostProcessing.PostProcessLayer>().enabled = false;
            }

            this.mainCamera = GetComponent<Camera>();

            //float halfFOV = Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad;
            //float rh = 1080f * 0.005f;
            //var dis = rh / Mathf.Tan(halfFOV);

            _cameraOffset = (-100 * this.transform.forward);
            _scrollOffset = transform.localPosition;
            _scrollOffset.z = 0f;

            //float x = Mathf.Cos(this.transform.localEulerAngles.y * Mathf.Deg2Rad) / Mathf.Tan(this.transform.localEulerAngles.x * Mathf.Deg2Rad);
            //this._scrollModification = new Vector3(x, -1f, x);

            _defaultZoomValue = this.GetZoomValue(this.defaultZoomLevel);
            var zoomList = new List<float>(this.zoomLevels.Length);
            for (int i = 0; i < this.zoomLevels.Length; i++)
            {
                var zoomLevel = this.zoomLevels[i];
                if (_minZoomValue > zoomLevel.value)
                {
                    _minZoomValue = zoomLevel.value;
                }
                if (_maxZoomValue < zoomLevel.value)
                {
                    _maxZoomValue = zoomLevel.value;
                }
                if (zoomLevel.snapping)
                {
                    zoomList.Add(zoomLevel.value);
                }
            }

            zoomList.Sort((l, r) => ((l - r) <= 0f) ? -1 : 1);
            _snapZooms = zoomList.ToArray();

            this.zoom = _defaultZoomValue;

            //Map.onGridUpdate = (Map.GridUpdateEventHandler)Delegate.Combine(Map.onGridUpdate, new Map.GridUpdateEventHandler(this.RefreshCameraConstraints));
            Screen.sleepTimeout = -1;
        }

        private void OnEnable()
        {
            SwitchState(OpState.kControl);
        }

        private void Start()
        {
            eventData = new PointerEventData(EventSystem.current);
            results = new List<RaycastResult>();
            //OnClickUGUI += () =>
            //{
            //    if (!IsConfirmBt(CurrentUICommponent))
            //    {
            //        if (Map.DragCancelCallBack != null)
            //            Map.DragCancelCallBack();



            //    }
            //    if (!IsFunctionBt(CurrentUICommponent))
            //    {
            //        //Debug.Log("关闭");
            //        KUIWindow.CloseWindow<UI.FunctionWindow>();
            //    }
            //};

            GameObject cameraFx;
            if (KAssetManager.Instance.TryGetBuildingPrefab("Fx/wave_highlight", out cameraFx))
            {
                TransformUtils.Instantiate(cameraFx, this.transform, true, false);
            }

        }

        private void Update()
        {
            UpdateState(Time.deltaTime);
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log(ExecuteEvents.GetEventHandler<IEventSystemHandler>(g));
                eventData.position = Input.mousePosition;
                results.Clear();
                if (true)
                {
                    if (EventSystem.current != null)
                    {
                        EventSystem.current.RaycastAll(eventData, results);
                        if (results.Count > 0)
                        {
                            CurrentUICommponent = results[0].gameObject;
                            if (OnClickUGUI != null)
                                OnClickUGUI();
                        }
                    }
                }
                //else
                //    Debug.Log("非城建区域" + CurrentUICommponent);
            }
        }

        private void LateUpdate()
        {
            //if (!this.show)
            {
                this.ClampScroll();
                this.transform.position = (this._cameraOffset + this._scrollOffset) /*+ this.shakeOffset*/;
            }
        }

        //private void OnGUI()
        //{
        //    if (GUILayout.Button("Block", GUILayout.MinWidth(100), GUILayout.MinHeight(100)))
        //    {
        //        Block(Map.Instance.gameObject, Restrictions.Map);
        //    }

        //    if (GUILayout.Button("Unblock", GUILayout.MinWidth(100), GUILayout.MinHeight(100)))
        //    {
        //        Unblock(Map.Instance.gameObject);
        //    }

        //    if (GUILayout.Button("BlockAll", GUILayout.MinWidth(100), GUILayout.MinHeight(100)))
        //    {
        //        Block(Map.Instance.gameObject, Restrictions.All);
        //    }

        //    if (GUILayout.Button("Allow", GUILayout.MinWidth(100), GUILayout.MinHeight(100)))
        //    {
        //        var self = GameObject.Find("OneSelfBuilding");
        //        var list = new List<GameObject>();
        //        for (int i = 0; i < self.transform.childCount; i++)
        //        {
        //            list.Add(self.transform.GetChild(i).gameObject);
        //        }

        //        Allow(this.gameObject, Restrictions.Click, list);
        //    }
        //}

        /// <summary>
        /// 是否是操作菜单中的按钮
        /// </summary>
        /// <param name="gm"></param>
        /// <returns></returns>
        private bool IsConfirmBt(GameObject gm)
        {
            for (int i = 0; i < BubbleConfirm.ConfirmBtLst.Length; i++)
            {
                if (BubbleConfirm.ConfirmBtLst[i] == gm)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 是否是功能按钮
        /// </summary>
        /// <param name="gm"></param>
        /// <returns></returns>
        private bool IsFunctionBt(GameObject gm)
        {
            if (FunctionWindow.CurrFunctionBtLst != null)
            {

                for (int i = 0; i < FunctionWindow.CurrFunctionBtLst.Length; i++)
                {
                    // Debug.Log("按钮" + FunctionWindow.FunctionBtLst[i].name + "/" + gm.name);
                    if (FunctionWindow.CurrFunctionBtLst[i] == gm)
                        return true;

                }
            }
            return false;
        }

        #endregion 

        #region Restrictions And Permissions

        /// <summary>
        /// 约束
        /// </summary>
        [Flags]
        public enum Restrictions : byte
        {
            None = 0,
            Click = 1,
            LongPress = 2,
            Drag = 4,
            Move = 8,
            Zoom = 16,
            Map = 127,
            UI = 128,
            All = 255,
        }

        /// <summary>
        /// 许可
        /// </summary>
        private class Permissions
        {
            public Restrictions permission;
            public IEnumerable<GameObject> allowedObjects;
            public IEnumerable<GameObject> allowedUIObjects;

            public Permissions(Restrictions perm, IEnumerable<GameObject> allowedObjects, IEnumerable<GameObject> allowedUI)
            {
                this.permission = perm;
                this.allowedObjects = allowedObjects;
                this.allowedUIObjects = allowedUI;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static Restrictions _CurrentRestrictions;
        /// <summary>
        /// 
        /// </summary>
        private static readonly Dictionary<GameObject, Restrictions> _MapRestrictions = new Dictionary<GameObject, Restrictions>();
        /// <summary>
        /// 
        /// </summary>
        private static readonly Dictionary<GameObject, Permissions> _MapPermissions = new Dictionary<GameObject, Permissions>();
        /// <summary>
        /// 
        /// </summary>
        private static bool _UIBlocked = false;
        /// <summary>
        /// 
        /// </summary>
        private static readonly List<GameObject> _AllowedObjects = new List<GameObject>();
        /// <summary>
        /// 
        /// </summary>
        private static readonly List<GameObject> _AllowedUIObjects = new List<GameObject>();

        /// <summary>
        /// 是否限制
        /// </summary>
        /// <param name="restrictions"></param>
        /// <returns></returns>
        public static bool IsRestriction(Restrictions restrictions)
        {
            return (_CurrentRestrictions & restrictions) != Restrictions.None;
        }
        /// <summary>
        /// 是否许可
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public static bool IsPermission(Restrictions permissions)
        {
            return (_CurrentRestrictions & permissions) == Restrictions.None;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static Restrictions GetRestrictions()
        {
            var restrictions = Restrictions.None;
            _MapRestrictions.TryGetValue(Instance.gameObject, out restrictions);
            return restrictions;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="caller"></param>
        /// <returns></returns>
        public static Restrictions GetRestrictions(GameObject caller)
        {
            var restrictions = Restrictions.None;
            _MapRestrictions.TryGetValue(caller, out restrictions);
            return restrictions;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rectrictions"></param>
        /// <param name="forced"></param>
        private static void Block(Restrictions rectrictions, bool forced = false)
        {
            if (forced)
            {
                _MapRestrictions.Clear();
            }
            Block(Instance.gameObject, rectrictions);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="rectrictions"></param>
        public static void Block(GameObject caller, Restrictions rectrictions)
        {
            if (rectrictions != Restrictions.None)
            {
                _MapRestrictions[caller] = rectrictions;
            }
            else
            {
                _MapRestrictions.Remove(caller);
            }
            UpdateRestrictions();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="forced"></param>
        private static void Unblock(bool forced = false)
        {
            if (forced)
            {
                _MapRestrictions.Clear();
            }
            Unblock(Instance.gameObject);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="caller"></param>
        public static void Unblock(GameObject caller)
        {
            _MapRestrictions.Remove(caller);
            UpdateRestrictions();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="permissions"></param>
        /// <param name="allowedObjects"></param>
        /// <param name="allowedUI"></param>
        public static void Allow(GameObject caller, Restrictions permissions, IEnumerable<GameObject> allowedObjects = null, IEnumerable<GameObject> allowedUI = null)
        {
            _MapPermissions[caller] = new Permissions(permissions, allowedObjects, allowedUI);
            UpdateRestrictions();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="caller"></param>
        public static void Disallow(GameObject caller)
        {
            _MapPermissions.Remove(caller);
            UpdateRestrictions();
        }
        /// <summary>
        /// 
        /// </summary>
        public static void UpdateRestrictions()
        {
            bool flag = false;

            _CurrentRestrictions = Restrictions.None;
            foreach (var pair in _MapRestrictions)
            {
                if (pair.Key)
                {
                    _CurrentRestrictions |= pair.Value;
                }
                else
                {
                    flag = true;
                }
            }

            _AllowedObjects.Clear();
            _AllowedUIObjects.Clear();
            foreach (var pair in _MapPermissions)
            {
                if (pair.Key)
                {
                    //_CurrentRestrictions = ~((~_CurrentRestrictions) | pair.Value.permission);

                    if (pair.Value.allowedObjects != null)
                    {
                        _AllowedObjects.AddRange(pair.Value.allowedObjects);
                    }
                    if (pair.Value.allowedUIObjects != null)
                    {
                        _AllowedUIObjects.AddRange(pair.Value.allowedUIObjects);
                    }
                }
                else
                {
                    flag = true;
                }
            }

            if (flag)
            {
                Debug.LogWarning("Permission owner destroyed!");

                var permissions = new List<KeyValuePair<GameObject, Permissions>>(_MapPermissions);
                for (int i = permissions.Count - 1; i >= 0; i--)
                {
                    var permission = permissions[i];
                    if ((permission.Key == null) && !object.ReferenceEquals(permission.Key, null))
                    {
                        _MapPermissions.Remove(permission.Key);
                    }
                }

                var restrictions = new List<KeyValuePair<GameObject, Restrictions>>(_MapRestrictions);
                for (int j = restrictions.Count - 1; j >= 0; j--)
                {
                    var restriction = restrictions[j];
                    if ((restriction.Key == null) && !object.ReferenceEquals(restriction.Key, null))
                    {
                        _MapPermissions.Remove(restriction.Key);
                    }
                }
            }

            bool blockUI = (_CurrentRestrictions & Restrictions.UI) > Restrictions.None;
            if (blockUI != _UIBlocked)
            {
                _UIBlocked = blockUI;
                if (_UIBlocked)
                {
                    GraphicRaycasterEx.OnRaycast += BlockUI;
                }
                else
                {
                    GraphicRaycasterEx.OnRaycast -= BlockUI;
                }
            }
            //OnUpdateRestrictions.Execute();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="raycastResults"></param>
        private static void BlockUI(PointerEventData data, List<RaycastResult> raycastResults)
        {
            if (_AllowedUIObjects.Count == 0)
            {
                raycastResults.Clear();
            }
            else
            {
                for (int i = raycastResults.Count - 1; i >= 0; i--)
                {
                    var result = raycastResults[i];
                    if (CheckUI(result.gameObject))
                    {
                        raycastResults.RemoveAt(i);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObj"></param>
        /// <returns></returns>
        public static bool CheckUI(GameObject gameObj)
        {
            if (!gameObj)
            {
                return true;
            }

            foreach (var allow in _AllowedUIObjects)
            {
                if (allow)
                {
                    if ((allow == gameObj) || gameObj.transform.IsChildOf(allow.transform))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        #endregion
    }
}
