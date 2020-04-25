/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/5/15 14:56:51
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/

namespace Game.DataModel
{
    public partial class LevelXDM
    {
        public bool Unlocked { get; set; }

        public int CurrStar
        {
            get { return LevelDataModel.Instance.GetStar(ID); }
        }

        public bool IsFinish
        {
            get { return CurrStar > 0; }
        }

    }
}
