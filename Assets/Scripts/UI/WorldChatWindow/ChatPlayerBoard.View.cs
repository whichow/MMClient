
///** 
//* 作用：世界聊天的界面人物头像弹出框的控制
//* 作者：wsy
//*/
//using Game.Build;
//using Msg.ClientMessage;
//using System;
//using UnityEngine;
//using UnityEngine.UI;

//namespace Game.UI
//{
//    public partial class ChatPlayerBoard
//    {
//        #region Field  
//        private Button _blackBack;
//        private KUIImage _img_head;
//        private Text _txt_name;
//        private Text _txt_level;
//        private Button _btn_add;
//        private Image _img_add;
//        private Button _btn_visit;
//        private Button _btn_room;
//        private Button _btn_zan;
//        private Image _img_zan;

//        private WorldChatItem _data_chatData;
//        #endregion

//        #region Method
//        public void InitView()
//        {
//            _blackBack = Find<Button>("BackGround");
//            _img_head = Find<KUIImage>("Panel/kuiimg_head");
//            _txt_name = Find<Text>("Panel/txt_name");
//            _txt_level = Find<Text>("Panel/txt_lv");
//            _btn_add = Find<Button>("Panel/btn_add");
//            _img_add = Find<Image>("Panel/btn_add");
//            _btn_visit = Find<Button>("Panel/btn_visit");
//            _btn_visit.onClick.AddListener(VisitPlayer);
//            _btn_room = Find<Button>("Panel/btn_room");
//            _btn_room.onClick.AddListener(GoToRoom);
//            _btn_zan = Find<Button>("Panel/btn_zan");
//            _img_zan = Find<Image>("Panel/btn_zan");
//        }
//        /// <summary>
//        /// View层入口
//        /// </summary>
//        public void RefreshView()
//        {
//            _img_head.overrideSprite = KIconManager.Instance.GetHeadIcon(_data_chatData.PlayerHead);
//            _txt_name.text = _data_chatData.PlayerName;
//            _txt_level.text = _data_chatData.PlayerLevel.ToString();
//            _btn_add.onClick.RemoveAllListeners();
//            if (Convert.ToBoolean(_data_chatData.IsFriend))
//            {
//                _img_add.material = null;
//               _btn_add.onClick.AddListener(AddFriend);
//            }
//            else {
//                _img_add.material = Resources.Load<Material>("Materials/UIGray");
//                _btn_add.onClick.AddListener(()=> {
//                    ToastBox.ShowText("已经是好友了");
//                });
//            }
//            _btn_zan.onClick.RemoveAllListeners();
//            if (Convert.ToBoolean(_data_chatData.IsZaned))
//            {
//                _img_zan.material = null;
//               _btn_zan.onClick.AddListener(ZanPlayer);
//            }
//            else
//            {
//                _img_zan.material = Resources.Load<Material>("Materials/UIGray");
//                _btn_zan.onClick.AddListener(() => {
//                    ToastBox.ShowText("今天已经赞过了");
//                });
//            }
//        }
//        private void AddFriend() {
//            KFriendManager.Instance.AddFriend(_data_chatData.PlayerId, AddFriendCallBack);
//        }
//        private void AddFriendCallBack(int code,string str,object obj) {
//            CloseWindow(this);
//            ToastBox.ShowText("已发送！");
//        }
//        private void VisitPlayer() {
//            BuildingManager.Instance.VisitPlayer(_data_chatData.PlayerId,Convert.ToBoolean(_data_chatData.IsFriend));
//            CloseWindow(this);            
//        }
//        private void GoToRoom() {
//            CloseWindow(this);
//            ToastBox.ShowText("功能暂未开放");
//        }
//        private void ZanPlayer() {
//            KFriendManager.Instance.C2SZanPlayer(_data_chatData.PlayerId, ZanPlayerCallBack);
//        }
//        private void ZanPlayerCallBack(int code, string str, object obj) {
//            CloseUI();
//            ToastBox.ShowText("已经点赞！");
//        }
//        #endregion
//    }
//}









