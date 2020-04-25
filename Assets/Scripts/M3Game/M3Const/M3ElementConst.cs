/** 
 *FileName:     M3ElementConst.cs 
 *Author:       HeJunJie 
 *Version:      1.0 
 *UnityVersion：5.6.2f1
 *Date:         2017-07-14 
 *Description:    
 *History: 
*/
using System.Collections.Generic;

namespace Game.Match3
{
    public class M3ElementType
    {
        public const int None = 0;
        /// <summary>
        /// 普通元素
        /// </summary>
        public const int NormalElement = 1;
        /// <summary>
        /// 特效元素，如红色横特效
        /// </summary>
        public const int SpecialElement = 2;
        /// <summary>
        /// 冰层元素
        /// </summary>
        public const int IceElement = 3;
        /// <summary>
        /// 绳索元素
        /// </summary>
        public const int RopeElement = 4;
        /// <summary>
        /// 传送元素，传送带
        /// </summary>
        public const int ConveyElement = 5;
        /// <summary>
        /// 猫舍元素，黄猫舍
        /// </summary>
        public const int CatteryElement = 6;
        /// <summary>
        /// 障碍元素，积雪，书，气球
        /// </summary>
        public const int MagicBookElement = 7;
        /// <summary>
        /// 牢笼
        /// </summary>
        public const int LockElement = 8;
        /// <summary>
        /// 水晶（可变元素）
        /// </summary>
        public const int CrystalElement = 10;
        /// <summary>
        /// 礼物盒
        /// </summary>
        public const int GiftElement = 11;
        /// <summary>
        /// 能量块
        /// </summary>
        public const int EnergyElement = 12;
        /// <summary>
        /// 魔力猫
        /// </summary>
        public const int MagicCatElement = 13;
        /// <summary>
        /// 灰毛球
        /// </summary>
        public const int GreyCoomElement = 14;
        /// <summary>
        /// 棕毛球
        /// </summary>
        public const int BrownCoomElement = 15;
        /// <summary>
        /// 毒液
        /// </summary>
        public const int VenomElement = 16;
        /// <summary>
        /// 毒液源
        /// </summary>
        public const int VenomParentElement = 17;
        /// <summary>
        /// 白云，窗帘
        /// </summary>
        public const int CurtainElement = 18;
        /// <summary>
        /// 宝藏
        /// </summary>
        public const int TreasureElement = 19;
        /// <summary>
        /// 隐藏元素
        /// </summary>
        public const int HiddenElement = 20;
        /// <summary>
        /// 铃铛
        /// </summary>
        public const int BellElement = 21;
        /// <summary>
        /// 雪怪
        /// </summary>
        public const int SnowMonster = 22;
        /// <summary>
        /// 草地
        /// </summary>
        public const int LawnElement = 23;
        /// <summary>
        /// 毛线球
        /// </summary>
        public const int WoolBall = 24;

        /// <summary>
        /// 银币
        /// </summary>
        public const int CoinElement = 89;
        /// <summary>
        /// 飞碟
        /// </summary>
        public const int UFOElement = 99;
        //public const int GoldExitElement = 98;//金豆荚
        /// <summary>
        /// 鲷鱼烧
        /// </summary>
        public const int FishElement = 97;

    }

    public class M3Const
    {
        public static int Red_NormalElement = 1001;
        public static int Yellow_NormalElement = 1002;
        public static int Blue_NormalElement = 1003;
        public static int Green_NormalElement = 1004;
        public static int Purple_NormalElement = 1005;
        public static int Brown_NormalElement = 1006;

        public static int Red_CrystalElement = 2020;
        public static int Yellow_CrystalElement = 2021;
        public static int Blue_CrystalElement = 2022;
        public static int Green_CrystalElement = 2023;
        public static int Purple_CrystalElement = 2024;
        public static int Brown_CrystalElement = 2025;

        public static int Red_GiftElement = 4036;
        public static int Yellow_GiftElement = 4037;
        public static int Blue_GiftElement = 4038;
        public static int Green_GiftElement = 4039;
        public static int Purple_GiftElement = 4040;
        public static int Brown_GiftElement = 4041;

        public static int FishElementID = 3052;


        public static int MagicCatGroupID = 118;


        private static int stateWaitFrame_1 = 40;
        private static int stateWaitFrame_2 = 60;
        private static int stateWaitFrame_3 = 100;


