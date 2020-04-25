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
    partial class ShowAwardList
    {
        #region WindowData
        #endregion

        #region Field

        private KMission _TAData;

        #endregion

        #region Method

        public void InitModel()
        {
            _TAData = new KMission();
        }

        public void RefreshModel()
        {
            if (data is KMission)
            {
                _TAData = (KMission)data;
            }
        }
        #endregion
    }
}

