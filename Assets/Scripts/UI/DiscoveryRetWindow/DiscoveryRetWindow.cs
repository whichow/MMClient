

using UnityEngine;
/** 
*FileName:     DiscoveryRetWindow.cs 
*Author:       LiMuChen 
*Version:      1.0 
*UnityVersion：5.6.3f1
*Date:         2017-10-25 
*Description:    
*History: 
*/
namespace Game.UI
{
    public partial class DiscoveryRetWindow : KUIWindow
    {
        #region Constructor

        public DiscoveryRetWindow()
            : base(UILayer.kPop, UIMode.kNone)
        {
            uiPath = "DiscoveryRet";
        }

        #endregion

        #region Action

        private void OnConfirmBtnClick()
        {
            if (_discoveryRetData.type ==(int)KExplore.Task.Result.Success)
            {
                Debug.Log("分享");
            }
            else
            {
                OpenWindow<MessageBox>(new MessageBox.Data {
                    content = string.Format("花费{0}{1}改变探索结果即可获得以上奖励，是否确定？",_discoveryRetData.task.savePrice.itemCount,KItemManager.Instance.GetItem(_discoveryRetData.task.savePrice.itemID).itemName),
                    onCancel = OnCanelClick,
                    onConfirm= OnConfirmClick,
                }
                );
            }
        }
        private void OnCanelClick()
        {

        }
        private void OnConfirmClick()
        {

        }
        private void OnCancelBtnClick()
        {
            GetWindow<DiscoveryWindow>().RefreshView();
            CloseWindow(this);
        }
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

