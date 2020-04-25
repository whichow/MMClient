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
//    public partial class CatFeedWindow : KUIWindow
//    {
//        #region Constructor

//        public CatFeedWindow()
//            : base(UILayer.kNormal, UIMode.kSequenceHide)
//        {
//            uiPath = "CatFeed";
//        }

//        #endregion

//        #region Action

//        private int _lastGrade;
//        private int _lastCatCoinAbility;
//        private int _lastCatExploreAbility;
//        private int _lastCatMatchAbility;
//        private KCat _cat;
//        private void OnFeedBtnClick()
//        {
//            var cat = GetCat();
//            if (cat.maxGrade <= cat.grade)
//            {
//                ToastBox.ShowText(KLocalization.GetLocalString(54117));
//                return;
//            }
//            if (cat != null)
//            {
//                var feedCost = cat.feedCost;
//                var remainExp = cat.maxExp - cat.exp;
//                feedCost = Mathf.Min(remainExp, feedCost);

//                if (PlayerDataModel.Instance.mPlayerData.mCatFood >= feedCost)
//                {
//                    _lastGrade = cat.grade;
//                    _lastCatCoinAbility = cat.coinAbility;
//                    _lastCatExploreAbility = cat.exploreAbility;
//                    _lastCatMatchAbility = cat.matchAbility;
//                    KUser.FeedCat(cat.catId, 1, OnFeedCallback);
//                }
//                else
//                {
//                    LackHintBox.ShowLackHintBox(4);
//                }
//            }
//        }

//        private void OnBackBtnClick()
//        {
//            CloseWindow(this);
//        }

//        private void OnFeedCallback(int code, string message, object data)
//        {
//            if (code == 0)
//            {
//                bool critical = false;
//                int currGrade = 0;

//                var protoDatas = data as ArrayList;
//                if (protoDatas != null)
//                {
//                    for (int i = 0; i < protoDatas.Count; i++)
//                    {
//                        var protoData = protoDatas[i];
//                        if (protoData is Msg.ClientMessage.S2CCatFeedResult)
//                        {
//                            var originData = (Msg.ClientMessage.S2CCatFeedResult)protoData;
//                            currGrade = originData.CatLevel;
//                            critical = originData.IsCritical;
//                            _cat = KCatManager.Instance.GetCat(originData.CatId);
//                            break;
//                        }
//                    }
//                }

//                if (_catModel)
//                {
//                    var catBehaviour = _catModel.GetComponent<KCatBehaviour>();
//                    if (critical)
//                        Debug.Log("baoji");
//                    if (currGrade > _lastGrade)
//                    {
//                        catBehaviour.Upgrade();
//                        StartCoroutine(ShowText());
//                    }
//                    else
//                    {
//                        catBehaviour.Eat();
//                    }
//                }
//                RefreshView();
//            }
//        }
//        private IEnumerator ShowText()
//        {
//            _animation.enabled = true;
//            _animation.AnimationName = null;
//            _animation.AnimationName = "top";
//            //_animation.AnimationState.SetAnimation(0, "top", true);
//            _animation1.enabled = true;
//            _animation1.AnimationName = null;
//            _animation1.AnimationName = "down";
//            //_animation1.AnimationState.SetAnimation(0, "down", true);
//            yield return new WaitForSeconds(0.3f);
//            OpenWindow<CatUpBox>(new CatUpBox.Data
//            {
//                oldLv = _lastGrade.ToString(),
//                nowLv = _cat.grade.ToString(),
//                oldCoin = _lastCatCoinAbility.ToString(),
//                nowCoin = _cat.coinAbility.ToString(),
//                oldExploreAbility = _lastCatExploreAbility.ToString(),
//                nowExploreAbility = _cat.exploreAbility.ToString(),
//                oldMatchAbility = _lastCatMatchAbility.ToString(),
//                nowMatchAbility = _cat.matchAbility.ToString(),
//            });
       
//            yield return null;
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

//        public override void UpdatePerSecond()
//        {
//            if (_catModel)
//            {
//                var catBehaviour = _catModel.GetComponent<KCatBehaviour>();
//            }
//        }

//        #endregion
//    }
//}

