/** 
 *FileName:     DiscoveryIngWindow.cs 
 *Author:       LiMuChen 
 *Version:      1.0 
 *UnityVersion：5.6.3f1
 *Date:         2017-10-26 
 *Description:    
 *History: 
*/
namespace Game.UI
{
    public partial class DiscoveryIngWindow : KUIWindow
    {
        #region Constructor

        public DiscoveryIngWindow()
            : base(UILayer.kNormal, UIMode.kSequence)
        {
            uiPath = "DiscoveryIng";
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

