//// ***********************************************************************
//// 作用：世界聊天和附近聊天的子物体控制类
////作者：wsy
//// ***********************************************************************
//using Msg.ClientMessage;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;
//using System.Collections.Generic;

//namespace Game.UI
//{
//    public class WorldAndNearbyChatItem : KUIItem, IPointerClickHandler
//    {
//        private Text _txt_time;
//        //self
//        private GameObject _go_self;
//        private KUIImage _img_myHead;
//        private Text _txt_myName;
//		//private Text _txt_myContent;
//		private EmojiText _emojitxt_myContent;
//        private Transform _trs_myBigExpression;
//        private GameObject _go_myPapaw;
//        //other
//        private GameObject _go_other;
//        private KUIImage _img_otherHead;
//        private Text _txt_otherName;
//        //private Text _txt_otherContent;
//        private EmojiText _emjtxt_otherContent;
//        private Button _btn_otherHead;
//        private Transform _trs_otherBigExpression;
//        private GameObject _go_otherPapaw;

//        public WorldChatItem _data_chat { get; private set; }
//        private void Awake()
//        {
//            _txt_time = Find<Text>("Time");

//            _go_self = Find<Transform>("item_self").gameObject;
//            _img_myHead = Find<KUIImage>("item_self/head/Image");
//            _txt_myName = Find<Text>("item_self/Name");
//			//_txt_myContent = Find<Text>("item_self/Image/Text");
//			_emojitxt_myContent = Find<EmojiText>("item_self/Image/EmojiText");
//            _trs_myBigExpression = Find<Transform>("item_self/bigExpression");
//            _go_myPapaw = Find<Transform>("item_self/Image").gameObject;

//            _go_other = Find<Transform>("item_other").gameObject;
//            _img_otherHead = Find<KUIImage>("item_other/head/Image");
//            _txt_otherName = Find<Text>("item_other/Name");
//            //_txt_otherContent = Find<Text>("item_other/Image/Text");
//            _emjtxt_otherContent = Find<EmojiText>("item_other/Image/EmojiText");
//            _btn_otherHead = Find<Button>("item_other/Icon");
//            _trs_otherBigExpression = Find<Transform>("item_other/bigExpression");
//            _go_otherPapaw = Find<Transform>("item_other/Image").gameObject;
//            _btn_otherHead.onClick.AddListener(ClickOherHeadOpenBtnPanel);
//        }
//        #region Method
//        protected override void Refresh()
//        {
//            ShowItem(data as WorldChatItem);
//        }
//        public void ShowItem(WorldChatItem ChatData)
//        {
//            _data_chat = ChatData;
//            for (int i = 0; i < _trs_myBigExpression.childCount; i++)
//            {
//                Destroy(_trs_myBigExpression.GetChild(i).gameObject);
//            }
//            for (int i = 0; i < _trs_otherBigExpression.childCount; i++)
//            {
//                Destroy(_trs_otherBigExpression.GetChild(i).gameObject);
//            }
//            _txt_time.text = ChatData.SendTime.ToString();
//            _go_self.SetActive(ChatData.PlayerId == KUser.SelfPlayer.id);
//            _go_other.SetActive(ChatData.PlayerId != KUser.SelfPlayer.id);
//            if (ChatData.PlayerId == KUser.SelfPlayer.id)
//            {               
//                RefillSelfAssembly();
//            }
//            else {
//                RefillOtherAssembly();
//            }
//            _txt_time.text = K.Extension.TimeExtension.ToDataTime(_data_chat.SendTime).ToLocalTime().ToString("HH:mm");
//            int lastIndex;
//            if (KUIWindow.GetWindow<ChatWindow>().Lst_ChatData.Count > 0)
//            {
//                lastIndex = KUIWindow.GetWindow<ChatWindow>().Lst_ChatData.Count - 1;
//                if (ChatData == KUIWindow.GetWindow<ChatWindow>().Lst_ChatData[lastIndex])
//                {
//                    KUIWindow.GetWindow<ChatWindow>().RefreshUnreadMsgNum(-1);
//                }
//            }
//        }
//        private void RefillSelfAssembly() {
//            _img_myHead.overrideSprite = KIconManager.Instance.GetHeadIcon(_data_chat.PlayerHead);
//            _txt_myName.text = _data_chat.PlayerName;
//            GameObject bigExpressionPrefab = InstantiateSpecialExpression();
//            if (bigExpressionPrefab != null)
//            {
//                _go_myPapaw.SetActive(false);
//                _trs_myBigExpression.gameObject.SetActive(true);
//                bigExpressionPrefab.transform.SetParent(_trs_myBigExpression);
//                bigExpressionPrefab.transform.localScale = Vector3.one;
//                bigExpressionPrefab.transform.localPosition = Vector3.zero;
//            }
//            else {
//                _go_myPapaw.SetActive(true);
//                _trs_myBigExpression.gameObject.SetActive(false);
//                //_txt_myContent.text = System.Text.Encoding.Default.GetString(_data_chat.Content);
//			    _emojitxt_myContent.text = System.Text.Encoding.Default.GetString(_data_chat.Content.ToByteArray());
//            }
//        }

