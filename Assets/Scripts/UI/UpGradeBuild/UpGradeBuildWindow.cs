/** 
 *FileName:     UpGradeBuildWindow.cs 
 *Author:       LiMuChen 
 *Version:      1.0 
 *UnityVersion：5.6.3f1
 *Date:         2017-11-03 
 *Description:    
 *History: 
*/

namespace Game.UI
{
    public partial class UpGradeBuildWindow : KUIWindow
    {
        #region Static

   

   
      

      
        public static void HideMessage()
        {
            CloseWindow<MessageBox>();
        }

        #endregion

        #region Constructor

        public UpGradeBuildWindow()
                    : base(UILayer.kPop, UIMode.kNone)
        {
            uiPath = "UpGradeBuild";
        }

        #endregion

        #region Action

  

        #endregion

        #region Unity  

        // Use this for initialization
        public override void Awake()
        {
            InitModel();
            InitView();
        }

        public override void OnEnable()
        {
            RefreshModel();
            RefreshView();
        }

        #endregion
    }
}

