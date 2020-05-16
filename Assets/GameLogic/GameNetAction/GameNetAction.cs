/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.5
 * 
 * Author:          Coamy
 * Created:	        2019/3/19 10:10:10
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using Game;
using Game.Build;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Msg.ClientMessage;
using UnityEngine;
using Game.Match3;
using System.Collections.Generic;

namespace Game
{
    public partial class GameServer
    {

        public void C2SRequest(MessageDescriptor descriptor, IMessage msg)
        {
            Send(ProtocolHelper.GetMsgID(msg.GetType()), msg);
        }

        public void C2SRequest(IMessage msg)
        {
            Send(ProtocolHelper.GetMsgID(msg.GetType()), msg);
        }

        #region 进入游戏

        public void EnterGameRequest()
        {
            C2SEnterGameRequest req = new C2SEnterGameRequest();
            req.Acc = KUser.AccountID;
            //C2SRequest(C2SEnterGameRequest.Descriptor, req);
            C2SRequest(req);
        }

        public void EnterGameHandler(S2CEnterGameResponse obj)
        {
            KUser.CreateSelfPlayer(obj.PlayerId, obj.Acc, obj.Acc);

            //设置心跳开始
            _blEnterGame = true;

           // IAPSdk.Instance.Initialize();
        }

        public void EnterGameCompleteNotifyHandler(S2CEnterGameCompleteNotify obj)
        {
            var gameFsm = KFramework.FsmManager.GetFsm<GameFsm>();
            gameFsm.SendEvent(this, LoginState.kGetFinish);
        }

        #endregion

        #region 拉取数据

        public void GetInfoRequest(C2SGetInfo req)
        {
            C2SRequest(req);
        }

        #region 玩家信息

        public void ReqPlayerInfo()
        {
            C2SPlayerInfoRequest req = new C2SPlayerInfoRequest();
            C2SRequest(req);
        }

        public void ExePlayerInfo(S2CPlayerInfoResponse obj)
        {
            //var player = KUser.SelfPlayer;
            //player.coin = obj.Gold;
            //player.stone = obj.Diamond;
            //player.food = obj.CatFood;
            //player.charm = obj.CharmVal;
            //player.grade = obj.Level;
            //player.exp = obj.Exp;
            //player.curStar = obj.Star;
            //player.maxStar = obj.HistoricalMaxStar;
            //player.nickName = obj.Name;
            //player.headURL = player.GetHeadIcon(obj.Head);
            //player.headID = obj.Head;
            //player.maxLevel = obj.CurMaxStage;
            //player.maxUnlockLevel = obj.CurUnlockMaxStage;
            //player.power = obj.Spirit;
            //player.soulStone = obj.SoulStone;
            //player.friendPoint = obj.FriendPoints;
            //player.charmMadal = obj.CharmMetal;
            //player.changeNameCount = obj.ChangeNameNum;
            //player.buyPowerCount = obj.DayBuyTiLiCount;
        }

        #endregion

        #region 物品信息

        public void ReqGetItemInfos()
        {
            C2SGetItemInfos req = new C2SGetItemInfos();
            C2SRequest(req);
        }

        //public void ExeGetItemInfos(S2CGetItemInfos obj)
        //{
        //    foreach (var itemInfo in obj.Items)
        //    {
        //        KDatabase.SetInt(itemInfo.ItemCfgId, "count", itemInfo.ItemNum);
        //    }
        //    ////假装发过
        //    //KTutorialManager.Instance.ProcessData(null);
        //}

        //#region 物品变化
        //public void ItemsInfoUpdateHandler(S2CItemsInfoUpdate obj)
        //{
        //    foreach (var itemInfo in obj.Items)
        //    {
        //        KDatabase.SetInt(itemInfo.ItemCfgId, "count", itemInfo.ItemNum);
        //    }
        //}
        //#endregion

        #endregion

        #region 建筑信息

        public void GetBuildingInfosRequest()
        {
            C2SGetBuildingInfos req = new C2SGetBuildingInfos();
            C2SRequest(req);
        }

        public void GetBuildingInfosHandler(S2CGetBuildingInfos obj)
        {
            BuildingManager.Instance.ProcessCommon(obj);
        }

        #endregion

        #region 仓库建筑信息

        public void GetDepotBuildingInfosRequest()
        {
            C2SGetDepotBuildingInfos req = new C2SGetDepotBuildingInfos();
            C2SRequest(req);
        }

