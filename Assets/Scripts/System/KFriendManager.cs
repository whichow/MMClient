//// ***********************************************************************
//// Company          : 
//// Author           : Kimch
//// Created          : 
////
//// Last Modified By : Kimch
//// Last Modified On : 
//// ***********************************************************************
//namespace Game
//{
//    using UnityEngine;
//    using System.Collections;
//    using System.Collections.Generic;
//    using Callback = System.Action<int, string, object>;
//    using Msg.ClientMessage;
//    using System.Linq;
//    using System;

//    /// <summary>
//    /// KFriendManager
//    /// </summary>
//    public class KFriendManager : SingletonUnity<KFriendManager>
//    {
//        /// <summary>
//        /// 一件赠送友情点的次数上限
//        /// </summary>
//        public const int Int_const_presentAllFriendNum = 100;

//        //public const int Int_const_onePerson


//        private Dictionary<int, List<FriendRichDialogData>> friendDialog = new Dictionary<int, List<FriendRichDialogData>>();
//        /// <summary>
//        /// 好友全部对话数据
//        /// </summary>
//        public Dictionary<int, List<FriendRichDialogData>> FriendDialog {
//            get {
//                return friendDialog;
//            }
//        }
//        private Dictionary<int, List<FriendRichDialogData>> currentFriendNewAddDialog = new Dictionary<int, List<FriendRichDialogData>>();
//        /// <summary>
//        /// 当前对话目标获取到的新的消息
//        /// </summary>
//        public Dictionary<int, List<FriendRichDialogData>> CurrentFriendNewAddDialog
//        {
//            get
//            {
//                return currentFriendNewAddDialog;
//            }
//        }
//        /// <summary>
//        /// 好友未读消息
//        /// </summary>
//        private Dictionary<int, int> _friendUnMsgNum = new Dictionary<int, int>();
//        public Dictionary<int, int> FriendUnMsgNum {
//            get {
//                return _friendUnMsgNum;
//            }
//        }
//        /// <summary>
//        /// 赠送友情点数的回馈
//        /// </summary>
//        public int PresentRewardPoints { get; private set; }
//        public void ClearOldPresentRewardPoints() {
//            PresentRewardPoints = 0;
//        }

//        #region FIELD
//        private bool pullServerData = false;
//        public bool PullServerData {
//            get {
//                return pullServerData;
//            }
//        }
//        //好友列表
//        private FriendInfo[] _arry_friends = new FriendInfo[] { };
//        public FriendInfo[] Arry_Friends {
//            get {
//                return _arry_friends;
//            }
//        }
//        //好友的申请列表
//        private FriendReq[] _arry_friendReqs = new FriendReq[] { };
//        public FriendReq[] Arry_FriendReqs
//        {
//            get {
//                return _arry_friendReqs;
//            }
//        }
//        //好友的申请列表
//        private FriendInfo[] _arry_friendNearby = new FriendInfo[] { };
//        public FriendInfo[] Arry_FriendNearby
//        {
//            get
//            {
//                return _arry_friendNearby;
//            }
//        }
//         /// <summary>
//         /// 赠送好友之后的反馈信息列表
//         /// </summary>
//        private FriendPointsResult[] _array_friendPointsResult = new FriendPointsResult[] { };
//        public FriendPointsResult[] Array_FriendPointsResult
//        {
//            get
//            {
//                return _array_friendPointsResult;
//            }
//        }
//        //搜索结果
//        private FriendInfo[] _arry_friendSearchResults = new FriendInfo[] { };
//        public FriendInfo[] Arry_FriendSearchResults
//        {
//            get
//            {
//                return _arry_friendSearchResults;
//            }
//        }
//        /// <summary>
//        /// 好友界面：当前选中的好友
//        /// </summary>
//        private UI.FriendPanelFriendItem currentSelectFriend;
//        public UI.FriendPanelFriendItem CurrentSelectFriend
//        {
//            get
//            {
//                return currentSelectFriend;
//            }
//        }
//        //最后一次添加的好友的ID
//        public int LastAddFriendID { get; private set; }
//        //当天剩余赠送友情点数的次数
//        public int LeftPresentNum { get; private set; }
//        /// <summary>
//        /// 最后一次收取全部好友赠送点数的列表
//        /// </summary>
//        private IList<FriendPoints> _frdPsLst_getAllPoints = new List<FriendPoints>();
//        public IList<FriendPoints> FrdPsLst_getAllPoints
//        {
//            get
//            {
//                return _frdPsLst_getAllPoints;
//            }
//        }
//        private List<FriendInfo> _lst_frdInfoHaseEmptyPosition = new List<FriendInfo>();
//        /// <summary>
//        /// 拥有空位置的好友列表
//        /// </summary>
//        public List<FriendInfo> Lst_frdInfoHaseEmptyPosition {
//            get {
//                return _lst_frdInfoHaseEmptyPosition;
//            }
//        }
//        #endregion

