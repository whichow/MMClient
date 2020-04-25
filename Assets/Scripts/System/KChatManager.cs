//// ***********************************************************************
//// 作用：世界聊天的数据存放和处理
//// 作者: wsy
//// ***********************************************************************
//namespace Game
//{
//    using UnityEngine;
//    using System.Collections;
//    using System.Collections.Generic;
//    using Callback = System.Action<int, string, object>;
//    using Msg.ClientMessage;
//    using UnityEngine.UI;
//    using Game.UI;

//    public class KChatManager : SingletonUnity<KChatManager>
//    {
//        //向服务器拉取世界消息的冷却计时
//        private float _flt_timeForPullWorldChatCooling = 0;
//        private Dictionary<int, CoolingTimeAndFinishCoolingStatusData> _dict_keyAndStatusForCooling = new Dictionary<int, CoolingTimeAndFinishCoolingStatusData>();
//        public const int Int_KeyPullWorldChat = 1;
//        public const int PullCoolingTime = 5;
//        public const int Int_KeySendWorldChat = 2;
//        public const int SendCoolingTime = 10;
//        public const int Int_KeyGetSystemAccouncement = 3;
//        public const int GetSystemAccouncementCoolingTime = 5;

//        private Dictionary<int, KSysMessage> _dict_systemAccouncementCfg = new Dictionary<int, KSysMessage>();
//        /// <summary>
//        /// 通告使用的表
//        /// </summary>
//        public Dictionary<int, KSysMessage> Dict_SystemAccouncementCfg
//        {
//            get
//            {
//                return _dict_systemAccouncementCfg;
//            }
//        }
//        /// <summary>
//        /// 面板划出划入的动画时间
//        /// </summary>
//        public const float PanelMoveTime = 0.2f;

