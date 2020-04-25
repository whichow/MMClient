/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/4/30 10:24:33
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game;
using Game.UI;

namespace Game
{
    public partial class GuideWindow : KUIWindow
    {

        #region C&D
        public GuideWindow() : base(UILayer.kPop, UIMode.kNone)
        {
            uiPath = "GuideWindow";
        }
        #endregion

        public override void Awake()
        {
            InitModel();
            InitView();
            AddEvent();
        }

        public override void OnEnable()
        {
            RefreshModel();
            RefreshView();
        }

        public override void OnDisable()
        {
            base.OnDisable();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            RemoveEvent();
        }

        private void AddEvent()
        {
        }

        private void RemoveEvent()
        {
        }
    }
}
