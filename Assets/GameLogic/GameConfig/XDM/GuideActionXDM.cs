// Auto Generated Code
using System.Collections;
using System.Collections.Generic;

namespace Game.DataModel
{
    public partial class GuideActionXDM : IXDM
    {
        /// <summary>
        /// 
        /// </summary>
        public int ID { get; protected set; }
        /// <summary>
        /// 1=按钮（强，弱）（箭头）小猫  2=文字对话  3=漫画动画  4=弹改名界面  5=聚焦（建筑）  6=三消（手指）  7=猫咪巴拉 大猫  8=播放动画  9=功能解锁
        /// </summary>
        public int Type { get; protected set; }
        /// <summary>
        /// 对话列表
        /// </summary>
        public List<int> Dialogs { get; protected set; }
        /// <summary>
        /// 0 = 旁白（不显示姓名）  1 = 我（显示角色名）  2 = 白公主
        /// </summary>
        public List<int> Speaker { get; protected set; }
        /// <summary>
        /// 猫咪的方向  1=左  2=右
        /// </summary>
        public int ModelType { get; protected set; }
        /// <summary>
        /// 遮罩透明度  1=不透明  0=透明
        /// </summary>
        public int IsMask { get; protected set; }
        /// <summary>
        /// 对话中动画索引
        /// </summary>
        public int LevelImage { get; protected set; }
        /// <summary>
        /// 三消中说话  （隐藏遮罩背景）
        /// </summary>
        public int LevelTalkingType { get; protected set; }
        /// <summary>
        /// 挖孔1坐标  (中心点0,0)
        /// </summary>
        public List<float> Hole1Pos { get; protected set; }
        /// <summary>
        /// 挖孔1大小
        /// </summary>
        public List<float> Hole1Size { get; protected set; }
        /// <summary>
        /// 挖孔2坐标  (中心点0,0)
        /// </summary>
        public List<float> Hole2Pos { get; protected set; }
        /// <summary>
        /// 挖孔2大小
        /// </summary>
        public List<float> Hole2Size { get; protected set; }
        /// <summary>
        /// 手指/箭头动画起始位置
        /// </summary>
        public List<float> HandStartPosition { get; protected set; }
        /// <summary>
        /// 手指/箭头动画终点位置
        /// </summary>
        public List<float> HandEndPosition { get; protected set; }
        /// <summary>
        /// 手指动画缓动时间
        /// </summary>
        public float HandTime { get; protected set; }
        /// <summary>
        /// 打开某窗口  1=FunctionWindow  2=LevelInfoWindow  3=MapSelectWindow  4=BagWindow  5=CatBagWindow  6=AreaUnlockBox  7=PhotoShopPickCardHighWindow  8=PhotoGotBuild  9=DiscoveryWindow  10=FormulaShopWindow  11=PhotoShopWindow  12=OrnamentShopWindow  13=ChooseCatWindow  14=BuildingShopWindow  15=DiscoveryMissionWindow  16=MessageBox  17=GameOverWindow
        /// </summary>
        public int Window { get; protected set; }
        /// <summary>
        /// 完成条件
        /// </summary>
        public int CompleteCondition { get; protected set; }
        /// <summary>
        /// 聚焦功能建筑  填写建筑ID
        /// </summary>
        public int FunctionID { get; protected set; }
        /// <summary>
        /// 强引聚焦
        /// </summary>
        public string ButtonName { get; protected set; }
        /// <summary>
        /// 1=建筑（点击抬起坐标位置必须在Dpi内）  2=按钮  3=建筑包裹Item
        /// </summary>
        public int ButtonType { get; protected set; }
        /// <summary>
        /// 建造建筑的网络坐标Int2
        /// </summary>
        public List<int> BuildingPos { get; protected set; }
        /// <summary>
        /// 三消交换位
        /// </summary>
        public ArrayList Change { get; protected set; }
        /// <summary>
        /// 是否暂停三消  1=暂停  0=不暂停
        /// </summary>
        public int M3Stop { get; protected set; }
        /// <summary>
        /// 是否锁住棋盘  1 = 锁  0 = 不锁
        /// </summary>
        public int InM3Lock { get; protected set; }
        /// <summary>
        /// 是否是关卡通关前的步骤  1=开始  2=结算界面前
        /// </summary>
        public int IsGameOverType { get; protected set; }
        /// <summary>
        /// 三消引导鼠标抬起直接完成  0=不是  1=是
        /// </summary>
        public int M3FromBuiding { get; protected set; }
        /// <summary>
        /// 主界面菜单是否展开
        /// </summary>
        public int ExtendMenu { get; protected set; }
        /// <summary>
        /// 触发当前步时，是否隐藏所有功能栏  1=关闭  0=不关闭
        /// </summary>
        public int CloseUi { get; protected set; }
        /// <summary>
        /// 1 = 背包  2 = 猫咪背包  3 = 商店  4 = 建筑打造  5 = 图鉴  6 = 任务  7 = 好友  8 = 抽卡  9 = 树洞  10 = 邮件  11 = 活动  12 = 排行榜
        /// </summary>
        public List<int> LockButton { get; protected set; }
        /// <summary>
        /// 解锁功能  0=全部开
        /// </summary>
        public List<int> UnlockButton { get; protected set; }
        /// <summary>
        /// 1打开黑背景窗口  2关闭黑背景窗口
        /// </summary>
        public int MaskType { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public List<int> Animation { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public string Movie { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public string Sound { get; protected set; }
        /// <summary>
        /// 将ui界面移动到指定位置，使城建物体居中
        /// </summary>
        public List<float> Vidicon { get; protected set; }
        /// <summary>
        /// 聚焦区域
        /// </summary>
        public int FunctionArea { get; protected set; }
        /// <summary>
        /// 填写建筑ID  场上有某建筑时跳过当前action
        /// </summary>
        public List<int> Jump { get; protected set; }
        /// <summary>
        /// 三消高亮格
        /// </summary>
        public List<Hashtable> Prominent { get; protected set; }

        public void Parse(Hashtable table)
        {
            ID = table.GetInt("ID");
            Type = table.GetInt("Type");
            Dialogs = table.GetIntList("Dialogs");
            Speaker = table.GetIntList("Speaker");
            ModelType = table.GetInt("ModelType");
            IsMask = table.GetInt("IsMask");
            LevelImage = table.GetInt("LevelImage");
            LevelTalkingType = table.GetInt("LevelTalkingType");
            Hole1Pos = table.GetFloatList("Hole1Pos");
            Hole1Size = table.GetFloatList("Hole1Size");
            Hole2Pos = table.GetFloatList("Hole2Pos");
            Hole2Size = table.GetFloatList("Hole2Size");
            HandStartPosition = table.GetFloatList("HandStartPosition");
            HandEndPosition = table.GetFloatList("HandEndPosition");
            HandTime = table.GetFloat("HandTime");
            Window = table.GetInt("Window");
            CompleteCondition = table.GetInt("CompleteCondition");
            FunctionID = table.GetInt("FunctionID");
            ButtonName = table.GetString("ButtonName");
            ButtonType = table.GetInt("ButtonType");
            BuildingPos = table.GetIntList("BuildingPos");
            Change = table.GetArrayList("Change");
            M3Stop = table.GetInt("M3Stop");
            InM3Lock = table.GetInt("InM3Lock");
            IsGameOverType = table.GetInt("IsGameOverType");
            M3FromBuiding = table.GetInt("M3FromBuiding");
            ExtendMenu = table.GetInt("ExtendMenu");
            CloseUi = table.GetInt("CloseUi");
            LockButton = table.GetIntList("LockButton");
            UnlockButton = table.GetIntList("UnlockButton");
            MaskType = table.GetInt("MaskType");
            Animation = table.GetIntList("Animation");
            Movie = table.GetString("Movie");
            Sound = table.GetString("Sound");
            Vidicon = table.GetFloatList("Vidicon");
            FunctionArea = table.GetInt("FunctionArea");
            Jump = table.GetIntList("Jump");
            Prominent = table.GetHashtableList("Prominent");

            ParseEx();
        }

        partial void ParseEx();

    }

    public partial class GuideActionXTable : XTable<GuideActionXDM>
    {
        public override string ResourceName
        {
            get { return "GuideActionXDM";}
        }
    }

    public partial class XTable
    {
        public static GuideActionXTable GuideActionXTable = new GuideActionXTable();
    }
}
