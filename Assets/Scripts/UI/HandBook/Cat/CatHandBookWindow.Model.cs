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
    partial class CatHandBookWindow
    {
        #region WindowData
        public enum PageType
        {
           allCat,
           NCat,
           RCat,
           SRCat,
           SSRCat,
        }

        public class ItemInformationData
        {
            public string _title;
        }
        #endregion

        #region Field
        private ItemInformationData _messageData;
        public PageType pageType
        {            
            get;
            set;
        }
        #endregion

        #region Method

        public void InitModel()
        {
            _messageData = new ItemInformationData();
        }

        public void RefreshModel()
        {
            pageType = PageType.allCat;
            _tgl_AllCat.isOn = true;
        }

        public KHandBookManager.HandBookConfiger[] GetTorAData()
        {
            KHandBookManager.HandBookConfiger[] currentList;
            switch (pageType)
            {
                case PageType.allCat:
                    currentList = KHandBookManager.Instance.GetCatDatas(PageType.allCat);
                    break;
                case PageType.NCat:
                    currentList = KHandBookManager.Instance.GetCatDatas(PageType.NCat);
                    break;
                case PageType.RCat:
                    currentList = KHandBookManager.Instance.GetCatDatas(PageType.RCat);
                    break;
                case PageType.SRCat:
                    currentList = KHandBookManager.Instance.GetCatDatas(PageType.SRCat);
                    break;
                case PageType.SSRCat:
                    currentList = KHandBookManager.Instance.GetCatDatas(PageType.SSRCat);
                    break;
                default:
                    currentList = null;
                    break;
            }
            return currentList;
        }
        #endregion
    }
}

