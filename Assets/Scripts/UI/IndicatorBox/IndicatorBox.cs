// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 2017-11-02
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "IndicatorBox" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;

namespace Game.UI
{
    public partial class IndicatorBox : KUIWindow
    {
        #region Static

        public static void ShowIndicator()
        {
            OpenWindow<IndicatorBox>();
        }

        public static void HideIndicator()
        {
            CloseWindow<IndicatorBox>();
        }

        #endregion

        #region Constructor

        public IndicatorBox()
            : base(UILayer.kPop, UIMode.kNone)
        {
            uiPath = "IndicatorBox";
        }

        #endregion

        #region Method

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
