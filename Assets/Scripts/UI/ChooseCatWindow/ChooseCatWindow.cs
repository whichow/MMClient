/** 
 *FileName:     ChooseCatWindow.cs 
 *Author:       LiMuChen 
 *Version:      1.0 
 *UnityVersion：5.6.3f1
 *Date:         2017-10-23 
 *Description:    
 *History: 
*/

namespace Game.UI
{
    public partial class ChooseCatWindow : KUIWindow
    {
        public ChooseCatWindow()
            : base(UILayer.kNormal, UIMode.kSequence)
        {
            uiPath = "ChooseCat";
        }

        private void OnCloseBtnClick()
        {
            CloseWindow(this);
            if (_chooseCatData.onCancel != null)
            {
                K.Events.EventInvoker.Invoke(_chooseCatData.onCancel);
            }
        }
        public void OnConfirmBtnClick()
        {
            CloseWindow(this);
            if (_chooseCatData.onConfirm != null)
            {
                K.Events.EventInvoker.Invoke(_chooseCatData.onConfirm, _cat,_chooseCatData.idx);
            }
        }

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
    }
}

