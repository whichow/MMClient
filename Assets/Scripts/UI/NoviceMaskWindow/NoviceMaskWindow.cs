// ***********************************************************************
// Assembly         : Unity
// Author           : LiMuChen
// Created          : 
//
// Last Modified By : LiMuChen
// Last Modified On : 
// ***********************************************************************
// <copyright file= "NoviceMaskWindow" company=""></copyright>
// <summary></summary>
// ***********************************************************************

namespace Game.UI
{
    public partial class NoviceMaskWindow : KUIWindow
    {
        #region Static


        #endregion

        #region Constructor

        public NoviceMaskWindow()
                    : base(UILayer.kPop, UIMode.kNone)
        {
            uiPath = "NovieMask";
        }

        #endregion

        #region Action

    

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
            KUIWindow.CloseWindow<GuideWindow>();
        }

        #endregion
    }
}

