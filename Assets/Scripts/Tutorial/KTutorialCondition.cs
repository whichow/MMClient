//// ***********************************************************************
//// Assembly         : Unity
//// Author           : Kimch
//// Created          : #DATA#
////
//// Last Modified By : Kimch
//// Last Modified On : 
//// ***********************************************************************
//// <copyright file= "KTutorialCondition" company=""></copyright>
//// <summary></summary>
//// ***********************************************************************
//namespace Game
//{
//    using Build;
//    using Game.Match3;
//    using System.Collections;
//    using UI;
//    using UnityEngine;

//    public class KTutorialCondition
//    {
//        public enum Type
//        {
//            kNone = 0,
//            kGrade = 2,//等级
//            kStage = 3,
//            kM3Levle = 4,
//            KChapter = 5,
//            kInM3Chapter = 6,
//            kInM3ChapterOver = 7,
//            kInM3ChapterOverLater = 8,
//            kInBuildingbyId = 10,
//            kInM3ChoooseCat = 11,
//            kInM3Anmitio = 12,
//            kInBuildingFramSowing = 13,
//            kInBuildingFramHarvest = 14,
//            kHaveItem = 15,
//            kMainWindowBtn = 17,
//            kMainWindowEnable = 18,
//            kCatHoser = 19,
//            kBuildingReady = 20,
//            kHaveName = 21,
//        }

//        /// <summary>
//        /// 根据类型创建条件(重载类用)
//        /// </summary>
//        /// <param name="type"></param>
//        /// <returns></returns>
//        public static KTutorialCondition CreateCondition(int type)
//        {
//            var ret = new KTutorialCondition();
//            ret.type = type;
//            return ret;
//        }

//        #region Property

//        public int id
//        {
//            get;
//            private set;
//        }

//        public int type
//        {
//            get;
//            private set;
//        }

//        public int target
//        {
//            get;
//            private set;
//        }

//        /// <summary>
//        /// 参数
//        /// </summary>
//        public string param
//        {
//            get;
//            private set;
//        }

//        #endregion

//        #region Method

//        /// <summary>
//        /// 获取结果
//        /// </summary>
//        /// <returns></returns>
//        public virtual bool GetResult()
//        {

//            switch ((Type)type)
//            {
//                case Type.kGrade:
//                    return KUser.SelfPlayer.grade >= target;
//                case Type.kStage:

//                    return KTutorialManager.Instance.GetStage(target).completed;
//                case Type.KChapter:

//                    return KLevelManager.Instance.GetChapterByChapterID(target).unlocked;
//                case Type.kM3Levle:

//                    return KLevelManager.Instance.GetLevelById(target).unlocked;

//                case Type.kInM3Chapter:

//                    return M3GameManager.Instance.isReady && M3GameManager.Instance.isGoalSignFinish && KLevelManager.Instance.currLevelID == target;

//                case Type.kInM3ChapterOver:

//                    return M3GameManager.Instance.isGameOver && KLevelManager.Instance.currLevelID == target && M3GameManager.Instance.modeManager.IsLevelFinish();
//                case Type.kInM3ChapterOverLater:
//                    var window = KUIWindow.GetWindow<GameOverWindow>();
//                    if (window != null && KLevelManager.Instance.currLevelID == target)
//                    {
//                        if (window.active)
//                        {
//                            return true;
//                        }
//                    }
//                    return false;
//                case Type.kInBuildingbyId:

//                    return BuildingManager.Instance.isExistBuilding(target);
//                case Type.kInM3ChoooseCat:

//                    return KLevelManager.Instance.GetLevelById(target).unlocked;

//                case Type.kInM3Anmitio:

//                    return M3GameManager.Instance.gameFsm.GetFSM().GetCurrentStateEnum() == Game.Match3.StateEnum.Idle;
//                case Type.kInBuildingFramHarvest:

//                    return BuildingManager.Instance.isFarmHarvest(target);
//                case Type.kInBuildingFramSowing:

//                    return BuildingManager.Instance.isFarmSowing(target);

//                case Type.kHaveItem:
//                    return KItemManager.Instance.GetItem(target).curCount > 0 ? true : false;
//                case Type.kMainWindowBtn:
//                    var mainWindow = KUIWindow.GetWindow<MainWindow>();
//                    if (mainWindow != null)
//                    {

//                        return mainWindow.isOpen;
//                    }
//                    return false;
//                case Type.kMainWindowEnable:

//                    return KUIWindow.GetActiveWindowsCount() <= 1;
//                case Type.kCatHoser:

//                    return KCattery.Instance.IsCatteryCompleted(target);
//                case Type.kBuildingReady:

//                    return BuildingManager.Instance.isCreateFinish;
//                case Type.kHaveName:

//                    if (!string.IsNullOrEmpty(KUser.SelfPlayer.nickName))
//                    {
//                        return true;
//                    }
//                    return false;
//            }
//            return false;
//        }

//        public void Load(Hashtable table)
//        {
//            id = table.GetInt("Id");
//            type = table.GetInt("Type");
//            target = table.GetInt("Target");
//            param = table.GetString("Param");
//        }

//        #endregion
//    }
//}
