// ***********************************************************************
// Assembly         : Unity
// Author           : LiMuChen
// Created          : 
//
// Last Modified By : LiMuChen
// Last Modified On : 
// ***********************************************************************
// <copyright file= "GiveNameWindow" company=""></copyright>
// <summary></summary>
// ***********************************************************************

namespace Game.UI
{
    public partial class GiveNameWindow : KUIWindow
    {
        #region Static


        #endregion

        #region Constructor

        public GiveNameWindow()
                    : base(UILayer.kPop, UIMode.kNone)
        {
            uiPath = "GiveName";
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
        }

        #endregion
    }
}

