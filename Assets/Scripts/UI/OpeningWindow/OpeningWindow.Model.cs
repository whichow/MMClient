// ***********************************************************************
// Assembly         : Unity
// Author           : LiMuChen
// Created          : 
//
// Last Modified By : LiMuchen
// Last Modified On : 
// ***********************************************************************
// <copyright file= "OpeningWindow.Model" company=""></copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;

namespace Game.UI
{
    partial class OpeningWindow
    {
        #region WindowData
        public class Data
        {
            public List<int>  describeId;
        }

        #endregion

        #region Field
        private Data _windowData;

        #endregion

        #region Method

        public void InitModel()
        {
            _windowData = new Data();
        }

        public void RefreshModel()
        {
            var passData = data as Data;
            if (passData!=null)
            {
                _windowData.describeId = passData.describeId;
            }
        }

        #endregion
    }
}

