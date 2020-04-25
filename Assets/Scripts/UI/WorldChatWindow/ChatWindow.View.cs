
//using Msg.ClientMessage;
//using System;
//using System.Collections;
///** 
//* 作用：世界聊天的界面控制类
//* 作者：wsy
//*/
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//namespace Game.UI
//{
//    public partial class ChatWindow
//    {
//        #region Field  
//        private const int _int_deltaTime = 1;

//        private const int _int_chatMovingLimit = 4;
//        private int _int_currentTime;
//        private bool _bl_isAcceptClick = true;
//        //世界聊天的发送冷却时间
//        private int _int_timeSendToWorld;
//        private bool _bool_sytemPanelReady = false;

//        private GameObject _btn_close;
//        private Button _btn_blackBack;
//        private Button _btn_closeAnother;

//        private List<Toggle> _lstTgl_channel;
//        private Toggle _tgl_system;
//        private Toggle _tgl_world;
//        private Toggle _tgl_nearby;
//        private Dictionary<Toggle, PageType> _dict_tglAndPgtp = new Dictionary<Toggle, PageType>();

//        private GameObject _go_backForSystem;
//        private GameObject _go_backForChat;
//        private InputField _input_channel;
//        private Button _btn_enterChannel;
//        private GameObject _go_nearby;
//        private KUIGrid _kgrd_chatContent;
//        private KUIGrid _kgrd_systemContent;
//        private Button _btn_ToBottom;
//        private Text _txt_unreadMsgNum;
//        private GameObject _go_chatInputParent;
//        private Button _btn_sound;
//        private Button _btn_emoj;
//        private Button _btn_send;
//        private InputField _input_myChat;
//        private Text _txt_inputDefault;

//        private int _int_currentNunReadNum = 0;
//        //view层当前存放的聊天记录，不会用于生成
//        public IList<WorldChatItem> Lst_ChatData = new List<WorldChatItem>();
//        //view层当前存放的系统消息记录，不会用于生成
//        public IList<AnouncementItem> Lst_anmtsDatas = new List<AnouncementItem>();
//        #endregion

//        #region Method
//        public void InitView()
//        {
//            _btn_blackBack = Find<Button>("BlackBack");
//            _btn_blackBack.onClick.AddListener(OnCloseBtnClick);
//            _btn_close = Find<Transform>("btn_close").gameObject;
//            _btn_closeAnother = Find<Button>("btn_close (1)");
//            _btn_closeAnother.onClick.AddListener(OnCloseBtnClick);

//            ToggleGroup _tglGrp = Find<ToggleGroup>("Tab View/ToggleGroup");
//            _lstTgl_channel = new List<Toggle>(_tglGrp.GetComponentsInChildren<Toggle>());
//            for (int i = 0; i < _lstTgl_channel.Count; i++)
//            {
//                _lstTgl_channel[i].onValueChanged.AddListener(this.OnPageChange);
//            }
//            _tgl_system = Find<Toggle>("Tab View/ToggleGroup/Toggle_system");
//            _tgl_world = Find<Toggle>("Tab View/ToggleGroup/Toggle_world");
//            _tgl_nearby = Find<Toggle>("Tab View/ToggleGroup/Toggle_nearby");
//            _dict_tglAndPgtp.Add(_tgl_system, PageType.kSystemChat);
//            _dict_tglAndPgtp.Add(_tgl_world, PageType.kWorldChat);
//            _dict_tglAndPgtp.Add(_tgl_nearby, PageType.kNearbyChat);