//        #region PROPERTY
//        #endregion

//        #region METHOD     
//        public void SelectFriendPanelFriend(UI.FriendPanelFriendItem newSelected) {
//            if (currentSelectFriend != null)
//            {
//                currentSelectFriend.SelectNewItem(false);
//            }
//            currentSelectFriend = newSelected;
//            if (currentSelectFriend != null)
//            {
//                currentSelectFriend.SelectNewItem(true);
//            }
//            KUIWindow.GetWindow<UI.FriendWindow>().InitializationFriendDialog();
//            KUIWindow.GetWindow<UI.FriendWindow>().InitializationFriendRight();
//        }
//        #endregion

//        #region UNITY

//        #endregion

//        #region 协议相关
//        /// <summary>
//        /// 客户端向服务器拉取好友列表
//        /// </summary>
//        /// <param name="callback"></param>
//        public void GetFriends(Callback callback)
//        {
//            KUser.GetFriends((code, message, data) =>
//            {
//                var protoDatas = data as ArrayList;
//                if (protoDatas != null)
//                {
//                    for (int i = 0; i < protoDatas.Count; i++)
//                    {
//                        if (protoDatas[0] is S2CRetFriendListResult)
//                        {
//                            var friendDatas = (S2CRetFriendListResult)protoDatas[0];
//                            List<FriendInfo> serverFriendList = new List<FriendInfo>();
//                            for (int j = 0; j < friendDatas.FriendList.Count; j++)
//                            {
//                                if (friendDatas.FriendList[j].PlayerId != 0)
//                                {
//                                    Debug.Log("[[KFriendManager] 拉取新好友，该好友未读消息数：" + friendDatas.FriendList[j].UnreadMessageNum + "已赠送：" + friendDatas.FriendList[j].IsZanToday + "剩余赠送时间：" + friendDatas.FriendList[j].LeftGiveSeconds + "可领取的点数：" + friendDatas.FriendList[j].FriendPoints);
//                                    serverFriendList.Add(friendDatas.FriendList[j]);
//                                    if (!_friendUnMsgNum.ContainsKey(friendDatas.FriendList[j].PlayerId))
//                                    {
//                                        _friendUnMsgNum.Add(friendDatas.FriendList[j].PlayerId, friendDatas.FriendList[j].UnreadMessageNum);
//                                    }
//                                    else {
//                                        _friendUnMsgNum[friendDatas.FriendList[j].PlayerId] = friendDatas.FriendList[j].UnreadMessageNum;
//                                    }
//                                }
//                            }
//                            List<FriendReq> serverFriendReqList = new List<FriendReq>();
//                            for (int j = 0; j < friendDatas.Reqs.Count; j++)
//                            {
//                                if (friendDatas.Reqs[j].PlayerId != 0)
//                                {
//                                    serverFriendReqList.Add(friendDatas.Reqs[j]);
//                                }
//                            }
//                            LeftPresentNum = friendDatas.LeftGivePointsNum;
//                            _arry_friends = serverFriendList.ToArray();
//                            _arry_friendReqs = serverFriendReqList.ToArray();
//                        }
//                    }
//                    pullServerData = true;
//                    if (callback != null)
//                    {
//                        callback(code, message, data);
//                    }
//                }
//            });
//        }
//        /// <summary>
//        /// 客户端向服务器搜索好友
//        /// </summary>
//        /// <param name="callback"></param>
//        public void SearchFriends(string searchdata, Callback callback) {
//            Debug.Log("输入搜索内容：" + searchdata);
//            KUser.SearchFriends(searchdata, (code, message, data) => {
//                var protoDatas = data as ArrayList;
//                if (protoDatas != null)
//                {
//                    for (int i = 0; i < protoDatas.Count; i++)
//                    {
//                        if (protoDatas[0] is S2CFriendSearchResult)
//                        {
//                            var friendDatas = (S2CFriendSearchResult)protoDatas[0];
//                            List<FriendInfo> serverFriendList = new List<FriendInfo>();
//                            for (int j = 0; j < friendDatas.Result.Count; j++)
//                            {
//                                if (friendDatas.Result[j].PlayerId != 0)
//                                {
//                                    serverFriendList.Add(friendDatas.Result[j]);
//                                }
//                            }
//                            _arry_friendSearchResults = serverFriendList.ToArray();
//                        }
//                    }
//                    if (callback != null)
//                    {
//                        callback(code, message, data);
//                    }
//                }
//            });
//        }
//        /// <summary>
//        /// 发送添加好友申请
//        /// </summary>
//        /// <param name="friendid"></param>
//        /// <param name="callback"></param>
//        public void AddFriend(int friendid, Callback callback) {
//            KUser.AddFriends(friendid, (code, message, data) => {
//                var protoDatas = data as ArrayList;
//                if (protoDatas != null)
//                {
//                    for (int i = 0; i < protoDatas.Count; i++)
//                    {
//                        if (protoDatas[0] is S2CAddFriendResult)
//                        {
//                            var friendDatas = (S2CAddFriendResult)protoDatas[0];
//                            LastAddFriendID = friendDatas.PlayerId;
//                        }
//                    }
//                    if (callback != null)
//                    {
//                        callback(code, message, data);
//                    }
//                }
//            });
//        }
//        /// <summary>
//        /// 同意添加好友的请求
//        /// </summary>
//        /// <param name="friendid"></param>
//        /// <param name="callback"></param>
//        public void C2SAgreeFriend(int friendid, Callback callback) {
//            KUser.C2SAgreeFriend(friendid, (code, message, data) => {
//                var protoDatas = data as ArrayList;
//                if (protoDatas != null)
//                {
//                    for (int i = 0; i < protoDatas.Count; i++)
//                    {
//                        if (protoDatas[0] is S2CAgreeFriendResult)
//                        {
//                            var friendDatas = (S2CAgreeFriendResult)protoDatas[0];
//                            FriendInfo oneNewFriend = new FriendInfo();
//                            oneNewFriend.PlayerId = friendDatas.PlayerId;
//                            oneNewFriend.Name = friendDatas.Name;
//                            oneNewFriend.Level = friendDatas.Level;
//                            oneNewFriend.Head = friendDatas.Head;
//                            int friendscore = friendDatas.Score;
//                            oneNewFriend.VipLevel = friendDatas.VipLevel;
//                            oneNewFriend.LastLogin = friendDatas.LastLogin;
//                            oneNewFriend.UnreadMessageNum = 0;
//                            List<FriendInfo> frdLst = new List<FriendInfo>(_arry_friends);
//                            bool dataIsInLst = false;
//                            for (int k = 0; k < _arry_friends.Length; k++)
//                            {
//                                if (_arry_friends[k].PlayerId == oneNewFriend.PlayerId)
//                                {
//                                    dataIsInLst = true;
//                                }
//                            }
//                            if (!dataIsInLst)
//                            {
//                                frdLst.Add(oneNewFriend);
//                                if (!_friendUnMsgNum.ContainsKey(oneNewFriend.PlayerId))
//                                {
//                                    _friendUnMsgNum.Add(oneNewFriend.PlayerId, oneNewFriend.UnreadMessageNum);
//                                }
//                                else
//                                {
//                                    _friendUnMsgNum[oneNewFriend.PlayerId] = oneNewFriend.UnreadMessageNum;
//                                }
//                            }
//                            _arry_friends = frdLst.ToArray();

