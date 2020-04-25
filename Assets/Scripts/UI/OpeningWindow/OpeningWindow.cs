// ***********************************************************************
// Assembly         : Unity
// Author           : LiMuChen
// Created          : 
//
// Last Modified By : LiMuChen
// Last Modified On : 
// ***********************************************************************
// <copyright file= "OpeningWindow" company=""></copyright>
// <summary></summary>
// ***********************************************************************

namespace Game.UI
{
    public partial class OpeningWindow : KUIWindow
    {
        #region Static


        #endregion

        #region Constructor

        public OpeningWindow()
                    : base(UILayer.kPop, UIMode.kNone)
        {
            uiPath = "Open";
        }

        #endregion

        #region Action
        private void OnCloseBtnClick()
        {
            GuideManager.Instance.CompleteStep();
            CloseWindow(this);
        }
    

        #endregion

        #region Unity  

        // Use this for initialization
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

