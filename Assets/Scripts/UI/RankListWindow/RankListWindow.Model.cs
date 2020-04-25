///** 
// *作用：排行榜的界面数据类，主要负责存放当前页签，负责初始化页签等
// *Author:       LiMuChen  
//*/
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Game.UI
//{
//    partial class RankListWindow
//    {
//        public enum RankType
//        {
//            TotalScore = 1,
//            kCustomsPass = 2,
//            kCharm = 3,
//            kOuQi = 4,
//            kZan = 5,
//        }
//        #region Field
//        private RankType _rankType;
//        public RankType CurrentRankType
//        {
//            get
//            {
//                return _rankType;
//            }
//        }
//        #endregion
//        #region Method
//        public void InitModel()
//        {
//        }
//        private void ResetModel()
//        {
//            KRankManager.Instance.ClearAllRankData();
//            _rankType = RankType.TotalScore;
//            _toggle1.isOn = true;
//        }
//        #endregion
//    }
//}

