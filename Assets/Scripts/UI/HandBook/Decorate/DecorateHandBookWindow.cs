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
    public partial class DecorateHandBookWindow : KUIWindow
    {
        #region Constructor

        public DecorateHandBookWindow()
            : base(UILayer.kNormal, UIMode.kSequenceHide)
        {
            uiPath = "HandBook_Decorate";
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
            if (page == "letter")
            {
                pageType = PageType.letter;
            }
            else if (page == "infrastructure")
            {
                pageType = PageType.infrastructure;
            }
            else if (page == "trees")
            {
                pageType = PageType.trees;
            }
            else if (page == "water")
            {
                pageType = PageType.water;
            }
            else if (page == "others")
            {
                pageType = PageType.others;
            }
            RefreshView();
        }

        #endregion
    }
}

