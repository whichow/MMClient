// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 2017-11-02
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "LackHintBox" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public partial class LackHintBox
    {
        #region Field

        private Button _gotoBtn;
        private Text _content;
        private Button _closeBtn;

        #endregion

        #region Method

        public void InitView()
        {
            _gotoBtn = Find<Button>("Panel/Confirm");
            _gotoBtn.onClick.AddListener(OnGotoClick);
            _content = Find<Text>("Panel/Content");
            _closeBtn = Find<Button>("Panel/Close");
            _closeBtn.onClick.AddListener(OnCloseBtnClick);
        }

        public void RefreshView()
        {
            if (data is int)
            {
                var id = (int)data;
                var item = KItemManager.Instance.GetItem(id);
                _content.text = string.Format("{0}不够啦!是否前往获取?", item.itemName);
            }
        }

        #endregion
    }
}
