//// ***********************************************************************
//// Assembly         : Unity
//// Author           : Kimch
//// Created          : 2017-11-22
////
//// Last Modified By : Kimch
//// Last Modified On : 
//// ***********************************************************************
//// <copyright file= "KItemFosterCard" company=""></copyright>
//// <summary></summary>
//// ***********************************************************************
//namespace Game
//{
//    using System.Collections;

//    /// <summary>
//    /// 寄养卡
//    /// </summary>
//    public class KItemFosterCard : KItem
//    {
//        /// <summary>
//        /// 绑定的物品Id
//        /// </summary>
//        public int bindItemId
//        {
//            get;
//            private set;
//        }

//        /// <summary>
//        /// 寄养卡持续秒数
//        /// </summary>
//        public int fosterTime
//        {
//            get;
//            private set;
//        }

//        /// <summary>
//        /// 掉落物(每小时）
//        /// </summary>
//        public ItemInfo[] rewards
//        {
//            get;
//            private set;
//        }

//        public override int curCount
//        {
//            get { return KDatabase.GetInt(bindItemId, "count"); }
//            set { KDatabase.SetInt(bindItemId, "count", value); }
//        }

//        #region METHOD

//        public override void Load(Hashtable table)
//        {
//            base.Load(table);
//            bindItemId = table.GetInt("ItemId");
//            fosterTime = table.GetInt("FosterTime");
//            rewards = ItemInfo.FromList(table.GetArrayList("Reward"));
//        }

//        #endregion
//    }
//}
