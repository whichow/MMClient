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
    partial class SuiteHandBookWindow
    {
        #region WindowData

        public class ItemInformationData
        {
            public string _title;
        }
        #endregion

        #region Field
        #endregion
        
        #region Method

        public void InitModel()
        {
        }

        public void RefreshModel()
        {
        }
        public KHandBookManager.HandBookConfiger[] GetTorAData()
        {
            KHandBookManager.HandBookConfiger[] currentList = new KHandBookManager.HandBookConfiger[] { };
            currentList = KHandBookManager.Instance.GetSuiteDatas();
            return currentList;
        }
        #endregion
    }
}

