// Auto Generated Code
using System.Collections;
using System.Collections.Generic;

namespace Game.DataModel
{
    public partial class ActivityXDM : IXDM
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; protected set; }
        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; protected set; }
        /// <summary>
        /// 参数
        /// </summary>
        public List<int> ActivityParam { get; protected set; }
        /// <summary>
        /// 奖励
        /// </summary>
        public List<int> Rewards { get; protected set; }
        /// <summary>
        /// 活动标题
        /// </summary>
        public int Title { get; protected set; }
        /// <summary>
        /// 描述
        /// </summary>
        public int Description { get; protected set; }
        /// <summary>
        /// 开启时间
        /// </summary>
        public List<int> StartTime { get; protected set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public List<int> EndTime { get; protected set; }

        public void Parse(Hashtable table)
        {
            ID = table.GetInt("ID");
            Type = table.GetInt("Type");
            ActivityParam = table.GetIntList("ActivityParam");
            Rewards = table.GetIntList("Rewards");
            Title = table.GetInt("Title");
            Description = table.GetInt("Description");
            StartTime = table.GetIntList("StartTime");
            EndTime = table.GetIntList("EndTime");

            ParseEx();
        }

        partial void ParseEx();

    }

    public partial class ActivityXTable : XTable<ActivityXDM>
    {
        public override string ResourceName
        {
            get { return "ActivityXDM";}
        }
    }

    public partial class XTable
    {
        public static ActivityXTable ActivityXTable = new ActivityXTable();
    }
}
