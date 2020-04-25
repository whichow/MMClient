//// ***********************************************************************
//// Assembly         : Unity
//// Author           : Kimch
//// Created          : 
////
//// Last Modified By : Kimch
//// Last Modified On : 
//// ***********************************************************************
//// <copyright file= "CatBagWindow" company=""></copyright>
//// <summary></summary>
//// ***********************************************************************

////using UnityEngine;

//namespace Game.UI
//{
//    public partial class FriendWindow : KUIWindow
//    {
//        #region Constructor

//        public FriendWindow() :
//            base(UILayer.kNormal, UIMode.kSequenceHide)
//        {
//            uiPath = "FriendWindow";
//            uiAnim = UIAnim.kAnim1;
//            hasMask = true;
//        }

//        #endregion

//        #region Action

//        private void OnQuitBtnClick()
//        {
//            CloseWindow(this);
//        }

//        public void RefreshListAfterGetAward(int code, string message, object data)
//        {
//            RefreshView();
//        }

//        private void OnPageChange(bool value)
//        {
//            var page = GetOnToggle();

//            if (page == "tgl_friend")
//            {
//                pageType = PageType.kFriend;
//            }
//            else if (page == "tgl_application")
//            {
//                pageType = PageType.kApplication;
//            }
//            else if (page == "tgl_room")
//            {
//                pageType = PageType.kRoom;
//            }
//            RefreshView();
//        }

//        #endregion

//        #region Unity  

//        public void OnGetDaily(int code, string message, object data)
//        {
//            //RefreshModel();
//            RefreshView();
//        }

//        public void OnAchievement(int code, string message, object data)
//        {
//            //RefreshModel();
//            RefreshView();
//        }

//        public override void Awake()
//        {
//            InitModel();
//            InitView();
//        }

//        // Use this for initialization
//        public override void Start()
//        {
//        }

//        public override void OnEnable()
//        {
//            KEmojiManager.Instance.event_AddSpecialExpression = AddSpecialExpression;
//            KEmojiManager.Instance.event_AddCommonExpression = AddCommonExpression; 

//            RefreshModel();
//        }

//        #endregion
//    }
//}

