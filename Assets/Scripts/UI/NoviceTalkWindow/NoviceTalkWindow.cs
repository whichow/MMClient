// ***********************************************************************
// Assembly         : Unity
// Author           : LiMuChen
// Created          : 
//
// Last Modified By : LiMuChen
// Last Modified On : 
// ***********************************************************************
// <copyright file= "NoviceTalkWindow" company=""></copyright>
// <summary></summary>
// ***********************************************************************

namespace Game.UI
{
    public partial class NoviceTalkWindow : KUIWindow
    {
        #region Static


        #endregion

        #region Constructor

        public NoviceTalkWindow()
                    : base(UILayer.kPop, UIMode.kNone)
        {
            uiPath = "NoviceTalk";
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

