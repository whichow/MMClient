// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "SkillInfoWindow" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public partial class SkillInfoWindow : KUIWindow
    {
        public SkillInfoWindow()
            : base(UILayer.kPop, UIMode.kNone)
        {
            uiPath = "SkillInfo";
        }

        public override void Awake()
        {
            InitView();
        }

        public override void OnEnable()
        {
            RefreshModel();
            RefreshView();
        }
    }
}

