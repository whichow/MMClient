using Google.Protobuf;
using Msg.ClientMessage;

namespace Game
{
    public partial class GameServer : ServerBase
    {
        protected override void InitNetMsgHandle()
        {
            _blEnable = false;
            base.InitNetMsgHandle();

            RegisterNetMsgType<S2CHeartbeat>(S2CHeartbeat.Descriptor, HeartBeatHandler);
            RegisterNetMsgType<S2CReconnectResponse>(S2CReconnectResponse.Descriptor, ExeReconnect);
            
            #region 进入游戏
            RegisterNetMsgType<S2CEnterGameResponse>(S2CEnterGameResponse.Descriptor, PlayerDataModel.Instance.ExeEnterGame);
            RegisterNetMsgType<S2CEnterGameCompleteNotify>(S2CEnterGameCompleteNotify.Descriptor, EnterGameCompleteNotifyHandler);
            #endregion

            #region 拉取数据
            RegisterNetMsgType<S2CGetBuildingInfos>(S2CGetBuildingInfos.Descriptor, GetBuildingInfosHandler);
            RegisterNetMsgType<S2CGetDepotBuildingInfos>(S2CGetDepotBuildingInfos.Descriptor, GetDepotBuildingInfosHandler);
            RegisterNetMsgType<S2CGetAreasInfos>(S2CGetAreasInfos.Descriptor, GetAreasInfosHandler);
            RegisterNetMsgType<S2CGetStageInfos>(S2CGetStageInfos.Descriptor, GetStageInfosHandler);
            #endregion

            //引导数据
            RegisterNetMsgType<S2CGuideDataResponse>(S2CGuideDataResponse.Descriptor, GuideDataHandler);
            RegisterNetMsgType<S2CGuideDataSaveResponse>(S2CGuideDataSaveResponse.Descriptor, GuideDataSaveHandler);

            //改名
            RegisterNetMsgType<S2CPlayerInfoResponse>(S2CPlayerInfoResponse.Descriptor, PlayerDataModel.Instance.ExePlayerData);
            RegisterNetMsgType<S2CPlayerChangeNameResponse>(S2CPlayerChangeNameResponse.Descriptor, PlayerDataModel.Instance.ExeChangeName);
            RegisterNetMsgType<S2CPlayerChangeHeadResponse>(S2CPlayerChangeHeadResponse.Descriptor, PlayerDataModel.Instance.ExeChangeHead);
            RegisterNetMsgType<S2CChgName>(S2CChgName.Descriptor, ChangeNameHandler);
            //空间
            RegisterNetMsgType<S2CFocusDataResponse>(S2CFocusDataResponse.Descriptor, SpaceDataModel.Instance.ExeFocusData);
            RegisterNetMsgType<S2CFocusPlayerResponse>(S2CFocusPlayerResponse.Descriptor, SpaceDataModel.Instance.ExeFocusPlayer);
            RegisterNetMsgType<S2CFocusPlayerCancelResponse>(S2CFocusPlayerCancelResponse.Descriptor, SpaceDataModel.Instance.ExeFocusCancal);
            RegisterNetMsgType<S2CMyPictureDataResponse>(S2CMyPictureDataResponse.Descriptor, SpaceDataModel.Instance.ExePictureData);
            RegisterNetMsgType<S2CMyPictureSetResponse>(S2CMyPictureSetResponse.Descriptor, SpaceDataModel.Instance.ExeSetPicture);
            RegisterNetMsgType<S2CSpaceDataResponse>(S2CSpaceDataResponse.Descriptor, SpaceDataModel.Instance.ExeSpaceOtherData);
            RegisterNetMsgType<S2CSpaceSetGenderResponse>(S2CSpaceSetGenderResponse.Descriptor, SpaceDataModel.Instance.ExeSetGender);
            RegisterNetMsgType<S2CSpaceFashionDataResponse>(S2CSpaceFashionDataResponse.Descriptor, SpaceDataModel.Instance.ExeFashionData);
            RegisterNetMsgType<S2CSpaceFashionSaveResponse>(S2CSpaceFashionSaveResponse.Descriptor, SpaceDataModel.Instance.ExeFashionSave);
            RegisterNetMsgType<S2CSpaceCatUnlockResponse>(S2CSpaceCatUnlockResponse.Descriptor, SpaceDataModel.Instance.ExeCatUnlock);
            //商店
            RegisterNetMsgType<S2CShopItemsResult>(S2CShopItemsResult.Descriptor, ShopDataModel.Instance.ExeShopData);
            RegisterNetMsgType<S2CBuyShopItemResult>(S2CBuyShopItemResult.Descriptor, ShopDataModel.Instance.ExeBuyItem);
            //帮助别人次数
            RegisterNetMsgType<S2CRetDayHelpUnlockCount>(S2CRetDayHelpUnlockCount.Descriptor, DayHelpUnlockCountHandler);

            #region 数据改变
            //背包数据
            RegisterNetMsgType<S2CGetItemInfos>(S2CGetItemInfos.Descriptor, BagDataModel.Instance.ExeBagData);
            RegisterNetMsgType<S2CItemsInfoUpdate>(S2CItemsInfoUpdate.Descriptor, BagDataModel.Instance.ExeUpdateItem);
            RegisterNetMsgType<S2CSellItemResult>(S2CSellItemResult.Descriptor, BagDataModel.Instance.ExeSellItem);
            RegisterNetMsgType<S2CUseItem>(S2CUseItem.Descriptor, BagDataModel.Instance.ExeUseItem);
            //猫数据
            RegisterNetMsgType<S2CGetCatInfos>(S2CGetCatInfos.Descriptor, CatDataModel.Instance.ExeCatData);
            RegisterNetMsgType<S2CCatsInfoUpdate>(S2CCatsInfoUpdate.Descriptor, CatDataModel.Instance.ExeCatUpdate);
            RegisterNetMsgType<S2CCatRenameNickResult>(S2CCatRenameNickResult.Descriptor, CatDataModel.Instance.ExeCatNick);
            RegisterNetMsgType<S2CCatDecomposeResult>(S2CCatDecomposeResult.Descriptor, CatDataModel.Instance.ExeCatDecompose);
            //建筑物变化
            RegisterNetMsgType<S2CBuildingsInfoUpdate>(S2CBuildingsInfoUpdate.Descriptor, BuildingsInfoUpdateHandler);
            //仓库建筑物变化
            RegisterNetMsgType<S2CDepotBuildingInfoUpdate>(S2CDepotBuildingInfoUpdate.Descriptor, DepotBuildingInfoUpdateHandler);
            //任务数据
            RegisterNetMsgType<S2CTaskDataResponse>(S2CTaskDataResponse.Descriptor, TaskDataModel.Instance.ExeTaskData);
            RegisterNetMsgType<S2CTaskRewardResponse>(S2CTaskRewardResponse.Descriptor, TaskDataModel.Instance.ExeTaskReward);
            RegisterNetMsgType<S2CTaskValueNotify>(S2CTaskValueNotify.Descriptor, TaskDataModel.Instance.ExeTaskNotify);
            //排行榜数据
            RegisterNetMsgType<S2CRankListResponse>(S2CRankListResponse.Descriptor, RankDataModel.Instance.ExeRankData);
            //好友
            RegisterNetMsgType<S2CRetFriendListResult>(S2CRetFriendListResult.Descriptor, FriendDataModel.Instance.ExeFriendData);
            RegisterNetMsgType<S2CAddFriendResult>(S2CAddFriendResult.Descriptor, FriendDataModel.Instance.ExeAddFriend);
            RegisterNetMsgType<S2CFriendSearchResult>(S2CFriendSearchResult.Descriptor, FriendDataModel.Instance.ExeFriendSearch);
            RegisterNetMsgType<S2CFriendStateNotify>(S2CFriendStateNotify.Descriptor, FriendDataModel.Instance.ExeFriendNotify);
            RegisterNetMsgType<S2CFriendReqNotify>(S2CFriendReqNotify.Descriptor, FriendDataModel.Instance.ExeFriendReqNotify);
            RegisterNetMsgType<S2CAgreeFriendResult>(S2CAgreeFriendResult.Descriptor, FriendDataModel.Instance.ExeAgreeFriend);
            RegisterNetMsgType<S2CRefuseFriendResult>(S2CRefuseFriendResult.Descriptor, FriendDataModel.Instance.ExeRefuseFriend);
            RegisterNetMsgType<S2CRemoveFriendResult>(S2CRemoveFriendResult.Descriptor, FriendDataModel.Instance.ExeRemoveFriend);
            RegisterNetMsgType<S2CGiveFriendPointsResult>(S2CGiveFriendPointsResult.Descriptor, FriendDataModel.Instance.ExeGiveFriendPoint);
            RegisterNetMsgType<S2CGetFriendPointsResult>(S2CGetFriendPointsResult.Descriptor, FriendDataModel.Instance.ExeGetFriendPoint);
            RegisterNetMsgType<S2CZanPlayerResult>(S2CZanPlayerResult.Descriptor, FriendDataModel.Instance.ExeZanPlayer);
            //邮件
            RegisterNetMsgType<S2CMailListResponse>(S2CMailListResponse.Descriptor, MailDataModel.Instance.ExeMailData);
            RegisterNetMsgType<S2CMailDetailResponse>(S2CMailDetailResponse.Descriptor, MailDataModel.Instance.ExeMailDetail);
            RegisterNetMsgType<S2CMailGetAttachedItemsResponse>(S2CMailGetAttachedItemsResponse.Descriptor, MailDataModel.Instance.ExeMailGetAtt);
            RegisterNetMsgType<S2CMailDeleteResponse>(S2CMailDeleteResponse.Descriptor, MailDataModel.Instance.ExeMailDelete);
            RegisterNetMsgType<S2CMailsNewNotify>(S2CMailsNewNotify.Descriptor, MailDataModel.Instance.ExeNewNotify);
            //充值
            RegisterNetMsgType<S2CChargeDataResponse>(S2CChargeDataResponse.Descriptor, ActivityDataModel.Instance.ExeChargeData);
            RegisterNetMsgType<S2CChargeFirstAwardResponse>(S2CChargeFirstAwardResponse.Descriptor, ActivityDataModel.Instance.ExeChargeReward);
            RegisterNetMsgType<S2CChargeFirstRewardNotify>(S2CChargeFirstRewardNotify.Descriptor, ActivityDataModel.Instance.ExeChargeNotify);
            //签到
            RegisterNetMsgType<S2CSignDataResponse>(S2CSignDataResponse.Descriptor, ActivityDataModel.Instance.ExeSignData);
            RegisterNetMsgType<S2CSignAwardResponse>(S2CSignAwardResponse.Descriptor, ActivityDataModel.Instance.ExeSignReward);
            #endregion

            RegisterNetMsgType<S2COtherPlaceLogin>(S2COtherPlaceLogin.Descriptor, MsgHandler);

            RegisterNetMsgType<S2CGetDiamond>(S2CGetDiamond.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CGetCharmVal>(S2CGetCharmVal.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CGetZan>(S2CGetZan.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CGetCatFood>(S2CGetCatFood.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CGetSpirit>(S2CGetSpirit.Descriptor, MsgHandler);

            //关卡通关
            RegisterNetMsgType<S2CStagePass>(S2CStagePass.Descriptor, ExeStagePass);
            RegisterNetMsgType<S2CChapterUnlock>(S2CChapterUnlock.Descriptor, ExeChapterUnlock);
            RegisterNetMsgType<S2CRetCurHelpReqPIds>(S2CRetCurHelpReqPIds.Descriptor, MsgHandler);

            RegisterNetMsgType<S2COpenChest>(S2COpenChest.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CBuyChest>(S2CBuyChest.Descriptor, MsgHandler);
            //RegisterNetMsgType<S2CPayOrder>(S2CPayOrder.Descriptor, MsgHandler);
            //RegisterNetMsgType<S2CRankItems>(S2CRankItems.Descriptor, MsgHandler);

            //RegisterNetMsgType<S2CMailList>(S2CMailList.Descriptor, MsgHandler);
            //RegisterNetMsgType<S2CNoticeAdd>(S2CNoticeAdd.Descriptor, MsgHandler);
            //RegisterNetMsgType<S2CNoticeList>(S2CNoticeList.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CRetOptions>(S2CRetOptions.Descriptor, MsgHandler);
            //RegisterNetMsgType<S2CSyncDialyTask>(S2CSyncDialyTask.Descriptor, MsgHandler);
            //RegisterNetMsgType<S2CNotifyTaskValueChg>(S2CNotifyTaskValueChg.Descriptor, MsgHandler);
            //RegisterNetMsgType<S2CSyncAchieveData>(S2CSyncAchieveData.Descriptor, MsgHandler);
            //RegisterNetMsgType<S2CNotifyAchieveValueChg>(S2CNotifyAchieveValueChg.Descriptor, MsgHandler);
            //RegisterNetMsgType<S2CRetTaskReward>(S2CRetTaskReward.Descriptor, MsgHandler);
            //RegisterNetMsgType<S2CRetAchieveReward>(S2CRetAchieveReward.Descriptor, MsgHandler);
            //RegisterNetMsgType<S2CChangeHead>(S2CChangeHead.Descriptor, MsgHandler);
            //RegisterNetMsgType<S2CRetPlayerInfo>(S2CRetPlayerInfo.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CSyncSignInfo>(S2CSyncSignInfo.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CDaySign>(S2CDaySign.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CRetDaySignSumReward>(S2CRetDaySignSumReward.Descriptor, MsgHandler);
            //RegisterNetMsgType<S2CSyncFirstPayState>(S2CSyncFirstPayState.Descriptor, MsgHandler);
            //RegisterNetMsgType<S2CRetFirstPayReward>(S2CRetFirstPayReward.Descriptor, MsgHandler);
            //RegisterNetMsgType<S2CSyncSevenActivity>(S2CSyncSevenActivity.Descriptor, MsgHandler);
            //RegisterNetMsgType<S2CNotifySevenActValueChg>(S2CNotifySevenActValueChg.Descriptor, MsgHandler);
            //RegisterNetMsgType<S2CRetSevenActReward>(S2CRetSevenActReward.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CSetBuilding>(S2CSetBuilding.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CGetBackBuilding>(S2CGetBackBuilding.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CSellBuilding>(S2CSellBuilding.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CRemoveBlock>(S2CRemoveBlock.Descriptor, MsgHandler);
            RegisterNetMsgType<S2COpenMapChest>(S2COpenMapChest.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CUnlockArea>(S2CUnlockArea.Descriptor, MsgHandler);
            //抽奖
            RegisterNetMsgType<S2CDrawResult>(S2CDrawResult.Descriptor, DrawResultHandler);
            RegisterNetMsgType<S2CComposeCatResult>(S2CComposeCatResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CItemResourceValue>(S2CItemResourceValue.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CItemResourceResult>(S2CItemResourceResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CShopNeedRefreshNotify>(S2CShopNeedRefreshNotify.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CCatFeedResult>(S2CCatFeedResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CCatUpgradeStarResult>(S2CCatUpgradeStarResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CCatSkillLevelUpResult>(S2CCatSkillLevelUpResult.Descriptor, MsgHandler);
            //RegisterNetMsgType<S2CRenameCatNickResult>(S2CRenameCatNickResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CCatLockResult>(S2CCatLockResult.Descriptor, MsgHandler);
            //关卡开始
            RegisterNetMsgType<S2CStageBeginResult>(S2CStageBeginResult.Descriptor, StageBeginResultHandler);
            RegisterNetMsgType<S2CGetMakingFormulaBuildingsResult>(S2CGetMakingFormulaBuildingsResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CExchangeBuildingFormulaResult>(S2CExchangeBuildingFormulaResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CGetFormulasResult>(S2CGetFormulasResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CMakeFormulaBuildingResult>(S2CMakeFormulaBuildingResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CBuyMakeBuildingSlotResult>(S2CBuyMakeBuildingSlotResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CSpeedupMakeBuildingResult>(S2CSpeedupMakeBuildingResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CGetCompletedFormulaBuildingResult>(S2CGetCompletedFormulaBuildingResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CCancelMakingFormulaBuildingResult>(S2CCancelMakingFormulaBuildingResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CGetCropsResult>(S2CGetCropsResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CPlantCropResult>(S2CPlantCropResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CCropSpeedupResult>(S2CCropSpeedupResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CHarvestCropResult>(S2CHarvestCropResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CHarvestCropsResult>(S2CHarvestCropsResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CRetAllExpedition>(S2CRetAllExpedition.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CStartExpedition>(S2CStartExpedition.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CGetExpeditionReward>(S2CGetExpeditionReward.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CGetCatHousesInfoResult>(S2CGetCatHousesInfoResult.Descriptor, MsgHandler);
            //获取单个猫舍
            RegisterNetMsgType<S2CGetCatHouseInfoResult>(S2CGetCatHouseInfoResult.Descriptor, GetCatHouseInfoResultHandler);

            RegisterNetMsgType<S2CCatHouseAddCatResult>(S2CCatHouseAddCatResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CCatHouseRemoveCatResult>(S2CCatHouseRemoveCatResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CCatHouseGetGoldResult>(S2CCatHouseGetGoldResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CCatHousesGetGoldResult>(S2CCatHousesGetGoldResult.Descriptor, MsgHandler);
            //猫舍开始升级
            RegisterNetMsgType<S2CCatHouseStartLevelupResult>(S2CCatHouseStartLevelupResult.Descriptor, CatHouseStartLevelupResultHandler);
            RegisterNetMsgType<S2CCatHouseSpeedLevelupResult>(S2CCatHouseSpeedLevelupResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CSellCatHouseResult>(S2CSellCatHouseResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CCatHouseSetDoneResult>(S2CCatHouseSetDoneResult.Descriptor, MsgHandler);
            //猫舍开始产金
            RegisterNetMsgType<S2CCatHouseProduceGoldResult>(S2CCatHouseProduceGoldResult.Descriptor, CatHouseProduceGoldResultHandler);


            RegisterNetMsgType<S2CFriendChatResult>(S2CFriendChatResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CFriendGetUnreadMessageNumResult>(S2CFriendGetUnreadMessageNumResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CFriendPullUnreadMessageResult>(S2CFriendPullUnreadMessageResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CFriendConfirmUnreadMessageResult>(S2CFriendConfirmUnreadMessageResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CRetOnlineFriends>(S2CRetOnlineFriends.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CGetHandbookResult>(S2CGetHandbookResult.Descriptor, MsgHandler);
            //新图鉴物品
            RegisterNetMsgType<S2CNewHandbookItemNotify>(S2CNewHandbookItemNotify.Descriptor, NewHandbookItemNotifyHandler);
            RegisterNetMsgType<S2CGetHeadResult>(S2CGetHeadResult.Descriptor, MsgHandler);
            //新头像
            RegisterNetMsgType<S2CNewHeadNotify>(S2CNewHeadNotify.Descriptor, NewHeadNotifyHandler);
            RegisterNetMsgType<S2CGetSuitHandbookRewardResult>(S2CGetSuitHandbookRewardResult.Descriptor, MsgHandler);
            //RegisterNetMsgType<S2CRetActivityReward>(S2CRetActivityReward.Descriptor, MsgHandler);

            RegisterNetMsgType<S2CPullFosterDataResult>(S2CPullFosterDataResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CPullFosterCatsWithFriendResult>(S2CPullFosterCatsWithFriendResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CFosterCardComposeResult>(S2CFosterCardComposeResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CFosterEquipCardResult>(S2CFosterEquipCardResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CFosterUnequipCardResult>(S2CFosterUnequipCardResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CFosterSetCatResult>(S2CFosterSetCatResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CFosterOutCatResult>(S2CFosterOutCatResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CFosterSetCat2FriendResult>(S2CFosterSetCat2FriendResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CFosterGetEmptySlotFriendsResult>(S2CFosterGetEmptySlotFriendsResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CGetPlayerFosterCatsResult>(S2CGetPlayerFosterCatsResult.Descriptor, MsgHandler);
            //RegisterNetMsgType<S2CPullRankingListResult>(S2CPullRankingListResult.Descriptor, MsgHandler);
            //RegisterNetMsgType<S2CWorldChatSendResult>(S2CWorldChatSendResult.Descriptor, MsgHandler);
            //RegisterNetMsgType<S2CWorldChatMsgPullResult>(S2CWorldChatMsgPullResult.Descriptor, MsgHandler);
            //RegisterNetMsgType<S2CWorldChatForbid>(S2CWorldChatForbid.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CVisitPlayerResult>(S2CVisitPlayerResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2COpenFriendChestResult>(S2COpenFriendChestResult.Descriptor, MsgHandler);
            //系统公告通知
            RegisterNetMsgType<S2CAnouncementNotify>(S2CAnouncementNotify.Descriptor, AnouncementNotifyHandler);
            RegisterNetMsgType<S2CPlayerCatInfoResult>(S2CPlayerCatInfoResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CGetPersonalSpaceResult>(S2CGetPersonalSpaceResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CPersonalSpaceModifySignatureResult>(S2CPersonalSpaceModifySignatureResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CPersonalSpaceZanResult>(S2CPersonalSpaceZanResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CPersonalSpaceGetPictureResult>(S2CPersonalSpaceGetPictureResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2SPersonalSpaceDeletePicResult>(S2SPersonalSpaceDeletePicResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CPersonalSpacePullLeaveMsgResult>(S2CPersonalSpacePullLeaveMsgResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CPersonalSpaceSendLeaveMsgResult>(S2CPersonalSpaceSendLeaveMsgResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CPersonalSpaceDelLeaveMsgResult>(S2CPersonalSpaceDelLeaveMsgResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CPersonalSpaceSendLeaveMsgCommentResult>(S2CPersonalSpaceSendLeaveMsgCommentResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CPersonalSpaceDelLeaveMsgCommentResult>(S2CPersonalSpaceDelLeaveMsgCommentResult.Descriptor, MsgHandler);
            RegisterNetMsgType<S2CPersonalSpacePullLeaveMsgCommentResult>(S2CPersonalSpacePullLeaveMsgCommentResult.Descriptor, MsgHandler);

            #region 地板
            //请求地板数据
            RegisterNetMsgType<S2CSurfaceDataResponse>(S2CSurfaceDataResponse.Descriptor, SurfaceDataHandler);
            //更新地板数据
            RegisterNetMsgType<S2CSurfaceUpdateResponse>(S2CSurfaceUpdateResponse.Descriptor, SurfaceUpdateHandler);
            #endregion

            #region 充值
            RegisterNetMsgType<S2CChargeResponse>(S2CChargeResponse.Descriptor, ExeCharge);
            #endregion

        }

        private void MsgHandler(IMessage obj)
        {

        }

        #region override

        private float _flShowMaskTime = 0.5f;
        private bool _blShowMask = false;
        private S2C_ONE_MSG recOneMsg;

        protected override void Send(int msgId, IMessage msg)
        {
            base.Send(msgId, msg);

            if (msgId != ProtocolHelper.GetMsgID(C2SHeartbeat.Descriptor))
                _flHeartBeatTime = HeartBeatTime;
        }

        protected override void OnDataError()
        {
            base.OnDataError();
            GameNetMgr.Instance.AddNetErrorCount();
        }

        protected override void DoRequestSend()
        {
            var url = string.Format("http://{0}/client_msg", KConfig.ConnectURL);
#if DEBUG_MY
            Debuger.Log("[HTTP_URL] " + url);
#endif

            _curRequest = new GamePostRequest(url, OnReceiveData, OnDataError);

            C2S_MSG_DATA data = new C2S_MSG_DATA();
            data.Token = KUser.AccountToken;
            C2S_ONE_MSG oneMsg;
            while (_postDataPools.Count > 0)
            {
                oneMsg = _postDataPools.Dequeue();
                if (oneMsg.MsgCode == ProtocolHelper.GetMsgID(C2SEnterGameRequest.Descriptor))
                {
                    data.MsgList.Clear();
                    _curRequest.AddMsgID(oneMsg.MsgCode);
                    data.MsgList.Add(oneMsg);
                    _postDataPools.Clear();
                    _lstRequestID.Clear();
                    break;
                }
                _curRequest.AddMsgID(oneMsg.MsgCode);
                data.MsgList.Add(oneMsg);
            }
            _curRequest.InitPostData(data.ToByteArray());
            _curRequest.Send();
        }

        protected override void OnReceiveData(object data)
        {
            S2C_MSG_DATA pbValue = data as S2C_MSG_DATA;

            for (int i = 0; i < pbValue.MsgList.Count; i++)
            {
                recOneMsg = pbValue.MsgList[i];
                if (recOneMsg.ErrorCode < 0)
                {
                    //NetErrorHelper.DoErrorCode(recOneMsg.ErrorCode);
                    GameNetMgr.Instance.ResetNetErrorCount();
                    Debuger.LogError("ErrCode: " + recOneMsg.ErrorCode);
                    KServer.ProcessError(recOneMsg.ErrorCode);
                }
                else
                {
                    ProccessOneMsg(recOneMsg);
                    GameNetMgr.Instance.ResetNetErrorCount();
                }
                _flShowMaskTime = 0.5f;
                Game.UI.IndicatorBox.HideIndicator(); //-------------
            }
            if (_postDataPools.Count == 0)
            {
                if (_blShowMask)
                {
                    _blShowMask = false;
                    //LoadingMgr.Instance.HideRechargeMask();
                    Game.UI.IndicatorBox.HideIndicator();
                }
            }
        }

        public override void Update()
        {
            base.Update();
            if (_blEnterGame)
            {
                _flHeartBeatTime -= UnityEngine.Time.deltaTime;
                if (_flHeartBeatTime <= 0.01f)
                {
                    _flHeartBeatTime = HeartBeatTime;
                    SendHeartBeat();
                }
            }
            if (_curRequest != null && !_curRequest.BlEnd)
            {
                if (_curRequest.mBlNeedCheckMask)
                {
                    if (_flShowMaskTime <= 0.01f)
                    {
                        _blShowMask = true;
                        //LoadingMgr.Instance.ShowRechargeMask();
                        //Game.UI.IndicatorBox.ShowIndicator(); //-------------
                        return;
                    }
                    _flShowMaskTime -= UnityEngine.Time.deltaTime;
                }
            }
        }

        #endregion

        #region HeartBeat
        private static bool _blEnterGame;
        private static float _flHeartBeatTime = HeartBeatTime;
        private const float HeartBeatTime = 3f * 60;

        private void SendHeartBeat()
        {
            C2SHeartbeat req = new C2SHeartbeat();
            Send(ProtocolHelper.GetMsgID(C2SHeartbeat.Descriptor), req);
        }

        private void HeartBeatHandler(S2CHeartbeat obj)
        {
            //Instance.mSystemTime = value.SysTime;
            //Instance.OnSystemValue(value.SysTime);
        }

        #endregion

    }
}