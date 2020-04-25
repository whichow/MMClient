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
    public partial class FormulaShopWindow
    {
        #region Field  

        private Toggle[] _typeToggles;

        private KUIGrid _itemPool;

        private Button _closeButton;

        #endregion

        #region Method

        public void InitView()
        {
            _closeButton = Find<Button>("Plane/Close");
            _closeButton.onClick.AddListener(this.OnCloseBtnClick);

            var toggleGroup = Find<ToggleGroup>("Plane/Tab View/ToggleGroup");
            _typeToggles = toggleGroup.GetComponentsInChildren<Toggle>();
            for (int i = 0; i < _typeToggles.Length; i++)
            {
                _typeToggles[i].onValueChanged.AddListener(this.OnPageChange);
            }

            _itemPool = Find<KUIGrid>("Plane/Scroll View");
            if (_itemPool)
            {
                _itemPool.uiPool.itemTemplate.AddComponent<FormulaShopItem>();
            }
        }

        public void RefreshItems()
        {
            _itemPool.RefreshItems();
        }

        public void RefreshView()
        {
            _itemPool.horizontal = !GuideManager.Instance.IsGuideing;

            var formulas = GetFormulas();
            _itemPool.uiPool.SetItemDatas(formulas);
            _itemPool.RefillItems();
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

