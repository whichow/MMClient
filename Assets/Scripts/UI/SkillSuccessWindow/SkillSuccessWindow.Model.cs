//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Msg.ClientMessage;

//namespace Game.UI
//{
//    public partial class SkillSuccessWindow
//    {
//        public class WindowData
//        {
//            public int oldCatLevl;

//            public S2CCatSkillLevelUpResult CatSkillLevelUpdata;

//        }

//        public WindowData windowData
//        {
//            get { return (WindowData)data; }
//        }

//        public string GetOldSKillLvl()
//        {
//            var data = windowData;
//            return data.oldCatLevl.ToString();
//        }
//        public string GetNewSkillLvl()
//        {
//            var data = windowData;
//            return data.CatSkillLevelUpdata.SkillLevel.ToString();
//        }
//        public string GetSkillName()
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
//                return retCat.GetSkillName();
//            }
//            return "";
//        }
//        public Sprite GetSkillIcon()
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
//                return retCat.GetSkillSprite();
//            }
//            return null;
//        }

//    }
//}