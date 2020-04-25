// ***********************************************************************
// Assembly         : Unity
// Author           : LiMuChen
// Created          : 
//
// Last Modified By : LiMuchen
// Last Modified On : 
// ***********************************************************************
// <copyright file= "NoviceMaskWindow.Model" company=""></copyright>
// <summary></summary>
// ***********************************************************************

using Game.DataModel;

namespace Game.UI
{
    partial class NoviceMaskWindow
    {
        #region WindowData

        public class Data
        {
            public GuideActionXDM action;
        }

        #endregion

        #region Field



        private Data _noviceData;

        #endregion

        #region Method

        public void InitModel()
        {
            _noviceData = new Data();
        }

        public void RefreshModel()
        {
            var passData = data as Data;
            if (passData != null)
            {
                _noviceData.action = passData.action;
            }
            else
            {
                _noviceData.action = null;
            }
        }

        #endregion
    }
}

