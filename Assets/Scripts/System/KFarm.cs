// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 2017-10-28
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KFarm" company="moyu"></copyright>
// <summary></summary>
// ***********************************************************************
using Msg.ClientMessage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    using Callback = System.Action<int, string, object>;

    /// <summary>
    /// 农田数据
    /// </summary>
    public class KFarm : KGameModule
    {
        #region Struct

        public class Farmland
        {
            private int _cropId;
            private KItemCrop _crop;
            private int _remainTimestamp;
            private int _oldOutPut;
            public int buildingId;

            public int cropId
            {
                get { return _cropId; }
                set
                {
                    _cropId = value;
                    if (value > 0)
                    {

                        _crop = KItemManager.Instance.GetCrop(value);
                        _oldOutPut = _crop.output;
                    }
                    else
                    {

                        _crop = null;
                    }
                }
            }

            public int remainTime
            {
                get { return _remainTimestamp - KLaunch.Timestamp; }
                set { _remainTimestamp = KLaunch.Timestamp + value; }
            }

            public float totalTime
            {
                get { return _crop != null ? _crop.GetTotalTime() : 1f; }
            }

            public int currStep
            {
                get
                {
                    return _crop != null ? _crop.GetStep(remainTime) : -1;
                }
            }
            public int outPut
            {
                get
                {
                    return _crop != null ? _crop.output : _oldOutPut;
                }
            }
            public int experience
            {
                get; set;
            }
            public bool GetStep(out int step, out string anim)
            {
                if (_crop != null)
                {
                    return _crop.GetStep(remainTime, out step, out anim);
                }
                step = 0;
                anim = null;
                return true;
            }
        }

        #endregion

        #region Field

        private Dictionary<int, Farmland> _farmlands = new Dictionary<int, Farmland>();

        private Dictionary<int, Farmland> _otherFarmlands = new Dictionary<int, Farmland>();

        #endregion

        #region Property

        public List<Farmland> farmlands
        {
            get { return new List<Farmland>(_farmlands.Values); }
        }

        #endregion

        #region Method

        public Farmland GetFarmland(int buildingId)
        {
            Farmland farmland;
            _farmlands.TryGetValue(buildingId, out farmland);
            return farmland;
        }
        public bool isHarvest(int farmId)
        {
            foreach (var item in _farmlands)
            {
                if (item.Value.cropId == farmId)
                {
                    if (item.Value.cropId > 0 && item.Value.remainTime < 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool isSowing(int farmId)
        {
            foreach (var item in _farmlands)
            {
                if (item.Value.cropId == farmId)
                {
                    if (item.Value.cropId > 0 && item.Value.remainTime > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public void GetAllInfos(Callback callback)
        {
            KUser.FarmGets((code, message, data) =>
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
        /// 播种回调
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="cropId"></param>
        /// <param name="callback"></param>
        public void Sowing(int buildingId, int cropId, Callback callback)
        {
            Debug.Log("建筑Id：" + buildingId + "农田Id：" + cropId);
            KUser.FarmSowing(buildingId, cropId, (code, message, data) =>
            {
                if (code == 0)
                {
                    OnSowing(code, message, data);
                }
                if (callback != null)
                {
                    callback(code, message, data);
                }
            });
        }


        /// <summary>
        /// 状态更新
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="callback"></param>
        public void SpeedUp(int buildingId, Callback callback)
        {
            KUser.FarmSpeedUp(buildingId, (code, message, data) =>
            {
                if (code == 0)
                {
                    OnSpeedUp(code, message, data);
                }
                if (callback != null)
                {
                    callback(code, message, data);
                }
            });
        }

        /// <summary>
        /// 收割农田回调
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="callback"></param>
        public void Harvest(int buildingId, Callback callback)
        {
            KUser.FarmHarvest(buildingId, (code, message, data) =>
            {
                if (code == 0)
                {
                    OnHarvest(code, message, data);
                }
                if (callback != null)
                {
                    callback(code, message, data);
                }
            });
        }
        public void Process(object data)
        {

            var protoData = data;
            if (protoData is S2CGetCropsResult)
            {
                var originData = (S2CGetCropsResult)protoData;
                _farmlands.Clear();
                foreach (var item in originData.Crops)
                {
                    var farmland = new Farmland
                    {
                        buildingId = item.BuildingId,
                        cropId = item.CropId,
                        remainTime = item.RemainSeconds,
                        //experience =item.ex
                    };
                    _farmlands.Add(farmland.buildingId, farmland);
                }
            }
        }
        /// <summary>
        /// 获取所有农田信息回调
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        private void OnGetAllInfos(int code, string message, object data)
        {
            var protoDatas = data as ArrayList;
            if (protoDatas != null)
            {
                for (int i = 0; i < protoDatas.Count; i++)
                {
                    var protoData = protoDatas[i];

                    if (protoData is S2CGetCropsResult)
                    {
                        var originData = (S2CGetCropsResult)protoData;
                        _farmlands.Clear();
                        foreach (var item in originData.Crops)
                        {
                            var farmland = new Farmland
                            {
                                buildingId = item.BuildingId,
                                cropId = item.CropId,
                                remainTime = item.RemainSeconds,
                            };
                            _farmlands.Add(farmland.buildingId, farmland);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 播种农田 回调事件处理
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        private void OnSowing(int code, string message, object data)
        {
            var protoDatas = data as ArrayList;
            if (protoDatas != null)
            {
                for (int i = 0; i < protoDatas.Count; i++)
                {
                    var protoData = protoDatas[i];

                    if (protoData is S2CPlantCropResult)
                    {
                        var originData = (S2CPlantCropResult)protoData;
                        var farmland = GetFarmland(originData.DestBuildingId);
                        if (farmland != null)
                        {
                            farmland.cropId = originData.CropId;
                            farmland.remainTime = originData.RemainSeconds;
                        }
                        else
                        {
                            farmland = new Farmland
                            {
                                buildingId = originData.DestBuildingId,
                                cropId = originData.CropId,
                                remainTime = originData.RemainSeconds,
                            };
                            _farmlands.Add(farmland.buildingId, farmland);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 加速 回调 事件处理
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        private void OnSpeedUp(int code, string message, object data)
        {
            var protoDatas = data as ArrayList;
            if (protoDatas != null)
            {
                for (int i = 0; i < protoDatas.Count; i++)
                {
                    var protoData = protoDatas[i];

                    if (protoData is S2CCropSpeedupResult)
                    {
                        var originData = (S2CCropSpeedupResult)protoData;
                        var farmland = GetFarmland(originData.FarmBuildingId);
                        farmland.cropId = originData.CropId;
                        farmland.remainTime = 0;
                    }
                }
            }
        }
        /// <summary>
        /// 收割农田回调 事件 处理
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        private void OnHarvest(int code, string message, object data)
        {
            var protoDatas = data as ArrayList;
            if (protoDatas != null)
            {
                for (int i = 0; i < protoDatas.Count; i++)
                {
                    var protoData = protoDatas[i];

                    if (protoData is S2CHarvestCropResult)
                    {
                        var originData = (S2CHarvestCropResult)protoData;
                        var farmland = GetFarmland(originData.FarmBuildingId);
                        farmland.cropId = 0;
                        farmland.remainTime = 0;
                        farmland.experience = originData.AddExp;
                    }
                }
            }
        }



        public void SetOtherPlayerFarm(int buildingId, CropInfo CropInfos)
        {

            if (CropInfos == null)
                return;
            CropInfos.BuildingId = buildingId;
            var farmland = new Farmland
            {
                buildingId = CropInfos.BuildingId,
                cropId = CropInfos.CropId,
                remainTime = CropInfos.RemainSeconds,
            };
            if (!_otherFarmlands.ContainsKey(CropInfos.BuildingId))
                _otherFarmlands.Add(farmland.buildingId, farmland);
            //var farmland = getOtherFarmland(CropInfos[i].BuildingId);
            //farmland.cropId = 0;
            //farmland.remainTime = 0;

        }
        public Farmland GetOtherFarmland(int building)
        {
            Farmland kFarm;
            if (_otherFarmlands.TryGetValue(building, out kFarm))
            {
                return kFarm;
            }
            return null;
            // _otherFarmlands
        }

        #endregion

        #region Unity   

        public static KFarm Instance;

        private void Awake()
        {
            Instance = this;
        }

        #endregion
    }
}
