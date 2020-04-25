// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "CropWindow" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using Game.Build;
using UnityEngine;

namespace Game.UI
{
    partial class CropWindow : KUIWindow
    {
        #region Field  

        #endregion

        #region Method 

        public override void Awake()
        {
            InitView();
        }

        public override void OnEnable()
        {
            RefreshView();
        }

        #endregion

        #region Constructor

        public CropWindow() :
            base(UILayer.kNormal, UIMode.kSequenceHide)
        {
            uiPath = "CropWindow";
        }

        #endregion
    }
}