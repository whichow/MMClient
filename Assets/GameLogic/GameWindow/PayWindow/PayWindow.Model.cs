/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/6/3 11:04:04
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/

using Game.DataModel;

namespace Game.UI
{
    public partial class PayWindow
    {
        private int _state;
        private int _payID;

        public void RefreshModel()
        {
            _state = 0;
            _payID = System.Convert.ToInt32(data);
        }

        private string GetProductID()
        {
            return XTable.PayXTable.GetByID(_payID).BundleID;
        }

    }
}
