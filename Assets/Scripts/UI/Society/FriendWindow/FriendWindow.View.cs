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
//using DG.Tweening;
//using Game.Build;
//using Msg.ClientMessage;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using UnityEngine.UI;

//namespace Game.UI
//{
//    public partial class FriendWindow
//    {
//        #region Field
//        private const int _int_deltaTime = 1;
//        private const int _int_limiteTime = 5;


//        private int _int_currentTime;

//        private Button _quitBtn;

//        private ToggleGroup _tglGp_Top;
//        private List<Toggle> _rarityToggles;
//        private Toggle _tgl_friend;
//        private Toggle _tgl_application;
//        private Toggle _tgl_room;
//        private GameObject _trans_friend;
//        private GameObject _trans_application;
//        private GameObject _trans_room;
//        [HideInInspector]
//        public List<FriendPanelFriendItem> LstFrd { get; private set; }
//        //好友部分
//        private KUIItemPool _layoutElementPool_friend;
//        private Text _txt_friendNum;
//        private Button _btn_getAll;
//        private Image _img_getAll;
//        private Button _btn_presentAll;
//        private Image _img_presentAll;
//        private GameObject _go_rightHead;
//        private KUIImage _img_talkerIcon;
//        private Text _txt_talkerLv;
//        private Text _txt_talkerName;
//        private Text _txt_thumbUpNum;
//        private Button _btn_thumbUp;
//        private KUIImage _img_thumbup;
//        private Button _btn_delete;
//        private Button _btn_Jiyangsuo;
//        private Button _btn_room;
//        private Button _btn_visit;
//        private Button _btn_voice;
//        private InputField _input_mychat;
//        private Text _txt_inputDefault;
//        private Button _btn_emoj;
//        private Button _btn_send;
//        private KUIItemPool _layoutElementPool_dialog;
//        private ScrollRect _scRct_dialog;
//        public List<FriendRichDialogData> CurrentDialogLst { get; private set; }
//        private KUIGrid _kgird_dialog;

//        //添加部分
//        private InputField _btn_input_application;
//        private Text _txt_defaultString;
//        private Button _btn_clear;
//        private Button _btn_search;
//        private KUIItemPool _layoutElementPool_nearby;
//        private KUIItemPool _layoutElementPool_application;
//        private GameObject _go_showNoApplication;

//        private Button _btn_VX;
//        private Button _btn_QQ;
//        private Button _btn_FB;
//        //空间部分
//        private KUIImage _img_roomOwnerIcon;
//        private Text _txt_rmOwnerLv;
//        private Text _txt_rmOwnerName;
//        private Text _txt_rmOwnerSignature;
//        private Text _txt_rmthumbUpNum;

//        private KUIItemPool _layoutElementPool_picture;
//        #endregion

//        #region Method

//        private void InitView()
//        {
//            _quitBtn = Find<Button>("close");
//            _quitBtn.onClick.AddListener(this.OnQuitBtnClick);
//            _tglGp_Top = Find<ToggleGroup>("Tab View/ToggleGroup");
//            _tgl_friend = Find<Toggle>("Tab View/ToggleGroup/tgl_friend");
//            _tgl_application = Find<Toggle>("Tab View/ToggleGroup/tgl_application");
//            _tgl_room = Find<Toggle>("Tab View/ToggleGroup/tgl_room");
//            _rarityToggles = new List<Toggle>(_tglGp_Top.GetComponentsInChildren<Toggle>());
//            for (int i = 0; i < _rarityToggles.Count; i++)
//            {
//                _rarityToggles[i].onValueChanged.AddListener(this.OnPageChange);
//            }
//            _trans_friend = Find<Transform>("Friend").gameObject;
//            _trans_application = Find<Transform>("Aplication").gameObject;
//            _trans_room = Find<Transform>("Room").gameObject;
//            //好友部分
//            _layoutElementPool_friend = Find<KUIItemPool>("Friend/Left/Scroll View");
//            if (_layoutElementPool_friend && _layoutElementPool_friend.elementTemplate)
//            {
//                _layoutElementPool_friend.elementTemplate.gameObject.AddComponent<FriendPanelFriendItem>();
//            }
//            _txt_friendNum = Find<Text>("Friend/Left/Num/Text");
//            _img_presentAll = Find<Image>("Friend/Left/btn_present");
//            _btn_presentAll = Find<Button>("Friend/Left/btn_present");
//            _img_getAll = Find<Image>("Friend/Left/btn_get");
//            _btn_getAll = Find<Button>("Friend/Left/btn_get");
          
