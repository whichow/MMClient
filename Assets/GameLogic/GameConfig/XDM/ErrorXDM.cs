// Auto Generated Code
using System.Collections;
using System.Collections.Generic;

namespace Game.DataModel
{
    public partial class ErrorXDM : IXDM
    {
        /// <summary>
        /// 
        /// </summary>
        public int ID { get; protected set; }
        /// <summary>
        /// 操作类型  1=弹框  2=重试  3=
        /// </summary>
        public int Type { get; protected set; }
        /// <summary>
        /// 返回内容
        /// </summary>
        public int Message { get; protected set; }

        public void Parse(Hashtable table)
        {
            ID = table.GetInt("ID");
            Type = table.GetInt("Type");
            Message = table.GetInt("Message");

            ParseEx();
        }

        partial void ParseEx();

    }

    public partial class ErrorXTable : XTable<ErrorXDM>
    {
        public override string ResourceName
        {
            get { return "ErrorXDM";}
        }
    }

    public partial class XTable
    {
        public static ErrorXTable ErrorXTable = new ErrorXTable();
    }
}
