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
using UnityEngine.EventSystems;
using UnityEngine.UI;
using K.Extension;
namespace Game.UI
{
    public class OrnamentShopItem : KUIItem, IPointerClickHandler
    {
        #region Field

        private Text _nameText;
        private Image _iconImage;
        private KItemFormula _showFormula;
        private Transform _transOkBg;
        private Text _textLove;
        private Text _textWood;
        private Transform[] transConsItems;
        private Text _textCoin;
        private Text _textTime;

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
                //Debug.Log("building id:"+ _showFormula.buildingId);
                _textLove.text = string.Format("+{0:N0}", KItemManager.Instance.GetBuilding(_showFormula.buildingId).charm);
                _textCoin.text = string.Format("+{0:N0}", _showFormula.costCoin);
                _textTime.text = TimeExtension.ToTimeString(_showFormula.costTime);
                if (_showFormula.costItems.Length % 2 == 0)
                {
                    for (int i = 1; i <= _showFormula.costItems.Length; i++)
                    {
                        if (i % 2 == 0)
                        {
                            transConsItems[i / 2 - 1].GetComponent<Image>().overrideSprite = KIconManager.Instance.GetItemIcon(_showFormula.costItems[i - 2]);
                            if (KItemManager.Instance.GetItem(_showFormula.costItems[i - 2]).curCount >= _showFormula.costItems[i - 1])
                            {
                                transConsItems[i / 2 - 1].GetComponentInChildren<Text>().text = string.Format("<color=#70563b>{0}</color><color=#70563b>/{1}</color>", KItemManager.Instance.GetItem(_showFormula.costItems[i - 2]).curCount, _showFormula.costItems[i - 1]);
                            }
                            else
                            {
                                transConsItems[i / 2 - 1].GetComponentInChildren<Text>().text = string.Format("<color=#f93535>{0}</color><color=#70563b>/{1}</color>", KItemManager.Instance.GetItem(_showFormula.costItems[i - 2]).curCount, _showFormula.costItems[i - 1]);
                            }
                        }
                    }
                    for (int i = 0; i < transConsItems.Length; i++)
                    {
                        if (i < _showFormula.costItems.Length / 2)
                        {
                            transConsItems[i].gameObject.SetActive(true);
                        }
                        else
                        {
                            transConsItems[i].gameObject.SetActive(false);
                        }

                    }

                }
                else
                {
                    Debug.Log("消耗物品配置 id 数量 有问题" + _showFormula.costItems.Length);
                }



            }
        }
        public void ShowBgBlack(bool isShow)
        {
            _transOkBg.gameObject.SetActive(isShow);
        }
        protected override void Refresh()
        {
            ShowFormula();
        }

        #endregion

        #region Action

        public void PointerClick()
        {
            KUIWindow.GetWindow<OrnamentShopWindow>().SetItem(_showFormula, this);
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            KUIWindow.GetWindow<OrnamentShopWindow>().SetItem(_showFormula, this);
        }
        #endregion

        #region Unity

        private void Awake()
        {
            _iconImage = Find<Image>("Item/Icon");
            _nameText = Find<Text>("Item/Name");
            _transOkBg = Find<Transform>("Item/OkBlack");
            _textLove = Find<Text>("Item/Love/Text");
            _textWood = Find<Text>("Item/ConstItems/Wood/Text");
            transConsItems = new Transform[4];
            for (int i = 0; i < 4; i++)
            {
                transConsItems[i] = Find<Transform>("Item/ConstItems/Item" + (1 + i));
            }
            _textCoin = Find<Text>("Item/Gold/Text");
            _textTime = Find<Text>("Item/Time/Text");
        }

        #endregion
    }
}

