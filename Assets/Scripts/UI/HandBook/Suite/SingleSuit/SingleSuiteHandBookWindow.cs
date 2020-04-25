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
    public partial class SingleSuiteHandBookWindow : KUIWindow
    {
        #region Constructor

        public SingleSuiteHandBookWindow()
            : base(UILayer.kNormal, UIMode.kSequenceHide)
        {
            uiPath = "HandBook_SingleSuit";
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
            RefreshModel();
            RefreshView();
        }

        public void CloseUI() {
            CloseWindow(this);
            OpenWindow<SuiteHandBookWindow>();
        }
        #endregion
    }
}