        public void GetDepotBuildingInfosHandler(S2CGetDepotBuildingInfos obj)
        {
            foreach (var itemInfo in obj.DepotBuilds)
            {
                KDatabase.SetInt(itemInfo.CfgId, "count", itemInfo.Num);
            }
        }

        #endregion

        #region 猫信息

        public void GetCatInfosRequest()
        {
            C2SGetCatInfos req = new C2SGetCatInfos();
            C2SRequest(req);
        }

        public void GetCatInfosHandler(S2CGetCatInfos obj)
        {
            KCatManager.Instance.ProcessCommon(obj);
        }

        #endregion

        #region 区域信息

        public void GetAreasInfosRequest()
        {
            C2SGetAreasInfos req = new C2SGetAreasInfos();
            C2SRequest(req);
        }

        public void GetAreasInfosHandler(S2CGetAreasInfos obj)
        {
            var areaTable = KDatabase.Database.GetTable(KDatabase.AREA_TABLE_NAME);
            foreach (var item in obj.Areas)
            {
                areaTable.SetRow(item.CfgId, 0, "");
            }

            if (AreaManager.Instance)
            {
                AreaManager.Instance.AreaDataUpdate();
            }
        }

        #endregion

        #region 关卡信息

        public void GetStageInfosRequest()
        {
            C2SGetStageInfos req = new C2SGetStageInfos();
            C2SRequest(req);
        }

        public void GetStageInfosHandler(S2CGetStageInfos obj)
        {
            LevelDataModel.Instance.SetCurLevelInfo(obj.CurMaxStage, obj.CurUnlockMaxStage);
            LevelDataModel.Instance.SetLevelStarAndScore(obj.Stages.ToArray());
            LevelDataModel.Instance.SetCurChapterUnlock(obj.CurUnlockStageId, obj.UnlockLeftSec);
        }

        #endregion

        #region 引导

        private GuideDM m_guideDM;

        /// <summary>
        /// 引导数据
        /// </summary>
        /// <param name="obj"></param>
        private void GuideDataHandler(S2CGuideDataResponse obj)
        {
            m_guideDM = GuideDM.Parser.ParseFrom(obj.Data.ToByteArray());
            if (m_guideDM == null)
            {
                m_guideDM = new GuideDM();
            }

            int guideID = PlayerPrefs.GetInt("Guide_ID_" + PlayerDataModel.Instance.mPlayerData.mPlayerID);
            int stepID = PlayerPrefs.GetInt("Guide_StepID_" + PlayerDataModel.Instance.mPlayerData.mPlayerID);
            if (guideID > 0)
            {
                m_guideDM.GuideID = guideID;
                m_guideDM.StepID = stepID;
            }

            if (m_guideDM.GuideID == 0 && PlayerDataModel.Instance.mPlayerData.mLevel == 1)
                m_guideDM.GuideID = 1;

            //GuideManager.Instance.SetGuideData(m_guideDM);
        }

        public void GuideDataSaveRequest(int guideID, int stepID)
        {
            if (m_guideDM == null)
            {
                m_guideDM = new GuideDM();
            }
            m_guideDM.GuideID = guideID;
            m_guideDM.StepID = stepID;

            C2SGuideDataSaveRequest req = new C2SGuideDataSaveRequest();
            req.Data = m_guideDM.ToByteString();
            C2SRequest(req);

            PlayerPrefs.SetInt("Guide_ID_" + PlayerDataModel.Instance.mPlayerData.mPlayerID, m_guideDM.GuideID);
            PlayerPrefs.SetInt("Guide_StepID_" + PlayerDataModel.Instance.mPlayerData.mPlayerID, m_guideDM.StepID);
            PlayerPrefs.Save();
        }

        public void GuideDataSaveHandler(S2CGuideDataSaveResponse obj)
        {
            //LocalDataMgr
        }
        #endregion

