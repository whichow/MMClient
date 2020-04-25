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

namespace Game.UI
{
    public partial class InfomationBox : KUIWindow
    {
        #region Constructor

        public InfomationBox() :
            base(UILayer.kPop, UIMode.kNone)
        {
            uiPath = "InfomationBox";
        }

        #endregion

        #region Method

        private void OnOkClick()
        {
            CloseWindow(this);
            if (_infomationData.onConfirm != null)
            {
                _infomationData.onConfirm();
            }
        }

        public void OnCloseClick()
        {
            CloseWindow(this);
            if (_infomationData.onCancel != null)
            {
                _infomationData.onCancel();
            }
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
