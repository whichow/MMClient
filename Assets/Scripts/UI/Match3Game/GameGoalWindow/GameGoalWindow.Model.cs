/** 
 *FileName:     GameGoalWindow.cs 
 *Author:       Hejunjie 
 *Version:      1.0 
 *UnityVersion：5.6.2f1
 *Date:         2017-11-07 
 *Description:    
 *History: 
*/
using Game.Match3;
namespace Game.UI
{
    public partial class GameGoalWindow
    {
        M3GameModeManager modeManager;
        M3LevelData mapData;
        public void InitModel()
        {
            modeManager = M3GameManager.Instance.modeManager;
            mapData = M3GameManager.Instance.level;
        }
    }
}
