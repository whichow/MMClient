// Auto Generated Code
using System.Collections;
using System.Collections.Generic;

namespace Game.DataModel
{
    public partial class GuideStepXDM : IXDM
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; protected set; }
        /// <summary>
        /// 行为
        /// </summary>
        public int ActionID { get; protected set; }
        /// <summary>
        /// 触发条件
        /// </summary>
        public int ConditionID { get; protected set; }
        /// <summary>
        /// 关键步
        /// </summary>
        public bool IsKey { get; protected set; }

        public void Parse(Hashtable table)
        {
            ID = table.GetInt("ID");
            ActionID = table.GetInt("ActionID");
            ConditionID = table.GetInt("ConditionID");
            IsKey = table.GetInt("IsKey") == 1;

            ParseEx();
        }

        partial void ParseEx();

    }

    public partial class GuideStepXTable : XTable<GuideStepXDM>
    {
        public override string ResourceName
        {
            get { return "GuideStepXDM";}
        }
    }

    public partial class XTable
    {
        public static GuideStepXTable GuideStepXTable = new GuideStepXTable();
    }
}
