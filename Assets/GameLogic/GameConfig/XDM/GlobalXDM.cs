// Auto Generated Code
using System.Collections;
using System.Collections.Generic;

namespace Game.DataModel
{
    public partial class GlobalXDM : IXDM
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; protected set; }
        /// <summary>
        /// 内容
        /// </summary>
        public int intVal { get; protected set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string stringVal { get; protected set; }
        /// <summary>
        /// 内容
        /// </summary>
        public List<string> stringListVal { get; protected set; }
        /// <summary>
        /// 内容
        /// </summary>
        public List<int> intListVal { get; protected set; }

        public void Parse(Hashtable table)
        {
            ID = table.GetInt("ID");
            intVal = table.GetInt("intVal");
            stringVal = table.GetString("stringVal");
            stringListVal = table.GetStringList("stringListVal");
            intListVal = table.GetIntList("intListVal");

            ParseEx();
        }

        partial void ParseEx();

    }

    public partial class GlobalXTable : XTable<GlobalXDM>
    {
        public override string ResourceName
        {
            get { return "GlobalXDM";}
        }
    }

    public partial class XTable
    {
        public static GlobalXTable GlobalXTable = new GlobalXTable();
    }
}
