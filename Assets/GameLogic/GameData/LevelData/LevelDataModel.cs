using Game.DataModel;
using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 关卡
    /// </summary>
    public class LevelDataModel : DataModelBase<LevelDataModel>
    {

        #region FIELD

        private Dictionary<int, int> m_levelStarDic = new Dictionary<int, int>();
        private Dictionary<int, int> m_levelScoreDic = new Dictionary<int, int>();

        private int _curUnlockChapterTime;

        #endregion

        #region PROPERTY
        public int userCurrentLevelID { get; set; }

        /// <summary>
        /// 当前章节ID
        /// </summary>
        public int CurrChapterID { get; set; }

        /// <summary>
        /// 当前关卡ID
        /// </summary>
        public int CurrLevelID { get; set; }

        /// <summary>
        /// 当前章节解锁配置数据
        /// </summary>
        public ChapterUnlockXDM CurrChapter
        {
            get { return XTable.ChapterUnlockXTable.GetByID(CurrChapterID); }
        }

        /// <summary>
        /// 当前当卡配置数据
        /// </summary>
        public LevelXDM CurrLevel
        {
            get { return XTable.LevelXTable.GetByID(CurrLevelID); }
        }

        /// <summary>
        /// 当前猫ID
        /// </summary>
        public int CurrentCatID { get; set; }

        /// <summary>
        /// 当前通关最大关卡ID
        /// </summary>
        public int CurrMaxLevelId { get; private set; }

        /// <summary>
        /// 当前解锁最大关卡ID
        /// </summary>
        public int CurrUnlockMaxLevelId { get; private set; }

        /// <summary>
        /// 战前准备道具
        /// </summary>
        public int[] prepareProps { get; set; }

        public int CurUnlockChapterID { get; set; }

        public int CurUnlockChapterTime
        {
            get { return _curUnlockChapterTime - KLaunch.Timestamp; }
            set { _curUnlockChapterTime = KLaunch.Timestamp + value; }
        }

        public int SpeedUpMoneyCost
        {
            get
            {
                var timeCost = XTable.ItemXTable.GetByID(ItemIDConst.TimeProp).Cost;
                return Mathf.CeilToInt(CurUnlockChapterTime / timeCost);
            }
        }

        #endregion

        #region METHOD

        /// <summary>
        /// 进入关卡场景
        /// </summary>
        public void EnterLevelScene()
        {
            KFramework.FsmManager.GetFsm<GameFsm>().SendEvent(this, InGameState.kStartLevel);
            KLoading.LoadAssets();
        }

        /// <summary>
        /// 退出关卡场景
        /// </summary>
        /// <param name="data"></param>
        public void ExitLevelScene(object data)
        {
            KLoading.UnloadAssets();
            KFramework.FsmManager.GetFsm<GameFsm>().SendEvent(this, InLevelState.kFinishLevel, data);
        }

        public ChapterUnlockXDM GetChapterByLevelID(int id)
        {
            var level = XTable.LevelXTable.GetByID(id);
            return XTable.ChapterUnlockXTable.GetByID(level.ChapterID);
        }

        public int GetStar(int levelID)
        {
            int val = 0;
            m_levelStarDic.TryGetValue(levelID, out val);
            return val;
        }

        public int GetScore(int levelID)
        {
            int val = 0;
            m_levelScoreDic.TryGetValue(levelID, out val);
            return val;
        }

        public void SetCurLevelInfo(int curMaxLevelID, int curUnlockMaxLevelID)
        {
            CurrMaxLevelId = curMaxLevelID;
            CurrUnlockMaxLevelId = curUnlockMaxLevelID;
            //for (int i = 0; i < allChapters.Length; i++)
            //{
            //    if (allChapters[i].allLevels[0].levelID < currUnlockMaxLevelId)
            //    {
            //        allChapters[i].unlocked = true;
            //    }
            //    else
            //    {
            //        allChapters[i].unlocked = false;
            //    }
            //}
            var allLevels = XTable.LevelXTable.GetAllList();
            foreach (var level in allLevels)
            {
                if (level.ID <= CurrMaxLevelId)
                {
                    level.Unlocked = true;
                }
                else break;
            }
            allLevels[0].Unlocked = true;
        }

        public void SetLevelStarAndScore(StageInfo[] infos)
        {
            foreach (var info in infos)
            {
                if (m_levelStarDic.ContainsKey(info.StageId))
                {
                    m_levelStarDic[info.StageId] = info.Star;
                }
                else
                {
                    m_levelStarDic.Add(info.StageId, info.Star);
                }

                if (m_levelScoreDic.ContainsKey(info.StageId))
                {
                    m_levelScoreDic[info.StageId] = info.TopScore;
                }
                else
                {
                    m_levelScoreDic.Add(info.StageId, info.TopScore);
                }
            }
        }

        public void SetCurChapterUnlock(int curUnlockChapterID, int curUnlockChapterTime)
        {
            CurUnlockChapterID = curUnlockChapterID;
            CurUnlockChapterTime = curUnlockChapterTime;
        }

        public void SetChapterUnlock(int maxUnlockStageId)
        {
            CurrUnlockMaxLevelId = maxUnlockStageId;
            DispatchEvent(LevelEvent.CHAPTER_UNLOCKED);
        }

        public void SetLevelResult(bool b)
        {
            DispatchEvent(LevelEvent.LEVEL_FINISHED);
        }

        #endregion

    }
}
