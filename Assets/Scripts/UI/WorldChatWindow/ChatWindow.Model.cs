//using Msg.ClientMessage;
//using System.Collections.Generic;
///** 
// * 作用：世界聊天的界面数据控制类
// * 作者：wsy
//*/
//namespace Game.UI
//{
//    public partial class ChatWindow
//    {
//        public enum PageType
//        {
//            kSystemChat = 1,
//            kWorldChat = 2,
//            kNearbyChat = 3,
//        }
//        public PageType CurrentPageType { get; private set; }

//        private void InitializationModel() {

//        }
//        private void ResetModel() {
//            CurrentPageType = KChatManager.Instance.LastOpenPageType;
//            foreach (var item in _dict_tglAndPgtp)
//            {
//                if (item.Value.Equals(CurrentPageType))
//                {
//                    item.Key.isOn = true;
//                }
//            }
//        }
//        private PageType GetOnToggle() {
//            foreach (var toggle in _lstTgl_channel)
//            {
//                if (toggle.isOn)
//                {
//                    return _dict_tglAndPgtp[toggle];
//                }
//            }
//            return PageType.kSystemChat;
//        }
//    }
//}

