// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "Map" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using Game.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Build
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Map : MonoBehaviour
    {
        #region Delegate

        public delegate void DragCancelDelegate(ref bool isAllowed, bool failedToAccept);
        public delegate void DragConfirmDelegate(ref bool isAllowed);
        public delegate int DargFinishDelegate(ref bool showConfirm);

        #endregion

        #region Static

        public static DragConfirmDelegate BeforeDragConfirm;
        public static DragCancelDelegate BeforeDragCancel;
        public static DargFinishDelegate BeforeDragFinish;

        /// <summary>
        /// 创建城建元素  取消
        /// </summary>
        public static System.Action DragCancelCallBack;

        #endregion

        #region Field 

        /// <summary>
        /// 城建元素取消事件
        /// </summary>
        private Action _onDragCancel;
        /// <summary>
        /// 城建元素确认事件
        /// </summary>
        private Action _onDragConfirm;
        /// <summary>
        /// 城建元素旋转事件
        /// </summary>
        private Action _onRotate;
        /// <summary>
        /// 城建元素出售事件
        /// </summary>
        private Action _onSell;

        /// <summary>
        /// 
        /// </summary>
        private float _dragModeTimer;

        /// <summary>
        /// 是否在拖拽过程中
        /// </summary>
        private bool _dragInProgress;

        /// <summary>
        /// 
        /// </summary>
        private Vector3 _objectOrigin;

        /// <summary>
        /// 标记城建元素是否可以安放 ,true 可以安放
        /// </summary>
        private bool _lastAllowed;

        /// <summary>
        /// 
        /// </summary>
        private Vector3 _lastAllowedPostion;

        /// <summary>
        /// 地图网格所有节点
        /// </summary>
        private Dictionary<int, MapNode> _mapNodes = new Dictionary<int, MapNode>(12800);

        #endregion

        #region Property

        /// <summary>
        /// 
        /// </summary>
        public Plane MapPlane { get; private set; }

        public bool LastAllowed
        {
            get
            {
                return _lastAllowed;
            }
        }

        /// <summary>
        /// 控制模式
        /// </summary>
        public ControlMode CurControlMode
        {
            get;
            private set;
        }

        /// <summary>
        /// 是否是拖动模式
        /// </summary>
        public bool IsDragMode
        {
            get { return CurControlMode == ControlMode.kDrag; }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //public Transform dragObject
        //{
        //    get
        //    {
        //        return this.CurControlMode != ControlMode.kDrag ? null : this.currMapObject.transform;
        //    }
        //}

        /// <summary>
        /// 当前 select map object
        /// </summary>
        public MapObject CurrMapObject
        {
            get;
            private set;
        }

        /* 无引用 */
        ///// <summary>
        ///// 
        ///// </summary>
        //public MapObject lastMapObject
        //{
        //    get;
        //    private set;
        //}

        /// <summary>
        /// 是否在城建状态
        /// </summary>
        public bool IsBuildingMoveState
        {
            get; set;
        }

        /// <summary>
        /// 当前拖拽元素 数据
        /// </summary>
        public Building CurBuildingData
        {
            get
            {
                if (CurrMapObject != null)
                {
                    return CurrMapObject.GetComponent<Building>();// BuildingManager.GetEntitie(currMapObject.gameObject);
                }
                return null;
            }
        }

        private readonly float showHideLongPressDuration = 0.2f;

        #endregion

        #region Methods 

        /// <summary>
        /// 每次拖拽完成后 初始化 状态，和注册元素中MapObject.OnMapObjectLongPress长按事件 。
        /// </summary>
        public void NormalMode()
        {
            Debuger.Log("NormalMode");
            this.CurControlMode = ControlMode.kNormal;

            this.DragAccept();

            MapObject.OnMapObjectSelect = mapItem =>
            {
                Debuger.Log(">>> OnMapObjectSelect: " + mapItem.name);
                return true;
            };

            MapObject.OnMapObjectLongPress = mapItem =>
            {
                Debuger.Log(">>> OnMapObjectLongPress: " + mapItem.name);

                BeforeDragFinish = (ref bool showConfirm) =>
                {
                    showConfirm = true;
                    return 1;
                };
                KUIWindow.CloseWindow<BuildingShopWindow>();

                this.DragMode(null, null);
                this.SelectObject(mapItem);
                return true;
            };

            this.SetupBuildingLongPress();

            GameCamera.OnScroll = () =>
            {
                //SelectionHighlight.Instance.Hide();
                //RoadHighlight.Instance.Hide();
            };

            //currMapObject = null;
        }

        /// <summary>
        /// 城建元素创建完成  ，长按事件注册
        /// </summary>
        /// <param name="dragConfirm">城建元素确认事件</param>
        /// <param name="dragCancel">城建元素取消事件</param>
        public void DragMode(Action dragConfirm = null, Action dragCancel = null, Action onRotate = null, Action onSell = null)
        {
            Debuger.Log("DragMode");
            this.CurControlMode = ControlMode.kDrag;

            _dragModeTimer = Time.time;
            _onDragConfirm = dragConfirm;
            _onDragCancel = dragCancel;
            _onRotate = onRotate;
            _onSell = onSell;

            MapObject.OnMapObjectSelect = mapItem =>
            {
                this.SelectObject(mapItem);
                return false;
            };
            MapObject.OnMapObjectLongPress = mapItem =>
            {
                KUIWindow.CloseWindow<BuildingShopWindow>();
                return true;
            };

            GameCamera.OnLongPressStart = null;
            GameCamera.OnLongPressProgress = null;
            GameCamera.OnLongPressBreak = null;
        }

        /// <summary>
        /// 编辑模式
        /// </summary>
        public void EditorMode()
        {
            this.CurControlMode = ControlMode.kEditor;
        }

        public void CurBuildingClickMove(Vector3 screenPos)
        {
            BuildingManager.Instance.CurBuildingMove(screenPos);
            OnDragObjectProgressHandler();
            ShowBubble(2, _lastAllowed);
        }

        public void CurBuildingClickMove(Int2 pos)
        {
            if (this.CurrMapObject)
            {
                BuildingManager.Instance.CurBuildingMove(pos);
                OnDragObjectProgressHandler();
                ShowBubble(2, _lastAllowed);
            }
        }

        /// <summary>
        /// 城建物体被选时
        /// </summary>
        /// <param name="target"></param>
        public void SelectObject(MapObject target)
        {
            if (this.CurrMapObject != null)
            {
                this.DeselectObject(null, true);
            }

            if (this.OnObjectSelectEvent != null)
            {
                this.OnObjectSelectEvent(target);
            }

            if (target.draggable)
            {
                GameCamera.OnDragObjectStart += this.OnDragObjectStartHandler;
                GameCamera.OnDragObjectProgress += this.OnDragObjectProgressHandler;
                GameCamera.OnDragObjectFinish += this.OnDragObjectFinishHandler;
                GameCamera.Instance.onTryObjectDrag += this.OnTryObjectDrag;
                GameCamera.OnClick += this.DeselectObjectHandler;
            }

            this.CurrMapObject = target;
            Debuger.Log(">>> 当前点击的城建元素----- " + CurrMapObject.name);
        }

        /// <summary>
        /// 城建元素失去焦点事件
        /// </summary>
        /// <param name="target"></param>
        public void DeselectObjectHandler(Transform target)
        {
            DeselectObject(target ? target.GetComponent<MapObject>() : null, false);
        }

        /// <summary>
        /// 判断 城建元素失去焦点 
        /// </summary>
        /// <param name="mapObject"></param>
        /// <param name="forced"></param>
        public void DeselectObject(MapObject mapObject, bool forced)
        {
            if ((this.CurrMapObject != null) &&
                ((mapObject == null) || (mapObject != this.CurrMapObject)) &&
                this.DragAccept(forced))            //城建 元素 失去焦点
            {
                LostFocus();
            }

            NormalMode();
        }

        /// <summary>
        /// 城建元素 失去焦点
        /// </summary>
        private void LostFocus()
        {
            Debuger.Log("城建元素失去焦点，清除事件，设置空 CurrMapObject:" + (CurrMapObject ? CurrMapObject.name:null));
            if (this.CurrMapObject != null)
            {
                this.CurrMapObject.Deselect();
            }
            if (this.OnObjectDeselectEvent != null)
            {
                this.OnObjectDeselectEvent(this.CurrMapObject);
            }

            IFunCommon iFunCommon = CurBuildingData as IFunCommon;
            if (iFunCommon != null)
                iFunCommon.OnRotateConfirm();

            GameCamera.OnDragObjectStart -= this.OnDragObjectStartHandler;
            GameCamera.OnDragObjectProgress -= this.OnDragObjectProgressHandler;
            GameCamera.OnDragObjectFinish -= this.OnDragObjectFinishHandler;

            GameCamera.Instance.onTryObjectDrag -= this.OnTryObjectDrag;
            GameCamera.OnClick -= this.DeselectObjectHandler;

            this.CurrMapObject = null;
        }

        /// <summary>
        /// 城建从商店里创建时 ，拖拽元素事件
        /// </summary>
        /// <param name="mapObject">城建地图</param>
        /// <param name="onDragConfirm">城建拖拽确认事件</param>
        /// <param name="onDragCancel">城建拖拽取消事件</param>
        public void StartDrag(MapObject mapObject, Action onDragConfirm = null, Action onDragCancel = null, Action onRotate = null, System.Action onSell = null)
        {
            if (mapObject != this.CurrMapObject)
            {
                this.DeselectObject(null, true);
            }
            Debug.Log("开始推拽");
            this.DragMode(onDragConfirm, onDragCancel, onRotate, onSell);
            this.SelectObject(mapObject);

            SnapToNavGrid(mapObject);
            this.OnTryObjectDrag(true);
            GameCamera.Instance.GrabMapObject(mapObject);
            mapObject.Select();
        }

        /// <summary>
        /// 获取当前拖拽物体
        /// </summary>
        /// <param name="noRaycast">获取鼠标点击的城建物体</param>
        /// <returns></returns>
        private Transform OnTryObjectDrag()
        {
            return this.OnTryObjectDrag(false);
        }

        /// <summary>
        /// 获取当前拖拽物体
        /// </summary>
        /// <param name="noRaycast">true  获取要创建的城建物体，false 获取鼠标点击的物体</param>
        /// <returns></returns>
        private Transform OnTryObjectDrag(bool noRaycast)
        {
            if ((this.CurrMapObject != null) && (this.CurControlMode == ControlMode.kDrag))
            {
                //if (!Player.Active.IsCurrent())
                //{
                //    return this.currentMapObject.transform;
                //}
                if (noRaycast)
                {
                    if (!_dragInProgress)
                    {
                        _objectOrigin = _lastAllowedPostion = this.CurrMapObject.transform.position;
                        _dragInProgress = true;
                    }
                    this.OnDragObjectProgressHandler();

                    return this.CurrMapObject.transform;
                }

                var hitObjs = GameCamera.Instance.GetScreenPointObjects();
                foreach (var hit in hitObjs)
                {
                    if (hit.transform == this.CurrMapObject.transform)
                    {
                        if (!_dragInProgress)
                        {
                            _objectOrigin = _lastAllowedPostion = this.CurrMapObject.transform.position;
                            _dragInProgress = true;
                        }
                        this.OnDragObjectProgressHandler();
                        return this.CurrMapObject.transform;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 开始拖拽事件
        /// </summary>
        /// <param name="target"></param>
        private void OnDragObjectStartHandler(Transform target)
        {
            HideBubble();
        }

        /// <summary>
        /// 城建元素拖拽过程中事件回调
        /// </summary>
        /// <returns></returns>
        private bool OnDragObjectProgressHandler()
        {
            Debuger.Log(">>> 城建元素拖拽过程中事件回调");

            CollisionHighlight.Instance.ShowCollisions(this.CurrMapObject, null, false);

            _lastAllowed = true;
            if (!NodeIsAvailable(this.CurrMapObject))
            {
                _lastAllowed = false;
            }

            if (_lastAllowed)
            {
                _lastAllowedPostion = this.CurrMapObject.transform.position;
                this.CurBuildingData.entityView.ColorRecovery();
            }

            if (!_lastAllowed)
            {
                this.CurBuildingData.entityView.ColorChangeSet();

                //SoaringText.instance.Init(Language.Get("DragNotAllowed", new string[0]));
                //SelectionHighlight.Instance.Hide();
            }
            else
            {
                ApplyNavArea(new Bounds()/*this.currMapItem.BoundingBoxCeiled*/, this.CurrMapObject, false, false);
                //SoaringText.instance.Hide();
            }
            return _lastAllowed;
        }

        /// <summary>
        /// 拖拽元素完成事件回调
        /// </summary>
        private void OnDragObjectFinishHandler()
        {
        }

        /// <summary>
        /// 一直绑订的----每次移动建筑完成后事件回调
        /// </summary>
        private void OnDragFinishHandler()
        {
            //Debug.Log("拖拽----完成");
            if ((this.CurControlMode == ControlMode.kDrag) && (this.CurrMapObject != null))
            {
                int type = 0;
                //bool flag = (this._dragModeTimer > 0f) && (Time.time - _dragModeTimer < 0.25f);
                bool flag = !_lastAllowed;
                if (BeforeDragFinish != null)
                {
                    type = BeforeDragFinish(ref flag);
                }

                //始终弹菜单
                //if (!flag)
                //{
                //    this.DeselectObject(null, false);
                //}
                //else
                //{
                    ShowBubble(type, _lastAllowed);
                //}

                _dragModeTimer = -1f;
            }
        }

        /// <summary>
        /// 城建元素开始创建  位置确认后 方法回调 ,(长按弹窗  确认按钮事件回调)
        /// </summary>
        /// <returns></returns>
        private bool DragAccept()
        {
            return DragAccept(false);
        }

        /// <summary>
        /// 城建元素开始创建  位置确认后 方法回调
        /// </summary>
        /// <param name="forced"></param>
        /// <returns></returns>
        private bool DragAccept(bool forced)
        {
            if (_dragInProgress && this.CurrMapObject)
            {
                Debug.Log("拖拽完成---");
                //if (!_lastAllowed)   //如果不可以安放
                //{
                //    Vector3 position = this.currMapObject.transform.position;
                //    _lastAllowed = FindFreeSpace(this.currMapObject, position, out position);
                //    if (_lastAllowed)
                //    {
                //        this.currMapObject.transform.position = position;
                //    }
                //}

                bool isAllowed = true;
                if (!_lastAllowed)   //当前区域不可以安放
                {
                    if (BeforeDragCancel != null)
                    {
                        BeforeDragCancel(ref isAllowed, true);
                    }
                    this.CurrMapObject.transform.position = _objectOrigin;// this.currMapObject.originalPos;

                    _objectOrigin = _lastAllowedPostion;

                    this.DragCancel(forced);

                    isAllowed = false;
                    if (!isAllowed && !forced)
                    {
                        //CollisionHighlight.Instance.ShowCollisions(this.currMapObject,null);
                        //BubbleManager.Instance.ShowConfirm(this.currMapObject.transform, null, this.DragCancel);
                        return false;
                    }

                    return true;
                }

                if (BeforeDragConfirm != null)
                {
                    BeforeDragConfirm(ref isAllowed);
                }

                if (isAllowed || forced)
                {
                    this.DragFinalize();
                    if (_onDragConfirm != null)
                    {
                        _onDragConfirm();
                    }
                    this._onDragConfirm = null;
                    this._onDragCancel = null;
                }
            }

            return true;
        }

        private void onRotate()
        {

        }

        private void onSell()
        {
            // en
        }

        /// <summary>
        /// 取消城建元素建设（销毁城建元素）
        /// </summary>
        private void DragCancel()
        {
            this.DragCancel(false);
        }

        /// <summary>
        /// 取消城建元素建设（销毁城建元素）
        /// </summary>
        /// <param name="forced"></param>
        private void DragCancel(bool forced)
        {
            Instance.IsBuildingMoveState = false;
            HideBubble(null);
            if (_dragInProgress)
            {
                bool isAllowed = true;
                if (BeforeDragCancel != null)
                {
                    BeforeDragCancel(ref isAllowed, false);
                }

                if (isAllowed || forced)
                {

                    if (this.CurrMapObject != null)
                    {
                        this.CurBuildingData.entityView.ColorRecovery();
                        this.CurrMapObject.transform.position = this._objectOrigin;
                    }
                    this.DragFinalize();
                    if (_onDragCancel != null)
                    {
                        this._onDragCancel();
                    }
                    this._onDragConfirm = null;
                    this._onDragCancel = null;
                }
            }
        }

        /// <summary>
        /// 城建元素位置改变完成后 确认按钮 事件回调
        /// </summary>
        private void DragFinalize()
        {
            HideBubble(null);
            Debug.Log("拖拽完成");
            if (CollisionHighlight.Instance != null)
            {
                CollisionHighlight.Instance.HideCollisions();
            }
            //SoaringText.Instance.Hide();
            if (this.CurrMapObject != null)
            {
                //this.HideBubble();
                if (this._dragInProgress)
                {
                    if (CurrMapObject.CurrBuildingData._buildingData.type != (int)Building.Category.kSurface)
                    {
                        this.ApplyTownUnitsToGrid(this.CurrMapObject);
                    }
                }
                this._dragInProgress = false;
                if (this.CurControlMode == ControlMode.kDrag)
                {
                    this.NormalMode();
                    if (Instance.IsBuildingMoveState && CurBuildingData.isMove)
                    {
                        if (CurBuildingData.buildingId > 0)
                        {
                            Debug.Log("发送移动数据");
                            BuildingManager.Instance.BuildingMoveRequest(this.CurrMapObject, BuildingMoveHandler);
                        }
                        else
                        {
                            BuildingManager.Instance.BuildingSetRequest();
                        }
                        Instance.IsBuildingMoveState = false;
                    }
                    this.DeselectObject(null, false);
                }
            }
            if (BuildingManager.CurrBuilding != null)
            {
                BuildingManager.CurrBuilding.ShowBuild();
                //BuildingManager.CurrBuilding = null;
            }
            //currMapObject = null;
        }

        /// <summary>
        /// 建筑移动结果回调
        /// </summary>
        /// <param name="id"></param>
        /// <param name="content"></param>
        /// <param name="data"></param>
        private void BuildingMoveHandler(int id, string content, object data)
        {
            if (id == 0)
            {
                Debug.Log("位置设置成功");
            }
            else
            {
                //currMapObject.transform.position = currMapObject.OriginalPos;
                DragAccept();
                Debug.Log("位置设置失败");
            }
            Instance.IsBuildingMoveState = false;
        }

        #endregion

        #region Enum

        /// <summary>
        /// 控制模式
        /// </summary>
        public enum ControlMode
        {
            kNormal,
            /// <summary>
            /// 拖动模式
            /// </summary>
            kDrag,
            /// <summary>
            /// 引导模式
            /// </summary>
            kTutorial,
            /// <summary>
            /// 编辑模式
            /// </summary>
            kEditor,
        }

        #endregion

        #region Static 

        private static GameObject _MainMap;
        /// <summary>
        /// 
        /// </summary>
        public static Map Instance;

        /// <summary>
        /// 对齐到网格
        /// </summary>
        /// <param name="mapObject"></param>
        /// <returns></returns>
        public static Vector3 SnapToNavGrid(MapObject mapObject)
        {
            if (mapObject != null)
            {
                Vector3 vector = MapHelper.AlignToGrid(mapObject.transform.position);
                mapObject.transform.position = vector;
                return vector;
            }
            return Vector3.zero;
        }

        /// <summary>
        /// 判断城建元素拖拽 碰撞底版 颜色
        /// </summary>
        /// <param name="item"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static bool NodeIsAvailable(KItemBuilding item, Int2 coordinate)
        {
            MapNode node;
            Map.Instance._mapNodes.TryGetValue(coordinate.GetUnit(), out node);
            if (node == null)
            {
                return false;
            }

            if (node.ownerArea != null)
            {
                if (!node.ownerArea.unlocked)
                {
                    return false;
                }

                var itemGeography = (AreaGeography)item.geography;
                if ((node.ownerArea.geography & itemGeography) == 0)      //判断网格节点是否与当前城建元素属性相同(陆地--水面)
                {
                    return false;
                }
            }

            if (node.ownerItem != null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 判断城建元素拖拽 碰撞底版 颜色
        /// </summary>
        /// <param name="item"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static bool NodeIsAvailable(MapObject item, MapNode node)
        {
            if (node == null)
            {
                return false;
            }

            if (node.ownerArea != null)
            {
                if (!node.ownerArea.unlocked)
                {
                    return false;
                }

                var itemGeography = (AreaGeography)item.CurrBuildingData.entityData.geography;
                if ((node.ownerArea.geography & itemGeography) == 0)      //判断网格节点是否与当前城建元素属性相同(陆地--水面)
                {
                    return false;
                }
            }

            if (node.ownerItem != null && node.ownerItem != item)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 判断城建元素拖拽 碰撞底版 颜色
        /// </summary>
        /// <param name="item"></param>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public static bool NodeIsAvailable(MapObject item, Int2 coordinate)
        {
            MapNode node;
            Map.Instance._mapNodes.TryGetValue(coordinate.GetUnit(), out node);
            return NodeIsAvailable(item, node);
        }

        /// <summary>
        /// 判断城建元素是否可拖拽 碰撞底版 颜色
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool NodeIsAvailable(MapObject item)
        {
            var mapNodes = item.mapNodes;
            for (int i = 0; i < mapNodes.Length; i++)
            {
                if (!NodeIsAvailable(item, mapNodes[i]))
                {
                    return false;
                }
            }
            return true;
        }

        /* 无引用 */
        //public static bool FindFreeSpace(MapObject _object, Vector3 _position, out Vector3 _result)
        //{
        //    _result = Vector3.zero;
        //    return false;
        //}

        public static void ApplyNavArea(Bounds bounds, MapObject mapObject = null, bool _override = false, bool _forceClear = false)
        {
            if (mapObject)
            {
                //Instance.RescindMapNode(mapObject);
                //Instance.ApplyMapNode(mapObject, true);
            }
        }

        #endregion

        #region Event  

        public event Action<MapObject> OnObjectDeselectEvent;
        public event Action<MapObject> OnObjectSelectEvent;

        #endregion 

        #region Area

        /// <summary>
        /// 注册区域可用的所有点
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public List<MapNode> RegisterArea(Area area)
        {
            var nodes = new List<MapNode>();

            if (area && area.mapPoints != null)
            {
                var nodePoints = area.mapPoints;

                for (int i = 0; i < nodePoints.Length; i++)
                {
                    var mapPoint = nodePoints[i];

                    var mapNode = new MapNode
                    {
                        mapPoint = mapPoint,
                        ownerArea = area,
                    };

                    var unit = mapPoint.GetUnit();
                    if (!_mapNodes.ContainsKey(unit))
                    {
                        _mapNodes.Add(unit, mapNode);
                        nodes.Add(mapNode);
                    }
                }
            }

            return nodes;
        }

        /// <summary>
        /// 使用当前网格节点
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool ApplyMapNode(MapObject item, bool apply = true)
        {
            if (item == null)
            {
                return false;
            }
            var mapPoints = item.mapNodes;

            bool checkFlag = true;
            for (int i = 0; i < mapPoints.Length; i++)
            {
                var mapPoint = mapPoints[i];
                MapNode node;
                if (!_mapNodes.TryGetValue(mapPoint.GetUnit(), out node) || (node.ownerItem && node.ownerItem != item))
                {
                    checkFlag = false;
                    break;
                }
            }

            if (checkFlag && apply)
            {
                for (int i = 0; i < mapPoints.Length; i++)
                {
                    var mapPoint = mapPoints[i];
                    MapNode node;
                    if (_mapNodes.TryGetValue(mapPoint.GetUnit(), out node))
                    {
                        node.ownerItem = item;
                    }
                }
            }

            return checkFlag;
        }

        /// <summary>
        /// 取消当前使用的点
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool RescindMapNode(MapObject item)
        {
            if (item)
            {
                foreach (var kv in _mapNodes)
                {
                    var node = kv.Value;
                    if (node.ownerItem == item)
                    {
                        node.ownerItem = null;
                    }
                }
            }
            return true;
        }

        #endregion


        private void ApplyTownUnitsToGrid(MapObject exeption = null)
        {
            RescindMapNode(exeption);
            ApplyMapNode(exeption);
        }

        /// <summary>
        /// 显示建筑菜单
        /// </summary>
        /// <param name="type"></param>
        /// <param name="allow">是否能安放，控制确定按钮是否显示</param>
        private void ShowBubble(int type = 0, bool allow = true)
        {
            if (type == 0)
            {
                return;
            }

            if (this.CurrMapObject)
            {
                if (type == 1)              //如果是默认状态（非城建元素创建状态）
                {
                    if (allow)          //如果元素可以安放
                    {
                        IFunCommon funCommon = CurBuildingData as IFunCommon;

                        if (funCommon != null)
                        {
                            Action onSell = null;
                            Action onRotate = null;
                            Action onRecorery = null;
                            if (funCommon.IsRotate)
                            {
                                onRotate = () => funCommon.OnRotate();
                            }
                            if (funCommon.IsSell)
                            {
                                onSell = () =>
                                {
                                    funCommon.OnSell();
                                    this.IsBuildingMoveState = false;
                                    this.DeselectObject(null, true);
                                };
                            }
                            if (funCommon.IsRecovery)
                            {
                                onRecorery = () =>
                                {
                                    funCommon.OnRecovery();
                                    this.IsBuildingMoveState = false;
                                    this.DeselectObject(null, true);
                                };
                            }
                            BubbleConfirm.Data data = new BubbleConfirm.Data()
                            {
                                onConfirm = this.DragFinalize,
                                onCancel = this.DragCancel,
                                onRotate = onRotate,
                                onSell = onSell,
                                onRecorery = onRecorery,
                            };
                            BubbleManager.Instance.ShowConfirm(this.CurrMapObject.transform, data);
                        }
                        else
                        {
                            BubbleConfirm.Data data = new BubbleConfirm.Data()
                            {
                                onConfirm = this.DragFinalize,
                                onCancel = this.DragCancel,
                            };
                            BubbleManager.Instance.ShowConfirm(this.CurrMapObject.transform, data);
                        }
                    }
                    else
                    {
                        BubbleManager.Instance.ShowConfirm(this.CurrMapObject.transform, null, this.DragCancel);
                    }
                }
                else if (type == 2)             //城建元素创建状态
                {
                    if (allow)          //如果元素可以安放
                    {
                        BubbleConfirm.Data data = new BubbleConfirm.Data()
                        {
                            onConfirm = this.DragFinalize,
                            onCancel = this.DragCancel,
                        };
                        BubbleManager.Instance.ShowConfirm(this.CurrMapObject.transform, data);
                    }
                    else
                    {
                        BubbleManager.Instance.ShowConfirm(this.CurrMapObject.transform, null, this.DragCancel);
                    }
                }

                BuildingManager.Instance.ShowMainWindowMenu(false);
                KUIWindow.CloseWindow<FunctionWindow>();
            }
        }

        /// <summary>
        /// 隐藏城建菜单
        /// </summary>
        /// <param name="mapObject"></param>
        private void HideBubble(MapObject mapObject = null)
        {
            BubbleManager.Instance.HideConfirm();
        }

        #region 鼠标事件处理 (菜单控制)

        /// <summary>
        /// 点击非UGUI元素,非建筑层 事件
        /// </summary>
        public void OnEmptySpaceClickHandler()
        {
            //Debug.Log("点击非UGUI元素 事件");
            KUIWindow.CloseWindow<FunctionWindow>();
            KUIWindow.CloseWindow<CropWindow>();
            BubbleManager.Instance.HideObstacleClear();
            DragCancel();

            if (this.CurControlMode == ControlMode.kDrag)
            {
                //this.NormalMode();
                //this.DragAccept(true);
                //this.DeselectObject(null, false);
            }
            else if (this.CurControlMode == ControlMode.kNormal)
            {
                //SelectionHighlight.Instance.Hide();
                //RoadHighlight.Instance.Hide();
            }
        }

        /* 无引用 */
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="item"></param>
        ///// <returns></returns>
        //private bool OnMapItemLongPress(MapObject item)
        //{
        //    this.DragMode(null, null);
        //    this.SelectObject(item);
        //    return true;
        //}

        /// <summary>
        /// 注册 城建元素长按开始事件 
        /// </summary>
        private void SetupBuildingLongPress()
        {
            GameCamera.OnLongPressStart = OnLongPressStartHandler;
        }

        /// <summary>
        /// 长按开始事件处理
        /// </summary>
        /// <param name="transform"></param>
        private void OnLongPressStartHandler(Transform transform)
        {
            Debuger.Log(">>> OnLongPressStartHandler~~~~~~~~~~~~~");
            var mi = transform.GetComponent<MapObject>();
            if ((mi == null) || !mi.draggable)
            {
                GameCamera.OnLongPressProgress = null;
                GameCamera.OnLongPressBreak = null;
            }
            else
            {
                //注册城建元素长按事件 
                GameCamera.OnLongPressProgress = OnLongPressProgressHandler;
                GameCamera.OnLongPressBreak = () => {
            Debuger.Log(">>> OnLongPressBreak~~~~~~~~~~~~~");
                    GameCamera.OnLongPressProgress = null;
                    GameCamera.OnLongPressBreak = null;
                    BubbleManager.Instance.HideLongPress();
                };
            }
        }

        /// <summary>
        /// 鼠标长按住Hode 事件回调
        /// </summary>
        /// <param name="target"></param>
        /// <param name="progress"></param>
        private void OnLongPressProgressHandler(Transform target, float progress)
        {
            if ((progress >= 1f) /*&& (transform != target)*/)
            {
                BubbleManager.Instance.HideLongPress();
            }
            else if (progress > showHideLongPressDuration)
            {
                if (BuildingManager.Instance.IsOneSelf)
                {
                    Building building = target.GetComponent<Building>();
                    if (building && building.isCanSelect)
                    {
                        var bubble = BubbleManager.Instance.ShowLongPress(building.entityView.centerNode);
                        bubble.curPorgress = (progress - showHideLongPressDuration) * 1.25f;
                    }
                }
            }
        }

        #endregion

        #region MapObject列表 用于Editor

        private List<MapObject> _mapItems = new List<MapObject>();

        /// <summary>
        /// MapObject列表
        /// </summary>
        public MapObject[] MapItems
        {
            get { return _mapItems.ToArray(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(MapObject item)
        {
            _mapItems.Add(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(MapObject item)
        {
            _mapItems.Remove(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Gm"></param>
        public MapObject GetItem(GameObject Gm)
        {
            return _mapItems.Find(item => item.gameObject == Gm);
        }

        public MapObject CreateMapItem()
        {
            var mapItem = new GameObject().AddComponent<MapObject>();
            return mapItem;
        }

        #endregion

        #region Unity

        private void Awake()
        {
            Instance = this;
            MapPlane = new Plane(Vector3.forward, Vector3.zero);
            DragCancelCallBack = DragCancel;
        }

        private void OnEnable()
        {
            GameCamera.OnDragObjectFinish += this.OnDragFinishHandler;
            if (_MainMap)
            {
                //_MainMap.transform.SetParent(this.transform, false);
                _MainMap.transform.localPosition = new Vector3(0f, 0f, 50f);
                _MainMap.SetActive(true);
            }
            else
            {
                GameObject mapBG;
                if (KAssetManager.Instance.TryGetGlobalPrefab("MainMap", out mapBG))
                {
                    _MainMap = Instantiate(mapBG); //  TransformUtils.Instantiate(mapBG, this.transform, true);
                    _MainMap.transform.localPosition = new Vector3(0f, 0f, 50f);
                    DontDestroyOnLoad(_MainMap);
                }
            }
        }

        private void OnDisable()
        {
            if (_MainMap)
            {
                //_MainMap.transform.SetParent(this.transform, false);
                _MainMap.transform.localPosition = new Vector3(0f, 0f, 50f);
                _MainMap.SetActive(false);
            }
        }

        private IEnumerator Start()
        {
            GameCamera.OnEmptySpaceClick += OnEmptySpaceClickHandler;

            yield return null;
            this.NormalMode();
            BuildingSurfaceManager.Instance.InitSurface();
            BuildingManager.Instance.InitBuilding();
        }

        private void Update()
        {

        }

#if UNITY_EDITOR

        /// <summary>
        /// 
        /// </summary>
        [Alias("显示网格")]
        public bool showGizmos;

        /// <summary>
        /// 
        /// </summary>
        public void SaveMap()
        {
            var mapTable = new Hashtable();
            var itemList = new ArrayList();
            foreach (var mapItem in MapItems)
            {
                var entity = mapItem.GetComponent<Building>();
                var itemTable = new Hashtable
                    {
                        { "id", entity.entityData.itemID },
                        { "point", mapItem.mapGrid.ToArray() },
                        { "rotation",0 },
                    };
                itemList.Add(itemTable);
            }

            mapTable.Add("buildings", itemList);
            var saveText = mapTable.ToJsonBytes();
            System.IO.File.WriteAllBytes(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/buildings.json", saveText);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        private void DrawMapGrids(int w, int h)
        {
            Gizmos.color = new Color32(220, 50, 0, 200);

            var uw_2 = MapConfig.UnitWidth_2;
            var uh_2 = MapConfig.UnitHeight_2;

            var center = new Vector3(((w + h) / 2) * uw_2, ((h - w) / 2) * uh_2, 99f);

            for (int i = 0; i < w; i++)
            {
                var p1 = new Vector3(i * uw_2, (-i) * uh_2) - center;
                var p2 = new Vector3((i + h) * uw_2, (-i + h) * uh_2) - center;
                Gizmos.DrawLine(p1, p2);
            }

            for (int j = 0; j < h; j++)
            {
                var p1 = new Vector3(j * uw_2, (j) * uh_2) - center;
                var p2 = new Vector3(j * uw_2 + w * uw_2, (j - w) * uh_2) - center;
                Gizmos.DrawLine(p1, p2);
            }
        }

        private void OnDrawGizmos()
        {
            if (showGizmos)
            {
                DrawMapGrids(280, 280);
            }
        }

#endif
        #endregion
    }
}
