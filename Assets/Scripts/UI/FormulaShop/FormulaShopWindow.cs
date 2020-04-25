// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "FormulaShopWindow" company=""></copyright>
// <summary></summary>
// ***********************************************************************

namespace Game.UI
{
    public partial class FormulaShopWindow : KUIWindow
    {
        #region Constructor

        public FormulaShopWindow()
            : base(UILayer.kNormal, UIMode.kSequenceRemove)
        {
            uiPath = "FormulaShop";
        }

        #endregion

        #region Method       

        #endregion

        #region Action

        private void OnCloseBtnClick()
        {
            CloseWindow(this);
        }

        private void OnPageChange(bool value)
        {
            var page = GetOnToggle();

            if (page == "Toggle2")
            {
                pageType = PageType.kTag2;
            }
            else if (page == "Toggle3")
            {
                pageType = PageType.kTag3;
            }
            else if (page == "Toggle4")
            {
                pageType = PageType.kTag4;
            }
            else if (page == "Toggle5")
            {
                pageType = PageType.kTag5;
            }
            else if (page == "Toggle6")
            {
                pageType = PageType.kTag6;
            }
            else
            {
                pageType = PageType.kAll;
            }

            if (isChanged)
            {
                RefreshView();
            }
        }

        #endregion

        #region Unity

        public override void Awake()
        {
            InitView();
        }

        public override void OnEnable()
        {
            RefreshView();
        }

        #endregion
    }
}

