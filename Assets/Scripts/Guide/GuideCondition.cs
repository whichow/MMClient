/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/4/11 17:16:17
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using System;
using Game.Build;
using Game.DataModel;
using Game.Match3;
using Game.UI;

namespace Game
{
    public class GuideCondition
    {
        /// <summary>
        /// 条件类型
        /// </summary>
        public enum EConditionType
        {
            kNone = 0,

            /// <summary>
            /// 等级达到某级
            /// </summary>
            kGrade = 2,
            /// <summary>
            /// 完成某步骤
            /// </summary>
            kStep = 3,
            /// <summary>
            /// 首次进入某关卡（选猫界面）
            /// </summary>
            kM3Levle = 4,
            /// <summary>
            /// 章节界面
            /// </summary>
            KChapter = 5,
            /// <summary>
            /// 关卡中（关卡目标展示过后）
            /// </summary>
            kInM3Chapter = 6,
            /// <summary>
            /// 关卡结算流程前（达成通关条件）
            /// </summary>
            kInM3ChapterOver = 7,
            /// <summary>
            /// 关卡结算界面
            /// </summary>
            kInM3ChapterOverLater = 8,

            /// <summary>
            /// 场景中拥有某建筑时
            /// </summary>
            kInBuildingbyId = 10,
            /// <summary>
            /// 进入某关的选猫界面
            /// </summary>
            kInM3ChoooseCat = 11,
            /// <summary>
            /// 棋盘稳定
            /// </summary>
            kInM3Anmitio = 12,
            /// <summary>
            /// 某种作物种植后
            /// </summary>
            kInBuildingFramSowing = 13,
            /// <summary>
            /// 某种作物种植可收割时
            /// </summary>
            kInBuildingFramHarvest = 14,
            /// <summary>
            /// 获得某种道具时触发
            /// </summary>
            kHaveItem = 15,
            /// <summary>
            /// 主界面折叠按钮是否展开
            /// </summary>
            kMainWindowBtn = 17,
            /// <summary>
            /// 是否关闭所有界面
            /// </summary>
            kMainWindowEnable = 18,
            /// <summary>
            /// 是否有猫舍
            /// </summary>
            kCatHoser = 19,
            /// <summary>
            /// 城建中建筑刷新稳定
            /// </summary>
            kBuildingReady = 20,
            kHaveName = 21,
            /// <summary>
            /// 建筑建造完成（倒计时为0）
            /// </summary>
            kBuildingTimeOver = 22,
            /// <summary>
            /// 手工作坊打造完成（倒计时为0）
            /// </summary>
            kWorkShopMakeTimeOver = 23,

            
            /// <summary>
            /// 开始探索成功
            /// </summary>
            kExpeditionTaskStart = 26,
            /// <summary>
            /// 建筑建完启用成功
            /// </summary>
            kBuildingEnlabe = 27,
            /// <summary>
            /// 放置猫成功
            /// </summary>
            kCatHouseAddCat = 28,
            /// <summary>
            /// 配方兑换成功
            /// </summary>
            kExchangeBuildingFormula = 29,

        }

        #region Method

        /// <summary>
        /// 获取结果
        /// </summary>
        /// <returns></returns>
        public virtual bool GetResult(int id)
        {
            GuideConditionXDM xdm = XTable.GuideConditionXTable.GetByID(id);
            switch ((EConditionType)xdm.Type)
            {
                case EConditionType.kGrade:
                    return PlayerDataModel.Instance.mPlayerData.mLevel >= xdm.Target;
                case EConditionType.kStep:
                    //return KTutorialManager.Instance.GetStup(Target).Completed;
                    return true;
                case EConditionType.KChapter:
                    return XTable.ChapterUnlockXTable.GetByID(xdm.Target).unlocked;
                case EConditionType.kM3Levle:
                    return XTable.LevelXTable.GetByID(xdm.Target).Unlocked;
                case EConditionType.kInM3Chapter:
                    return M3GameManager.Instance && M3GameManager.Instance.isReady && M3GameManager.Instance.isGoalSignFinish && (xdm.Target == 0 || LevelDataModel.Instance.CurrLevelID == xdm.Target);
                case EConditionType.kInM3ChapterOver:
                    return M3GameManager.Instance && M3GameManager.Instance.isGameOver && (xdm.Target == 0 || LevelDataModel.Instance.CurrLevelID == xdm.Target) && M3GameManager.Instance.modeManager.IsLevelFinish();
                case EConditionType.kInM3ChapterOverLater:
                    var window = KUIWindow.GetWindow<GameOverWindow>();
                    return window != null && window.active && (xdm.Target == 0 || LevelDataModel.Instance.CurrLevelID == xdm.Target);
                case EConditionType.kInBuildingbyId:
                    return BuildingManager.Instance.isExistBuilding(xdm.Target);
                case EConditionType.kInM3ChoooseCat:
                    return XTable.LevelXTable.GetByID(xdm.Target).Unlocked;
                case EConditionType.kInM3Anmitio:
                    return M3GameManager.Instance && M3GameManager.Instance.gameFsm.GetFSM().GetCurrentStateEnum() == Game.Match3.StateEnum.Idle;
                case EConditionType.kInBuildingFramHarvest:
                    return BuildingManager.Instance.isFarmHarvest(xdm.Target);
                case EConditionType.kInBuildingFramSowing:
                    return BuildingManager.Instance.isFarmSowing(xdm.Target);
                case EConditionType.kHaveItem:
                    return KItemManager.Instance.GetItem(xdm.Target).curCount > 0 ? true : false;
                case EConditionType.kMainWindowBtn:
                    var mainWindow = KUIWindow.GetWindow<MainWindow>();
                    if (mainWindow != null)
                    {
                        return mainWindow.isOpen;
                    }
                    return false;
                case EConditionType.kMainWindowEnable:
                    return KUIWindow.GetActiveWindowsCount() <= 1;
                case EConditionType.kCatHoser:
                    return KCattery.Instance.IsCatteryCompleted(xdm.Target);
                case EConditionType.kBuildingReady:
                    return BuildingManager.Instance.isCreateFinish;
                case EConditionType.kHaveName:
                    if (!string.IsNullOrEmpty(PlayerDataModel.Instance.mPlayerData.mName))
                    {
                        return true;
                    }
                    break;
                case EConditionType.kBuildingTimeOver:
                    var b = BuildingManager.Instance.GetEntityOfCfgType(xdm.Target);
                    if (b && b.curState == Building.State.kIdle)
                    {
                        return true;
                    }
                    else if(b.curState == Building.State.kProduce) // 已经开启使用了
                    {
                        return true;
                    }
                    break;
                case EConditionType.kWorkShopMakeTimeOver:
                    var list = BuildingManager.Instance.GetEntities<BuildingManualWorkShop>();
                    if (list.Count > 0 && !list[0].supportSpeedUp)
                    {
                        return true;
                    }
                    break;
                case EConditionType.kExpeditionTaskStart:
                    return KExplore.Instance.CheckExploreingTask();
                case EConditionType.kBuildingEnlabe:
                    var be = BuildingManager.Instance.GetEntityOfCfgType(xdm.Target);
                    return be && be.curState == Building.State.kProduce;
                case EConditionType.kCatHouseAddCat:
                    var cattery = KCattery.Instance.GetCatteryInfoByCfgID(xdm.Target);
                    return cattery != null && cattery.catIds.Length > 0;
                case EConditionType.kExchangeBuildingFormula:
                    var formula = KItemManager.Instance.GetFormula(xdm.Target);
                    return formula != null && formula.isHave;
            }

            return false;
        }

        #endregion

    }
}
