///** 
//作用：排行榜的界面流程控制类
//*/
//using Msg.ClientMessage;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using Callback = System.Action<int, string, object>;
//namespace Game.UI
//{
//    partial class RankListWindow : KUIWindow
//    {
//        #region Constructor
//        public RankListWindow()
//            : base(UILayer.kNormal, UIMode.kSequenceRemove)
//        {
//            uiPath = "RankList";
//            uiAnim = UIAnim.kAnim1;
//            hasMask = true;
//        }
//        #endregion
//        #region Action
//        public void OnToggleValueChanged(bool value)
//        {
//            var rankType = _rankType;

//            if (_toggle1.isOn)
//            {
//                rankType = RankType.TotalScore;
//            }
//            if (_toggle2.isOn)
//            {
//                //rankType = RankType.kCustomsPass;
//                rankType = RankType.kCharm;
//            }
//            if (_toggle3.isOn)
//            {
//                rankType = RankType.kOuQi;
//            }
//            if (_toggle4.isOn)
//            {
//                rankType = RankType.kZan;
//            }
//            if (rankType != _rankType)
//            {
//                _rankType = rankType;
//                RefreshView();
//            }
//        }
//        private void OnCloseBtnClick()
//        {
//            CloseWindow(this);
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
//            KRankManager.Instance.GetMyBestCatIDOperation();
//            ResetModel();
//            RefreshView();
//        }
//        #endregion
//    }
//}

