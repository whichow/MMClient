/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/5/20 15:07:45
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/

namespace Game.DataModel
{
    public partial class ErrorXDM
    {
        public override string ToString()
        {
            return KLocalization.GetLocalString(Message, ID.ToString());
        }

    }
}
