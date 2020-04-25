// ***********************************************************************
// Assembly         : Unity
// Author           : LiMuChen
// Created          : 
//
// Last Modified By : LiMuChen
// Last Modified On : 
// ***********************************************************************
// <copyright file= "GuideMovieWindow" company=""></copyright>
// <summary></summary>
// ***********************************************************************

namespace Game.UI
{
    public partial class GuideMovieWindow : KUIWindow
    {
        #region Static


        #endregion

        #region Constructor

        public GuideMovieWindow()
                    : base(UILayer.kPop, UIMode.kNone)
        {
            uiPath = "GuideMovie";
            hasMask = true;
        }

        #endregion

        #region Action
        private void OnCloseBtnClikc()
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

