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
using UnityEngine.UI;

namespace Game.UI
{
    partial class MessageBox
    {
        #region Field

        private Text _title;
        private Text _content;
        private Button _confirm;
        private Button _cancel;

        #endregion

        #region Method

        public void InitView()
        {
            _title = Find<Text>("Panel/Title");
            _content = Find<Text>("Panel/Content");

            _confirm = Find<Button>("Panel/ButtonGroup/Confirm");
            _confirm.onClick.AddListener(this.OnConfirmClick);
            _cancel = Find<Button>("Panel/ButtonGroup/Cancel");
            _cancel.onClick.AddListener(this.OnCancelClick);
        }

        public void RefreshView()
        {
            _title.text = _messageData.title;
            _content.text = _messageData.content;
            _cancel.gameObject.SetActive(_messageData.onCancel != null);
        }

        #endregion
    }
}

