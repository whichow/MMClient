// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "Building" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using Game.DataModel;
using Game.UI;
using Msg.ClientMessage;
using UnityEngine;

namespace Game.Build
{
    /// <summary>
    /// 城建元素基类（创建初始化城建元素）
    /// </summary>
    public class Building : MonoBehaviour
    {
        #region Enum

        public struct Date
        {
            public KItemBuilding kItemBuilding;
            public int dir;
            public InitType initType;
        }
        public enum State : byte
        {
            kNone = 0,
            /// <summary>建筑完成原地不动状态</summary>
            kIdle = 1,
            /// <summary> 建设状态</summary>
            kBuild = 2,
            /// <summary> 升级状态</summary>
            kUpgrade = 3,
            /// <summary> 建设完成状态</summary>
            kProduce = 4,
            /// <summary> 移动状态</summary>
            kMove = 5,
            /// <summary> 静止状态</summary>
            kStatic = 6,
            /// <summary> 切换到产品状态 </summary>
            KToProduce = 7,
        }

        /// <summary>
        /// 建筑分类
        /// </summary>
        public enum Category : byte
        {
            kNone,
            /// <summary> 1：农田类 </summary>
            kFarm = 1,
            /// <summary> 2：猫舍类 </summary>
            kCatHouse = 2,
            /// <summary> 3：加工类</summary>
            kFactory = 3,
            /// <summary>4：树洞 </summary>
            kTree = 4,
            /// <summary>5：障碍物类 </summary>
            kObstacle = 5,
            /// <summary>6：宝箱类 </summary>
            kBox = 6,
            /// <summary> 7：装饰物类</summary>
            kDecoration = 7,
            /// <summary>  8：拍照小摊</summary>
            kTakePhotos = 8,
            /// <summary>9：手工作坊</summary>
            kManualWorkShop = 9,
            /// <summary>10：探索地带   </summary>
            kExpeditionZone = 10,
            /// <summary>11：寄养所   </summary>
            kFosterCare = 11,
            /// <summary>12：体力池   </summary>
            kLifePool = 12,
            /// <summary>13：地板   </summary>
            kSurface = 13,
        }
        public enum InitType
        {
            None,
            /// <summary> 服务器数据实例化类型</summary>
            Server,
            /// <summary> 客户端数据实例化类型</summary>
            Create,
        }
        #endregion

        #region Field 

        private int _shopId;

        /// <summary>
        /// 
        /// </summary>
        public KItemBuilding _buildingData;
        /// <summary>
        /// 
        /// </summary>
        private State _currState;
        /// <summary>
        /// 
        /// </summary>
        private Building _parent;

        #endregion

        #region Property
        public int buildTime
        {
            get
            {
                return entityData.buildTime;
            }
        }

        public int buildingId
        {
            get;
            set;
        }

        public virtual string idleAnimation
        {
            get { return "idle"; }
        }

        public virtual string touchAnimation
        {
            get { return "touch"; }
        }

        public bool IsBuild
        {
            get
            {
                return entityData.buildTime > 0;
            }
        }
        //public int isNeedBuy;
        public int IsNeedBuy
        {
            get
            {
                return entityData.itemTag == 1 ? 1 : 0;
            }
        }
        public bool isCanSelect
        {
            get
            {
                return curState == State.kNone || curState == State.kProduce;
            }
        }