//        private void RefillOtherAssembly() {
//            _img_otherHead.overrideSprite = KIconManager.Instance.GetHeadIcon(_data_chat.PlayerHead);
//            _txt_otherName.text = _data_chat.PlayerName;
//            GameObject bigExpressionPrefab = InstantiateSpecialExpression();
//            if (bigExpressionPrefab != null)
//            {
//                _go_otherPapaw.SetActive(false);
//                _trs_otherBigExpression.gameObject.SetActive(true);
//                bigExpressionPrefab.transform.SetParent(_trs_otherBigExpression);
//                bigExpressionPrefab.transform.localScale = Vector3.one;
//                bigExpressionPrefab.transform.localPosition = Vector3.zero;
//            }
//            else
//            {
//                _go_otherPapaw.SetActive(true);
//                _trs_otherBigExpression.gameObject.SetActive(false);
//                //_txt_otherContent.text = System.Text.Encoding.Default.GetString(_data_chat.Content);
//                _emjtxt_otherContent.text = System.Text.Encoding.Default.GetString(_data_chat.Content.ToByteArray());
//            }
//        }
//        private GameObject InstantiateSpecialExpression() {
//            GameObject voidGo = null;
//            string unidentifiedString = System.Text.Encoding.Default.GetString(_data_chat.Content.ToByteArray());
//            string topTwoWord = string.Empty;
//            if (unidentifiedString.Length > 2)
//            {
//                topTwoWord = unidentifiedString.Substring(0, 2);
//            }
//            if (topTwoWord == KEmojiManager.SpecialExpressionMarker)
//            {
//                string str_exprTitle = unidentifiedString.Substring(2, unidentifiedString.Length - (1 + 2));
//                str_exprTitle = str_exprTitle.Split('_')[0];
//                voidGo = LoadSpecialExpressionPrefab("BigExpressionPrefab/BigExpression_" + str_exprTitle);
//            }
//            return voidGo;
//        }
//        private GameObject LoadSpecialExpressionPrefab(string path)
//        {
//            GameObject bigExpressionPrefab;
//            if (KAssetManager.Instance.TryGetUIPrefab(path, out bigExpressionPrefab))
//            {
//                return Instantiate(bigExpressionPrefab);
//            }
//            else
//            {
//                Debug.Log("[日志] [WorldAndNearbyChatItem] [LoadSpecialExpressionPrefab] 尝试获取过特殊表情的预设体，没有获取到。");
//                return null;
//            }
//        }
//        #endregion
//        #region Interface
//        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
//        {
//        }
//        /// <summary>
//        /// 其他玩家的头像点击事件
//        /// </summary>
//        private void ClickOherHeadOpenBtnPanel() {
//            KUIWindow.OpenWindow<ChatPlayerBoard>(_data_chat);
//        }
//        #endregion
//    }
//}