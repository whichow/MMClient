// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "MessageBox" company=""></copyright>
// <summary></summary>
// ***********************************************************************

namespace Game.UI
{
    public partial class ShowAwardList : KUIWindow
    {
        #region Constructor

        public ShowAwardList()
            : base(UILayer.kPop, UIMode.kNone)
        {
            uiPath = "AwardListBoard";
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

        public void CloseUI()
        {
            CloseWindow(this);
        }
        #endregion
    }
}