//        private IList<WorldChatItem> _lstWorldChatDatas = new List<WorldChatItem>();
//        /// <summary>
//        /// 世界聊天内容
//        /// </summary>
//        public IList<WorldChatItem> LstWorldChatDatas
//        {
//            get
//            {
//                return _lstWorldChatDatas;
//            }
//        }
//        private List<WorldChatItem> _lstAddWorldChatDatas = new List<WorldChatItem>();
//        /// <summary>
//        /// 世界聊天新增内容
//        /// </summary>
//        public List<WorldChatItem> LstAddWorldChatDatas
//        {
//            get
//            {
//                return _lstAddWorldChatDatas;
//            }
//        }
//        private IList<AnouncementItem> _lstSystemAnnouncementDatas = new List<AnouncementItem>();
//        /// <summary>
//        /// 系统消息
//        /// </summary>
//        public IList<AnouncementItem> LstSystemAnnouncementDatas
//        {
//            get
//            {
//                return _lstSystemAnnouncementDatas;
//            }
//        }
//        private List<AnouncementItem> _lstAddSystemAnnouncementDatas = new List<AnouncementItem>();
//        /// <summary>
//        /// 新增系统消息
//        /// </summary>
//        public List<AnouncementItem> LstAddSystemAnnouncementDatas
//        {
//            get
//            {
//                return _lstAddSystemAnnouncementDatas;
//            }
//        }
//        private List<AnouncementItem> _lstAddRockSystemAnnouncementDatas = new List<AnouncementItem>();
//        /// <summary>
//        /// 用于滚动的消息队列
//        /// </summary>
//        public List<AnouncementItem> LstAddRockSystemAnnouncementDatas
//        {
//            get
//            {
//                return _lstAddRockSystemAnnouncementDatas;
//            }
//        }
//        public AnouncementItem CarryOffAnnouncementItemByMainViewRock()
//        {
//            if (_lstAddRockSystemAnnouncementDatas != null && _lstAddRockSystemAnnouncementDatas.Count != 0)
//            {
//                AnouncementItem data_IndexZero = new AnouncementItem();
//                data_IndexZero = _lstAddRockSystemAnnouncementDatas.Find(x => x == _lstAddRockSystemAnnouncementDatas[0]);
//                _lstAddRockSystemAnnouncementDatas.Remove(_lstAddRockSystemAnnouncementDatas.Find(x => x == _lstAddRockSystemAnnouncementDatas[0]));
//                return data_IndexZero;
//            }
//            return null;
//        }
//        /// <summary>
//        /// 当新数据被界面使用之后，从List中移除这个数据
//        /// </summary>
//        public void RemoveNewDataElementByUse(WorldChatItem value)
//        {
//            _lstAddWorldChatDatas.Remove(_lstAddWorldChatDatas.Find(x => x == value));
//        }
//        /// <summary>
//        /// 当重新进入世界聊天的界面时候移除上次打开之后所有的后续增加的数据
//        /// </summary>
//        public void ClearNewDatasWhenInitializationChatView()
//        {
//            _lstAddWorldChatDatas.Clear();
//        }
//        /// <summary>
//        /// 存放最后一次的频道
//        /// </summary>
//        /// <param name="value"></param>
//        public void RecordLastPageType(ChatWindow.PageType value)
//        {
//            _lastPageType = value;
//        }
//        private ChatWindow.PageType _lastPageType = ChatWindow.PageType.kSystemChat;
//        /// <summary>
//        /// 获取最后一次频道
//        /// </summary>
//        public ChatWindow.PageType LastOpenPageType
//        {
//            get
//            {
//                return _lastPageType;
//            }
//        }
//        /// <summary>
//        /// 定时剪走一部分系统消息用于显示,1获取全部数据，2获取后续数据
//        /// </summary>
//        /// <returns></returns>
//        public IList<AnouncementItem> CarryOffSystemAnmt(int firstTime)
//        {
//            if (firstTime == 1)
//            {
//                IList<AnouncementItem> lstAnmts = _lstSystemAnnouncementDatas;
//                _lstAddSystemAnnouncementDatas.Clear();
//                return lstAnmts;
//            }
//            else
//            {
//                List<AnouncementItem> lstAnmts = new List<AnouncementItem>();
//                for (int i = 0; i < _lstAddSystemAnnouncementDatas.Count; i++)
//                {
//                    lstAnmts.Add(_lstAddSystemAnnouncementDatas[i]);
//                }
//                _lstAddSystemAnnouncementDatas.Clear();
//                return lstAnmts;
//            }
//        }
//        /// <summary>
//        /// 向服务器拉取世界聊天消息
//        /// </summary>
//        /// <param name="friendid"></param>
//        /// <param name="callback"></param>
//		public void C2SWorldChatMsgPull(Callback callback)
//        {
//            //_dict_keyAndStatusForCooling[Int_KeyPullWorldChat].isFinishCooling = false;
//            //_dict_keyAndStatusForCooling[Int_KeyPullWorldChat].CurrentCumulation = -1;
//            KUser.C2SWorldChatMsgPull((code, message, data) =>
//            {
//                var protoDatas = data as ArrayList;
//                if (protoDatas != null)
//                {
//                    //Debug.Log("-><color=#9400D3>" + "[KChatManager] [C2SWorldChatMsgPull] [日志] 首次向服务器索取世界聊天数据" + "</color>");
//                    for (int i = 0; i < protoDatas.Count; i++)
//                    {
//                        if (protoDatas[0] is S2CWorldChatMsgPullResult)
//                        {
//                            var chatDatas = (S2CWorldChatMsgPullResult)protoDatas[0];
//                            IList<WorldChatItem> lstChatData = new List<WorldChatItem>();
//                            if (chatDatas.Items is IList<WorldChatItem>)
//                            {
//                                lstChatData = chatDatas.Items as IList<WorldChatItem>;
//                                Debug.Log("[KChatManager] [C2SWorldChatMsgPull] 向服务器拉取一次数据,长度：" + lstChatData.Count + "时间：" + Time.time.ToString() + "间隔时间：" + Time.deltaTime);
//                            }
//                            if (_lstWorldChatDatas == null || _lstWorldChatDatas.Count <= 0)
//                            {
//                                _lstWorldChatDatas = lstChatData;
//                            }
//                            else
//                            {
//                                (_lstWorldChatDatas as List<WorldChatItem>).AddRange(lstChatData);
//                            }
//                            _lstAddWorldChatDatas.Clear();
//                            _lstAddWorldChatDatas.AddRange(lstChatData);
//                        }
//                    }
//                    _dict_keyAndStatusForCooling[Int_KeyPullWorldChat].isFinishCooling = false;
//                    _dict_keyAndStatusForCooling[Int_KeyPullWorldChat].CurrentCumulation = 0;
//                    if (callback != null)
//                    {
//                        callback(code, message, data);
//                    }
//                }
//            });
//        }
//        /// <summary>
//        /// 在世界聊天频道发送一次消息
//        /// </summary>
//        /// <param name="myChata"></param>
//        /// <param name="callback"></param>
//        public void C2SWorldChatSend(byte[] myChata, Callback callback)
//        {
//            //_dict_keyAndStatusForCooling[Int_KeySendWorldChat].isFinishCooling = false;
//            //_dict_keyAndStatusForCooling[Int_KeySendWorldChat].CurrentCumulation = -1;
//            KUser.C2SWorldChatSend(myChata, (code, message, data) =>
//            {
//                var protoDatas = data as ArrayList;
//                if (protoDatas != null)
//                {
//                    for (int i = 0; i < protoDatas.Count; i++)
//                    {
//                        if (protoDatas[0] is S2CWorldChatSendResult)
//                        {
//                            var chatDatas = (S2CWorldChatSendResult)protoDatas[0];
//                            byte[] lstChatData;
//                            if (chatDatas.Content is byte[])
//                            {
//                                //lstChatData = chatDatas.Content as byte[];
//                                //Debug.Log("[KChatManager] [C2SWorldChatSend] 服务器向世界发送消息的反馈内容" + System.Text.Encoding.Default.GetString(lstChatData));
//                                //List<WorldChatItem> newLstChat = new List<WorldChatItem>();
//                                //WorldChatItem myChatData = new WorldChatItem
//                                //{
//                                //    Content = lstChatData,
//                                //    PlayerId = KUser.SelfPlayer.id,
//                                //    PlayerName = KUser.SelfPlayer.name,
//                                //    PlayerHead = KUser.SelfPlayer.headURL,
//                                //    PlayerLevel = KUser.SelfPlayer.grade,
//                                //    SendTime = KFriendManager.GetTimeStamp()
//                                //};
//                                //newLstChat.Add(myChatData);
//                                //if (_lstWorldChatDatas == null || _lstWorldChatDatas.Count <= 0)
//                                //{
//                                //    _lstWorldChatDatas = newLstChat;
//                                //}
//                                //else
//                                //{
//                                //    _lstWorldChatDatas.AddRange(newLstChat);
//                                //    _lstAddWorldChatDatas.AddRange(newLstChat);
//                                //}
//                            }
//                        }
//                    }
//                    _dict_keyAndStatusForCooling[Int_KeySendWorldChat].isFinishCooling = false;
//                    _dict_keyAndStatusForCooling[Int_KeySendWorldChat].CurrentCumulation = 0;
//                    if (callback != null)
//                    {
//                        callback(code, message, data);
//                    }
//                }
//            });
//        }
//        /// <summary>
//        /// 服务器推送消息
//        /// </summary>
//        /// <param name="data"></param>
//        public void Process(object data)
//        {
//            if (data is Msg.ClientMessage.S2CAnouncementNotify)
//            {
//                var originData = (S2CAnouncementNotify)data;

