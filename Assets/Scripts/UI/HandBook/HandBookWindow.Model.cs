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
    partial class HandBookWindow
    {
        #region WindowData
        #endregion

        #region Field

        #endregion
        
        #region Method

        public void InitModel()
        {
        }

        public void RefreshModel()
        {
            KHandBookManager.Instance.GetHandBooks(GetServerData);
        }
        #endregion
    }
}