        #region 玩家
        //改名
        public void ReqChangeName(string name)
        {
            C2SPlayerChangeNameRequest req = new C2SPlayerChangeNameRequest();
            req.NewName = name;
            C2SRequest(req);
        }
        //换头像
        public void ReqChangeHead(int head)
        {
            C2SPlayerChangeHeadRequest req = new C2SPlayerChangeHeadRequest();
            req.NewHead = head;
            C2SRequest(req);
        }
        //关注
        public void ReqFocusData()
        {
            C2SFocusDataRequest req = new C2SFocusDataRequest();
            C2SRequest(req);
        }
        //关注玩家
        public void ReqFocusPlayer(int id)
        {
            C2SFocusPlayerRequest req = new C2SFocusPlayerRequest();
            req.PlayerId = id;
            C2SRequest(req);
        }
        //取消关注
        public void ReqFocusCancal(int id)
        {
            C2SFocusPlayerCancalRequest req = new C2SFocusPlayerCancalRequest();
            req.PlayerId = id;
            C2SRequest(req);
        }
        //解锁空间照片
        public void ReqCatUnlock(int playerId,int catId)
        {
            C2SSpaceCatUnlockRequest req = new C2SSpaceCatUnlockRequest();
            req.PlayerId = playerId;
            req.CatId = catId;
            C2SRequest(req);
        }
        //设置空间照片 增加照片 索引+猫ID   索引=-1 删除照片
        public void ReqSetSpacePicture(int catId,bool isCancel)
        {
            C2SMyPictureSetRequest req = new C2SMyPictureSetRequest();
            req.CatId = catId;
            req.IsCancel = isCancel;
            C2SRequest(req);
        }
        //查看其它玩家空间
        public void ReqSpaceOther(int playerId)
        {
            C2SSpaceDataRequest req = new C2SSpaceDataRequest();
            req.PlayerId = playerId;
            C2SRequest(req);
        }
        //获取形象数据
        public void ReqFashionData()
        {
            C2SSpaceFashionDataRequest req = new C2SSpaceFashionDataRequest();
            C2SRequest(req);
        }
        //性别设置
        public void ReqSetGender(int gender)
        {
            C2SSpaceSetGenderRequest req = new C2SSpaceSetGenderRequest();
            req.Gender = gender;
            C2SRequest(req);
        }
        //形象保存
        public void ReqFashionSace(List<int> fashionId)
        {
            C2SSpaceFashionSaveRequest req = new C2SSpaceFashionSaveRequest();
            req.FashionIds.AddRange(fashionId);
            C2SRequest(req);
        }
        #endregion

        #region 背包
        //出售物品
        public void ReqSellItem(int id,int num)
        {
            C2SSellItem req = new C2SSellItem();
            req.ItemId = id;
            req.ItemNum = num;
            C2SRequest(req);
        }
        public void ReqUseItem(int id)
        {
            C2SUseItem req = new C2SUseItem();
            req.ItemCfgId = id;
            req.ItemNum = 1;
            C2SRequest(req);

        }
        #endregion

        #region 任务
        //任务数据
        public void ReqTaskData(int type)
        {
            C2STaskDataRequest req = new C2STaskDataRequest();
            req.TaskType = type;
            C2SRequest(req);
        }
        //任务奖励
        public void ReqTaskReward(int id)
        {
            C2STaskRewardRequest req = new C2STaskRewardRequest();
            req.TaskId = id;
            C2SRequest(req);
        }
        #endregion

        #region 排行榜
        //排行榜数据
        public void ReqRankData(int type, int startRank, int rankNum, params int[] args)
        {
            C2SRankListRequest req = new C2SRankListRequest();
            req.RankType = type;
            req.StartRank = startRank;
            req.RankNum = rankNum;
            if (args != null && args.Length > 0)
            {
                for (int i = 0; i < args.Length; i++)
                    req.Params.Add(args[i]);
            }
            C2SRequest(req);
        }
        #endregion

        #region 猫
        //猫改名
        public void ReqCatNick(int id,string name)
        {
            C2SCatRenameNick req = new C2SCatRenameNick();
            req.CatId = id;
            req.NewNick = name;
            C2SRequest(req);
        }
        public void ReqCatDecompose(List<int> lstId)
        {
            C2SCatDecompose req = new C2SCatDecompose();
            req.CatId.AddRange(lstId);
            C2SRequest(req);
        }

        public void CatsInfoUpdateHandler(S2CCatsInfoUpdate obj)
        {
            KCatManager.Instance.ProcessCommon(obj);
        }
        #endregion

