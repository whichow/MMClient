// Auto Generated Code
using System.Collections;
using System.Collections.Generic;

namespace Game.DataModel
{
    public partial class CatXDM : IXDM
    {
        /// <summary>
        /// 
        /// </summary>
        public int ID { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public string Model { get; protected set; }
        /// <summary>
        /// 全身像Icon
        /// </summary>
        public string Icon { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; protected set; }
        /// <summary>
        /// 主颜色
        /// </summary>
        public int MainColor { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public int Color { get; protected set; }
        /// <summary>
        /// 品阶
        /// </summary>
        public int Rarity { get; protected set; }
        /// <summary>
        /// 碎片ID，合成数量  根据品阶来决定合成数量
        /// </summary>
        public List<int> Piece { get; protected set; }
        /// <summary>
        /// 熔炼魂石价值
        /// </summary>
        public int Price { get; protected set; }
        /// <summary>
        /// 技能
        /// </summary>
        public int SkillId { get; protected set; }
        /// <summary>
        /// 玩家头像Icon
        /// </summary>
        public int AvatarId { get; protected set; }
        /// <summary>
        /// 全身像Icon
        /// </summary>
        public string Photo { get; protected set; }
        /// <summary>
        /// 产金能力  初始值范围（权重）
        /// </summary>
        public List<int> CoinAbilityRange { get; protected set; }
        /// <summary>
        /// 探索能力  初始值范围（权重）
        /// </summary>
        public List<int> ExploreAbilityRange { get; protected set; }
        /// <summary>
        /// 消除能力  初始值范围（权重）
        /// </summary>
        public List<int> MatchAbilityRange { get; protected set; }
        /// <summary>
        /// 暴击几率
        /// </summary>
        public List<int> CriticalChance { get; protected set; }
        /// <summary>
        /// 获取渠道
        /// </summary>
        public List<int> Getinformation { get; protected set; }
        /// <summary>
        /// 猫技能欧气
        /// </summary>
        public int SkillScore { get; protected set; }
        /// <summary>
        /// 音效前缀
        /// </summary>
        public string AudioPrefix { get; protected set; }

        public void Parse(Hashtable table)
        {
            ID = table.GetInt("ID");
            Name = table.GetString("Name");
            Model = table.GetString("Model");
            Icon = table.GetString("Icon");
            Description = table.GetString("Description");
            MainColor = table.GetInt("MainColor");
            Color = table.GetInt("Color");
            Rarity = table.GetInt("Rarity");
            Piece = table.GetIntList("Piece");
            Price = table.GetInt("Price");
            SkillId = table.GetInt("SkillId");
            AvatarId = table.GetInt("AvatarId");
            Photo = table.GetString("Photo");
            CoinAbilityRange = table.GetIntList("CoinAbilityRange");
            ExploreAbilityRange = table.GetIntList("ExploreAbilityRange");
            MatchAbilityRange = table.GetIntList("MatchAbilityRange");
            CriticalChance = table.GetIntList("CriticalChance");
            Getinformation = table.GetIntList("Getinformation");
            SkillScore = table.GetInt("SkillScore");
            AudioPrefix = table.GetString("AudioPrefix");

            ParseEx();
        }

        partial void ParseEx();

    }

    public partial class CatXTable : XTable<CatXDM>
    {
        public override string ResourceName
        {
            get { return "CatXDM";}
        }
    }

    public partial class XTable
    {
        public static CatXTable CatXTable = new CatXTable();
    }
}
