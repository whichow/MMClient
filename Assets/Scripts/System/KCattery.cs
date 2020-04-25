// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 2017-10-30
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KCattery" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using Game.Build;
using Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    using Callback = System.Action<int, string, object>;

    /// <summary>
    /// 猫舍数据管理
    /// </summary>
    public class KCattery : KGameModule
    {
        #region Struct

        public class CatteryInfo
        {
            private int _coin;
            private int _coinTimestamp;
            private int _upgradeRemainTimestamp;
            private int _produceGoldRemainSeconds;  //产金剩余时间   -1表示没有产金  >=0表示产金剩余时间

            /// <summary>
            /// 实例id
            /// </summary>
            public int buildingId;
            /// <summary>
            /// 当前等级
            /// </summary>
            public int grade;
            public int isDone;//1已经0还没有

            /// <summary>
            /// 当前产金
            /// </summary>
            public int coin
            {
                get
                {
                    int coinAbility = 0;
                    foreach (var catId in catIds)
                    {
                        var cat = KCatManager.Instance.GetCat(catId);
                        if (cat == null)
                        {
                            //Debug.LogError("猫 id不存在猫列表里");
                            //throw new System.NullReferenceException("猫 id不存在猫列表里");
                            return 0;
                        }
                        coinAbility += CatDataModel.Instance.GetCatDataVOById(catId).mCatInfo.CoinAbility;
                    }

                    //var add = (KLaunch.Timestamp - _coinTimestamp) * coinAbility / 60;

                    //改为固定8小时结算了
                    var add = (8 * 60 * 60 - (_produceGoldRemainSeconds - KLaunch.Timestamp)) * coinAbility / 60;
                    return add;
                }
                set { _coin = value; _coinTimestamp = KLaunch.Timestamp; }
            }

            /// <summary>
            /// 猫咪
            /// </summary>
            public int[] catIds;

            public int upgradeRemainTime
            {
                get { return _upgradeRemainTimestamp - KLaunch.Timestamp; }
                set
                {
                    _upgradeRemainTimestamp = KLaunch.Timestamp + value;
                }
            }

            /// <summary>
            /// 产金剩余时间   -1表示没有产金  >=0表示产金剩余时间
            /// </summary>
            public int ProduceGoldRemainSeconds {
                get
                {
                    if (_produceGoldRemainSeconds > -1)
                    {
                        int val = _produceGoldRemainSeconds - KLaunch.Timestamp;
                        if (val < 0) val = 0;
                        return val;
                    }
                    else
                    {
                        return _produceGoldRemainSeconds;
                    }
                }
                set
                {
                    if (value > -1)
                    {
                        _produceGoldRemainSeconds = KLaunch.Timestamp + value;
                    }
                    else
                    {
                        _produceGoldRemainSeconds = value;
                    }
                }
            }

            //下面是配置数据
            public int maxGrade
            {
                get
                {
                    var building = KItemManager.Instance.GetBuilding(shopId);
                    if (building != null)
                    {
                        return building.maxGrade;
                    }
                    return 1;
                }
            }

            public int shopId;
            public int unlockStar;
            public int unlockGrade;
            /// <summary>
            /// 当前最多能放少个猫
            /// </summary>
            public int catStorage;
            public int nextCatStorage;
            public int coinStorage;
            public int nextCoinStorage;
            public int upgradeCost;
            public int nextUpgradeCost;
            public int upgradeTime;
            public int nextUpgradeTime;
            public int catColor;
            public int salePrice;
            public int showCoinValue;

            /// <summary>
            /// 当前默认动画名
            /// </summary>
            public string idleAnim;
            public string touchAnim;
        }

        private class CatteryConfig
        {
            public int id;
            public int buildingId;
            public int grade;
            public int unlockStar;
            public int unlockGrade;
            public int upgradeCost;
            public int catStorage;
            public int coinStorage;
            public int upgradeTime;
            public int catColor;
            public int salePrice;
            public int showCoinValue;
            public string idleAnim;
            public string touchAnim;
        }

        #endregion

        #region Field

        private Dictionary<int, CatteryConfig> _catteryConfigs = new Dictionary<int, CatteryConfig>();
        private Dictionary<int, CatteryInfo> _catteryInfos = new Dictionary<int, CatteryInfo>();

        #endregion

        #region Property      

        #endregion

        #region Method

        public bool IsCatteryCompleted(int shopId)
        {
            foreach (var kv in _catteryInfos)
            {
                CatteryInfo ci = kv.Value;
                if (ci.shopId == shopId)
                {
                    return ci.isDone == 1;
                }
            }
            return false;
        }

        private CatteryConfig GetCatteryConfig(int buildingId, int grade)
        {
            var id = buildingId * 1000 + grade;
            CatteryConfig catteryConfig;
            _catteryConfigs.TryGetValue(id, out catteryConfig);
            return catteryConfig;
        }

        //内部调用
        public void UpdataCatteryConfig(CatteryInfo catteryInfo)
        {
            if (catteryInfo.grade == 0)   // 建筑建造时， 服务器初始化 建筑等级为（grade）0 
            {
                var nextConfig = GetCatteryConfig(catteryInfo.shopId, catteryInfo.grade + 1);
                if (nextConfig != null)
                {
                    catteryInfo.nextCatStorage = nextConfig.catStorage;
                    catteryInfo.nextCoinStorage = nextConfig.coinStorage;
                    catteryInfo.nextUpgradeCost = nextConfig.upgradeCost;
                    catteryInfo.nextUpgradeTime = nextConfig.upgradeTime;
                }
                return;

            }
            var config = GetCatteryConfig(catteryInfo.shopId, catteryInfo.grade);

            if (config != null)
            {
                //catteryInfo.shopId = config.buildingId;
                catteryInfo.unlockStar = config.unlockStar;
                catteryInfo.unlockGrade = config.unlockGrade;
                catteryInfo.upgradeCost = config.upgradeCost;
                catteryInfo.catStorage = config.catStorage;
                catteryInfo.coinStorage = config.coinStorage;
                catteryInfo.upgradeTime = config.upgradeTime;
                catteryInfo.catColor = config.catColor;
                catteryInfo.salePrice = config.salePrice;
                catteryInfo.showCoinValue = config.showCoinValue;
                catteryInfo.idleAnim = config.idleAnim;
                catteryInfo.touchAnim = config.touchAnim;

                var nextConfig = GetCatteryConfig(catteryInfo.shopId, catteryInfo.grade + 1);
                if (nextConfig != null)
                {
                    catteryInfo.nextCatStorage = nextConfig.catStorage;
                    catteryInfo.nextCoinStorage = nextConfig.coinStorage;
                    catteryInfo.nextUpgradeCost = nextConfig.upgradeCost;
                    catteryInfo.nextUpgradeTime = nextConfig.upgradeTime;
                }
            }
        }

        public CatteryInfo GetCatteryInfo(int buildingId)
        {
            CatteryInfo catteryInfo;
            _catteryInfos.TryGetValue(buildingId, out catteryInfo);
            return catteryInfo;
        }

        public CatteryInfo GetCatteryInfoByCfgID(int cfgID)
        {
            CatteryInfo catteryInfo = null;
            foreach (var kv in _catteryInfos)
            {
                CatteryInfo ci = kv.Value;
                if (ci.shopId == cfgID)
                {
                    catteryInfo = ci;
                    break;
                }
            }
            return catteryInfo;
        }

        public void GetAllInfos(Callback callback)
        {
            KUser.CatteryGets((code, message, data) =>
            {
                if (code == 0)
                {
                    OnGetAllInfos(code, message, data);
                }
                if (callback != null)
                {
                    callback(code, message, data);
                }
            });
        }

        public void GetInfo(int id, Callback callback)
        {
            KUser.CatteryGet(id, (code, message, data) =>
             {
                 if (code == 0)
                 {
                     OnGetAllInfos(code, message, data);
                 }
                 if (callback != null)
                 {
                     callback(code, message, data);
                 }
             });
        }

        /// <summary>
        /// 猫舍开始升级
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public void Upgrade(int id, Callback callback)
        {
            KUser.CatteryUpgrade(id, (code, message, data) =>
            {
                if (code == 0)
                {
                    OnGetAllInfos(code, message, data);
                }
                if (callback != null)
                {
                    callback(code, message, data);
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public void SpeedUp(int id, Callback callback)
        {
            KUser.CatterySpeedUp(id, (code, message, data) =>
            {
                if (code == 0)
                {
                    OnGetAllInfos(code, message, data);
                }
                if (callback != null)
                {
                    callback(code, message, data);
                }
            });
        }

        /// <summary>
        /// 猫舍完成(升级 建筑)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public void Complete(int id, Callback callback)
        {
            KUser.CatteryComplete(id, (code, message, data) =>
            {
                if (code == 0)
                {
                    OnGetAllInfos(code, message, data);
                }
                if (callback != null)
                {
                    callback(code, message, data);
                }
            });
        }

        /// <summary>
        /// 收集金币
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public void Harvest(int id, Callback callback)
        {
            KUser.CatteryHarvest(id, (code, message, data) =>
            {
                if (code == 0)
                {
                    OnGetAllInfos(code, message, data);
                }
                if (callback != null)
                {
                    callback(code, message, data);
                }
            });
        }

        /// <summary>
        /// 出售建筑
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public void Sell(int id, Callback callback)
        {
            KUser.CatterySell(id, (code, message, data) =>
            {
                if (code == 0)
                {
                    OnGetAllInfos(code, message, data);
                }
                if (callback != null)
                {
                    callback(code, message, data);
                }
            });
        }


        public void AddCat(int id, int catId, Callback callback)
        {
            KUser.CatteryAddCat(id, catId, (code, message, data) =>
             {
                 if (code == 0)
                 {
                     OnGetAllInfos(code, message, data);
                 }
                 if (callback != null)
                 {
                     callback(code, message, data);
                 }
             });
        }

        public void RemoveCat(int id, int catId, Callback callback)
        {
            foreach (var catteryInfo in _catteryInfos.Values)
            {
                if (System.Array.Exists(catteryInfo.catIds, cid => cid == catId))
                {
                    id = catteryInfo.buildingId;
                    break;
                }
            }

            KUser.CatteryRemoveCat(id, catId, (code, message, data) =>
            {
                if (code == 0)
                {
                    OnGetAllInfos(code, message, data);
                    BuildingCattery buildingCattery = BuildingManager.Instance.GetEntitie(id) as BuildingCattery;
                    if (buildingCattery)
                    {
                        buildingCattery.catMove();
                    }
                    //if(Map.Instance.CurBuildingData)
                    //    ((BuildingCattery)Map.Instance.CurBuildingData).RefurbishCats();
                }
                if (callback != null)
                {
                    callback(code, message, data);
                }
            });
        }

        public void Process(object data)
        {
            if (data is Msg.ClientMessage.S2CGetCatHouseInfoResult)
            {
                var originData = (Msg.ClientMessage.S2CGetCatHouseInfoResult)data;

                //Building.BuildingManager.GetEntitie(item.CatHouseId)
                var item = originData.House;
                if (item != null)
                {
                    var cattery = new CatteryInfo
                    {
                        buildingId = item.CatHouseId,
                        shopId = item.BuildingConfigId,

                        grade = item.Level,
                        coin = item.Gold,
                        catIds = item.CatIds.ToArray(),
                        upgradeRemainTime = item.NextLevelRemainSeconds,
                        isDone = item.IsDone,
                        ProduceGoldRemainSeconds = item.ProduceGoldRemainSeconds,
                    };
                    UpdataCatteryConfig(cattery);
                    _catteryInfos[cattery.buildingId] = cattery;
                    if (FunctionWindow.RefurbishFunView != null)
                        FunctionWindow.RefurbishFunView();
                }
            }
            if (data is Msg.ClientMessage.S2CGetCatHousesInfoResult)
            {
                var protoData = data;

                if (protoData is Msg.ClientMessage.S2CGetCatHousesInfoResult)
                {
                    var originData = (Msg.ClientMessage.S2CGetCatHousesInfoResult)protoData;
                    _catteryInfos.Clear();
                    foreach (var item in originData.Houses)
                    {
                        var cattery = new CatteryInfo
                        {
                            buildingId = item.CatHouseId,
                            shopId = item.BuildingConfigId,
                            grade = item.Level,
                            coin = item.Gold,
                            catIds = item.CatIds.ToArray(),
                            upgradeRemainTime = item.NextLevelRemainSeconds,
                            isDone = item.IsDone,
                            ProduceGoldRemainSeconds = item.ProduceGoldRemainSeconds,
                        };
                        UpdataCatteryConfig(cattery);
                        _catteryInfos.Add(cattery.buildingId, cattery);
                    }
                }
            }
        }

        private void OnGetAllInfos(int code, string message, object data)
        {
            var protoDatas = data as ArrayList;
            if (protoDatas != null)
            {
                for (int i = 0; i < protoDatas.Count; i++)
                {
                    var protoData = protoDatas[i];

                    if (protoData is Msg.ClientMessage.S2CGetCatHousesInfoResult)
                    {
                        var originData = (Msg.ClientMessage.S2CGetCatHousesInfoResult)protoData;
                        _catteryInfos.Clear();
                        foreach (var item in originData.Houses)
                        {
                            var cattery = new CatteryInfo
                            {
                                buildingId = item.CatHouseId,
                                shopId = item.BuildingConfigId,
                                grade = item.Level,
                                coin = item.Gold,
                                catIds = item.CatIds.ToArray(),
                                upgradeRemainTime = item.NextLevelRemainSeconds,
                                isDone = item.IsDone,
                                ProduceGoldRemainSeconds = item.ProduceGoldRemainSeconds,
                            };
                            UpdataCatteryConfig(cattery);
                            _catteryInfos.Add(cattery.buildingId, cattery);
                        }
                    }
                }
            }
        }

        private void Load(Hashtable table)
        {
            var catHouseList = table.GetArrayList("CatHouse");
            if (catHouseList != null && catHouseList.Count > 0)
            {
                _catteryConfigs.Clear();

                var tmpT = new Hashtable();
                var tmpL0 = (ArrayList)catHouseList[0];
                for (int i = 1; i < catHouseList.Count; i++)
                {
                    var tmpLi = (ArrayList)catHouseList[i];
                    for (int j = 0; j < tmpL0.Count; j++)
                    {
                        tmpT[tmpL0[j]] = tmpLi[j];
                    }

                    var catteryConfig = new CatteryConfig
                    {
                        id = tmpT.GetInt("Id"),
                        buildingId = tmpT.GetInt("BuildingId"),
                        grade = tmpT.GetInt("Level"),
                        unlockStar = tmpT.GetInt("UnlockStar"),
                        unlockGrade = tmpT.GetInt("UnlockLv"),
                        catStorage = tmpT.GetInt("CatStorage"),
                        coinStorage = tmpT.GetInt("CoinStorage"),
                        upgradeCost = tmpT.GetInt("Cost"),
                        upgradeTime = tmpT.GetInt("Time"),
                        catColor = tmpT.GetInt("Color"),
                        salePrice = tmpT.GetInt("SalePrice"),
                        showCoinValue = tmpT.GetInt("IconShowCoin"),
                        idleAnim = tmpT.GetString("idle"),
                        touchAnim = tmpT.GetString("touch"),

                    };

                    _catteryConfigs.Add(catteryConfig.id, catteryConfig);
                }
            }
        }

        #endregion

        #region Unity

        public static KCattery Instance;

        private void Awake()
        {
            Instance = this;
        }

        public override void Load()
        {
            TextAsset textAsset;
            KAssetManager.Instance.TryGetExcelAsset("CatHouse", out textAsset);
            if (textAsset)
            {
                var table = textAsset.bytes.ToJsonTable();
                if (table != null)
                {
                    Load(table);
                }
            }
        }

        #endregion
    }
}
