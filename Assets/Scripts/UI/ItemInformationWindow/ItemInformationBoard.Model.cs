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
    partial class ItemInformationBoard
    {
        #region WindowData

        public class ItemInformationData
        {
            public string _title;
            public string _img_icon;
            public int _int_id;
            public string _txt_first;
            public string _txt_fourth;
            public string _txt_fifth;
        }
        #endregion

        #region Field

        private ItemInformationData _messageData;

        #endregion
        
        #region Method

        public void InitModel()
        {
            _messageData = new ItemInformationData();
        }

        public void RefreshModel()
        {
            _messageData._title = string.Empty;
            _messageData._int_id = 0;
            _messageData._img_icon = string.Empty;
            _messageData._txt_first = string.Empty;
            _messageData._txt_fourth = string.Empty;
            _messageData._txt_fifth = string.Empty;

            if (data is ItemInformationData)
            {
                var tmpData = (ItemInformationData)data;
                _messageData._title = tmpData._title;
                _messageData._int_id = tmpData._int_id;
                _messageData._img_icon = tmpData._img_icon;
                _messageData._txt_first = tmpData._txt_first;
                _messageData._txt_fourth = tmpData._txt_fourth;
                _messageData._txt_fifth = tmpData._txt_fifth;
            }
        }
        #endregion
    }
}

