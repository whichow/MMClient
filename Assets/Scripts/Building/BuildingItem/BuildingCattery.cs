// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "BuildingCatHome" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using Game.DataModel;
using Game.UI;
using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Build
{
    /// <summary>
    /// 猫舍
    /// </summary>
    public class BuildingCattery : BuildingFunction, ICatFunction
    {
        #region Field

        /// <summary>
        /// 
        /// </summary>
        private int _currGrade;
        /// <summary>
        /// 
        /// </summary>
        private int _maxGrade;

        /// <summary>
        /// 
        /// </summary>
        private float _currBuildTime;
        /// <summary>
        /// 
        /// </summary>
        private float _maxBuildTime;

        /// <summary>
        /// 
        /// </summary>
        private BubbleTimeProgress _timeProgress;
        /// <summary>
        /// 
        /// </summary>
        private int _showCoinValue;
        /// <summary>
        /// 标记是否在idle状态
        /// </summary>
        private bool isIdileState;
        /// <summary>
        /// 
        /// </summary>
        BuildingGoldBubble _entityStataVeiw;

        //private Dictionary<KCat, GameObject> catModels;

        BuildingOfCat _buildingOfCat;

        MainWindow _mainWindow;

        //private Sprite _sprite;
        //private Sprite _Sprite
        //{
        //    get
        //    {
        //        if (!_sprite)
        //        {
        //            KItem kItem = KItemManager.Instance.GetItem(ItemIDConst.Gold);
        //            _sprite = KIconManager.Instance.GetItemIcon(kItem.iconName);
        //        }
        //        return _sprite;
        //    }
        //}
        #endregion
        //public BuildingCattery()
        //{
        //    //_buildingOfCat = new BuildingOfCat(this);
        //}
        #region Property
        /// <summary>
        /// 是否建造完成
        /// </summary>
        public bool buildCompleted
        {
            get { return CurrBuildTime >= _maxBuildTime; }
        }


        /// <summary>
        /// 
        /// </summary>
        public int speedUpMoneyCost
        {
            get
            {
                //1f - CurrBuildTime / MaxBuildTime
                return speedUpMoneyCostGet((int)(MaxBuildTime - CurrBuildTime));
                //return Mathf.CeilToInt((_maxBuildTime - CurrBuildTime) / 300f);
            }
        }


        public int speedUpMoneyType
        {
            get { return 2; }
        }


        private int CurrGrade
        {
            get
            {
                if (CatteryInfoData != null)
                    return CatteryInfoData.grade;
                else
                    return 0;
            }
            //set
            //{
            //    _currGrade = value;
            //}
        }
        /// <summary>
        /// 
        /// </summary>
        private int MaxGrade
        {
            get
            {
                if (entityData != null)
                    return entityData.maxGrade;
                else
                    return -1;
            }
            //set
            //{
            //    _maxGrade = value;
            //}
        }
        public bool IsGradeMax
        {
            get
            {
                return CurrGrade >= MaxGrade;
            }
        }
        public int ShowCoinValue
        {
            get
            {
                //return 10;
                if (CatteryInfoData != null)
                    return CatteryInfoData.showCoinValue;
                else
                    return 0;
            }
            //set
            //{
            //    _showCoinValue = value;
            //}
        }

        private float MaxBuildTime
        {
            get
            {
                if (CatteryInfoData != null)
                {
                    return CatteryInfoData.nextUpgradeTime;
                }
                else
                    return -1;
            }
            //    set
            //{
            //    _maxBuildTime = value;
            //}
        }
        /// <summary>
        /// 
        /// </summary>
        private float CurrBuildTime
        {
            get
            {
                return _currBuildTime;
                //if (CatteryInfoData != null)
                //    return MaxBuildTime - CatteryInfoData.upgradeRemainTime;
                //else
                //    return -0.000001f;
            }
            set
            {
                _currBuildTime = value;
            }
        }

        #endregion

        #region IFunction

        public override string title
        {
            get
            {
                return entityData.itemName + string.Format(KLocalization.GetLocalString(52112), CatteryInfoData != null? CatteryInfoData.grade<=0?1: CatteryInfoData.grade:0);
            }
        }
        public override bool supportSpeedUp
        {
            get
            {
                return curState == State.kBuild && !_isDone;
            }
        }
        /// <summary>
        /// 是否可以升级  由是否达到最高等级、是否在升级状态、升级完成后是否已经点击过  共同决定
        /// </summary>
        public override bool supportGradeUp
        {
            get
            {
                return CurrGrade < MaxGrade && !supportSpeedUp && _isDone;// _currBuildTime < 0f;
            }
        }
        public int CatGrade
        {
            get
            {
                return CatteryInfoData.grade;
            }
        }
        /// <summary>
        /// 猫槽总数
        /// </summary>
        public int CatStorage
        {
            get
            {
                if (CatteryInfoData != null)
                    return CatteryInfoData.catStorage;
                else
                    return 0;
            }
        }
        /// <summary>
        /// 1 已经按下 ，0 没有按下
        /// </summary>
        private bool _isDone
        {
            get
            {
                if (CatteryInfoData != null)
                    return CatteryInfoData.isDone == 1;
                else
                    return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>

        public KCat[] CatLst
        {
            get
            {
                //Game.KCattery.CatteryInfo catteryInfo = KCattery.Instance.GetCatteryInfo(buildingId);
                if (CatteryInfoData != null)
                {
                    KCat[] CatLst = new KCat[CatteryInfoData.catIds.Length];
                    for (int i = 0; i < CatteryInfoData.catIds.Length; i++)
                    {
                        CatLst[i] = KCatManager.Instance.GetCat(CatteryInfoData.catIds[i]);
                    }
                    return CatLst;
                }
                else
                    return new KCat[0];
            }
        }
        public int Coin
        {
            get
            {
                if (CatteryInfoData != null)
                    return CatteryInfoData.coin;
                else
                    return 0;
            }
        }
        public int CoinMax
        {
            get
            {
                //return 10;
                if (CatteryInfoData != null)
                    return CatteryInfoData.coinStorage;
                else
                    return -1;
            }
        }
        private KCattery.CatteryInfo _catteryInfoData;
        public KCattery.CatteryInfo CatteryInfoData
        {
            get
            {
                //if(catteryInfoData !=null)
                return KCattery.Instance.GetCatteryInfo(buildingId);
            }
        }

        /// <summary>
        /// 产金剩余时间   -1表示没有产金  >=0表示产金剩余时间
        /// </summary>
        public int ProduceGoldRemainSeconds
        {
            get { return this.CatteryInfoData.ProduceGoldRemainSeconds; }
        }

        public override bool SpeedUpInfo(ref int moneyCost, ref int moneyType)
        {
            moneyCost = speedUpMoneyCost;
            moneyType = speedUpMoneyType;
            return true;
        }

        public override bool OnSpeedUp()
        {
            SpeedUp();
            return true;
        }

        /// <summary>
        /// 添加猫
        /// </summary>
        public void AddCat()
        {
            var cattery = KCattery.Instance.GetCatteryInfo(buildingId);
            var catList = CatDataModel.Instance.GetCatsByColor(cattery.catColor);
            catList.Sort(CatDataModel.Instance.CionSortItem);
            KUIWindow.CloseWindow<FunctionWindow>();
            KUIWindow.OpenWindow<ChooseCatWindow>(
                new ChooseCatWindow.Data()
                {
                    idx = 0,
                    type = 1,
                    catsList = catList,
                    onConfirm = (kCat, id) =>
                    {
                        KCattery.Instance.AddCat(buildingId, kCat.mCatInfo.Id, (codeId, content, data) =>
                         {
                             if (codeId == 0)
                             {
                                 //FunctionWindow.RefurbishFunView();
                                 _buildingOfCat.refurbishCatModel();
                                 playCionAnim();
                                 Debug.Log("添加成功");
                             }
                             else
                             {
                                 Debug.Log("添加失败");
                             }
                         });

                    },
                    onCancel = () => { Debug.Log("添加猫取消"); },
                }
               );
            Debuger.Log("添加猫");
        }

        /// <summary>
        /// 获取猫的图标
        /// </summary>
        /// <param name="catId"></param>
        /// <returns></returns>
        public int CatIconGet(int catId)
        {
            // Game.KCattery.CatteryInfo catteryInfo = KCattery.Instance.GetCatteryInfo(buildingId);
            if (CatteryInfoData != null)
                return CatteryInfoData.catStorage;
            else
                return -1;
        }
        public void CatHouseUpGrade()
        {
            KUIWindow.OpenWindow<UpGradeBuildWindow>(new UpGradeBuildWindow.Data()
            {
                BuildingId = buildingId,
                Model = entityView.GetModel(),
                CallBack = code =>
                {
                    DataRefurbish();
                    //RefurbishCats();
                    // Upgrade();
                    //FunctionWindow.RefurbishFunListData();
                },
            });
            Debug.Log("猫舍升级ID" + buildingId);
        }
        int coinTemp;

        /// <summary>
        /// 金币收集开始
        /// </summary>
        public void CatCollectStart()
        {
            //if (CatLst.Length == 0)
            //{
            //    //没有猫
            //    UI.MessageBox.ShowMessage("提示", KLanguageManager.Instance.GetLocalString(57132));
            //    return;
            //}
            if (PlayerDataModel.Instance.mPlayerData.Vigour < 1)
            {
                //行动力不足
                UI.MessageBox.ShowMessage("提示", KLanguageManager.Instance.GetLocalString(57132));
                return;
            }

            GameApp.Instance.GameServer.CatHouseProduceGoldRequest(buildingId);
            Debuger.Log("猫舍收集开始");
        }

        /// <summary>
        /// 金币收集终止
        /// </summary>
        public void CatCollectStop()
        {
            string msg = KLanguageManager.Instance.GetLocalString(53335);
            msg = string.Format(msg, Coin);
            UI.MessageBox.ShowMessage("提示", msg, () =>
            {
                KCattery.Instance.Harvest(buildingId, CatCollectBack);
                Debuger.Log("猫舍收集终止");
            }, () => { });
        }
        /// <summary>
        /// 金币收集
        /// </summary>
        public void CatCollect()
        {
            Debuger.Log("猫舍产金剩余时间:" + ProduceGoldRemainSeconds);
            if (ProduceGoldRemainSeconds > 0)
                return;
            //if (Coin < 1)
            //    return;
            //coinTemp = Coin;
            KCattery.Instance.Harvest(buildingId, CatCollectBack);
            Debuger.Log("猫舍收集");
        }
        private void CatCollectBack(int id, string content, object data)
        {
            if (id == 0)
            {
                if (FunctionWindow.RefurbishFunView != null)
                    FunctionWindow.RefurbishFunView();
                //IconFlyMgr.Instance.IconFlyStart(this.transform);
                //IconFlyMgr.Instance.IconFlyStart(entityView.gmPoint.position, mainWindow.goldNode.position);
                //IconFlyMgr.Instance.IconGroupFlyStart(entityView.gmPoint.position, mainWindow.goldNode.position, 0.1f,1 /*Coin > 10 ? 10: Coin*/);

                S2CCatHouseGetGoldResult[] s2CCatHouseGetGoldResults;
                BuildingManager.Instance.DataParse<S2CCatHouseGetGoldResult>(data, out s2CCatHouseGetGoldResults);

                Debug.Log(Coin.ToString());
                bubbleHide();
                if (s2CCatHouseGetGoldResults != null && s2CCatHouseGetGoldResults.Length > 0)
                {
                    Debug.Log("金币数"+s2CCatHouseGetGoldResults[0].Gold);
                    IconFlyMgr.Instance.IconPathGroupStart(
                        entityView.gmPoint.position,
                        (int)KItem.GiftType.kGold,
                        0.1f,
                        IconFlyMgr.GiftType.KNone,
                        s2CCatHouseGetGoldResults[0].Gold > 5 ? 5 : s2CCatHouseGetGoldResults[0].Gold
                    );
                    IconFlyMgr.Instance.IconFlyStart(
                        entityView.gmPoint.position,
                        new Vector3(0,100f,0),
                        "+"+s2CCatHouseGetGoldResults[0].Gold.ToString()
                        );

                    //IconFlyMgr.Instance.IconFlyStart(this.entityView.gmPoint.position);
                    //IconFlyMgr.Instance.IconPathGroupStart(this.transform.position, (int)KItem.GiftType.kGold, 0.1f, IconFlyMgr.GiftType.KNone, 10);


                    //IconFlyMgr.Instance.IconPathStart(entityView.gmPoint.position,
                    //    _mainWindow.goldNode.position, "+" + s2CCatHouseGetGoldResults[0].Gold.ToString(),
                    //    new Vector2(0, 0),
                    //    MainWindow.ScaleAnimType.kGoldNode,
                    //    _Sprite,
                    //    true);.

                    Debug.Log("收集成功");
                }
            }
            else
            {
                Debug.Log("收集失败");
            }
        }
        public void CatInfoShow(int catId)
        {
            CatDataVO catDataVO = CatDataModel.Instance.GetCatDataVOById(catId);
            CatDatas catDatas = new CatDatas();
            catDatas.mCatDataVOs = new List<CatDataVO>();
            catDatas.mCatDataVOs.Add(catDataVO);
            catDatas.mIndex = 0;
            catDatas.mOpenType = CatOpenType.Normal;
            KUIWindow.OpenWindow<CatInfoWindow>(catDatas);

            //var dat = new KCat[1] { KCatManager.Instance.GetCat(catId) };
            //KUIWindow.CloseWindow<FunctionWindow>();
            //Debug.Log("显示猫信息" + catId);
            //KCat[] t = CatLst;
            //KUIWindow.OpenWindow<CatInfoWindow>(windowData: new CatInfoWindow.WindowData
            //{
            //    catIndex = 0,
            //    catArray = new KCat[1] { KCatManager.Instance.GetCat(catId) },
            //});
        }

        public override void OnSell()
        {
            this.isMove = false;
            KCattery.CatteryInfo catteryInfo = KCattery.Instance.GetCatteryInfo(this.buildingId);
            KUIWindow.OpenWindow<MessageBox>(
               new MessageBox.Data
               {
                   content = string.Format(KLocalization.GetLocalString(52016),
                   catteryInfo != null ? catteryInfo.salePrice.ToString() : string.Empty,
                   XTable.ItemXTable.GetByID(ItemIDConst.Gold).Name),
                   onConfirm = onSellCallBack,
                   onCancel = () => { this.isMove = true; },

               }
                );
        }
        public void onSellCallBack()
        {

            BubbleManager.Instance.HideConfirm();
            CollisionHighlight.Instance.HideCollisions();

            Debuger.Log("猫舍出售"+ buildingId);
            KUser.BuildingSell(buildingId, BuildingSellBack);
        }
        private void BuildingSellBack(int code, string content, object data)
        {
            MainWindow mainWindow = _MainWindow as MainWindow;
            if (code == 0)
            {
                Debuger.Log("猫舍出售成功");
                bubbleHide();
                IconFlyMgr.Instance.IconPathStart(this.transform.position,
                    mainWindow.goldNode.position,
                    (KCattery.Instance.GetCatteryInfo(this.buildingId).salePrice + Coin).ToString(),
                    new Vector2(0, 0),
                    MainWindow.ScaleAnimType.kGoldNode,
                    KIconManager.Instance.GetItemIcon(XTable.ItemXTable.GetByID(ItemIDConst.Gold).Icon),
                    true
                    );
                Destroy(Map.Instance.CurrMapObject);
                BuildingManager.Instance.DestroyEntity(buildingId);
                BubbleManager.Instance.HideConfirm();
                CollisionHighlight.Instance.HideCollisions();
            }
            else
            {
                Debuger.Log("出售失败");
            }
        }


        //public void OnRotate()
        //{
        //    if (IsRotate)
        //    {
        //        isRotating = true;
        //        entityView.RotateModel();
        //        MapObject mapObject = GetComponent<MapObject>();
        //        CollisionHighlight.Instance.ShowCollisions(mapObject, null);

        //        //Transform rotate = Map.Instance.currMapObject.transform;
        //        // rotate.localRotation = rotate.localRotation + new Vector3(); new Quaternion();
        //        // Map.Instance.currMapObject.transform.Rotate(rotate.localRotation);
        //    }
        //}

        #endregion

        #region Method

        //private bool istouch
        //{
        //    set
        //    {
        //        if (value)
        //        {
        //            entityView.TouchModel(false);
        //        }
        //        else
        //        {
        //            entityView.TouchModel(true);
        //        }

        //    }
        //}
        public override string idleAnimation
        {
            get
            {
                KCattery.CatteryInfo catteryInfo = CatteryInfoData;
                if (catteryInfo != null)
                {
                    return catteryInfo.idleAnim;
                }
                return "idle1";
            }
        }

        public override string touchAnimation
        {
            get
            {
                KCattery.CatteryInfo catteryInfo = CatteryInfoData;
                if (catteryInfo != null)
                {
                    return catteryInfo.touchAnim;
                }
                return "";
            }
        }
        public override void RefurbishData()
        {
            base.RefurbishData();
            //DataRefurbish();

        }
        /// <summary>
        /// 建筑物建造或者成功 向服务器发送确认请求
        /// </summary>
        private void buildingConfirm()
        {
            KCattery.Instance.Complete(buildingId, callBackuildingConfirm);
        }
        /// <summary>
        /// 建筑物建造或者成功 向服务器发送确认请求 回调
        /// </summary>
        /// <param name="codeID"></param>
        /// <param name="content"></param>
        /// <param name="data"></param>
        private void callBackuildingConfirm(int codeID, string content, object data)
        {
            if (codeID == 0)
            {

                //entityView.ShowTent(false);
                //StateChange(State.kIdle);
                DataRefurbish();

                //RefurbishCats();
                //entityView.ShowModel();

                if (FunctionWindow.RefurbishFunView != null)
                    FunctionWindow.RefurbishFunView();
            }
            else
            {
                Debug.Log("发送猫舍成功失败");
            }
        }

        public void catMove()
        {
            playCionAnim();
            RefurbishCats();
        }

        /// <summary>
        /// 刷新猫模型 
        /// </summary>
        public void RefurbishCats()
        {
            if (/*!supportSpeedUp*/curState == State.kProduce)  //KToProduce状态下  会直接切换到kProduce
                _buildingOfCat.refurbishCatModel();
            else
                _buildingOfCat.HideCats();
        }


        /// <summary>
        /// 城建元素获取焦点事件
        /// </summary>
        protected override void OnTap()
        {
            //if(_isBubbleShow)
           // if (_isDone)
                base.OnTap();
            //BuildingManager.Instance.CloseMainWindowMenu();

            //Debug.Log("获得焦点");
            //GameCamera.Instance.Show(this.entityView.centerNode.position);
            if (isOneSelf)
            {
                if (curState == State.kIdle)
                {
                    buildingConfirm();
                    return;
                }
                if (CatteryInfoData != null)
                {
                    if (CatteryInfoData.coin > ShowCoinValue)
                    {
                        CatCollect();
                    }
                }
            }
            //是否显示建造进度条
            if (curState == State.kBuild /*&& !buildCompleted/* && !IsServerBuilding*/)
            {
                _timeProgress = BubbleManager.Instance.ShowTimeProgress(this.entityView.progressNode) as BubbleTimeProgress;
            }
            else
            {
                BubbleManager.Instance.HideTimeProgress();
                _timeProgress = null;
            }
        }
        /// <summary>
        /// 获取焦点事件
        /// </summary>
        /// <param name="focus"></param>
        protected override void OnFocus(bool focus)
        {
            base.OnFocus(focus);

            if (focus)
            {
                //if (CatteryInfoData.coin > ShowCoinValue)
                //{
                //    bubbleHide();
                //}
                //istouch = true;
                //Debug.Log("获得焦点");
            }
            else
            {
                //if (Coin > ShowCoinValue)
                //{
                //    bubbleshow();
                //}
                //istouch = false;
                //Debug.Log("失去焦点");
            }


        }

        protected override void OnStateEnter(State state)
        {
            base.OnStateEnter(state);

            if (entityView == null)
                return;
            switch (state)
            {
                case State.kBuild:
                    {
                        entityView.ShowBuild();
                        break;
                    }
                case State.kIdle:
                    {
                        entityView.ShowTent(true);
                        break;
                    }
                case State.KToProduce:
                    {
                        entityView.ShowTent(false);

                        entityView.ShowModel();
                        StateCtrl(State.kProduce);


                        break;
                    }
                case State.kProduce:
                    {
                        //entityView.ModelHeight();
                        entityView.ShowModel();
                        break;
                    }
                default: break;
            }


        }

        protected override void OnEntityDataChanged()
        {
            base.OnEntityDataChanged();
        }
        /// <summary>
        /// 刷新数据
        /// </summary>
        public void DataRefurbish()
        {
            if (!isOneSelf)
                return;
            StateCtrl();
            RefurbishCats();
            playCionAnim();
            ////MaxBuildTime = entityData.buildTime;
            //if (entityView != null)
            //    Upgrade();
            if (FunctionWindow.RefurbishFunView != null)
                FunctionWindow.RefurbishFunView();


        }
        /// <summary>
        /// 播放工作状态动画 ,暂时停止使用 后期处理
        /// </summary>
        private void playCionAnim()
        {
            return;
            if (CatteryInfoData.catIds.Length > 0)
            {
                entityView.PlayAnimation(touchAnimation, true);
                IsToggleIdle = false;
            }
            else
                IsToggleIdle = true;
        }
        /// <summary>
        /// 设置猫舍状态
        /// </summary>
        /// <param name="isSpecial">是否 特例设置状态</param>
        private void StateCtrl(State isSpecialStateSet = State.kNone)
        {
            if (isSpecialStateSet != State.kNone)
            {
                curState = isSpecialStateSet;
                return;
            }
            if (CatteryInfoData != null)
            {
                if (CatteryInfoData.upgradeRemainTime > 0)
                {
                    this.curState = State.kBuild;
                    CurrBuildTime = MaxBuildTime - CatteryInfoData.upgradeRemainTime;
                }
                else
                {
                    CurrBuildTime = -0.000001f;
                    if (!_isDone)
                    {
                        curState = State.kIdle;
                        isIdileState = true;
                    }
                    else if (isIdileState)
                    {
                        curState = State.KToProduce;   //idle 切换模型状态
                        isIdileState = false;
                    }
                    else
                        curState = State.kProduce;
                }
            }

            else
            {
                CurrBuildTime = -0.000001f;
                curState = State.kProduce;
                Debug.LogError("猫舍信息数据获取失败:" + buildingId);

            }
            StateChange(curState);

        }
        /// <summary>
        /// 
        /// </summary>
        private void Upgrade()
        {

            if (curState == State.kIdle)
            {
                curState = State.kIdle;
                entityView.ShowTent(true);
                return;
            }


            if (curState == State.kBuild)
            {
                entityView.ShowBuild();
            }

            if (curState == State.kProduce)
            {
                entityView.ShowModel();
            }
            //istouch = false;        //动画播放刷新
        }

        /// <summary> 标记猫舍金币是否显示</summary>
        bool _isBubbleShow;
        GameObject _bubblePoint;
        private void bubbleshow()
        {
            if (_entityStataVeiw == null && supportGradeUp)
            {
                //Debug.Log("金币" + ShowCoinValue);
                _entityStataVeiw = BuildingStateMgr.Instance.BubbleShow(this, onClickBubble);
                _entityStataVeiw.IconSet(XTable.ItemXTable.GetByID(ItemIDConst.Gold).Icon);

                _entityStataVeiw.GoldIconSet((int)KItem.MoneyType.kCoin);
                _isBubbleShow = true;
                if (entityView.gmPoint)
                    _bubblePoint = entityView.gmPoint.gameObject;

            }
        }
        private void bubbleHide()
        {
            if (_isBubbleShow)
            {
                Debug.Log("隐藏金币" + ShowCoinValue);
                _entityStataVeiw = null;
                BuildingStateMgr.Instance.BubbleHide(this);
                _isBubbleShow = false;
            }
        }
        /// <summary>
        /// 猫舍气泡点击收集金币
        /// </summary>
        private void onClickBubble()
        {
            CatCollect();
        }
        /// <summary> 标记是否已经第一次初始化</summary>
        bool isFirst;

        private void UpdateState()
        {
            if (!isOneSelf)
                return;
            if (!isFirst)
            {
                if (CatteryInfoData != null)
                {
                    isFirst = true;
                    DataRefurbish();
                }
            }
            if (this.curState == State.kBuild /*&& !IsServerBuilding*/)
            {
                if (CurrBuildTime < MaxBuildTime)
                {
                    CurrBuildTime += Time.deltaTime;
                    if (CurrBuildTime >= MaxBuildTime - 1)               //建造完成
                    {
                        //StateCtrl();
                        isIdileState = true;
                        curState = State.kIdle;
                        StateChange(curState);
                        //entityView.ShowTent(true);
                        BubbleManager.Instance.HideTimeProgress();
                        _timeProgress = null;
                    }

                    if (_timeProgress != null)
                    {
                        _timeProgress.porgress = CurrBuildTime / MaxBuildTime;
                        _timeProgress.time = K.Extension.TimeExtension.ToTimeString((int)(MaxBuildTime - CurrBuildTime));
                    }
                }
                else
                {
                    curState = State.kIdle;
                }
            }
            if (!_isBubbleShow)
            {
                if (CatteryInfoData != null)
                {
                    //if (Coin > ShowCoinValue)
                    if (this.CatteryInfoData.ProduceGoldRemainSeconds == 0)
                    {
                        bubbleshow();
                    }
                    else
                    {
                        bubbleHide();
                    }
                }
            }
        }

        private void SpeedUp()
        {
            BubbleManager.Instance.HideTimeProgress();
            KCattery.Instance.SpeedUp(buildingId, (codeId, concent, data) =>
            {
                if (codeId == 0)
                {
                    DataRefurbish();
                    KUIWindow.CloseWindow<FunctionWindow>();

                    Debug.Log("加速成功");
                }
                else
                {
                    Debug.Log("加速失败");
                }
            });
            //UpdateState();
        }

        #endregion

        #region Unity 

        // Use this for initialization
        private void Start()
        {
            entityView.ShowModel();
            if (!isOneSelf)
                _buildingOfCat.refurbishCatModel();

            _mainWindow = KUIWindow.GetWindow<MainWindow>();
            //RefurbishCats();
        }

        private void Update()
        {
            UpdateState();

        }
        private void Awake()
        {
            _buildingOfCat = new BuildingOfCat(this);
            // catModels = new Dictionary<KCat, GameObject>();
        }
        private void LateUpdate()
        {
            //if (_entityStataVeiw !=null && _bubblePoint)
            //{
            //    _entityStataVeiw.BubblePosSet( BuildingStateMgr.Instance.WorldToUI(_bubblePoint.transform.position));
            //}

        }

        #endregion
    }
}

