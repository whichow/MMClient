//// ***********************************************************************
//// 作用：世界聊天和附近聊天的子物体控制类
////作者：wsy
//// ***********************************************************************
//using Game.Build;
//using Msg.ClientMessage;
//using System;
//using System.Text.RegularExpressions;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;

//namespace Game.UI
//{
//    public class SystemChatItem : KUIItem//, IPointerClickHandler
//    {
//        private EmojiText _txt_content;

//        public const string str_wildCard = "#P";
//        public AnouncementItem _data_chat { get; private set; }
//        private string _str_acnmtContent;
//        private void Awake()
//        {
//            _txt_content = Find<EmojiText>("Image/Text");
//            _txt_content.onHrefClick = ClickHrefWord;
//        }
//        #region Method
//        protected override void Refresh()
//        {
//            ShowItem(data as AnouncementItem);
//        }
//        public void ShowItem(AnouncementItem systemData)
//        {
//            _data_chat = systemData;
//            _str_acnmtContent = KLocalization.GetLocalString(KChatManager.Instance.Dict_SystemAccouncementCfg[systemData.MsgType].description);
//            string[] SubstringBywildCard = Regex.Split(_str_acnmtContent,str_wildCard);
//            string _str_hrefContent = string.Empty;
//            if (SubstringBywildCard[1] != string.Empty)
//            {
//                _str_hrefContent = SubstringBywildCard[1];
//                _str_hrefContent = _str_hrefContent.Replace(" ", "");
//                _str_hrefContent = "<a href='xx'>" + _str_hrefContent + "</a>";
//            }
//            SubstringBywildCard[0] = SubstringBywildCard[0].Replace(" ", "");
//            string subjectName = "<color=#02ac88>" + _data_chat.PlayerName + "</color>";
//            string objectName = "<color=#ff5c01>" + KChatManager.FillobjectName(_data_chat) + "</color>";
//            _txt_content.text = string.Format(SubstringBywildCard[0], subjectName, objectName) + _str_hrefContent;
//            int lastIndex;
//            if (KUIWindow.GetWindow<ChatWindow>().Lst_anmtsDatas.Count > 0)
//            {
//                lastIndex = KUIWindow.GetWindow<ChatWindow>().Lst_anmtsDatas.Count - 1;
//                if (systemData == KUIWindow.GetWindow<ChatWindow>().Lst_anmtsDatas[lastIndex])
//                {
//                    KUIWindow.GetWindow<ChatWindow>().RefreshUnreadMsgNum(-1);
//                }
//            }
//            //_txt_content.
//        }
//        #endregion
//        #region Interface
//        //void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
//        //{
//        //}
//        private void ClickHrefWord(string value) {
//            if (value == "'xx'")
//            {
//                Debug.Log("施工中..." + value);
//            }
//            switch (_data_chat.MsgType)
//            {
//                //1 获得4/5星寄养卡  2 获得4阶装饰物  3 获得4阶装饰物配方  4 获得SSR猫  5 排行榜首位  6 猫满级  7 纯文本）
//                case 1:
//                    KFriendManager.Instance.AddFriend(_data_chat.PlayerId, AddFriendCallBack);
//                    break;
//                case 2:
//                    KUIWindow.CloseWindow<ChatWindow>();
//                    KUIWindow.OpenWindow<ShopWindow>(ShopWindow.ShopType.CharmPack);
//                    break;
//                case 3:
//                    if (BuildingManager.Instance.isExistBuilding(Building.Category.kManualWorkShop))
//                    {
//                        KUIWindow.CloseWindow<ChatWindow>();
//                        KUIWindow.OpenWindow<FormulaShopWindow>();
//                    }
//                    else {
//                        ToastBox.ShowText("还没有手工作坊！");
//                    }
//                    break;
//                case 4:
//                    KUIWindow.CloseWindow<ChatWindow>();
//                    KUIWindow.OpenWindow<PhotoShopWindow>();
//                    break;
//                case 5:
//                    break;
//                case 6:
//                    break;
//                case 7:
//                    break;
//                default:
//                    break;
//            }
//        }
//        private void AddFriendCallBack(int code, string str, object obj)
//        {
//            Debug.Log("发送加好友通知，对方ID：" + KFriendManager.Instance.LastAddFriendID);
//            if (code >= 0)
//            {
//                ToastBox.ShowText("已发送！");
//            }
//        }


//        #endregion
//    }
//}