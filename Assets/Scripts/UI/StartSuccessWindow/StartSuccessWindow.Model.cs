//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Msg.ClientMessage;

//namespace Game.UI
//{
//    public partial class StartSuccessWindow
//    {
//        public class WindowData
//        {
//            public int oldCatStart;
//            public int oldCatAddCoin;
//            public int oldCatMaxGrad;
//            public int oldCatExplore;
//            public int oldCatMatch;

//            public S2CCatUpgradeStarResult CatSkillLevelUpdata;

//        }

//        public WindowData windowData
//        {
//            get { return (WindowData)data; }
//        }

//        public int GetOldStart()
//        {
//            var data = windowData;
//            return data.oldCatStart;
//        }
       
//        public int GetNewStart()
//        {
//            var data = windowData;
//            return data.CatSkillLevelUpdata.CatStar;
//        }
//        public string GetNewCatMaxGrad()
//        {
//            var data = windowData;
//            KCat[] allCat = KCatManager.Instance.allCats;
//            KCat retCat = null;
//            for (int i = 0; i < allCat.Length; i++)
//            {
//                if (allCat[i].catId == data.CatSkillLevelUpdata.CatId)
//                {
//                    retCat = allCat[i];
//                }
//            }
//            if (retCat != null)
//            {
//                return retCat.maxGrade.ToString();
//            }
//            return "";
//        }

//        public string GetNewCatAddCoin()
//        {
//            var data = windowData;
//            KCat[] allCat= KCatManager.Instance.allCats;
//            KCat retCat=null;
//            for (int i = 0; i < allCat.Length; i++)
//            {
//                if (allCat[i].catId==data.CatSkillLevelUpdata.CatId)
//                {
//                    retCat = allCat[i];
//                }
//            }
//            if (retCat!=null)
//            {
//                return retCat.coinAbility.ToString();
//            }
//            return "";
//        }

//        public string GetNewCatAddExplore()
//        {
//            var data = windowData;
//            KCat[] allCat = KCatManager.Instance.allCats;
//            KCat retCat = null;
//            for (int i = 0; i < allCat.Length; i++)
//            {
//                if (allCat[i].catId == data.CatSkillLevelUpdata.CatId)
//                {
//                    retCat = allCat[i];
//                }
//            }
//            if (retCat != null)
//            {
//                return retCat.exploreAbility.ToString();
//            }
//            return "";
//        }

//        public string GetNewCatAddMatch()
//        {
//            var data = windowData;
//            KCat[] allCat = KCatManager.Instance.allCats;
//            KCat retCat = null;
//            for (int i = 0; i < allCat.Length; i++)
//            {
//                if (allCat[i].catId == data.CatSkillLevelUpdata.CatId)
//                {
//                    retCat = allCat[i];
//                }
//            }
//            if (retCat != null)
//            {
//                return retCat.matchAbility.ToString();
//            }
//            return "";
//        }
//        public Sprite GetCatIcon()
//        {
//            var data = windowData;
//            KCat[] allCat = KCatManager.Instance.allCats;
//            KCat retCat = null;
//            for (int i = 0; i < allCat.Length; i++)
//            {
//                if (allCat[i].catId == data.CatSkillLevelUpdata.CatId)
//                {
//                    retCat = allCat[i];
//                }
//            }
//            if (retCat != null)
//            {
//                return retCat.GetIconSprite();
//            }
//            return null;
//        }

//    }
//}