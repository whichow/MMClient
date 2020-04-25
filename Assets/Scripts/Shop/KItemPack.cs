// ***********************************************************************
// Company          : 
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
namespace Game
{
    using System.Collections;

    /// <summary>
    /// 物品包(商店)
    /// </summary>
    public class KItemPack : KItem
    {
        #region FIELD
     
        private int _remainTimestamp;

        #endregion

        #region PROPERTY

        public int remainCount
        {
            get;
            set;
        }

        public int remainTime
        {
            get { return _remainTimestamp - KLaunch.Timestamp; }
            set { _remainTimestamp = KLaunch.Timestamp + value; }
        }

        public ItemInfo[] bonusItems
        {
            get;
            private set;
        }
    
        public int limitedType
        {
            get;
            private set;
        }

        public string bundleId
        {
            get;
            private set;
        }
        public enum CommoditType
        {
            Cat=10,
            Buiding=11,
        }
        public int commodityType
        {
            get;
            private set;
        }
        public int number
        {
            get;
            private set;
        }
        #endregion

        #region METHOD

        public override void Load(Hashtable table)
        {
            base.Load(table);
            bonusItems = ItemInfo.FromList(table.GetArrayList("Show"));
            var costList = table.GetArrayList("BuyCost");
            if (costList != null && costList.Count == 2)
            {
                Money = (int)costList[0];
                Cost = (int)costList[1];
            }
            limitedType = table.GetInt("LimitedType");
            bundleId = table.GetString("BundleID");
            commodityType = table.GetInt("CommodityType");
            number = table.GetInt("Number");
        }

        #endregion

        #region UNITY     

        #endregion

        #region STATIC


        #endregion
    }
}
