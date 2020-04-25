

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
using Game;
using Game.UI;
using System;
using Game.Match3;
using Game.DataModel;

namespace Game.UI
{
    public partial class M3PayStepWindow : KUIWindow
    {

        public M3PayStepWindow() :
                    base(UILayer.kPop, UIMode.kSequence)
        {
            uiPath = "PayGuide";
            uiAnim = UIAnim.kAnim1;
            hasMask = true;
        }
        public override void Awake()
        {
            base.Awake();
            InitModel();
            InitView();
        }
        public override void OnEnable()
        {
            base.OnEnable();
            RefreshModel();
            RefreshView();
        }

        public override void OnBlackMaskClick()
        {
            
        }

        private void OnCloseClick()
        {
            M3GameManager.Instance.modeManager.ShowGameOver();
            CloseWindow(this);
        }

        private void OnBuyClick()
        {
            var item = KItemManager.Instance.GetItem((int)M3GameManager.Instance.modeManager.GetReburnCount());
            var count = PlayerDataModel.Instance.GetCurrency(item.Money);
            if (count >= item.Cost)
            {
                KShop.Instance.BuyItem(item.itemID, 1, OnBuyCallBack);
            }
            else
            {
                CloseWindow(this);
                //OpenWindow<LackHintBox>(item.moneyType);
                M3GameManager.Instance.modeManager.ShowGameOver();
            }
        }

        private void OnBuyCallBack(int arg1, string arg2, object arg3)
        {
            M3GameManager.Instance.modeManager.AddStepCallBack(5);
            CloseWindow(this);
            M3GameManager.Instance.modeManager.AddReburnCount();
        }
    }
}