        #region 好友
        //好友数据请求
        public void ReqFriendData()
        {
            C2SGetFriendList req = new C2SGetFriendList();
            C2SRequest(req);
        }
        //ID或昵称查找玩家
        public void ReqFriendSearch(string key)
        {
            C2SFriendSearch req = new C2SFriendSearch();
            req.Key = key;
            C2SRequest(req);
        }
        //ID添加好友
        public void ReqAddFriendById(int id)
        {
            C2SAddFriendByPId req = new C2SAddFriendByPId();
            req.PlayerId = id;
            C2SRequest(req);
        }
        //同意添加好友
        public void ReqAddFriend(int id)
        {
            C2SAgreeFriend req = new C2SAgreeFriend();
            req.PlayerId = id;
            C2SRequest(req);
        }
        //拒绝添加好友
        public void ReqRefuseFirend(int id)
        {
            C2SRefuseFriend req = new C2SRefuseFriend();
            req.PlayerId = id;
            C2SRequest(req);
        }
        //删除好友
        public void ReqRemoveFirend(int id)
        {
            C2SRemoveFriend req = new C2SRemoveFriend();
            req.PlayerId = id;
            C2SRequest(req);
        }
        //赠送友情点
        public void ReqGiveFriendPoint(List<int> lstId)
        {
            C2SGiveFriendPoints req = new C2SGiveFriendPoints();
            req.FriendId.AddRange(lstId);
            C2SRequest(req);
        }
        //收取友情点
        public void ReqGetFriendPoint(List<int> lstId)
        {
            C2SGetFriendPoints req = new C2SGetFriendPoints();
            req.FriendId.AddRange(lstId);
            C2SRequest(req);
        }
        #endregion

        #region 商店
        //商店数据
        public void ReqShopData(int shopId)
        {
            C2SShopItems req = new C2SShopItems();
            req.ShopId = shopId;
            C2SRequest(req);
        }
        //购买商品
        public void ReqBuyShopItem(int itemId,int itemNum)
        {
            C2SBuyShopItem req = new C2SBuyShopItem();
            req.ItemId = itemId;
            req.ItemNum = itemNum;
            C2SRequest(req);
        }
        #endregion

        #region 邮件
        //邮件数据
        public void ReqMailData()
        {
            C2SMailListRequest req = new C2SMailListRequest();
            C2SRequest(req);
        }
        //邮件详情
        public void ReqMailDetail(List<int> lstId)
        {
            C2SMailDetailRequest req = new C2SMailDetailRequest();
            req.Ids.AddRange(lstId);
            C2SRequest(req);
        }
        //获取附件
        public void ReqMailGetAtt(List<int> lstId)
        {
            C2SMailGetAttachedItemsRequest req = new C2SMailGetAttachedItemsRequest();
            req.MailIds.AddRange(lstId);
            C2SRequest(req);
        }
        //删除邮件
        public void ReqMailDelete(List<int> lstId)
        {
            C2SMailDeleteRequest req = new C2SMailDeleteRequest();
            req.MailIds.AddRange(lstId);
            C2SRequest(req);
        }
        #endregion

        #region 充值
        //充值数据
        public void ReqChargeData()
        {
            C2SChargeDataRequest req = new C2SChargeDataRequest();
            C2SRequest(req);
        }
        //首充领奖
        public void ReqChargeReward()
        {
            C2SChargeFirstAwardRequest req = new C2SChargeFirstAwardRequest();
            C2SRequest(req);
        }
        #endregion

        #region 签到
        //签到数据
        public void ReqSignData()
        {
            C2SSignDataRequest req = new C2SSignDataRequest();
            C2SRequest(req);
        }
        //签到领奖
        public void ReqSignReward(int index)
        {
            C2SSignAwardRequest req = new C2SSignAwardRequest();
            req.Index = index;
            C2SRequest(req);
        }
        #endregion

        #region 点赞
        public void ReqZanPlayer(int id)
        {
            C2SZanPlayer req = new C2SZanPlayer();
            req.PlayerId = id;
            C2SRequest(req);
        }
        #endregion

        #region 建筑物变化

        public void BuildingsInfoUpdateHandler(S2CBuildingsInfoUpdate obj)
        {
            BuildingManager.Instance.ProcessCommon(obj);
        }

        #endregion

        #region 仓库建筑物变化

        public void DepotBuildingInfoUpdateHandler(S2CDepotBuildingInfoUpdate obj)
        {
            foreach (var itemInfo in obj.Buildings)
            {
                KDatabase.SetInt(itemInfo.CfgId, "count", itemInfo.Num);
            }
        }

        #endregion

        #endregion

        #region 系统公告通知

        public void AnouncementNotifyHandler(S2CAnouncementNotify obj)
        {
            //KChatManager.Instance.Process(protoData);
        }

        #endregion

        #region 断线重连

        public void ReqReconnect()
        {
            C2SReconnectRequest req = new C2SReconnectRequest();
            C2SRequest(req);
        }

        public void ExeReconnect(S2CReconnectResponse obj)
        {
            KUser.AccountToken = obj.NewToken;
        }
        
        #endregion
    }
}