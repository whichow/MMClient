// ***********************************************************************
// Assembly         : Unity
// Author           : LiMuChen
// Created          : 
//
// Last Modified By : LiMuchen
// Last Modified On : 
// ***********************************************************************
// <copyright file= "GuideMovieWindow.Model" company=""></copyright>
// <summary></summary>
// ***********************************************************************

using Game.DataModel;

namespace Game.UI
{
    partial class GuideMovieWindow
    {
        #region WindowData

        public class Data
        {
            public GuideActionXDM action;

        }

        #endregion

        #region Field


        private Data _guideMovie;

        #endregion

        #region Method

        public void InitModel()
        {

            _guideMovie = new Data();
        }

        public void RefreshModel()
        {
            var passData = data as Data;
            if (passData != null)
            {
                _guideMovie.action = passData.action;
            }
            else
            {
                _guideMovie.action = null;

            }
        }

        #endregion
    }
}

