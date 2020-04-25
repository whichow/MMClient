// Auto Generated Code
using System.Collections;
using System.Collections.Generic;

namespace Game.DataModel
{
    public partial class MailXDM : IXDM
    {
        /// <summary>
        /// 邮件ID
        /// </summary>
        public int ID { get; protected set; }
        /// <summary>
        /// 邮件类型
        /// </summary>
        public int MailType { get; protected set; }
        /// <summary>
        /// 邮件抬头
        /// </summary>
        public int MailTitleID { get; protected set; }
        /// <summary>
        /// 邮件内容
        /// </summary>
        public int MailContentID { get; protected set; }

        public void Parse(Hashtable table)
        {
            ID = table.GetInt("ID");
            MailType = table.GetInt("MailType");
            MailTitleID = table.GetInt("MailTitleID");
            MailContentID = table.GetInt("MailContentID");

            ParseEx();
        }

        partial void ParseEx();

    }

    public partial class MailXTable : XTable<MailXDM>
    {
        public override string ResourceName
        {
            get { return "MailXDM";}
        }
    }

    public partial class XTable
    {
        public static MailXTable MailXTable = new MailXTable();
    }
}