//            _go_backForSystem = Find<Transform>("BackChatContent/BackSystemContent").gameObject;
//            _go_backForChat = Find<Transform>("BackChatContent/BackChatContent").gameObject;
//            _input_channel = Find<InputField>("input_channel");
//            _btn_enterChannel = Find<Button>("input_channel/btn_choice");
//            _go_nearby = Find<Transform>("Dropdown").gameObject;
//            _kgrd_chatContent = Find<KUIGrid>("ScrollView_Chat");
//            if (_kgrd_chatContent)
//            {
//                _kgrd_chatContent.uiPool.itemTemplate.AddComponent<WorldAndNearbyChatItem>();
//            }
//            _kgrd_systemContent = Find<KUIGrid>("ScrollView_System");
//            if (_kgrd_systemContent)
//            {
//                _kgrd_systemContent.uiPool.itemTemplate.AddComponent<SystemChatItem>();
//            }
//            _btn_ToBottom = Find<Button>("UnreadNum");
//            _btn_ToBottom.onClick.AddListener(GoToBottom);
//            _txt_unreadMsgNum = Find<Text>("UnreadNum/txt_num");
//            _btn_sound = Find<Button>("go_input/btn_sound");
//            _btn_emoj = Find<Button>("go_input/btn_emoj");
//            _btn_emoj.onClick.AddListener(ClickOpenExpressionBoard);
//            _btn_send = Find<Button>("go_input/btn_send");
//            _btn_send.onClick.AddListener(ClickSend);
//            _input_myChat = Find<InputField>("go_input/input_myChat");
//            _txt_inputDefault = Find<Text>("go_input/input_myChat/Placeholder");
//            _txt_inputDefault.text = KLocalization.GetLocalString(53047);
//            _go_chatInputParent = Find<Transform>("go_input").gameObject;
//        }
//        public override void UpdatePerSecond()
//        {
//            base.UpdatePerSecond();
//            if (_int_currentTime >= KChatManager.PullCoolingTime)
//            {
//                _int_currentTime = 0;
//                //if (_bool_sytemPanelReady) {
//                //    AddSystemAnnouncements(KChatManager.Instance.CarryOffSystemAnmt(2));
//                //}
//            }
//            else
//            {                
//                _int_currentTime = _int_currentTime + _int_deltaTime;                
//            }
//            if (_int_timeSendToWorld > 0)
//            {
//                _int_timeSendToWorld -= 1;
//            }
//            //新计时方式
//            if (KChatManager.Instance.GetCoolingStatusByKey(KChatManager.Int_KeyPullWorldChat).isFinishCooling  && CurrentPageType == PageType.kWorldChat)
//            {
//				Debug.Log ("->< color =#9400D3>" + Time.time + "累计：" + KChatManager.Instance.GetCoolingStatusByKey(KChatManager.Int_KeyPullWorldChat).CurrentCumulation + "CD:" + KChatManager.Instance.GetCoolingStatusByKey(KChatManager.Int_KeyPullWorldChat).CoolingTime + "</color>");
//                KChatManager.Instance.C2SWorldChatMsgPull(AddChatDataCallBack);
//            }
//            if (KChatManager.Instance.GetCoolingStatusByKey(KChatManager.Int_KeyGetSystemAccouncement).isFinishCooling && CurrentPageType == PageType.kSystemChat)
//            {
//                if (_bool_sytemPanelReady)
//                {
//                    AddSystemAnnouncements(KChatManager.Instance.CarryOffSystemAnmt(2));
//                }
//            }
//        }
//        /// <summary>
//        /// View层入口
//        /// </summary>
//        public void RefreshView()
//        {
//            _int_currentNunReadNum = 0;
//            _btn_ToBottom.gameObject.SetActive(false);
//            _kgrd_chatContent.ClearItems();
//            _kgrd_systemContent.ClearItems();
//            _bool_sytemPanelReady = false;
//            _go_backForSystem.SetActive(CurrentPageType == PageType.kSystemChat || CurrentPageType == PageType.kWorldChat);
//            _go_backForChat.SetActive(CurrentPageType == PageType.kNearbyChat);
//            _kgrd_systemContent.gameObject.SetActive(CurrentPageType == PageType.kSystemChat);
//            _kgrd_chatContent.gameObject.SetActive(CurrentPageType != PageType.kSystemChat);
//            _go_chatInputParent.SetActive(CurrentPageType != PageType.kSystemChat);
//            _input_channel.gameObject.SetActive(false);
//            switch (CurrentPageType)
//            {
//                case PageType.kSystemChat:
//                    Lst_anmtsDatas = new List<AnouncementItem>();
//                    StartCoroutine(IEnumerator_RefreshSystemView());
//                    break;
//                case PageType.kWorldChat:
//                    Lst_ChatData = new List<WorldChatItem>();
//                    StartCoroutine(IEnumerator_RefreshWorldView());
//                    break;
//                case PageType.kNearbyChat:
//                    StartCoroutine(IEnumerator_RefreshNearbyView());                    
//                    break;
//            }
//        }
//        private IEnumerator IEnumerator_RefreshSystemView()
//        {
//            yield return null;
//            RefreshSystemView();
//        }
//        private IEnumerator IEnumerator_RefreshWorldView()
//        {
//            yield return null;
//            RefreshWorldView();
//        }
//        private IEnumerator IEnumerator_RefreshNearbyView()
//        {
//            yield return null;
//            RefreshNearbyView();
//        }
//        /// <summary>
//        /// 刷新系统消息页签
//        /// </summary>
//        private void RefreshSystemView() {
//            IList<AnouncementItem> anmtsDatas = KChatManager.Instance.CarryOffSystemAnmt(1);
//            CreateSystemAnnouncements(anmtsDatas);
//            _bool_sytemPanelReady = true;
//        }
//        /// <summary>
//        /// 刷新世界聊天页签
//        /// </summary>
//        private void RefreshWorldView() {
//            ClearChatInputField();
//            if (KChatManager.Instance.GetCoolingStatusByKey(KChatManager.Int_KeyPullWorldChat).isFinishCooling)
//            {
//                KChatManager.Instance.C2SWorldChatMsgPull(GetOldAndNewChatDataCallBack);
//            }
//            else {
//                GetOldAndNewChatData();
//            }
//        }
//        /// <summary>
//        /// 刷新附近聊天页签
//        /// </summary>
//        private void RefreshNearbyView() {