//            _go_rightHead = Find<Transform>("Friend/Right/Top/Head").gameObject;
//            _img_talkerIcon = Find<KUIImage>("Friend/Right/Top/Head/head/Image");
//            _txt_talkerLv = Find<Text>("Friend/Right/Top/Head/Level/Text");
//            _txt_talkerName = Find<Text>("Friend/Right/Top/Head/Name");
//            _txt_thumbUpNum = Find<Text>("Friend/Right/Top/Head/Image/Text");
//            _btn_thumbUp = Find<Button>("Friend/Right/Top/Head/Image");
//            _btn_thumbUp.onClick.AddListener(this.ThumbUpFriend);
//            _img_thumbup = Find<KUIImage>("Friend/Right/Top/Head/Image/Image");
//            _btn_delete = Find<Button>("Friend/Right/Top/Operate/delete");
//            _btn_delete.onClick.AddListener(this.ClickDeleteFriend);
//            _btn_Jiyangsuo = Find<Button>("Friend/Right/Top/Operate/Jiyangsuo");
//            _btn_Jiyangsuo.onClick.AddListener(this.ClickJiyangsuo);
//            _btn_room = Find<Button>("Friend/Right/Top/Operate/room");
//            _btn_room.onClick.AddListener(this.ClickRoom);
//            _btn_visit = Find<Button>("Friend/Right/Top/Operate/visite");
//            _btn_visit.onClick.AddListener(this.ClickVisit);
//            _btn_voice = Find<Button>("Friend/Right/Bottom/btn_sound");
//            _btn_voice.onClick.AddListener(this.ClickVoice);
//            _input_mychat = Find<InputField>("Friend/Right/Bottom/btn_input");
//            _txt_inputDefault = Find<Text>("Friend/Right/Bottom/btn_input/Placeholder");
//            _txt_inputDefault.text = KLocalization.GetLocalString(53047);
//            ClearDialogInputField();
//            _btn_emoj = Find<Button>("Friend/Right/Bottom/btn_emoj");
//            _btn_emoj.onClick.AddListener(this.ClickEmoj);
//            _btn_send = Find<Button>("Friend/Right/Bottom/btn_send");
//            _btn_send.onClick.AddListener(this.ClickSend);
//            _layoutElementPool_dialog = Find<KUIItemPool>("Friend/Right/Scroll View");
//            _scRct_dialog = Find<ScrollRect>("Friend/Right/Scroll View");
//            _kgird_dialog = Find<KUIGrid>("Friend/Right/Scroll View");
//            if (_kgird_dialog)
//            {
//                _kgird_dialog.uiPool.itemTemplate.AddComponent<FriendPanelDialogItem>();
//            }
//            if (_layoutElementPool_dialog && _layoutElementPool_dialog.elementTemplate)
//            {
//            }
//            //添加部分
//            _btn_input_application = Find<InputField>("Aplication/input_search");
//            _txt_defaultString = Find<Text>("Aplication/input_search/Placeholder");
//            _txt_defaultString.text = KLocalization.GetLocalString(53075);
//            _btn_clear = Find<Button>("Aplication/input_search/clear");
//            _btn_clear.onClick.AddListener(ClickClearInput);
//            _btn_search = Find<Button>("Aplication/btn_search");
//            _btn_search.onClick.AddListener(ClickSearch);
//            _layoutElementPool_nearby = Find<KUIItemPool>("Aplication/Left/Scroll View");
//            if (_layoutElementPool_nearby && _layoutElementPool_nearby.elementTemplate)
//            {
//                _layoutElementPool_nearby.elementTemplate.gameObject.AddComponent<ApplicationPanelLeftFriendItem>();
//            }
//            _layoutElementPool_application = Find<KUIItemPool>("Aplication/Right/Scroll View");
//            if (_layoutElementPool_application && _layoutElementPool_application.elementTemplate)
//            {
//                _layoutElementPool_application.elementTemplate.gameObject.AddComponent<ApplicationPanelRightFriendItem>();
//            }
//            _go_showNoApplication = Find<Transform>("Aplication/Right/Empty").gameObject;
//            _btn_VX = Find<Button>("Aplication/Share/Wechat");
//            _btn_QQ = Find<Button>("Aplication/Share/QQ");
//            _btn_FB = Find<Button>("Aplication/Share/Facebook");
//            //空间部分
//        }
//        public override void UpdatePerSecond()
//        {
//            base.UpdatePerSecond();
//            if (_int_currentTime >= _int_limiteTime)
//            {
//                _int_currentTime = 0;
//                if (pageType == PageType.kFriend)
//                {
//                    //更新好友列表在线状态
//                    //更新好友列表未读消息数量
//                    if (LstFrd != null)
//                    {                        
//                        List<int> allOnlineFrdIDs = new List<int>();
//                        for (int i = 0; i < KFriendManager.Instance.Arry_Friends.Length; i++)
//                        {
//                            Debug.Log("[[UpdatePerSecond]断点：该玩家在线状况]" + KFriendManager.Instance.Arry_Friends[i].IsOnline);
//                            allOnlineFrdIDs.Add(KFriendManager.Instance.Arry_Friends[i].PlayerId);
//                        }
//                        //不在此处判空，为在mgr中清除旧数据
//                        KFriendManager.Instance.C2SFriendGetUnreadMessageNum(allOnlineFrdIDs, C2SFriendGetUnreadMessageNumCallBack);                       
//                    }
//                }
//                if (KFriendManager.Instance.CurrentSelectFriend != null)
//                {
//                    //拉一次当前选中玩家聊天
//                    KFriendManager.Instance.C2SFriendPullUnreadMessage(KFriendManager.Instance.CurrentSelectFriend._FriendData.PlayerId, AddFriendDialog);
//                }
//            }
//            else {
//                _int_currentTime = _int_currentTime + _int_deltaTime;
//            }
//        }
//        private void GetFriendsCallBack(int code, string str, object obj)
//        {
//            ResetToggle();
//            RefreshView();
//        }
//        /// <summary>
//        /// 根据页签初始化当前页签数据
//        /// </summary>
//        private void RefreshView()
//        {
//            _trans_friend.SetActive(pageType == PageType.kFriend);
//            _tgl_friend.isOn = (pageType == PageType.kFriend);
//            _trans_application.SetActive(pageType == PageType.kApplication);
//            _tgl_application.isOn = (pageType == PageType.kApplication);
//            _trans_room.SetActive(pageType == PageType.kRoom);
//            _tgl_room.isOn = (pageType == PageType.kRoom);
//            switch (pageType)
//            {
//                case PageType.kFriend:
//                    RefreshFriendView();
//                    break;
//                case PageType.kApplication:
//                    RefreshApplication();
//                    break;
//                case PageType.kRoom:
//                    RefreshRoomView();
//                    break;
//                default:
//                    break;
//            }
//        }
//        /// <summary>
//        /// 第一个页签初始化方法：好友列表
//        /// </summary>
//        private void RefreshFriendView()
//        {
//            RefreshFriendLeft();
//            KFriendManager.Instance.SelectFriendPanelFriend(null);
//        }
//        /// <summary>
//        /// 第二个页签初始化方法：搜索和申请列表
//        /// </summary>
//        private void RefreshApplication()
//        {
//            ResetInputLabel();
//            RefreshApplicationLeft();
//            RefreshApplicationRight();
//        }
//        /// <summary>
//        /// 第三个页签的初始化方法：个人空间
//        /// </summary>
//        private void RefreshRoomView()
//        {
//        }
//        /// <summary>
//        /// 获取当前页签的文字名称
//        /// </summary>
//        /// <returns></returns>
//        private string GetOnToggle()
//        {
//            foreach (var toggle in _rarityToggles)
//            {
//                if (toggle.isOn)
//                {
//                    return toggle.name;
//                }
//            }
//            return null;
//        }
//        #endregion
//        #region 好友
//        /// <summary>
//        /// 生成好友列表
//        /// </summary>
//        private void RefreshFriendLeft() {
//            _txt_friendNum.text = "好友数量：" + KFriendManager.Instance.Arry_Friends.Length.ToString() + "/" + "200";
//            StartCoroutine(FillElements_Friend());
//            RefreshLeftBottomBtn();
//        }
//        /// <summary>
//        /// 未读消息数量更新
//        /// </summary>
//        private void RefreshAllFrdUnMsgNum() {
//            for (int i = 0; i < LstFrd.Count; i++)
//            {
//                LstFrd[i].RefreshUnMsgNum();
//            }
//        }
//        /// <summary>
//        /// 向服务器拉取所有好友未读消息的回调
//        /// </summary>
//        /// <param name="code"></param>
//        /// <param name="str"></param>
//        /// <param name="obj"></param>
//        private void C2SFriendGetUnreadMessageNumCallBack(int code,string str,object obj) {
//            Debug.Log("[回调：服务器反馈未读消息数量]");
//            RefreshAllFrdUnMsgNum();
//        }
//        /// <summary>
//        /// 按键数据更新
//        /// </summary>
//        public void RefreshLeftBottomBtn() {
//            //群赠
//            _btn_presentAll.onClick.RemoveAllListeners();
//            if (KFriendManager.Instance.IntArray_PresentTargetIDs.Count > 0)
//            {
//                _img_presentAll.material = null;
//                _btn_presentAll.onClick.AddListener(this.ClickPresentAll);
//            }
//            else {
//                _img_presentAll.material = Resources.Load<Material>("Materials/UIGray");
//                _btn_presentAll.onClick.AddListener(this.ClickPresentAllWhenDisable);
//            }
//            //群收
//            int SendMeNum = 0;
//            foreach (FriendInfo item in KFriendManager.Instance.Arry_Friends)
//            {
//                if (item.FriendPoints > 0)
//                {
//                    SendMeNum++;
//                }
//            }
//            if (SendMeNum != 0)
//            {
//                _img_getAll.material = null;
//                _btn_getAll.onClick.AddListener(this.ClickGetAll);
//            }
//            else {
//                _img_getAll.material = Resources.Load<Material>("Materials/UIGray");
//                _btn_getAll.onClick.AddListener(this.ClickGetAllWhenDisable);
//            }
//        }
//        private void ClickPresentAll() {
//            KFriendManager.Instance.C2SGiveFriendPoints(KFriendManager.Instance.IntArray_PresentTargetIDs, ClickPresentAllCallBack);
//        }
//        private void ClickPresentAllWhenDisable() {
//            List<string> strLst_reasons = new List<string>();
//            //次数不够
//            if (KFriendManager.Instance.LeftPresentNum <= 0)
//            {
//                strLst_reasons.Add(KLocalization.GetLocalString(53044));
//            }
//            //当前好友列表的好友都已不可赠送
//            if (KFriendManager.Instance.LeftPresentNum > 0 && KFriendManager.Instance.Arry_Friends.Length > 0)
//            {
//                strLst_reasons.Add(KLocalization.GetLocalString(53045));
//            }
//            if (strLst_reasons.Count > 0)
//            {
//                ToastBox.ShowText(strLst_reasons[0]);
//            }
//        }
//        private void ClickGetAll()
//        {
//            List<int> _intLst_FrdIDs = new List<int>();
//            foreach (FriendInfo item in KFriendManager.Instance.Arry_Friends)
//            {
//                if (item.FriendPoints > 0)
//                {
//                    _intLst_FrdIDs.Add(item.PlayerId);
//                }
//            }
//            KFriendManager.Instance.C2SGetFriendPoints(_intLst_FrdIDs, ClickGetAllCallBack);
//        }
//        private void ClickGetAllWhenDisable()
//        {
//            ToastBox.ShowText(KLocalization.GetLocalString(53046));
//        }
//        private void ClickGetAllCallBack(int code,string str,object obj) {
//            for (int i = 0; i < KFriendManager.Instance.FrdPsLst_getAllPoints.Count; i++)
//            {
//                if (KFriendManager.Instance.FrdPsLst_getAllPoints[i].Points > 0)
//                {                    
//                    ToastBox.ShowText(string.Format(KLocalization.GetLocalString(53002), KFriendManager.Instance.FrdPsLst_getAllPoints[i].Points));
//                }
//            }
//            for (int i = 0; i < LstFrd.Count; i++)
//            {
//                LstFrd[i].RefreshInteractStatus();
//            }
//            RefreshLeftBottomBtn();
//        }
//        public void InitializationFriendRight() {
//            FriendPanelFriendItem targetdata = KFriendManager.Instance.CurrentSelectFriend;
//            if (targetdata == null)
//            {
//                ClearFriendRight();
//            }
//            else {
//                _go_rightHead.SetActive(true);
//                _img_talkerIcon.overrideSprite = KIconManager.Instance.GetHeadIcon(targetdata._FriendData.Head);
//                _txt_talkerLv.text = targetdata._FriendData.Level.ToString();
//                _txt_talkerName.text = targetdata._FriendData.Name;
//                _txt_thumbUpNum.text = targetdata._FriendData.Zan.ToString();
//                RefreshThumbUp();
//            }
//        }
//        /// <summary>
//        /// 点赞动作
//        /// </summary>
//        private void ThumbUpFriend() {
//            if (!KFriendManager.Instance.CurrentSelectFriend._FriendData.IsZanToday)
//            {
//                KFriendManager.Instance.C2SZanPlayer(KFriendManager.Instance.CurrentSelectFriend._FriendData.PlayerId, ThumbUpFriendCallBack);
//            }
//            else {
//                ToastBox.ShowText(KLocalization.GetLocalString(55002));
//            }
//        }
//        private void ThumbUpFriendCallBack(int code,string str,object obj) {
//            RefreshThumbUp();
//        }
//        /// <summary>
//        /// 刷新右侧点赞按键
//        /// </summary>
//        private void RefreshThumbUp() {
//            FriendPanelFriendItem targetdata = KFriendManager.Instance.CurrentSelectFriend;
//            if (targetdata._FriendData.IsZanToday)
//            {
//                _img_thumbup.overrideSprite = _img_thumbup.sprites[1];
//            }
//            else {
//                _img_thumbup.overrideSprite = _img_thumbup.sprites[0];
//            }
//            _txt_thumbUpNum.text = targetdata._FriendData.Zan.ToString();
//        }
//        /// <summary>
//        /// 初始化聊天框头像信息
//        /// </summary>
//        private void ClearFriendRight() {
//            _go_rightHead.SetActive(false);
//            //KFriendManager.Instance.SelectFriendPanelFriend(null);
//        }
//        private void ClearDialog() {
//            List<FriendRichDialogData> emptyLst = new List<FriendRichDialogData>();
//            _kgird_dialog.uiPool.SetItemDatas(emptyLst);
//            _kgird_dialog.ClearItems();
//        }
//        private void ClearDialogInputField() {
//            _input_mychat.text = string.Empty;
//        }
//        public void InitializationFriendDialog()
//        {
//            FriendPanelFriendItem targetdata = KFriendManager.Instance.CurrentSelectFriend;
//            if (targetdata == null)
//            {
//                ClearDialog();
//            }
//            else
//            {
//                KFriendManager.Instance.C2SFriendPullUnreadMessage(KFriendManager.Instance.CurrentSelectFriend._FriendData.PlayerId, GetOldAndNewFriendDialogCallBack);
//            }
//            ClearDialogInputField();
//        }
//        public void GetOldAndNewFriendDialogCallBack(int code,string str,object obj) {            
//            //重置一次向服务器拉取消息的时间：避开在生成旧对话的过程中拉取服务器消息可能造成的冲突
//            _int_currentTime = 0;
//            CreateFriendDialog();
//        }
//        private void CreateFriendDialog()
//        {
//            _kgird_dialog.ClearItems();
//            CurrentDialogLst = new List<FriendRichDialogData>();
//            if (KFriendManager.Instance.FriendDialog.ContainsKey(KFriendManager.Instance.CurrentSelectFriend._FriendData.PlayerId))
//            {
//                CurrentDialogLst = KFriendManager.Instance.FriendDialog[KFriendManager.Instance.CurrentSelectFriend._FriendData.PlayerId];
//            }
//            ConcreteCreateFriendDialog(CurrentDialogLst);
//        }
//        private void AddFriendDialog(int code, string str, object obj)
//        {
//            var Lstdialog = new List<FriendRichDialogData>();
//            if (KFriendManager.Instance.FriendDialog.ContainsKey(KFriendManager.Instance.CurrentSelectFriend._FriendData.PlayerId))
//            {
//                Lstdialog = KFriendManager.Instance.CurrentFriendNewAddDialog[KFriendManager.Instance.CurrentSelectFriend._FriendData.PlayerId];
//            }
//            if (Lstdialog != null && Lstdialog.Count != 0)
//            {
//                ConcreteAddFriendDialog(Lstdialog);
//            }
//        }
//        /// <summary>
//        /// 生成对话的具体实现
//        /// </summary>
//        /// <returns></returns>
//        private void ConcreteCreateFriendDialog(List<FriendRichDialogData> value)
//        {
//            FriendRichDialogData[] _dialogArray = value.ToArray();
//            _kgird_dialog.uiPool.SetItemDatas(_dialogArray);
//            _kgird_dialog.RefillItems();
//        }
//        /// <summary>
//        /// 补充对话的具体实现
//        /// </summary>
//        /// <returns></returns>
//        private void ConcreteAddFriendDialog(List<FriendRichDialogData> value)
//        {
//            FriendRichDialogData[] _dialogArray = value.ToArray();
//            _kgird_dialog.uiPool.AddItemDatas(_dialogArray);
//            int count = _kgird_dialog.uiPool.itemCount;
//            _kgird_dialog.RefillItems(count - 1);
//        }
//        private IEnumerator FillElements_Friend()
//        {
//            _layoutElementPool_friend.Clear();
//            LstFrd = new List<FriendPanelFriendItem>();
//            var friends = GetFriendArry();
//            var elements = _layoutElementPool_friend.SpawnElements(friends.Length);
//            for (int i = 0; i < friends.Length; i++)
//            {
//                var element = elements[i];
//                var friendItem = element.GetComponent<FriendPanelFriendItem>();
//                LstFrd.Add(friendItem);
//                friendItem.ShowItem(friends, i);
//            }
//            yield return null;
//        }
//        private void ClickPresentAllCallBack(int code,string str,object obj) {
//            FriendPointsResult[] cbackDatas = KFriendManager.Instance.Array_FriendPointsResult;
//            for (int i = 0; i < cbackDatas.Length; i++)
//            {//0 成功  1 好友不存在  2 上次赠送的对方还未收取  3 CD时间未到
//                string msgForShow;
//                switch (cbackDatas[i].Error)
//                {
//                    case 0:
//                        msgForShow = KLocalization.GetLocalString(53048);
//                        break;
//                    case 1:
//                        msgForShow = KLocalization.GetLocalString(53049);
//                        break;
//                    case 2:
//                        msgForShow = string.Empty;//"上次赠送的对方还未收取";
//                        break;
//                    case 3:
//                        msgForShow = KLocalization.GetLocalString(53050);
//                        break;
//                    default:
//                        msgForShow = string.Empty;
//                        break;
//                }
//                //ToastBox.ShowText(msgForShow);
//            }
//            for (int i = 0; i < LstFrd.Count; i++)
//            {
//                LstFrd[i].RefreshInteractStatus();
//            }
//            if (KFriendManager.Instance.PresentRewardPoints > 0)
//            {
//                ToastBox.ShowText("赠送成功，获得奖励" + KFriendManager.Instance.PresentRewardPoints + "点");
//            }
//            KFriendManager.Instance.ClearOldPresentRewardPoints();
//            RefreshLeftBottomBtn();
//        }
//        private void ClickDeleteFriend() {
//            if (KFriendManager.Instance.CurrentSelectFriend == null)
//            {
//            }
//            else {
//                UI.MessageBox.ShowMessage(KLocalization.GetLocalString(53051),KLocalization.GetLocalString(53052), () =>
//                {
//                    KFriendManager.Instance.DeleteFriend(KFriendManager.Instance.CurrentSelectFriend._FriendData.PlayerId, DeleteFriendCallBack);
//                },()=>{ });
//            }
//        }
//        private void DeleteFriendCallBack(int code,string str,object obj) {
//            RefreshFriendLeft();
//            KFriendManager.Instance.SelectFriendPanelFriend(null);
//            ClearFriendRight();
//            ClearDialog();
//        }
//        private void ClickJiyangsuo() {
//            if (KFriendManager.Instance.CurrentSelectFriend != null)
//            {
//                AdoptionWindow.OpenAdoption(KFriendManager.Instance.CurrentSelectFriend._FriendData.PlayerId);
//            }
//        }
//        private void ClickRoom() {

