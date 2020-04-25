// Auto Generated Code
using System.Collections;
using System.Collections.Generic;

namespace Game.DataModel
{
    public partial class ThingsXDM : IXDM
    {
        /// <summary>
        /// 道具ID
        /// </summary>
        public int ID { get; protected set; }
        /// <summary>
        /// 道具名
        /// </summary>
        public int NameId { get; protected set; }
        /// <summary>
        /// 道具图标
        /// </summary>
        public string Icon { get; protected set; }
        /// <summary>
        /// 物品类型  1-资源  2-抽卡券  3-消除道具  4-障碍物道具  5-装饰物材料  6-寄养卡  7-猫咪碎片  8-体力道具  9-跳跳道具
        /// </summary>
        public int Type { get; protected set; }
        /// <summary>
        /// 1-宠物碎片  2-装饰物材料  3-消耗品
        /// </summary>
        public int Tag { get; protected set; }
        /// <summary>
        /// 是否可合成
        /// </summary>
        public int Compose { get; protected set; }
        /// <summary>
        /// 出售价格
        /// </summary>
        public int SaleCoin { get; protected set; }
        /// <summary>
        /// 是否可使用
        /// </summary>
        public int Use { get; protected set; }
        /// <summary>
        /// 是否有详情
        /// </summary>
        public int More { get; protected set; }
        /// <summary>
        /// 最大堆叠数量
        /// </summary>
        public int MaxNumber { get; protected set; }
        /// <summary>
        /// 文本ID
        /// </summary>
        public int DescriptionId { get; protected set; }
        /// <summary>
        /// 用钻石兑换比例
        /// </summary>
        public int Cost { get; protected set; }
        /// <summary>
        /// 钻石
        /// </summary>
        public int Money { get; protected set; }
        /// <summary>
        /// 获取信息
        /// </summary>
        public List<int> Getinformation { get; protected set; }
        /// <summary>
        /// 稀有度：数字越大越稀有
        /// </summary>
        public int Rarity { get; protected set; }
        /// <summary>
        /// 类型，数字
        /// </summary>
        public List<int> Number { get; protected set; }

        public void Parse(Hashtable table)
        {
            ID = table.GetInt("ID");
            NameId = table.GetInt("NameId");
            Icon = table.GetString("Icon");
            Type = table.GetInt("Type");
            Tag = table.GetInt("Tag");
            Compose = table.GetInt("Compose");
            SaleCoin = table.GetInt("SaleCoin");
            Use = table.GetInt("Use");
            More = table.GetInt("More");
            MaxNumber = table.GetInt("MaxNumber");
            DescriptionId = table.GetInt("DescriptionId");
            Cost = table.GetInt("Cost");
            Money = table.GetInt("Money");
            Getinformation = table.GetIntList("Getinformation");
            Rarity = table.GetInt("Rarity");
            Number = table.GetIntList("Number");
        }
    }

    public partial class ThingsXTable : XTable<ThingsXDM>
    {
        public override string ResourceName
        {
            get { return "ThingsXDM";}
        }
    }

    public partial class XTable
    {
        public static ThingsXTable ThingsXTable = new ThingsXTable();
    }
}
