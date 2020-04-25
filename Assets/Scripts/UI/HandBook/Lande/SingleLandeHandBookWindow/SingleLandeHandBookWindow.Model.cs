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
    partial class SingleLandeHandBookWindow
    {
        #region WindowData

        private KItemSuit _data_suit;
        //private KHandBookManager.HandBookConfiger[] _hbDataArry;

        #endregion

        #region Field


        #endregion

        #region Method

        public void InitModel()
        {

        }

        public void RefreshModel()
        {
            if (data is LandeHandBookWindow.PageType)
            {
                var pt = (LandeHandBookWindow.PageType)data;
                _data_suit = KItemManager.Instance.GetSuit(GetSuitID(pt));
            }
        }

        private int GetSuitID(LandeHandBookWindow.PageType paTy) {
            switch (paTy)
            {
                case LandeHandBookWindow.PageType.Kmaozhen:
                    return KHandBookManager._int_LandeID_maozhen;
                case LandeHandBookWindow.PageType.Kkongzhongfeidao:
                    return KHandBookManager._int_LandID_kongzhongfeidao;
                case LandeHandBookWindow.PageType.Kyangguangdao:
                    return KHandBookManager._int_LandeID_yangguangdao;
                case LandeHandBookWindow.PageType.Kyouleyuan:
                    return KHandBookManager._int_LandeID_youleyuan;
                case LandeHandBookWindow.PageType.Kxinyangdao:
                    return KHandBookManager._int_LandeID_xinyangdao;
                case LandeHandBookWindow.PageType.Kliulangmaojidi:
                    return KHandBookManager._int_LandeID_liulangmaojidi;
                case LandeHandBookWindow.PageType.Kmeishidao:
                    return KHandBookManager._int_LandeID_meishidao;
                default:
                    return 0;
            }
        }
        private KHandBookManager.HandBookConfiger[] GetDatas() {
            KHandBookManager.HandBookConfiger[] currentLandBuildings = new KHandBookManager.HandBookConfiger[] { };
            currentLandBuildings = KHandBookManager.Instance.GetLandeDatas(_data_suit.itemID);
            return currentLandBuildings;
        }
        #endregion
    }
}

