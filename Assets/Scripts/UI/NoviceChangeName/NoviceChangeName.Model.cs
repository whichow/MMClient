// ***********************************************************************
// Assembly         : Unity
// Author           : LiMuChen
// Created          : 
//
// Last Modified By : LiMuchen
// Last Modified On : 
// ***********************************************************************
// <copyright file= "NoviceChangeName.Model" company=""></copyright>
// <summary></summary>
// ***********************************************************************

using Game.DataModel;

namespace Game.UI
{
    partial class NoviceChangeName
    {
        #region WindowData

        public class Data
        {
            public GuideActionXDM action;
          
        }

        #endregion

        #region Field
        private Data _changeNameData;

        #endregion

        #region Method

        public void InitModel()
        {

            _changeNameData = new Data();
        }

        public void RefreshModel()
        {
            var passData = data as Data;
            if (passData != null)
            {
                _changeNameData.action = passData.action;
            }
            else
            {
                _changeNameData.action = null;
            }
        }
        #endregion
    }
}

