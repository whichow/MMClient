// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "BuildingManager" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using Game.UI;
using Msg.ClientMessage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Build
{
    using Callback = System.Action<int, string, object>;
    /// <summary>
    /// 城建管理器，统一管理城建资源（城建创建 ，销毁，数据获取...）
    /// </summary>
    public class BuildingManager : SingletonUnity<BuildingManager>
    {
        #region 其他玩家建筑

        public class OtherPlayerInfo
        {
            public int PlayerId; // 玩家ID
            public string PlayerName; // 玩家昵称
            public int PlayerLevel; // 玩家等级
            public int PlayerVipLevel; // 玩家VIP等级
            public int PlayerGold; // 玩家金币
            public int PlayerDiamond; // 玩家钻石
            public int PlayerCharm; // 玩家魅力值
            public string PlayerHead; // 玩家头像
        }

        /// <summary>
        /// 其他玩家城建数据
        /// </summary>
        private List<ViewBuildingInfo> buildingInfoOtherlist;
        public OtherPlayerInfo otherPlayerInfo;

        public OtherPlayerInfo GetOtherPlayerInfo()
        {
            return otherPlayerInfo;
        }

        #endregion

        #region Field

        public readonly List<int> areaInfoList;

        private GameObject buildingParent;
        private GameObject buildingOtherParent;

        MainWindow _mainWindow;
        private MainWindow _MainWindow
        {
            get
            {
                if (_mainWindow == null)
                {
                    _mainWindow = KUIWindow.GetWindow<MainWindow>();
                    //_mainWindow.buildingInit = ShowEntityAll;
                }
                return _mainWindow;
            }
        }

        //private readonly List<KItemBuilding> entitiesInStorage = new List<KItemBuilding>();
        //private List<Building> entitiesToShow = new List<Building>(100);
        //private List<Building> entitiesToShowLast = new List<Building>(20);
        //private List<Building> entitiesToStart = new List<Building>(100);

        private Action<object> _entityDataChange;
        //private System.Func<List<Building>> 

        /// <summary>
        /// 玩家城建元素(数据对象)
        /// </summary>
        private static IList<BuildingInfo> buildingInfosServer;

        /// <summary>
        /// 玩家城建元素(显示对象)
        /// </summary>
        private Dictionary<int, Building> _entitiesDic = new Dictionary<int, Building>();

        /// <summary>
        /// 其它玩家城建(显示对象)
        /// </summary>
        private Dictionary<int, Building> _otherPlayerentities = new Dictionary<int, Building>();

        ///// <summary>
        ///// 本地数据（配置表）
        ///// </summary>
        //private Dictionary<int, KItemBuilding> _entityDataDictionary = new Dictionary<int, KItemBuilding>();
        ///// <summary>
        ///// 
        ///// </summary>
        //private KItemBuilding[] _entityDataArray;

        public bool isCreateFinish;
        public bool IsOneSelf { get; private set; }

        #endregion

        #region C&D
        public BuildingManager()
        {
            otherPlayerInfo = new OtherPlayerInfo();
            buildingInfoOtherlist = new List<ViewBuildingInfo>();
            areaInfoList = new List<int>();
        }
        #endregion

        #region 网络消息发送与处理

        /// <summary>
        /// 数据解析
        /// </summary>
        public void DataParse<T>(object data, out T[] dataBack)
        {
            ArrayList arrayList = data as ArrayList;
            List<T> list = new List<T>();
            foreach (var item in arrayList)
            {
                if (item.GetType() == typeof(T))
                {
                    T listTemp = (T)item;
                    if (listTemp != null)
                    {
                        list.Add(listTemp);
                    }
                }
            }
            if (list.Count > 0)
                dataBack = list.ToArray();
            else
                dataBack = null;
        }

        #region 其它玩家城建  拜访好友

        public bool isFriend { get; set; }

        private void InitOtherArea()
        {
            AreaManager.Instance.AreaDataOtherUpdate(areaInfoList);
        }

        /// <summary>
        ///实例化 其它玩家建筑
        /// </summary>
        private void InitOtherBuilding()
        {
            Instance.IsOneSelf = false;
            StartCoroutine(InitOtherBuilding(0f));
        }

        private void callBackVisitPlayer(int codeId, string content, object data)
        {
            if (codeId == 0)
            {
                ArrayList list = data as ArrayList;
                foreach (var item in list)
                {
                    S2CVisitPlayerResult s2CVisitPlayerResult = item as S2CVisitPlayerResult;
                    if (s2CVisitPlayerResult != null)
                    {
                        //BuildingStateMgr.Instance.BubbleAllHide();
                        otherPlayerDataParse(s2CVisitPlayerResult);
                        GameCamera.Block(this.gameObject, GameCamera.Restrictions.UI);
                        GameCamera.Allow(this.gameObject, GameCamera.Restrictions.UI, null, _MainWindow.GetFriendBtn());
                        InitOtherBuilding();
                        BuildingSurfaceManager.Instance.OnOtherSuifaceInfos(s2CVisitPlayerResult.Surfaces);
                        BuildingSurfaceManager.Instance.InitOtherSurface();
                        InitOtherArea();
                        _MainWindow.togglePlayer(true);
                        HideEntityAll();
                    }
                }
            }
            else
            {

            }
        }

        /// <summary>
        /// 拜访好友 服务器数据返回 数据解析
        /// </summary>
        /// <param name="s2CVisitPlayerResult"></param>
        private void otherPlayerDataParse(S2CVisitPlayerResult s2CVisitPlayerResult)
        {
            areaInfoList.Clear();
            foreach (var item in s2CVisitPlayerResult.Areas)
            {
                areaInfoList.Add(item.CfgId);
            }

            buildingInfoOtherlist.Clear();
            buildingInfoOtherlist.AddRange(s2CVisitPlayerResult.Buildings);

            otherPlayerInfo.PlayerCharm = s2CVisitPlayerResult.PlayerCharm;
            otherPlayerInfo.PlayerDiamond = s2CVisitPlayerResult.PlayerDiamond;
            otherPlayerInfo.PlayerGold = s2CVisitPlayerResult.PlayerGold;
            otherPlayerInfo.PlayerHead = KUser.SelfPlayer.GetHeadIcon(s2CVisitPlayerResult.PlayerHead);
            otherPlayerInfo.PlayerId = s2CVisitPlayerResult.PlayerId;
            otherPlayerInfo.PlayerLevel = s2CVisitPlayerResult.PlayerLevel;
            otherPlayerInfo.PlayerName = s2CVisitPlayerResult.PlayerName;
            otherPlayerInfo.PlayerVipLevel = s2CVisitPlayerResult.PlayerVipLevel;
        }

        public void VisitPlayer(int playerId, bool isFriend = false)
        {
            Debuger.Log("玩家ID:" + playerId);
            DestroyOtherEntityAll();
            this.isFriend = isFriend;
            KUser.VisitPlayer(playerId, callBackVisitPlayer);
        }

        /// <summary>
        /// 添加建筑实体 到词典  ,仅用于建筑创建方法
        /// </summary>
        /// <param name="entity"></param>
        public void AddOtherEntity(int buildingId, Building entity)
        {
            Building building;
            entity.buildingId = buildingId;
            if (_otherPlayerentities.TryGetValue(buildingId, out building))
            {
                building = entity;
            }
            else
            {
                _otherPlayerentities.Add(buildingId, entity);
            }

        }

        /// <summary>
        /// 删除所有其他玩家建筑元素
        /// </summary>
        /// <param name="buildingId"></param>
        public void DestroyOtherEntityAll()
        {
            //foreach (var item in _otherPlayerentities.Values)
            //{

            //}
            AreaManager.Instance.AreaDataUpdate();
            Destroy(buildingOtherParent);
            //buildingOtherParent = null;
            buildingOtherParent = new GameObject("OtherBuildings");
            buildingInfoOtherlist.Clear();
            //buildingInfosServer.Clear();
            _otherPlayerentities.Clear();

            areaInfoList.Clear();

            GameCamera.Unblock(this.gameObject);
            //GameCamera.Allow(this.gameObject, GameCamera.Restrictions.UI, null, _mainWindow.GetFriendBtn());

        }

        #endregion

        #region 城建操作向服务器请求

        /// <summary>
        /// 获取城建实例
        /// </summary>
        /// <param name="callback"></param>
        public void BuildingGetsRequest(Callback callback)
        {
            KUser.BuildingGets(callback);
        }

        /// <summary>
        /// 客户端向服务器请求设置建筑
        /// </summary>
        /// <param name="callback"></param>
        public void BuildingSetRequest(/*C2SSetBuilding buildingData, Callback callback*/)
        {
            Building date = GetEntitie(currOjbect);
            KItemBuilding entityData = date.entityData;

            Debuger.LogFormat("城建元素创建 开始--x:{0},y:{1},Id:{2},Dir:{3}",
                MapHelper.PositionToGrid(currOjbect.transform.position).x,
                MapHelper.PositionToGrid(currOjbect.transform.position).y,
               entityData.itemID,
               date.RotateDir);

            Int2 pos = MapHelper.PositionToGrid(currOjbect.transform.position);
            KUser.BuildingSet(new C2SSetBuilding()
            {
                X = pos.x,
                Y = pos.y,
                BuildingCfgId = entityData.itemID,
                Dir = date.RotateDir,
                IfBuy = date.IsNeedBuy,
                // 1,// isNeedBuy,
            },
            (id, content, data) =>
            {
                if (id == 0)
                {
                    KUIWindow.OpenWindow<BuildingShopWindow>();

                    BuildingCattery buildingCattery = CurrBuilding as BuildingCattery;
                    if (buildingCattery)
                    {
                        buildingCattery.DataRefurbish();
                    }
                    KUIWindow.GetWindow<BuildingShopWindow>().RefreshView();
                    //else
                    //{
                    //    Debug.LogWarning("创建当前建筑不存在" + id);
                    //}

                }
                else
                {
                    Debuger.Log("建筑创建失败:" + entityData.itemID);
                    PayDecline();
                    //Debug.Log("城建元素创建成功" + data.GetType()/*(data as S2CSetBuilding).X*/+ "/--/" + id);
                }
                CurrBuilding = null;
            }
            );
        }

        /// <summary>
        /// 请求移动建筑
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="callback"></param>
        public void BuildingMoveRequest(MapObject obj, Callback callback)
        {
            //Debug.Log("设置位置");
            if (!obj) return;

            Building building = GetEntitie(obj.gameObject);
            if (!building) return;

            Debuger.Log("旋转方向:" + building.RotateDir);
            Int2 Pos = MapHelper.PositionToGrid(obj.transform.position);
            Debuger.LogFormat("移动位置：x:{0};y:{1}---id：{2}---dir{3}", Pos.x, Pos.y, building.buildingId, building.RotateDir);
            C2SMoveBuilding data = new C2SMoveBuilding()
            {
                BuildingId = building.buildingId,
                X = Pos.x,
                Y = Pos.y,
                Dir = building.RotateDir
            };
            KUser.BuildingMove(data, callback);
        }

        /// <summary>
        /// 客户端向服务器请求回收建筑
        /// </summary>
        /// <param name="callback"></param>
        public void BuildingRecycleRequest(int id, Callback callback)
        {
            KUser.BuildingRecycle(id, callback);
        }

        /// <summary>
        /// 客户端向服务器请求出售建筑
        /// </summary>
        /// <param name="callback"></param>
        public void BuildingSellRequest(int id, Callback callback)
        {
            KUser.BuildingSell(id, callback);
        }

        /// <summary>
        /// 客户端向服务器请求清除障碍
        /// </summary>
        /// <param name="callback"></param>
        public void BuildingRemoveBlockRequest(int id, Callback callback)
        {
            KUser.BuildingRemoveBlock(id, callback);
        }

        /// <summary>
        /// 客户端向服务器请求开启地图宝箱
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public void BuildingOpenBoxRequest(int playerId, int id, Callback callback)
        {
            KUser.BuildingOpenBox(playerId, id, callback);
        }

        /// <summary>
        /// 客户端向服务器请求解锁地图区域
        /// </summary>
        /// <param name="id">区域Id</param>
        /// <param name="useStone">是否使用钻石快速解锁</param>
        /// <param name="callback"></param>
        public void BuildingUnlockAreaRequest(int id, int useStone, Callback callback)
        {
            KUser.BuildingUnlockArea(id, useStone, callback);
        }

        ///// <summary>
        ///// 创建城建元素
        ///// </summary>
        ///// <param name="code"></param>
        ///// <param name="message"></param>
        ///// <param name="data"></param>
        //private void OnBuildingSet(int code, string message, object data)
        //{
        //    var protoDatas = data as ArrayList;
        //    if (protoDatas != null)
        //    {
        //        for (int i = 0; i < protoDatas.Count; i++)
        //        {
        //            var protoData = protoDatas[i];
        //            if (protoData is S2CSetBuilding)
        //            {
        //                var originData = (S2CSetBuilding)protoData;
        //            }
        //        }
        //    }
        //}

        //private void OnBuildingUnlockArea(int code, string message, object data)
        //{
        //    var protoDatas = data as ArrayList;
        //    if (protoDatas != null)
        //    {
        //        for (int i = 0; i < protoDatas.Count; i++)
        //        {
        //            var protoData = protoDatas[i];
        //            if (protoData is S2CUnlockArea)
        //            {
        //                var originData = (S2CUnlockArea)protoData;
        //            }
        //        }
        //    }
        //}

        #endregion

        #region 城建服务器返回数据处理

        /// <summary>
        /// 同步服务器数据 (直接替换)
        /// </summary>
        /// <param name="buildingInfos"></param>
        public void OnSyncBuildingDataHandler(IList<BuildingInfo> buildingInfos)
        {
            Debuger.Log("同步服务器城建元素数据 count" + buildingInfos.Count);
            buildingInfosServer = buildingInfos;
        }

        /// <summary>
        /// 创建城建元素  服务器数据同步
        /// </summary>
        /// <param name="buildingInfos"></param>
        public void OnCreateBuildingsHandler(IList<BuildingInfo> buildingInfos)
        {
            Debuger.Log("城建元素创建成功" + buildingInfos.Count);
            for (int i = 0; i < buildingInfos.Count; i++)
            {
                if (!currOjbect)
                    continue;

                AddBuildingInfoServer(buildingInfos[i]);
                AddEntity(buildingInfos[i].Id, CurrBuilding);
                //GetEntitie(currOjbect).buildingId = buildingInfos[i].Id;  //创建成功后刷新id
                //Debug.Log("城建元素创建成功" + buildingInfos[i].Id);
            }
        }

        /// <summary>
        /// 彻底删除城建 从服务器数据
        /// </summary>
        /// <param name="buildingIds"></param>
        public void OnDeleteBuildingsHandler(IList<int> buildingIds)
        {
            for (int i = 0; i < buildingIds.Count; i++)
            {
                Debuger.Log("删除建筑ID:" + buildingIds[i]);
                DestroyEntity(buildingIds[i]);
            }
        }

        /// <summary>
        /// 更新建筑
        /// </summary>
        /// <param name="buildingInfos"></param>
        public void OnUpdateBuildingsHandler(IList<BuildingInfo> buildingInfos)
        {
            Debuger.Log("更新建筑count:" + buildingInfos.Count);
            BuildingInfo buildingInfo = null;
            for (int i = 0; i < buildingInfos.Count; i++)
            {
                Debuger.LogFormat("更新建筑 Id:{0}-- X:{1}--Y:{2}--Dir:{3}", buildingInfos[i].Id, buildingInfos[i].X, +buildingInfos[i].Y, buildingInfos[i].Dir);
                //buildingInfo = buildingInfosServer.Find((item) => { return item.Id == buildingInfos[i].Id; });
                for (int j = 0; j < buildingInfosServer.Count; j++)
                {
                    if (buildingInfosServer[j].Id == buildingInfos[i].Id)
                    {
                        buildingInfo = buildingInfosServer[j];
                        break;
                    }
                }
                if (buildingInfo != null)
                {
                    buildingInfo = buildingInfos[i];
                    Building building = GetEntitie(buildingInfo.Id);
                    if (building)
                        building.RotateDir = buildingInfo.Dir;
                }
                else
                {
                    buildingInfosServer.Add(buildingInfos[i]);
                    AddBuildingEntity(buildingInfos[i]);
                    //CreateEntity(data, new Int2(buildingInfos[i].X, buildingInfos[i].Y), buildingInfos[i].Id);
                }
            }
            //buildingInfosServer = buildingInfos;
        }

        public void ProcessCommon(object protoData)
        {
            if (protoData is S2CGetBuildingInfos)
            {
                var origin = (S2CGetBuildingInfos)protoData;
                OnSyncBuildingDataHandler(origin.Builds);
            }
            //建筑更新
            if (protoData is S2CBuildingsInfoUpdate)
            {
                var origin = (S2CBuildingsInfoUpdate)protoData;
                OnCreateBuildingsHandler(origin.AddBuildings);
                OnDeleteBuildingsHandler(origin.RemoveBuildings);
                OnUpdateBuildingsHandler(origin.UpdateBuildings);
            }
        }

        #endregion

        #endregion

        #region Methods     
        
        /// <summary>
        /// 打开寄养所
        /// </summary>
        /// <param name="openFosterCateBack"></param>
        public void OpenFosterCate(Action<bool> openFosterCateBack)
        {
            if (GetEntities<BuildingFosterCare>().Count > 0)
            {
                //AdoptionWindow.OpenAdoption(KUser.SelfPlayer.id);
                openFosterCateBack(true);
            }
            else
            {
                openFosterCateBack(false);
            }

        }

        /// <summary>
        /// 关闭打开主界面 菜单
        /// </summary>
        /// <param name="isShow">是否显示主界面菜单</param>
        public void ShowMainWindowMenu(bool isShow = true)
        {
            _MainWindow.MajorBtnClick(isShow);
        }

        /// <summary>
        /// 添加城建数据
        /// </summary>
        /// <param name="buildingInfo"></param>
        private void AddBuildingInfoServer(BuildingInfo buildingInfo)
        {
            //if (buildingInfosServer.Find(item => item == buildingInfo) == null)
            if (!buildingInfosServer.Contains(buildingInfo))
            {
                buildingInfosServer.Add(buildingInfo);
                var curBuilding = GetEntitie(currOjbect);
                if (curBuilding.entityData.cfgId == buildingInfo.CfgId)
                {
                    curBuilding.buildingId = buildingInfo.Id;
                    //AddBuildingEntity(buildingInfo);
                }
                else
                {
                    Debuger.LogError(string.Format("当前添加的建筑不是正在建筑的建筑! S2C_BCfgID:{0} - C2S_BCfgID:{1}", buildingInfo.CfgId, curBuilding.entityData.cfgId));
                }
            }
        }

        #region 

        public void entityDataChangeSet(System.Action<object> fun)
        {
            _entityDataChange = fun;
        }

        public void entityDataChangeCall()
        {
            if (_entityDataChange != null)
            {
                _entityDataChange(_entitiesDic);
            }
        }

        public BuildingFarmland currBuildingFarmland
        {
            get; set;
        }

        /// <summary>
        /// 获取地图上建筑的数量
        /// </summary>
        /// <param name="buildingID"></param>
        /// <returns></returns>
        public int BuildingTypeCountGet(int buildingID)
        {
            int count = 0;
            foreach (var item in _entitiesDic)
            {
                if (item.Value.entityData.itemID == buildingID)
                    count++;
            }
            return count;
        }

        /// <summary>
        /// 获取地图上所有可播种的农田
        /// </summary>
        /// <returns></returns>
        public List<BuildingFarmland> GetCanSowFarnlandAll()
        {
            List<BuildingFarmland> farmland = new List<BuildingFarmland>();
            foreach (var item in _entitiesDic)
            {
                IFunction iFunction = item.Value as IFunction;
                if (iFunction != null)
                {
                    if (iFunction.functionTypeType == Building.Category.kFarm)
                    {
                        BuildingFarmland buildingFarmland = item.Value as BuildingFarmland;
                        if (buildingFarmland.canSow)
                            farmland.Add(buildingFarmland);
                    }
                }
            }
            return farmland;
        }

        /// <summary>
        /// 获取下一个可种植农田
        /// </summary>
        /// <returns></returns>
        public BuildingFarmland GetNextCanSowFarmland()
        {
            if (currBuildingFarmland && currBuildingFarmland.canSow)
                return currBuildingFarmland;

            List<BuildingFarmland> farmland = GetCanSowFarnlandAll();
            farmland.Sort((x, y) =>
            {
                return x.buildingId.CompareTo(y.buildingId);
            });

            if (farmland.Count > 0)
                return farmland[0];
            else
                return null;
        }

        /// <summary>
        /// 获取所有可加速农田
        /// </summary>
        /// <returns></returns>
        public List<BuildingFarmland> GetBuildingFarmlandAll()
        {
            List<BuildingFarmland> farmland = new List<BuildingFarmland>();
            foreach (var item in _entitiesDic)
            {
                IFunction iFunction = item.Value as IFunction;
                if (iFunction != null)
                {
                    if (iFunction.functionTypeType == Building.Category.kFarm)
                    {
                        BuildingFarmland buildingFarmland = item.Value as BuildingFarmland;
                        if (buildingFarmland.canSpeedUp)
                            farmland.Add(buildingFarmland);
                    }
                }
            }
            return farmland;

        }

        /// <summary>
        /// 获取下一个可加速农田
        /// </summary>
        /// <returns></returns>
        public BuildingFarmland GetNextUpSleepFarmland()
        {
            if (currBuildingFarmland && currBuildingFarmland.canSpeedUp)
                return currBuildingFarmland;

            List<BuildingFarmland> farmland = GetBuildingFarmlandAll();
            farmland.Sort((x, y) =>
            {
                return x.remainTime.CompareTo(y.remainTime);
            });

            if (farmland.Count > 0)
                return farmland[0];
            else
                return null;
        }

        /// <summary>
        /// 获取建筑可容纳的最大数量
        /// </summary>
        /// <param name="dataId"></param>
        /// <returns></returns>
        public int GetBuildingMaxCount(int dataId)
        {
            var itemB = KItemManager.Instance.GetBuilding(dataId);
            if (itemB != null)
            {
                var category = (Building.Category)itemB.type;
                switch (category)
                {
                    case Building.Category.kCatHouse:
                        return KUser.SelfPlayer.maxCattery;
                    case Building.Category.kFarm:
                        return KUser.SelfPlayer.maxFarmland;
                    default:
                        return 1;
                }
            }
            return 0;
        }

        /// <summary>
        /// 获取地图上建筑的数量(特殊处理猫舍)
        /// </summary>
        /// <param name="dataID"></param>
        /// <returns></returns>
        public int GetEntityCount(int dataID)
        {
            var itemB = KItemManager.Instance.GetBuilding(dataID);
            if (itemB.type == (int)Building.Category.kCatHouse)
            {
                return GetEntityCatCount();
            }
            else
            {
                return BuildingTypeCountGet(dataID);
            }
        }

        /// <summary>
        /// 获取地图上猫舍的数量
        /// </summary>
        /// <returns></returns>
        private int GetEntityCatCount()
        {
            int count = 0;
            foreach (var entity in _entitiesDic)
            {
                if ((Building.Category)entity.Value.entityData.type == Building.Category.kCatHouse)
                {
                    count++;
                }
            }
            return count;
        }

        ///// <summary>
        ///// 获取第一个指定类型的建筑
        ///// </summary>
        ///// <param name="dataID"></param>
        ///// <returns></returns>
        //public Building GetEntityOfType(int dataID)
        //{
        //    var itemB = KItemManager.Instance.GetBuilding(dataID);


        //    foreach (var entity in _entities)
        //    {
        //        if(entity.Value.entityData.type ==itemB.type)
        //        return entity.Value;
        //    }
        //    return null;
        //}

        /// <summary>
        /// 获取第一个指定类型的建筑
        /// </summary>
        /// <param name="dataID"></param>
        /// <returns></returns>
        public Building GetEntityOfType(int cfgID)
        {
            foreach (var entity in _entitiesDic)
            {
                if (entity.Value.entityData.cfgId == cfgID)
                    return entity.Value;
            }
            return null;
        }

        /// <summary>
        /// 根据建筑类型获取所有
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public List<Building> GetEntityAllOfType(Building.Category category)
        {
            List<Building> list = new List<Building>();
            foreach (var entity in _entitiesDic)
            {
                if ((Building.Category)entity.Value.entityData.type == category)
                {
                    list.Add(entity.Value);
                }

            }
            return list;
        }

        /// <summary>
        /// 获取某种配置类型的建筑物
        /// </summary>
        /// <param name="buildingId">建筑id</param>
        /// <returns></returns>
        public Building GetEntityOfCfgType(int buildingCfgId)
        {
            foreach (var entity in _entitiesDic)
            {
                if (entity.Value.entityData.cfgId == buildingCfgId)
                {
                    return entity.Value;
                }
            }
            return null;
        }

        #endregion

        #endregion

        #region 建筑实体词典操作

        /// <summary>
        /// 添加建筑实体（显示对象） 到词典  ,仅用于建筑创建方法
        /// </summary>
        /// <param name="entity"></param>
        public void AddEntity(int buildingId, Building entity)
        {
            if (entity == null)
            {
                Debuger.LogError("添加的建筑为空! " + buildingId);
                return;
            }

            Building building;
            entity.buildingId = buildingId;
            if (_entitiesDic.TryGetValue(buildingId, out building))
            {
                building = entity;
            }
            else
            {
                _entitiesDic.Add(buildingId, entity);
            }
        }

        /// <summary>
        /// 删除城建元素 在词典里的元素
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool RemoveEntity(int buildingId)
        {
            return _entitiesDic.Remove(buildingId);
        }

        /// <summary>
        /// 删除城建所有元素 在词典里的元素
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void RemoveEntityAll()
        {
            _entitiesDic.Clear();
        }

        /// <summary>
        /// 彻底删除指定id的建筑
        /// </summary>
        /// <param name="buildingId"></param>
        public void DestroyEntity(int buildingId)
        {
            Building building;
            if (_entitiesDic.TryGetValue(buildingId, out building))
            {
                Debuger.Log("建筑总数 删除前 " + buildingInfosServer.Count);
                //buildingInfosServer.RemoveAll(item => { return item.Id == buildingId; });
                for (var i = buildingInfosServer.Count - 1; i >= 0; i--)
                {
                    if (buildingInfosServer[i].Id == buildingId)
                    {
                        buildingInfosServer.Remove(buildingInfosServer[i]);
                    }
                }
                Debuger.Log("建筑总是 删除后" + buildingInfosServer.Count);
                _entitiesDic.Remove(buildingId);

                building.DelBuilding();
                // Destroy(building.gameObject);
            }
        }

        ///// <summary>
        ///// 删除所有建筑元素(显示对象)
        ///// </summary>
        ///// <param name="buildingId"></param>
        //public void DestroyEntityAll()
        //{
        //    foreach (var item in _entitiesDic)
        //    {
        //        Destroy(item.Value.gameObject);
        //    }
        //    //buildingInfosServer.Clear();
        //    _entitiesDic.Clear();
        //}

        /// <summary>
        /// 隐藏玩家所有建筑
        /// </summary>
        private void HideEntityAll()
        {
            buildingParent.gameObject.SetActive(false);
            //foreach (var item in _entities.Values)
            //{
            //     item.gameObject.SetActive(false);
            //}
        }

        /// <summary>
        /// 显示玩家所有建筑
        /// </summary>
        public void ShowEntityAll(bool friend = false)
        {
            Instance.IsOneSelf = true;
            isFriend = friend;
            BuildingStateMgr.Instance.BubbleAllShow();
            DestroyOtherEntityAll();
            buildingParent.gameObject.SetActive(true);
            if (isFriend)
            {
                AreaManager.Instance.AreaDataUpdate();
            }
            //foreach (var item in _entities.Values)
            //{
            //    item.gameObject.SetActive(true);
            //}
        }

        /// <summary>
        /// 从词典中获取 城建元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetEntities<T>() where T : Building
        {
            List<T> retList = new List<T>();
            foreach (var entity in _entitiesDic)
            {
                var target = entity.Value as T;
                if (target != null)
                {
                    retList.Add(target);
                }
            }
            return retList;
        }

        /// <summary>
        /// 获取建筑（显示对象）
        /// </summary>
        /// <param name="buildingId"></param>
        /// <returns></returns>
        public Building GetEntitie(int buildingId)
        {
            Building building;
            _entitiesDic.TryGetValue(buildingId, out building);
            return building;
        }

        /// <summary>
        /// 根据配置ID获取建筑数据
        /// </summary>
        /// <param name="buildingId"></param>
        /// <returns></returns>
        public GameObject GetEntitieObj(int buildingCfgId)
        {
            foreach (var item in _entitiesDic)
            {
                if (item.Value.entityData.cfgId == buildingCfgId)
                    return item.Value.gameObject;
            }
            return null;
        }

        /// <summary>
        /// 获取城建元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Building GetEntitie(GameObject gm)
        {
            return gm.GetComponent<Building>();
        }

        //private void buildingAdd(int buildingId)
        //{
        //    Instance.AddEntity(buildingId, CurrBuilding);
        //    if (_entityDataChange != null)
        //        _entityDataChange(_entities);
        //}

        #endregion

        #region Static Method

        /// <summary>
        /// 当前正在创建的城建元素节点
        /// </summary>
        private static GameObject currOjbect;
        /// <summary>
        /// 当前正在建设的城建元素
        /// </summary>
        public static Building CurrBuilding;

        public static event Action<Building, bool> OnGlobalBuilt;

        public static event Action<Building, bool> OnGlobalConfirm;

        public static event Action<Building> OnGlobalPlaced;

        //public event Action OnPlaced;

        static BuildingManager()
        {
            OnGlobalConfirm += (builder, _b) =>
            {
                Map.BeforeDragCancel = null;
                Map.BeforeDragFinish = null;
            };
        }

        //private BuildingView CretateEntityView(KItemBuilding data)
        //{
        //    return null;
        //}

        /// <summary>
        /// 销毁当前要建设的物体
        /// </summary>
        public static void PayDecline()
        {
            OnGlobalConfirm(null, false);
            if (currOjbect)
            {
                Destroy(currOjbect);
            }
            // BuildingManager.Instance.RemoveEntity(GetEntitie(currOjbect.gameObject));
            KUIWindow.OpenWindow<BuildingShopWindow>();
        }

        ///// <summary>
        ///// 城建元素创建位置确认  ，创建后事件回调
        ///// </summary>
        ///// <param name="_time"></param>
        ///// <param name="_forced"></param>
        //public static void BuildTimerStart(int _time, bool _forced = false)
        //{
        //    KUIWindow.OpenWindow<BuildingShopWindow>();
        //    //KUIWindow.GetWindow<>()
        //    OnGlobalConfirm(null, true);
        //    if (_forced || TakeMoney())
        //    {
        //        if (_time <= 0)
        //        {
        //        }
        //        else
        //        {
        //        }
        //    }
        //}

        //public static bool TakeMoney()
        //{
        //    return true;
        //}

        private static void TryCancel(ref bool isAllowed, bool failedToAccept)
        {
            //if (GUIManager.Instanse.AnyWindowsOpened(true, true))
            //{
            //    _isAllowed = true;
            //}
            //else 
            if (GameCamera.IsRestriction(GameCamera.Restrictions.Click) || failedToAccept)
            {
                isAllowed &= !failedToAccept;
            }
        }

        private static int NoConfirm(ref bool showConfirm)
        {
            return 2;
            //showConfirm = false;
        }

        #endregion

        #region Method

        //private bool _isRegisterCameraReachEvent;
        private System.Action<bool, Vector3> _buildingFocusBack;
        private Building buildingFocus;
        private System.Action _zoomFocus;

        public void SetBuildingFocus(int cfgBuildingId, System.Action zoomFocus = null, System.Action<bool, Vector3> focusBack = null)
        {
            //if (!isRegisterCameraReachEvent)
            //cfgBuildingId
            Debuger.Log("聚焦：" + cfgBuildingId);
            buildingFocus = GetEntityOfType(cfgBuildingId);
            if (!buildingFocus)
            {
                if (focusBack != null)
                    focusBack(false, Vector2.zero);
                return;
            }
            _zoomFocus = zoomFocus;
            _buildingFocusBack = focusBack;
            GameCamera.OnCameraReach += OnCameraReachCallBack;
            GameCamera.Instance.Show(buildingFocus.entityView.centerNode.position);
            GameCamera.Instance.zoomFocus(this.ZoomFocus);

            GameCamera.Block(buildingFocus.gameObject, GameCamera.Restrictions.All);

        }

        private void ZoomFocus()
        {
            GameCamera.Disallow(buildingFocus.gameObject);
            GameCamera.Unblock(buildingFocus.gameObject);
            if (buildingFocus != null)
            {
                Debuger.Log("打开功能按键" + (Building.Category)buildingFocus.entityData.type);
                if ((Building.Category)buildingFocus.entityData.type == Building.Category.kCatHouse)
                {
                    var function = buildingFocus as IFunction;
                    if (function != null)
                    {
                        Debuger.Log("打开功能按键");
                        KUIWindow.OpenWindow<Game.UI.FunctionWindow>(function);
                    }
                }
            }
            if (_zoomFocus != null)
            {
                _zoomFocus();
            }
        }

        public void SetBuildingFocus(Vector2 pos)
        {
            Vector3 pos3 = new Vector3(pos.x, pos.y, 0);
            GameCamera.Instance.Show(pos3);
        }

        /// <summary>
        /// 开启屏蔽建筑功能
        /// </summary>
        /// <param name="category">建筑类型</param>
        /// <param name="isShield">是否开启, true 开启</param>
        public void ShieldBuildingFun(Building.Category category, bool isShield)
        {
            List<Building> list = GetEntityAllOfType(category);
            foreach (var item in list)
            {
                PolygonCollider2D polygonCollider2D = item.GetComponent<PolygonCollider2D>();
                if (polygonCollider2D)
                    polygonCollider2D.enabled = isShield;
            }
        }

        /// <summary>
        /// 是否存在某种建筑
        /// </summary>
        /// <param name="category">建筑类型</param>
        public bool isExistBuilding(Building.Category category)
        {
            List<Building> list = GetEntityAllOfType(category);
            return list.Count > 0;
        }

        /// <summary>
        /// 是否存在某种建筑
        /// </summary>
        /// <param name="category">建筑类型</param>
        public bool isExistBuilding(int buildingId)
        {
            return GetEntityOfCfgType(buildingId);
        }

        /// <summary>
        /// 农田作物成熟
        /// </summary>
        /// <param name="farmId">作物Id</param>
        public bool isFarmHarvest(int farmId)
        {
            return KFarm.Instance.isHarvest(farmId);
        }

        /// <summary>
        /// 农田作物播种后
        /// </summary>
        /// <param name="farmId">作物Id</param>
        public bool isFarmSowing(int farmId)
        {
            return KFarm.Instance.isSowing(farmId);
        }

        private void OnCameraReachCallBack()
        {
            if (_buildingFocusBack != null)
                _buildingFocusBack(true, ScreenCoordinateTransform.Instance.WorldScenePointToUIPiontGet(buildingFocus.transform.position));
            GameCamera.OnCameraReach -= OnCameraReachCallBack;
        }

        #endregion

        #region 初始化建筑

        /// <summary>
        /// 初始化网络城建所有元素
        /// </summary>
        public void InitBuilding()
        {
            Instance.IsOneSelf = true;
            StartCoroutine(InitEntityAll(0f));
        }

        IEnumerator InitEntityAll(float time = 0)
        {
            Debuger.LogWarning("初始化所有建筑" + buildingInfosServer.Count);
            float timeDuration = Time.deltaTime - Time.deltaTime / 50;
            //Debug.Log("delta"+ Time.deltaTime);
            float timeCur = Time.realtimeSinceStartup;
            List<BuildingInfo> screenBuilding = new List<BuildingInfo>();
            List<BuildingInfo> notScreenBiulding = new List<BuildingInfo>();
            //Vector3 pos;
            foreach (var item in buildingInfosServer)
            {
                //pos.x = MapHelper.GridToPosition( item.X,item.Y);
                //Debug.Log(MapHelper.GridToPosition(item.X, item.Y));
                if (ScreenCoordinateTransform.Instance.IsSceneScreenCoord(MapHelper.GridToPosition(item.X, item.Y)))
                    screenBuilding.Add(item);
                else
                    notScreenBiulding.Add(item);
            }
            for (int i = 0; i < screenBuilding.Count; i++)
            {
                AddBuildingEntity(screenBuilding[i]);
                //GameObject gm= new GameObject("text");
                //gm.transform.position = MapHelper.GridToPosition(screenBuilding[i].X, screenBuilding[i].Y);
            }
            for (int i = 0; i < notScreenBiulding.Count; i++)
            {
                AddBuildingEntity(notScreenBiulding[i]);
                //Debug.Log("使用的时间"+ (Time.realtimeSinceStartup - timeCur));
                //if (Time.realtimeSinceStartup - timeCur > timeDuration)
                //{
                //    Debug.Log("一帧时间使用完毕");
                //    yield return  0;
                //}
                yield return new WaitForSeconds(Time.deltaTime / 200);
            }
            isCreateFinish = true;
            Instance.entityDataChangeCall();
        }

        IEnumerator InitOtherBuilding(float time = 0)
        {
            Debuger.LogWarning("初始化所有建筑" + buildingInfoOtherlist.Count);
            Building building;
            for (int i = 0; i < buildingInfoOtherlist.Count; i++)
            {
                building = AddBuildingEntity(buildingInfoOtherlist[i].BaseData, false);
                if (!IsOneSelf)
                {

                    building.viewBuildingInfo = buildingInfoOtherlist[i];
                }

                yield return new WaitForSeconds(time);
            }
        }

        public Building AddBuildingEntity(BuildingInfo buildingInfo, bool isOneSelf = true)
        {
            KItemBuilding itemBuilding = KItemManager.Instance.GetBuilding(buildingInfo.CfgId);
            //Debug.Log("一创建的元素,位置：" + buildingInfosServer[i].X + "//" + buildingInfosServer[i].Y);
            //Debug.Log("一创建的元素,位置：" + MapHelper.GridToPosition(buildingInfosServer[i].X, buildingInfosServer[i].Y));
            Building.Date data = new Building.Date()
            {
                kItemBuilding = itemBuilding,
                dir = buildingInfo.Dir,
                initType = Building.InitType.Server
            };
            return CreateEntity(data, new Int2(buildingInfo.X, buildingInfo.Y), buildingInfo.Id, isOneSelf);
        }

        /// <summary>
        /// 创建网络城建元素
        /// </summary>
        /// <param name="entityData"></param>
        /// <param name="pos"></param>
        /// <param name="buildingId"></param>
        /// <returns></returns>
        private Building CreateEntity(Building.Date data, Int2 pos, int buildingId, bool isOneSelf = true)
        {
            Building entity = null;
            //RemoveEntityAll();
            if (data.kItemBuilding != null)
            {
                switch ((Building.Category)data.kItemBuilding.type)
                {
                    case Building.Category.kFarm:
                        entity = Building.CreateEntity<BuildingFarmland>(data);
                        break;
                    case Building.Category.kCatHouse:
                        entity = Building.CreateEntity<BuildingCattery>(data);
                        break;
                    case Building.Category.kLifePool:
                        entity = Building.CreateEntity<BuildingLifePool>(data);
                        break;
                    case Building.Category.kManualWorkShop:
                        entity = Building.CreateEntity<BuildingManualWorkShop>(data);
                        break;
                    case Building.Category.kExpeditionZone:
                        entity = Building.CreateEntity<BuildingExpeditionZone>(data);
                        break;
                    case Building.Category.kTree:
                        entity = Building.CreateEntity<BuildingTree>(data);
                        break;
                    case Building.Category.kFosterCare:
                        entity = Building.CreateEntity<BuildingFosterCare>(data);
                        break;
                    case Building.Category.kTakePhotos:
                        entity = Building.CreateEntity<BuildingTakePhotos>(data);
                        break;
                    case Building.Category.kBox:
                        entity = Building.CreateEntity<BuildingBox>(data);
                        break;
                    case Building.Category.kObstacle:
                        entity = Building.CreateEntity<BuildingObstacle>(data);
                        break;
                    case Building.Category.kDecoration:
                        entity = Building.CreateEntity<BuildingDecorate>(data);
                        break;
                    case Building.Category.kSurface:
                        entity = Building.CreateEntity<BuildingSurface>(data);
                        break;
                    default:
                        entity = Building.CreateEntity<BuildingDefault>(data);
                        break;
                }
            }
            Vector3 touchPoint = MapHelper.GridToPosition(pos);
            entity.CurrCategory = (Building.Category)data.kItemBuilding.type;
            //CurrBuilding = entity; //???
            entity.transform.localPosition = touchPoint;
            entity.buildingId = buildingId;
            //entity.RotateDir = buildingInfo.Dir;
            //entity.IsServerBuilding = true;
            var mapItem = entity.gameObject.AddComponent<MapObject>();
            mapItem.mapSize = data.kItemBuilding.mapSize;
            if (isOneSelf)
            {
                Instance.AddEntity(buildingId, entity);
                entity.transform.SetParent(buildingParent.transform, false);
            }
            else
            {
                Instance.AddOtherEntity(buildingId, entity);
                entity.transform.SetParent(buildingOtherParent.transform, false);
            }
            currOjbect = entity.gameObject;
            IFunCommon funCommon = entity as IFunCommon;

            if (funCommon != null && funCommon.IsRotate && entity.RotateDir > 0)
            {
                mapItem.RotateMap();
            }

            //Map.Instance.RescindMapNode(mapItem);
            Map.Instance.ApplyMapNode(mapItem);

            //Debug.Log(entity.buildingId);
            return entity;
        }

        #endregion

        #region 建造时

        /// <summary>
        /// 创建指定类型的城建元素
        /// </summary>
        /// <param name="data">城建元素数据</param>
        /// <returns></returns>
        public Building CreateBuilding(KItemBuilding itemBuilding, bool isDrag)
        {
            Building.Date data = new Building.Date()
            {
                kItemBuilding = itemBuilding,
                dir = 0,
                initType = Building.InitType.Create
            };

            Vector3 pos;
            if (isDrag)
            {
                pos = Input.mousePosition;
            }
            else
            {
                pos = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f);
            }
            return CreateBuilding(data, new Int2(), false, pos);
        }

        /// <summary>
        /// 创建指定类型的城建元素
        /// </summary>
        /// <param name="data">城建元素数据</param>
        /// <param name="pos">网格坐标</param>
        /// <param name="isUsePos">是否使用网格坐标</param>
        /// <param name="screenPos">屏幕坐标</param>
        /// <returns></returns>
        public Building CreateBuilding(Building.Date data, Int2 pos, bool isUsePos, Vector3 screenPos)
        {

            Map.BeforeDragCancel = TryCancel;
            Map.BeforeDragFinish = NoConfirm;
            Vector3 touchPoint;
            if (isUsePos)
                touchPoint = MapHelper.GridToPosition(pos);
            else
                touchPoint = MapHelper.AlignToGrid(GameCamera.Instance.ScreenPointToNavCoord(screenPos));

            Building entity = null;


            switch ((Building.Category)data.kItemBuilding.type)
            {
                case Building.Category.kFarm:
                    entity = Building.CreateEntity<BuildingFarmland>(data);
                    break;
                case Building.Category.kCatHouse:
                    entity = Building.CreateEntity<BuildingCattery>(data);
                    break;
                case Building.Category.kLifePool:
                    entity = Building.CreateEntity<BuildingLifePool>(data);
                    break;
                case Building.Category.kManualWorkShop:
                    entity = Building.CreateEntity<BuildingManualWorkShop>(data);
                    break;
                case Building.Category.kExpeditionZone:
                    entity = Building.CreateEntity<BuildingExpeditionZone>(data);
                    break;
                case Building.Category.kTree:
                    entity = Building.CreateEntity<BuildingTree>(data);
                    break;
                case Building.Category.kFosterCare:
                    entity = Building.CreateEntity<BuildingFosterCare>(data);
                    break;
                case Building.Category.kTakePhotos:
                    entity = Building.CreateEntity<BuildingTakePhotos>(data);
                    break;
                case Building.Category.kDecoration:
                    entity = Building.CreateEntity<BuildingDecorate>(data);
                    break;
                case Building.Category.kSurface:
                    entity = Building.CreateEntity<BuildingSurface>(data);
                    break;
                default:
                    entity = Building.CreateEntity<BuildingDefault>(data);
                    break;
            }
            entity.CurrCategory = (Building.Category)data.kItemBuilding.type;
            CurrBuilding = entity;
            entity.transform.localPosition = touchPoint;
            entity.transform.SetParent(buildingParent.transform, false);
            //entity.entityData.RotateDir = 180;
            //entity.IsServerBuilding = false;
            var mapItem = entity.gameObject.AddComponent<MapObject>();
            mapItem.mapSize = data.kItemBuilding.mapSize;



            currOjbect = entity.gameObject;
            if (!isUsePos)
            {
                Action dragConfirm = () =>
                {
                    BuildingManager.Instance.BuildingSetRequest();
                    //Building currNode = GetEntitie(currOjbect);
                    //if (currNode)
                    //    currNode.BuildingSet();
                    //else
                    //    Debug.Log("未加载指定的 building");
                };
                Action dragCancel = () => PayDecline();
                Action onSell = null;
                Action onRotate = null;
                IFunCommon funCommon = entity as IFunCommon;
                if (funCommon != null)
                {
                    if (funCommon.IsRotate)
                    {
                        onRotate = () => funCommon.OnRotate();
                    }
                    if (funCommon.IsSell)
                    {
                        onSell = () => funCommon.OnSell();
                    }
                }
                Map.Instance.StartDrag(mapItem, dragConfirm, dragCancel, onRotate, onSell);
            }
            else
            {
                Map.Instance.StartDrag(mapItem);
            }

            Map.Instance.IsBuildingMoveState = true;

            return null;
        }

        public void CurBuildingMove(Vector3 screenPos)
        {
            Vector3 touchPoint = MapHelper.AlignToGrid(GameCamera.Instance.ScreenPointToNavCoord(screenPos));
            if (Map.Instance.CurrMapObject != null)
            {
                Map.Instance.CurrMapObject.transform.localPosition = touchPoint;
            }
            else if (CurrBuilding != null)
            {
                CurrBuilding.transform.localPosition = touchPoint;
            }
        }

        public void CurBuildingMove(Int2 pos)
        {
            Vector3 touchPoint = MapHelper.GridToPosition(pos);
            if (Map.Instance.CurrMapObject != null)
            {
                Map.Instance.CurrMapObject.transform.localPosition = touchPoint;
            }
            else if (CurrBuilding != null)
            {
                CurrBuilding.transform.localPosition = touchPoint;
            }
        }

        #endregion

        #region Unity
        void Start()
        {
            buildingParent = new GameObject("BuildingSelf");
            buildingOtherParent = new GameObject("BuildingOther");
        }
        #endregion

    }
}
