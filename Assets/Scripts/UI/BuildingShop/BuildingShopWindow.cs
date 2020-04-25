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

namespace Game.UI
{
    using Game.Build;
    /// <summary>
    /// 城建主弹窗控制类
    /// </summary>
    public partial class BuildingShopWindow : KUIWindow
    {
        #region Static

        public static BuildingShopWindow Instance;

        public static event Action OnAllItemsLoad;

        public static event Action OnCategoryChanged;

        public static event Action OnSubcategoryChanged;

        #endregion

        #region Field

        public List<BuildingShopItem> allItems = new List<BuildingShopItem>();

        #endregion

        #region Property   


        #endregion

        #region Constructor

        public BuildingShopWindow() :
            base(UILayer.kNormal, UIMode.kSequenceHide)
        {
            uiPath = "BuildingShop";
        }

        #endregion

        #region Action

        public void OnEmptySpaceClick()
        {
            if (this.active)
            {
                Debuger.Log(">>> 点击到空地板，关闭建筑商店窗口");

                CloseWindow<BuildingShopWindow>();
            }
        }

        private void OnChangeTog(PageType type)
        {
            _pageType = type;
            RefreshView();
        }

        #endregion       

        #region Unity

        public override void Awake()
        {
            InitModel();
            InitView();
            GameCamera.OnEmptySpaceClick += OnEmptySpaceClick;
        }

        public override void OnEnable()
        {
            RefreshModel();
            RefreshView();
        }

        #endregion 
    }
}