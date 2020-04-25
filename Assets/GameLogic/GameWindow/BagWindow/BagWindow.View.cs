// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "BagWindow.View" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using Game.DataModel;
using Msg.ClientMessage;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    partial class BagWindow
    {
        private UIList _bagList;
        private Text _itemName;
        private Text _itemCount;
        private Text _itemPrice;
        private Text _itemDescription;
        private Text _descriptionText;
        private Image _itemIcon;
        private Button _saleBtn;
        private Button _composeBtn;
        private RectTransform _rectSale;
        private ItemInfo _curItemInfo;
        private ItemXDM _curItemXDM;
        private GameObject _panelUp;
        private GameObject _price;
        private GameObject _butSale;
        private GameObject _butCompose;
        private GameObject _leftText;
        private GameObject _rightText;
        int catShopID = 0;
        int catComposePiece = 0;

        public void InitView()
        {
            _bagList = Find<UIList>("Right/List");
            _bagList.SetRenderHandler(RenderHandler);
            _bagList.SetSelectHandler(SelectHandler);
            _bagList.SetPointerHandler(PointerHandler);

            _itemName = Find<Text>("Left/PanelUP/Image/Name");
            _itemCount = Find<Text>("Left/PanelUP/Image/Have/Count");
            _itemPrice = Find<Text>("Left/Price/Price");
            _itemDescription = Find<Text>("Left/PanelUP/Description/Text");
            _descriptionText = Find<Text>("Left/ButtonCompose/Text");
            _itemIcon = Find<Image>("Left/PanelUP/Image/Icon");
            _saleBtn = Find<Button>("Left/ButtonSale");
            _saleBtn.onClick.AddListener(OnSaleBtnClick);
            _composeBtn = Find<Button>("Left/ButtonCompose");
            _composeBtn.onClick.AddListener(OnComposeClick);
            _rectSale = Find<RectTransform>("Left/ButtonSale");
            _panelUp = Find("Left/PanelUP");
            _price = Find("Left/Price");
            _butSale = Find("Left/ButtonSale");
            _butCompose = Find("Left/ButtonCompose");
            _leftText = Find("Left/Text");
            _rightText = Find("Right/Text");
        }

        private void PointerHandler(UIListItem item, int index)
        {
            item.GetComp<TweenScl>("ItemObj").PlayBack();
        }

        private void RenderHandler(UIListItem item, int index)
        {
            ItemInfo vo = item.dataSource as ItemInfo;
            if (vo == null)
                return;
            ItemXDM xdm = XTable.ItemXTable.GetByID(vo.ItemCfgId);
            item.GetComp<Text>("ItemObj/Count").text = vo.ItemNum.ToString();
            item.GetComp<Image>("ItemObj/Icon").overrideSprite = KIconManager.Instance.GetItemIcon(vo.ItemCfgId);
            item.GetGameObject("ItemObj/Point").SetActive(false);
            item.GetGameObject("ItemObj/FrameObj/Frame1").SetActive(xdm.Rarity == ItemRarityConst.RarityTwo);
            item.GetGameObject("ItemObj/FrameObj/Frame2").SetActive(xdm.Rarity == ItemRarityConst.RarityThree);
            item.GetGameObject("ItemObj/FrameObj/Frame3").SetActive(xdm.Rarity == ItemRarityConst.RarityFour);
            item.GetGameObject("ItemObj/FrameObj/Frame4").SetActive(xdm.Rarity == ItemRarityConst.RarityFive);
        }

        private void SelectHandler(UIListItem item, int index)
        {
            _curItemInfo = item.dataSource as ItemInfo;
            if (_curItemInfo == null)
                return;
            _curItemXDM = XTable.ItemXTable.GetByID(_curItemInfo.ItemCfgId);
            _itemName.text = KLocalization.GetLocalString(_curItemXDM.NameId);
            _itemCount.text = _curItemInfo.ItemNum.ToString();
            _itemPrice.text = _curItemXDM.SaleCoin.ToString();
            _itemDescription.text = KLocalization.GetLocalString(_curItemXDM.DescriptionId);
            _itemIcon.overrideSprite = KIconManager.Instance.GetItemIcon(_curItemInfo.ItemCfgId);
            _composeBtn.gameObject.SetActive(_curItemXDM.Tag == ItemTagConst.Fragment || _curItemXDM.Type == ItemTypeConst.DrawCard);
            switch (_curItemXDM.Tag)
            {
                case ItemTagConst.Fragment:
                    _descriptionText.text = KLocalization.GetLocalString(58212);
                    _rectSale.anchoredPosition = new Vector2(-93.6f, -266f);
                    break;
                case ItemTagConst.Material:
                    _rectSale.anchoredPosition = new Vector2(0f, -266f);
                    break;
                case ItemTagConst.Consumable:
                    if (_curItemXDM.Type == ItemTypeConst.DrawCard)
                    {
                        _rectSale.anchoredPosition = new Vector2(-93.6f, -266f);
                        _descriptionText.text = KLocalization.GetLocalString(52169);
                    }
                    else
                    {
                        _rectSale.anchoredPosition = new Vector2(0f, -266f);
                    }
                    break;
            }
        }

        public void RefreshView(int type)
        {
            _bagList.DataArray = BagDataModel.Instance.GetBagItemDataByType(type);
            _panelUp.SetActive(_bagList.DataArray.Count > 0);
            _price.SetActive(_bagList.DataArray.Count > 0);
            _butSale.SetActive(_bagList.DataArray.Count > 0);
            _butCompose.SetActive(_bagList.DataArray.Count > 0);
            _leftText.SetActive(_bagList.DataArray.Count <= 0);
            _rightText.SetActive(_bagList.DataArray.Count <= 0);
            _bagList.SelectedIndex = -1;
            _bagList.SelectedIndex = 0;
        }

        public void OnSaleBtnClick()
        {
            OpenWindow<BagSaleBoxWindow>(_curItemInfo);
        }

        private  void OnComposeClick()
        {
            if (_curItemXDM.Tag == ItemTagConst.Fragment)
            {
                var kcat = XTable.CatXTable.GetAllList();
                for (int i = 0; i < kcat.Count; i++)
                {
                    if (kcat[i].Piece == null || kcat[i].Piece.Count == 0)
                        continue;
                    if (kcat[i].Piece[0] == _curItemInfo.ItemCfgId)
                    {
                        catShopID = kcat[i].ID;
                        catComposePiece = kcat[i].Piece[1];
                    }
                }
                if (_curItemInfo.ItemNum < catComposePiece)
                {
                    OpenWindow<MessageBox>(new MessageBox.Data()
                    {
                        onConfirm = OnTips,
                        content = KLocalization.GetLocalString(52008),
                    });
                }
                else
                {
                    OpenWindow<MessageBox>(new MessageBox.Data()
                    {
                        onConfirm = CompCat,
                        onCancel = OnCancel,
                        content = string.Format(KLocalization.GetLocalString(58209), catComposePiece, KLocalization.GetLocalString(_curItemXDM.NameId), KItemManager.Instance.GetItem(catShopID).itemName),
                    });
                }
            }
            else
            {
                CloseWindow(this);
                OpenWindow<PhotoShopWindow>();
            }
        }

        private void CompCat()
        {
            KUser.ComposeCat(catShopID, OnCompCatCallBack);
        }

        private void OnTips()
        {

        }

        private void OnCancel()
        {

        }
    }
}

