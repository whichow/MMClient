// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "CatBagWindow" company=""></copyright>
// <summary></summary>
// ***********************************************************************

namespace Game.UI
{
    public partial class TaskAndAchievementWindow
    {
        #region Enum

        public enum PageType
        {
            kTask,
            kAchievement,
        }

        #endregion

        #region Field

        private PageType _pageType;

        private bool _isChanged;


        #endregion

        #region Porperty

        /// <summary>
        /// 
        /// </summary>
        public PageType pageType
        {
            get { return _pageType; }
            private set
            {
                if (_pageType != value)
                {
                    _pageType = value;
                    _isChanged = true;
                }
            }
        }

        public bool isChanged
        {
            get { return _isChanged; }
        }

        #endregion

        #region Method

        private void InitModel()
        {
            pageType = PageType.kTask;
        }
        public void RefreshModel()
        {
            pageType = PageType.kTask;
        }
        public KMission[] GetTorAData()
        {
            KMission[] currentMission;
            switch (pageType)
            {
                case PageType.kTask:
                    currentMission = KMissionManager.Instance.dailyMissions;
                    break;
                case PageType.kAchievement:
                    currentMission = KMissionManager.Instance.achievementMissions;
                    break;
                default:
                    currentMission = null;
                    break;
            }
            return currentMission;
        }

        #endregion
    }
}