//                IList<AnouncementItem> newDatas = new List<AnouncementItem>();
//                if (originData.Items is IList<AnouncementItem>)
//                {
//                    newDatas = originData.Items;
//                }
//                if (_lstSystemAnnouncementDatas == null || _lstSystemAnnouncementDatas.Count == 0)
//                {
//                    _lstSystemAnnouncementDatas = newDatas;
//                }
//                else
//                {
//                    (_lstSystemAnnouncementDatas as List<AnouncementItem>).AddRange(newDatas);
//                }
//                List<AnouncementItem> newScrolDatas = new List<AnouncementItem>();
//                for (int i = 0; i < newDatas.Count; i++)
//                {
//                    if (_dict_systemAccouncementCfg[newDatas[i].MsgType].newsTicker == 1)
//                    {
//                        newScrolDatas.Add(newDatas[i]);
//                    }
//                }
//                if (_lstAddRockSystemAnnouncementDatas == null || _lstAddRockSystemAnnouncementDatas.Count == 0)
//                {
//                    _lstAddRockSystemAnnouncementDatas = newScrolDatas;
//                }
//                else
//                {
//                    _lstAddRockSystemAnnouncementDatas.AddRange(newScrolDatas);
//                }
//                _lstAddSystemAnnouncementDatas.AddRange(newDatas);
//                for (int i = 0; i < newDatas.Count; i++)
//                {
//                    Debug.Log("-><color=#9400D3>" + "[日志] [KChatManager] [Process] 服务器推送系统通告，类型ID：" + newDatas[i].MsgType + "</color>");
//                }
//            }
//        }
//        public void Load(Hashtable table)
//        {
//            if (table != null)
//            {
//                var list = table.GetList("SysMessage");
//                if (list != null && list.Count > 0)
//                {
//                    var tmpT = new Hashtable();
//                    for (int i = 0; i < list.Count - 1; i++)
//                    {
//                        var tmpL0 = (ArrayList)list[0];
//                        var tmpLi = (ArrayList)list[i + 1];
//                        for (int j = 0; j < tmpL0.Count; j++)
//                        {
//                            tmpT[tmpL0[j]] = tmpLi[j];
//                        }
//                        var systemmsg = new KSysMessage();
//                        systemmsg.Load(tmpT);
//                        _dict_systemAccouncementCfg.Add(systemmsg.id, systemmsg);
//                    }
//                }
//            }
//        }
//        #region Unity
//        private void Start()
//        {
//            Object textObject;
//            if (KAssetManager.Instance.TryGetExcelAsset("SysMessage", out textObject))
//            {
//                var tmpText = textObject as TextAsset;
//                if (tmpText)
//                {
//                    var tmpJson = tmpText.bytes.ToJsonTable();
//                    Load(tmpJson);
//                }
//            }

