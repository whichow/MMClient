using Game.Match3;
using Msg.ClientMessage;
using System.Collections.Generic;

namespace Game.UI
{
    public partial class BagSaleBoxWindow : KUIWindow
    {
        public BagSaleBoxWindow() :
            base(UILayer.kPop, UIMode.kSequence)
        {
            uiPath = "BagSaleBox";
        }

        public override void Awake()
        {
            InitView();
        }

        public override void AddEvents()
        {
            base.AddEvents();
            BagDataModel.Instance.AddEvent(BagEvent.BagSellItem, OnSellItem);
        }

        public override void RemoveEvents()
        {
            base.RemoveEvents();
            BagDataModel.Instance.RemoveEvent(BagEvent.BagSellItem, OnSellItem);
        }

        public override void OnEnable()
        {
            _itemNum = 1;
            RefreshView();
        }
        private void OnBackBtnClick()
        {
            CloseWindow(this);
        }
        private void OnPlusClick()
        {
            if (_itemNum < _itemInfo.ItemNum)
            {
                _itemNum++;
                RefreshView();
            }
        }
    
        private void OnMinusClick()
        {
            if (_itemNum > 1)
            {
                _itemNum--;
                RefreshView();
            }
        }

        public void OnSellClick()
        {
            GameApp.Instance.GameServer.ReqSellItem(_itemInfo.ItemCfgId, _itemNum);
        }

        private void OnSellItem()
        {
            RefreshView();
            CloseWindow(this);
            List<ItemInfo> listInfo = new List<ItemInfo>();
            ItemInfo info = new ItemInfo();
            info.ItemCfgId = 2;
            info.ItemNum = _itemNum * _itemXDM.SaleCoin;
            listInfo.Add(info);
            KUIWindow.OpenWindow<GetItemTipsWindow>(listInfo);
            _itemNum = 1;
        }

        private void OnMaxClick()
        {
            _itemNum = _itemInfo.ItemNum;
            RefreshView();
        }
    }
}
