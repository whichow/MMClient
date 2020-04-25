// ***********************************************************************
// Company          : 
// Author           : KimCh
// Created          : 
//
// Last Modified By : KimCh
// Last Modified On : 
// ***********************************************************************
namespace Game
{
    using System.Collections.Generic;
    using UnityEngine;
    using Callback = System.Action<int, string, object>;

    /// <summary>
    /// 用户相关
    /// </summary>
    public class KUser : KGameModule
    {
        #region 平台相关

        private static string _OpenId = string.Empty;
        /// <summary>Gets or sets the open identifier.</summary>
        public static string OpenID
        {
            get
            {
                if (string.IsNullOrEmpty(_OpenId))
                {
                    var postfix = UnityEngine.PlayerPrefs.GetString("account_postfix", "");
                    if (string.IsNullOrEmpty(postfix))
                    {
                        postfix = System.Guid.NewGuid().ToString();
                        UnityEngine.PlayerPrefs.SetString("account_postfix", postfix);
                    }
                    _OpenId = postfix;
                }
                return _OpenId;
            }
            set { _OpenId = value; }
        }

        private static string _OpenToken = string.Empty;
        /// <summary>Gets or sets the open token.</summary>
        public static string OpenToken
        {
            get
            {
                if (string.IsNullOrEmpty(_OpenToken))
                {
                    _OpenToken = UnityEngine.SystemInfo.deviceUniqueIdentifier;
                }
                return _OpenToken;
            }
            set { _OpenToken = value; }
        }

        /// <summary>Gets or sets the channel.</summary>
        public static int Channel
        {
            get;
            set;
        }

        /// <summary>Gets or sets the platform.</summary>
        public static int Platform
        {
            get
            {
#if ANDROID_MY
                return 9;
#else
                return 0;
#endif
            }
        }

        /// <summary>游戏账号ID</summary>
        public static string AccountID
        {
            get;
            set;
        }

        /// <summary>Gets the account token.</summary>
        public static string AccountToken
        {
            get;
            set;
        }

        /// <summary>
        /// 玩家id
        /// </summary>
        public static int PlayerId
        {
            get
            {
                return SelfPlayer != null ? SelfPlayer.id : 0;
            }
        }

        /// <summary>
        /// 玩家token
        /// </summary>
        public static string PlayerToken
        {
            get
            {
                return SelfPlayer != null ? SelfPlayer.token : string.Empty;
            }
        }

        public static KPlayer CurrPlayer
        {
            get;
            set;
        }

        public static KPlayer SelfPlayer
        {
            get;
            private set;
        }

        #endregion

        #region 协议相关

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        public static void Login(Callback callback)
        {
            KServer.Instance.Login(callback);
            Debuger.Log("用户帐号: " + _OpenId);
        }

        /// <summary>Logouts the specified callback.注销()</summary>
        public static void Logout(Callback callback)
        {
            KServer.Instance.Logout(callback);
        }

        public static void PullAllInfos(Callback callback)
        {
            KServer.Instance.PullInfos(callback);
        }
        public static void PullAllInfos1(Callback callback)
        {
            KServer.Instance.PullInfos1(callback);
        }
        public static void PullAllInfos2(Callback callback)
        {
            KServer.Instance.PullInfos2(callback);
        }

        public static void PullBaseInfo(Callback callback)
        {
            KServer.Instance.PullBaseInfo(callback);
        }

        public static void PullItemInfo(Callback callback)
        {
            KServer.Instance.PullItemInfo(callback);
        }

        public static void PullBuildingInfo(Callback callback)
        {
            KServer.Instance.BuildingGets(callback);
        }

        public static void PullCharacterInfo(Callback callback)
        {
            KServer.Instance.CatGetInfos(callback);
        }

        public static void ComposeCat(int catShopId, Callback callback)
        {
            KServer.Instance.CatCompose(catShopId, callback);
        }

        public static void FeedCat(int catId, int feedCount, Callback callback)
        {
            KServer.Instance.CatFeed(catId, feedCount, callback);
        }

        public static void CatUpstar(int catId, int[] costCats, Callback callback)
        {
            KServer.Instance.CatUpstar(catId, costCats, callback);
        }

