/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/6/19 16:35:25
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using UnityEngine;

namespace Game.DataModel
{
    public partial class ItemXDM
    {
        public string Name
        {
            get { return KLocalization.GetLocalString(NameId); }
        }

        public Sprite GetIconSprite()
        {
            return KIconManager.Instance.GetItemIcon(Icon);
        }

    }
}
