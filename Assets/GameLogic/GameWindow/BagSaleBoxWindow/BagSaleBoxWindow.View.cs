using Game.DataModel;
using Msg.ClientMessage;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Game.UI
{
    public partial class BagSaleBoxWindow
    {
        private ItemInfo _itemInfo;
        private ItemXDM _itemXDM;
        private Button _clase;
        private Button _btnBack;
        private UIButtonExtension _btnPlus;
        private UIButtonExtension _btnMinus;
        private Button _btnMax;
        private Text _textName;
        private Text _textNum;
        private Text _textCurNum;
        private Text _textMoney;
        private Image _imageIcon;
        private Button _btnSell;

        private int _itemNum;

        public void InitView()
        {
            _clase = Find<Button>("Panel");
            _clase.onClick.AddListener(OnBackBtnClick);
            _btnBack = Find<Button>("Close");
            _btnBack.onClick.AddListener(OnBackBtnClick);
            _btnPlus = Find<UIButtonExtension>("ImageBack/Choose/ButtonPlus");
            _btnPlus.onClick.AddListener(OnPlusClick);
            _btnPlus.onLongClick.AddListener(OnPlusClick);
            _btnMinus = Find<UIButtonExtension>("ImageBack/Choose/ButtonMinus");
            _btnMinus.onClick.AddListener(OnMinusClick);
            _btnMinus.onLongClick.AddListener(OnMinusClick);
            _btnMax = Find<Button>("ImageBack/Choose/ButtonMax");
            _btnMax.onClick.AddListener(OnMaxClick);
            _textNum = Find<Text>("ImageBack/Choose/Text");
            _textName = Find<Text>("ImageBack/Text");
            _textCurNum = Find<Text>("ImageBack/Have/Text");
            _textMoney = Find<Text>("GoldBack/Text");
            _imageIcon = Find<Image>("ImageBack/Item/Image");
            _btnSell = Find<Button>("ImageBack/ButtonOK");
            _btnSell.onClick.AddListener(OnSellClick);

            _itemNum = 1;
        }

        public void RefreshView()
        {
            _itemInfo = data as ItemInfo;
            if (_itemInfo == null)
                return;
            _itemXDM = XTable.ItemXTable.GetByID(_itemInfo.ItemCfgId);
            _textNum.text = _itemNum + "/" + _itemInfo.ItemNum;
            _imageIcon.overrideSprite = KIconManager.Instance.GetItemIcon(_itemInfo.ItemCfgId);
            _textCurNum.text = _itemInfo.ItemNum.ToString();
            _textMoney.text = (_itemNum * _itemXDM.SaleCoin).ToString();
            _textName.text = KLocalization.GetLocalString(_itemXDM.NameId);
        }
    }
}
