// Auto Generated Code
using System.Collections;
using System.Collections.Generic;

namespace Game.DataModel
{
    public partial class GuideXDM : IXDM
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; protected set; }
        /// <summary>
        /// 步骤列表
        /// </summary>
        public List<int> Steps { get; protected set; }
        /// <summary>
        /// 触发的引导ID
        /// </summary>
        public int TriggerGuideID { get; protected set; }
        /// <summary>
        /// 触发的引导步骤
        /// </summary>
        public int TriggerStep { get; protected set; }

        public void Parse(Hashtable table)
        {
            ID = table.GetInt("ID");
            Steps = table.GetIntList("Steps");
            TriggerGuideID = table.GetInt("TriggerGuideID");
            TriggerStep = table.GetInt("TriggerStep");

            ParseEx();
        }

        partial void ParseEx();

    }

    public partial class GuideXTable : XTable<GuideXDM>
    {
        public override string ResourceName
        {
            get { return "GuideXDM";}
        }
    }

    public partial class XTable
    {
        public static GuideXTable GuideXTable = new GuideXTable();
    }
}
