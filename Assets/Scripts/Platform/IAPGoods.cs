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
    using System;

    [Serializable]
    public class IAPGoods
    {
        /// <summary>
        /// 
        /// </summary>
        public int id
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string storeId
        {
            get;
            set;
        }

        public string priceString
        {
            get { return localizedPriceString; }
            set { localizedPriceString = value; }
        }

        /// <summary>
        /// Stock Keeping Unit(库存量单位)
        /// </summary>
        public string sku
        {
            get
            {
                return storeId;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool canPurchase
        {
            get;
            set;
        }

        //
        // 摘要:
        //     Localized price string.
        public string localizedPriceString { get; set; }
        //
        // 摘要:
        //     Localized product title as retrieved from the store subsystem; e.g. Apple or
        //     Google.
        public string localizedTitle { get; set; }
        //
        // 摘要:
        //     Localized product description as retrieved from the store subsystem; e.g. Apple
        //     or Google.
        public string localizedDescription { get; set; }
        //
        // 摘要:
        //     Product currency in ISO 4217 format; e.g. GBP or USD.
        public string isoCurrencyCode { get; set; }
        //
        // 摘要:
        //     Decimal product price denominated in the currency indicated by isoCurrencySymbol.
        public decimal localizedPrice { get; set; }
    }
}

