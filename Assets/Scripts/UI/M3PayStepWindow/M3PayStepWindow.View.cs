
/** 
*FileName:     M3PayStepWindow.cs 
*Author:        
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2017-12-25 
*Description:    
*History: 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game;
using System;
using Game.Match3;

namespace Game.UI
{
    public partial class M3PayStepWindow
    {

        private KUIImage stepIcon;
        private Text countText;
        private KUIImage moneyIcon;
        private Text costText;
        private Button buyBtn;
        private Button backBtn;
        //private Text 


        private void InitView()
        {
            stepIcon = Find<KUIImage>("Text/Image");
            countText = Find<Text>("Countdown");
            moneyIcon = Find<KUIImage>("ButtonPay/Gold");
            costText = Find<Text>("ButtonPay/Gold/Number");
            buyBtn = Find<Button>("ButtonPay");
            backBtn = Find<Button>("Close");

            buyBtn.onClick.AddListener(OnBuyClick);
            backBtn.onClick.AddListener(OnCloseClick);
        }



        private void RefreshView()
        {
            Debug.Log(M3GameManager.Instance.modeManager.GetReburnCount());
            var item = KItemManager.Instance.GetItem((int)M3GameManager.Instance.modeManager.GetReburnCount());
            //var itemMoneyType = KItemManager.Instance.GetItem(item.Money).curCount;
            costText.text = item.Cost.ToString();
            moneyIcon.overrideSprite = KIconManager.Instance.GetItemIcon(item.Money);
        }
    }
}