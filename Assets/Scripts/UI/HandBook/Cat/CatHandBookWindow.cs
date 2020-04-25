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
    public partial class CatHandBookWindow : KUIWindow
    {
        #region Constructor

        public CatHandBookWindow()
            : base(UILayer.kNormal, UIMode.kSequenceHide)
        {
            uiPath = "HandBook_Cat";
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

        private void OnPageChange(bool value)
        {
            var page = GetOnToggle();
            if (page == "All")
            {
                pageType = PageType.allCat;
            }
            else if (page == "N")
            {
                pageType = PageType.NCat;
            }
            else if (page == "R")
            {
                pageType = PageType.RCat;
            }
            else if (page == "SR")
            {
                pageType = PageType.SRCat;
            }
            else if (page == "SSR")
            {
                pageType = PageType.SSRCat;
            }
            RefreshView();
        }

        public void CloseUI() {
            CloseWindow(this);
            OpenWindow<HandBookWindow>();
        }
        #endregion
    }
}

