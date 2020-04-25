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

//namespace Game.UI
//{
//    public class FriendPanelDialogItem : KUIItem, IPointerClickHandler
//    {
//        #region Filed
//        public FriendInfo _FriendData { get; private set; }
//        public FriendRichDialogData _dialogData { get; private set; }

//        private Text _txt_createTime;

//        private GameObject _go_my;
//        private KUIImage _img_myHead;
//        private Text _txt_myName;
//        private EmojiText _emjtxt_myContent;
//        private Text _txt_myContent;
//        private Transform _trs_myBigExpression;
//        private GameObject _go_myPapaw;

//        private KUIImage _img_otherHead;
//        private Text _txt_otherName;
//        private GameObject _go_other;
//        private Text _txt_otherContent;
//        private EmojiText _emjtxt_otherContent;
//        private Transform _trs_otherBigExpression;
//        private GameObject _go_otherPapaw;
//        #endregion
//        #region Method
//        private void Awake()
//        {
//            _txt_createTime = Find<Text>("Time");

//            _go_my = Find<Transform>("item_self").gameObject;
//            _img_myHead = Find<KUIImage>("item_self/head/Image");
//            _txt_myName = Find<Text>("item_self/Name");
//            _txt_myContent = Find<Text>("item_self/Image/Text");
//            _emjtxt_myContent = Find<EmojiText>("item_self/Image/EmojiText");
//            _trs_myBigExpression = Find<Transform>("item_self/bigExpression");
//            _go_myPapaw = Find<Transform>("item_self/Image").gameObject;

//            _go_other = Find<Transform>("item_other").gameObject;
//            _img_otherHead = Find<KUIImage>("item_other/head/Image");
//            _txt_otherName = Find<Text>("item_other/Name");
//            _txt_otherContent = Find<Text>("item_other/Image/Text");
//            _emjtxt_otherContent = Find<EmojiText>("item_other/Image/EmojiText");
//            _trs_otherBigExpression = Find<Transform>("item_other/bigExpression");
//            _go_otherPapaw = Find<Transform>("item_other/Image").gameObject;
//    }
//        #endregion
//        #region Method
//        protected override void Refresh()
//        {
//            ShowItem(data as FriendRichDialogData);
//        }

//        public void ShowItem(FriendRichDialogData DialogData)
//        {
//            if (DialogData == null)// || DialogData.Length == 0)
//            {
//                return;
//            }
//            _dialogData = DialogData;//DialogData[index];
//            Debug.Log("[日志] [FriendPanelDialogItem] [ShowItem] 好友对话，获取新消息：" + _dialogData.DialogContent.ToString() + "玩家ID：" + _dialogData.PlayerID);
//            _triggerTimestamp = _dialogData.CreateTime;
//            _txt_createTime.text = ShowTime;
//            _go_other.SetActive(!(_dialogData.PlayerID == KUser.SelfPlayer.id));
//            _go_my.SetActive(_dialogData.PlayerID == KUser.SelfPlayer.id);
//            if (_dialogData.PlayerID == KUser.SelfPlayer.id)
//            {
//                ShowMyself();
//            }
//            else
//            {
//                ShowOther();
//            }
//        }
//        private void ShowMyself() {
//            _img_myHead.overrideSprite = KIconManager.Instance.GetHeadIcon(KUser.SelfPlayer.headURL);
//            _txt_myName.text = KUser.SelfPlayer.nickName;
//            //_txt_myContent.text = System.Text.Encoding.Default.GetString(_dialogData.DialogContent);
//            //_emjtxt_myContent.text = System.Text.Encoding.Default.GetString(_dialogData.DialogContent);
//            GameObject bigExpressionPrefab = InstantiateSpecialExpression();
//            if (bigExpressionPrefab != null)
//            {
//                _go_myPapaw.SetActive(false);
//                _trs_myBigExpression.gameObject.SetActive(true);
//                bigExpressionPrefab.transform.SetParent(_trs_myBigExpression);
//                bigExpressionPrefab.transform.localScale = Vector3.one;
//                bigExpressionPrefab.transform.localPosition = Vector3.zero;
//            }
//            else
//            {
//                _go_myPapaw.SetActive(true);
//                _trs_myBigExpression.gameObject.SetActive(false);
//                _txt_myContent.text = System.Text.Encoding.Default.GetString(_dialogData.DialogContent);
//                _emjtxt_myContent.text = System.Text.Encoding.Default.GetString(_dialogData.DialogContent);
//            }
//        }
//        private void ShowOther() {
//            for (int i = 0; i < KFriendManager.Instance.Arry_Friends.Length; i++)
//            {
//                if (KFriendManager.Instance.Arry_Friends[i].PlayerId == _dialogData.PlayerID)
//                {
//                    _FriendData = KFriendManager.Instance.Arry_Friends[i];
//                }
//            }
//            if (_FriendData != null)
//            {
//                _img_otherHead.overrideSprite = KIconManager.Instance.GetHeadIcon(_FriendData.Head);
//                _txt_otherName.text = _FriendData.Name;
//                //_txt_otherContent.text = System.Text.Encoding.Default.GetString(_dialogData.DialogContent);
//                //_emjtxt_otherContent.text = System.Text.Encoding.Default.GetString(_dialogData.DialogContent);
//                GameObject bigExpressionPrefab = InstantiateSpecialExpression();
//                if (bigExpressionPrefab != null)
//                {
//                    _go_otherPapaw.SetActive(false);
//                    _trs_otherBigExpression.gameObject.SetActive(true);
//                    bigExpressionPrefab.transform.SetParent(_trs_otherBigExpression);
//                    bigExpressionPrefab.transform.localScale = Vector3.one;
//                    bigExpressionPrefab.transform.localPosition = Vector3.zero;
//                }
//                else
//                {
//                    _go_otherPapaw.SetActive(true);
//                    _trs_otherBigExpression.gameObject.SetActive(false);
//                    _txt_otherContent.text = System.Text.Encoding.Default.GetString(_dialogData.DialogContent);
//                    _emjtxt_otherContent.text = System.Text.Encoding.Default.GetString(_dialogData.DialogContent);
//                }
//            }
//        }
//        private GameObject InstantiateSpecialExpression()
//        {
//            GameObject voidGo = null;
//            string unidentifiedString = System.Text.Encoding.Default.GetString(_dialogData.DialogContent);
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
//                Debug.Log("[日志] [WorldAndNearbyChatItem] [LoadSpecialExpressionPrefab] 疑似尝试获取过特殊表情的预设体，没有获取到。");
//                return null;
//            }
//        }



//        #endregion
//        #region Interface
//        void IPointerClickHandler.OnPointerClick(PointerEventData eventData){}
//        #endregion
//        #region 时间
//        private int _triggerTimestamp;
//        private int triggerTime
//        {
//            get { return _triggerTimestamp - KLaunch.Timestamp; }
//            set { _triggerTimestamp = KLaunch.Timestamp + value; }
//        }
//        private string ShowTime
//        {
//            get { return K.Extension.TimeExtension.ToDataTime(_triggerTimestamp).ToLocalTime().ToString("HH:mm"); }
//        }
//        #endregion
//    }
//}