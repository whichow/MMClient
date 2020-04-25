/** 
 *FileName:     DiscoveryWindow.cs 
 *Author:       LiMuChen 
 *Version:      1.0 
 *UnityVersion：5.6.3f1
 *Date:         2017-10-19 
 *Description:    
 *History: 
*/

namespace Game.UI
{
    public partial class DiscoveryWindow : KUIWindow
    {
        #region Constructor

        public DiscoveryWindow()
            : base(UILayer.kNormal, UIMode.kSequenceRemove)
        {
            uiPath = "Discovery";
        }

        #endregion

        #region Method       

        #endregion

        #region Action

        private void OnCloseBtnClick()
        {
            CloseWindow(this);
        }

      
        #endregion

        #region Unity

        public override void Awake()
        {
            InitView();
        }

        public override void OnEnable()
        {
            RefreshView();
        }

        #endregion
    }
}

