// ***********************************************************************
// Assembly         : Unity
// Author           : LiMuChen
// Created          : 
//
// Last Modified By : LiMuchen
// Last Modified On : 
// ***********************************************************************
// <copyright file= "NoviceTalkWindow.Model" company=""></copyright>
// <summary></summary>
// ***********************************************************************

using Game.DataModel;

namespace Game.UI
{
    partial class NoviceTalkWindow
    {
        #region WindowData

        public class Data
        {
            public GuideActionXDM action;

        }

        #endregion

        #region Field


        private Data _noviceTalkData;

        #endregion

        #region Method

        public void InitModel()
        {
            talkIndx = 0;
            _noviceTalkData = new Data();
        }

        public void RefreshModel()
        {
            var passData = data as Data;
            if (passData != null)
            {
                _noviceTalkData.action = passData.action;
            }
            else
            {
                _noviceTalkData.action = null;
            }
        }

        #endregion
    }
}

