using Game.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Build
{
    using Callback = System.Action<int, string, object>;
    /// <summary>
    /// 手工作坊
    /// </summary>
    class BuildingManualWorkShop : BuildingFunction, IManualWorkShopFunction
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
        private bool _isUseFunctionView;
        /// <summary>
        /// 
        /// </summary>
        private int _remainTime;
        //private KItemBuilding[] _buildingMakeData;
        ///// <summary>
        ///// 
        ///// </summary>
        //private float _currBuildTime;
        ///// <summary>
        ///// 
        ///// </summary>
        //private float _maxBuildTime;

        ///// <summary>
        ///// 
        ///// </summary>
        //private BubbleTimeProgress _timeProgress;
        BuildingGoldBubble _entityStataVeiw;
        TimeProgress _entityProgressVeiw;

        MainWindow mainWindow;
    
        #endregion

        #region Interface
        public  KWorkshop.Slot[] CurrSlot {
            get
            {
                return KWorkshop.Instance.GetSlot();
            }

        }
        int _slotIndex;
        public void BuySlot(int slotIndex)
        {
            var cost = KWorkshop.Instance.GetSlot(slotIndex).unlockCost;
            cost = CurrSlot[slotIndex].unlockCost;
            _slotIndex = slotIndex;
            string itemName ="";
            if (KItemManager.Instance.GetItem(cost.itemID) != null)
                itemName = KItemManager.Instance.GetItem(cost.itemID).itemName;
            KUIWindow.OpenWindow<MessageBox>(new MessageBox.Data()
            {
                content = string.Format("是否确定花费{0}{1}购买一个打造槽位",
                cost.itemCount,
               itemName),
                onCancel = () => { },
                onConfirm = buySlot
            }
              );
        }
        private void buySlot()
        {
            KWorkshop.Instance.BuySlot(_slotIndex, buySlotBack);
        }
        private void buySlotBack(int codeId,string content,object data)
        {
            if (codeId == 0)
            {
                FunctionWindow.RefurbishFunView();
                Debug.Log("购买锻造槽成功");

            }
            else
            {
                Debug.Log("购买锻造槽失败");
            }
        }
        public void Make(int slotIndex)
        {

            Debug.Log("开始锻造--"+ slotIndex);
            KUIWindow.OpenWindow<OrnamentShopWindow>(new OrnamentShopWindow.Data {
                indx = slotIndex,
                onConfirm = (codeId, content, data) =>
                 {
                     if (codeId == 0)
                     {
                         Debug.Log("锻造成功");
                         //FunctionWindow.RefurbishFunListData();
                         RefurbishData();
                     }
                     else
                     {
                         Debug.Log("锻造失败");
                     }
                 }
            });

        }
        /// <summary>
        /// 锻造槽 加速
        /// </summary>
        /// <param name="slotIndex"></param>
        public void OnMakeSpeedUp(int slotIndex)
        {
            if (GuideManager.Instance.IsGuideing)
            {
                RequestSpeedUp(slotIndex);
                return;
            }

            KUIWindow.OpenWindow<MessageBox>(new MessageBox.Data()
            {
                content = string.Format("是否确定花费{0}{1},立刻获得该装饰物？", speedUpMoneyCostGet(_remainTime), "钻石"),
                onCancel = () => { },
                onConfirm = () =>
                {
                    RequestSpeedUp(slotIndex);
                }
            });
        }

        private void RequestSpeedUp(int slotIndex)
        {
            KWorkshop.Instance.SpeedUp(slotIndex, (codeId, content, data) =>
            {
                if (codeId == 0)
                {
                    timeCountDownHide();
                    RefurbishData();
                    //FunctionWindow.RefurbishFunListData();
                    _remainTime = 0;
                    Debug.Log("锻造加速成功");
                }
                else
                {
                    Debug.Log("锻造加速失败");
                }
            });
        }

        public void GetSlot(int index)
        {

            Debug.Log("获取");

        }
        public void FormulaGet()
        {
            KUIWindow.OpenWindow<FormulaShopWindow>();
            Debug.Log("打开配方");

        }
        public override bool SpeedUpInfo(ref int moneyCost, ref int moneyType)
        {
            moneyCost = speedUpMoneyCostGet((int)_remainTime);
            moneyType = speedUpMoneyType;
            return true;
        }
        public override bool supportSpeedUp{ get { return _remainTime > 0; } }
        #endregion


        #region Property

        private KItemBuilding[] buildingMakeData
        {
            get
            {
               return KWorkshop.Instance.GetMakedItem();
            }
        }



        #endregion


        #region Method
        RectTransform _bubble;

        /// <summary>
        /// 城建元素获取焦点事件
        /// </summary>
        protected override void OnTap()
        {
            if (_isUseFunctionView)
            {
                base.OnTap();
            }
            else
            {
                if (isOneSelf)
                {
                    Collect();
                    base.OnTap();
                }
            }
            //entityView.ColorChangeSet();
            //Debug.Log("获得焦点----");
            //GameCamera.Instance.Show(this.entityView.centerNode.position);                 //元素窗口居中
        }

        /// <summary>
        /// 获取焦点事件
        /// </summary>
        /// <param name="focus"></param>
        protected override void OnFocus(bool focus)
        {
            base.OnFocus(focus);
            //if (focus)
            //{
            //    entityView.ColorChangeSet();
            //}
            //else
            //{
            //    entityView.ColorRecovery();
            //}
        }

        //BuildingStateView entityProgressVeiw
        //{
        //    get
        //    {
        //        if (_entityProgressVeiw == null)
        //        {
        //        }
        //        return _entityProgressVeiw;
        //    }
        //}

        BuildingTimeCounDown _buildingTimeCounDown;
        BuildingTimeCounDown buildingTimeCounDown
        {
            get
            {
                if (_buildingTimeCounDown == null)
                {
                    _entityProgressVeiw = BuildingStateMgr.Instance.ProgressShow(this);


                   _buildingTimeCounDown = _entityProgressVeiw.buildingTimeCounDown;
                   _buildingTimeCounDown.SetUpdateDelegate(timeUpdata);
                }
                return _buildingTimeCounDown;
            }
            set
            {
                _buildingTimeCounDown = value;
            }
        }

        bool isMakeInfoGet;
        /// <summary>
        /// 时间进度条 change回调
        /// </summary>
        /// <param name="time"></param>
        private void timeUpdata(int time)
        {
            _remainTime = time;
            if (_remainTime <= 0)
            {
                if (isMakeInfoGet)
                {
                    isMakeInfoGet = false;
                    timeCountDownHide();
                    Debug.Log("获取气泡数据");
                    KWorkshop.Instance.GetInfos(
                        (codeId, content, data) =>
                        {
                            RefurbishData();

                            Debug.Log("刷新气泡数据");
                        });
                }
            }
        }

        /// <summary>
        /// 时间进度条 ui显示
        /// </summary>
        private void timeCountDownShow()
        {
            KWorkshop.Slot slot =System.Array.Find(CurrSlot, item => item.MakeSate == KWorkshop.MakeSateType.Makeing);
            if (!(buildingMakeData!=null && buildingMakeData.Length > 0))
            {
                if (slot != null && _entityStataVeiw == null)
                {
                    _remainTime = slot.remainTime;
                    isMakeInfoGet = true;

                    buildingTimeCounDown.porgress = slot.remainTime;
                    buildingTimeCounDown.porgressMax = slot.totalTime;

                }
            }
        }
        /// <summary>
        /// 隐藏时间倒计时
        /// </summary>
        public void timeCountDownHide()
        {
            buildingTimeCounDown = null;
            _entityProgressVeiw = null;
            BuildingStateMgr.Instance.ProgressHide(this);
        }

        private void bubbleShow(KItemBuilding[] data)
        {

            if (data.Length>0)
            {
                if (_entityStataVeiw == null)
                    _entityStataVeiw = BuildingStateMgr.Instance.BubbleShow(this, onClickBubble);

                _entityStataVeiw.IconSet(data[0].iconName);
            }
        }
        private void bubbleHide()
        {
            _entityStataVeiw = null;
            BuildingStateMgr.Instance.BubbleHide(this);
        }
        private void onClickBubble()
        {

            Collect();
        }
        private Sprite oldSprite;
        /// <summary>
        /// 收集
        /// </summary>
        public void Collect()
        {
            KUIWindow.CloseWindow<FunctionWindow>();
            if (buildingMakeData.Length > 0)
                oldSprite = KIconManager.Instance.GetBuildingIcon(buildingMakeData[0].iconName);

            CollectBack();
        }
        /// <summary>
        /// 收集回调
        /// </summary>
        public void CollectBack()
        {
            KWorkshop.Instance.Collect(0, (codeId, content, data) =>
            {
                if (codeId == 0)
                {
                    Debug.Log("收集成功");
                    
                    RefurbishData();



                    if (mainWindow.showMajor)
                    {
                        new GameObject("node").transform.position= mainWindow.buildingBagNode.position;
                        IconFlyMgr.Instance.IconFlyStart(entityView.gmPoint.position,mainWindow.buildingBagNode.position, oldSprite);
                    }
                    else
                        IconFlyMgr.Instance.IconFlyStart(entityView.gmPoint.position, mainWindow.majorBtn.position, oldSprite);

                }
                else
                {
                    Debug.Log("收集失败");
                }

            }
          );
        }
        private KWorkshop.Slot getMakeFinishlost()
        {
            return System.Array.Find(CurrSlot,slot=> { return slot.MakeSate == KWorkshop.MakeSateType.MakeFinish; });
        }

        private KItemBuilding[] buildingMakeDataGet()
        {
            return KWorkshop.Instance.GetMakedItem();
        }
        private void RefurbishData()
        {
            Debug.Log("刷新数据");
           // KItemBuilding[] kItemBuilding = buildingMakeDataGet();
            if (buildingMakeData != null && buildingMakeData.Length > 0)
            {
                _isUseFunctionView = false;
                bubbleShow(buildingMakeData);
                timeCountDownHide();
            }
            else
            {
                _isUseFunctionView = true;
                bubbleHide();


            }
            timeCountDownShow();
            if (FunctionWindow.RefurbishFunView!=null)
                FunctionWindow.RefurbishFunView();


            playMakeingStateAnim();

        }
        private void playMakeingStateAnim()
        {
            if (System.Array.Find(CurrSlot, slot => slot.MakeSate == KWorkshop.MakeSateType.Makeing) != null)
            {
                entityView.PlayAnimation(touchAnimation, true);
                IsToggleIdle = false;
            }  
            else
                IsToggleIdle = true;
        }
          

        #endregion

        #region Unity 

        // Use this for initialization
        private void Start()
        {
            //viewTransform.localScale = Vector3.one * 0.348f;
            entityView.ShowModel();
            if(isOneSelf)
                RefurbishData();
            mainWindow = KUIWindow.GetWindow<MainWindow>();


        }

        private void Update()
        {
        }
        private void LateUpdate()
        {
            //if (_entityStataVeiw != null)
            //{
            //    _entityStataVeiw.BubblePosSet(  BuildingStateMgr.Instance.WorldToUI(entityView.gmPoint.position));
                 
            //}
            //if (_entityProgressVeiw != null)
            //{
            //    _entityProgressVeiw.BubblePosSet(BuildingStateMgr.Instance.WorldToUI(entityView.gmPoint.position));

            //}

        }

        #endregion
    }
}
