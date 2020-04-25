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
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Game.UI
{
    partial class LandeHandBookWindow
    {
        #region Field
        private Button _btn_close;

        private ToggleGroup _tglGp_TA;
        private List<Toggle> _rarityToggles;
        private Toggle _btn_maozhen;
        private Toggle _btn_yangguangdao;
        private Toggle _btn_youleyuan;
        private Toggle _btn_xinyangdao;
        private Toggle _btn_kongzhongfeidao;
        private Toggle _btn_meishidao;
        private Toggle _btn_liulangmaojidi;
        #endregion

        #region Method

        public void InitView()
        {
            _btn_close = Find<Button>("close");
            _btn_close.onClick.AddListener(this.CloseUI);

            _btn_maozhen = Find<Toggle>("go_lande/Image/btn_8018");
            _btn_yangguangdao = Find<Toggle>("go_lande/Image/btn_8018");
            _btn_youleyuan = Find<Toggle>("go_lande/Image/btn_8018");
            _btn_xinyangdao = Find<Toggle>("go_lande/Image/btn_8018");
            _btn_kongzhongfeidao = Find<Toggle>("go_lande/Image/btn_8018");
            _btn_meishidao = Find<Toggle>("go_lande/Image/btn_8018");
            _btn_liulangmaojidi = Find<Toggle>("go_lande/Image/btn_8018");
            _tglGp_TA = Find<ToggleGroup>("go_lande/Image");
            _rarityToggles = new List<Toggle>(_tglGp_TA.GetComponentsInChildren<Toggle>());
            for (int i = 0; i < _rarityToggles.Count; i++)
            {
                _rarityToggles[i].onValueChanged.AddListener(this.OnPageChange);
            }
        }

        public void RefreshView()
        {


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
        #endregion
    }
}

