// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "CatInfoWindow" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public partial class CatInfoWindow : KUIWindow
    {
        private Button _closeBtn;

        public CatInfoWindow()
           : base(UILayer.kNormal, UIMode.kSequenceHide)
        {
            uiPath = "CatInfoWindow";
            uiAnim = UIAnim.kAnim1;
            hasMask = true;
        }

        public override void Awake()
        {
            _closeBtn = Find<Button>("Quit");
            _closeBtn.onClick.AddListener(OnCloseBtnClick);
            InitView();
        }

        public override void OnEnable()
        {
            RefreshView();
        }

        private void OnCloseBtnClick()
        {
            CloseWindow(this);
        }
    }
}

