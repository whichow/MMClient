// Auto Generated Code
using System.Collections;
using System.Collections.Generic;

namespace Game.DataModel
{
    public partial class LevelXDM : IXDM
    {
        /// <summary>
        /// 关卡ID
        /// </summary>
        public int ID { get; protected set; }
        /// <summary>
        /// 关卡名
        /// </summary>
        public string Name { get; protected set; }
        /// <summary>
        /// 关卡索引
        /// </summary>
        public int LevelIndex { get; protected set; }
        /// <summary>
        /// 章节ID
        /// </summary>
        public int ChapterID { get; protected set; }
        /// <summary>
        /// 棋盘ID
        /// </summary>
        public string ChessboardID { get; protected set; }
        /// <summary>
        /// 坐标
        /// </summary>
        public List<int> Position { get; protected set; }
        /// <summary>
        /// 消耗体力
        /// </summary>
        public int NeedPower { get; protected set; }
        /// <summary>
        /// 忽略列表
        /// </summary>
        public List<int> IgnoreCatID { get; protected set; }
        /// <summary>
        /// 猫咪等级控制
        /// </summary>
        public int CatChoiceLevel { get; protected set; }
        /// <summary>
        /// 猫咪星级控制
        /// </summary>
        public int CatChoiceStar { get; protected set; }
        /// <summary>
        /// 消耗品选择
        /// </summary>
        public List<int> ChooseProp { get; protected set; }
        /// <summary>
        /// 战前商品ID
        /// </summary>
        public List<int> ChooseShopId { get; protected set; }
        /// <summary>
        /// 战斗中消耗
        /// </summary>
        public List<int> BattleProp { get; protected set; }
        /// <summary>
        /// 战斗商品ID
        /// </summary>
        public List<int> BattleShopId { get; protected set; }
        /// <summary>
        /// 解锁关卡
        /// </summary>
        public int NextLevelID { get; protected set; }
        /// <summary>
        /// 金币奖励基数
        /// </summary>
        public int CoinReward { get; protected set; }

        public void Parse(Hashtable table)
        {
            ID = table.GetInt("ID");
            Name = table.GetString("Name");
            LevelIndex = table.GetInt("LevelIndex");
            ChapterID = table.GetInt("ChapterID");
            ChessboardID = table.GetString("ChessboardID");
            Position = table.GetIntList("Position");
            NeedPower = table.GetInt("NeedPower");
            IgnoreCatID = table.GetIntList("IgnoreCatID");
            CatChoiceLevel = table.GetInt("CatChoiceLevel");
            CatChoiceStar = table.GetInt("CatChoiceStar");
            ChooseProp = table.GetIntList("ChooseProp");
            ChooseShopId = table.GetIntList("ChooseShopId");
            BattleProp = table.GetIntList("BattleProp");
            BattleShopId = table.GetIntList("BattleShopId");
            NextLevelID = table.GetInt("NextLevelID");
            CoinReward = table.GetInt("CoinReward");

            ParseEx();
        }

        partial void ParseEx();

    }

    public partial class LevelXTable : XTable<LevelXDM>
    {
        public override string ResourceName
        {
            get { return "LevelXDM";}
        }
    }

    public partial class XTable
    {
        public static LevelXTable LevelXTable = new LevelXTable();
    }
}
