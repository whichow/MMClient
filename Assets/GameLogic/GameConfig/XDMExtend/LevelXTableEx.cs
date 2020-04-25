/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/5/15 15:36:56
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using System.Collections.Generic;

namespace Game.DataModel
{
    public partial class LevelXTable
    {
        public List<LevelXDM> AllLevels
        {
            get { return GetAllList(); }
        }

        public List<LevelXDM> AllUnlockLevels
        {
            get { return GetAllList().FindAll(level => level.Unlocked); }
        }

        public List<LevelXDM> AllOneStarUnlockLevels
        {
            get { return GetAllList().FindAll(level => level.Unlocked && level.CurrStar == 1); }
        }
        public List<LevelXDM> AllTwoStarUnlockLevels
        {
            get { return GetAllList().FindAll(level => level.Unlocked && level.CurrStar == 2); }
        }
        public List<LevelXDM> AllThreeStarUnlockLevels
        {
            get { return GetAllList().FindAll(level => level.Unlocked && level.CurrStar == 3); }
        }
        public List<LevelXDM> AllNoStarUnlockLevels
        {
            get { return GetAllList().FindAll(level => level.Unlocked && level.CurrStar == 0); }
        }

        public List<LevelXDM> GetAllLevelsBytChapterID(int chapterID)
        {
            var levels = GetAllList().FindAll(level => level.ChapterID == chapterID);
            if (levels != null)
                return levels;
            return null;
        }

        public int GetCurrentAllStars()
        {
            int num = 0;
            for (int i = 0; i < AllLevels.Count; i++)
            {
                num += AllLevels[i].CurrStar;
            }
            return num;
        }

    }
}
