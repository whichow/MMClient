/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/6/4 15:42:42
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using System.Collections;

namespace Game.DataModel
{
    public partial class GuideXTable
    {
        public override void LoadFromHashtable(Hashtable table)
        {
            base.LoadFromHashtable(table);
            XTable.GuideStepXTable.LoadFromHashtable(table);
            XTable.GuideActionXTable.LoadFromHashtable(table);
            XTable.GuideConditionXTable.LoadFromHashtable(table);
        }
    }
}