        public Category CurrCategory;
        //public bool IsServerBuilding
        //{
        //    get;set;
        //}
        /// <summary>
        /// 
        /// </summary>
        public State curState
        {
            get { return _currState; }
            set
            {
                this.StateChange(value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual KItemBuilding entityData
        {
            get { return _buildingData; }
            set
            {
                _buildingData = value;
                this.OnEntityDataChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        //public KItemBuilding entityDataActual
        //{
        //    get { return _buildingData; }
        //}

        /// <summary>
        /// 
        /// </summary>
        public Int2 mapSize
        {
            get { return _buildingData.mapSize; }
        }

        /// <summary>
        /// 
        /// </summary>
        public BuildingView entityView
        {
            get;
            protected set;
        }

        /// <summary>
        /// 
        /// </summary>
        public GameObject viewObject
        {
            get
            {
                return this.entityView ? this.entityView.gameObject : null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Transform viewTransform
        {
            get
            {
                return this.entityView ? this.entityView.transform : null;
            }
        }

        /// <summary>
        /// 气泡的节点
        /// </summary>
        public Transform bubbleNode
        {
            get
            {
                return this.entityView ? this.entityView.bubbleNode : null;
            }
        }
        /// <summary>
        /// 实例化类型
        /// </summary>
        public InitType InitOfType { get; set; }

        public ViewBuildingInfo viewBuildingInfo{ get; set; }
       /// <summary>
       /// 是否是玩家自己的城建
       /// </summary>
        public bool isOneSelf { get { return BuildingManager.Instance.IsOneSelf; } }

        /// <summary>
        /// 是否切换idle动画
        /// </summary>
        private bool _isToggleIdle = true;
        public bool IsToggleIdle
        {
            get
            {
                return _isToggleIdle;
            }
            set
            {
                _isToggleIdle = value;
            }
        }
        private bool _isMove =true;
        /// <summary>
        /// 当前建筑是否可移动
        /// </summary>
        public bool isMove {
            get
            {
                return _isMove;
            }
            set
            {
                _isMove = value;
            }
         }
        private object _mainWindow;
        public object _MainWindow
        {
            get
            {
                if (_mainWindow == null)
                    _mainWindow = KUIWindow.GetWindow<MainWindow>();
                return _mainWindow;
            }
        }

        /// <summary>
        /// 建筑旋转方向
        /// </summary>
        public int RotateDir
        {
            get;
            set;
        }

        public System.Action AnimationFinish;
        # endregion

        #region Method 
        /// <summary>
        /// 创建城建元素实体节点
        /// </summary>
        /// <param name="data"></param>
        public void InitEnity(KItemBuilding data)
        {
            entityData = data;

            entityView = new GameObject("View").AddComponent<BuildingView>();
            entityView.transform.SetParent(this.transform, false);
          
            entityView.Init(this);

            RefurbishData();


        }
        /// <summary>
        /// 城建元素创建发送服务端信息
        /// </summary>
        public void BuildingSet()
        {

        }
        /// <summary>
        /// 设置变化状态
        /// </summary>
        /// <param name="state"></param>
        public void StateChange(State state)
        {
            State lastState = _currState;
            State newState = this.CheckState(state);
            _currState = newState;

            OnStateLeave(lastState);
            OnStateEnter(newState);
        }

        public void ShowBuild()
        {
            //if (IsBuild)
            //{

            //    //entityView.ShowBuild();
            //}
        }
        public virtual void DelBuilding()
        {
            Debug.Log("销毁建筑"+ buildingId);
            Destroy(this.gameObject);
        }
        public virtual void RefurbishData()
        {

        }
        /// <summary>
        /// 点击事件(每次点击都会调用)
        /// </summary>
        protected virtual void OnTap()
        {
            if (isOneSelf)
            {
                var function = this as IFunction;
                if (function != null)
                {
                    KUIWindow.OpenWindow<Game.UI.FunctionWindow>(function);
                }
            }
            if(IsToggleIdle)
                entityView.TouchModel(true);
            //entityView.PlayAnimation(touchAnimation,true);
        }

        /// <summary>
        /// 获得或失去焦点事件(变化时调用)
        /// </summary>
        /// <param name="focus"></param>
        protected virtual void OnFocus(bool focus)
        {
            if (!focus)
            {
                //if (isOneSelf)
                //{
                //    KUIWindow.CloseWindow<Game.UI.FunctionWindow>();
                //}
            }
            else
            {
                BubbleManager.Instance.HideObstacleClear();
            }
        }

        protected virtual void OnViewDisable()
        {
        }

        protected virtual void OnViewEnable()
        {
        }

        protected virtual State CheckState(State state)
        {
            return state;
        }

        protected virtual void OnStateEnter(State state)
        {

        }

        protected virtual void OnStateLeave(State state)
        {

        }

        protected virtual void OnEntityDataChanged()
        {

        }

        #endregion

        #region Static Method

        /// <summary>
        /// 创建城建元素根节点
        /// </summary>
        /// <typeparam name="T">城建元素类型</typeparam>
        /// <param name="entityData">城建元素数据</param>
        /// <returns></returns>
        public static Building CreateEntity<T>(Building.Date data) where T : Building
        {
            if (data.kItemBuilding == null)
            {
                Debug.LogError("Entity data is null.");
                return null;
            }

            var gameObj = new GameObject(data.kItemBuilding.itemName);
            gameObj.SetActive(false);
            var entity = gameObj.AddComponent<T>();
            entity.InitOfType = data.initType;
            entity.RotateDir = data.dir;
            entity.InitEnity(data.kItemBuilding);

            gameObj.SetActive(true);
            return entity;
        }

        /// <summary>
        /// 创建城建元素根节点
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="entityData"></param>
        /// <returns></returns>
        public static Building CreateEntity(System.Type entityType, KItemBuilding entityData)
        {
            if (entityData == null)
            {
                Debug.LogError("Entity data is null.");
                return null;
            }

            var gameObj = new GameObject(entityData.itemName);
            gameObj.SetActive(false);
            var entity = gameObj.AddComponent(entityType) as Building;
            entity.InitEnity(entityData);
            gameObj.SetActive(true);
            return entity;
        }
        /// <summary>
        /// 加速的消耗货币数量
        /// </summary>
        public int speedUpMoneyCostGet(int remainTime)
        {
            var kItem = XTable.ItemXTable.GetByID(ItemIDConst.TimeProp);
            int timeCost = 0;
            if (kItem != null)
            {
                timeCost = kItem.Cost;
            }
            if(kItem.Money == ItemIDConst.Gold)
                return Mathf.FloorToInt((float)remainTime / timeCost);
            else if(kItem.Money == ItemIDConst.Diamond)
                return Mathf.CeilToInt((float)remainTime / timeCost);
            else
                return Mathf.CeilToInt((float)remainTime / timeCost);
        }

        /// <summary>
        /// 加速的消耗货币类型
        /// </summary>
        public int speedUpMoneyType
        {
            get { return XTable.ItemXTable.GetByID(ItemIDConst.TimeProp).Money; }
        }
        #endregion

        #region Unity
        
        #endregion
    }
}
