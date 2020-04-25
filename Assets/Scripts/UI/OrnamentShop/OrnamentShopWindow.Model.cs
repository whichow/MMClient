// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "FormulaShopWindow.Model" company=""></copyright>
// <summary></summary>
// ***********************************************************************

namespace Game.UI
{
    public partial class OrnamentShopWindow
    {
        public enum PageType
        {
            kAll,
            kTag2 = 2,
            kTag3 = 3,
            kTag4 = 4,
            kTag5 = 5,
            kTag6 = 6,
        }
        public class Data
        {
            public int indx;
            public System.Action<int, string, object> onConfirm;
        }
        private Data _ornamentShopData;
        #region Field

        private PageType _pageType;
        private bool _isChanged;

        #endregion

        #region Porperty

        /// <summary>
        /// 
        /// </summary>
        public PageType pageType
        {
            get { return _pageType; }
            private set
            {
                if (_pageType != value)
                {
                    _pageType = value;
                    _isChanged = true;
                }
            }
        }

        public bool isChanged
        {
            get { return _isChanged; }
        }

        #endregion

        #region Method

        public void InitModel()
        {
            _ornamentShopData = new Data();
        }
        public void RefreshModel()
        {
            _ornamentShopData.indx = 0;
            _ornamentShopData.onConfirm = null;
            if (data is Data)
            {
                var tmpData = (Data)data;
                _ornamentShopData.onConfirm = tmpData.onConfirm;
                _ornamentShopData.indx = tmpData.indx;

            }
        }
        public KItemFormula[] GetFormulas()
        {
            var formulas = System.Array.FindAll(KItemManager.Instance.GetFormulas(), f => f.isHave);
            if (pageType == PageType.kAll)
            {
                System.Array.Sort(formulas, (f1, f2) => f1.sortValue.CompareTo(f2.sortValue));
                return formulas;
            }
            else
            {
                var tag = (int)pageType;
                var tagFormulas = System.Array.FindAll(formulas, f => f.buildingTag == tag);
                System.Array.Sort(tagFormulas, (f1, f2) => f1.sortValue.CompareTo(f2.sortValue));
                return tagFormulas;
            }
        }

        #endregion
    }
}

