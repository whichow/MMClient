// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "InfomationBox.Model" company=""></copyright>
// <summary></summary>
// ***********************************************************************

namespace Game.UI
{
    partial class InfomationBox
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

        private Data _infomationData;

        #endregion

        #region Method

        public void InitModel()
        {
            _infomationData = new Data();
        }

        public void RefreshModel()
        {
            var passData = data as Data;
            if (passData != null)
            {
                _infomationData.title = passData.title;
                _infomationData.content = passData.content;
                _infomationData.onConfirm = passData.onConfirm;
                _infomationData.onCancel = passData.onCancel;
            }
            else
            {
                _infomationData.title = string.Empty;
                _infomationData.content = string.Empty;
                _infomationData.onConfirm = null;
                _infomationData.onCancel = null;
            }
        }

        #endregion
    }
}
