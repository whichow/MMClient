// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "MessageBox.View" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Game.UI
{
    partial class DecorateHandBookWindow
    {
        #region Field
        private Button _btn_close;

        private Text _txt_decorateNum;
        private ToggleGroup _tggp_allcat;
        private List<Toggle> _rarityToggles;
        private KUIItemPool _layoutElementPool;

        private Toggle _tg_letter;
        private Text _txt_totalNum;
        #endregion

        #region Method

        public void InitView()
        {
            _btn_close = Find<Button>("close");
            _btn_close.onClick.AddListener(this.CloseUI);

            _tggp_allcat = Find<ToggleGroup>("go_decorate/Tab View/ToggleGroup");
            _rarityToggles = new List<Toggle>(_tggp_allcat.GetComponentsInChildren<Toggle>());
            for (int i = 0; i < _rarityToggles.Count; i++)
            {
                _rarityToggles[i].onValueChanged.AddListener(this.OnPageChange);
            }
            _layoutElementPool = Find<KUIItemPool>("go_decorate/Scroll View");
            if (_layoutElementPool && _layoutElementPool.elementTemplate)
            {
                _layoutElementPool.elementTemplate.gameObject.AddComponent<DecoratetHandBookWindowItem>();
            }

            _tg_letter = Find<Toggle>("go_decorate/Tab View/ToggleGroup/letter");
            _txt_totalNum = Find<Text>("go_decorate/NumBack/Text");
        }

        private string GetOnToggle()
        {
            foreach (var toggle in _rarityToggles)
            {
                if (toggle.isOn)
                {
                    return toggle.name;
                }
            }
            return null;
        }

        public void RefreshView()
        {
            StartCoroutine(FillElements());
        }

        private IEnumerator FillElements()
        {
            _layoutElementPool.Clear();
            var Catdata = GetTorAData();
            for (int i = 0; i < Catdata.Length; i++)
            {
                var element = _layoutElementPool.SpawnElement();
                var catItem = element.GetComponent<DecoratetHandBookWindowItem>();
                catItem.ShowCat(Catdata, i);
            }
            yield return null;
            RefreshTotalNum(Catdata);
        }

        private void RefreshTotalNum(KHandBookManager.HandBookConfiger[] dt)
        {
            int getNum = 0;
            for (int i = 0; i < dt.Length; i++)
            {
                if (dt[i].learned == 1)
                {
                    getNum++;
                }
            }
            _txt_totalNum.text = "已激活：" + getNum.ToString() + "/" + dt.Length.ToString();
        }
        #endregion
    }
}

