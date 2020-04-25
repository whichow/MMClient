// ***********************************************************************
// Assembly         : Unity
// Author           : LiMuChen
// Created          : 
//
// Last Modified By : LiMuchen
// Last Modified On : 
// ***********************************************************************
// <copyright file= "GiveNameWindow.Model" company=""></copyright>
// <summary></summary>
// ***********************************************************************

using Game.DataModel;

namespace Game.UI
{
    partial class GiveNameWindow
    {
        #region WindowData

        public class Data
        {
            public GuideActionXDM action;
        }

        #endregion

        #region Field
        private Data _giveNameData;

        #endregion

        #region Method

        public void InitModel()
        {
            
            _giveNameData = new Data();
        }

        public void RefreshModel()
        {
            var passData = data as Data;
            if (passData != null)
            {
                _giveNameData.action = passData.action;
            }
            else
            {
                _giveNameData.action = null;
            }
        }
        #endregion
    }
}

