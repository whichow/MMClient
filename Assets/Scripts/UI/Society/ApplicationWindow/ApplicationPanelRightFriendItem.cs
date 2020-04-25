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
//    public class ApplicationPanelRightFriendItem : KUIItem, IPointerClickHandler
//    {
//        #region Filed        
//        private Text _txt_name;
//        private Image _img_icon;
//        private Text _txt_lv;
//        private Text _txt_lastIime;
//        private KUIImage _img_interact;
//        private Button _btn_interact;

//        private Button _btn_agree;
//        private Button _btn_ignore;

//        private FriendReq _FriendData;
//        #endregion
//        #region Method
//        private void Awake()
//        {
//            _txt_name = Find<Text>("Name");
//            _img_icon = Find<Image>("Head/head/Image");
//            _txt_lv = Find<Text>("Head/Icon/Level/Text");
//            _txt_lastIime = Find<Text>("Time");
//            _img_interact = Find<KUIImage>("btn_interact");
//            _btn_interact = Find<Button>("btn_interact");

//            _btn_agree = Find<Button>("btn_agree");
//            _btn_agree.onClick.AddListener(AgreeFriendApplication);
//            _btn_ignore = Find<Button>("btn_ignore");
//            _btn_ignore.onClick.AddListener(IgnoreFriendApplication);
//        }
//        #endregion
//        #region Method
//        public void ShowItem(FriendReq[] Frds, int index)
//        {
//            _btn_agree.gameObject.GetComponent<Image>().material = null;
//            _btn_agree.enabled = true;
//            _btn_ignore.gameObject.GetComponent<Image>().material = null;
//            _btn_ignore.enabled = true;

//            if (Frds == null || Frds.Length == 0)
//            {
//                return;
//            }
//            var FD = Frds[index];
//            _FriendData = FD;
//            _txt_name.text = _FriendData.Name;
//            _img_icon.overrideSprite = KIconManager.Instance.GetHeadIcon(_FriendData.Head);
//        }
//        #endregion
//        #region Interface
//        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
//        {


//        }
//        //同意好友请求
//        private void AgreeFriendApplication() {
//            KFriendManager.Instance.C2SAgreeFriend(_FriendData.PlayerId, AgreeCallBack);
//        }
//        //同意之后的响应
//        private void AgreeCallBack(int code,string str,object obj) {
//            //按键置灰，失效
//            _btn_agree.gameObject.GetComponent<Image>().material = Resources.Load<Material>("Materials/UIGray");
//            _btn_agree.enabled = false;
//            _btn_ignore.gameObject.GetComponent<Image>().material = Resources.Load<Material>("Materials/UIGray");
//            _btn_ignore.enabled = false;

//            KUIWindow.GetWindow<FriendWindow>().RefreshApplicationRight();
//        }
//        //忽略好友请求
//        private void IgnoreFriendApplication() {
//            KFriendManager.Instance.C2SRefuseFriend(_FriendData.PlayerId, AgreeCallBack);
//        }
//        #endregion
//    }
//}