// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "FormulaShopItem" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class FormulaShopItem : KUIItem
    {
        #region Field

        private Text _nameText;
        private Image _iconImage;

        private Text _starText;

        private Text _addExpText;

        private Button _exchangeButton;
        private Text _tipsText;
        private Image _getImage;

        private GameObject _black;
        private GameObject _bottom;

        private KItemFormula _showFormula;

        #endregion

        #region Method

        public void ShowFormula(KItemFormula formula)
        {
            _showFormula = formula;
            ShowFormula();
        }

        private void ShowFormula()
        {
            if (_showFormula != null)
            {
                _iconImage.overrideSprite = KIconManager.Instance.GetBuildingIcon(_showFormula.GetBuildindIcon());
                _nameText.text = _showFormula.itemName;

                if (_showFormula.isHave)
                {
                    _bottom.SetActive(false);
                    _black.SetActive(true);
                    _exchangeButton.gameObject.SetActive(false);
                    _tipsText.gameObject.SetActive(false);
                    _getImage.gameObject.SetActive(true);
                }
                else if (_showFormula.isLock)
                {
                    _bottom.SetActive(false);
                    _black.SetActive(true);
                    _exchangeButton.gameObject.SetActive(false);
                    _tipsText.gameObject.SetActive(true);
                    _tipsText.text = string.Format("通关消除关卡\n第{0}关解锁", _showFormula.unlockLevel);
                    _getImage.gameObject.SetActive(false);
                }
                else
                {
                    _bottom.SetActive(true);
                    _black.SetActive(false);

                    _addExpText.text = string.Format("+{0:N0}", 999999);// _showFormula.getExp.ToString("+ N0");
                    if (PlayerDataModel.Instance.mPlayerData.mStar>= _showFormula.costStar)
                    {
                        _starText.text = string.Format("<color=#70563b>{0}</color><color=#70563b>/{1}</color>", PlayerDataModel.Instance.mPlayerData.mStar, _showFormula.costStar);
                    }
                    else
                    {
                        _starText.text = string.Format("<color=#f93535>{0}</color><color=#70563b>/{1}</color>", PlayerDataModel.Instance.mPlayerData.mStar, _showFormula.costStar);
                    }
                    _exchangeButton.gameObject.SetActive(true);
                    _tipsText.gameObject.SetActive(false);
                    _getImage.gameObject.SetActive(false);
                }
            }
        }

        protected override void Refresh()
        {
            ShowFormula(data as KItemFormula);
        }

        #endregion

        #region Action

        private void OnExchangeBtnClick()
        {
            if (_showFormula != null)
            {
                if (PlayerDataModel.Instance.mPlayerData.mStar >= _showFormula.costStar)
                {
                    KUIWindow.OpenWindow<MessageBox>(new MessageBox.Data
                    {
                        content = string.Format("是否用{0}颗星星兑换{1}配方", _showFormula.costStar, _showFormula.itemName),
                        onConfirm = OnConfirmExchange,
                        onCancel = OnCancelExchange,
                    });
                }
                else
                {
                    KUIWindow.OpenWindow<MessageBox>(new MessageBox.Data
                    {
                        content = string.Format("目前星星数不足,是否前往消除关卡获取"),
                        onConfirm = OnConfirmGoto,
                        onCancel = OnCancelGoto,
                    });
                }
            }
        }

        private void OnConfirmExchange()
        {
            if (_showFormula != null)
            {
                KUser.ExchangeFormula(_showFormula.itemID, OnExchangeCallback);
            }
        }

        private void OnCancelExchange()
        {
        }

        private void OnConfirmGoto()
        {
            KUIWindow.OpenWindow<MapSelectWindow>();
        }

        private void OnCancelGoto()
        {

        }

        private void OnExchangeCallback(int code, string message, object data)
        {
            if (code == 0)
            {
                KUIWindow.OpenWindow<MessageBox>(new MessageBox.Data
                {
                    content = "兑换成功",
                });
                KUIWindow.GetWindow<FormulaShopWindow>().RefreshItems();
                //Refresh();
            }
            else
            {
                Debug.Log("OnExchangeCallback:" + code);
            }
        }

        #endregion

        #region Unity

        private void Awake()
        {
            _iconImage = Find<Image>("Item/Up/Icon");
            _nameText = Find<Text>("Item/Name");
            _starText = Find<Text>("Item/Bottom/Star/Value");
            _addExpText = Find<Text>("Item/Bottom/Blue/Text");

            _bottom = Find("Item/Bottom");

            _tipsText = Find<Text>("Item/Tips");
            _getImage = Find<Image>("Item/Get");
            _black = Find("Item/Black");

            _exchangeButton = Find<Button>("Item/Button");
            _exchangeButton.onClick.AddListener(this.OnExchangeBtnClick);
        }

        #endregion
    }
}

