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
    public partial class ItemInformationBoard : KUIWindow
    {
        #region Constructor

        public ItemInformationBoard()
            : base(UILayer.kPop, UIMode.kNone)
        {
            uiPath = "ItemInformationBoard";
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

        public void CloseUI() {
            CloseWindow(this);
        }
        #endregion
    }
}

