/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/6/20 11:10:56
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/

using UnityEngine;

namespace Game.DataModel
{
    public enum ECatColor
    {
        fRed = 1,
        fYellow = 2,
        fBlue = 4,
        fGreen = 8,
        fPurple = 16,
        fBrown = 32,
    }

    public partial class CatXDM
    {
        public Sprite GetIconSprite()
        {
            return KIconManager.Instance.GetCatIcon(Icon);
        }

        public Sprite GetPhotoSprite()
        {
            return KIconManager.Instance.GetCatFull(Photo);
        }
        
        public ECatColor CatColor
        {
            get { return (ECatColor)Color; }
        }

        public bool ContainColor(int color)
        {
            return (Color & color) != 0;
        }

        public string GetCatColorString()
        {
            return CatUtils.GetCatColorString(CatColor);
        }

    }
}
