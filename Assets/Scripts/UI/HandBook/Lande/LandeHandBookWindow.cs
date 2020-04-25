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
    public partial class LandeHandBookWindow : KUIWindow
    {
        #region Constructor

        public LandeHandBookWindow()
            : base(UILayer.kNormal, UIMode.kSequenceHide)
        {
            uiPath = "HandBook_Lande";
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
            OpenWindow<HandBookWindow>();
        }

        private void OnPageChange(bool value)
        {
            var page = GetOnToggle();

            if (page == "btn_8018")
            {
                pageType = PageType.Kmaozhen;
            }
            else if (page == "btn_8019")
            {
                pageType = PageType.Kyangguangdao;
            }
            else if (page == "btn_8020")
            {
                pageType = PageType.Kyouleyuan;
            }
            else if (page == "btn_8021")
            {
                pageType = PageType.Kxinyangdao;
            }
            else if (page == "btn_8022")
            {
                pageType = PageType.Kkongzhongfeidao;
            }
            else if (page == "btn_8023")
            {
                pageType = PageType.Kmeishidao;
            }
            else if (page == "btn_8024")
            {
                pageType = PageType.Kliulangmaojidi;
            }
            OpenWindow<SingleLandeHandBookWindow>(pageType);
        }

        #endregion
    }
}

