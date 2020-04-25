using Game.DataModel;
using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    partial class ShopWindow
    {
        private ShopDataVO _curShopVO;
        private GameObject _waresObj;
        private UIList _waresList;
        private Image _backImg;
        private Text _backNum;
        private GameObject _waresBuy;
        private Button _buyCloseBtn;
        private Button _buyPanelBtn;
        private Text _buyItemName;
        private UIList _waresBuyList;
        private Text _buyNum;
        private UIButtonExtension _subBtn;
        private UIButtonExtension _addBtn;
        private Image _subImg;
        private Image _addImg;
        private Button _buyBtn;
        private Image _buyBtnIcon;
        private Text _buyBtnNum;
        private int _buyItemNum;
        private GameObject _propTog;
        private GameObject _soulStoneTog;
        private GameObject _gain;
        private UIList _propList;
        private Toggle[] _propToggles;
        private Toggle[] _soulStoneToggles;

        private UIList _curList;
        private UIList _catList;
        private List<CatDataVO> _curListCatVO;
        private Button _canel;
        private Button _explain;
        private int _soulStoneType;

        public void InitView()
        {
            _waresObj = Find("Wares");
            _waresList = Find<UIList>("WaresList");
            _waresList.SetRenderHandler(RenderHandler);
            _waresList.SetPointerHandler(PointerHandler);
            _backImg = Find<Image>("Back/Icon");
            _backNum = Find<Text>("Back/Text");
            _waresBuy = Find("WaresBuy");
            _buyCloseBtn = Find<Button>("WaresBuy/Close");
            _buyCloseBtn.onClick.AddListener(OnBuyClose);
            _buyPanelBtn = Find<Button>("WaresBuy/BgBlack");
            _buyPanelBtn.onClick.AddListener(OnBuyClose);
            _buyItemName = Find<Text>("WaresBuy/Title");
            _waresBuyList = Find<UIList>("WaresBuy/List");
            _waresBuyList.SetRenderHandler(BuyRenderHandler);
            _buyNum = Find<Text>("WaresBuy/NumObj/Num");
            _subBtn = Find<UIButtonExtension>("WaresBuy/NumObj/Sub");
            _subBtn.onClick.AddListener(OnSubBtn);
            _subBtn.onLongClick.AddListener(OnSubBtn);
            _addBtn = Find<UIButtonExtension>("WaresBuy/NumObj/Add");
            _addBtn.onClick.AddListener(OnAddBtn);
            _addBtn.onLongClick.AddListener(OnAddBtn);
            _subImg = Find<Image>("WaresBuy/NumObj/Sub");
            _addImg = Find<Image>("WaresBuy/NumObj/Add");
            _buyBtn = Find<Button>("WaresBuy/BuyBtn");
            _buyBtn.onClick.AddListener(OnBuyBtn);
            _buyBtnIcon = Find<Image>("WaresBuy/BuyBtn/Icon");
            _buyBtnNum = Find<Text>("WaresBuy/BuyBtn/Num");
            _propTog = Find("Wares/PropTog");
            _soulStoneTog = Find("Wares/SoulStone");
            _gain = Find("Wares/Decompose");
            _propList = Find<UIList>("Wares/List");
            _propList.SetRenderHandler(RenderHandler);
            _propList.SetPointerHandler(PointerHandler);

            _propToggles = new Toggle[3];
            for (int i = 0; i < 3; i++)
                _propToggles[i] = Find<Toggle>("Wares/PropTog/Tog" + (i + 1));
            foreach (Toggle tog in _propToggles)
                tog.onValueChanged.AddListener((bool blSelect) => { if (blSelect) OnPropChange(tog); });

            _soulStoneToggles = new Toggle[2];
            for (int i = 0; i < 2; i++)
                _soulStoneToggles[i] = Find<Toggle>("Wares/SoulStone/SoulStoneTog/Tog" + (i + 1));
            foreach (Toggle tog in _soulStoneToggles)
                tog.onValueChanged.AddListener((bool blSelect) => { if (blSelect) OnSoulStoneChange(tog); });

            _curList = Find<UIList>("Wares/Decompose/CurList");
            _curList.SetRenderHandler(CurRenderHandler);
            _curList.SetPointerHandler(CurPointerHandler);
            _catList = Find<UIList>("Wares/Decompose/CatList");
            _catList.SetRenderHandler(CatRenderHandler);
            _catList.SetPointerHandler(CatPointerHandler);
            _canel = Find<Button>("Wares/Decompose/Buttons/Canel");
            _canel.onClick.AddListener(OnCanel);
            _explain = Find<Button>("Wares/Decompose/Buttons/Explain");
            _explain.onClick.AddListener(OnExplain);

            _soulStoneType = SoulStoneConst.SoulStoneExchange;
        }

        public override void AddEvents()
        {
            base.AddEvents();
            ShopDataModel.Instance.AddEvent(ShopEvent.ShopData, OnShopData);
            ShopDataModel.Instance.AddEvent(ShopEvent.BuyItem, OnBuyItem);
            CatDataModel.Instance.AddEvent(CatEvent.CatDecompose, OnCatDecompose);
            EventManager.Instance.GlobalDispatcher.AddEvent(GlobalEvent.PAY_SUCC, OnPayReward);
        }

        public override void RemoveEvents()
        {
            base.RemoveEvents();
            ShopDataModel.Instance.RemoveEvent(ShopEvent.ShopData, OnShopData);
            ShopDataModel.Instance.RemoveEvent(ShopEvent.BuyItem, OnBuyItem);
            CatDataModel.Instance.RemoveEvent(CatEvent.CatDecompose, OnCatDecompose);
            EventManager.Instance.GlobalDispatcher.RemoveEvent(GlobalEvent.PAY_SUCC, OnPayReward);
        }

        private void OnPayReward(IEventData value)
        {
            int itemId = int.Parse((value as EventData).Integer.ToString());
            PayXDM payXDM = XTable.PayXTable.GetByID(itemId);
            List<ItemInfo> itemInfos = new List<ItemInfo>();
            ItemInfo itemInfo = new ItemInfo();
            itemInfo.ItemCfgId = ItemIDConst.Diamond;
            itemInfo.ItemNum = payXDM.GemReward;
            itemInfos.Add(itemInfo);
            KUIWindow.OpenWindow<GetItemTipsWindow>(itemInfos);
        }

        private void OnCatDecompose(IEventData value)
        {
            OnCatInit();
            List<ItemInfo> listInfo = new List<ItemInfo>();
            ItemInfo info = new ItemInfo();
            info.ItemCfgId = ItemIDConst.SoulStone;
            info.ItemNum = (value as EventData).Integer;
            listInfo.Add(info);
            KUIWindow.OpenWindow<GetItemTipsWindow>(listInfo);
        }

        private void OnShopData(IEventData value)
        {
            List<ShopDataVO> lstShopData = new List<ShopDataVO>();
            lstShopData = ShopDataModel.Instance._allShop[(value as EventData).Integer];
            if (_shopType > 1)
            {
                if ((value as EventData).Integer == ShopIDConst.Attire && SpaceDataModel.Instance.mGender != GenderConst.All)
                {
                    List<ShopDataVO> lstShopVO = new List<ShopDataVO>();
                    for (int i = 0; i < lstShopData.Count; i++)
                    {
                        if (lstShopData[i].itemXDM.RoleType == SpaceDataModel.Instance.mGender)
                            lstShopVO.Add(lstShopData[i]);
                    }
                    _waresList.DataArray = lstShopVO;
                }
                else
                {
                    _waresList.DataArray = lstShopData;
                }
            }
            else
            {
                _propList.DataArray = lstShopData;
            }
        }

        private void OnBuyItem()
        {
            OnBuyClose();
            RefreshView();
            KUIWindow.OpenWindow<GetItemTipsWindow>(_curShopVO.mShow);
        }

        private void BuyRenderHandler(UIListItem item, int index)
        {
            ItemInfo vo = item.dataSource as ItemInfo;
            if (vo == null)
                return;
            item.GetComp<Image>("IconTool").overrideSprite = KIconManager.Instance.GetItemIcon(vo.ItemCfgId);
            item.GetComp<Text>("Text").text = vo.ItemNum.ToString();
        }

        private void PointerHandler(UIListItem item, int index)
        {
            ShopDataVO vo = item.dataSource as ShopDataVO;
            if (vo == null)
                return;
        }

        private void RenderHandler(UIListItem item, int index)
        {
            ShopDataVO vo = item.dataSource as ShopDataVO;
            if (vo == null)
                return;
            item.GetComp<Text>("Text").text = KLocalization.GetLocalString(vo.mNameId);
            item.GetGameObject("RewardIcon").SetActive(_shopId == ShopIDConst.Special);
            item.GetGameObject("Icon").SetActive(_shopId != ShopIDConst.Special);
            item.GetGameObject("Image1").SetActive(_shopType != ShopTypeConst.Diamond);
            item.GetGameObject("Image2").SetActive(_shopType == ShopTypeConst.Diamond);
            GameObject obj1= item.GetGameObject("Black");
            item.GetGameObject("Surplus").SetActive(vo.mLimitedType > 0);
            if (_shopType == ShopTypeConst.Diamond)
            {
                obj1.SetActive(false);
                // if (IAPSdk.Instance.IsInitialized())
                // {
                //     PayXDM payxdm = XTable.PayXTable.GetByID(vo.mPayID);
                //     item.GetComp<Text>("Image2/Value").text = IAPSdk.Instance.GetProductsLocalizedPriceString(payxdm.BundleID);
                // }
                // else
                // {
                //     item.GetComp<Text>("Image2/Value").text = "￥ " + vo.mByCost.ItemNum;
                // }
            }
            else
            {
                item.GetComp<Image>("Image1/Back01").overrideSprite = KIconManager.Instance.GetItemIcon(vo.mByCost.ItemCfgId);
                item.GetComp<Text>("Image1/Back01/Value").text = vo.mByCost.ItemNum.ToString();
                obj1.SetActive(vo.mItemNum == 0 && vo.mLimitedType > 0);
                item.GetComp<Text>("Surplus/Text").text = KLocalization.GetLocalString(52192) + vo.mItemNum + "/" + vo.mLimitedNum;
            }
            if (_shopId == ShopIDConst.Special)
            {
                List<Image> imageRewardIcon = new List<Image>();
                for (int i = 0; i < item.GetGameObject("RewardIcon").transform.childCount; i++)
                {
                    imageRewardIcon.Add(item.GetComp<Image>("RewardIcon/Icon" + (i + 1)));
                    item.GetComp<Button>("RewardIcon/Icon" + (i + 1)).onClick.RemoveAllListeners();
                    item.GetComp<Button>("RewardIcon/Icon" + (i + 1)).onClick.AddListener(() => { OnBuyBtn(vo); });
                }
                for (int i = 0; i < imageRewardIcon.Count; i++)
                {
                    if (i < vo.mShow.Count)
                    {
                        imageRewardIcon[i].overrideSprite = KIconManager.Instance.GetItemIcon(vo.mShow[i].ItemCfgId);
                        imageRewardIcon[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        imageRewardIcon[i].gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                item.GetComp<Image>("Icon").overrideSprite = KIconManager.Instance.GetItemIcon(vo.mIcon);
                item.GetComp<Button>("Icon").onClick.RemoveAllListeners();
                item.GetComp<Button>("Icon").onClick.AddListener(() => { OnBuyBtn(vo); });
            }
        }

        private void OnBuyBtn(ShopDataVO vo)
        {
            _curShopVO = vo;
            if (_shopType == ShopTypeConst.Diamond)
            {
                Debuger.Log(vo.mItemId);
                Debuger.Log(vo.mPayID);
                KUIWindow.OpenWindow<PayWindow>(vo.mPayID);
                //ToastBox.ShowText(KLocalization.GetLocalString(52195));
            }
            else
            {
                if (_curShopVO.mByCost.ItemCfgId == ItemIDConst.Gold)
                {
                    if (_curShopVO.mByCost.ItemNum > PlayerDataModel.Instance.mPlayerData.mGold)
                        ToastBox.ShowText(KLocalization.GetLocalString(57110));
                    else
                        OnBuyItemChange();
                }
                else if (_curShopVO.mByCost.ItemCfgId == ItemIDConst.Diamond)
                {
                    if (_curShopVO.mByCost.ItemNum > PlayerDataModel.Instance.mPlayerData.mDiamond)
                        ToastBox.ShowText(KLocalization.GetLocalString(57109));
                    else
                        OnBuyItemChange();
                }
                else if(_curShopVO.mByCost.ItemCfgId == ItemIDConst.FriendPoint)
                {
                    if (_curShopVO.mByCost.ItemNum > PlayerDataModel.Instance.mPlayerData.mFriendPoints)
                        ToastBox.ShowText(KLocalization.GetLocalString(57112));
                    else
                        OnBuyItemChange();
                }
                else if(_curShopVO.mByCost.ItemCfgId == ItemIDConst.CharmBadge)
                {
                    if (_curShopVO.mByCost.ItemNum > PlayerDataModel.Instance.mPlayerData.mCharmMetal)
                        ToastBox.ShowText(KLocalization.GetLocalString(57114));
                    else
                        OnBuyItemChange();
                }
                else if(_curShopVO.mByCost.ItemCfgId == ItemIDConst.SoulStone)
                {
                    if (_curShopVO.mByCost.ItemNum > PlayerDataModel.Instance.mPlayerData.mSoulStone)
                        ToastBox.ShowText(KLocalization.GetLocalString(57113));
                    else
                        OnBuyItemChange();
                }
                else
                {
                    if (_curShopVO.mByCost.ItemNum > BagDataModel.Instance.GetItemCountById(_curShopVO.mByCost.ItemCfgId))
                        ToastBox.ShowText(KLocalization.GetLocalString(52194));
                    else
                        OnBuyItemChange();
                }
            }
        }

        private void OnBuyItemChange()
        {
            _buyItemNum = 1;
            _waresBuy.SetActive(true);
            _buyItemName.text = KLocalization.GetLocalString(_curShopVO.mNameId);
            _waresBuyList.DataArray = _curShopVO.mShow;
            _buyBtnIcon.overrideSprite = KIconManager.Instance.GetItemIcon(_curShopVO.mByCost.ItemCfgId);
            OnBuyInit();
        }

        private void OnBuyInit()
        {
            _buyNum.text = _buyItemNum.ToString();
            _buyBtnNum.text = (_curShopVO.mByCost.ItemNum * _buyItemNum).ToString();
            if (_buyItemNum == 1)
                _subImg.material = Resources.Load<Material>("Materials/UIGray");
            else
                _subImg.material = null;

            if (_curShopVO.mLimitedType > 0 && _buyItemNum >= _curShopVO.mItemNum)
                _addImg.material = Resources.Load<Material>("Materials/UIGray");
            else
            {
                if (_curShopVO.mByCost.ItemCfgId == ItemIDConst.Gold)
                {
                    if (!isBuy(PlayerDataModel.Instance.mPlayerData.mGold))
                        _addImg.material = Resources.Load<Material>("Materials/UIGray");
                    else
                        _addImg.material = null;
                }
                else if (_curShopVO.mByCost.ItemCfgId == ItemIDConst.Diamond)
                {
                    if (!isBuy(PlayerDataModel.Instance.mPlayerData.mDiamond))
                        _addImg.material = Resources.Load<Material>("Materials/UIGray");
                    else
                        _addImg.material = null;
                }
                else if (_curShopVO.mByCost.ItemCfgId == ItemIDConst.FriendPoint)
                {
                    if (!isBuy(PlayerDataModel.Instance.mPlayerData.mFriendPoints))
                        _addImg.material = Resources.Load<Material>("Materials/UIGray");
                    else
                        _addImg.material = null;
                }
                else if (_curShopVO.mByCost.ItemCfgId == ItemIDConst.SoulStone)
                {
                    if (!isBuy(PlayerDataModel.Instance.mPlayerData.mSoulStone))
                        _addImg.material = Resources.Load<Material>("Materials/UIGray");
                    else
                        _addImg.material = null;
                }
                else if (_curShopVO.mByCost.ItemCfgId == ItemIDConst.CharmBadge)
                {
                    if (!isBuy(PlayerDataModel.Instance.mPlayerData.mCharmMetal))
                        _addImg.material = Resources.Load<Material>("Materials/UIGray");
                    else
                        _addImg.material = null;
                }
                else
                {
                    if (!isBuy(BagDataModel.Instance.GetItemCountById(_curShopVO.mByCost.ItemCfgId)))
                        _addImg.material = Resources.Load<Material>("Materials/UIGray");
                    else
                        _addImg.material = null;
                }
            }
        }

        private bool isBuy(int num)
        {
            return _buyItemNum * _curShopVO.mByCost.ItemNum <= num - _curShopVO.mByCost.ItemNum;
        }

        private void OnSubBtn()
        {
            if (_buyItemNum > 1)
            {
                _buyItemNum--;
                OnBuyInit();
            }
        }

        private void OnAddBtn()
        {
            if (_curShopVO.mLimitedType > 0)
            {
                if (_buyItemNum < _curShopVO.mItemNum)
                {
                    if (_curShopVO.mByCost.ItemCfgId == ItemIDConst.Gold)
                    {
                        if (isBuy(PlayerDataModel.Instance.mPlayerData.mGold))
                        {
                            _buyItemNum++;
                            OnBuyInit();
                        }
                        else
                        {
                            ToastBox.ShowText(KLocalization.GetLocalString(57110));
                        }
                    }
                    else if (_curShopVO.mByCost.ItemCfgId == ItemIDConst.Diamond)
                    {
                        if (isBuy(PlayerDataModel.Instance.mPlayerData.mDiamond))
                        {
                            _buyItemNum++;
                            OnBuyInit();
                        }
                        else
                        {
                            ToastBox.ShowText(KLocalization.GetLocalString(57109));
                        }
                    }
                    else if (_curShopVO.mByCost.ItemCfgId == ItemIDConst.FriendPoint)
                    {
                        if (isBuy(PlayerDataModel.Instance.mPlayerData.mFriendPoints))
                        {
                            _buyItemNum++;
                            OnBuyInit();
                        }
                        else
                        {
                            ToastBox.ShowText(KLocalization.GetLocalString(57112));
                        }
                    }
                    else if (_curShopVO.mByCost.ItemCfgId == ItemIDConst.SoulStone)
                    {
                        if (isBuy(PlayerDataModel.Instance.mPlayerData.mSoulStone))
                        {
                            _buyItemNum++;
                            OnBuyInit();
                        }
                        else
                        {
                            ToastBox.ShowText(KLocalization.GetLocalString(57113));
                        }
                    }
                    else if (_curShopVO.mByCost.ItemCfgId == ItemIDConst.CharmBadge)
                    {
                        if (isBuy(PlayerDataModel.Instance.mPlayerData.mCharmMetal))
                        {
                            _buyItemNum++;
                            OnBuyInit();
                        }
                        else
                        {
                            ToastBox.ShowText(KLocalization.GetLocalString(57114));
                        }
                    }
                    else
                    {
                        if (isBuy(BagDataModel.Instance.GetItemCountById(_curShopVO.mByCost.ItemCfgId)))
                        {
                            _buyItemNum++;
                            OnBuyInit();
                        }
                        else
                        {
                            ToastBox.ShowText(KLocalization.GetLocalString(52194));
                        }
                    }
                }
                else
                {
                    ToastBox.ShowText(KLocalization.GetLocalString(52193));
                }
            }
            else
            {
                if (_curShopVO.mByCost.ItemCfgId == ItemIDConst.Gold)
                {
                    if (isBuy(PlayerDataModel.Instance.mPlayerData.mGold))
                    {
                        _buyItemNum++;
                        OnBuyInit();
                    }
                    else
                    {
                        ToastBox.ShowText(KLocalization.GetLocalString(57110));
                    }
                }
                else if (_curShopVO.mByCost.ItemCfgId == ItemIDConst.Diamond)
                {
                    if (isBuy(PlayerDataModel.Instance.mPlayerData.mDiamond))
                    {
                        _buyItemNum++;
                        OnBuyInit();
                    }
                    else
                    {
                        ToastBox.ShowText(KLocalization.GetLocalString(57109));
                    }
                }
                else if (_curShopVO.mByCost.ItemCfgId == ItemIDConst.FriendPoint)
                {
                    if (isBuy(PlayerDataModel.Instance.mPlayerData.mFriendPoints))
                    {
                        _buyItemNum++;
                        OnBuyInit();
                    }
                    else
                    {
                        ToastBox.ShowText(KLocalization.GetLocalString(57112));
                    }
                }
                else if (_curShopVO.mByCost.ItemCfgId == ItemIDConst.SoulStone)
                {
                    if (isBuy(PlayerDataModel.Instance.mPlayerData.mSoulStone))
                    {
                        _buyItemNum++;
                        OnBuyInit();
                    }
                    else
                    {
                        ToastBox.ShowText(KLocalization.GetLocalString(57113));
                    }
                }
                else if (_curShopVO.mByCost.ItemCfgId == ItemIDConst.CharmBadge)
                {
                    if (isBuy(PlayerDataModel.Instance.mPlayerData.mCharmMetal))
                    {
                        _buyItemNum++;
                        OnBuyInit();
                    }
                    else
                    {
                        ToastBox.ShowText(KLocalization.GetLocalString(57114));
                    }
                }
                else
                {
                    if (isBuy(BagDataModel.Instance.GetItemCountById(_curShopVO.mByCost.ItemCfgId)))
                    {
                        _buyItemNum++;
                        OnBuyInit();
                    }
                    else
                    {
                        ToastBox.ShowText(KLocalization.GetLocalString(52194));
                    }
                }
            }
        }

        private void OnBuyBtn()
        {
            GameApp.Instance.GameServer.ReqBuyShopItem(_curShopVO.mItemId, _buyItemNum);
        }

        public void RefreshView()
        {
            OnBuyClose();
            _propTog.SetActive(_shopType == ShopTypeConst.Prop);
            _soulStoneTog.SetActive(_shopType == ShopTypeConst.SoulStone);
            switch (_shopType)
            {
                case 0:
                    _waresObj.SetActive(true);
                    _waresList.gameObject.SetActive(false);
                    _gain.SetActive(false);
                    _propList.gameObject.SetActive(true);
                    int index = int.Parse(data.ToString());
                    if (index > ShopIDConst.CharmMedal)
                        index = ShopIDConst.Special;
                    OnPropChange(_propToggles[index - 1]);
                    _propToggles[index - 1].isOn = true;
                    break;
                case 1:
                    _shopId = ShopIDConst.SoulStone;
                    _waresObj.SetActive(true);
                    _waresList.gameObject.SetActive(false);
                    OnSoulStoneChange(_soulStoneToggles[_soulStoneType]);
                    _soulStoneToggles[_soulStoneType].isOn = true;
                    _backImg.overrideSprite = KIconManager.Instance.GetItemIcon(ItemIDConst.SoulStone);
                    _backNum.text = PlayerDataModel.Instance.mPlayerData.mSoulStone.ToString();
                    break;
                case 2:
                    _shopId = ShopIDConst.Diamond;
                    _waresObj.SetActive(false);
                    _waresList.gameObject.SetActive(true);
                    if (ShopDataModel.Instance._allShop.ContainsKey(ShopIDConst.Diamond))
                        _waresList.DataArray = ShopDataModel.Instance._allShop[ShopIDConst.Diamond];
                    else
                        GameApp.Instance.GameServer.ReqShopData(ShopIDConst.Diamond);
                    _backImg.overrideSprite = KIconManager.Instance.GetItemIcon(ItemIDConst.Diamond);
                    _backNum.text = PlayerDataModel.Instance.mPlayerData.mDiamond.ToString();
                    break;
                case 3:
                    _shopId = ShopIDConst.Attire;
                    _waresObj.SetActive(false);
                    _waresList.gameObject.SetActive(true);
                    if (ShopDataModel.Instance._allShop.ContainsKey(ShopIDConst.Attire))
                    {
                        List<ShopDataVO> lstShopData = new List<ShopDataVO>();
                        lstShopData = ShopDataModel.Instance._allShop[ShopIDConst.Attire];
                        if (SpaceDataModel.Instance.mGender != GenderConst.All)
                        {
                            List<ShopDataVO> lstShopVO = new List<ShopDataVO>();
                            for (int i = 0; i < lstShopData.Count; i++)
                            {
                                if (lstShopData[i].itemXDM.RoleType == SpaceDataModel.Instance.mGender)
                                    lstShopVO.Add(lstShopData[i]);
                            }
                            _waresList.DataArray = lstShopVO;
                        }
                        else
                        {
                            _waresList.DataArray = lstShopData;
                        }
                    }
                    else
                        GameApp.Instance.GameServer.ReqShopData(ShopIDConst.Attire);
                    _backImg.overrideSprite = KIconManager.Instance.GetItemIcon(ItemIDConst.Gold);
                    _backNum.text = PlayerDataModel.Instance.mPlayerData.mGold.ToString();
                    break;
            }
        }

        private void OnBuyClose()
        {
            _waresBuy.SetActive(false);
        }

        private void OnPropChange(Toggle tog)
        {
            switch (tog.name)
            {
                case "Tog1":
                    data = ShopIDConst.Special;
                    _shopId = ShopIDConst.Special;
                    if (ShopDataModel.Instance._allShop.ContainsKey(ShopIDConst.Special))
                        _propList.DataArray = ShopDataModel.Instance._allShop[ShopIDConst.Special];
                    else
                        GameApp.Instance.GameServer.ReqShopData(ShopIDConst.Special);
                    _backImg.overrideSprite = KIconManager.Instance.GetItemIcon(ItemIDConst.Diamond);
                    _backNum.text = PlayerDataModel.Instance.mPlayerData.mDiamond.ToString();
                    break;
                case "Tog2":
                    data = ShopIDConst.FriendPoint;
                    _shopId = ShopIDConst.FriendPoint;
                    if (ShopDataModel.Instance._allShop.ContainsKey(ShopIDConst.FriendPoint))
                        _propList.DataArray = ShopDataModel.Instance._allShop[ShopIDConst.FriendPoint];
                    else
                        GameApp.Instance.GameServer.ReqShopData(ShopIDConst.FriendPoint);
                    _backImg.overrideSprite = KIconManager.Instance.GetItemIcon(ItemIDConst.FriendPoint);
                    _backNum.text = PlayerDataModel.Instance.mPlayerData.mFriendPoints.ToString();
                    break;
                case "Tog3":
                    data = ShopIDConst.CharmMedal;
                    _shopId = ShopIDConst.CharmMedal;
                    if (ShopDataModel.Instance._allShop.ContainsKey(ShopIDConst.CharmMedal))
                        _propList.DataArray = ShopDataModel.Instance._allShop[ShopIDConst.CharmMedal];
                    else
                        GameApp.Instance.GameServer.ReqShopData(ShopIDConst.CharmMedal);
                    _backImg.overrideSprite = KIconManager.Instance.GetItemIcon(ItemIDConst.CharmBadge);
                    _backNum.text = PlayerDataModel.Instance.mPlayerData.mCharmMetal.ToString();
                    break;
            }
        }

        private void OnSoulStoneChange(Toggle tog)
        {
            switch (tog.name)
            {
                case "Tog1":
                    _soulStoneType = SoulStoneConst.SoulStoneExchange;
                    _gain.SetActive(false);
                    _propList.gameObject.SetActive(true);
                    if (ShopDataModel.Instance._allShop.ContainsKey(ShopIDConst.SoulStone))
                        _propList.DataArray = ShopDataModel.Instance._allShop[ShopIDConst.SoulStone];
                    else
                        GameApp.Instance.GameServer.ReqShopData(ShopIDConst.SoulStone);
                    break;
                case "Tog2":
                    _soulStoneType = SoulStoneConst.SoulStoneObtain;
                    _gain.SetActive(true);
                    _propList.gameObject.SetActive(false);
                    OnCatInit();
                    break;
            }
        }

        private int OnCatSort(CatDataVO v1, CatDataVO v2)
        {
            int index = 0;
            index = -v2.mCatXDM.Rarity.CompareTo(v1.mCatXDM.Rarity);
            if (index != 0)
                return index;
            index = v1.mCatXDM.MainColor.CompareTo(v2.mCatXDM.MainColor);
            if (index != 0)
                return index;
            index = -v1.mCatInfo.CatCfgId.CompareTo(v2.mCatInfo.CatCfgId);
            if (index != 0)
                return index;
            index = -v1.mCatInfo.Id.CompareTo(v2.mCatInfo.Id);
            return index;
        }

        private void CurPointerHandler(UIListItem item, int index)
        {
            CatDataVO vo = item.dataSource as CatDataVO;
            if (vo == null)
                return;
            _curListCatVO.Remove(vo);
            _curList.DataArray = _curListCatVO;
            List<CatDataVO> lstVO = new List<CatDataVO>();
            lstVO = CatDataModel.Instance.GetAllCatDataVO();
            lstVO.Sort(OnCatSort);
            _catList.Refresh(lstVO);
        }

        private void CurRenderHandler(UIListItem item, int index)
        {
            item.gameObject.SetActive(true);
            CatDataVO vo = item.dataSource as CatDataVO;
            item.GetGameObject("Cat").SetActive(vo != null);
            item.GetGameObject("Empty").SetActive(vo == null);
            if (vo == null)
                return;
            item.GetComp<Image>("Cat").overrideSprite = KIconManager.Instance.GetCatIcon(vo.mCatXDM.Icon);
            KUIImage frameImage = item.GetComp<KUIImage>("Cat/Light");
            frameImage.gameObject.SetActive(vo.mCatXDM.Rarity != CatRarityConst.N);
            frameImage.ShowSprite(vo.mCatXDM.Rarity - 1);
            item.GetComp<Text>("Cat/Name").text = vo.mName;
            item.GetComp<Text>("Cat/Item/Text").text = vo.mCatXDM.Price.ToString();
        }

        private void CatPointerHandler(UIListItem item, int index)
        {
            CatDataVO vo = item.dataSource as CatDataVO;
            if (vo == null)
                return;
            if (!SpaceDataModel.Instance.OnIsExhibition(vo.mCatInfo.Id) && vo.mCatInfo.State == CatStateConst.None)
            {
                if (_curListCatVO.Count < 6)
                {
                    if (IsCurCat(vo))
                    {
                        item.GetGameObject("OK").SetActive(false);
                        _curListCatVO.Remove(vo);
                        _curList.DataArray = _curListCatVO;
                    }
                    else
                    {
                        item.GetGameObject("OK").SetActive(true);
                        _curListCatVO.Add(vo);
                        _curList.DataArray = _curListCatVO;
                    }
                }
                else
                {
                    ToastBox.ShowText(KLocalization.GetLocalString(52196));
                }
            }
        }

        private bool IsCurCat(CatDataVO vo)
        {
            for (int i = 0; i < _curListCatVO.Count; i++)
            {
                if (_curListCatVO[i].mCatInfo.Id == vo.mCatInfo.Id)
                    return true;
            }
            return false;
        }

        private void CatRenderHandler(UIListItem item, int index)
        {
            CatDataVO vo = item.dataSource as CatDataVO;
            if (vo == null)
                return;
            item.GetGameObject("OK").SetActive(IsCurCat(vo));
            item.GetComp<Image>("Icon").overrideSprite = KIconManager.Instance.GetCatIcon(vo.mCatXDM.Icon);
            KUIImage frameImage = item.GetComp<KUIImage>("Frame");
            frameImage.gameObject.SetActive(vo.mCatXDM.Rarity != CatRarityConst.N);
            frameImage.ShowSprite(vo.mCatXDM.Rarity - 1);
            GameObject obj = item.GetGameObject("State");
            Text stateText = item.GetComp<Text>("State/Item/Text");
            if (SpaceDataModel.Instance.OnIsExhibition(vo.mCatInfo.Id))
            {
                obj.SetActive(true);
                stateText.text = KLocalization.GetLocalString(54168);
            }
            else
            {
                obj.SetActive(vo.mCatInfo.State != CatStateConst.None);
                switch (vo.mCatInfo.State)
                {
                    case CatStateConst.None:
                        break;
                    case CatStateConst.Cattery:
                        stateText.text = KLocalization.GetLocalString(54108);
                        break;
                    case CatStateConst.Explore:
                        stateText.text = KLocalization.GetLocalString(54110);
                        break;
                    case CatStateConst.Foster:
                        stateText.text = KLocalization.GetLocalString(54109);
                        break;
                }
            }
        }

        private void OnCatInit()
        {
            if (_curListCatVO != null)
                _curListCatVO.Clear();
            _curListCatVO = new List<CatDataVO>();
            _curList.DataArray = _curListCatVO;
            List<CatDataVO> lstVO = new List<CatDataVO>();
            lstVO = CatDataModel.Instance.GetAllCatDataVO();
            lstVO.Sort(OnCatSort);
            _catList.DataArray = lstVO;
        }

        private void OnCanel()
        {
            if (_curListCatVO.Count > 0)
                OnCatInit();
        }

        private void OnExplain()
        {
            if (_curListCatVO.Count > 0)
            {
                List<int> lstId = new List<int>();
                for (int i = 0; i < _curListCatVO.Count; i++)
                    lstId.Add(_curListCatVO[i].mCatInfo.Id);
                GameApp.Instance.GameServer.ReqCatDecompose(lstId);
            }
        }
    }
}

