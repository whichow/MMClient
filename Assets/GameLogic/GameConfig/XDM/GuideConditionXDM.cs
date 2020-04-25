// Auto Generated Code
using System.Collections;
using System.Collections.Generic;

namespace Game.DataModel
{
    public partial class GuideConditionXDM : IXDM
    {
        /// <summary>
        /// 
        /// </summary>
        public int ID { get; protected set; }
        /// <summary>
        /// 1首次进入某场景  2等级达到某级  3完成某步骤  4首次进入某关卡（选猫界面）  5章节界面  6关卡中（关卡目标展示过后）  7关卡结算流程前（达成通关条件）  8关卡结算界面  9进入某场景  10场景中拥有某建筑时  11进入某关的选猫界面  12棋盘稳定  13某种作物种植后  14某种作物种植可收割时  15获得某种道具时触发  16关闭视频后触发  17主界面折叠按钮是否展开  18是否关闭所有界面  19是否有猫舍  20城建中建筑刷新稳定  22建筑建造完成（倒计时为0）  23手工作坊打造完成（倒计时为0）  26开始探索成功  27建筑建完启用成功  28放置猫成功  29配方兑换成功
        /// </summary>
        public int Type { get; protected set; }
        /// <summary>
        /// 类型1 目标场景ID  类型2 填写达到的等级  类型3 填写stageId  类型4 目标关卡ID  类型5 填写章节ID  类型6 目标关卡ID  类型7 目标关卡ID  类型8 目标关卡ID  类型9 目标场景ID  类型10 填写建筑ID  类型13 填写作物ID  类型14 填写作物ID  类型15 填写道具ID  类型18 为1时关闭  类型19 填写猫舍ID  
        /// </summary>
        public int Target { get; protected set; }

        public void Parse(Hashtable table)
        {
            ID = table.GetInt("ID");
            Type = table.GetInt("Type");
            Target = table.GetInt("Target");

            ParseEx();
        }

        partial void ParseEx();

    }

    public partial class GuideConditionXTable : XTable<GuideConditionXDM>
    {
        public override string ResourceName
        {
            get { return "GuideConditionXDM";}
        }
    }

    public partial class XTable
    {
        public static GuideConditionXTable GuideConditionXTable = new GuideConditionXTable();
    }
}
