
using Game.DataModel;
using UnityEngine;
/** 
*FileName:     BuyPopNumWindow.View.cs 
*Author:       LiMuChen 
*Version:      1.0 
*UnityVersion：5.6.3f1
*Date:         2017-10-31 
*Description:    
*History: 
*/
using UnityEngine.UI;

namespace Game.UI
{
    partial class BuyPopNumWindow
    {
        #region Field
        private Button _btnClose;
        private Button _btnBgClose;
        private int buyNum = 1;
        private Text _textTitle;
        private Image _imageItemIcon;
        private Text _textStoneNum;
        private UIButtonExtension _btnAdd;
        private UIButtonExtension _btnRemove;
        private Text _textToolNum;
        private Button _btnBuy;
        private Image _imageMoneyType;
        private Image _imageSub;
        private Image _imageAdd;
        private Text _textContent;
        //购买礼包新增
        private Transform tranIcon;
        private Transform _giftGroup;

        private Transform[] _transShowReward;
        #endregion

        #region Method

        public void InitView()
        {
            _btnClose = Find<Button>("BlackMask/ButtonQuit");
            _btnClose.onClick.AddListener(OnCloseBtnClick);
            _btnBgClose = Find<Button>("BgBlack");
            _btnBgClose.onClick.AddListener(OnCloseBtnClick);
            _textTitle = Find<Text>("BlackMask/Title");
            tranIcon = Find<Transform>("BlackMask/Back02");
            _imageItemIcon = Find<Image>("BlackMask/Back02/IconTool");
            _btnAdd = Find<UIButtonExtension>("BlackMask/ButtonAdd");
            _btnAdd.onClick.AddListener(OnAddBtnClick);
            _btnRemove = Find<UIButtonExtension>("BlackMask/ButtonSub");
            _btnRemove.onClick.AddListener(OnRemoveBtnClick);
            _btnAdd.onLongClick.AddListener(OnAddBtnClick);
            _btnRemove.onLongClick.AddListener(OnRemoveBtnClick);
            _textToolNum = Find<Text>("BlackMask/ToolNum");
            _textStoneNum = Find<Text>("BlackMask/ButtonBuy/StoneNum");
            _btnBuy = Find<Button>("BlackMask/ButtonBuy");
            _btnBuy.onClick.AddListener(OnBuyBtnClick);
            _imageMoneyType = Find<Image>("BlackMask/ButtonBuy/Stone");
            _imageSub = Find<Image>("BlackMask/ButtonSub");
            _imageAdd = Find<Image>("BlackMask/ButtonAdd");
            _textContent = Find<Text>("BlackMask/Title (1)");
            _textContent.text = string.Empty;

            _giftGroup = Find<Transform>("BlackMask/Grid");

            _transShowReward = new Transform[_giftGroup.childCount];
            for (int i = 0; i < _transShowReward.Length; i++)
            {
                _transShowReward[i] = _giftGroup.GetChild(i);
            }
        }

        public void RefreshView()
        {
            ShopXDM shopXDM = XTable.ShopXTable.GetByID(_buyPopNumData.itemId);
            var item = KItemManager.Instance.GetItem(shopXDM.CommodityId);
            _textToolNum.text = buyNum.ToString();
            _textContent.text = item.description;
            _textTitle.text = item.itemName;
            _textStoneNum.text = (shopXDM.BuyCost[1] * buyNum).ToString();
            _imageItemIcon.overrideSprite = KIconManager.Instance.GetItemIcon(item.itemID);
            _imageMoneyType.overrideSprite = KIconManager.Instance.GetItemIcon(shopXDM.BuyCost[0]);

            if (_buyPopNumData.type != 1)
            {

                _giftGroup.gameObject.SetActive(false);
                tranIcon.gameObject.SetActive(true);
                _textContent.gameObject.SetActive(true);
            }
            else
            {
                RefreshReward();
                _giftGroup.gameObject.SetActive(true);
                tranIcon.gameObject.SetActive(false);
                _textContent.gameObject.SetActive(false);
            }
            if (buyNum <= 1)
            {
                _imageSub.material = Resources.Load<Material>("Materials/UIGray");
            }
            else
            {
                _imageSub.material = null;
            }

            var itemMoneyType = BagDataModel.Instance.GetItemCountById(item.itemID);
            int count = shopXDM.BuyCost[1] * (buyNum + 1);
            if (itemMoneyType > count)
            {
                _imageAdd.material = Resources.Load<Material>("Materials/UIGray");
            }
            else
            {
                _imageAdd.material = null;
            }
        }
        private void RefreshReward()
        {

            for (int i = 0; i < _transShowReward.Length; i++)
            {
                if (i < _buyPopNumData.itemInfor.Length)
                {
                    _transShowReward[i].Find("IconTool").GetComponent<Image>().overrideSprite = KIconManager.Instance.GetItemIcon(_buyPopNumData.itemInfor[i].itemID);
                    _transShowReward[i].Find("Text").GetComponent<Text>().text = (_buyPopNumData.itemInfor[i].itemCount).ToString();
                    _transShowReward[i].gameObject.SetActive(true);
                }
                else
                {
                    _transShowReward[i].gameObject.SetActive(false);
                }
            }
        }
        public override void OnDisable()
        {
            buyNum = 1;
        }

        #endregion
    }
}

