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
    partial class LandeHandBookWindow
    {
        #region WindowData

        public enum PageType
        {
            Kmaozhen,
            Kyangguangdao,
            Kyouleyuan,
            Kxinyangdao,
            Kkongzhongfeidao,
            Kmeishidao,
            Kliulangmaojidi,
        }

        private PageType _pageType;

        public PageType pageType
        {
            get { return _pageType; }
            private set
            {
                if (_pageType != value)
                {
                    _pageType = value;
                }
            }
        }
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
            foreach (var item in _rarityToggles)
            {
                item.isOn = false;
            }
        }
        #endregion
    }
}

