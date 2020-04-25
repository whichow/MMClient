using Game.DataModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public partial class PhotoShopWindow
    {
        #region Field
        private Button _quiteBtn;

        private Button _lowBtn;

        private Button _middleBtn;

        private Button _highBtn;
        private Button _diamondBtn;


        private Text _blueBackText;
        private Text _purpleBackText;
        private Text _diamondBackText;
        private Text _textLowCardDraw;
        private Text _textLowCardDrawAdd;
        private Text _textMidCardDraw;
        private Text _textHightCardDraw;
        #endregion



        #region Method
        private void InitView()
        {
            _quiteBtn = Find<Button>("ButtonBack");
            _quiteBtn.onClick.AddListener(this.OnQuitBtnClick);

            _lowBtn = Find<Button>("Wood01");
            _lowBtn.onClick.AddListener(this.SendBuyLowCat);

            _middleBtn = Find<Button>("Wood02");
            _middleBtn.onClick.AddListener(this.SendBuyMiddleCat);

            _highBtn = Find<Button>("Wood03");
            _highBtn.onClick.AddListener(this.SendBuyHightCat);

            _diamondBtn = Find<Button>("DiamondBack/ButtonAdd");
            _diamondBtn.onClick.AddListener(OnDiamondBtn);

            _blueBackText = Find<Text>("BlueBack/Text");
            _purpleBackText = Find<Text>("PurpleBack/Text");
            _diamondBackText = Find<Text>("DiamondBack/Text");
            _textLowCardDraw = Find<Text>("Wood01/PickBlueNum");
            _textLowCardDrawAdd = Find<Text>("Wood01/PickBlueNumAdd");
            _textMidCardDraw = Find<Text>("Wood02/PickBlueNumAdd");
            _textHightCardDraw = Find<Text>("Wood03/PickBlueNumAdd");
        }

        private void OnDiamondBtn()
        {
            KUIWindow.OpenWindow<ShopWindow>(ShopIDConst.Diamond);
        }

        private void RefreshView()
        {

            _blueBackText.text = GetBlueBlack();
            _purpleBackText.text = GetPurpoleBlack();
            _diamondBackText.text = GetDiamBlack();
            _textMidCardDraw.text = "1";
            _textHightCardDraw.text = XTable.ItemXTable.GetByID(ItemIDConst.MidCard).Cost.ToString();
            var lowCardNum = BagDataModel.Instance.GetItemCountById(ItemIDConst.LowCard);

            if (lowCardNum >= 5)
            {
                _textLowCardDraw.text = "5 抽";
                _textLowCardDrawAdd.text = "5";
            }
            else if (lowCardNum < 5 && lowCardNum > 0)
            {
                _textLowCardDraw.text = BagDataModel.Instance.GetItemCountById(ItemIDConst.LowCard) + " 抽";
                _textLowCardDrawAdd.text = BagDataModel.Instance.GetItemCountById(ItemIDConst.LowCard).ToString();
            }
            else
            {
                _textLowCardDraw.text = 1 + " 抽";
                _textLowCardDrawAdd.text = "1";
            }


        }

        #endregion





    }
}
