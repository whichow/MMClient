// Auto Generated Code
using System.Collections;
using System.Collections.Generic;

namespace Game.DataModel
{
    public partial class ShopXDM : IXDM
    {
        /// <summary>
        /// 
        /// </summary>
        public int ID { get; protected set; }
        /// <summary>
        /// 商店类型ID  1-特殊商店  2-友情点商店  3-魅力勋章商店  4-人民币商店  5-魂石商店  6-复活步数商店  7-装扮             8-体力商店            
        /// </summary>
        public int Tag { get; protected set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public int Name { get; protected set; }
        /// <summary>
        /// 物品类型  1-资源  2-抽卡券  3-消除道具  4-障碍物道具  5-装饰物材料  6-寄养卡  7-猫咪碎片  8-体力道具  9-跳跳道具  10-猫咪  11-建筑物  12-礼包  13-装饰物配方  14-装扮  15-月卡
        /// </summary>
        public int CommodityType { get; protected set; }
        /// <summary>
        /// 物品ID
        /// </summary>
        public int CommodityId { get; protected set; }
        /// <summary>
        /// 商品数量
        /// </summary>
        public int Number { get; protected set; }
        /// <summary>
        /// 商品Icon
        /// </summary>
        public string Icon { get; protected set; }
        /// <summary>
        /// 购买消耗ID,数量
        /// </summary>
        public List<int> BuyCost { get; protected set; }
        /// <summary>
        /// 商品限量类型  0-不限量  1-服务器限量  2-个人限量
        /// </summary>
        public int LimitedType { get; protected set; }
        /// <summary>
        /// 商品限量时间（天）
        /// </summary>
        public int LimitedTime { get; protected set; }
        /// <summary>
        /// 商品限量数量
        /// </summary>
        public int LimitedNumber { get; protected set; }
        /// <summary>
        /// 展示
        /// </summary>
        public List<int> Show { get; protected set; }
        /// <summary>
        /// 支付ID
        /// </summary>
        public int PayID { get; protected set; }

        public void Parse(Hashtable table)
        {
            ID = table.GetInt("ID");
            Tag = table.GetInt("Tag");
            Name = table.GetInt("Name");
            CommodityType = table.GetInt("CommodityType");
            CommodityId = table.GetInt("CommodityId");
            Number = table.GetInt("Number");
            Icon = table.GetString("Icon");
            BuyCost = table.GetIntList("BuyCost");
            LimitedType = table.GetInt("LimitedType");
            LimitedTime = table.GetInt("LimitedTime");
            LimitedNumber = table.GetInt("LimitedNumber");
            Show = table.GetIntList("Show");
            PayID = table.GetInt("PayID");

            ParseEx();
        }

        partial void ParseEx();

    }

    public partial class ShopXTable : XTable<ShopXDM>
    {
        public override string ResourceName
        {
            get { return "ShopXDM";}
        }
    }

    public partial class XTable
    {
        public static ShopXTable ShopXTable = new ShopXTable();
    }
}