//                            List<FriendReq> frdreqLst = new List<FriendReq>(_arry_friendReqs);
//                            bool dataIsInReqLst = false;
//                            for (int k = 0; k < frdreqLst.Count; k++)
//                            {
//                                if (_arry_friendReqs[k].PlayerId == oneNewFriend.PlayerId)
//                                {
//                                    dataIsInReqLst = true;
//                                    frdreqLst.Remove(frdreqLst[k]);
//                                }
//                            }
//                            if (dataIsInReqLst)
//                            {
//                                _arry_friendReqs = frdreqLst.ToArray();
//                            }
//                        }
//                    }
//                    if (callback != null)
//                    {
//                        callback(code, message, data);
//                    }
//                }
//            });
//        }
//        /// <summary>
//        /// 忽略添加好友
//        /// </summary>
//        /// <param name="friendid"></param>
//        /// <param name="callback"></param>
//        public void C2SRefuseFriend(int friendid, Callback callback)
//        {
//            KUser.C2SRefuseFriend(friendid, (code, message, data) => {
//                var protoDatas = data as ArrayList;
//                if (protoDatas != null)
//                {
//                    for (int i = 0; i < protoDatas.Count; i++)
//                    {
//                        if (protoDatas[0] is S2CRefuseFriendResult)
//                        {
//                            var friendDatas = (S2CRefuseFriendResult)protoDatas[0];
//                            FriendInfo oneNewFriend = new FriendInfo();
//                            oneNewFriend.PlayerId = friendDatas.PlayerId;
//                            oneNewFriend.Name = friendDatas.Name;
//                            List<FriendReq> frdreqLst = new List<FriendReq>(_arry_friendReqs);
//                            bool dataIsInReqLst = false;
//                            for (int k = 0; k < frdreqLst.Count; k++)
//                            {
//                                if (_arry_friendReqs[k].PlayerId == oneNewFriend.PlayerId)
//                                {
//                                    dataIsInReqLst = true;
//                                    frdreqLst.Remove(frdreqLst[k]);
//                                }
//                            }
//                            if (dataIsInReqLst)
//                            {
//                                _arry_friendReqs = frdreqLst.ToArray();
//                            }
//                        }
//                    }
//                    if (callback != null)
//                    {
//                        callback(code, message, data);
//                    }
//                }
//            });
//        }
//        /// <summary>
//        /// 删除好友
//        /// </summary>
//        /// <param name="friendid"></param>
//        /// <param name="callback"></param>
//        public void DeleteFriend(int friendid, Callback callback)
//        {
//            KUser.C2SRemoveFriend(friendid, (code, message, data) => {
//                var protoDatas = data as ArrayList;
//                if (protoDatas != null)
//                {
//                    for (int i = 0; i < protoDatas.Count; i++)
//                    {
//                        if (protoDatas[0] is S2CRemoveFriendResult)
//                        {
//                            var friendDatas = (S2CRemoveFriendResult)protoDatas[0];
//                            FriendInfo oneNewFriend = new FriendInfo();
//                            oneNewFriend.PlayerId = friendDatas.PlayerId;
//                            List<FriendInfo> frdLst = new List<FriendInfo>(_arry_friends);
//                            bool dataIsInLst = false;
//                            for (int k = 0; k < _arry_friends.Length; k++)
//                            {
//                                if (_arry_friends[k].PlayerId == oneNewFriend.PlayerId)
//                                {
//                                    dataIsInLst = true;
//                                }
//                            }
//                            if (dataIsInLst)
//                            {
//                                //frdLst.Remove(oneNewFriend);
//                                frdLst.Remove(frdLst.First(x => x.PlayerId == oneNewFriend.PlayerId));
//                                friendDialog.Remove(oneNewFriend.PlayerId);
//                            }
//                            _arry_friends = frdLst.ToArray();
//                        }
//                    }
//                    if (callback != null)
//                    {
//                        callback(code, message, data);
//                    }
//                }
//            });
//        }
//        /// <summary>
//        /// 拉取好友消息
//        /// </summary>
//        /// <param name="friendid"></param>
//        /// <param name="callback"></param>
//        public void C2SFriendPullUnreadMessage(int friendid, Callback callback)
//        {
//            KUser.C2SFriendPullUnreadMessage(friendid, (code, message, data) => {
//                var protoDatas = data as ArrayList;
//                if (protoDatas != null)
//                {
//                    for (int i = 0; i < protoDatas.Count; i++)
//                    {
//                        if (protoDatas[0] is S2CFriendPullUnreadMessageResult)
//                        {
//                            var friendDatas = (S2CFriendPullUnreadMessageResult)protoDatas[0];
//                            int friendDialogID = friendDatas.FriendId;
//                            IList<FriendChatData> theFrdDialog = new List<FriendChatData>();
//                            if (friendDatas.Data is IList<FriendChatData>)
//                            {
//                                theFrdDialog = friendDatas.Data;
//                            }
//                            if (friendDialog.ContainsKey(friendDialogID))
//                            {
//                                List<FriendRichDialogData> dialogs = new List<FriendRichDialogData>();
//                                for (int k = 0; k < theFrdDialog.Count; k++)
//                                {
//                                    FriendRichDialogData onedata = new FriendRichDialogData();
//                                    onedata.PlayerID = friendDialogID;
//                                    onedata.CreateTime = theFrdDialog[k].SendTime;
//                                    onedata.DialogContent = theFrdDialog[k].Content.ToArray();
//                                    dialogs.Add(onedata);
//                                }
//                                friendDialog[friendDialogID].AddRange(dialogs);
//                                currentFriendNewAddDialog.Clear();
//                                currentFriendNewAddDialog.Add(friendDialogID, dialogs);
//                            }
//                            else
//                            {
//                                List<FriendRichDialogData> dialogs = new List<FriendRichDialogData>();
//                                for (int k = 0; k < theFrdDialog.Count; k++)
//                                {
//                                    FriendRichDialogData onedata = new FriendRichDialogData();
//                                    onedata.PlayerID = friendDialogID;
//                                    onedata.CreateTime = theFrdDialog[k].SendTime;
//                                    onedata.DialogContent = theFrdDialog[k].Content.ToArray();
//                                    dialogs.Add(onedata);
//                                }
//                                friendDialog.Add(friendDialogID, dialogs);
//                            }
//                            if (theFrdDialog.Count > 0)
//                            {
//                                C2SFriendConfirmUnreadMessage(friendDialogID, theFrdDialog.Count, null);
//                            }
//                        }
//                    }
//                    if (callback != null && friendid == currentSelectFriend._FriendData.PlayerId)
//                    {
//                        callback(code, message, data);
//                    }
//                }
//            });
//        }       
//        /// <summary>
//        /// 向服务器拉取一次所有好友未读消息数量
//        /// </summary>
//        /// <param name="friendids"></param>
//        /// <param name="msgNum"></param>
//        /// <param name="callback"></param>
//        public void C2SFriendGetUnreadMessageNum(List<int> friendids, Callback callback)
//        {
//            if (friendids == null || friendids.Count == 0)
//            {
//                _friendUnMsgNum.Clear();
//                return;
//            }
//            KUser.C2SFriendGetUnreadMessageNum(friendids,(code, message, data) => {
//                var protoDatas = data as ArrayList;
//                if (protoDatas != null)
//                {
//                    for (int i = 0; i < protoDatas.Count; i++)
//                    {
//                        if (protoDatas[0] is S2CFriendGetUnreadMessageNumResult)
//                        {
//                            var friendDatas = (S2CFriendGetUnreadMessageNumResult)protoDatas[0];
//                            IList<FriendUnreadMessageNumData> unreadMsgNumLst = new List<FriendUnreadMessageNumData>();
//                            if (friendDatas.Data is IList<FriendUnreadMessageNumData>)
//                            {
//                                unreadMsgNumLst = friendDatas.Data;
//                            }
//                            for (int k = 0; k < unreadMsgNumLst.Count; k++)
//                            {
//                                if (_friendUnMsgNum.ContainsKey(unreadMsgNumLst[k].FriendId))
//                                {
//                                    _friendUnMsgNum[unreadMsgNumLst[k].FriendId] = unreadMsgNumLst[k].MessageNum;
//                                }
//                                else {
//                                    _friendUnMsgNum.Add(unreadMsgNumLst[k].FriendId,unreadMsgNumLst[k].MessageNum);
//                                }
//                            }
//                        }
//                    }
//                    if (callback != null)
//                    {
//                        callback(code, message, data);
//                    }
//                }
//            });
//        }
//        //向服务器同步已读消息数量：当前模式是：打开面板之后获取首次的好友未读消息数量，然后在心跳中更新好友未读消息数量，然后在打开某好友面板后向服务器获取好友未读消息，把获取到的消息数量发送给服务器
//        public void C2SFriendConfirmUnreadMessage(int friendid, int msgNum, Callback callback)
//        {
//            KUser.C2SFriendConfirmUnreadMessage(friendid, msgNum, (code, message, data) => {
//                var protoDatas = data as ArrayList;
//                if (protoDatas != null)
//                {
//                    for (int i = 0; i < protoDatas.Count; i++)
//                    {
//                        if (protoDatas[0] is S2CFriendConfirmUnreadMessageResult)
//                        {
//                            var friendDatas = (S2CFriendConfirmUnreadMessageResult)protoDatas[0];
//                            int friendDialogID = friendDatas.FriendId;
//                            if (_friendUnMsgNum.ContainsKey(friendDialogID))
//                            {
//                                //清除一次未读数字
//                                _friendUnMsgNum[friendDialogID] = 0;
//                            }
//                        }
//                    }
//                    if (callback != null)
//                    {
//                        callback(code, message, data);
//                    }
//                }
//            });
//        }
//        /// <summary>
//        /// 领取友情点数
//        /// </summary>
//        /// <param name="friendids"></param>
//        /// <param name="callback"></param>
//        public void C2SGetFriendPoints(List<int> friendids, Callback callback)
//        {
//            KUser.C2SGetFriendPoints(friendids, (code, message, data) => {
//                var protoDatas = data as ArrayList;
//                if (protoDatas != null)
//                {
//                    for (int i = 0; i < protoDatas.Count; i++)
//                    {
//                        if (protoDatas[0] is S2CGetFriendPointsResult)
//                        {
//                            var SendDatas = (S2CGetFriendPointsResult)protoDatas[0];
//                            if (SendDatas.PointsData is List<FriendPoints>)
//                            {
//                                IList<FriendPoints> serverdata = SendDatas.PointsData;
//                                for (int k = 0; k < serverdata.Count; k++)
//                                {
//                                    for (int j = 0; j < _arry_friends.Length; j++)
//                                    {
//                                        if (_arry_friends[j].PlayerId == serverdata[k].FriendId)
//                                        {
//                                            _arry_friends[j].FriendPoints = serverdata[k].Points;
//                                        }
//                                    }
//                                }
//                                _frdPsLst_getAllPoints = serverdata;
//                            }
//                        }
//                    }
//                    if (callback != null)
//                    {
//                        callback(code, message, data);
//                    }
//                }
//            });
//        }
//        /// <summary>
//        /// 赠送友情点数
//        /// </summary>
//        /// <param name="friendids"></param>
//        /// <param name="callback"></param>
//        public void C2SGiveFriendPoints(List<int> friendids, Callback callback)
//        {
//            KUser.C2SGiveFriendPoints(friendids, (code, message, data) => {
//                var protoDatas = data as ArrayList;
//                if (protoDatas != null)
//                {
//                    for (int i = 0; i < protoDatas.Count; i++)
//                    {
//                        if (protoDatas[i] is S2CGiveFriendPointsResult)
//                        {
//                            var SendDatas = (S2CGiveFriendPointsResult)protoDatas[i];
//                            if (SendDatas.PointsData is List<FriendPointsResult>)
//                            {
//                                IList<FriendPointsResult> serverdata = SendDatas.PointsData;
//                                for (int mm = 0; mm < friendids.Count; mm++)
//                                {
//                                    Debug.Log("[KFriendManager] [为侦测向好友赠送友情点数的数据]赠送好友ID:" + friendids[mm] + "回馈ID：" + serverdata[mm].FriendId + "点数：" + serverdata[mm].Points + "时间：" + serverdata[mm].RemainSeconds + "错误码：" + serverdata[mm].Error);
//                                }
//                                for (int k = 0; k < serverdata.Count; k++)
//                                {
//                                    for (int l = 0; l < _arry_friends.Length; l++)
//                                    {
//                                        if (_arry_friends[l].PlayerId == serverdata[k].FriendId)
//                                        {
//                                            //_arry_friends[l].FriendPoints = serverdata[k].Points;
//                                            //Debug.Log("[[KFriendManager]断点：赠送好友之后获取该好友的馈赠]" + _arry_friends[l].FriendPoints + "剩余赠送时间：" + serverdata[k].RemainSeconds);
//                                            if (serverdata[k].Error == 0)
//                                            {
//                                                PresentRewardPoints += serverdata[k].BackPoints;
//                                            }
//                                            Debug.Log("-><color=#9400D3>" + "[日志] [KFriendManager] [C2SGiveFriendPoints] 赠送友情点之后的回馈消息，Error：" + serverdata[k].Error + "，反馈点数：" + serverdata[k].BackPoints + "</color>");
//                                            _arry_friends[l].LeftGiveSeconds = serverdata[k].RemainSeconds;
//                                        }
//                                    }
//                                }
//                                _array_friendPointsResult = serverdata.ToArray();
//                            }
//                            LeftPresentNum = SendDatas.LeftGivePointsNum;
//                        }
//                    }
//                    if (callback != null)
//                    {
//                        callback(code, message, data);
//                    }
//                }
//            });
//        }
//        /// <summary>
//        /// 点赞
//        /// </summary>
//        /// <param name="friendid"></param>
//        /// <param name="callback"></param>
//        public void C2SZanPlayer(int friendid, Callback callback)
//        {
//            KUser.C2SZanPlayer(friendid, (code, message, data) => {
//                var protoDatas = data as ArrayList;
//                if (protoDatas != null)
//                {
//                    for (int i = 0; i < protoDatas.Count; i++)
//                    {
//                        if (protoDatas[i] is S2CZanPlayerResult)
//                        {
//                            var friendDatas = (S2CZanPlayerResult)protoDatas[i];
//                            int friendId = friendDatas.PlayerId;
//                            int friendzanNum = friendDatas.TotalZan;
//                            for (int k = 0; k < _arry_friends.Length; k++)
//                            {
//                                if (_arry_friends[k].PlayerId == friendId)
//                                {
//                                    _arry_friends[k].Zan = friendzanNum;
//                                    _arry_friends[k].IsZanToday = true;
//                                }
//                            }
//                            foreach (var item in KRankManager.Instance.Dict_allRankLst.Values)
//                            {
//                                if (item.Find(x => x.PlayerId == friendId) != null)
//                                {
//                                    item.Find(x => x.PlayerId == friendId).IsZaned = true;
//                                }
//                            }
//                            Debug.Log("-><color=#9400D3>" + "[日志] [KFriendManager] [C2SZanPlayer] 点赞目标玩家ID：" + friendid + "，返回玩家ID：" + friendId + "执行目标玩家被赞次数:" + friendzanNum);
//                        }
//                    }
//                    if (callback != null)
//                    {
//                        callback(code, message, data);
//                    }
//                }
//            });
//        }
//        /// <summary>
//        /// 向好友发送消息
//        /// </summary>
//        /// <param name="friendid"></param>
//        /// <param name="dialogcontent"></param>
//        /// <param name="callback"></param>
//        public void C2SFriendChat(int friendid, byte[] dialogcontent, Callback callback) {
//            KUser.C2SFriendChat(friendid, dialogcontent, (code, message, data) => {
//                var protoDatas = data as ArrayList;
//                if (protoDatas != null)
//                {
//                    for (int i = 0; i < protoDatas.Count; i++)
//                    {
//                        if (protoDatas[0] is S2CFriendChatResult)
//                        {
//                            var friendDatas = (S2CFriendChatResult)protoDatas[0];
//                            int friendId = friendDatas.PlayerId;
//                            FriendRichDialogData onedata = new FriendRichDialogData();
//                            onedata.PlayerID = KUser.PlayerId;
//                            onedata.DialogContent = friendDatas.Content.ToArray();
//                            onedata.CreateTime = GetTimeStamp(true);
//                            if (friendDialog.ContainsKey(friendId))
//                            {
//                                friendDialog[friendId].Add(onedata);
//                                List<FriendRichDialogData> oneLst = new List<FriendRichDialogData>();
//                                oneLst.Add(onedata);
//                                currentFriendNewAddDialog.Clear();
//                                currentFriendNewAddDialog.Add(friendId, oneLst);
//                            }
//                            else {
//                                List<FriendRichDialogData> oneLst = new List<FriendRichDialogData>();
//                                oneLst.Add(onedata);
//                                friendDialog.Add(friendId, oneLst);
//                                currentFriendNewAddDialog.Add(friendId, oneLst);
//                            }
//                        }
//                    }
//                    if (callback != null)
//                    {
//                        callback(code, message, data);
//                    }
//                }
//            });
//        }
//        /// <summary>
//        /// 罗列出一批供一键赠送的好友列表
//        /// </summary>
//        public List<int> IntArray_PresentTargetIDs { get {
//                List < int > friendIDs = new List<int>();
//                for (int i = 0; i < KFriendManager.Instance.Arry_Friends.Length; i++)
//                {
//                    if (KFriendManager.Instance.Arry_Friends[i].LeftGiveSeconds <= 0)
//                    {
//                        friendIDs.Add(KFriendManager.Instance.Arry_Friends[i].PlayerId);
//                    }
//                    if (friendIDs.Count >= LeftPresentNum)
//                    {
//                        break;
//                    }
//                }
//                return friendIDs;
//            }
//        }
//        /// <summary>
//        /// 向服务器索取一次拥有空寄养位置的好友列表,将赋值： KFriendManager.Instance.Lst_frdInfoHaseEmptyPosition
//        /// </summary>
//        /// <param name="callback"></param>
//        public void C2SFosterGetEmptySlotFriends(Callback callback)
//        {
//            KUser.C2SFosterGetEmptySlotFriends((code, message, data) =>
//            {
//                var protoDatas = data as ArrayList;
//                if (protoDatas != null)
//                {
//                    for (int i = 0; i < protoDatas.Count; i++)
//                    {
//                        if (protoDatas[0] is S2CFosterGetEmptySlotFriendsResult)
//                        {
//                            var friendDatas = (S2CFosterGetEmptySlotFriendsResult)protoDatas[0];
//                            _lst_frdInfoHaseEmptyPosition = new List<FriendInfo>();
//                            for (int j = 0; j < friendDatas.Friends.Count; j++)
//                            {
//                                if (friendDatas.Friends[j].PlayerId != 0)
//                                {
//                                    _lst_frdInfoHaseEmptyPosition.Add(friendDatas.Friends[j]);
//                                }
//                            }
//                        }
//                    }
//                    pullServerData = true;
//                    if (callback != null)
//                    {
//                        callback(code, message, data);
//                    }
//                }
//            });
//        }

//        #endregion
//        /// <summary>
//        /// 获取当前时间戳
//        /// </summary>
//        /// <param name="bflag">为真时获取10位时间戳,为假时获取13位时间戳.</param>
//        /// <returns></returns>
//        public static int GetTimeStamp(bool bflag = true)
//        {
//            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
//            int ret;
//            if (bflag)
//                ret = Convert.ToInt32(ts.TotalSeconds);
//            else
//                ret = Convert.ToInt32(ts.TotalMilliseconds);
//            return ret;
//        }
//    }
//    /// <summary>
//    /// 与好友对话的单个聊天内容的结构
//    /// </summary>
//    public class FriendRichDialogData{
//        public int PlayerID;
//        public byte[] DialogContent;
//        public int CreateTime;
//    }

//}
