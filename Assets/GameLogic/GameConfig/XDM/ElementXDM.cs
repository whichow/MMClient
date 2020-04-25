// Auto Generated Code
using System.Collections;
using System.Collections.Generic;

namespace Game.DataModel
{
    public partial class ElementXDM : IXDM
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; protected set; }
        /// <summary>
        /// 名称(编辑器中显示)
        /// </summary>
        public string Name { get; protected set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; protected set; }
        /// <summary>
        /// 模型名字
        /// </summary>
        public string ModelName { get; protected set; }
        /// <summary>
        /// Spine动作
        /// </summary>
        public Hashtable Animations { get; protected set; }
        /// <summary>
        /// Spine皮肤
        /// </summary>
        public string Skin { get; protected set; }
        /// <summary>
        /// 待机动作
        /// </summary>
        public string IdleAnim { get; protected set; }
        /// <summary>
        /// 选中动作
        /// </summary>
        public string SelectAnim { get; protected set; }
        /// <summary>
        /// 消除动作
        /// </summary>
        public string ClearAnim { get; protected set; }
        /// <summary>
        /// 元素类型层级
        /// </summary>
        public int TypeLevel { get; protected set; }
        /// <summary>
        /// 100~n（数字越大层级越高）  100=最低层级，放最下面
        /// </summary>
        public int Level { get; protected set; }
        /// <summary>
        /// 1-普通，如红色  2-特效，如红色横特效  3-地面，在消除时此单元格会有事件触发，如冰块  4-绳索  5-传送带  6-不可选中，如猫窝、毒液源  7-障碍，如雪块等  8-覆盖，如牢笼、灰毛球  9-传送门  10-水晶球  11-礼物盒  12-能量块  13-魔力猫  89-银币  97-金豆荚  98-金豆荚出口  99-飞碟  98-金豆荚出口
        /// </summary>
        public int Type { get; protected set; }
        /// <summary>
        /// 是否移动
        /// </summary>
        public bool CanMove { get; protected set; }
        /// <summary>
        /// 是否下落
        /// </summary>
        public bool CanDrop { get; protected set; }
        /// <summary>
        /// 技能阻挡0-不阻挡  1-阻挡
        /// </summary>
        public bool CanResist { get; protected set; }
        /// <summary>
        /// 消除类型  0-不可消除  1-正常消除  2-技能消除  3-道具消除  4-内含消除  5-相邻消除  6-关联消除  7-步数消除  8-时间消除
        /// </summary>
        public List<int> ClearType { get; protected set; }
        /// <summary>
        /// 关联元素ID
        /// </summary>
        public int LinkID { get; protected set; }
        /// <summary>
        /// 消除事件  0-无事件  1-转化  2-横消  3-竖消  4-13消  5-全屏同色  6-回合暂停  7-被收集
        /// </summary>
        public List<int> ClearEvent { get; protected set; }
        /// <summary>
        /// 消除后转化ID
        /// </summary>
        public int ClearTransforID { get; protected set; }
        /// <summary>
        /// 跳跃类型  0.不跳跃  1.跳跃
        /// </summary>
        public bool JumpType { get; protected set; }
        /// <summary>
        /// 跳跃组
        /// </summary>
        public List<int> JumpGroup { get; protected set; }
        /// <summary>
        /// 跳跃间隔
        /// </summary>
        public int JumpSpace { get; protected set; }
        /// <summary>
        /// 叠层回合
        /// </summary>
        public int OverlyingRound { get; protected set; }
        /// <summary>
        /// 叠层上限
        /// </summary>
        public int OverlyingMaxNum { get; protected set; }
        /// <summary>
        /// 增加叠层方式  1.消除成功  2.0层消除掉（猫咪罐子）  3.常规方式消除掉（普通颜色）
        /// </summary>
        public int OverlyingAddType { get; protected set; }
        /// <summary>
        /// 叠层方式  0.不叠层  1.翻n倍  2.增加n层
        /// </summary>
        public int OverlyingType { get; protected set; }
        /// <summary>
        /// 消除叠层方式  1.满层消除掉（硬币）  2.0层消除掉（猫咪罐子）  3.常规方式消除掉（普通颜色）
        /// </summary>
        public int OverlyingClearType { get; protected set; }
        /// <summary>
        /// 叠加系数（翻n倍数、增加n层）
        /// </summary>
        public int OverlyingRatio { get; protected set; }
        /// <summary>
        /// 生成时获得积分
        /// </summary>
        public int Score { get; protected set; }
        /// <summary>
        /// 被消除类型（1,3,4,5,6）中任意一种消除时获得积分
        /// </summary>
        public int Point { get; protected set; }
        /// <summary>
        /// 颜色类型
        /// </summary>
        public int ColorType { get; protected set; }
        /// <summary>
        /// 绳索阻挡类型
        /// </summary>
        public int RopeType { get; protected set; }
        /// <summary>
        /// 收集元素目标
        /// </summary>
        public List<int> Collect { get; protected set; }
        /// <summary>
        /// 血量
        /// </summary>
        public int Health { get; protected set; }

        public void Parse(Hashtable table)
        {
            ID = table.GetInt("ID");
            Name = table.GetString("Name");
            Icon = table.GetString("Icon");
            ModelName = table.GetString("ModelName");
            Animations = table.GetTable("Animations");
            Skin = table.GetString("Skin");
            IdleAnim = table.GetString("IdleAnim");
            SelectAnim = table.GetString("SelectAnim");
            ClearAnim = table.GetString("ClearAnim");
            TypeLevel = table.GetInt("TypeLevel");
            Level = table.GetInt("Level");
            Type = table.GetInt("Type");
            CanMove = table.GetInt("CanMove") == 1;
            CanDrop = table.GetInt("CanDrop") == 1;
            CanResist = table.GetInt("CanResist") == 1;
            ClearType = table.GetIntList("ClearType");
            LinkID = table.GetInt("LinkID");
            ClearEvent = table.GetIntList("ClearEvent");
            ClearTransforID = table.GetInt("ClearTransforID");
            JumpType = table.GetInt("JumpType") == 1;
            JumpGroup = table.GetIntList("JumpGroup");
            JumpSpace = table.GetInt("JumpSpace");
            OverlyingRound = table.GetInt("OverlyingRound");
            OverlyingMaxNum = table.GetInt("OverlyingMaxNum");
            OverlyingAddType = table.GetInt("OverlyingAddType");
            OverlyingType = table.GetInt("OverlyingType");
            OverlyingClearType = table.GetInt("OverlyingClearType");
            OverlyingRatio = table.GetInt("OverlyingRatio");
            Score = table.GetInt("Score");
            Point = table.GetInt("Point");
            ColorType = table.GetInt("ColorType");
            RopeType = table.GetInt("RopeType");
            Collect = table.GetIntList("Collect");
            Health = table.GetInt("Health");

            ParseEx();
        }

        partial void ParseEx();

    }

    public partial class ElementXTable : XTable<ElementXDM>
    {
        public override string ResourceName
        {
            get { return "ElementXDM";}
        }
    }

    public partial class XTable
    {
        public static ElementXTable ElementXTable = new ElementXTable();
    }
}
