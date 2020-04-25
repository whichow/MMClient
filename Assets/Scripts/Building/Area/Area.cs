// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "Area" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Build
{
    public class Area : MonoBehaviour
    {
        #region Field

        /// <summary>
        /// 
        /// </summary>
        public int areaId
        {
            get;
            private set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string areaName
        {
            get;
            private set;
        }
        /// <summary>
        /// 锁的模型
        /// </summary>
        public string[] areaModels
        {
            get;
            private set;
        }
        /// <summary>
        /// 解锁玩家等级
        /// </summary>
        public int unlockGrade
        {
            get;
            private set;
        }
        /// <summary>
        /// 解锁星星数
        /// </summary>
        public int unlockStar
        {
            get;
            private set;
        }
        /// <summary>
        /// 解锁消耗
        /// </summary>
        public KItem.ItemInfo unlockCost
        {
            get;
            private set;
        }
        /// <summary>
        /// 快速解锁消耗
        /// </summary>
        public KItem.ItemInfo quickUnlockCost
        {
            get;
            private set;
        }

        /// <summary>
        /// 前置区域
        /// </summary>
        public int[] frontArea
        {
            get;
            private set;
        }

        private bool isPumping;
        /// <summary>
        /// 
        /// </summary>
        private AreaData _areaData;
        /// <summary>
        /// 地图节点
        /// </summary>
        private List<MapNode> _mapNodes;

        #endregion

        #region Property       

        /// <summary>
        /// 地图坐标
        /// </summary>
        public Int2[] mapPoints
        {
            get;
            private set;
        }

        /// <summary>
        /// 解锁
        /// </summary>
        public bool unlocked
        {
            get;
            set;
        }

        /// <summary>
        /// 陆地建筑
        /// </summary>
        public bool isLand
        {
            get
            {
                return (geography & AreaGeography.fLand) != 0;
            }
        }

        /// <summary>
        /// 水面建筑
        /// </summary>
        public bool isWater
        {
            get
            {
                return (geography & AreaGeography.fWater) != 0;
            }
        }

        /// <summary>
        /// 地理特征
        /// </summary>
        public AreaGeography geography
        {
            get;
            private set;
        }

        /// <summary>
        /// 边界
        /// </summary>
        public AreaBorder areaBorder
        {
            get;
            private set;
        }
        private GameObject _areaTag;
        #endregion

        #region Method

        public void LoadTable(Hashtable table)
        {
            areaId = table.GetInt("AreaId");
            areaName = table.GetString("Area");
            areaModels = table.GetArray<string>("Model");
            unlockGrade = table.GetInt("UnlockLevel");
            unlockStar = table.GetInt("UnlockStar");
            unlockCost = KItem.ItemInfo.Convert(table.GetArrayList("UnlockCost"));
            quickUnlockCost = KItem.ItemInfo.Convert(table.GetArrayList("QuickUnlock"));
            frontArea = table.GetArray<int>("FrontArea");
        }

        public void LoadData()
        {
            GameObject dataObj;
            if (KAssetManager.Instance.TryGetBuildingPrefab("Area/" + areaName, out dataObj))
            {
                var tmpData = dataObj.GetComponent<AreaData>();
                if (tmpData)
                {
                    _areaData = tmpData;
                    geography = tmpData.geography;
                    mapPoints = tmpData.GetGrids();

                    this.name = areaName;
                    this.gameObject.layer = LayerManager.AreaLayer;
                    this.transform.localPosition = tmpData.transform.localPosition;

                    areaBorder = gameObject.AddComponent<AreaBorder>();
                    var polygon = _areaData.GetComponent<PolygonCollider2D>();
                    for (int i = 0; i < polygon.pathCount; i++)
                    {
                        areaBorder.SetPath(i, polygon.GetPath(i));
                    }
                }
            }
        }
        public void ShowLockModel()
        {
            var model = transform.Find("Model");
            if (unlocked)
            {
                if (model)
                {
                    Destroy(model.gameObject);
                }
            }
            else
            {
                if (!model)
                {
                    model = new GameObject("Model").transform;
                    model.SetParent(transform, false);
                    foreach (var areaModel in areaModels)
                    {
                        GameObject prefab;
                        if (KAssetManager.Instance.TryGetBuildingPrefab("Area/" + areaModel, out prefab))
                        {

                            Transform trans = Instantiate(prefab).transform;
                            trans.SetParent(model, true);
                            Transform findTrans = trans.Find("Panel_Sx_17");
                            if (findTrans)
                            {
                                _areaTag = findTrans.gameObject;
                                _areaTag.SetActive(false);
                            }
                            else
                                Debug.LogError("美术资源未配置区域 标记：Panel_Sx_17");
                        }
                    }
                }
            }
        }
        public void setAreaTag(/*bool isShow*/)
        {
            if (!_areaTag)
                return;
            _areaTag.SetActive(frontArea.Length > 0);
            if (frontArea != null && frontArea.Length > 0)
            {
                bool isShowTag = true;
                foreach (var areaId in frontArea)
                {
                    var fArea = AreaManager.Instance.GetArea(areaId);
                    if (fArea != null && !fArea.unlocked)
                    {
                        isShowTag = false;
                        break;
                    }
                }
                _areaTag.SetActive(isShowTag);
            }


        }
        private void Pump()
        {
            if (isPumping)
            {
                return;
            }
            isPumping = true;
            var model = transform.Find("Model");
            if (model)
            {
                KTweenUtils.DOPunchScale(model, Vector3.one * 0.1f, 0.6f, 4, 1, OnPumpComplete);
            }
        }

        private void OnPumpComplete()
        {
            var model = transform.Find("Model");
            if (model)
            {
                model.localScale = Vector3.one;
            }
            isPumping = false;
        }
        /// <summary>
        /// 检测是否在这个区域
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool IsInside(Vector3 position)
        {
            return areaBorder.IsInside(position);
        }

        private void ApplyNavArea(int index)
        {
        }

        public void ToggleArea(bool colliderEnabled, bool canBuy)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="show"></param>
        public void ToggleHighlight(bool show)
        {
            //if (show)
            //{
            //    AreaHighlight.Instance.ShowHighlight(this, _mapNodes);
            //}
            //else
            //{
            //    AreaHighlight.Instance.HideHighlight();
            //}
        }

        public void OnTap()
        {
            if (BuildingManager.Instance.isFriend)
            {
                return;
            }
            //if (_areaData.geography == AreaGeography.fLand)
            //{
            //    MouseFxMgr.Instance.playGrassFx();
            //}
            //else if (_areaData.geography == AreaGeography.fWater)
            //{
            //    MouseFxMgr.Instance.playWaterFx();
            //}
            if (!unlocked)
            {
                //this.Pump();  //美术需求

                bool meet = true;
                if (frontArea != null && frontArea.Length > 0)
                {
                    meet = false;
                    foreach (var areaId in frontArea)
                    {
                        var fArea = AreaManager.Instance.GetArea(areaId);
                        if (fArea != null && fArea.unlocked)
                        {
                            meet = true;
                            break;
                        }
                    }
                }

                if (meet)
                {
                    UI.AreaUnlockBox.ShowBox(unlockGrade, unlockStar, unlockCost.itemCount, unlockCost.itemID, OnUnlockConfirm);
                }
                else
                {
                    UI.ToastBox.ShowText(KLocalization.GetLocalString(52001));
                }
            }

        }

        private void OnUnlockConfirm()
        {
            KUser.BuildingUnlockArea(areaId, 0, UnlockAreaCallback);
        }

        private void UnlockAreaCallback(int code, string message, object data)
        {
            if (code == 0)
            {
                Debug.Log("解锁成功");
                BuildingFxManager.Instance.playunlockingFx(this.transform.position);
                AreaManager.Instance.AreaDataUpdate();
            }
            else
                Debug.Log("解锁失败");
        }

        #endregion

        #region Unity

        // Use this for initialization
        private void Start()
        {
            _mapNodes = Map.Instance.RegisterArea(this);
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            if (mapPoints != null && mapPoints.Length > 0)
            {
                for (int i = 0; i < mapPoints.Length; i++)
                {
                    MapHelper.DrawRhombus(mapPoints[i].x, mapPoints[i].y, 5f);
                }
            }
        }

#endif

        #endregion
    }
}
