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
        #region Field

        /// <summary>
        /// 
        /// </summary>
        private KUIGrid _uiGrid;
        private Transform _buildingInfor;
        private Text _buildingInfoContent;

        #endregion

        #region Method

        public void InitView()
        {
            _uiGrid = Find<KUIGrid>("Panel/Scroll View");
            _buildingInfor = Find<Transform>("BuildingInfor");
            _buildingInfoContent = Find<Text>("BuildingInfor/Concent");
            if (_uiGrid)
            {
                _uiGrid.uiPool.itemTemplate.AddComponent<BuildingShopItem>();
            }
            var toggle1 = Find<Toggle>("Panel/Tab View/ToggleGroup/Toggle1");
            toggle1.onValueChanged.AddListener((value) => { if(value) OnChangeTog(PageType.Function); });
            var toggle2 = Find<Toggle>("Panel/Tab View/ToggleGroup/Toggle2");
            toggle2.onValueChanged.AddListener((value) => { if (value) OnChangeTog(PageType.Land); });
            var toggle3 = Find<Toggle>("Panel/Tab View/ToggleGroup/Toggle3");
            toggle3.onValueChanged.AddListener((value) => { if (value) OnChangeTog(PageType.Plant); });
            var toggle4 = Find<Toggle>("Panel/Tab View/ToggleGroup/Toggle4");
            toggle4.onValueChanged.AddListener((value) => { if (value) OnChangeTog(PageType.Building); });
            var toggle5 = Find<Toggle>("Panel/Tab View/ToggleGroup/Toggle5");
            toggle5.onValueChanged.AddListener((value) => { if (value) OnChangeTog(PageType.Sight); });
            var toggle6 = Find<Toggle>("Panel/Tab View/ToggleGroup/Toggle6");
            toggle6.onValueChanged.AddListener((value) => { if (value) OnChangeTog(PageType.BaseBuilding); });
            var toggle7 = Find<Toggle>("Panel/Tab View/ToggleGroup/Toggle7");
            toggle7.onValueChanged.AddListener((value) => { if (value) OnChangeTog(PageType.JoyBuilding); });
            var toggle8 = Find<Toggle>("Panel/Tab View/ToggleGroup/Toggle8");
            toggle8.onValueChanged.AddListener((value) => { if (value) OnChangeTog(PageType.WaterBuilding); });
        }

        public void showInfor(bool isShow, Transform node = null)
        {
            if (isShow)
            {
                _buildingInfor.position = node.transform.position;
                _buildingInfor.gameObject.SetActive(true);
                _buildingInfoContent.text = "建筑信息";
            }
            else
            {
                _buildingInfor.gameObject.SetActive(false);
            }
        }

        private bool IsReachMaxCount(KItemBuilding entity)
        {
            Building.Category category = (Building.Category)entity.type;
            if (category == Building.Category.kFarm || category == Building.Category.kCatHouse)
            {
                return false;
            }

            if (BuildingManager.Instance)
            {
                int cur = BuildingManager.Instance.GetEntityCount(entity.itemID);
                int max = BuildingManager.Instance.GetBuildingMaxCount(entity.itemID);
                return cur >= max;
            }

            return false;
        }

        public void RefreshView()
        {
            var buildings = GetBuildingItems();
            int tab = (int)_pageType;
            System.Array.Sort(buildings, (KItemBuilding item1, KItemBuilding item2) =>
            {
                return item1.sort.CompareTo(item2.sort);
            });
            if (_pageType == PageType.Function)
            {
                _showBuildings = System.Array.FindAll(buildings, (b) => b.tab == tab &&
               (Building.Category)b.type != Building.Category.kLifePool &&
               !IsReachMaxCount(b));
            }
            else
            {
                _showBuildings = System.Array.FindAll(buildings, (b) => b.tab == tab);
            }
            _uiGrid.uiPool.SetItemDatas(_showBuildings);
            _uiGrid.RefillItems();
        }

        #endregion
    }
}