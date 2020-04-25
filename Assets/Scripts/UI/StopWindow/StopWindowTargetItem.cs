/** 
 *FileName:     StopWindowTargetItem.cs 
 *Author:       HeJunJie 
 *Version:      1.0 
 *UnityVersion：5.6.2f1
 *Date:         2018-01-18 
 *Description:    
 *History: 
*/
using Game.DataModel;
using Game.Match3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class StopWindowTargetItem : KUIItem
    {

        private KUIImage icon;

        private void Awake()
        {
            icon = Find<KUIImage>();
        }
        public void ShowInfo(int elementID, int count)
        {
            string iconName = XTable.ElementXTable.GetByID(elementID).Icon;
            Sprite sprite = Game.KIconManager.Instance.GetMatch3ElementIcon(iconName);
            if (sprite != null)
                icon.overrideSprite = sprite;
        }
    }
}