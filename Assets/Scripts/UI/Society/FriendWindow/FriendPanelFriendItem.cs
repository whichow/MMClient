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
//using System.Collections.Generic;
//using System.Collections;

//namespace Game.UI
//{
//    public class FriendPanelFriendItem : KUIItem, IPointerClickHandler
//    {
//        #region Filed        
//        private Text _txt_name;
//        private KUIImage _img_icon;
//        private Text _txt_lv;
//        private GameObject _go_select;
//        private Text _txt_lastIime;
//        private KUIImage _img_interact;
//        private Button _btn_interact;
//        private GameObject _go_messagePoint;
//        private Text _txt_messagNum;

//        public FriendInfo _FriendData { get; private set; }
//        #endregion
//        #region Method
//        private void Awake()
//        {
//            _txt_name = Find<Text>("Name");
//            _img_icon = Find<KUIImage>("Head/head/Image");
//            _txt_lv = Find<Text>("Head/Icon/Level/Text");
//            _go_select = Find<Transform>("Select").gameObject;
//            _go_select.SetActive(false);
//            _txt_lastIime = Find<Text>("Time");
//            _img_interact = Find<KUIImage>("btn_interact");
//            _btn_interact = Find<Button>("btn_interact");
//            _go_messagePoint = Find<Transform>("Point").gameObject;
//            _txt_messagNum = Find<Text>("Point/Text");
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
//            _txt_lv.text = _FriendData.Level.ToString();
//            _txt_lastIime.text = ShowTime;
//            RefreshInteractStatus();
//            RefreshUnMsgNum();
//        }
//        #endregion
//        #region Interface
//        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
//        {
//            KFriendManager.Instance.SelectFriendPanelFriend(this);
//        }
//        //点击选中
//        public void SelectNewItem(bool value)
//        {
//            _go_select.SetActive(value);
//        }
//        //更新互动按键的状态
//        public void RefreshInteractStatus()
//        {
//            _btn_interact.onClick.RemoveAllListeners();
//            Debug.Log("[FriendPanelFriendItem] 侦测刷新好友互动按键的状态，可领取点数：" + _FriendData.FriendPoints + "到下次的刷新时间：" + _FriendData.LeftGiveSeconds);
//            if (_FriendData.FriendPoints <= 0)
//            {
//                _img_interact.overrideSprite = _img_interact.sprites[1];
//                if (_FriendData.LeftGiveSeconds <= 0)
//                {
//                    _img_interact.material = null;
//                    _btn_interact.onClick.AddListener(SendFriendPoints);
//                }
//                else
//                {
//                    _img_interact.material = Resources.Load<Material>("Materials/UIGray");
//                    _btn_interact.onClick.AddListener(PopuText);
//                }
//            }
//            else
//            {
//                _img_interact.overrideSprite = _img_interact.sprites[0];
//                _btn_interact.onClick.AddListener(GetFriendPoints);
//            }
//            KUIWindow.GetWindow<FriendWindow>().RefreshLeftBottomBtn();
//        }
//        //飘字：今天已经赠送过了
//        private void PopuText()
//        {
//            ToastBox.ShowText("今天已经赠送结束！");
//        }
//        //领取友情点
//        private void GetFriendPoints()
//        {
//            List<int> friendids = new List<int>();
//            friendids.Add(_FriendData.PlayerId);
//            KFriendManager.Instance.C2SGetFriendPoints(friendids, GetFriendPointsCallBack);
//        }
//        private void GetFriendPointsCallBack(int code, string str, object obj)
//        {
//            RefreshInteractStatus();
//        }
//        //赠送友情点
//        private void SendFriendPoints()
//        {
//            List<int> friendids = new List<int>();
//            friendids.Add(_FriendData.PlayerId);
//            KFriendManager.Instance.C2SGiveFriendPoints(friendids, SendFriendPointsCallBack);
//        }
//        private void SendFriendPointsCallBack(int code, string str, object obj)
//        {
//            RefreshInteractStatus();
//            if (KFriendManager.Instance.PresentRewardPoints > 0)
//            {
//                ToastBox.ShowText("赠送成功，获得奖励" + KFriendManager.Instance.PresentRewardPoints + "点");
//            }
//            KFriendManager.Instance.ClearOldPresentRewardPoints();
//        }
//        /// <summary>
//        /// 刷新未读消息的红点
//        /// </summary>
//        public void RefreshUnMsgNum()
//        {
//            int unNum = KFriendManager.Instance.FriendUnMsgNum[_FriendData.PlayerId];
//            Debug.Log("[[FriendPanelFriendItem]断点：当前玩家未读消息]" + unNum);
//            if (unNum <= 0)
//            {
//                _go_messagePoint.SetActive(false);
//            }
//            else
//            {
//                _go_messagePoint.SetActive(true);
//                _txt_messagNum.text = unNum.ToString();
//            }
//            _txt_lastIime.text = ShowTime;
//        }
//        private string ShowTime
//        {
//            get { return (K.Extension.TimeExtension.ToDataTime(KFriendManager.GetTimeStamp(true) - _FriendData.LastLogin)).ToLocalTime().ToString("HH:mm") + "前"; }
//        }
//        #endregion
//    }
//}