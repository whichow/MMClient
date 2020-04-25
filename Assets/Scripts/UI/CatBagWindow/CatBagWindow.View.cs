//// ***********************************************************************
//// Assembly         : Unity
//// Author           : Kimch
//// Created          : 
////
//// Last Modified By : Kimch
//// Last Modified On : 
//// ***********************************************************************
//// <copyright file= "CatBagWindow" company=""></copyright>
//// <summary></summary>
//// ***********************************************************************
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//namespace Game.UI
//{
//    public partial class CatBagWindow
//    {
//        #region Field

//        private Dropdown _sortDropdown;
//        private Button _quitBtn;
//        private Text _catCount;

//        private List<Toggle> _rarityToggles;
//        private GameObject _goTextBlack;
//        private KUIGrid _uiGrid;

//        #endregion

//        #region Method

//        private void InitView()
//        {
//            _sortDropdown = Find<Dropdown>("Panel/Dropdown");
//            _sortDropdown.onValueChanged.AddListener(this.OnSortTypeChange);

//            _quitBtn = Find<Button>("Panel/Close");
//            _quitBtn.onClick.AddListener(this.OnQuitBtnClick);
//            _goTextBlack = Find("Panel/Hint");
//            _catCount = Find<Text>("Panel/CatCount/Text");

//            var toggleGroup = Find<ToggleGroup>("Panel/Tab View/ToggleGroup");
//            _rarityToggles = new List<Toggle>(toggleGroup.GetComponentsInChildren<Toggle>());
//            for (int i = 0; i < _rarityToggles.Count; i++)
//            {
//                _rarityToggles[i].onValueChanged.AddListener(this.OnPageChange);
//            }

//            _uiGrid = Find<KUIGrid>("Panel/Scroll View");
//            if (_uiGrid)
//            {
//                _uiGrid.uiPool.itemTemplate.AddComponent<CatBagItem>();
//            }

//            RefreshView();
//        }

//        private void RefreshView()
//        {
//            _sortDropdown.value = (int)sortType;
//            _catCount.text = GetCountText();

//            StartCoroutine(FillElements());
//        }

//        private IEnumerator FillElements()
//        {
//            yield return null;
//            var cats = GetCatInfos();
//            if (cats.Length > 0)
//            {
//                _goTextBlack.SetActive(false);
//            }
//            else
//            {
//                _goTextBlack.SetActive(true);
//            }
//            var datas = new ArrayList(cats.Length);
//            for (int i = 0; i < cats.Length; i++)
//            {
//                //datas.Add(new CatInfoWindow.WindowData
//                //{
//                //    catIndex = i,
//                //    catArray = cats,
//                //});
//            }
//            _uiGrid.uiPool.SetItemDatas(datas);
//            _uiGrid.RefillItems(0);
//            yield return null;
//        }

//        private string GetOnToggle()
//        {
//            foreach (var toggle in _rarityToggles)
//            {
//                if (toggle.isOn)
//                {
//                    return toggle.name;
//                }
//            }
//            return null;
//        }

//        #endregion
//    }
//}

