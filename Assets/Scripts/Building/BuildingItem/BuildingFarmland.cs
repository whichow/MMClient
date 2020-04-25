// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "BuildingFarmland" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using Game.UI;
using Spine.Unity;
using UnityEngine;

namespace Game.Build
{
    /// <summary>
    /// 农田
    /// </summary>
    public class BuildingFarmland : BuildingFunction
    {
        #region Field

        private float _deltaAdd = 1f;

        private string _currAnim;
        private bool _processing;
        private Sprite _sprite;
        private Dictionary<int, KFx> _kFxDict;
        private Sprite _Sprite
        {
            get
            {
                if(!_sprite)
                    _sprite = KIconManager.Instance.GetItemIcon("Icon_food_03");
                return _sprite;
            }

        }
        #endregion

        #region Property

        /// <summary>
        /// 作物成长剩余时间
        /// </summary>
        public float remainTime
        {
            get
            {
                var farmland = KFarm.Instance.GetFarmland(this.buildingId);
                if (farmland != null)
                {
                    return farmland.remainTime;
                }
                return 0f;
            }
        }

        /// <summary>
        /// 作物成长总共时间
        /// </summary>
        public float totalTime
        {
            get
            {
                var farmland = KFarm.Instance.GetFarmland(this.buildingId);
                if (farmland != null)
                {
                    return farmland.totalTime;
                }
                return 1f;
            }
        }
        public int uotPut
        {
            get
            {
                var farmland = KFarm.Instance.GetFarmland(this.buildingId);
                if (farmland != null)
                {
                    return farmland.outPut;
                }
                return -1;
            }

        }
        public int experionce
        {
            get
            {
                var farmland = KFarm.Instance.GetFarmland(this.buildingId);
                if (farmland != null)
                {
                    return farmland.experience;
                }
                return -1;
            }

        }
        public bool canSow
        {
            get
            {
                var farmland = KFarm.Instance.GetFarmland(this.buildingId);
                if (farmland != null && farmland.cropId > 0)
                {
                    return false;
                }
                return true;
            }
        }

