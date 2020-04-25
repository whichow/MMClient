//// ***********************************************************************
//// Assembly         : Unity
//// Author           : LiMuChen
//// Created          : #DATA#
////
//// Last Modified By : LiMuChen
//// Last Modified On : 
//// ***********************************************************************
//// <copyright file= "AdoptionCatWindow.Model" company="moyu"></copyright>
//// <summary></summary>
//// ***********************************************************************

//namespace Game.UI
//{
//    partial class AdoptionFriendWindow
//    {

//        #region WindowData
//        public class Data
//        {
//            public int playerId;
//        }


//        #endregion

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
//        private Data _windowdata;
//        private bool _isChanged;

//        private KCat[] _cacheCats;
  
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
//        public void InitModel()
//        {
//            pageType = PageType.kAll;
//            sortType = SortType.kStar;
//            _windowdata = new Data();
//        }

//        public void RefreshModel()
//        {
//            var passData = data as Data;
//            if (passData != null)
//            {
//                _windowdata.playerId = passData.playerId;
//            }
//            _isChanged = true;
//            _cacheCats = null;
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
//                    break;
//                default:
//                    break;
//            }

//            return _cacheCats;
//        }

//        #endregion
//    }
//}

