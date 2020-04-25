//// ***********************************************************************
//// Assembly         : Unity
//// Author           : LiMuChen
//// Created          : #DATA#
////
//// Last Modified By : LiMuChen
//// Last Modified On : 
//// ***********************************************************************
//// <copyright file= "AdoptionCatWindow" company="moyu"></copyright>
//// <summary></summary>
//// ***********************************************************************

//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;

//namespace Game.UI
//{
//    public partial class AdoptionFriendWindow : KUIWindow
//    {
    

//        #region Constructor

//        public AdoptionFriendWindow()
//                    : base(UILayer.kNormal, UIMode.kSequenceRemove)
//        {
//            uiPath = "AdoptionFriends";
//        }

//        #endregion

//        #region Action


//        private void OnCancelClick()
//        {
//            KUIWindow.GetWindow<AdoptionWindow>().RefreshView();
//            //KUIWindow.GetWindow<AdoptionWindow>().ShowModel(true);
//            CloseWindow(this);
//        }
//        private void OnSortTypeChange(int value)
//        {
//            sortType = (SortType)value;
//            if (isChanged)
//            {
//                RefreshView();
//            }
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
//        private void OnOpenFriendClick()
//        {
           
        
//                    //OpenWindow<FriendListWindow>();
        
//        }
      
//        #endregion

//        #region Unity  

//        // Use this for initialization
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

