// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "InfomationBox.View" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using UnityEngine.UI;

namespace Game.UI
{
    partial class InfomationBox
    {
        #region Field

        private Button _okButton;
        private Button _closeButton;
        private Button _maskButton;

        private Text _titleText;
        private Text _contentText;

        #endregion

        #region Method

        public void InitView()
        {
            _okButton = Find<Button>("Panel/Ok");
            _okButton.onClick.AddListener(this.OnOkClick);

            _closeButton = Find<Button>("Panel/Close");
            _closeButton.onClick.AddListener(this.OnOkClick);

            _maskButton = Find<Button>("BlackMask");
            _maskButton.onClick.AddListener(this.OnOkClick);

            _titleText = Find<Text>("Panel/Title");
            _contentText = Find<Text>("Panel/Content");
        }

        public void RefreshView()
        {
            _titleText.text = _infomationData.title;
            _contentText.text = _infomationData.content;
        }

        #endregion
    }
}
