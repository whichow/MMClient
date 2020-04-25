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
using UnityEngine;
using UnityEngine.UI;


namespace Game.UI
{
    partial class HandBookWindow
    {

        #region Field
        private Button _btn_close;
        private Button _btn_cat;
        private Button _btn_decorate;
        private Button _btn_suite;
        private Button _btn_lande;
        #endregion

        #region Method

        public void InitView()
        {
            _btn_close = Find<Button>("go_total/Close");
            _btn_close.onClick.AddListener(this.CloseUI);
            _btn_cat = Find<Button>("go_total/Dex/Viewport/Content/btn_cat");
            _btn_cat.onClick.AddListener(this.OpenCatBook);
            _btn_decorate = Find<Button>("go_total/Dex/Viewport/Content/btn_decorate");
            _btn_decorate.onClick.AddListener(this.OpenDecorateBook);
            _btn_suite = Find<Button>("go_total/Dex/Viewport/Content/btn_suite");
            _btn_suite.onClick.AddListener(this.OpenSuiteBook);
            _btn_lande = Find<Button>("go_total/Dex/Viewport/Content/btn_lande");
            _btn_lande.onClick.AddListener(this.OpenLandeBook);
            ShowOrHideBtns(false);
        }

        public void RefreshView()
        {
        }

        private void GetServerData(int code,string str,object obj) {
            ShowOrHideBtns(true);
        }
        private void ShowOrHideBtns(bool bl) {
            _btn_cat.gameObject.SetActive(bl);
            _btn_decorate.gameObject.SetActive(bl);
            _btn_suite.gameObject.SetActive(bl);
            _btn_lande.gameObject.SetActive(bl);
        }

        #endregion
    }
}

