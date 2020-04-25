//// ***********************************************************************
//// Assembly         : Unity
//// Author           : Kimch
//// Created          : 
////
//// Last Modified By : Kimch
//// Last Modified On : 
//// ***********************************************************************
//// <copyright file= "CatBagWindow" company=""></copyright>
//// <summary></summary>
//// ***********************************************************************

//namespace Game.UI
//{
//    public partial class CatBagWindowa
//    {
//        #region Enum

//        public enum PageType
//        {
//            kAll,
//            kN,
//            kR,
//            kSR,
//            kSSR,
//        }

//        public enum SortType
//        {
//            kStar,
//            kGrade,
//            kColor,
//            kRarity,
//        }

//        #endregion

//        #region Field

//        private PageType _pageType;
//        private SortType _sortType;

//        private bool _isChanged;

//        private KCat[] _cacheCats;

//        #endregion

//        #region Porperty

//        /// <summary>
//        /// 
//        /// </summary>
//        public PageType pageType
//        {
//            get { return _pageType; }
//            private set
//            {
//                if (_pageType != value)
//                {
//                    _pageType = value;
//                    _isChanged = true;
//                    _cacheCats = null;
//                }
//            }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public SortType sortType
//        {
//            get { return _sortType; }
//            private set
//            {
//                if (_sortType != value)
//                {
//                    _sortType = value;
//                    _isChanged = true;
//                }
//            }
//        }

//        public bool isChanged
//        {
//            get { return _isChanged; }
//        }

//        #endregion

//        #region Method

//        private void InitModel()
//        {
//            pageType = PageType.kAll;
//            sortType = SortType.kStar;
//        }

//        public void RefreshModel()
//        {
//            _isChanged = true;
//            _cacheCats = null;
//        }

//        public string GetCountText()
//        {
//            return "我的喵咪:" + GetCatInfos().Length;
//        }

//        public KCat[] GetCatInfos()
//        {
//            var cats = KCatManager.Instance.allCats;

//            if (_cacheCats == null)
//            {
//                switch (pageType)
//                {
//                    case PageType.kN:
//                        _cacheCats = System.Array.FindAll(cats, cat => cat.rarity == 1);
//                        break;
//                    case PageType.kR:
//                        _cacheCats = System.Array.FindAll(cats, cat => cat.rarity == 2);
//                        break;
//                    case PageType.kSR:
//                        _cacheCats = System.Array.FindAll(cats, cat => cat.rarity == 3);
//                        break;
//                    case PageType.kSSR:
//                        _cacheCats = System.Array.FindAll(cats, cat => cat.rarity == 4);
//                        break;
//                    default:
//                        _cacheCats = cats;
//                        break;
//                }
//            }

//            switch (sortType)
//            {
//                case SortType.kStar:
//                    System.Array.Sort(_cacheCats, KCat.GetStarComparison);
//                    break;
//                case SortType.kGrade:
//                    System.Array.Sort(_cacheCats, KCat.GetGradeComparison);
//                    break;
//                case SortType.kRarity:
//                    System.Array.Sort(_cacheCats, KCat.GetRarityComparison);
//                    break;
//                case SortType.kColor:
//                    System.Array.Sort(_cacheCats, KCat.GetColorComparison);
//                    System.Array.Reverse(_cacheCats);
//                    break;
//                default:
//                    break;
//            }

//            return _cacheCats;
//        }

//        #endregion
//    }
//}

