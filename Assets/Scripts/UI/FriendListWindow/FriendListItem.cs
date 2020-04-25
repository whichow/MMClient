///** 
// *FileName:     FriendListItem.cs 
// *Author:       LiMuChen 
// *Version:      1.0 
// *UnityVersion：5.6.3f1
// *Date:         2017-10-23 
// *Description:    
// *History: 
//*/
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//namespace Game.UI
//{
//    using K.Extension;
//    using Msg.ClientMessage;
//    using UnityEngine.EventSystems;

//    public class FriendListItem : KUIItem, IPointerClickHandler
//    {

//        #region Field
//        private Image _imageFriendHead;
//        private Text _textPlayerLvl;
//        private Text _textPlayerName;
//        private Transform _transBlack;
//        private FriendInfo _friendData;
//        #endregion

//        #region Method

//        public void ShowFriend(FriendInfo friendData)
//        {
//            _friendData = friendData;
//            _imageFriendHead.overrideSprite = KIconManager.Instance.GetHeadIcon(friendData.Head);
//            _textPlayerLvl.text = friendData.Level.ToString();
//            _textPlayerName.text = friendData.Name;
//            RefreshBlack(false);
//        }

//        protected override void Refresh()
//        {
//            ShowFriend(data as FriendInfo);
//        }
//        public void RefreshBlack(bool isActive)
//        {
//            _transBlack.gameObject.SetActive(isActive);
//        }
       


//        #endregion

//        #region Action





//        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
//        {
//            KUIWindow.GetWindow<FriendListWindow>().ChooseFriend(_friendData, this);

//        }



//        #endregion

//        #region Unity

//        private void Awake()
//        {
//            _imageFriendHead = Find<Image>("Friend/PlayerHead");
//            _textPlayerLvl = Find<Text>("Friend/LevelBack/Text");
//            _textPlayerName = Find<Text>("Friend/PlayerName");
//            _transBlack = Find<Transform>("Friend/Black");
//        }
//        void Update()
//        {

//        }

//        #endregion
//    }
//}