//        }
//        /// <summary>
//        /// 以现在所显示的最下端的子物体数据是不是数据源列表的最末端来判断是否要显示未读消息的按键,num:为0表示清空未读消息，不为0表示需要遍历
//        /// </summary>
//        /// <param name="num"></param>
//        public void RefreshUnreadMsgNum(int num) {
//            if (num == -1)
//            {
//                _int_currentNunReadNum = num == -1 ? 0 : _int_currentNunReadNum + num;
//                _btn_ToBottom.gameObject.SetActive(!(_int_currentNunReadNum == 0));
//                _txt_unreadMsgNum.text = "未读消息：" + _int_currentNunReadNum.ToString();
//            }
//            else
//            {
//                bool gotoBottom = false;               
//                if (CurrentPageType == PageType.kSystemChat)
//                {
//                    int indexr;
//                    if (Lst_anmtsDatas.Count == 0)
//                    {
//                        indexr = 0;
//                    }
//                    else
//                    {
//                        indexr = Lst_anmtsDatas.Count - 1;
//                        AnouncementItem lstdata = Lst_anmtsDatas[indexr];
//                        SystemChatItem[] lstitem = _kgrd_systemContent.GetComponentsInChildren<SystemChatItem>();
//                        for (int i = 0; i < lstitem.Length; i++)
//                        {
//                            if (lstitem[i]._data_chat == lstdata)
//                            {
//                                gotoBottom = true;
//                            }
//                        }
//                    }
//                    int count = _kgrd_systemContent.uiPool.itemCount;
//                    if (count >= 5 && gotoBottom)
//                    {
//                        _kgrd_systemContent.RefillItems(count - 1);
//                    }
//                    else if (count <= 5)
//                    {
//                        _kgrd_systemContent.RefillItems(count - 1);
//                    }
//                }
//                else if(CurrentPageType == PageType.kWorldChat)
//                {
//                    int indexr;
//                    if (Lst_ChatData.Count == 0)
//                    {
//                        indexr = 0;
//                    }
//                    else
//                    {
//                        indexr = Lst_ChatData.Count - 1;
//                        WorldChatItem lstdata = Lst_ChatData[indexr];
//                        WorldAndNearbyChatItem[] lstitem = _kgrd_systemContent.GetComponentsInChildren<WorldAndNearbyChatItem>();
//                        for (int i = 0; i < lstitem.Length; i++)
//                        {
//                            if (lstitem[i]._data_chat == lstdata)
//                            {
//                                gotoBottom = true;
//                            }
//                        }
//                    }
//                    int count = _kgrd_chatContent.uiPool.itemCount;
//                    if (count >= 5 && gotoBottom)
//                    {
//                        _kgrd_chatContent.RefillItems(count - 1);
//                    }
//                    else if (count <= 5)
//                    {
//                        _kgrd_chatContent.RefillItems(count - 1);
//                    }
//                }
//                if (gotoBottom)
//                {
//                    _int_currentNunReadNum = 0;
//                }
//                else {
//                    _int_currentNunReadNum = _int_currentNunReadNum + num;
//                }
//                _btn_ToBottom.gameObject.SetActive(!gotoBottom);
//                _txt_unreadMsgNum.text = "未读消息：" + _int_currentNunReadNum.ToString();
//            }
//        }
//        //内容刷新至最底端
//        private void GoToBottom() {
//            if (CurrentPageType == PageType.kSystemChat)
//            {
//                int count = _kgrd_systemContent.uiPool.itemCount;
//                _kgrd_systemContent.RefillItems(count - 1);                
//            }
//            else if (CurrentPageType == PageType.kWorldChat)
//            {
//                int count = _kgrd_chatContent.uiPool.itemCount;
//                _kgrd_chatContent.RefillItems(count - 1);
//            }
//        }
//        /// <summary>
//        /// 每当打开世界聊天界面时向服务器拉取一次数据，然后结合已经存在本地的数据生成一次已存在的聊天
//        /// </summary>
//        /// <param name="code"></param>
//        /// <param name="str"></param>
//        /// <param name="obj"></param>
//        private void GetOldAndNewChatDataCallBack(int code,string str,object obj) {
//            GetOldAndNewChatData();
//        }
//        private void GetOldAndNewChatData() {
//            _int_currentTime = 0;
//            //_kgrd_chatContent.ClearItems();
//            IList<WorldChatItem> lst_ChatData = KChatManager.Instance.LstWorldChatDatas;
//            CreateChat(lst_ChatData);
//        }
//        /// <summary>
//        /// 目前每五秒拉取一次世界聊天的新内容
//        /// </summary>
//        /// <param name="code"></param>
//        /// <param name="str"></param>
//        /// <param name="obj"></param>
//        private void AddChatDataCallBack(int code, string str, object obj) {
//            List<WorldChatItem> lstChatData = KChatManager.Instance.LstAddWorldChatDatas;
//            AddChat(lstChatData);
//        }
//        private void ClearChatInputField()
//        {
//            _input_myChat.text = string.Empty;
//        }
//        /// <summary>
//        /// 生成聊天的具体实现
//        /// </summary>
//        /// <param name="value"></param>
//        private void CreateChat(IList<WorldChatItem> value) {
//            WorldChatItem[] datas = value.ToArray();
//            Lst_ChatData = value;
//            _kgrd_chatContent.uiPool.SetItemDatas(datas);
//            StartCoroutine(waitForRefreshWorldChat());
//        }
//        private IEnumerator waitForRefreshWorldChat()
//        {
//            yield return null;
//            int count = _kgrd_chatContent.uiPool.itemCount;
//            if (count >= _int_chatMovingLimit)
//            {
//                _kgrd_chatContent.RefillItems(count - 1);
//            }
//            else
//            {
//                _kgrd_chatContent.RefillItems();
//            }
//        }
//        /// <summary>
//        /// 累加聊天的具体实现
//        /// </summary>
//        /// <param name="value"></param>
//        private void AddChat(List<WorldChatItem> value)
//        {
//            WorldChatItem[] datas = value.ToArray();
//            if (datas.Length == 0)
//                return;
//            RefreshUnreadMsgNum(value.Count);
//            Lst_ChatData.AddRange(value);
//            _kgrd_chatContent.uiPool.AddItemDatas(datas);
//            for (int i = 0; i < datas.Length; i++)
//            {
//                KChatManager.Instance.RemoveNewDataElementByUse(datas[i]);
//            }
//        }
//        /// <summary>
//        /// 首次填充系统消息的具体实现
//        /// </summary>
//        /// <param name="value"></param>
//        private void CreateSystemAnnouncements(IList<AnouncementItem> value)
//        {
//            AnouncementItem[] datas = value.ToArray();
//            Lst_anmtsDatas = value;
//            _kgrd_systemContent.uiPool.SetItemDatas(datas);
//            StartCoroutine(waitForRefreshSystemAccouncement());
//        }
//        private IEnumerator waitForRefreshSystemAccouncement() {
//            yield return null;
//            int count = _kgrd_systemContent.uiPool.itemCount;
//            if (count >= _int_chatMovingLimit)
//            {
//                _kgrd_systemContent.RefillItems(count - 1);
//            }
//            else
//            {
//                _kgrd_systemContent.RefillItems();
//            }
//        }
//        /// <summary>
//        /// 后续添加系统消息的具体实现
//        /// </summary>
//        /// <param name="value"></param>
//        private void AddSystemAnnouncements(IList<AnouncementItem> value)
//        {
//            AnouncementItem[] datas = value.ToArray();
//            if (datas.Length == 0)
//                return;
//            RefreshUnreadMsgNum(value.Count);
//            Lst_anmtsDatas.AddRange(value);
//            _kgrd_systemContent.uiPool.AddItemDatas(datas);            
//            for (int i = 0; i < datas.Length; i++)
//            {
//                Debug.Log("-><color=#9400D3>" + "[ChatWindow] [AddSystemAnnouncements] 增加的数据：" + datas[i].MsgType + "</color>");
//            }
//        }
//        /// <summary>
//        /// 点击发送
//        /// </summary>
//        private void ClickSend()
//        {
//            if (KChatManager.Instance.GetCoolingStatusByKey(KChatManager.Int_KeySendWorldChat).isFinishCooling)
//            {
//                if (_input_myChat.text.Length <= 0)
//                {
//                    ToastBox.ShowText("不可以发送空的消息！");
//                }
//                else {
//                    byte[] contents = System.Text.Encoding.Default.GetBytes(_input_myChat.text);
//                    KChatManager.Instance.C2SWorldChatSend(contents, ClickSendCallBack);
//                }
//            }
//            else {
//                WarnningOperatTooFrequent();
//                ClearChatInputField();
//            }
//        }
//        private void ClickSendCallBack(int code, string str, object obj)
//        {
//            Debug.Log("[ChatWindow] [ClickSendCallBack] 获取服务器返回的自己发送的消息：" + KChatManager.Instance.LstWorldChatDatas);
//            _int_timeSendToWorld = KChatManager.SendCoolingTime;
//            //AddChatDataCallBack(code, str, obj);
//            ClearChatInputField();
//        }
//        /// <summary>
//        /// 点击打开表情列表面板
//        /// </summary>
//        private void ClickOpenExpressionBoard() {
//            KUIWindow.OpenWindow<ExpressionBoard>();


