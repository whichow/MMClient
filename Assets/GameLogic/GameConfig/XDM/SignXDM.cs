// Auto Generated Code
using System.Collections;
using System.Collections.Generic;

namespace Game.DataModel
{
    public partial class SignXDM : IXDM
    {
        /// <summary>
        /// 总索引
        /// </summary>
        public int ID { get; protected set; }
        /// <summary>
        /// 编号ID
        /// </summary>
        public int Group { get; protected set; }
        /// <summary>
        /// 组内索引
        /// </summary>
        public int GroupIndex { get; protected set; }
        /// <summary>
        /// 奖励
        /// </summary>
        public List<int> Reward { get; protected set; }

        public void Parse(Hashtable table)
        {
            ID = table.GetInt("ID");
            Group = table.GetInt("Group");
            GroupIndex = table.GetInt("GroupIndex");
            Reward = table.GetIntList("Reward");

            ParseEx();
        }

        partial void ParseEx();

    }

    public partial class SignXTable : XTable<SignXDM>
    {
        public override string ResourceName
        {
            get { return "SignXDM";}
        }
    }

    public partial class XTable
    {
        public static SignXTable SignXTable = new SignXTable();
    }
}