        public static void CatSkillUpgrade(int catId, int[] costCatIds, Callback callback)
        {
            KServer.Instance.CatSkillUpgrade(catId, costCatIds, callback);
        }

        public static void CatRename(int catId, string name, Callback callback)
        {
            KServer.Instance.CatRename(catId, name, callback);
        }

        public static void LockCat(int catId, bool isLock, Callback callback)
        {
            KServer.Instance.CatLock(catId, isLock, callback);
        }

        /// <summary>
        /// 分解猫
        /// </summary>
        /// <param name="catId"></param>
        /// <param name="callback"></param>
        public static void CatDecompose(int[] catIds, Callback callback)
        {
            KServer.Instance.CatDecompose(catIds, callback);
        }

        /// <summary>
        /// 抽卡
        /// </summary>
        /// <param name="type">1.低级 2.中级 3.高级</param>
        /// <param name="count"></param>
        /// <param name="callback"></param>
        public static void DrawCard(int type, int count, Callback callback)
        {
            KServer.Instance.Draw(type, count, callback);
        }

        public static void PullLevelInfos(Callback callback)
        {
            KServer.Instance.PullLevelInfos(callback);
        }

        public static void StartLevel(Msg.ClientMessage.C2SStageBegin data, Callback callback)
        {
            KServer.Instance.StartLevel(data, callback);
        }

        public static void FinishLevel(Msg.ClientMessage.C2SStagePass data, Callback callback)
        {
            KServer.Instance.FinishLevel(data, callback);
        }

        public static void BuyItem(int id, int count, Callback callback)
        {
            KServer.Instance.BuyItem(id, count, callback);
        }

        public static void SellItem(int id, int count, Callback callback)
        {
            KServer.Instance.SellItem(id, count, callback);
        }

        public static void ShopGetItems(int shopId, Callback callback)
        {
            KServer.Instance.ShopGetItems(shopId, callback);
        }
        public static void UseItems(int shopId, int count, Callback callback)
        {
            KServer.Instance.UseItem(shopId, count, callback);
        }
        public static void PullFormulaInfo(Callback callback)
        {
            KServer.Instance.FormulaGetInfos(callback);
        }

        public static void ExchangeFormula(int id, Callback callback)
        {
            KServer.Instance.FormulaExchange(id, callback);
        }
        /// <summary>
        /// 购买每日体力
        /// </summary>
        public static void BuyDayPower(Callback callback)
        {
            KServer.Instance.BuyDayPower(callback);
        }

        /// <summary>
        /// 拉取正在打造的配方建筑
        /// </summary>
        /// <param name="callback"></param>
        public static void WorkshopGets(Callback callback)
        {
            KServer.Instance.WorkshopGets(callback);
        }

        public static void WorkshopBuySlot(int slotIndex, Callback callback)
        {
            KServer.Instance.WorkshopBuySlot(slotIndex, callback);
        }

        public static void WorkshopMakeFormula(int slotIndex, int formulaId, Callback callback)
        {
            KServer.Instance.WorkshopMakeFormula(slotIndex, formulaId, callback);
        }

        public static void WorkshopMakeCancel(int slotIndex, Callback callback)
        {
            KServer.Instance.WorkshopMakeCancel(slotIndex, callback);
        }

        public static void WorkshopSpeedUp(int slotIndex, Callback callback)
        {
            KServer.Instance.WorkshopSpeedUp(slotIndex, callback);
        }
        /// <summary>
        /// 收取
        /// </summary>
        /// <param name="slotIndex"></param>
        /// <param name="callback"></param>
        public static void WorkshopCollect(int slotIndex, Callback callback)
        {
            KServer.Instance.WorkshopCollect(slotIndex, callback);
        }

        public static void ExploreGetTasks(Callback callback)
        {
            KServer.Instance.ExploreGetTasks(callback);
        }

        public static void ExploreStart(int taskId, int[] catIds, Callback callback)
        {
            KServer.Instance.ExploreStart(taskId, catIds, callback);
        }

        public static void ExploreBreak(int taskId, Callback callback)
        {
            KServer.Instance.ExploreBreak(taskId, callback);
        }

