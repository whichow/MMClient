// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "MapObject" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Game.Build
{
    /// <summary>
    /// 网格数据  碰撞添加
    /// </summary>
    public class MapObject : MonoBehaviour
    {
        #region Static
        /// <summary>
        /// 长按事件 回调  --在Map.DragMode()和Map.NormalMode() 注册
        /// </summary>
        public static Func<MapObject, bool> OnMapObjectLongPress;
        public static Func<MapObject, bool> OnMapObjectSelect;

        public static readonly List<MapObject> ObjectList = new List<MapObject>();
        public static readonly List<MapObject> ActivationQueue = new List<MapObject>(150);

        #endregion

        #region Field

        /// <summary>
        /// 
        /// </summary>
        private Int2 _mapSize;
        /// <summary>
        /// 
        /// </summary>
        private Int2[] _mapNodes;

        #endregion

        #region Property

        /// <summary>
        /// 地图大小
        /// </summary>
        public Int2 mapSize
        {
            get { return _mapSize; }
            set
            {
                _mapSize = value;
                _mapNodes = new Int2[value.x * value.y];
            }
        }

        /// <summary>
        /// 网格坐标
        /// </summary>
        public Int2 mapGrid
        {
            get { return MapHelper.PositionToGrid(transform.localPosition); }
            set
            {
                transform.localPosition = MapHelper.GridToPosition(value);
            }
        }

        /// <summary>
        /// 网格节点
        /// </summary>
        public Int2[] mapNodes
        {
            get
            {
                var origin = this.mapGrid;

                for (int i = 0, index = 0; i < _mapSize.x; i++)
                {
                    for (int j = 0; j < _mapSize.y; j++)
                    {
                        _mapNodes[index++].Set(origin.x + i, origin.y + j);
                    }
                }

                return _mapNodes;
            }
        }


        /// <summary>
        /// 可拖动
        /// </summary>
        public bool draggable
        {
            get { return CurrBuildingData.entityData.movable > 0; }
        }
        /// <summary>
        /// 存储veiw节点（用于城建元素上移）
        /// </summary>
        public Transform viewTransform
        {
            get;
            private set;
        }
        public Building CurrBuildingData
        {
            get; private set;
        }

        public Vector3 originalPos;
        #endregion

        #region Method
        /// <summary>
        /// 被选中 位移动画
        /// </summary>
        private void SetupSelectionTweener()
        {
            //KTweenUtils.DoFade(this.transform.GetChild(0).gameObject, 0.7f, 0.2f);
            if (viewTransform)
            {
                KTweenUtils.LocalMoveTo(viewTransform, new Vector3(0f, MapConfig.UnitHeight_2, -1f), 0.1f);
            }
        }
        /// <summary>
        /// 元素被选中  事件回调
        /// </summary>
        public virtual void Select()
        {
            //this.Pump();
            //SelectionHighlight.Instance.Hide();
            //RoadHighlight.instance.Hide();
            this.SetupSelectionTweener();
            CurrBuildingData.entityView.SetBrightness();
            CurrBuildingData.entityView.StartScale();
            //if (this.tweener != null)
            //{
            //    this.tweener.PlayForward();
            //}
        }

        public void SelectSkipToEnd()
        {
            //if (this.tweener != null)
            //{
            //    this.tweener.value = this.tweener.to;
            //}
        }
        /// <summary>
        /// 失去焦点  ，取消选择时  事件回调
        /// </summary>
        public virtual void Deselect()
        {
            //KTweenUtils.DoFade(this.transform.GetChild(0).gameObject, 1f, 0.2f);
            if (viewTransform)
            {
                KTweenUtils.LocalMoveTo(viewTransform, Vector3.zero, 0.1f);
                CurrBuildingData.entityView.ResetBrightness();
                CurrBuildingData.entityView.StartScale();
            }
            //HighlightSelection.Instance.Hide();
            //if (this.tweener != null)
            //{
            //    this.tweener.PlayReverse();
            //}
        }

        /// <summary>
        /// 长按城建元素 事件（在GameCamera.UpdateScroll中调用）
        /// </summary>
        protected virtual void OnLongPress()
        {
            Debuger.Log(">>> 长按城建元素");
            if (Map.Instance.CurrMapObject == null)
            {
                if (BuildingManager.Instance.IsOneSelf && CurrBuildingData.isCanSelect)
                {
                    //SelectionHighlight.Instance.HideSelection();
                    originalPos = this.transform.position;
                    Map.Instance.IsBuildingMoveState = true;

                    if (this.draggable)
                    {
                        if (OnMapObjectLongPress != null)
                        {
                            OnMapObjectLongPress(this);
                        }
                        this.Select();
                    }
                }
            }
        }

        /// <summary>
        /// 旋转地图
        /// </summary>
        public void RotateMap()
        {
            mapSize = new Int2(mapSize.y, mapSize.x);
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void OnTap()
        {

        }

        public virtual void OnFocus(bool focus)
        {
            if (focus)
            {
                CurrBuildingData.entityView.SetBrightness(true);
                //SelectionHighlight.Instance.ShowSelection(this);
            }
            else
            {
                CurrBuildingData.entityView.ResetBrightness();
                //SelectionHighlight.Instance.HideSelection();
            }
        }

        private void AddPolygon()
        {
            PolygonCollider2D polygon = gameObject.GetComponent<PolygonCollider2D>();
            if (!polygon)
                polygon = gameObject.AddComponent<PolygonCollider2D>();

            Vector2[] points = new Vector2[5];
            points[0] = Vector2.zero;
            points[1] = new Vector2(mapSize.y * MapConfig.UnitWidth_2, mapSize.y * MapConfig.UnitHeight_2);
            points[2] = new Vector2((mapSize.x + mapSize.y) * MapConfig.UnitWidth_2, (mapSize.y - mapSize.x) * MapConfig.UnitHeight_2);
            points[3] = new Vector2(mapSize.x * MapConfig.UnitWidth_2, -mapSize.x * MapConfig.UnitHeight_2);
            points[4] = Vector2.zero;

            polygon.SetPath(0, points);
        }
        public void AddPolygonFromModel()
        {
            PolygonCollider2D polygon = gameObject.GetComponent<PolygonCollider2D>();
            if (!polygon)
                polygon = gameObject.AddComponent<PolygonCollider2D>();


            if (CurrBuildingData.entityView.polygonColliderVec != null && CurrBuildingData.entityView.polygonColliderVec.Length > 0)
                polygon.SetPath(0, CurrBuildingData.entityView.polygonColliderVec);
            else
                AddPolygon();
        }

        #endregion

        #region Unity

        /// <summary>
        /// 
        /// </summary>
        private void Awake()
        {
            viewTransform = transform.Find("View");
            CurrBuildingData = GetComponent<Building>();

            if (CurrBuildingData.entityData.type == (int)Building.Category.kSurface)
            {
                this.gameObject.layer = LayerManager.ClickerLayer;
            }
            else
            {
                this.gameObject.layer = LayerManager.BuildingLayer;
            }

            //AddPolygon();
        }

        // Use this for initialization
        private void Start()
        {
            //viewTransform = transform.Find("View");
            AddPolygonFromModel();
            //AddPolygon();


        }

        /// <summary>
        /// 
        /// </summary>
        private void OnEnable()
        {
            if (Map.Instance)
            {
                Map.Instance.AddItem(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnDisable()
        {
            if (Map.Instance)
            {
                Map.Instance.RemoveItem(this);
            }
        }

        private void OnDestroy()
        {
            if (Map.Instance)
            {
                Map.Instance.RemoveItem(this);
            }
        }

        // Update is called once per frame
        private void Update()
        {
        }

#if UNITY_EDITOR  

        private void OnDrawGizmos()
        {
            var vp = MapHelper.PositionToGrid(transform.position);
            int x = vp.x;
            int y = vp.y;
            int w = this.mapSize.x;
            int h = this.mapSize.y;

            Gizmos.color = Color.red;

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    int xx = x + i;
                    int yy = y + j;
                    MapHelper.DrawRhombus(xx, yy);
                }
            }
        }

#endif
        #endregion
    }
}