//        }
//        private void ClickVisit() {
//            if (KFriendManager.Instance.CurrentSelectFriend != null)
//            {
//                BuildingManager.Instance.VisitPlayer(KFriendManager.Instance.CurrentSelectFriend._FriendData.PlayerId, true);
//                CloseWindow(this);
//            }
//        }
//        private void ClickVoice() {

//        }
//        private void ClickEmoj() {
//            KUIWindow.OpenWindow<ExpressionBoard>();
//        }
//        private void ClickSend() {
//            byte[] contents = System.Text.Encoding.Default.GetBytes(_input_mychat.text);
//            if (KFriendManager.Instance.CurrentSelectFriend != null)
//            {
//                if (contents.Length <= 0)
//                {
//                }
//                else {
//                    KFriendManager.Instance.C2SFriendChat(KFriendManager.Instance.CurrentSelectFriend._FriendData.PlayerId, contents, ClickSendCallBack);
//                }
//            }
//            else
//            {
//                ToastBox.ShowText(KLocalization.GetLocalString(53089));//"[类名：FriendWindow.View.cs][函数名：ClickSend]:异常可能原因：1.没有选中一个好友时向聊天框发送了内容");
//            }
//        }
//        private void ClickSendCallBack(int code,string str,object obj) {
//            AddFriendDialog(code,str,obj);
//            ClearDialogInputField();
//        }
//        #endregion
//        #region 添加好友
//        //添加好友内容
//        private void ResetInputLabel() {
//            _btn_input_application.text = string.Empty;
//            //获取一次附近的人
//        }
//        private void ClickClearInput() {
//            ResetInputLabel();
//            RefreshApplicationLeft();
//        }
//        private void ClickSearch() {
//            KFriendManager.Instance.SearchFriends(GetSearchFieldValue(), RefreshSearchResult);
//        }
//        private string GetSearchFieldValue() {
//            string targetOutline = string.Empty;
//            targetOutline = _btn_input_application.text;
//            targetOutline = targetOutline.Trim();
//            return targetOutline;
//        }
//        private void RefreshSearchResult(int code,string str,object obj) {
//            StartCoroutine(FillElements_ApplicationLeft(KFriendManager.Instance.Arry_FriendSearchResults));
//            if (KFriendManager.Instance.Arry_FriendSearchResults.Length <= 0)
//            {
//                ToastBox.ShowText(KLocalization.GetLocalString(53054));//"搜索不到玩家");
//            }
//        }
//        private void RefreshApplicationLeft()
//        {
//            StartCoroutine(FillElements_ApplicationLeft(KFriendManager.Instance.Arry_FriendNearby));
//        }
//        public void RefreshApplicationRight()
//        {
//            StartCoroutine(FillElements_ApplicationRight());
//        }
//        private IEnumerator FillElements_ApplicationLeft(FriendInfo[] showDatas)
//        {
//            _layoutElementPool_nearby.Clear();
//            List<FriendInfo> msnLst = new List<FriendInfo>(showDatas);
//            for (int i = 0; i < showDatas.Length; i++)
//            {
//                var element = _layoutElementPool_nearby.SpawnElement();
//                var friendItem = element.GetComponent<ApplicationPanelLeftFriendItem>();
//                friendItem.ShowItem(showDatas, i);
//            }
//            yield return null;
//        }
//        private IEnumerator FillElements_ApplicationRight()
//        {
//            _layoutElementPool_application.Clear();
//            var friends = GetFriendApplicationArry();
//            List<FriendReq> msnLst = new List<FriendReq>(friends);
//            friends = msnLst.ToArray();
//            for (int i = 0; i < friends.Length; i++)
//            {
//                var element = _layoutElementPool_application.SpawnElement();
//                var friendItem = element.GetComponent<ApplicationPanelRightFriendItem>();
//                friendItem.ShowItem(friends, i);
//            }
//            _go_showNoApplication.SetActive(friends.Length <= 0);            
//            yield return null;
//        }
//        /// <summary>
//        /// 选中了通用表情，将Key贴到输入框中
//        /// </summary>
//        /// <param name="commonExpressionKey"></param>
//        public void AddCommonExpression(string commonExpressionKey)
//        {
//            int newLength = _input_mychat.text.Length + commonExpressionKey.Length;
//            if (_input_mychat.characterLimit < newLength)
//            {
//                _input_mychat.text = _input_mychat.text + commonExpressionKey;
//            }
//            else
//            {
//                ToastBox.ShowText("超出字符长度上限");
//            }
//        }
//        /// <summary>
//        /// 选中了特殊表情，将Key直接发送到服务端，并且不与聊天部分发生关系
//        /// </summary>
//        /// <param name="commonExpressionKey"></param>
//        public void AddSpecialExpression(string commonExpressionKey)
//        {
//            if (KFriendManager.Instance.CurrentSelectFriend != null)
//            {
//                byte[] contents = System.Text.Encoding.Default.GetBytes(commonExpressionKey);
//                //KChatManager.Instance.C2SWorldChatSend(contents, SpecialExpressionCallBack);
//                KFriendManager.Instance.C2SFriendChat(KFriendManager.Instance.CurrentSelectFriend._FriendData.PlayerId, contents, SpecialExpressionCallBack);
//            }
//            else
//            {
//                ToastBox.ShowText(KLocalization.GetLocalString(53089));//"[类名：FriendWindow.View.cs][函数名：ClickSend]:异常可能原因：1.没有选中一个好友时向聊天框发送了内容");
//            }
//        }
//        private void SpecialExpressionCallBack(int code, string str, object obj)
//        {
//            Debug.Log("[日志] [ChatWindow] [ClickSendCallBack] 获取服务器返回的自己发送的特殊表情：" + KChatManager.Instance.LstWorldChatDatas);
//            AddFriendDialog(code, str, obj);
//        }
//        #endregion
//        #region 空间
//        //空间内容
//        private void RefreshRoom() {

//        }
//        #endregion
//    }
//}

