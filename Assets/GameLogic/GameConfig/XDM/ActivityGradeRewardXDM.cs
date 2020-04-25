// Auto Generated Code
using System.Collections;
using System.Collections.Generic;

namespace Game.DataModel
{
    public partial class ActivityGradeRewardXDM : IXDM
    {
        /// <summary>
        /// 等级
        /// </summary>
        public int ID { get; protected set; }
        /// <summary>
        /// 奖励
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

    public partial class ActivityGradeRewardXTable : XTable<ActivityGradeRewardXDM>
    {
        public override string ResourceName
        {
            get { return "ActivityGradeRewardXDM";}
        }
    }

    public partial class XTable
    {
        public static ActivityGradeRewardXTable ActivityGradeRewardXTable = new ActivityGradeRewardXTable();
    }
}
