// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "BuildingFunction" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Build
{
    using Callback = System.Action<int, string, object>;
    //public enum FunctionType
    //{
    //    KNone,
    //    /// <summary> 1：农田类 </summary>
    //    kFarm = 1,
    //    /// <summary> 2：猫舍类 </summary>
    //    kCatHouse = 2,
    //    /// <summary> 3：加工类</summary>
    //    kFactory = 3,
    //    /// <summary>4：树洞 </summary>
    //    kTree = 4,
    //    /// <summary>5：障碍物类 </summary>
    //    kObstacle = 5,
    //    /// <summary>6：宝箱类 </summary>
    //    kBox = 6,
    //    /// <summary> 7：装饰物类</summary>
    //    kDecoration = 7,
    //    /// <summary>  8：拍照小摊</summary>
    //    kTakePhotos = 8,
    //    /// <summary>9：手工作坊</summary>
    //    kManualWorkShop = 9,
    //    /// <summary>10：探索地带   </summary>
    //    kExpeditionZone = 10,
    //    /// <summary>10：寄养所   </summary>
    //    kEntrustPlace = 11,
    //    /// <summary>11：体力池   </summary>
    //    kPhysicalFitness = 12,

    //}
    public interface ICatFunction
    {
        int CatStorage { get; }
        KCat[] CatLst { get; }
        int Coin { get; }
        int CoinMax { get; }
        int CatGrade { get; }
        bool IsGradeMax { get; }
        void AddCat();
        void CatInfoShow(int catId);
        void CatCollectStart();
        void CatCollectStop();
        void CatCollect();
        void CatHouseUpGrade();
        int CatIconGet(int catId);
        int ProduceGoldRemainSeconds { get; }
        //KCat CatGet(int);
    }
    public interface IManualWorkShopFunction
    {
        void BuySlot(int slotIndex);

        void Make(int slotIndex);

        void OnMakeSpeedUp(int slotIndex);
        void FormulaGet();
        void GetSlot(int index);
        KWorkshop.Slot[] CurrSlot { get; }
    }
    public interface IFunCommon
    {
        bool IsSell
        {
            get;
        }
        bool IsRotate
        {
            get;
        }
        ///// <summary>
        ///// 是否正在旋转
        ///// </summary>
        //bool IsRotating
        //{
        //    get;
        //}
        bool IsRecovery
        {
            get;
        }
        void OnSell();
        void OnRotate();
        void OnRecovery();
        void OnRotateConfirm();

    }
    public interface IFunction
    {
        Building.Category functionTypeType
        {
            get;
        }
        string title
        {
            get;
        }
        bool supportSpeedUp
        {
            get;
        }

        bool supportGradeUp
        {
            get;
        }

        bool supportCollect
        {
            get;
        }

        bool supportInfomation
        {
            get;
        }

        bool SpeedUpInfo(ref int moneyCost, ref int moneyType);

        bool OnSpeedUp();

        bool OnInfomation();
        void OnIntoView();

        void GetAllTask();
    }

    /// <summary>
    /// 功能建筑
    /// </summary>
    public class BuildingFunction : Building, IFunction, IFunCommon
    {
        bool isRotating;
    public virtual Building.Category functionTypeType
        {
            get
            {
                if (_buildingData != null)
                    return (Building.Category)_buildingData.type;
                else
                    return Category.kNone;

            }
            // get; set;
        }
        string ttl;
        public virtual string title
        {
            get
            {
                return entityData.itemName;
            }
            set
            {
                ttl = value;
            }

        }
        public virtual bool supportSpeedUp
        {
            get; set;

        }

        public virtual bool supportGradeUp
        {
            get; set;
        }

        public virtual bool supportCollect
        {
            get; set;
        }

        public virtual bool supportInfomation
        {
            get; set;
        }

        public virtual bool SpeedUpInfo(ref int moneyCost, ref int moneyType)
        {
            return false;
        }

        public virtual bool OnSpeedUp()
        {
            return false;
        }

        public virtual bool OnInfomation()
        {
            Building building = this as Building;
            if(building)
            KUIWindow.OpenWindow<InfomationBox>(new InfomationBox.Data()
            {
                title = this.title,
                content = building.entityData.description,
            });
            return true;
        }
        public virtual void GetAllTask()
        {
            KExplore.Instance.GetAllTask(null);
        }
        public virtual void OnIntoView()
        {

        }
        public bool IsSell
        {
            get
            {
                if (_buildingData != null)
                    return _buildingData.saleable > 0;
                else
                    return false;
            }
        }
        public bool IsRotate
        {
            get
            {
                if (_buildingData != null)
                    return _buildingData.rotatable == 1;
                else
                    return false;
            }
        }
        //public bool IsRotating
        //{
        //    get
        //    {
        //        return false;
        //    }
        //}
        public bool IsRecovery
        {
            get
            {
                return entityData.recovery > 0;
            }
        }

        public virtual void OnSell()
        {
     
           
        }
      
        public virtual void OnRotate()
        {
            Debug.Log("旋转");
            if (IsRotate)

            {
                isRotating = true;
                entityView.RotateModel();
                MapObject mapObject = GetComponent<MapObject>();
                CollisionHighlight.Instance.ShowCollisions(mapObject, null);

                
            }
        }
        public virtual void OnRecovery()
        {
            BubbleManager.Instance.HideConfirm();
            CollisionHighlight.Instance.HideCollisions();
            if (IsRecovery)
            {

            }
        }
        public virtual void OnRotateConfirm()
        {
            Debug.Log("旋转确认");

            if (isRotating)
            {
                //Rotate();
                isRotating = false;
            }
        }
    }
}