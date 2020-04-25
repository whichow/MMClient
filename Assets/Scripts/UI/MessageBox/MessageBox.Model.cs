// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "MessageBox.Model" company=""></copyright>
// <summary></summary>
// ***********************************************************************

namespace Game.UI
{
    partial class MessageBox
    {
        #region WindowData

        public class Data
        {
            public string title;
            public string content;
            public System.Action onConfirm;
            public System.Action onCancel;
        }

        #endregion

        #region Field

        public readonly static Data DefaultData = new Data();

        private Data _messageData;

        #endregion

        #region Method

        public void InitModel()
        {
            _messageData = new Data();
        }

        public void RefreshModel()
        {     
            var passData = data as Data;
            if (passData != null)
            {
                _messageData.title = passData.title;
                _messageData.content = passData.content;
                _messageData.onConfirm = passData.onConfirm;
                _messageData.onCancel = passData.onCancel;
            }
            else
            {
                _messageData.title = string.Empty;
                _messageData.content = string.Empty;
                _messageData.onConfirm = null;
                _messageData.onCancel = null;
            }
        }

        #endregion
    }
}