        public static void ExploreComplete(int taskId, Callback callback)
        {
            KServer.Instance.ExploreComplete(taskId, callback);
        }

        public static void ExploreSave(int taskId, Callback callback)
        {
            KServer.Instance.ExploreSave(taskId, callback);
        }

        public static void ExploreDelete(int taskId, Callback callback)
        {
            KServer.Instance.ExploreDelete(taskId, callback);
        }

        public static void MissionGetDaily(Callback callback)
        {
            KServer.Instance.MissionGetDaily(callback);
        }

        public static void MissionGetArchievement(Callback callback)
        {
            KServer.Instance.MissionGetArchievement(callback);
        }

        public static void MissionRewardDaily(int id, Callback callback)
        {
            KServer.Instance.MissionRewardDaily(id, callback);
        }

        public static void MissionRewardArchievement(int id, Callback callback)
        {
            KServer.Instance.MissionRewardArchievement(id, callback);
        }

        /// <summary>
        /// 获取城建实例
        /// </summary>
        /// <param name="callback"></param>
        public static void BuildingGets(Callback callback)
        {
            KServer.Instance.BuildingGets(callback);
        }

        /// <summary>
        /// 客户端向服务器请求设置建筑
        /// </summary>
        /// <param name="callback"></param>
        public static void BuildingSet(Msg.ClientMessage.C2SSetBuilding data, Callback callback)
        {
            KServer.Instance.BuildingSet(data, callback);
        }
        /// <summary>
        /// 客户端向服务器移动建筑
        /// </summary>
        /// <param name="callback"></param>
        public static void BuildingMove(Msg.ClientMessage.C2SMoveBuilding data, Callback callback)
        {
            KServer.Instance.BuildingMove(data, callback);
        }

        /// <summary>
        /// 客户端向服务器请求转向建筑
        /// </summary>
        /// <param name="callback"></param>
        public static void BuildingChangeDirection(Msg.ClientMessage.C2SChgBuildingDir data, Callback callback)
        {
            KServer.Instance.BuildingChangeDirection(data, callback);
        }

        /// <summary>
        /// 客户端向服务器请求回收建筑
        /// </summary>
        /// <param name="callback"></param>
        public static void BuildingRecycle(int id, Callback callback)
        {
            KServer.Instance.BuildingRecycle(id, callback);
        }

        /// <summary>
        /// 客户端向服务器请求出售建筑
        /// </summary>
        /// <param name="callback"></param>
        public static void BuildingSell(int id, Callback callback)
        {
            KServer.Instance.BuildingSell(id, callback);
        }

        /// <summary>
        /// 客户端向服务器请求清除障碍
        /// </summary>
        /// <param name="callback"></param>
        public static void BuildingRemoveBlock(int id, Callback callback)
        {
            KServer.Instance.BuildingRemoveBlock(id, callback);
        }

        /// <summary>
        /// 客户端向服务器请求开启地图宝箱
        /// </summary>
        /// <param name="playerId">0为玩家自己，其它数值 是其它玩家id</param>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public static void BuildingOpenBox(int playerId, int id, Callback callback)
        {
            KServer.Instance.BuildingOpenBox(playerId, id, callback);
        }

        /// <summary>
        /// 客户端向服务器请求解锁地图区域
        /// </summary>
        /// <param name="id">区域Id</param>
        /// <param name="useStone">是否使用钻石快速解锁</param>
        /// <param name="callback"></param>
        public static void BuildingUnlockArea(int id, int useStone, Callback callback)
        {
            KServer.Instance.BuildingUnlockArea(id, useStone, callback);
        }

        /// <summary>
        /// 拉取农作物
        /// </summary>
        /// <param name="callback"></param>
        public static void FarmGets(Callback callback)
        {
            KServer.Instance.FarmGets(callback);
        }

        /// <summary>
        /// 种植作物
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="cropId"></param>
        /// <param name="callback"></param>
        public static void FarmSowing(int buildingId, int cropId, Callback callback)
        {
            KServer.Instance.FarmSowing(buildingId, cropId, callback);
        }

