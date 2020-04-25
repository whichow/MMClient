// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KServer" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using Game.DataModel;
using Msg.ClientMessage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Game
{
    using Callback = System.Action<int, string, object>;

    /// <summary>
    /// 服务器代理
    /// </summary>
    public class KServer : KGameModule
    {
        #region Static Field

        public static KServer Instance;

        #endregion

        #region Protocol

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        public void Login(Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            Instance.StartCoroutine(LoginRoutine(callback));
        }

        /// <summary>Logouts the specified callback.注销()</summary>
        public void Logout(Callback callback)
        {
        }

        public void PullInfos(Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SGetInfo), callback);
            var data = new C2SGetInfo
            {
                Base = true,
                Item = true,
                //Cat = true,
                //Building = true,
                //Area = true,
                //Farm = true,
                //WorkShop = true,
                //CatHouse = true,
                //DepotBuilding = true,
                Stage = true,
                Formula = true,
                Guide = true,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        public void PullInfos1(Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SGetInfo), callback);
            var data = new C2SGetInfo
            {
                //Base = true,
                //Item = true,
                Cat = true,
                //Building = true,
                //Area = true,
                //Farm = true,
                //WorkShop = true,
                //CatHouse = true,
                //DepotBuilding = true,
                //Stage = true,
                //Formula = true,
                //Guide = true,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        public void PullInfos2(Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SGetInfo), callback);
            var data = new C2SGetInfo
            {
                //Base = true,
                //Item = true,
                //Cat = true,
                Building = true,
                Area = true,
                Farm = true,
                WorkShop = true,
                CatHouse = true,
                DepotBuilding = true,
                //Guide = true,
                //Stage = true,
                //Formula = true,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 拉取基础信息
        /// </summary>
        /// <param name="callback"></param>
        public void PullBaseInfo(Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SPlayerInfoRequest), callback);
            packet.AddData(new C2SPlayerInfoRequest());
            PacketManager.Instance.SendPostPacket(packet);
        }

        public void PullItemInfo(Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SGetItemInfos), callback);
            packet.AddData(new C2SGetItemInfos());
            PacketManager.Instance.SendPostPacket(packet);
        }

        public void PullLevelInfos(Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SGetStageInfos), callback);
            packet.AddData(new C2SGetStageInfos());
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 抽奖
        /// </summary>
        /// <param name="drawType"></param>
        /// <param name="drawCount"></param>
        /// <param name="callback"></param>
        public void Draw(int drawType, int drawCount, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SDraw), callback);
            var drawData = new C2SDraw
            {
                DrawType = drawType,
                DrawCount = drawCount
            };
            packet.AddData(drawData);
            PacketManager.Instance.SendPostPacket(packet);
            //GameNetMgr.Instance.mGameServer.C2SDrawRequest(drawType, drawCount, callback);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="count"></param>
        /// <param name="callback"></param>
        public void BuyItem(int id, int count, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SBuyShopItem), callback);
            var data = new C2SBuyShopItem
            {
                ItemId = id,
                ItemNum = count,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="count"></param>
        /// <param name="callback"></param>
        public void SellItem(int id, int count, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SSellItem), callback);
            var data = new C2SSellItem
            {
                ItemId = id,
                ItemNum = count,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 客户端请求使用物品
        /// </summary>
        /// <param name="id"></param>
        /// <param name="count"></param>
        /// <param name="callback"></param>
        public void UseItem(int id, int count, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SUseItem), callback);
            var data = new C2SUseItem
            {
                ItemCfgId = id,
                ItemNum = count,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="callback"></param>
        public void ShopGetItems(int shopId, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SShopItems), callback);
            var data = new C2SShopItems
            {
                ShopId = shopId,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="args"></param>
        public void GMCommand(string cmd, string[] args)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2S_TEST_COMMAND), null);
            var data = new C2S_TEST_COMMAND
            {
                Cmd = cmd
            };
            data.Args.AddRange(args);
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 获取猫咪信息
        /// </summary>
        /// <param name="callback"></param>
        public void CatGetInfos(Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SGetCatInfos), callback);
            packet.AddData(new C2SGetCatInfos());
            PacketManager.Instance.SendPostPacket(packet);
        }
        /// <summary>
        /// 喂猫
        /// </summary>
        /// <param name="catId"></param>
        /// <param name="feedCount"></param>
        /// <param name="callback"></param>
        public void CatFeed(int catId, int feedCount, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SCatFeed), callback);
            var data = new C2SCatFeed
            {
                CatId = catId,
                CatFood = feedCount,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 合成猫咪
        /// </summary>
        /// <param name="catShopId"></param>
        /// <param name="callback"></param>
        public void CatCompose(int catShopId, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SComposeCat), callback);
            var data = new C2SComposeCat
            {
                CatConfigId = catShopId
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        public void CatUpstar(int catId, int[] costCats, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SCatUpgradeStar), callback);
            var data = new C2SCatUpgradeStar
            {
                CatId = catId,
            };
            data.CostCatIds.AddRange(costCats);
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        public void CatSkillUpgrade(int catId, int[] costCatId, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SCatSkillLevelUp), callback);
            var data = new C2SCatSkillLevelUp
            {
                CatId = catId,
            };
            data.CostCatIds.AddRange(costCatId);
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        public void CatRename(int catId, string name, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SCatRenameNick), callback);
            var data = new C2SCatRenameNick
            {
                CatId = catId,
                NewNick = name,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        public void CatLock(int catId, bool locked, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SCatLock), callback);
            var data = new C2SCatLock
            {
                CatId = catId,
                IsLock = locked,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 分解猫
        /// </summary>
        /// <param name="catId"></param>
        /// <param name="callback"></param>
        public void CatDecompose(int[] catIds, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SCatDecompose), callback);
            var data = new C2SCatDecompose
            {
            };
            data.CatId.AddRange(catIds);
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 开始关卡
        /// </summary>
        /// <param name="data"></param>
        /// <param name="callback"></param>
        public void StartLevel(C2SStageBegin data, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SStageBegin), callback);
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }
        /// <summary>
        /// 结束关卡
        /// </summary>
        /// <param name="data"></param>
        /// <param name="callback"></param>
        public void FinishLevel(C2SStagePass data, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SStagePass), callback);
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 获取城建实例
        /// </summary>
        /// <param name="callback"></param>
        public void BuildingGets(Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SGetBuildingInfos), callback);
            packet.AddData(new C2SGetBuildingInfos());
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 客户端向服务器请求设置建筑
        /// </summary>
        /// <param name="callback"></param>
        public void BuildingSet(C2SSetBuilding data, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SSetBuilding), callback);
            if (data != null)
            {
                packet.AddData(data);
            }
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 客户端向服务器移动建筑
        /// </summary>
        /// <param name="callback"></param>
        public void BuildingMove(C2SMoveBuilding data, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SMoveBuilding), callback);
            if (data != null)
            {
                packet.AddData(data);
            }
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 客户端向服务器请求转向建筑
        /// </summary>
        /// <param name="callback"></param>
        public void BuildingChangeDirection(C2SChgBuildingDir data, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SChgBuildingDir), callback);
            if (data != null)
            {
                packet.AddData(data);
            }
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 客户端向服务器请求回收建筑
        /// </summary>
        /// <param name="callback"></param>
        public void BuildingRecycle(int id, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SGetBackBuilding), callback);
            var data = new C2SGetBackBuilding
            {
                BuildingId = id
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 客户端向服务器请求出售建筑
        /// </summary>
        /// <param name="callback"></param>
        public void BuildingSell(int id, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SSellBuilding), callback);
            var data = new C2SSellBuilding
            {
                BuildingId = id
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 客户端向服务器请求清除障碍
        /// </summary>
        /// <param name="callback"></param>
        public void BuildingRemoveBlock(int id, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SRemoveBlock), callback);
            var data = new C2SRemoveBlock
            {
                BuildingId = id
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 客户端向服务器请求开启地图宝箱
        /// </summary>
        /// <param name="playerId">0为玩家自己，其它数值 是其它玩家id</param>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public void BuildingOpenBox(int playerId,int id, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SOpenMapChest), callback);
            var data = new C2SOpenMapChest
            {
                FriendId = playerId,
                BuildingId = id
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 客户端向服务器请求解锁地图区域
        /// </summary>
        /// <param name="id">区域Id</param>
        /// <param name="useStone">是否使用钻石快速解锁</param>
        /// <param name="callback"></param>
        public void BuildingUnlockArea(int id, int useStone, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SUnlockArea), callback);
            var data = new C2SUnlockArea
            {
                AreaId = id,
                IfQuick = useStone,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 获取配方信息
        /// </summary>
        /// <param name="callback"></param>
        public void FormulaGetInfos(Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SGetFormulas), callback);
            var data = new C2SGetFormulas();
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 配方兑换
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public void FormulaExchange(int id, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SExchangeBuildingFormula), callback);
            var data = new C2SExchangeBuildingFormula
            {
                FormulaId = id
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 拉取正在打造的配方建筑
        /// </summary>
        /// <param name="callback"></param>
        public void WorkshopGets(Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SGetMakingFormulaBuildings), callback);
            var data = new C2SGetMakingFormulaBuildings
            {

            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }
        /// <summary>
        /// 购买槽
        /// </summary>
        /// <param name="slotIndex"></param>
        /// <param name="callback"></param>
        public void WorkshopBuySlot(int slotIndex, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SBuyMakeBuildingSlot), callback);
            var data = new C2SBuyMakeBuildingSlot
            {

            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }
        /// <summary>
        /// 购买每日体力
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public void BuyDayPower( Callback callback)
        {
            //UI.IndicatorBox.ShowIndicator();
            //var packet = PacketManager.Instance.CreatePacket(typeof(C2SDayBuyTiLi), callback);
            //PacketManager.Instance.SendPostPacket(packet);
        }
        public void WorkshopMakeFormula(int slotIndex, int formulaId, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SMakeFormulaBuilding), callback);
            var data = new C2SMakeFormulaBuilding
            {
                FormulaId = formulaId,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        public void WorkshopSpeedUp(int slotIndex, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SSpeedupMakeBuilding), callback);
            var data = new C2SSpeedupMakeBuilding
            {
                SlotId = slotIndex,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        public void WorkshopMakeCancel(int slotIndex, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SCancelMakingFormulaBuilding), callback);
            var data = new C2SCancelMakingFormulaBuilding
            {
                SlotId = slotIndex,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 收取
        /// </summary>
        /// <param name="slotIndex"></param>
        /// <param name="callback"></param>
        public void WorkshopCollect(int slotIndex, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SGetCompletedFormulaBuilding), callback);
            var data = new C2SGetCompletedFormulaBuilding
            {
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        public void ExploreGetTasks(Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SGetAllExpedition), callback);
            var data = new C2SGetAllExpedition
            {
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        public void ExploreStart(int taskId, int[] catIds, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SStartExpedition), callback);
            var data = new C2SStartExpedition
            {
                Id = taskId,
            };
            data.CatIds.AddRange(catIds);
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        public void ExploreBreak(int taskId, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SStopExpedition), callback);
            var data = new C2SStopExpedition
            {
                Id = taskId,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        public void ExploreDelete(int taskId, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SChgExpedition), callback);
            var data = new C2SChgExpedition
            {
                Id = taskId,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        public void ExploreComplete(int taskId, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SGetExpeditionReward), callback);
            var data = new C2SGetExpeditionReward
            {
                Id = taskId,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }
        /// <summary>
        /// 花费钻石拯救任务
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="callback"></param>
        public void ExploreSave(int taskId, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SChgExpeditionResult), callback);
            var data = new C2SChgExpeditionResult
            {
                Id = taskId,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        public void MissionGetDaily(Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2STaskDataRequest), callback);
            var data = new C2STaskDataRequest
            {
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        public void MissionGetArchievement(Callback callback)
        {
            //UI.IndicatorBox.ShowIndicator();
            //var packet = PacketManager.Instance.CreatePacket(typeof(C2SGetAchieve), callback);
            //var data = new C2SGetAchieve
            //{
            //};
            //packet.AddData(data);
            //PacketManager.Instance.SendPostPacket(packet);
        }

        public void MissionRewardDaily(int id, Callback callback)
        {
            //UI.IndicatorBox.ShowIndicator();
            //var packet = PacketManager.Instance.CreatePacket(typeof(C2SGetTaskReward), callback);
            //var data = new C2SGetTaskReward
            //{
            //    TaskId = id,
            //};
            //packet.AddData(data);
            //PacketManager.Instance.SendPostPacket(packet);
        }

        public void MissionRewardArchievement(int id, Callback callback)
        {
            //UI.IndicatorBox.ShowIndicator();
            //var packet = PacketManager.Instance.CreatePacket(typeof(C2SGetAchieveReward), callback);
            //var data = new C2SGetAchieveReward
            //{
            //    AchieveReward = id,
            //};
            //packet.AddData(data);
            //PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 拉取农作物
        /// </summary>
        /// <param name="callback"></param>
        public void FarmGets(Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SGetCrops), callback);
            var data = new C2SGetCrops
            {
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 种植作物
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="cropId"></param>
        /// <param name="callback"></param>
        public void FarmSowing(int buildingId, int cropId, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SPlantCrop), callback);
            var data = new C2SPlantCrop
            {
                CropId = cropId,
                DestBuildingId = buildingId,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 加速农田
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="callback"></param>
        public void FarmSpeedUp(int buildingId, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SCropSpeedup), callback);
            var data = new C2SCropSpeedup
            {
                FarmBuildingId = buildingId,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 收割
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="callback"></param>
        public void FarmHarvest(int buildingId, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SHarvestCrop), callback);
            var data = new C2SHarvestCrop
            {
                FarmBuildingId = buildingId,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 拉取猫舍
        /// </summary>
        /// <param name="callback"></param>
        public void CatteryGets(Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SGetCatHousesInfo), callback);
            var data = new C2SGetCatHousesInfo
            {
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 拉取单个猫舍
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public void CatteryGet(int id, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SGetCatHouseInfo), callback);
            var data = new C2SGetCatHouseInfo
            {
                CatHouseId = id,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
            //GameNetMgr.Instance.mGameServer.GetCatHouseInfoRequest(id);
        }

        /// <summary>
        /// 猫舍收金
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public void CatteryHarvest(int id, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SCatHouseGetGold), callback);
            var data = new C2SCatHouseGetGold
            {
                CatHouseId = id,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 猫舍开始升级
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public void CatteryUpgrade(int id, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SCatHouseStartLevelup), callback);
            var data = new C2SCatHouseStartLevelup
            {
                CatHouseId = id,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 猫舍完成(升级 建筑)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public void CatteryComplete(int id, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SCatHouseSetDone), callback);
            var data = new C2SCatHouseSetDone
            {
                CatHouseId = id,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 猫舍加速升级
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public void CatterySpeedUp(int id, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SCatHouseSpeedLevelup), callback);
            var data = new C2SCatHouseSpeedLevelup
            {
                CatHouseId = id,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 出售猫舍
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public void CatterySell(int id, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SSellCatHouse), callback);
            var data = new C2SSellCatHouse
            {
                CatHouseId = id,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 猫舍加猫
        /// </summary>
        /// <param name="id">建筑物ID</param>
        /// <param name="catId">猫ID</param>
        /// <param name="callback"></param>
        public void CatteryAddCat(int id, int catId, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SCatHouseAddCat), callback);
            var data = new C2SCatHouseAddCat
            {
                CatHouseId = id,
                CatId = catId,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 猫舍减猫
        /// </summary>
        /// <param name="id"></param>
        /// <param name="catId"></param>
        /// <param name="callback"></param>
        public void CatteryRemoveCat(int id, int catId, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SCatHouseRemoveCat), callback);
            var data = new C2SCatHouseRemoveCat
            {
                CatHouseId = id,
                CatId = catId,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 客户端向服务器拉取邮件列表
        /// </summary>
        /// <param name="callback"></param>
        public void MailGets(Callback callback)
        {
            //UI.IndicatorBox.ShowIndicator();
            //var packet = PacketManager.Instance.CreatePacket(typeof(C2SGetMailList), callback);
            //var data = new C2SGetMailList
            //{
            //};
            //packet.AddData(data);
            //PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 客户端向服务器发送领取附件的请求
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public void MailGetAttach(int id, Callback callback)
        {
            //UI.IndicatorBox.ShowIndicator();
            //var packet = PacketManager.Instance.CreatePacket(typeof(C2SGetMailAttach), callback);
            //var data = new C2SGetMailAttach
            //{
            //    MailId = id,
            //};
            //packet.AddData(data);
            //PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 客户端向服务器发送删除邮件请求
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public void MailDelete(int id, Callback callback)
        {
            //UI.IndicatorBox.ShowIndicator();
            //var packet = PacketManager.Instance.CreatePacket(typeof(C2SMailRemove), callback);
            //var data = new C2SMailRemove
            //{
            //    MailId = id,
            //};
            //packet.AddData(data);
            //PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 客户端向服务器设置邮件已读标志
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public void MailRead(int id, Callback callback)
        {
            //UI.IndicatorBox.ShowIndicator();
            //var packet = PacketManager.Instance.CreatePacket(typeof(C2SSetMailRead), callback);
            //var data = new C2SSetMailRead
            //{
            //    MailId = id,
            //};
            //packet.AddData(data);
            //PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public void MailAgreeHelp(int id, Callback callback)
        {
            //UI.IndicatorBox.ShowIndicator();
            //var packet = PacketManager.Instance.CreatePacket(typeof(C2SAgreeMailHelpReq), callback);
            //var data = new C2SAgreeMailHelpReq
            //{
            //    MailId = id,
            //};
            //packet.AddData(data);
            //PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 客户端向服务器拉取图鉴列表
        /// </summary>
        /// <param name="callback"></param>
        public void HandBookGets(Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SGetHandbook), callback);
            var data = new C2SGetHandbook
            {
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 客户端向服务器领取区域奖励
        /// </summary>
        /// <param name="callback"></param>
        public void GetLandeAward(int id, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SGetSuitHandbookReward), callback);
            var data = new C2SGetSuitHandbookReward
            {
                SuitId = id,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
            Debuger.Log("-><color=#9400D3>" + "[日志] [KServer] [GetLandeAward] 向服务器发送领取区域奖励，ID：" + id + "</color>");
        }
        public void ChapterUnlock(int type, int chapterId, int[] firendId, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SChapterUnlock), callback);
            var data = new C2SChapterUnlock
            {
                ChapterId = chapterId,
                UnLockType = type,
            };
            if (firendId != null)
            {
                data.FriendIds.AddRange(firendId);
            }
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 活动获取
        /// </summary>
        public void ActivityGets(Callback callback)
        {
            //UI.IndicatorBox.ShowIndicator();
            //var packet = PacketManager.Instance.CreatePacket(typeof(C2SGetAllActivityInfos), callback);
            //var data = new C2SGetAllActivityInfos
            //{
            //};
            //packet.AddData(data);
            //PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 活动领奖
        /// </summary>
        /// <param name="id"></param>
        /// <param name="values"></param>
        /// <param name="callback"></param>
        public void ActivityGetRewards(int id, int[] values, Callback callback)
        {
            //UI.IndicatorBox.ShowIndicator();
            //var packet = PacketManager.Instance.CreatePacket(typeof(C2SGetActivityReward), callback);
            //var data = new C2SGetActivityReward
            //{
            //    ActivityCfgId = id,
            //};
            //if (values != null && values.Length > 0)
            //{
            //    data.ExtraParams.AddRange(values);
            //}
            //packet.AddData(data);
            //PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 客户端向服务器拉取邮件列表
        /// </summary>
        /// <param name="callback"></param>
        public void GetFriends(Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SGetFriendList), callback);
            var data = new C2SGetFriendList
            {
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }
        /// <summary>
        /// 客户端向服务器搜索好友
        /// </summary>
        /// <param name="callback"></param>
        public void SearchFriends(string friendName, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SFriendSearch), callback);
            var data = new C2SFriendSearch
            {
                Key = friendName,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }
        /// <summary>
        /// 客户端向服务器搜索好友
        /// </summary>
        /// <param name="callback"></param>
        public void AddFriends(int friendid, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SAddFriendByPId), callback);
            var data = new C2SAddFriendByPId
            {
                PlayerId = friendid,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }
        /// <summary>
        /// 客户端同意某个好友的请求
        /// </summary>
        /// <param name="callback"></param>
        public void C2SAgreeOneFriend(int friendid, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SAgreeFriend), callback);
            var data = new C2SAgreeFriend
            {
                PlayerId = friendid,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }
        /// <summary>
        /// 客户端拒绝某个好友的请求
        /// </summary>
        /// <param name="callback"></param>
        public void C2SRefuseOneFriend(int friendid, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SRefuseFriend), callback);
            var data = new C2SRefuseFriend
            {
                PlayerId = friendid,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }
        /// <summary>
        /// 客户端通知移除某个好友
        /// </summary>
        /// <param name="friendid"></param>
        /// <param name="callback"></param>
        public void C2SRemoveOneFriend(int friendid, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SRemoveFriend), callback);
            var data = new C2SRemoveFriend
            {
                PlayerId = friendid,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);

        }
        /// <summary>
        /// 拉取未读消息
        /// </summary>
        /// <param name="friendid"></param>
        /// <param name="callback"></param>
        public void C2SOneFriendPullUnreadMessage(int friendid, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SFriendPullUnreadMessage), callback);
            var data = new C2SFriendPullUnreadMessage
            {
                FriendId = friendid,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }
        /// <summary>
        /// 客户端向服务器同步已读消息的数量
        /// </summary>
        /// <param name="friendid"></param>
        /// <param name="callback"></param>
        public void C2SOneFriendConfirmUnreadMessage(int friendid, int msgNum, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SFriendConfirmUnreadMessage), callback);
            var data = new C2SFriendConfirmUnreadMessage
            {
                FriendId = friendid,
                MessageNum = msgNum,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 向服务器拉取一次所有好友的未读消息数量
        /// </summary>
        /// <param name="friendids"></param>
        /// <param name="callback"></param>
        public void C2SFriendOneGetUnreadMessageNum(List<int> friendids, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SFriendGetUnreadMessageNum), callback);
            var data = new C2SFriendGetUnreadMessageNum
            {
            };
            data.FriendIds.AddRange(friendids);
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }
        /// <summary>
        /// 向某个好友赠送友情点数
        /// </summary>
        /// <param name="friendids"></param>
        /// <param name="callback"></param>
        public void C2SGiveOneFriendPoints(List<int> friendids, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SGiveFriendPoints), callback);
            var data = new C2SGiveFriendPoints
            {
            };
            data.FriendId.AddRange(friendids);
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }
        /// <summary>
        /// 领取某些好友赠送的友情点数
        /// </summary>
        /// <param name="friendids"></param>
        /// <param name="callback"></param>
        public void C2SGetOneFriendPoints(List<int> friendids, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SGetFriendPoints), callback);
            var data = new C2SGetFriendPoints
            {
            };
            data.FriendId.AddRange(friendids);
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }
        /// <summary>
        /// 点赞某个好友
        /// </summary>
        /// <param name="friendid"></param>
        /// <param name="callback"></param>
        public void C2SZanOnePlayer(int friendid, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SZanPlayer), callback);
            var data = new C2SZanPlayer
            {
                PlayerId = friendid,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }
        /// <summary>
        /// 向好友发送消息
        /// </summary>
        /// <param name="friendid"></param>
        /// <param name="dialogcontent"></param>
        /// <param name="callback"></param>
        public void C2SOneFriendChat(int friendid, byte[] dialogcontent, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SFriendChat), callback);
            var data = new C2SFriendChat
            {
                PlayerId = friendid,
                Content = Google.Protobuf.ByteString.CopyFrom(dialogcontent),
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 向服务器索取一次拥有空寄养位置的好友列表
        /// </summary>
        public void C2SOneFosterGetEmptySlotFriends(Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SFosterGetEmptySlotFriends), callback);
            var data = new C2SFosterGetEmptySlotFriends
            {
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }
        //////////寄养

        /// <summary>
        /// 寄养
        /// </summary>
        /// <param name="settle"></param>
        /// <param name="callback"></param>
        public void FosterGets(bool settle, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SPullFosterData), callback);
            var data = new C2SPullFosterData
            {
                IsSettle = settle,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 拉取寄养在好友的猫和好友寄养的猫
        /// </summary>
        /// <param name="settle"></param>
        /// <param name="callback"></param>
        public void FosterGets2(Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SPullFosterCatsWithFriend), callback);
            var data = new C2SPullFosterCatsWithFriend
            {
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 获取玩家的寄养所
        /// </summary>
        /// <param name="callback"></param>
        public void FosterGetsFriend(int playerId, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SGetPlayerFosterCats), callback);
            var data = new C2SGetPlayerFosterCats
            {
                PlayerId = playerId
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 寄养所增加猫
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="catId"></param>
        /// <param name="callback"></param>
        public void FosterAddCat(int buildingId, int catId, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SFosterSetCat), callback);
            var data = new C2SFosterSetCat
            {
                BuildingId = buildingId,
                CatId = catId,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 寄养所移除猫
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="catId"></param>
        /// <param name="callback"></param>
        public void FosterRemoveCat(int buildingId, int catId, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SFosterOutCat), callback);
            var data = new C2SFosterOutCat
            {
                BuildingId = buildingId,
                CatId = catId,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 寄养所增加猫(好友)
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="catId"></param>
        /// <param name="callback"></param>
        public void FosterAddCatInFriend(int friendId, int catId, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SFosterSetCat2Friend), callback);
            var data = new C2SFosterSetCat2Friend
            {
                FriendId = friendId,
                CatId = catId,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 装备寄养卡
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="cardId">bingid</param>
        /// <param name="callback"></param>
        public void FosterEquipCard(int buildingId, int bindCardId, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SFosterEquipCard), callback);
            var data = new C2SFosterEquipCard
            {
                BuildingId = buildingId,
                CardId = bindCardId,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 卸载寄养卡
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="cardId"></param>
        /// <param name="callback"></param>
        public void FosterUnequipCard(int buildingId, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SFosterUnequipCard), callback);
            var data = new C2SFosterUnequipCard
            {
                BuildingId = buildingId,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 合成寄养卡
        /// </summary>
        /// <param name="bindCardIds"></param>
        /// <param name="callback"></param>
        public void FosterComposeCard(int[] bindCardIds, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SFosterCardCompose), callback);
            var data = new C2SFosterCardCompose
            {
            };
            if (bindCardIds != null)
            {
                data.ItemIds.AddRange(bindCardIds);
            }
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 拜访玩家
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="callback"></param>
        public void VisitPlayer(int playerId, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SVisitPlayer), callback);
            var data = new C2SVisitPlayer
            {
                PlayerId = playerId,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 修改昵称
        /// </summary>
        /// <param name="name"></param>
        /// <param name="callback"></param>
        public void ChangeName(string name, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SChgName), callback);
            var data = new C2SChgName
            {
                Name = name,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 更改头像
        /// </summary>
        /// <param name="head"></param>
        /// <param name="callback"></param>
        public void ChangeHead(string head, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SChangeHead), callback);
            var data = new C2SChangeHead
            {
                NewHead = Convert.ToInt32(head)
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }

        /// <summary>
        /// 向服务器拉取一次某排行榜数据
        /// </summary>
        /// <param name="RankType"></param>
        /// <param name="StartRank"></param>
        /// <param name="RankNum"></param>
        /// <param name="Param"></param>
        /// <param name="callback"></param>
        public void C2SPullOneRankingList(int rankType, int startRank, int rankNum, int param, Callback callback)
        {
            //UI.IndicatorBox.ShowIndicator();
            //var packet = PacketManager.Instance.CreatePacket(typeof(C2SPullRankingList), callback);
            //var data = new C2SPullRankingList
            //{
            //    RankType = rankType,
            //    StartRank = startRank,
            //    RankNum = rankNum,
            //    Param = param
            //};
            //packet.AddData(data);
            //PacketManager.Instance.SendPostPacket(packet);
        }
        /// <summary>
        /// 向服务器索取被点中的猫咪的信息供打开猫咪信息面板时使用
        /// </summary>
        /// <param name="playerid"></param>
        /// <param name="catid"></param>
        /// <param name="callback"></param>
        public void C2SOnePlayerCatInfo(int playerid, int catid, Callback callback)
        {
            UI.IndicatorBox.ShowIndicator();
            var packet = PacketManager.Instance.CreatePacket(typeof(C2SPlayerCatInfo), callback);
            var data = new C2SPlayerCatInfo
            {
                PlayerId = playerid,
                CatId = catid,
            };
            packet.AddData(data);
            PacketManager.Instance.SendPostPacket(packet);
        }
        /// <summary>
        /// 向服务器拉取一次聊天内容
        /// </summary>
        /// <param name="callback"></param>
        public void C2SOneWorldChatMsgPull(Callback callback)
        {
            //UI.IndicatorBox.ShowIndicator();
            //var packet = PacketManager.Instance.CreatePacket(typeof(C2SWorldChatMsgPull), callback);
            //var data = new C2SWorldChatMsgPull
            //{
            //};
            //packet.AddData(data);
            //PacketManager.Instance.SendPostPacket(packet);
            //Debug.Log("协议：世界聊天向服务器拉取一次聊天内容");
        }
        /// <summary>
        /// 在世界频道发送一次消息
        /// </summary>
        /// <param name="chatdata"></param>
        /// <param name="callback"></param>
        public void C2SOneWorldChatSend(byte[] chatdata, Callback callback)
        {
            //UI.IndicatorBox.ShowIndicator();
            //var packet = PacketManager.Instance.CreatePacket(typeof(C2SWorldChatSend), callback);
            //var data = new C2SWorldChatSend
            //{
            //    Content = Google.Protobuf.ByteString.CopyFrom(chatdata),
            //};
            //packet.AddData(data);
            //PacketManager.Instance.SendPostPacket(packet);
            //Debug.Log("协议：在世界频道发送一次消息,内容byte长度：" + chatdata.Length);
        }

        //public void VerifyOrder(C2SPayOrder orderData, Callback callback)
        //{
        //    UI.IndicatorBox.ShowIndicator();
        //    var packet = PacketManager.Instance.CreatePacket(typeof(C2SPayOrder), callback);
        //    packet.AddData(orderData);
        //    PacketManager.Instance.SendPostPacket(packet);
        //}

        #endregion

        #region Routine

        private IEnumerator LoginRoutine(Callback callback)
        {
            //{ "Code":0,"Account":"76ffb61273c0c2b92e3ea1b8fbac4b9f2d0abaee","Token":"1502348489","HallIP":"192.168.10.113:30100"}

            var url = string.Format("http://{0}/login?account={1}&token={2}", KConfig.ServerURL, KUser.OpenID, KUser.OpenToken);

            var request = UnityWebRequest.Get(url);
            request.timeout = 8;
            yield return request.Send();
            if (!request.isNetworkError)
            {
                var bytes = request.downloadHandler.data;
                var table = bytes.ToJsonTable();
                if (table != null)
                {
                    KUser.AccountID = table.GetString("Account");
                    KUser.AccountToken = table.GetString("Token");
                    KConfig.ConnectURL = table.GetString("HallIP") + "/client_msg";

                    Debuger.Log("LoginSuccess:" + KUser.AccountID + "," + KUser.AccountToken + "," + KConfig.ConnectURL);

                    var packet = PacketManager.Instance.CreatePacket(typeof(C2SEnterGameRequest), callback);
                    var data = new C2SEnterGameRequest
                    {
                        Acc = KUser.AccountID
                    };
                    packet.AddData(data);
                    PacketManager.Instance.SendPostPacket(packet);
                }
                else
                {
                    if (callback != null)
                    {
                        callback(1, "", null);
                    }
                }
            }
            else
            {
                if (callback != null)
                {
                    callback(1, "", null);
                }
            }
        }

        #endregion

        #region Process Method

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        public static void ProcessData(ArrayList list)
        {
            UI.IndicatorBox.HideIndicator();
            if (list == null || list.Count == 0)
            {
                return;
            }

            for (int i = 0; i < list.Count; i++)
            {
                var protoData = list[i];
                if (protoData != null)
                {
                    Action<object> processor;
                    if (_Processors.TryGetValue(protoData.GetType(), out processor))
                    {
                        processor(protoData);
                        continue;
                    }
                }

                //if (LevelDataModel.Instance != null)
                //{
                //    LevelDataModel.Instance.Process(protoData);
                //}

                //养成模块
                if (KCatManager.Instance)
                {
                    KCatManager.Instance.ProcessCommon(protoData);
                }
                //建筑模块
                if (Build.BuildingManager.Instance)
                {
                    Build.BuildingManager.Instance.ProcessCommon(protoData);
                }

                ////成就模块
                //if (KMissionManager.Instance)
                //{
                //    KMissionManager.Instance.Process(protoData);
                //}
                ////邮件
                //if (KMailManager.Instance)
                //{
                //    KMailManager.Instance.Process(protoData);
                //}
                //图鉴
                //if (KHandBookManager.Instance)
                //{
                //    KHandBookManager.Instance.Process(protoData);
                //}
                //猫舍
                if (KCattery.Instance)
                {
                    KCattery.Instance.Process(protoData);
                }
                //农田
                if (KFarm.Instance)
                {
                    KFarm.Instance.Process(protoData);
                }
                //农田
                if (KWorkshop.Instance)
                {
                    KWorkshop.Instance.Process(protoData);
                }
                ////帮助别人次数
                //if (KMailManager.Instance)
                //{
                //    KMailManager.Instance.UpdateLeftHelpData(protoData);
                //}

                //if (KActivityManager.Instance)
                //{
                //    KActivityManager.Instance.ProcessData(protoData);
                //}
                //if (KChatManager.Instance)
                //{
                //    KChatManager.Instance.Process(protoData);
                //}
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        public static void ProcessError(int error)
        {
            UI.IndicatorBox.HideIndicator();
            if (XTable.ErrorXTable.ContainsID(error))
            {
                ErrorXDM errorXdm = XTable.ErrorXTable.GetByID(error);
                if (error == -17 || error == -20)
                {
                    UI.MessageBox.ShowMessage("连接失败", "服务器连接已断开，请重试", () =>
                    {
                        GameApp.Instance.GameServer.ReqReconnect();
                    });
                }
                else if (errorXdm.Type == 1)
                {
                    UI.MessageBox.ShowMessage("错误码："+ errorXdm.ID, errorXdm.ToString());
                }
                else if (errorXdm.Type == 2)
                {
                    UI.MessageBox.ShowMessage("网络错误", "网络连接错误,请重试.", () =>
                     {
                         PacketManager.Instance.ResendPostPacket();
                     });
                }
                else if (errorXdm.Type == 3)
                {
                    //error表中Type为3时定为白名单，除了输出警告日志以外不做任何响应
                    Debuger.LogWarning("[警告] [KServer.cs] [ProcessError] 服务端发送来一个列入白名单的错误码,该错误码获取自配置表'error.txt',Type：3,错误码ID：" + "-><color=#FF0000>" + errorXdm.Message + "</color>");
                }
            }
            else
            {
                UI.MessageBox.ShowMessage("错误码", "未知错误 " + error);
            }
        }

        private static Dictionary<Type, Action<object>> _Processors = new Dictionary<Type, Action<object>>();
        public static void InitProcess()
        {
            //_Processors.Add(typeof(S2CPlayerInfoResponse), OnBaseInfoProcess);

            //_Processors.Add(typeof(S2CGetItemInfos), OnItemInfoProcess);
            //_Processors.Add(typeof(S2CItemsInfoUpdate), OnItemInfoProcess);

            //_Processors.Add(typeof(S2CGetDepotBuildingInfos), OnItemInfoProcess);
            //_Processors.Add(typeof(S2CDepotBuildingInfoUpdate), OnItemInfoProcess);

            _Processors.Add(typeof(S2CGetFormulasResult), OnFormulaInfoProcess);
            _Processors.Add(typeof(S2CExchangeBuildingFormulaResult), OnFormulaInfoProcess);

            _Processors.Add(typeof(S2CGetAreasInfos), OnAreaInfoProcess);

            //_Processors.Add(typeof(S2CChgName), OnChangeNameProcess);
            //_Processors.Add(typeof(S2CChangeHead), OnChangeHeadProcess);

        }

        //private static void OnBaseInfoProcess(object data)
        //{
        //    var protoData = (S2CPlayerInfoResponse)data;
        //    var player = KUser.SelfPlayer;
        //    player.coin = protoData.Gold;
        //    player.stone = protoData.Diamond;
        //    player.food = protoData.CatFood;
        //    player.charm = protoData.CharmVal;
        //    player.grade = protoData.Level;
        //    player.exp = protoData.Exp;
        //    player.curStar = protoData.Star;
        //    player.maxStar = protoData.HistoricalMaxStar;
        //    player.nickName = protoData.Name;
        //    player.headURL = player.GetHeadInfo(obj.Head).icon;
        //    player.maxLevel = protoData.CurMaxStage;
        //    player.maxUnlockLevel = protoData.CurUnlockMaxStage;
        //    player.power = protoData.Spirit;
        //    player.soulStone = protoData.SoulStone;
        //    player.friendPoint = protoData.FriendPoints;
        //    player.charmMadal = protoData.CharmMetal;
        //    player.changeNameCount = protoData.ChangeNameNum;
        //    //player.buyPowerCount = protoData.DayBuyTiLiCount;
        //}

        ///// <summary>
        ///// 物品获取
        ///// </summary>
        ///// <param name="data"></param>
        //private static void OnItemInfoProcess(object data)
        //{
        //    if (data is S2CGetItemInfos)
        //    {
        //        var protoData = (S2CGetItemInfos)data;
        //        foreach (var itemInfo in protoData.Items)
        //        {
        //            KDatabase.SetInt(itemInfo.ItemCfgId, "count", itemInfo.ItemNum);
        //        }
        //        ////假装发过
        //        //KTutorialManager.Instance.ProcessData(null);
        //    }
        //    else if (data is S2CItemsInfoUpdate)
        //    {
        //        var protoData = (S2CItemsInfoUpdate)data;
        //        foreach (var itemInfo in protoData.Items)
        //        {
        //            KDatabase.SetInt(itemInfo.ItemCfgId, "count", itemInfo.ItemNum);
        //        }
        //    }
        //    else if (data is S2CGetDepotBuildingInfos)
        //    {//仓库建筑获取
        //        var protoData = (S2CGetDepotBuildingInfos)data;
        //        foreach (var itemInfo in protoData.DepotBuilds)
        //        {
        //            KDatabase.SetInt(itemInfo.CfgId, "count", itemInfo.Num);
        //        }
        //    }
        //    //仓库建筑更新
        //    else if (data is S2CDepotBuildingInfoUpdate)
        //    {
        //        var protoData = (S2CDepotBuildingInfoUpdate)data;
        //        foreach (var itemInfo in protoData.Buildings)
        //        {
        //            KDatabase.SetInt(itemInfo.CfgId, "count", itemInfo.Num);
        //        }
        //    }
        //}

        private static void OnFormulaInfoProcess(object data)
        {
            //配方更新
            if (data is S2CGetFormulasResult)
            {
                var origin = (S2CGetFormulasResult)data;
                foreach (var formula in origin.Formulas)
                {
                    KDatabase.SetInt(formula, "count", 1);
                }
            }
            else if (data is S2CExchangeBuildingFormulaResult)
            {
                var origin = (S2CExchangeBuildingFormulaResult)data;
                KDatabase.SetInt(origin.FormulaId, "count", 1);
            }
        }

        private static void OnAreaInfoProcess(object data)
        {
            var protoData = (S2CGetAreasInfos)data;
            var areaTable = KDatabase.Database.GetTable(KDatabase.AREA_TABLE_NAME);
            foreach (var item in protoData.Areas)
            {
                areaTable.SetRow(item.CfgId, 0, "");
            }

            if (Build.AreaManager.Instance)
            {
                Build.AreaManager.Instance.AreaDataUpdate();
            }
        }

        //private static void OnChangeNameProcess(object data)
        //{
        //    var protoData = (S2CChgName)data;
        //    var player = KUser.SelfPlayer;
        //    player.nickName = protoData.Name;
        //    player.changeNameCount = protoData.ChgNameCount;
        //}

        //private static void OnChangeHeadProcess(object data)
        //{
        //    var protoData = (S2CChangeHead)data;
        //    var player = KUser.SelfPlayer;
        //    player.headURL = player.GetHeadIcon(protoData.NewHead);
        //    player.headID = protoData.NewHead;
        //}

        #endregion

        #region Unity 

        // Use this for initialization
        private void Awake()
        {
            Instance = this;
        }

        public override void Load()
        {
            InitProcess();
        }

        #endregion
    }
}

