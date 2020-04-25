// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "FormulaShopWindow.View" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public partial class OrnamentShopWindow
    {
        #region Field  

        private Toggle[] _typeToggles;
        private KUIItemPool _itemPool;
        private ScrollRect _itemScroll;

        private Button _closeButton;
        private Button _btnOk;
        private OrnamentShopItem Kitem;
        private KItemFormula shopItem;
        #endregion

        #region Method

        public void InitView()
        {
            _closeButton = Find<Button>("Plane/Quit");
            _closeButton.onClick.AddListener(this.OnCloseBtnClick);

            var toggleGroup = Find<ToggleGroup>("Plane/Tab View/ToggleGroup");
            _typeToggles = toggleGroup.GetComponentsInChildren<Toggle>();
            for (int i = 0; i < _typeToggles.Length; i++)
            {
                _typeToggles[i].onValueChanged.AddListener(this.OnPageChange);
            }

            _itemScroll = Find<ScrollRect>("Plane/Scroll View");
            _itemPool = Find<KUIItemPool>("Plane/Scroll View");
            if (_itemPool && _itemPool.elementTemplate)
            {
                _itemPool.elementTemplate.gameObject.AddComponent<OrnamentShopItem>();
            }

            _btnOk = Find<Button>("Plane/Button");
            _btnOk.onClick.AddListener(OnOkBtnClick);
        }

        public void RefreshItems()
        {
            _itemPool.RefreshItems();
        }
        public void SetItem(KItemFormula shopItem, OrnamentShopItem item)
        {
            if (Kitem!=null)
            {
                Kitem.ShowBgBlack(false);
            }
            Kitem = item;
            item.ShowBgBlack(true);
            this.shopItem = shopItem;
        }

        public void RefreshView()
        {
            _itemScroll.vertical = !GuideManager.Instance.IsGuideing;

            _itemPool.Clear();

            var formulas = GetFormulas();
            if (formulas != null && formulas.Length > 0)
            {
                var items = _itemPool.SpawnElements(formulas.Length);
                for (int i = 0; i < formulas.Length; i++)
                {
                    items[i].GetComponent<OrnamentShopItem>().ShowFormula(formulas[i]);
                }
            }
        }

        private string GetOnToggle()
        {
            foreach (var toggle in _typeToggles)
            {
                if (toggle.isOn)
                {
                    return toggle.name;
                }
            }
            return null;
        }

        #endregion
    }
}

