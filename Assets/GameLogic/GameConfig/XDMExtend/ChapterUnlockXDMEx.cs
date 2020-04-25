/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/5/15 15:24:15
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using System.Collections.Generic;

namespace Game.DataModel
{
    public partial class ChapterUnlockXDM
    {
        private List<LevelXDM> _levels;

        public bool unlocked
        {
            get
            {
                return Levels[0].ID < LevelDataModel.Instance.CurrUnlockMaxLevelId;
            }

        }

        public List<LevelXDM> Levels
        {
            get
            {
                if (_levels == null)
                {
                    _levels = XTable.LevelXTable.GetAllList().FindAll(l => l.ChapterID == ID);
                }
                return _levels;
            }
        }

        public LevelXDM LastLevel
        {
            get
            {
                return Levels[Levels.Count - 1];
            }
        }

    }
}
