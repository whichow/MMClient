//// ***********************************************************************
//// Assembly         : Unity
//// Author           : Kimch
//// Created          : 
////
//// Last Modified By : Kimch
//// Last Modified On : 
//// ***********************************************************************
//// <copyright file= "CatLearnWindow" company=""></copyright>
//// <summary></summary>
//// ***********************************************************************
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Game.UI
//{
//    partial class CatLearnWindow
//    {
//        #region Method
//        private List<KCat> cats;
//        private KCat[] allCats;
//        private int oldCatSkillGrade;
//        public int oldCatStart;
//        public int oldCatAddCoin;
//        public int oldCatMaxGrad;
//        public int oldCatExplore;
//        public int oldCatMatch;
//        public void InitModel()
//        {

//        }

//        public List<KCat> GetCatStartInfos()
//        {
//            List<KCat> retCatList = new List<KCat>();
//            var cat = data as KCat;
//            var cats = KCatManager.Instance.allCats;
//            for (int i = 0; i < cats.Length; i++)
//            {
//                if (cat.star==cats[i].star&& cat.catId != cats[i].catId)
//                {
//                    retCatList.Add(cats[i]);
//                }
//            }
//            retCatList.Sort(KCat.GetStarComparison);
//            retCatList.Reverse();
//            return retCatList;
//        }

//        public List<KCat> GetCatSkillInfos()
//        {
//            List<KCat> retCatList = new List<KCat>();
//            var cat = data as KCat;
//            var cats = KCatManager.Instance.allCats;
//            for (int i = 0; i < cats.Length; i++)
//            {
//                if (cat.shopId == cats[i].shopId&&cat.catId!=cats[i].catId)
//                {
//                    retCatList.Add(cats[i]);
//                }
//            }
//            retCatList.Sort(KCat.GetSkillLvlOrderComparison);
//            retCatList.Reverse();
//            return retCatList;
//        }

//        #endregion
//    }
//}