        /// <summary>
        /// 加速农田
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="callback"></param>
        public static void FarmSpeedUp(int buildingId, Callback callback)
        {
            KServer.Instance.FarmSpeedUp(buildingId, callback);
        }

        /// <summary>
        /// 收割
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="callback"></param>
        public static void FarmHarvest(int buildingId, Callback callback)
        {
            KServer.Instance.FarmHarvest(buildingId, callback);
        }

        /// <summary>
        /// 拉取猫舍
        /// </summary>
        /// <param name="callback"></param>
        public static void CatteryGets(Callback callback)
        {
            KServer.Instance.CatteryGets(callback);
        }

        /// <summary>
        /// 拉取单个猫舍
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public static void CatteryGet(int id, Callback callback)
        {
            KServer.Instance.CatteryGet(id, callback);
        }

        /// <summary>
        /// 猫舍收金
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public static void CatteryHarvest(int id, Callback callback)
        {
            KServer.Instance.CatteryHarvest(id, callback);
        }

        /// <summary>
        /// 猫舍开始升级
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public static void CatteryUpgrade(int id, Callback callback)
        {
            KServer.Instance.CatteryUpgrade(id, callback);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public static void CatteryComplete(int id, Callback callback)
        {
            KServer.Instance.CatteryComplete(id, callback);
        }
        /// <summary>
        /// 猫舍加速升级
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public static void CatterySpeedUp(int id, Callback callback)
        {
            KServer.Instance.CatterySpeedUp(id, callback);
        }

        /// <summary>
        /// 出售猫舍
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public static void CatterySell(int id, Callback callback)
        {
            KServer.Instance.CatterySell(id, callback);
        }

        /// <summary>
        /// 猫舍加猫
        /// </summary>
        /// <param name="id">建筑物ID</param>
        /// <param name="catId">猫ID</param>
        /// <param name="callback"></param>
        public static void CatteryAddCat(int id, int catId, Callback callback)
        {
            KServer.Instance.CatteryAddCat(id, catId, callback);
        }

        /// <summary>
        /// 猫舍减猫
        /// </summary>
        /// <param name="id"></param>
        /// <param name="catId"></param>
        /// <param name="callback"></param>
        public static void CatteryRemoveCat(int id, int catId, Callback callback)
        {
            KServer.Instance.CatteryRemoveCat(id, catId, callback);
        }

        /// <summary>
        /// 客户端向服务器拉取邮件列表
        /// </summary>
        /// <param name="callback"></param>
        public static void MailGets(Callback callback)
        {
            KServer.Instance.MailGets(callback);
        }

        /// <summary>
        /// 客户端向服务器发送领取附件的请求
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public static void MailGetAttach(int id, Callback callback)
        {
            KServer.Instance.MailGetAttach(id, callback);
        }

        /// <summary>
        /// 客户端向服务器发送删除邮件请求
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public static void MailDelete(int id, Callback callback)
        {
            KServer.Instance.MailDelete(id, callback);
        }

        /// <summary>
        /// 客户端向服务器设置邮件已读标志
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public static void MailRead(int id, Callback callback)
        {
            KServer.Instance.MailRead(id, callback);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public static void MailAgreeHelp(int id, Callback callback)
        {
            KServer.Instance.MailAgreeHelp(id, callback);
        }

        /// <summary>
        /// 客户端向服务器拉取图鉴列表
        /// </summary>
        /// <param name="callback"></param>
        public static void HandBookGets(Callback callback)
        {
            KServer.Instance.HandBookGets(callback);
        }

        /// <summary>
        /// 客户端向领取区域奖励
        /// </summary>
        /// <param name="callback"></param>
        public static void GetLandeAward(int id, Callback callback)
        {
            KServer.Instance.GetLandeAward(id, callback);
        }
        /// <summary>
        /// 请求解锁三消关卡
        /// </summary>
        /// <param name="type">// 解锁方式 0时间解锁 1星星解锁 2钻石解锁 3请求好友</param>
        /// <param name="chapterId">章节ID</param>
        /// <param name="firendId">好友Id</param>
        /// <param name="callback"></param>
        public static void ChapterUnlock(int type, int chapterId, int[] firendId, Callback callback)
        {
            Debuger.Log(type + "|" + chapterId + "|" + firendId);
            KServer.Instance.ChapterUnlock(type, chapterId, firendId, callback);
        }

        /// <summary>
        /// 活动获取
        /// </summary>
        public static void ActivityGets(Callback callback)
        {
            KServer.Instance.ActivityGets(callback);
        }

        /// <summary>
        /// 活动领奖
        /// </summary>
        /// <param name="id"></param>
        /// <param name="values"></param>
        /// <param name="callback"></param>
        public static void ActivityGetRewards(int id, int[] values, Callback callback)
        {
            KServer.Instance.ActivityGetRewards(id, values, callback);
        }

        /// <summary>
        /// 客户端向服务器拉取好友列表
        /// </summary>
        /// <param name="callback"></param>
        public static void GetFriends(Callback callback)
        {
            KServer.Instance.GetFriends(callback);
        }
        /// <summary>
        /// 客户端向服务器搜索好友
        /// </summary>
        /// <param name="callback"></param>
        public static void SearchFriends(string friendName, Callback callback)
        {
            KServer.Instance.SearchFriends(friendName, callback);
        }
        /// <summary>
        /// 客户端向服务器通过ID添加好友
        /// </summary>
        /// <param name="callback"></param>
        public static void AddFriends(int friendid, Callback callback)
        {
            KServer.Instance.AddFriends(friendid, callback);
        }
        /// <summary>
        /// 客户端同意某个好友的请求
        /// </summary>
        /// <param name="callback"></param>
        public static void C2SAgreeFriend(int friendid, Callback callback)
        {
            KServer.Instance.C2SAgreeOneFriend(friendid, callback);
        }
        /// <summary>
        /// 客户端拒绝某个好友的请求
        /// </summary>
        /// <param name="friendid"></param>
        /// <param name="callback"></param>
        public static void C2SRefuseFriend(int friendid, Callback callback)
        {
            KServer.Instance.C2SRefuseOneFriend(friendid, callback);
        }
        /// <summary>
        /// 客户端删除某个好友
        /// </summary>
        /// <param name="friendid"></param>
        /// <param name="callback"></param>
        public static void C2SRemoveFriend(int friendid, Callback callback)
        {
            KServer.Instance.C2SRemoveOneFriend(friendid, callback);
        }
        /// <summary>
        /// 客户端拉取某玩家的未读消息
        /// </summary>
        /// <param name="friendid"></param>
        /// <param name="callback"></param>
        public static void C2SFriendPullUnreadMessage(int friendid, Callback callback)
        {
            KServer.Instance.C2SOneFriendPullUnreadMessage(friendid, callback);
        }
        /// <summary>
        /// 客户端向服务器同步已读消息的数量
        /// </summary>
        /// <param name="friendid"></param>
        /// <param name="callback"></param>
        public static void C2SFriendConfirmUnreadMessage(int friendid, int msgNum, Callback callback)
        {
            KServer.Instance.C2SOneFriendConfirmUnreadMessage(friendid, msgNum, callback);
        }

        /// <summary>
        /// 向服务器拉取一次所有好友的未读消息数量
        /// </summary>
        /// <param name="friendids"></param>
        /// <param name="callback"></param>
        public static void C2SFriendGetUnreadMessageNum(List<int> friendids, Callback callback)
        {
            KServer.Instance.C2SFriendOneGetUnreadMessageNum(friendids, callback);
        }
        /// <summary>
        /// 向某好友赠送友情点
        /// </summary>
        /// <param name="friendid"></param>
        /// <param name="callback"></param>
        public static void C2SGiveFriendPoints(List<int> friendid, Callback callback)
        {
            KServer.Instance.C2SGiveOneFriendPoints(friendid, callback);
        }
        /// <summary>
        /// 领取某些好友赠送的友情点数
        /// </summary>
        /// <param name="friendid"></param>
        /// <param name="callback"></param>
        public static void C2SGetFriendPoints(List<int> friendid, Callback callback)
        {
            KServer.Instance.C2SGetOneFriendPoints(friendid, callback);
        }
        /// <summary>
        /// 好友界面给好友点赞
        /// </summary>
        /// <param name="friendid"></param>
        /// <param name="callback"></param>
        public static void C2SZanPlayer(int friendid, Callback callback)
        {
            KServer.Instance.C2SZanOnePlayer(friendid, callback);
        }
        /// <summary>
        /// 向好友发送消息
        /// </summary>
        /// <param name="friendid"></param>
        /// <param name="dialogcontent"></param>
        /// <param name="callback"></param>
        public static void C2SFriendChat(int friendid, byte[] dialogcontent, Callback callback)
        {
            KServer.Instance.C2SOneFriendChat(friendid, dialogcontent, callback);
        }

        /// <summary>
        /// 向服务器索取一次拥有空寄养位置的好友列表
        /// </summary>
        public static void C2SFosterGetEmptySlotFriends(Callback callback)
        {
            KServer.Instance.C2SOneFosterGetEmptySlotFriends(callback);
        }
        /// <summary>
        /// 拜访玩家
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="callback"></param>
        public static void VisitPlayer(int playerId, Callback callback)
        {
            KServer.Instance.VisitPlayer(playerId, callback);
        }

        ///// <summary>
        ///// 修改昵称
        ///// </summary>
        ///// <param name="name"></param>
        ///// <param name="callback"></param>
        //public static void ChangeName(string name, Callback callback)
        //{
        //    KServer.Instance.ChangeName(name, callback);
        //}

        ///// <summary>
        ///// 更改头像
        ///// </summary>
        ///// <param name="head"></param>
        ///// <param name="callback"></param>
        //public static void ChangeHead(string head, Callback callback)
        //{
        //    KServer.Instance.ChangeHead(head, callback);
        //}

        /// <summary>
        /// 向服务器拉取一次某排行榜数据
        /// </summary>
        /// <param name="RankType"></param>
        /// <param name="StartRank"></param>
        /// <param name="RankNum"></param>
        /// <param name="Param"></param>
        /// <param name="callback"></param>
        public static void C2SPullRankingList(int RankType, int StartRank, int RankNum, int Param, Callback callback)
        {
            KServer.Instance.C2SPullOneRankingList(RankType, StartRank, RankNum, Param, callback);
        }
        /// <summary>
        /// 向服务器索取被点中的猫咪的信息供打开猫咪信息面板时使用
        /// </summary>
        /// <param name="playerid"></param>
        /// <param name="catid"></param>
        /// <param name="callback"></param>
        public static void C2SPlayerCatInfo(int playerid, int catid, Callback callback)
        {
            KServer.Instance.C2SOnePlayerCatInfo(playerid, catid, callback);
        }
        /// <summary>
        /// 向服务器拉取一次世界聊天内容
        /// </summary>
        /// <param name="callback"></param>
        public static void C2SWorldChatMsgPull(Callback callback)
        {
            KServer.Instance.C2SOneWorldChatMsgPull(callback);
        }
        /// <summary>
        /// 在世界频道发送一次消息
        /// </summary>
        /// <param name="chatdata"></param>
        /// <param name="callback"></param>
        public static void C2SWorldChatSend(byte[] chatdata, Callback callback)
        {
            KServer.Instance.C2SOneWorldChatSend(chatdata, callback);
        }

        ///// <summary>
        ///// 验证订单
        ///// </summary>
        ///// <param name="orderData"></param>
        ///// <param name="callback"></param>
        //public static void VerifyOrder(Msg.ClientMessage.C2SPayOrder orderData, Callback callback)
        //{
        //    KServer.Instance.VerifyOrder(orderData, callback);
        //}

        #endregion

        #region MyRegion

        public static void CreateSelfPlayer(int playerId, string playerName, string playerAccount)
        {
            Debuger.Log("Player:" + playerId + " name " + playerName + " account " + playerAccount);
            SelfPlayer = new KPlayer(playerId, playerName, playerAccount, AccountToken);
        }

        public override void Load()
        {
            TextAsset tmpText;
            if (KAssetManager.Instance.TryGetExcelAsset("player", out tmpText))
            {
                if (tmpText)
                {
                    var tmpJson = tmpText.bytes.ToJsonTable();
                    KPlayer.Load(tmpJson);
                }
            }
        }

        #endregion
    }
}
