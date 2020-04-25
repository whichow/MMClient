/** 
*FileName:     BuyPopNumWindow.cs 
*Author:       LiMuChen 
*Version:      1.0 
*UnityVersion：5.6.3f1
*Date:         2017-10-31 
*Description:    
*History: 
*/
using UnityEngine;

namespace Game.UI
{
    public partial class BuyPopNumWindow : KUIWindow
    {
        #region Constructor

        public BuyPopNumWindow()
            : base(UILayer.kPop, UIMode.kNone)
        {
            uiPath = "BuyPopNum";
        }

        #endregion

        #region Action

        private void OnCloseBtnClick()
        {
            if (_buyPopNumData.onCancel != null)
            {
                K.Events.EventInvoker.Invoke(_buyPopNumData.onCancel);
            }
            CloseWindow(this);
        }
        #endregion

        private void OnAddBtnClick()
        {
            int num = buyNum + 1;
            var item = KItemManager.Instance.GetItem(_buyPopNumData.itemId);
            var itemMoneyType = KItemManager.Instance.GetItem(item.Money).curCount;
            int count = item.Cost * num;
            if (itemMoneyType >= count)
            {
                buyNum++;
                RefreshView();
            }
        }

        private void OnRemoveBtnClick()
        {
            if (buyNum > 1)
            {
                buyNum--;
                RefreshView();
            }
        }

        private void OnBuyBtnClick()
        {
            var item = KItemManager.Instance.GetItem(_buyPopNumData.itemId);
            var count = PlayerDataModel.Instance.GetCurrency(item.Money);
            int cost = item.Cost * buyNum;
            if (count >= cost)
            {
                KShop.Instance.BuyItem(item.itemID, buyNum, OnBuyCallBack);
            }
            else
            {
                CloseWindow(this);
                OpenWindow<LackHintBox>(item.Money);
            }
        }
        private void OnBuyCallBack(int code, string message, object data)
        {
            if (code == 0)
            {
                if (_buyPopNumData.onConfirm != null)
                {
                    K.Events.EventInvoker.Invoke(_buyPopNumData.onConfirm);
                }
                CloseWindow(this);
            }

        }
        #region Unity  

        // Use this for initialization
        public override void Awake()
        {
            InitModel();
            InitView();
        }

        public override void OnEnable()
        {
            RefreshModel();
            RefreshView();
        }

        #endregion
    }
}

