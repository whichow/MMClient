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

using System.Collections.Generic;

namespace Game.UI
{
    partial class SingleSuiteHandBookWindow
    {
        #region WindowData

        private KItemSuit _suitData;
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
            _suitData = new KItemSuit();
        }

        public void RefreshModel()
        {
            if (data is KItemSuit)
            {
                _suitData = (KItemSuit)data;
            }
        }
        public KItemBuilding[] GetSuitDatas()
        {
            List<KItemBuilding> currentList = new List<KItemBuilding>();
            KItemBuilding[] allbuilkd = KItemManager.Instance.GetBuildings();
            for (int i = 0; i < allbuilkd.Length; i++)
            {
                if (_suitData.itemID == allbuilkd[i].suitID && allbuilkd[i].suitID != 0)
                {
                    currentList.Add(allbuilkd[i]);
                }
            }
            return currentList.ToArray();
        }
        #endregion
    }
}

