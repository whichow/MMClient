/** 
 *FileName:     BuyPopNumWindow.Model.cs 
 *Author:       LiMuChen 
 *Version:      1.0 
 *UnityVersion：5.6.3f1
 *Date:         2017-10-31 
 *Description:    
 *History: 
*/

namespace Game.UI
{
    partial class BuyPopNumWindow
    {
        #region WindowData

        public class Data
        {
            public int itemId;
            public KItem.ItemInfo[] itemInfor;
            public int type;
            public System.Action onConfirm;
            public System.Action onCancel;
        }

        #endregion

        #region Field

      

        private Data _buyPopNumData;

        #endregion

        #region Method

        public void InitModel()
        {
            _buyPopNumData = new Data();
        }

        public void RefreshModel()
        {
            var passData = data as Data;
            if (passData != null)
            {
                _buyPopNumData.itemId = passData.itemId;
                _buyPopNumData.itemInfor = passData.itemInfor;
                _buyPopNumData.onCancel = passData.onCancel;
                _buyPopNumData.type = passData.type;
                _buyPopNumData.onConfirm = passData.onConfirm;
            }
            else
            {
                _buyPopNumData.itemId =0;
                _buyPopNumData.onCancel = null;
                _buyPopNumData.onConfirm =null;
            }
        }

        #endregion
    }
}

