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

//namespace Game.UI
//{
//    using Msg.ClientMessage;
//    public partial class FriendWindow
//    {
//        #region Enum

//        public enum PageType
//        {
//            kFriend,
//            kApplication,
//            kRoom,
//        }
//        //添加好友界面左侧内容，默认是1：附近的人，搜索后是2：搜索结果
//        public enum ApplicationPanelLeftBottomContent {
//            kNearBy,
//            kSearchResult,
//        }
//        #endregion

//        #region Field
//        private PageType _pageType;
//        #endregion

//        #region Porperty
//        /// <summary>
//        /// 
//        /// </summary>
//        public PageType pageType
//        {
//            get { return _pageType; }
//            private set
//            {
//                if (_pageType != value)
//                {
//                    _pageType = value;
//                }
//            }
//        }
//        #endregion

//        #region Method
//        private void InitModel()
//        {
//            ResetToggle();
//        }
//        private void ResetToggle() {
//            pageType = PageType.kFriend;
//        }
//        public void RefreshModel()
//        {
//            KFriendManager.Instance.GetFriends(GetFriendsCallBack);
//        }
//        public FriendInfo[] GetFriendArry()
//        {
//            return KFriendManager.Instance.Arry_Friends;
//        }
//        public FriendReq[] GetFriendApplicationArry() {
//            return KFriendManager.Instance.Arry_FriendReqs;
//        }
//        #endregion
//    }
//}

