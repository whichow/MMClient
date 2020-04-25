// Auto Generated Code
using System.Collections;
using System.Collections.Generic;

namespace Game.DataModel
{
    public partial class MissionXDM : IXDM
    {
        /// <summary>
        /// id
        /// </summary>
        public int ID { get; protected set; }
        /// <summary>
        /// 标题
        /// </summary>
        public int Title { get; protected set; }
        /// <summary>
        /// 描述
        /// </summary>
        public int DescriptionId { get; protected set; }
        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; protected set; }
        /// <summary>
        /// 等级下限
        /// </summary>
        public int MinLevel { get; protected set; }
        /// <summary>
        /// 等级上限
        /// </summary>
        public int Maxlevel { get; protected set; }
        /// <summary>
        /// 事件id  101=完成所有每日任务  102=挑战任意关卡【不论胜负】  103=挑战周赛【不论胜负】  104=完成任意类型探索任务【不论胜负】  105=完成任意装饰物打造  106=喂养任意猫咪  107=寄养任意猫咪并收取寄养经验  108=赠送任意好友友情点  109=拜访任意好友家园【不同好友】  201=通关指定关卡章节                   <参数>指定章节号  202=收集消除星星【历史收集数】          <参数>星星数量  203=成功完成任意探索任务【只计算成功，历史总数】  204=打造出指定品阶的装饰物【历史收取数】 <参数>装饰物品阶  205=收集指定品阶猫咪【历史收集总量】     <参数>猫咪品阶  206=将任意猫咪升至指定等级【历史最高等级】<参数>猫咪等级  207=将任意猫咪升至指定星级【历史最高星级】<参数>星级  208=将任一猫咪技能升至指定等级【历史最高技能等级】<参数>技能等级  209=魅力值【历史最高】                   <参数>魅力值  210=获赞数【历史最高】                   <参数>获赞数  211=偷取任意好友的宝箱【历史总数】
        /// </summary>
        public int EventId { get; protected set; }
        /// <summary>
        /// 事件参数
        /// </summary>
        public int EventParam { get; protected set; }
        /// <summary>
        /// 完成次数
        /// </summary>
        public int CompleteNum { get; protected set; }
        /// <summary>
        /// 前置任务
        /// </summary>
        public int Prev { get; protected set; }
        /// <summary>
        /// 后置任务
        /// </summary>
        public int Next { get; protected set; }
        /// <summary>
        /// 奖励经验
        /// </summary>
        public int Exp { get; protected set; }
        /// <summary>
        /// 奖励
        /// </summary>
        public List<int> Reward { get; protected set; }
        /// <summary>
        /// 引导界面
        /// </summary>
        public int Ruide { get; protected set; }

        public void Parse(Hashtable table)
        {
            ID = table.GetInt("ID");
            Title = table.GetInt("Title");
            DescriptionId = table.GetInt("DescriptionId");
            Type = table.GetInt("Type");
            MinLevel = table.GetInt("MinLevel");
            Maxlevel = table.GetInt("Maxlevel");
            EventId = table.GetInt("EventId");
            EventParam = table.GetInt("EventParam");
            CompleteNum = table.GetInt("CompleteNum");
            Prev = table.GetInt("Prev");
            Next = table.GetInt("Next");
            Exp = table.GetInt("Exp");
            Reward = table.GetIntList("Reward");
            Ruide = table.GetInt("Ruide");
        }
    }

    public partial class MissionXTable : XTable<MissionXDM>
    {
        public override string ResourceName
        {
            get { return "MissionXDM";}
        }
    }

    public partial class XTable
    {
        public static MissionXTable MissionXTable = new MissionXTable();
    }
}
