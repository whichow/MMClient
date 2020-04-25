// Auto Generated Code
using System.Collections;
using System.Collections.Generic;

namespace Game.DataModel
{
    public partial class ChapterUnlockXDM : IXDM
    {
        /// <summary>
        /// 章节ID
        /// </summary>
        public int ID { get; protected set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; protected set; }
        /// <summary>
        /// 章节解锁星星数
        /// </summary>
        public int UnlockStarNum { get; protected set; }
        /// <summary>
        /// 解锁对应章节
        /// </summary>
        public int UnlockChapter { get; protected set; }
        /// <summary>
        /// 章节解锁冷却时间（S）
        /// </summary>
        public int UnlockTime { get; protected set; }
        /// <summary>
        /// UIPrefabName
        /// </summary>
        public string UIPrefabName { get; protected set; }

        public void Parse(Hashtable table)
        {
            ID = table.GetInt("ID");
            Icon = table.GetString("Icon");
            UnlockStarNum = table.GetInt("UnlockStarNum");
            UnlockChapter = table.GetInt("UnlockChapter");
            UnlockTime = table.GetInt("UnlockTime");
            UIPrefabName = table.GetString("UIPrefabName");

            ParseEx();
        }

        partial void ParseEx();

    }

    public partial class ChapterUnlockXTable : XTable<ChapterUnlockXDM>
    {
        public override string ResourceName
        {
            get { return "ChapterUnlockXDM";}
        }
    }

    public partial class XTable
    {
        public static ChapterUnlockXTable ChapterUnlockXTable = new ChapterUnlockXTable();
    }
}
