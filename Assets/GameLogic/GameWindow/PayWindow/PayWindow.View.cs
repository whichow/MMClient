/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/6/3 11:00:33
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/

using UnityEngine.UI;

namespace Game.UI
{
    public partial class PayWindow
    {
        private Text _txt;

        public void InitView()
        {
            _txt = Find<Text>("Text");
        }

        public void RefreshView()
        {
            if (_state == 0)
            {
                _txt.text = "支付处理中...";
            }
            else if (_state == 1)
            {
                _txt.text = "支付成功，发货处理中...";
            }
            else if (_state == -1)
            {
                _txt.text = "支付失败!";
            }
        }

        private void ShowMsg(string msg)
        {
            _txt.text = msg;
        }

    }
}
