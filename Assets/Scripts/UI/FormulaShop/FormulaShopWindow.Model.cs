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
    public partial class FormulaShopWindow
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

        public KItemFormula[] GetFormulas()
        {
            var formulas = KItemManager.Instance.GetFormulas();
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

