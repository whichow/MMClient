using Game.DataModel;
using System;
using System.Collections.Generic;

namespace Game.UI
{
    public partial class MapSelectWindow
    {
        public class MapSelectData
        {
            Action backAction;

            public LevelXDM currentLevel;
            public LevelXDM nextLevel;
            public bool showInfoWindow;

            public MapSelectData()
            {
                currentLevel = XTable.LevelXTable.GetByID(LevelDataModel.Instance.CurrMaxLevelId);
            }

            public MapSelectData(LevelXDM curLevel, LevelXDM nLevel, bool needShow, Action action = null)
            {
                currentLevel = curLevel;
                nextLevel = nLevel;
                showInfoWindow = needShow;
            }

            public MapSelectData(int curLevel, int nLevel, bool needShow, Action action = null)
            {
                currentLevel = XTable.LevelXTable.GetByID(curLevel);
                nextLevel = XTable.LevelXTable.GetByID(nLevel);
                showInfoWindow = needShow;
            }

            public MapSelectData(LevelXDM curLevel, bool needShow, Action action = null)
            {
                currentLevel = curLevel;
                showInfoWindow = needShow;
            }

            public MapSelectData(int curLevelID, bool needShow, Action action = null)
            {
                currentLevel = XTable.LevelXTable.GetByID(curLevelID);
                showInfoWindow = needShow;
            }
        }


        private Dictionary<int, MapItem> mapDic = new Dictionary<int, MapItem>();
        private List<ChapterUnlockXDM> chapters;
        private MapSelectData mapData;
        private ChapterUnlockXDM currentChapter;

        public void InitModel()
        {
            if (data != null)
                mapData = (MapSelectData)data;
            else
            {
                mapData = new MapSelectData();
            }
            chapters = new List<ChapterUnlockXDM>();
            //for (int i = 0; i < KLevelManager.Instance.allChapters.Length; i++)
            //{
            //    if (KLevelManager.Instance.allChapters[i].unlocked)
            //    {
            //        chapters.Add(KLevelManager.Instance.allChapters[i]);
            //    }
            //}
            itemDic = new Dictionary<int, MapSelectItem>();
            //currentChapter = KLevelManager.Instance.GetChapterByLevelID(KLevelManager.Instance.currUnlockMaxLevelId);
        }

        private void RefreshModel()
        {
            chapters.Clear();
            var list = XTable.ChapterUnlockXTable.GetAllList();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].unlocked)
                {
                    chapters.Add(list[i]);

                }
            }
            currentChapter = LevelDataModel.Instance.GetChapterByLevelID(LevelDataModel.Instance.CurrUnlockMaxLevelId);
        }

    }
}
