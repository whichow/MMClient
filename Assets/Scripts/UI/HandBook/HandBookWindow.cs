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
    public partial class HandBookWindow : KUIWindow
    {
        #region Constructor

        public HandBookWindow()
            : base(UILayer.kNormal, UIMode.kSequenceRemove)
        {
            uiPath = "HandBook_Main";
            uiAnim = UIAnim.kAnim1;
            hasMask = true;
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
            ShowOrHideBtns(false);
            RefreshModel();
            RefreshView();
        }

        public void CloseUI() {
            CloseWindow(this);
        }
        private void OpenCatBook() {
            OpenWindow<CatHandBookWindow>();
        }
        private void OpenDecorateBook() {
            OpenWindow<DecorateHandBookWindow>();
        }
        private void OpenSuiteBook() {
            OpenWindow<SuiteHandBookWindow>();
        }
        private void OpenLandeBook() {
            OpenWindow<LandeHandBookWindow>();
        }
        #endregion
    }
}

