/** 
 *FileName:     GameGoalWindow.cs 
 *Author:       Hejunjie 
 *Version:      1.0 
 *UnityVersion：5.6.2f1
 *Date:         2017-11-07 
 *Description:    
 *History: 
*/

namespace Game.UI
{
    public partial class GameGoalWindow : KUIWindow
    {

        public GameGoalWindow() : base(UILayer.kPop, UIMode.kNone)
        {
            uiPath = "GameGoalWindow";
        }

        public override void Awake()
        {
            base.Awake();
            InitModel();
            InitView();
        }
        public override void OnEnable()
        {
            base.OnEnable();
            RefreshView();
        }

    }
}
