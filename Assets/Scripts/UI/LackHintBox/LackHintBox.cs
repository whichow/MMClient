// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 2017-11-02
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "LackHintBox" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;

namespace Game.UI
{
    public partial class LackHintBox : KUIWindow
    {
        #region Static

        public static void ShowLackHintBox(int id)
        {
            OpenWindow<LackHintBox>(id);
        }

        public static void HideLackHintBox()
        {
            CloseWindow<LackHintBox>();
        }

        #endregion

        #region Constructor

        public LackHintBox()
            : base(UILayer.kPop, UIMode.kNone)
        {
            uiPath = "LackHintBox";
        }

        #endregion

        #region Method

        private void OnGotoClick()
        {
            CloseWindow(this);
            OpenWindow<ShopWindow>(ShopIDConst.Special);
        }
        private void OnCloseBtnClick()
        {
            CloseWindow(this);
        }
        public override void Awake()
        {
            InitView();
        }

        public override void Update()
        {
            RefreshView();
        }

        #endregion
    }
}
