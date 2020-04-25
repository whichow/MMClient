
using System;
/** 
*FileName:     LackHintWindow.cs 
*Author:       HeJunJie 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2018-01-22 
*Description:    
*History: 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public partial class LackHintWindow
    {
        private GameObject lackObj;
        private GameObject fullObj;
        private Button clostBtn;
        private Button buyBtn;
        private Button continueBtn;
        private LackHintItem smallItem;
        private LackHintItem middleItem;
        private LackHintItem bigItem;
        private Text titleText;
        private int _index = 0;
        private ShopDataVO _curShopDataVO;

        public void InitView()
        {
            clostBtn = Find<Button>("Panel/Close");
            buyBtn = Find<Button>("Panel/lack/Confirm");
            continueBtn = Find<Button>("Panel/full/Confirm");
            lackObj = transform.Find("Panel/lack").gameObject;
            fullObj = transform.Find("Panel/full").gameObject;
            smallItem = transform.Find("Panel/ImageBack/Grid/ImageBack1").gameObject.GetComponent<LackHintItem>();
            middleItem = transform.Find("Panel/ImageBack/Grid/ImageBack2").gameObject.GetComponent<LackHintItem>();
            bigItem = transform.Find("Panel/ImageBack/Grid/ImageBack3").gameObject.GetComponent<LackHintItem>();
            titleText = transform.Find("Panel/lack/Content").GetComponent<Text>();
            smallItem.Init();
            middleItem.Init();
            bigItem.Init();

            clostBtn.onClick.AddListener(OnCloseBtnClick);
            buyBtn.onClick.AddListener(OnBuyBtnClick);
            continueBtn.onClick.AddListener(OnCloseBtnClick);
        }

        private void OnCloseBtnClick()
        {
            CloseWindow(this);
        }

        private void OnBuyBtnClick()
        {
            if (_index < 5)
                GameApp.Instance.GameServer.ReqBuyShopItem(_curShopDataVO.mItemId, 1);
            else
                ToastBox.ShowText(KLocalization.GetLocalString(54194));
        }

        public void RefreshView()
        {
            smallItem.Refresh(111);
            middleItem.Refresh(112);
            bigItem.Refresh(113);
            lackObj.SetActive(PlayerDataModel.Instance.mPlayerData.mSpirit < KUser.SelfPlayer.maxPower);
            fullObj.SetActive(PlayerDataModel.Instance.mPlayerData.mSpirit >= KUser.SelfPlayer.maxPower);
            if (PlayerDataModel.Instance.mPlayerData.mSpirit < KUser.SelfPlayer.maxPower)
            {
                if (ShopDataModel.Instance._allShop.ContainsKey(ShopIDConst.BodyPower))
                {
                    List<ShopDataVO> shopDataVOs = new List<ShopDataVO>();
                    shopDataVOs = ShopDataModel.Instance._allShop[ShopIDConst.BodyPower];
                    ShowLack(shopDataVOs);
                }
                else
                {
                    GameApp.Instance.GameServer.ReqShopData(ShopIDConst.BodyPower);
                }
            }
        }
        private void ShowLack(List<ShopDataVO> shopDataVOs)
        {
            if (shopDataVOs == null || shopDataVOs.Count == 0)
                return;
            shopDataVOs.Sort((x, y) => x.mItemId.CompareTo(y.mItemId));
            for (int i = 0; i < shopDataVOs.Count; i++)
            {
                if (shopDataVOs[i].mItemNum > 0)
                {
                    _index = i;
                    OnTitle(shopDataVOs[i]);
                    break;
                }
                if (i == shopDataVOs.Count - 1)
                {
                    _index = i + 1;
                    OnTitle(shopDataVOs[i]);
                    //体力已经买完
                }
            }
        }

        private void OnTitle(ShopDataVO shopDataVO)
        {
            _curShopDataVO = shopDataVO;
            titleText.text = string.Format(KLocalization.GetLocalString(53336), _curShopDataVO.mByCost.ItemNum, _curShopDataVO.mNumber, 5 - _index, 5);

        }

        public override void AddEvents()
        {
            base.AddEvents();
            ShopDataModel.Instance.AddEvent(ShopEvent.ShopData, OnShopData);
            ShopDataModel.Instance.AddEvent(ShopEvent.BuyItem, RefreshView);
            PlayerDataModel.Instance.AddEvent(PlayerEvent.PlayerDataRefresh, RefreshView);
        }

        public override void RemoveEvents()
        {
            base.RemoveEvents();
            ShopDataModel.Instance.RemoveEvent(ShopEvent.ShopData, OnShopData);
            ShopDataModel.Instance.RemoveEvent(ShopEvent.BuyItem, RefreshView);
            PlayerDataModel.Instance.RemoveEvent(PlayerEvent.PlayerDataRefresh, RefreshView);
        }

        private void OnShopData(IEventData value)
        {
            List<ShopDataVO> shopDataVOs = new List<ShopDataVO>();
            shopDataVOs = ShopDataModel.Instance._allShop[(value as EventData).Integer];
            ShowLack(shopDataVOs);
        }
    }
}
