
using Msg.ClientMessage;
/** 
*FileName:     DiscoveryRetItem.cs 
*Author:       LiMuChen 
*Version:      1.0 
*UnityVersion：5.6.3f1
*Date:         2017-11-09 
*Description:    
*History: 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Game.UI
{
    public class DiscoveryRetItem : KUIItem, IPointerClickHandler
    {

        #region Filed
        private Image _imageIcon;
        private Text _textItemNum;
        #endregion

        #region Method
        public void ShowData(IdNum data)
        {
            _imageIcon.overrideSprite = KIconManager.Instance.GetItemIcon(data.Id);
            _textItemNum.text = "x" + data.Num.ToString();
        }


        #endregion

        #region Interface
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {



        }
        #endregion


        #region Unity

        private void Awake()
        {
            _imageIcon = Find<Image>("Image/Icon");
            _textItemNum = Find<Text>("Image/Icon/Text");
        }


        #endregion
    }
}
