// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "InfomationBox" company=""></copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace Game.UI
{
    public partial class ItemGetPath : KUIWindow
    {
        #region static
        public static void ShowGetPath(int itemId, Type dataclass)
        {
            DefaultData.itemID = itemId;
            DefaultData.itemClass = dataclass;
            OpenWindow<ItemGetPath>(DefaultData);
        }
        #endregion
        #region Constructor

        public ItemGetPath() :
            base(UILayer.kPop, UIMode.kNone)
        {
            uiPath = "itemGetPath";
        }

        #endregion

        #region Method
        private void UIClose() {
            CloseWindow(this);
        }
        #endregion

        #region Unity  

        public override void Awake()
        {
            InitModel();
            InitView();
        }

        public override void OnEnable()
        {
            RefreshModel();
            RefreshView();
        }
        #endregion
    }
}
