// ***********************************************************************
// Company          : 
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************

namespace Game.IAP
{
    using System.Collections.Generic;
    using UnityEngine;

    public class IAPGoodsBank : MonoBehaviour
    {
        #region Field

        /// <summary>
        /// 
        /// </summary>
        private static IAPGoodsBank _Instance;
        /// <summary>
        /// 
        /// </summary>
        private static int _Buysum = -1;

        /// <summary>
        /// 
        /// </summary>
        private List<IAPGoods> _allGoods = new List<IAPGoods>();

        #endregion  

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string[] ProductArray()
        {
            return ProductList().ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<string> ProductList()
        {
            var list = new List<string>();
            foreach (var good in GoodsList)
            {
                list.Add(good.sku);
            }
            return list;
        }

        /// <summary>
        /// 商品列表
        /// </summary>
        public static List<IAPGoods> GoodsList
        {
            get
            {
                if (_Instance == null)
                {
                    return null;
                }
                return _Instance._allGoods;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sku"></param>
        /// <returns></returns>
        public static IAPGoods GetBySKU(string sku)
        {
            foreach (var goods in GoodsList)
            {
                if (goods.sku == sku)
                {
                    return goods;
                }
            }
            return null;
        }

        public void RegisterGoods(IAPGoods goods)
        {
            if (_allGoods != null)
            {
                _allGoods.Add(goods);
            }
        }

        #region Unity

        private void Awake()
        {
            _Instance = this;
        }

        #endregion

    }
}