//        }
//        /// <summary>
//        /// 选中了通用表情，将Key贴到输入框中
//        /// </summary>
//        /// <param name="commonExpressionKey"></param>
//        public void AddCommonExpression(string commonExpressionKey) {
//            int newLength = _input_myChat.text.Length + commonExpressionKey.Length;
//            if (_input_myChat.characterLimit < newLength)
//            {
//                _input_myChat.text = _input_myChat.text + commonExpressionKey;
//            }
//            else {
//                ToastBox.ShowText("超出字符长度上限");
//            }
//        }
//        /// <summary>
//        /// 选中了特殊表情，将Key直接发送到服务端，并且不与聊天部分发生关系
//        /// </summary>
//        /// <param name="commonExpressionKey"></param>
//        public void AddSpecialExpression(string commonExpressionKey)
//        {
//            if (KChatManager.Instance.GetCoolingStatusByKey(KChatManager.Int_KeySendWorldChat).isFinishCooling)
//            {
//                byte[] contents = System.Text.Encoding.Default.GetBytes(commonExpressionKey);
//                KChatManager.Instance.C2SWorldChatSend(contents, SpecialExpressionCallBack);              
//            }
//            else
//            {
//                WarnningOperatTooFrequent();
//            }
//        }
//        private void SpecialExpressionCallBack(int code, string str, object obj)
//        {
//            Debug.Log("[日志] [ChatWindow] [ClickSendCallBack] 获取服务器返回的自己发送的特殊表情：" + KChatManager.Instance.LstWorldChatDatas);
//            _int_timeSendToWorld = KChatManager.SendCoolingTime;
//        }
//        //飘字：发送间隔过短
//        private void WarnningOperatTooFrequent() {
//            CoolingTimeAndFinishCoolingStatusData sendCoolingStatus = KChatManager.Instance.GetCoolingStatusByKey(KChatManager.Int_KeySendWorldChat);
//            float leftPreciseSecond = (float)Math.Round(sendCoolingStatus.CoolingTime - sendCoolingStatus.CurrentCumulation,1);
//            if (leftPreciseSecond > 1)
//            {
//                int leftSecond = Mathf.CeilToInt(sendCoolingStatus.CoolingTime - sendCoolingStatus.CurrentCumulation);
//                ToastBox.ShowText("需要" + _int_timeSendToWorld.ToString() + "秒才能发送下一条消息！");
//            }
//            else {
//                ToastBox.ShowText("需要" + leftPreciseSecond.ToString() + "秒才能发送下一条消息！");
//            }
//        }
//        #endregion
//    }
//}









