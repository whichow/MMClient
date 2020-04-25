using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public  partial class SettingPushWindow : KUIWindow
    {
        #region Field


        #endregion

        #region Constructor

        public SettingPushWindow():
            base(UILayer.kNormal, UIMode.kSequence)
        {
            uiPath = "SettingPush";
        }

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
        private void OnBackBtnClick()
        {
            CloseWindow<SettingPushWindow>();
        }

        #endregion 
    }
}