        /// <summary>
        /// 格子类型
        /// </summary>
        public static List<int> GRID_TYPE = new List<int>
        {
            M3ElementType.RopeElement,
            M3ElementType.ConveyElement,
            M3ElementType.IceElement,
            M3ElementType.LawnElement,
        };

        public static List<int> ITEM_TYPE = new List<int>
        {
            M3ElementType.NormalElement,
            M3ElementType.SpecialElement,
            M3ElementType.CatteryElement,
            M3ElementType.MagicBookElement,
            M3ElementType.LockElement,
            M3ElementType.CrystalElement,
            M3ElementType.GiftElement,
            M3ElementType.EnergyElement,
            M3ElementType.MagicCatElement,
            M3ElementType.CoinElement,
            M3ElementType.GreyCoomElement,
            M3ElementType.BrownCoomElement,
            M3ElementType.VenomElement,
            M3ElementType.VenomParentElement,
            M3ElementType.FishElement,
            M3ElementType.CurtainElement,
            M3ElementType.BellElement,
            M3ElementType.WoolBall
        };

        /// <summary>
        /// 爆炸范围
        /// </summary>
        public static Int2[] Boom41Array = new Int2[]
        {
            new Int2(0,0),
            new Int2(-1,0),
            new Int2(-1,1),
            new Int2(0,1),
            new Int2(1,1),
            new Int2(1,0),
            new Int2(1,-1),
            new Int2(0,-1),
            new Int2(-1,-1),

            new Int2(-2,0),
            new Int2(-2,1),
            new Int2(-2,2),
            new Int2(-1,2),
            new Int2(0,2),
            new Int2(1,2),
            new Int2(2,2),
            new Int2(2,1),

            new Int2(2,0),
            new Int2(2,-1),
            new Int2(2,-2),
            new Int2(1,-2),
            new Int2(0,-2),
            new Int2(-1,-2),
            new Int2(-2,-2),
            new Int2(-2,-1),

            new Int2(-3,-1),
            new Int2(-3,0),
            new Int2(-3,1),

            new Int2(-1,3),
            new Int2(0,3),
            new Int2(1,3),

            new Int2(3,1),
            new Int2(3,0),
            new Int2(3,-1),

            new Int2(1,-3),
            new Int2(0,-3),
            new Int2(-1,-3),

            new Int2(-4,0),
            new Int2(0,4),
            new Int2(4,0),
            new Int2(0,-4),
        };

        /// <summary>
        /// 方向偏移 0左 1下 2右 3 上
        /// </summary>
        public static Int2[] DirectionOffset = new Int2[]
        {
            new Int2(0, -1),    //0左
            new Int2(1, 0),     //1下
            new Int2(0, 1),     //2右
            new Int2(-1, 0)     //3上
        };

        /// <summary>
        /// 八方向 0左 1下 2右 3 上 4左下 5右下 6右上 7左上  坐标系是x向下，y向右
        /// </summary>
        public static Int2[] DirectionEightOffset = new Int2[]
        {
            new Int2(0, -1),
            new Int2(1, 0),
            new Int2(0, 1),
            new Int2(-1, 0),
            new Int2(1, -1),
            new Int2(1, 1),
            new Int2(-1, 1),
            new Int2(-1, -1)
        };

        /// <summary>
        /// 魔力猫可消除的类型
        /// </summary>
        public static List<int> MagicEliminateContains = new List<int>
        {
            M3ElementType.NormalElement,
            M3ElementType.SpecialElement,
            M3ElementType.GiftElement,
            M3ElementType.CrystalElement,
            M3ElementType.CoinElement,
            M3ElementType.EnergyElement,
        };

        /// <summary>
        /// 怪物动画名称
        /// </summary>
        public static string[] CoomDivisionAnimationKeys = new string[]
        {
            "DivisionUp",
            "DivisionDown",
            "DivisionLeft",
            "DivisionRight",
        };

        /// <summary>
        /// 猫窝动画名称
        /// </summary>
        public static string[] CatterAnimationKeys = new string[]
        {
            "Open1",
            "Open2",
            "Open3",
            "Open4",
            "Shake",
        };

