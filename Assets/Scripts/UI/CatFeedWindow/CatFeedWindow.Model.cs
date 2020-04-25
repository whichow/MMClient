//// ***********************************************************************
//// Assembly         : Unity
//// Author           : Kimch
//// Created          : 
////
//// Last Modified By : Kimch
//// Last Modified On : 
//// ***********************************************************************
//// <copyright file= "CatFeedWindow" company=""></copyright>
//// <summary></summary>
//// ***********************************************************************
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Game.UI
//{
//    public partial class CatFeedWindow
//    {
//        #region Field

//        private GameObject _catModel;

//        #endregion

//        #region Method

//        public void InitModel()
//        {
//        }

//        public void RefreshModel()
//        {
//            if (_catModel)
//            {
//                Object.Destroy(_catModel);
//            }
//            _catModel = null;
//        }

//        public KCat GetCat()
//        {
//            return data as KCat;
//        }

//        public string GetNameText()
//        {
//            var cat = GetCat();

//            if (cat != null)
//            {
//                if (string.IsNullOrEmpty(cat.nickName))
//                {
//                    return cat.name;
//                }
//                else
//                {
//                    return cat.nickName;
//                }
//            }
//            return "";
//        }

//        public string GetGradeText()
//        {
//            var cat = GetCat();
//            if (cat != null)
//            {
//                return cat.grade.ToString();
//            }
//            return "0";
//        }

//        public string GetExpRequired()
//        {
//            int rExp = 0;
//            var cat = GetCat();
//            if (cat != null)
//            {
//                rExp = cat.maxExp - cat.exp;
//                rExp = Mathf.Min(cat.feedCost, rExp);
//            }
//            return "投食消耗:" + rExp;
//        }
//      public int GetNeedFood()
//        {
//            int rExp = 0;
//            var cat = GetCat();
//            if (cat != null)
//            {
//                rExp = cat.maxExp - cat.exp;
//                rExp = Mathf.Min(cat.feedCost, rExp);
//            }
//            return rExp;
//        }

//        public string GetExp()
//        {
//            return "当前拥有:" + PlayerDataModel.Instance.mPlayerData.mCatFood;
//        }

//        public float GetExpProgress()
//        {
//            var cat = GetCat();
//            if (cat != null)
//            {
//                return (float)cat.exp / cat.maxExp;
//            }
//            return 0.0f;
//        }

//        public GameObject GetCatModel()
//        {
//            if (_catModel)
//            {
//                return _catModel;
//            }

//            var cat = GetCat();
//            if (cat != null)
//            {
//                _catModel = cat.GetUIModel();
//            }
//            return _catModel;
//        }

//        #endregion
//    }
//}

