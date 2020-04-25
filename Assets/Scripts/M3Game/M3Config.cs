namespace Game.Match3
{
    public class M3Config
    {
        public static bool isEditor = false;
        public static KCat editorCat;

        public static int[] gridBottomBlankOffset = new int[] { 0, -1, 1 };
        public static int GridWidth = 10;
        public static int GridHeight = 8;
        public static int GridWidthIndex = 9;
        public static int GridHeightIndex = 7;

        public static int RealFirstRow;
        public static int RealLastRow;
        public static int RealFirsCol;
        public static int RealLastCol;

        public static int RealWidth;
        public static int RealHeight;

        public static float DistancePerUnit = 1f;

        public static float MaskRateX = 0.8f;//遮罩大小
        public static float MaskRateY = 0.8f;//遮罩大小
        public static float MaskRateZ = 0.8f;//遮罩大小


        private static float itemMoveTime = 0.2f;
        public static float FishMoveTim3 = 0.4f;
        public static float ZombieMoveTime = 0.4f;
        //public static float ItemBoomDisappearTime = 0.2f;
        public static float refreshTime = 0.6f;
        public static int refreshWaitFrame = 40;
        public static float ConveyorTime = 0.6f;

        public static string levelId = "";
        public static int BaseElement = 1000;
        public static int SpecialElement = 2000;
        public static int ColorCount = 6;
        public static int levelIndex;


        private static int normalElementDisapperFrame = 15;//元素销毁持续的帧数
        private static int elementDisapperFrame = 30;//元素销毁持续的帧数
        private static int magicCatWaitFrame = 60;//魔力猫等待的帧数

        public static float ElementDisapperTime = 0.5f;//元素销毁持续的帧数

        private static int lineTOColorWaitFrame = 60;
        private static int elementLandDelayFrame = 30;

        private static int coomJumpFrame = 13;//毛球跳跃的时间
        public static int brownCoomFenlieFrame = 18;//毛球分裂时间

        private static int venomFenlieFrame = 20;//毒液分裂时间

        //Speed
        private static float dropInitialSpeed = 8;//掉落初始速度
        private static float dropAcceleratedSpeed = 15;//掉落加速度
        private static float dropSpeedMax = 12f;//掉落最大速度

        public static float SkillFlySpeed = 16f;

        private static float inclinedDropTime = 0.18f;/// 
        public static float InclinedDropTimeCycle = 0.009f;/// 
        public static float PieceDropSpeedCycle = 0.4f;
        public static float PieceDropAccelerateCycle = 0.75f;
        public static float DropSpeedMaxCycle = 0.6f;

        public static float ItemMoveTime
        {
            get
            {
                if (M3GameManager.Instance.isAutoAI)
                    return 0;
                return itemMoveTime;
            }

            set
            {
                itemMoveTime = value;
            }
        }

        public static float DropInitialSpeed
        {
            get
            {
                if (M3GameManager.Instance.isAutoAI)
                    return 99;
                return dropInitialSpeed;
            }

            set
            {

                dropInitialSpeed = value;
            }
        }

        public static float DropAcceleratedSpeed
        {
            get
            {
                if (M3GameManager.Instance.isAutoAI)
                    return 99;
                return dropAcceleratedSpeed;
            }

            set
            {
                dropAcceleratedSpeed = value;
            }
        }

        public static float DropSpeedMax
        {
            get
            {
                if (M3GameManager.Instance.isAutoAI)
                    return 99;
                return dropSpeedMax;
            }

            set
            {
                dropSpeedMax = value;
            }
        }

        public static int NormalElementDisapperFrame
        {
            get
            {
                if (M3GameManager.Instance.isAutoAI)
                    return 1;
                return normalElementDisapperFrame;
            }

            set
            {
                normalElementDisapperFrame = value;
            }
        }

        public static float InclinedDropTime
        {
            get
            {
                if (M3GameManager.Instance.isAutoAI)
                    return 0;
                return inclinedDropTime;
            }

            set
            {
                inclinedDropTime = value;
            }
        }

        public static int MagicCatWaitFrame
        {
            get
            {
                if (M3GameManager.Instance.isAutoAI)
                    return 0;
                return magicCatWaitFrame;
            }

            set
            {
                magicCatWaitFrame = value;
            }
        }

        public static int LineTOColorWaitFrame
        {
            get
            {
                if (M3GameManager.Instance.isAutoAI)
                    return 0;
                return lineTOColorWaitFrame;
            }

            set
            {
                lineTOColorWaitFrame = value;
            }
        }

        public static int CoomJumpFrame
        {
            get
            {
                if (M3GameManager.Instance.isAutoAI)
                    return 0;
                return coomJumpFrame;
            }

            set
            {
                coomJumpFrame = value;
            }
        }

        public static int ElementDisapperFrame
        {
            get
            {
                if (M3GameManager.Instance.isAutoAI)
                    return 0;
                return elementDisapperFrame;
            }

            set
            {
                elementDisapperFrame = value;
            }
        }

        public static int VenomFenlieFrame
        {
            get
            {
                if (M3GameManager.Instance.isAutoAI)
                    return 0;
                return venomFenlieFrame;
            }

            set
            {
                venomFenlieFrame = value;
            }
        }

        public static int ElementLandDelayFrame
        {
            get
            {
                if (!M3GameManager.Instance.isAutoAI)
                    return 0;
                return elementLandDelayFrame;
            }

            set
            {
                elementLandDelayFrame = value;
            }
        }
    }
    public class M3EffectConfig
    {
        public const string ShakeItem = "ShakeHead";

        public const string ArrowEffectName = "FireArrow";

        public const string ItemCrashName = "crash";

        public const string ItemAreaEffect = "boom_flag";



        public static float ItemIdleAnimSpeed = 1;

        public static float ItemSelectAnimaSpeed = 1;

        public static float ItemLinkageAnimaSpeed = 1;

        public static float ItemBoomCrashSpeed = 2f;//爆炸星星播放速度
        public static float ItemBoomCrashDestroyTime = 0.5f;//爆炸销毁时间

        public static float ItemLineCrashSpeed = 1;//横竖特效播放速度
        public static float ItemLineCrashDestroyTime = 1;//横竖特效销毁时间

        public static float MagicScrollSpeed = 1;//魔法卷轴播放速度
        public static float MagicScrollDestroyTime = 1;//魔法卷轴销毁时间

    }
}