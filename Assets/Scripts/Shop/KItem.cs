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
    using System.Collections.Generic;
    using Callback = System.Action<int, string, object>;

    /// <summary>
    /// 物品基类
    /// </summary>
    public class KItem
    {
        #region CONST

        #endregion

        #region ENUM

        public enum MoneyType
        {
            kRmb = 1,
            kCoin = 2,
            kStone = 3,
            kTime = 11,
            kLowCard = 201,
            kMidCard = 202,
        }
        public enum GiftType
        {
            kNone=-1,
            kRmb = 1,
            kGold = 2,
            kDiamont = 3,
            kFood = 4,
            kEnergy,
            kFriendValue,
            kCharmValue,
            kExp,
            kStone1,
            kMedel,
        }
        public enum GiftOtherType
        {
            kNone =0,
            /// <summary>资源 </summary>
            kResourse,
            /// <summary>抽卡券 </summary>
            kLuckyCard,
            /// <summary>消除道具</summary>
            kRemoveProp,
            /// <summary>障碍物道具</summary>
            kObstacleProp,
            /// <summary>装饰物材料</summary>
            kDecorateProp,
            /// <summary> 寄养卡</summary>
            kFosterCare,
            /// <summary>猫咪碎片</summary>
            kCatCard,
            /// <summary>体力道具 </summary>
            kStrength,
            /// <summary>跳跳道具</summary>
            kJump,

        }
        #endregion

        #region MODEL

        /// <summary>
        /// 物品信息
        /// </summary>
        public struct ItemInfo
        {
            public int itemID;
            public int itemCount;

            public static ItemInfo[] FromArray(int[] data)
            {
                if (data != null)
                {
                    int length = data.Length;
                    var ret = new ItemInfo[length / 2];
                    for (int i = 0; i < length - 1;)
                    {
                        ret[i / 2] = new ItemInfo
                        {
                            itemID = data[i++],
                            itemCount = data[i++],
                        };
                    }
                    return ret;
                }
                else
                {
                    return new ItemInfo[0];
                }
            }

            public static ItemInfo[] FromList(ArrayList data)
            {
                if (data != null)
                {
                    int count = data.Count;
                    var ret = new ItemInfo[count / 2];
                    for (int i = 0; i < count - 1;)
                    {
                        ret[i / 2] = new ItemInfo
                        {
                            itemID = (int)data[i++],
                            itemCount = (int)data[i++],
                        };
                    }
                    return ret;
                }
                else
                {
                    return new ItemInfo[0];
                }
            }

            public static ItemInfo[] FromList(IList<int> data)
            {
                if (data != null)
                {
                    int count = data.Count;
                    var ret = new ItemInfo[count / 2];
                    for (int i = 0; i < count - 1;)
                    {
                        ret[i / 2] = new ItemInfo
                        {
                            itemID = data[i++],
                            itemCount = data[i++],
                        };
                    }
                    return ret;
                }
                else
                {
                    return new ItemInfo[0];
                }
            }

            public static ItemInfo Convert(int[] data)
            {
                if (data != null && data.Length >= 2)
                {
                    var ret = new ItemInfo()
                    {
                        itemID = data[0],
                        itemCount = data[1],
                    };
                    return ret;
                }
                else
                {
                    return new ItemInfo();
                }
            }

            public static ItemInfo Convert(ArrayList data)
            {
                if (data != null && data.Count >= 2)
                {
                    var ret = new ItemInfo()
                    {
                        itemID = (int)data[0],
                        itemCount = (int)data[1],
                    };
                    return ret;
                }
                else
                {
                    return new ItemInfo();
                }
            }

            public static ItemInfo Convert(List<int> data)
            {
                if (data != null && data.Count >= 2)
                {
                    var ret = new ItemInfo()
                    {
                        itemID = data[0],
                        itemCount = data[1],
                    };
                    return ret;
                }
                else
                {
                    return new ItemInfo();
                }
            }
        }

        #endregion

        #region FIELD 

        private int _nameId;
        /// <summary>
        /// 描述Id
        /// </summary>
        private int _descriptionId;

        #endregion

        #region PROPERTY

        /// <summary>Gets the item identifier.物品ID(唯一).</summary>
        public int itemID
        {
            get;
            private set;
        }

        //private string _itemName;
        /// <summary>Gets the item name.物品名字(唯一).</summary>
        public string itemName
        {
            get { return KLocalization.GetLocalString(_nameId); }
        }

        /// <summary>Gets the item tag.1 bag.</summary>
        /// <value>The item tag.</value>
        public int itemTag
        {
            get;
            private set;
        }

        /// <summary>Gets the</summary>
        public int itemType
        {
            get;
            private set;
        }

        /// <summary>Gets the icon name.物品图标.</summary>
        public string iconName
        {
            get;
            private set;
        }

        /// <summary>Gets the display name.物品名称.</summary>
        public string displayName
        {
            get { return KLocalization.GetLocalString(_nameId); }
        }

        /// <summary>Gets the description.物品描述</summary>
        public string description
        {
            get { return KLocalization.GetLocalString(_descriptionId); }
            //private set;
        }

        /// <summary>Gets the item quality.物品品质.稀有度.稀有度 1,2,3,4,5</summary>
        public int rarity
        {
            get;
            private set;
        }

        /// <summary>Gets the curr price.物品兑换比例.</summary>
        public int Cost
        {
            get;
            set;
        }

        /// <summary>Gets the money type.物品当前价格.</summary>
        public int Money
        {
            get;
            set;
        }

        /// <summary>Gets or sets the money cost 2.</summary>
        public int SaleCoin
        {
            get;
            set;
        }

        /// <summary>Gets the curr count.物品当前数量.</summary>
        public virtual int curCount
        {
            get { return KDatabase.GetInt(itemID, "count"); }
            set { KDatabase.SetInt(itemID, "count", value); }
        }
        /// <summary> 获取途径的ID </summary>
        public int[] getInformation
        {
            get;
            private set;
        }

        #endregion

        #region METHOD

        /// <summary>Gets the is lock.是否未解锁.</summary>
        public virtual bool isLock
        {
            get { return curCount < 0; }
        }
        /// <summary>Gets the is have.是否未拥有.</summary>
        public virtual bool isHave
        {
            get { return curCount > 0; }
        }

        #endregion

        #region READ AND SAVE

        /// <summary>Loads the specified table.</summary>
        /// <param name="table">The table.</param>
        public virtual void Load(Hashtable table)
        {
            this.itemID = table.GetInt("ID");
            this.itemTag = table.GetInt("Tag");
            this.itemType = table.GetInt("Type");
            string itemName = table.GetString("Name");
            if (!string.IsNullOrEmpty(itemName))
                _nameId = KLanguageManager.Instance.GetStringId(itemName);
            this.iconName = table.GetString("Icon");
            this.rarity = table.GetInt("Rarity");

            int nameId = table.GetInt("NameId");
            if (nameId != 0)
                _nameId = nameId;
            //if (_nameId != 0)
            //{
            //    this.itemName = displayName;
            //}
            //this.description = table.GetString("Description");
            _descriptionId = table.GetInt("DescriptionId");

            this.Cost = table.GetInt("Cost");
            this.Money = table.GetInt("Money");
            this.SaleCoin = table.GetInt("SaleCoin");
            this.getInformation = table.GetArray<int>("Getinformation");
        }

        public virtual void LoadComplete()
        {

        }

        #endregion
    }
}
