using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public partial class SetLanguangeWindow : KUIWindow
    {
        #region Field


        #endregion

        #region Constructor

        public SetLanguangeWindow() :
            base(UILayer.kNormal, UIMode.kSequence)
        {
            uiPath = "SetLanguage";
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
            CloseWindow<SetLanguangeWindow>();
        }




    }
    #endregion
}

