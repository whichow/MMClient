// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "BuildingShopWindow" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using Game.Build;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /// <summary>
    /// 城建主弹窗控制类
    /// </summary>
    partial class BuildingShopWindow
    {
        #region Enum

        public enum PageType : uint
        {
            None,
            Function,
            Land,
            Plant,
            Building,
            Sight,
            BaseBuilding,
            JoyBuilding,
            WaterBuilding,
        }

        #endregion

        #region Field

        private PageType _pageType;
        private KItemBuilding[] _showBuildings;

        #endregion

        #region Method

        public void InitModel()
        {
            _pageType = PageType.Function;
        }

        public void RefreshModel()
        {
        }

        public KItemBuilding[] GetBuildingItems()
        {
            var buildings = KItemManager.Instance.GetBuildings();
            return buildings;      
        }

        #endregion
    }
}