        public bool canHavrest
        {
            get
            {
                var farmland = KFarm.Instance.GetFarmland(this.buildingId);
                if (farmland != null)
                {
                    if (farmland.cropId > 0 && farmland.remainTime < 0)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public KItemCrop currCrop
        {
            get
            {
                var farmland = KFarm.Instance.GetFarmland(buildingId);
                if (farmland != null && farmland.cropId != 0)
                {
                    return KItemManager.Instance.GetCrop(farmland.cropId);
                }
                return null;
            }

        }

        private KFarm.Farmland farmlandInfo
        {
            get
            {
                return KFarm.Instance.GetFarmland(this.buildingId);
            }
        }

        public override string idleAnimation
        {
            get { return "MB01_100"; }
        }
        public override string touchAnimation
        {
            get
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool canSpeedUp
        {
            get { return remainTime > 0; }
        }

        /// <summary>
        /// 显示进度条
        /// </summary>
        public float showProgress
        {
            get
            {
                return (totalTime-remainTime) / totalTime;
            }
        }

        /// <summary>
        /// 显示剩余时间
        /// </summary>
        public string showRemainTime
        {
            get
            {
                return K.Extension.TimeExtension.ToTimeString((int)(remainTime));
            }
        }
        private MainWindow _mainWindow;
        private MainWindow _MainWindow
        {
            get
            {
                if (_mainWindow == null)
                {
                    _mainWindow = KUIWindow.GetWindow<MainWindow>();
                }
                return _mainWindow;
            }
        }

        #endregion

        #region IFunction

        public override bool supportSpeedUp
        {
            get
            {
                var farmland = KFarm.Instance.GetFarmland(buildingId);
                return farmland != null && farmland.remainTime > 0;
            }
        }

        public override bool supportInfomation
        {
            get
            {
                return true;
            }
        }

        public override string title
        {
            get
            {
                return currCrop != null ? currCrop.itemName : this.entityData.itemName;
            }
        }

        public override bool SpeedUpInfo(ref int moneyCost, ref int moneyType)
        {
            moneyCost = speedUpMoneyCostGet((int)remainTime);
            moneyType = speedUpMoneyType;
            return true;
        }

        public override bool OnSpeedUp()
        {
            Debug.Log("OnSpeedUp");
            SpeedUp();
            return true;
        }

        #endregion

        #region Method

        /// <summary>
        /// 播种农田
        /// </summary>
        /// <param name="data"></param>
        public void Sowing(KItemCrop data)
        {
            if (_processing)
            {
                return;
            }
            var farmland = KFarm.Instance.GetFarmland(this.buildingId);
            if (farmland != null && farmland.cropId > 0)
            {
                return;
            }
            _processing = true;
            //Debug.Log("农田Id："+ buildingId +"农田Id："+data.itemID);
            KFarm.Instance.Sowing(buildingId, data.itemID, OnSowing);
        }

        public void Harvest()
        {
            if (_processing)
            {
                return;
            }
            Debuger.Log("Harvest");
            var farmland = KFarm.Instance.GetFarmland(this.buildingId);
            if (farmland != null)
            {
                if (farmland.cropId > 0 && farmland.remainTime < 0)
                {
                    _processing = true;
                    KFarm.Instance.Harvest(buildingId, OnHarvest);
                }
            }
        }

        public void SpeedUp()
        {

            KUIWindow.CloseWindow<FunctionWindow>();
            BubbleManager.Instance.HideTimeProgress();

            if (_processing)
            {
                return;
            }
            var farmland = KFarm.Instance.GetFarmland(this.buildingId);
            if (farmland != null)
            {
                if (farmland.cropId != 0 && farmland.remainTime > 0)
                {
                    _processing = true;
                    KFarm.Instance.SpeedUp(buildingId, OnSowing);
                }
            }
            RipeFx();
            //if (_currStep < 2)
            //{
            //    _currStep = 2;
            //    var anim = currCrop.animations[_currStep];
            //    this.entityView.PlayAnimation(anim);

            //    Mature();

            //    var allFarms = BuildingManager.Instance.GetEntities<BuildingFarmland>();
            //    if (allFarms != null && allFarms.Count > 0)
            //    {
            //        foreach (var item in allFarms)
            //        {
            //            if (item.canSpeedUp)
            //            {
            //                GameCamera.Instance.Show(item.bubbleNode.position);
            //                KUIWindow.OpenWindow<Game.UI.FunctionWindow>(item);
            //                var bubble = BubbleManager.Instance.ShowTimeProgress(item.bubbleNode) as BubbleTimeProgress;
            //                bubble.SetUpdateDelegate(item.OnUpdateProgress);
            //                return;
            //            }
            //        }
            //    }
            //    KUIWindow.CloseWindow<FunctionWindow>();
            //    BubbleManager.Instance.HideTimeProgress();
            //}
        }
        /// <summary>
        /// 播种 农田  回调
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        private void OnSowing(int code, string message, object data)
        {
            _processing = false;

            if (code == 0)
            {
                var farmland = KFarm.Instance.GetFarmland(buildingId);
                if (farmland != null)
                {
                    int step; string stepAnim;
                    if (farmland.GetStep(out step, out stepAnim))
                    {
                        PlayAnimation(stepAnim, false);
                    }
                }
                WaterFx();
            }
        }
        /// <summary>
        /// 农田收割完成回调
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        private void OnHarvest(int code, string message, object data)
        {
            _processing = false;
            if (code == 0)
            {
                PlayAnimation(idleAnimation, true);
                HarvestFx();
                RipeFxStop();
            }
        }
        /// <summary>
        /// 农田加速回调
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        private void OnSpeedUp(int code, string message, object data)
        {
            _processing = false;
            if (code == 0)
            {
                var farmland = KFarm.Instance.GetFarmland(buildingId);
                if (farmland != null)
                {
                    int step; string stepAnim;
                    if (farmland.GetStep(out step, out stepAnim))
                    {
                        PlayAnimation(stepAnim, false);
                    }
                }
                RipeFx();
            }
        }
        /// <summary>
        /// 播放成熟植物动画
        /// </summary>
        private void playMaturingPlantAnim()
        {
            var farmland = KFarm.Instance.GetFarmland(buildingId);
            if (farmland != null)
            {
                int step; string stepAnim;
                if (farmland.GetStep(out step, out stepAnim))
                {
                    PlayAnimation(stepAnim, false);
                }
            }

        }

        /// <summary>
        /// 浇水特效
        /// </summary>
        private void WaterFx()
        {
            GameObject prefab;
            if (KAssetManager.Instance.TryGetBuildingPrefab("Fx/WateringFx", out prefab))
            {
                KPool.Spawn(prefab, Vector3.zero, Vector3.zero, this.transform);
            }
        }
        private Int3[] gifts =new Int3[1];
        /// <summary>
        /// 收割特效
        /// </summary>
        private void HarvestFx()
        {
            GameObject prefab;
            if (KAssetManager.Instance.TryGetBuildingPrefab("Fx/HarvestFx", out prefab))
            {
                KPool.Spawn(prefab, Vector3.zero, Vector3.zero, this.transform);
            }
            //if (KAssetManager.Instance.TryGetBuildingPrefab("Fx/FarmDrop", out prefab))
            //{
            //    var obj = GameObject.Instantiate(prefab);
            //    obj.transform.SetParent(this.transform, false);
            //    obj.transform.localPosition = Vector3.zero;
            //    //GameObject.Destroy(obj, 2f);
            //}
            // KItem kItem = KItemManager.Instance.GetItem((int)KItem.MoneyType.kFood);
            //gifts[0] = new Int3((int)KItem.GiftType.kFood, (int)uotPut, (int)IconFlyMgr.GiftType.KNone);
            //gifts[1] = new Int3((int)KItem.GiftType.kExp, experionce, (int)IconFlyMgr.GiftType.KNone);
            gifts[0] = new Int3((int)KItem.GiftType.kExp, experionce, (int)IconFlyMgr.GiftType.KNone);
            IconFlyMgr.Instance.IconPathGroupStart(entityView.gmPoint.position,
                0.2f, gifts);
        }

        /// <summary>
        /// 成熟
        /// </summary>
        private void RipeFx()
        {
            GameObject prefab;
           // _kFxDict.Clear();
            if (KAssetManager.Instance.TryGetBuildingPrefab("Fx/RipeFx", out prefab))
            {

                var farmland = KFarm.Instance.GetFarmland(buildingId);
                if (farmland != null)
                {


                    KFx kFxTemp;
                    if (!_kFxDict.TryGetValue(buildingId, out kFxTemp))
                    {
                        GameObject ripeFx = KPool.Spawn(prefab, Vector3.zero, Vector3.zero, this.transform);
                        kFxTemp = ripeFx.GetComponent<KFx>();

                        _kFxDict.Add(buildingId, kFxTemp);
                    }
                    else
                    {
                        //_kFxDict[buildingId]=
                    }
                    kFxTemp.gameObject.SetActive(true);
                    kFxTemp.transform.position = entityView.gmPoint.position;
                    kFxTemp.fxDuration = 0;
                    GameObject ripeFxItem = kFxTemp.fxPrefab;
                    SkeletonAnimation skeletonAnimation = ripeFxItem.GetComponentInChildren<SkeletonAnimation>();
                    skeletonAnimation.loop = true;
                    //skeletonAnimation.AnimationName = skeletonAnimation.state.Data.SkeletonData.
                     //skeletonAnimation.Reset();

                }
               
            }
        }
        /// <summary>
        /// 成熟
        /// </summary>
        private void RipeFxStop()
        {
            var farmland = KFarm.Instance.GetFarmland(buildingId);
            if (farmland != null)
            {
                KFx kFxTemp;
                if (_kFxDict.TryGetValue(buildingId, out kFxTemp))
                {
                    kFxTemp.fxDuration = 0.01f;
                    kFxTemp.gameObject.SetActive(false);
                }
               
            }
        }

        protected override void OnTap()
        {
            //GameCamera.Instance.Show(this.entityView.centerNode.position);

            if (isOneSelf)
            {
                //GameCamera.Instance.Follow(this.transform);
                KUIWindow.CloseWindow<BuildingShopWindow>();
                BuildingManager.Instance.currBuildingFarmland = this;
                //BuildingFarmland f= BuildingManager.GetNextUpSleepFarmland();
                var farmland = KFarm.Instance.GetFarmland(buildingId);
                if (farmland != null && farmland.cropId > 0)
                {
                    if (farmland.currStep < 2)
                    {

                        var bubble = BubbleManager.Instance.ShowTimeProgress(this.bubbleNode) as BubbleTimeProgress;
                        bubble.SetUpdateDelegate(this.OnUpdateProgress);
                        base.OnTap();
                    }
                    else
                    {
                        BubbleManager.Instance.ShowHarvest(this.transform);
                    }
                }
                else
                {
                    KUIWindow.OpenWindow<CropWindow>();
                }
            }
        }

        protected override void OnFocus(bool focus)
        {
            base.OnFocus(focus);
            if (!focus)
            {
                if (isOneSelf)
                {
                    KUIWindow.CloseWindow<CropWindow>();
                    BuildingManager.Instance.currBuildingFarmland = null;
                    BubbleManager.Instance.HideTimeProgress();
                    BubbleManager.Instance.HideHarvest();
                }
            }
        }

        private void UpdateTime(float delta)
        {
            if (isOneSelf)
            {
                UpdateOneSelfFarmlandState();
            }
            else
            {
                UpdateOtherPlayerFarmland();
            }
        }

        /// <summary>
        /// 更新自家农田状态
        /// </summary>
        private void  UpdateOneSelfFarmlandState()
        {
            var farmland = KFarm.Instance.GetFarmland(buildingId);
            if (farmland != null)
            {
                int step; string stepAnim;
                if (farmland.GetStep(out step, out stepAnim))
                {
                    if (PlayAnimation(stepAnim, false) && step == 2)
                    {
                        RipeFx();
                        KUIWindow.CloseWindow<FunctionWindow>();
                        BubbleManager.Instance.HideTimeProgress();
                    }
                }
            }
            else
            {
                PlayAnimation(idleAnimation, true);
            }
        }


        private void UpdateOtherPlayerFarmland()
        {
            var farmland = KFarm.Instance.GetOtherFarmland(buildingId);
            if (farmland != null)
            {
                int step; string stepAnim;
                if (farmland.GetStep(out step, out stepAnim))
                {

                    PlayAnimation(stepAnim, false);
                    //if (PlayAnimation(stepAnim, false) && step == 2)
                    //{
                    //    //RipeFx();
                    //    //KUIWindow.CloseWindow<FunctionWindow>();
                    //    //BubbleManager.Instance.HideTimeProgress();
                    //}
                }
            }
            else
            {
                PlayAnimation(idleAnimation, true);
            }
        }

        private void OnUpdateProgress(out float progress, out string time)
        {
            progress = showProgress;
            time = showRemainTime;
        }

        private bool PlayAnimation(string animation, bool loop)
        {
            if (!string.IsNullOrEmpty(animation) && _currAnim != animation)
            {
                _currAnim = animation;
                if (this.entityView)
                {
                    this.entityView.PlayAnimation(animation, loop);
                }
                return true;
            }
            return false;
        }
        public int speedUpMoneyCost
        {
            get
            {
                return speedUpMoneyCostGet((int)remainTime);
            }
        }

        #endregion

        #region Unity

        private void OnEnable()
        {
            entityView.ShowModel();


        }
        private void Awake()
        {
            _kFxDict = new Dictionary<int, KFx>();
        }
        private void OnDisable()
        {
        }
        bool isOtherPlayerFarmInit =false;
        private void Update()
        {
            _deltaAdd += Time.deltaTime;
            if (_deltaAdd >= 0.1f)
            {
                UpdateTime(_deltaAdd);
                _deltaAdd = 0f;
            }
            if (!isOneSelf&& !isOtherPlayerFarmInit)
            {

                 KFarm.Instance.SetOtherPlayerFarm(buildingId,viewBuildingInfo.CropData);
                isOtherPlayerFarmInit = true;
            }
        }

        #endregion
    }
}
