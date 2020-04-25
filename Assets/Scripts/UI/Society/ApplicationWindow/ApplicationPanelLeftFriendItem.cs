//// ***********************************************************************
//// Assembly         : Unity
//// Author           : Kimch
//// Created          : 
////
//// Last Modified By : Kimch
//// Last Modified On : 
//// ***********************************************************************
//// <copyright file= "BagItem" company=""></copyright>
//// <summary></summary>
//// ***********************************************************************
//using Msg.ClientMessage;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;

//namespace Game.UI
//{
//    public class ApplicationPanelLeftFriendItem : KUIItem, IPointerClickHandler
//    {
//        #region Filed        
//        private Text _txt_name;
//        private Image _img_icon;
//        private Text _txt_lv;
//        private GameObject _go_select;
//        private Text _txt_lastIime;
//        private KUIImage _img_interact;
//        private Button _btn_interact;
//        private GameObject _go_messagePoint;
//        private Text _txt_messagNum;

//        private FriendInfo _FriendData;
//        #endregion
//        #region Method
//        private void Awake()
//        {
//            _txt_name = Find<Text>("Name");
//            _img_icon = Find<Image>("Head/head/Image");
//            _txt_lv = Find<Text>("Head/Icon/Level/Text");
//            _txt_lastIime = Find<Text>("Signature");
//            _btn_interact = Find<Button>("Button");
//            _btn_interact.onClick.AddListener(ClickAddFriend);
//        }
//        #endregion
//        #region Method
//        public void ShowItem(FriendInfo[] Frds, int index)
//        {
//            if (Frds == null || Frds.Length == 0)
//            {
//                return;
//            }
//            var FD = Frds[index];
//            _FriendData = FD;
//            _txt_name.text = _FriendData.Name;
//            _img_icon.overrideSprite = KIconManager.Instance.GetHeadIcon(_FriendData.Head);
//            _txt_lastIime.text = ShowTime;
//        }
//        private void ClickAddFriend() {
//            bool targetIsFrd = false;
//            for (int i = 0; i < KFriendManager.Instance.Arry_Friends.Length; i++)
//            {
//                if (KFriendManager.Instance.Arry_Friends[i].PlayerId == _FriendData.PlayerId)
//                {
//                    targetIsFrd = true;
//                }
//            }
//            if (_FriendData.PlayerId == KUser.SelfPlayer.id)
//            {
//                ToastBox.ShowText(KLocalization.GetLocalString(53041));
//                return;
//            }
//            if (targetIsFrd)
//            {
//                ToastBox.ShowText(KLocalization.GetLocalString(53042));
//                return;
//            }
//            KFriendManager.Instance.AddFriend(_FriendData.PlayerId, AddFriendCallBack);            
//        }
//        private void AddFriendCallBack(int code,string str,object obj) {
//            //Debug.Log("发送加好友通知，对方ID：" + KFriendManager.Instance.LastAddFriendID);
//            ToastBox.ShowText(KLocalization.GetLocalString(53043));
//        }
//        #endregion
//        #region Interface
//        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
//        {        
//        }
//        private string ShowTime
//        {
//            get { return (K.Extension.TimeExtension.ToDataTime(KFriendManager.GetTimeStamp(true) - _FriendData.LastLogin)).ToLocalTime().ToString("HH:mm") + "前"; }
//        }
//        #endregion
//    }
//}