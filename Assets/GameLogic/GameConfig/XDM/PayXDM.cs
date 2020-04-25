// Auto Generated Code
using System.Collections;
using System.Collections.Generic;

namespace Game.DataModel
{
    public partial class PayXDM : IXDM
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public int ID { get; protected set; }
        /// <summary>
        /// 订单名称
        /// </summary>
        public int Name { get; protected set; }
        /// <summary>
        /// 是否是活动类型支付
        /// </summary>
        public int ActivePay { get; protected set; }
        /// <summary>
        /// 支付ID
        /// </summary>
        public string BundleID { get; protected set; }
        /// <summary>
        /// 首次直接获得钻石
        /// </summary>
        public int GemRewardFirst { get; protected set; }
        /// <summary>
        /// 直接获得钻石
        /// </summary>
        public int GemReward { get; protected set; }
        /// <summary>
        /// 月卡天数
        /// </summary>
        public int MonthCardDay { get; protected set; }
        /// <summary>
        /// 月卡钻石
        /// </summary>
        public int MonthCardReward { get; protected set; }
        /// <summary>
        /// 奖励展示
        /// </summary>
        public int RewardShow { get; protected set; }
        /// <summary>
        /// 额外奖励展示
        /// </summary>
        public int RewardBonusShow { get; protected set; }
        /// <summary>
        /// 额外道具
        /// </summary>
        public string ItemReward { get; protected set; }
        /// <summary>
        /// 给TK记录数据
        /// </summary>
        public string RecordGold { get; protected set; }

        public void Parse(Hashtable table)
        {
            ID = table.GetInt("ID");
            Name = table.GetInt("Name");
            ActivePay = table.GetInt("ActivePay");
            BundleID = table.GetString("BundleID");
            GemRewardFirst = table.GetInt("GemRewardFirst");
            GemReward = table.GetInt("GemReward");
            MonthCardDay = table.GetInt("MonthCardDay");
            MonthCardReward = table.GetInt("MonthCardReward");
            RewardShow = table.GetInt("RewardShow");
            RewardBonusShow = table.GetInt("RewardBonusShow");
            ItemReward = table.GetString("ItemReward");
            RecordGold = table.GetString("RecordGold");

            ParseEx();
        }

        partial void ParseEx();

    }

    public partial class PayXTable : XTable<PayXDM>
    {
        public override string ResourceName
        {
            get { return "PayXDM";}
        }
    }

    public partial class XTable
    {
        public static PayXTable PayXTable = new PayXTable();
    }
}
