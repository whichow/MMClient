// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "FunctionWindow" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using Game.Build;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    using Callback = System.Action<int, string, object>;
    public partial class FunctionWindow : KUIWindow
    {
        //#region Field


      

        //#endregion
        //#region
        //#endregion
        #region Constructor

        public FunctionWindow()
            : base(UILayer.kNormal, UIMode.kSequenceHide)
        {
            uiPath = "BuildingFunction";
            RefurbishFunView = RefurbishData;
        }

        #endregion

        #region Method


        #region 建筑物 统一接口 调用

        /// <summary>
        /// 建筑物加速
        /// </summary>
        private void OnSpeedUp()
        {
            if (_iFunction == null)
                return;
            _iFunction.OnSpeedUp();
            KUIWindow.CloseWindow(this);

        }
        /// <summary>
        /// 显示建筑物信息
        /// </summary>
        private void OnInfo()
        {
            if (_iFunction != null)
            {
                _iFunction.OnInfomation();
            }
        }
        #endregion
        #region 树洞 拍照小摊 探索地带接口 统一接口调用
        /// <summary>
        /// 建筑物进入按钮
        /// </summary>
        private void OnIntoView()
        {
            _iFunction.OnIntoView();
        }
        #endregion
        #region 手工作坊
        /// <summary>
        /// 手工作坊加速
        /// </summary>
        /// <param name="index"></param>
        private void OnMakeSpeedUp(int index)
        {
            Debug.Log("手工作坊打造加速" + index);
            _iMWShop.OnMakeSpeedUp(index);
        }
        /// <summary>
        /// 手工作坊打造
        /// </summary>
        /// <param name="index"></param>
        private void IMWShopMake(int index)
        {
            Debug.Log("手工作坊打造"+ index);
            _iMWShop.Make(index);
        }
        /// <summary>
        /// 手工作坊槽位购买
        /// </summary>
        /// <param name="count"></param>
        private void IMWShopBuySlot(int count)
        {
            Debug.Log("手工作坊打造 购买" + count);
            _iMWShop.BuySlot(count);
        }
        /// <summary>
        /// 手工作坊获取配方
        /// </summary>
        private void IMWShopFormulaGet()
        {
            _iMWShop.FormulaGet();
        }
        /// <summary>
        /// 获取槽位购买金币数
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private int IMWShopCurrSlot(int index)
        {
            return _iMWShop.CurrSlot[index].unlockCost.itemCount;
        }
        #endregion

        #region 猫舍

        /// <summary>
        /// 猫舍信息
        /// </summary>
        /// <param name="catId"></param>
        private void CatInfoShow(int catId)
        {
            _icat.CatInfoShow(catId);
        }
        /// <summary>
        /// 添加猫
        /// </summary>
        private void AddCat()
        {
            _icat.AddCat();
        }
        /// <summary>
        /// 猫舍升级
        /// </summary>
        private void CatHouseUpGrade()
        {
            _icat.CatHouseUpGrade();
        }
        /// <summary>
        ///猫舍加速
        /// </summary>
        private void OnCatSpeedUp()
        {
            _iFunction.OnSpeedUp();
        }
        #endregion
        #endregion

        //#endregion
    }
}

