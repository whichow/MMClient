// Auto Generated Code
using System.Collections;
using System.Collections.Generic;

namespace Game.DataModel
{
    public partial class ActivityAddDayRewardXDM : IXDM
    {
        /// <summary>
        /// 日期
        /// </summary>
        public int ID { get; protected set; }
        /// <summary>
        /// [物品ID,物品数量]
        /// </summary>
        public List<int> Rewards { get; protected set; }

        public void Parse(Hashtable table)
        {
            ID = table.GetInt("ID");
            Rewards = table.GetIntList("Rewards");

            ParseEx();
        }

        partial void ParseEx();

    }

    public partial class ActivityAddDayRewardXTable : XTable<ActivityAddDayRewardXDM>
    {
        public override string ResourceName
        {
            get { return "ActivityAddDayRewardXDM";}
        }
    }

    public partial class XTable
    {
        public static ActivityAddDayRewardXTable ActivityAddDayRewardXTable = new ActivityAddDayRewardXTable();
    }
}
