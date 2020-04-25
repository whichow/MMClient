/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/4/24 12:17:46
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/

namespace Game.DataModel
{
    public partial class GuideXDM 
    {

        public int GetStepByIndex(int step)
        {
            if (step < 0 || step >= Steps.Count)
            {
                step = 0;
            }
            return Steps[step];
        }

    }
}