//            {
//                CoolingTimeAndFinishCoolingStatusData onePair = new CoolingTimeAndFinishCoolingStatusData
//                {
//                    CoolingTime = PullCoolingTime,
//                    CurrentCumulation = PullCoolingTime,
//                    isFinishCooling = true,
//                };
//                _dict_keyAndStatusForCooling.Add(Int_KeyPullWorldChat, onePair);
//                CoolingTimeAndFinishCoolingStatusData twoPair = new CoolingTimeAndFinishCoolingStatusData
//                {
//                    CoolingTime = SendCoolingTime,
//                    CurrentCumulation = SendCoolingTime,
//                    isFinishCooling = true,
//                };
//                _dict_keyAndStatusForCooling.Add(Int_KeySendWorldChat, twoPair);
//                CoolingTimeAndFinishCoolingStatusData threePair = new CoolingTimeAndFinishCoolingStatusData
//                {
//                    CoolingTime = GetSystemAccouncementCoolingTime,
//                    CurrentCumulation = GetSystemAccouncementCoolingTime,
//                    isFinishCooling = true,
//                };
//                _dict_keyAndStatusForCooling.Add(Int_KeyGetSystemAccouncement, threePair);
//            }
//        }

//        private void LateUpdate()
//        {
//            //Debug.Log("-><color=#FF00D3>" + "计时输出：" + Time.fixedDeltaTime + "</color>");
//            foreach (var kv in _dict_keyAndStatusForCooling)
//            {
//                var item = kv.Value;
//                //if (!item.isFinishCooling && item.CurrentCumulation >= 0)
//                //{
//                item.CurrentCumulation += Time.deltaTime;
//                if (item.CurrentCumulation >= item.CoolingTime)
//                {
//                    item.CurrentCumulation = item.CoolingTime;
//                    item.isFinishCooling = true;
//                }
//                //}
//            }
//        }
//        /// <summary>
//        /// 用ID索引事件的冷却状态
//        /// </summary>
//        /// <param name="timekey"></param>
//        /// <returns></returns>
//        public CoolingTimeAndFinishCoolingStatusData GetCoolingStatusByKey(int timekey)
//        {
//            return _dict_keyAndStatusForCooling[timekey];
//        }
//        #endregion
//        //获取系统消息的宾语
//        public static string FillobjectName(AnouncementItem value)
//        {
//            //1 获得4/5星寄养卡  2 获得4阶装饰物  3 获得4阶装饰物配方  4 获得SSR猫  5 排行榜首位  6 猫满级  7 纯文本
//            switch (value.MsgType)
//            {
//                case 1: return KLocalization.GetLocalString(KItemManager.Instance.GetFosterCardbyBindId(value.FosterCardTableId).itemName);
//                case 2: return KLocalization.GetLocalString(KItemManager.Instance.GetBuilding(value.BuildingTableId).itemName);
//                case 3:
//                    if (KItemManager.Instance.GetFormula(value.FormulaTableId) != null)
//                    {
//                        return KLocalization.GetLocalString(KItemManager.Instance.GetFormula(value.FormulaTableId).itemName);
//                    }
//                    else
//                    {
//                        Debug.Log("[SystemChatItem] [FillobjectName] [异常] 根据：" + "-><color=#9400D3>" + value.FormulaTableId + "</color>" + "获取不到对应ID的静态数据.");
//                        return value.FormulaTableId.ToString();
//                    }
//                case 4: return KLocalization.GetLocalString(KItemManager.Instance.GetCat(value.SSRCatTableId).itemName);
//                case 5:
//                    if (value.RankType == 1)
//                    {
//                        return KLocalization.GetLocalString(55016);
//                    }
//                    else if (value.RankType == 2)
//                    {
//                        if (KLevelManager.Instance.GetLevelById(value.StageId) == null)
//                        {
//                            Debug.Log("-><color=#9400D3>" + "[异常] [SystemChatItem] [FillobjectName] 根据：" + value.StageId + "获取不到对应ID的静态数据." + "</color>");
//                            return value.StageId.ToString();
//                        }
//                        else
//                        {
//                            return KLevelManager.Instance.GetLevelById(value.StageId).levelName;
//                        }
//                    }
//                    else if (value.RankType == 3)
//                    {
//                        return KLocalization.GetLocalString(55018);
//                    }
//                    else if (value.RankType == 4)
//                    {
//                        return KLocalization.GetLocalString(55019);
//                    }
//                    else if (value.RankType == 5)
//                    {
//                        return KLocalization.GetLocalString(55021);
//                    }
//                    else
//                    {
//                        return "[SystemChatItem] [FillobjectName] 异常：系统消息类型为 [5] 时，RankType 即排行榜类型字段不在约定范围内，错误的 RankType为：" + value.RankType.ToString();
//                    }
//                case 6: return KLocalization.GetLocalString(KItemManager.Instance.GetCat(value.CatFullLevelTableId).itemName);
//                default: return "";
//            }
//        }
//    }

//    public class CoolingTimeAndFinishCoolingStatusData
//    {
//        public int CoolingTime;
//        public float CurrentCumulation;
//        public bool isFinishCooling;
//    }
//}