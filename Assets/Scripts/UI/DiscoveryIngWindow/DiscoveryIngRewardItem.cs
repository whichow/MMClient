/** 
 *FileName:     DiscoveryIngRewardItem.cs 
 *Author:       LiMuChen 
 *Version:      1.0 
 *UnityVersion：5.6.3f1
 *Date:         2017-10-26 
 *Description:    
 *History: 
*/ 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class DiscoveryIngRewardItem : KUIItem
    {

        #region Field
        private Image _imageIcon;
        private Text _textRewardNum;
        #endregion
        public void ShowReward(KItem.ItemInfo data)
        {

            _imageIcon.overrideSprite = KIconManager.Instance.GetItemIcon(data.itemID);
            _textRewardNum.text = "x" + data.itemCount;
        }















        private void Awake()
        {
            _imageIcon = Find<Image>("Item/Image");
            _textRewardNum = Find<Text>("Item/Text");
        }


        void Update()
        {

        }
    }
}
