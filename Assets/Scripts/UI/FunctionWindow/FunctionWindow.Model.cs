using Game.Build;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public partial class FunctionWindow
    {
        #region field
        /// <summary>
        /// 建筑通用接口
        /// </summary>
        IFunction _iFunction;
        /// <summary>
        /// 手工作坊接口
        /// </summary>
        IManualWorkShopFunction _iMWShop;
        /// <summary>
        /// 猫舍建筑特例接口
        /// </summary>
        ICatFunction _icat;

        /// <summary>
        /// 建筑类型
        /// </summary>
        public Build.Building.Category FunctionType;

        /// <summary>
        /// 刷新界面
        /// </summary>
        public static System.Action RefurbishFunView;

        KWorkshop.Slot[] _makeLstData;  
        KWorkshop.Slot[] _makeingLstData;  
        KWorkshop.Slot[] _makeReadyLstData; 
        KWorkshop.Slot[] _unlockLstData;

        KCat[] _usedCat;
        KCat[] _unUsedCat;
        #endregion
        #region Method

        /// <summary>
        /// 手工作坊槽位初始化数据
        /// </summary>
        /// <param name="slotList"></param>
        private void makeLstInit(KWorkshop.Slot[] slotList)
        {
            
            _makeLstData = System.Array.FindAll(slotList,item => item.MakeSate == KWorkshop.MakeSateType.Make || item.MakeSate == KWorkshop.MakeSateType.MakeFinish);
            _makeingLstData = System.Array.FindAll(slotList,item => item.MakeSate == KWorkshop.MakeSateType.Makeing);
            _makeReadyLstData = System.Array.FindAll(slotList,item => item.MakeSate == KWorkshop.MakeSateType.MakeReady);
            _unlockLstData = System.Array.FindAll(slotList,item => item.MakeSate != KWorkshop.MakeSateType.Lock);
        }
        private void catLstInit(KCat[] kCats)
        {
              _usedCat = kCats;
             _unUsedCat =new KCat[_icat.CatStorage - _icat.CatLst.Length];
        }
        #endregion
    }
}