        /// <summary>
        /// 毒液动画名称
        /// </summary>
        public static string[] VenomAnimationKeys = new string[]
        {
            "FenLie_Up",
            "FenLie_Down",
            "FenLie_Left",
            "FenLie_Right",
            "Create_Up",
            "Create_Down",
            "Create_Left",
            "Create_Right",
        };


        //***************************************************************************************************************************************
        public static List<int> CrystalChangeArr = new List<int>()
        {
            M3ElementType.LockElement,
        };

        public static List<int> HammerIngoreArr = new List<int>()
        {
            M3ElementType.FishElement,
            M3ElementType.VenomParentElement,
        };


        //***************************************************************************************************************************************


        public static int EnergyBottleValue = 10;
        public static int EnergtStepValue = 10;

        public static int HintMoveTime = 300;

        public static int StateWaitFrame_1
        {
            get
            {
                if (M3GameManager.Instance.isAutoAI)
                    return 0;
                return stateWaitFrame_1;
            }
            set
            {

                stateWaitFrame_1 = value;
            }
        }

        public static int StateWaitFrame_2
        {
            get
            {
                if (M3GameManager.Instance.isAutoAI)
                    return 0;
                return stateWaitFrame_2;
            }
            set
            {
                stateWaitFrame_2 = value;
            }
        }

        public static int StateWaitFrame_3
        {
            get
            {
                if (M3GameManager.Instance.isAutoAI)
                    return 0;
                return stateWaitFrame_3;
            }
            set
            {
                stateWaitFrame_3 = value;
            }
        }

        /// <summary>
        /// 阻挡草坪障碍类型
        /// </summary>
        public static List<int> ResistLawnObstacleType = new List<int>() {
            M3ElementType.RopeElement,
            M3ElementType.CatteryElement,
            M3ElementType.MagicBookElement,
            M3ElementType.LockElement,
            M3ElementType.GiftElement,
            M3ElementType.EnergyElement,
            M3ElementType.GreyCoomElement,
            M3ElementType.BrownCoomElement,
            M3ElementType.VenomElement,
            M3ElementType.VenomParentElement,
            M3ElementType.CurtainElement,
            M3ElementType.BellElement,
            M3ElementType.CoinElement,
            M3ElementType.FishElement
        };

        /// <summary>
        /// 阻挡毛线球的元素类型
        /// </summary>
        public static List<int> WoolBallResistType = new List<int>() {
            M3ElementType.RopeElement,
            M3ElementType.CatteryElement,
            M3ElementType.VenomParentElement,
            M3ElementType.WoolBall,
            M3ElementType.FishElement
        };

        /// <summary>
        /// 停留毛线球障碍类型
        /// </summary>
        public static List<int> WoolBallObstacleType = new List<int>() {
            M3ElementType.MagicBookElement,
            M3ElementType.LockElement,
            M3ElementType.GiftElement,
            M3ElementType.EnergyElement,
            M3ElementType.BrownCoomElement,
            M3ElementType.VenomElement,
            M3ElementType.CurtainElement,
            M3ElementType.CoinElement
        };

    }

    /// <summary>
    /// 特殊消除积分
    /// </summary>
    public class SpecialEliminateScore
    {
        /// <summary>
        /// 双直线
        /// </summary>
        public const int DoubleLine = 500;
        /// <summary>
        /// 炸弹与直线
        /// </summary>
        public const int BoomAndLine = 1000;
        /// <summary>
        /// 双炸弹
        /// </summary>
        public const int DoubleBoom = 1500;
        /// <summary>
        /// 魔力猫与直线
        /// </summary>
        public const int MagicAndLine = 2000;
        /// <summary>
        /// 魔力猫与炸弹
        /// </summary>
        public const int MagicAndBoom = 2500;
        /// <summary>
        /// 双魔力猫
        /// </summary>
        public const int DoubleMagic = 5000;
    }

    /// <summary>
    /// 消除积分
    /// </summary>
    public class ConstElementScore
    {
        /// <summary>
        /// 三消
        /// </summary>
        public const int score_Three = 30;
        /// <summary>
        /// 四消
        /// </summary>
        public const int score_Four = 50;
        /// <summary>
        /// 五消
        /// </summary>
        public const int score_Five = 70;
        /// <summary>
        /// 颜色消除
        /// </summary>
        public const int score_Color = 100;
        /// <summary>
        /// 额外奖励
        /// </summary>
        public const int score_bonus = 2500;
    }

}