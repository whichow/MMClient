// Auto Generated Code
using System.Collections;
using System.Collections.Generic;

namespace Game.DataModel
{
    public partial class SkillXDM : IXDM
    {
        /// <summary>
        /// 技能ID
        /// </summary>
        public int ID { get; protected set; }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; protected set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; protected set; }
        /// <summary>
        /// 描述
        /// </summary>
        public int DescriptionId { get; protected set; }
        /// <summary>
        /// 射击描述
        /// </summary>
        public int ShortDescriptionId { get; protected set; }
        /// <summary>
        /// 1001-【需手动操作，棋盘上可操作性的元素为基础元素，其余元素加一层遮罩】使指定位置的普通元素变成相应猫的颜色，且变色了的猫拥有得分暴击能力（该猫被消除时，得分*N）  1002-主动释放技能，随机向当前棋盘内发散N个相应颜色猫  1003-释放技能后，消除所有当前棋盘的相应颜色基础猫，并有一定几率返还随机炸弹（点头、摇头、13消、活力猫）  1004-引爆棋盘中所有炸弹，并返还N个直线炸弹（4004黑公主）  1005-主动向棋盘中发散N个随机颜色的礼物盒（4010魔术师）  1006-释放技能后3个回合内/5秒内进入Super Time，得分变为N倍（4009提琴家）  1007-步数增加N步/时间增加N秒（4008白公主）  1008-魔力扫把（N行）（4007女巫）  1009-【需手动操作，棋盘上可操作的元素为基本元素+特效元素】发射交叉特效，直接引爆，并返还一定能量（4002女警）  1010-【需手动操作，棋盘上可操作的元素为基本元素+特效元素】发射L型/十字特效，直接引爆，并返还一定比例的能量（4005警官）  1011-【需手动操作，棋盘上可操作的元素为基本元素+特效元素】发射活力猫，直接引爆，并返还一定比例的能量（4003国王）  1012-向棋盘发射N个震荡波（优先级为雪块>冰块>附有能量的基础元素>暴击元素>水晶球>基本元素>特效）（4001舞蹈家）  1013-向棋盘发射N个导弹，随机砸中目标（飞碟>章鱼>牢笼>雪块>冰块>云块>宝藏>毒液>棕毛球>灰毛球>猫窝>银币>能量块>礼物>附有能量的基础元素>暴击元素>水晶球>基础元素>特效）（4006僵尸猫）  1014-散发特效元素
        /// </summary>
        public int Type { get; protected set; }
        /// <summary>
        /// 作用颜色类型  None = 0,  Green = 1,  Blue = 2,  Purple = 3,  Brown = 4,  Red = 5,  Yellow = 6,  Energy = 7,  Coin = 8,  CatColor = 90,  Random = 99,
        /// </summary>
        public int SkillColorType { get; protected set; }
        /// <summary>
        /// 特效元素  2一行（摇头）  3一列（点头）  4区域（13消）  5颜色（活力猫）
        /// </summary>
        public List<int> ElementSpecial { get; protected set; }
        /// <summary>
        /// 参数为等级对应下标
        /// </summary>
        public List<int> SkillParam { get; protected set; }
        /// <summary>
        /// 能量花费
        /// </summary>
        public int EnergyCost { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public Hashtable SkillEffect { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public Hashtable SkillEffectTime { get; protected set; }

        public void Parse(Hashtable table)
        {
            ID = table.GetInt("ID");
            Name = table.GetString("Name");
            Icon = table.GetString("Icon");
            DescriptionId = table.GetInt("DescriptionId");
            ShortDescriptionId = table.GetInt("ShortDescriptionId");
            Type = table.GetInt("Type");
            SkillColorType = table.GetInt("SkillColorType");
            ElementSpecial = table.GetIntList("ElementSpecial");
            SkillParam = table.GetIntList("SkillParam");
            EnergyCost = table.GetInt("EnergyCost");
            SkillEffect = table.GetTable("SkillEffect");
            SkillEffectTime = table.GetTable("SkillEffectTime");
        }
    }

    public partial class SkillXTable : XTable<SkillXDM>
    {
        public override string ResourceName
        {
            get { return "SkillXDM";}
        }
    }

    public partial class XTable
    {
        public static SkillXTable SkillXTable = new SkillXTable();
    }
}
