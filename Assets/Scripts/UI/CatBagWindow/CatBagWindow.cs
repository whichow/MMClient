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
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Game.UI
//{
//    public partial class CatBagWindow : KUIWindow
//    {
//        #region Constructor

//        public CatBagWindow() :
//            base(UILayer.kNormal, UIMode.kSequenceRemove)
//        {
//            uiPath = "CatBag";
//            uiAnim = UIAnim.kAnim1;
//            hasMask = true;
//        }

//        #endregion

//        #region Action

//        private void OnSortTypeChange(int value)
//        {
//            sortType = (SortType)value;
//            if (isChanged)
//            {
//                RefreshView();
//            }
//        }

//        private void OnQuitBtnClick()
//        {
//            CloseWindow(this);
//        }

//        private void OnPageChange(bool value)
//        {
//            var page = GetOnToggle();

//            if (page == "N")
//            {
//                pageType = PageType.kN;
//            }
//            else if (page == "R")
//            {
//                pageType = PageType.kR;
//            }
//            else if (page == "SR")
//            {
//                pageType = PageType.kSR;
//            }
//            else if (page == "SSR")
//            {
//                pageType = PageType.kSSR;
//            }
//            else
//            {
//                pageType = PageType.kAll;
//            }

//            if (isChanged)
//            {
//                RefreshView();
//            }
//        }

//        #endregion

//        #region Unity  


//        public override void Awake()
//        {
//            InitModel();
//            InitView();
//        }

//        public override void OnEnable()
//        {
//            RefreshModel();
//            RefreshView();
//        }

//        #endregion
//    }
//}

