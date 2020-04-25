//// ***********************************************************************
//// Assembly         : Unity
//// Author           : Kimch
//// Created          : 
////
//// Last Modified By : Kimch
//// Last Modified On : 
//// ***********************************************************************
//// <copyright file= "MessageBox.View" company=""></copyright>
//// <summary></summary>
//// ***********************************************************************
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//namespace Game.UI
//{
//    partial class CatUpBox
//    {
//        #region Field
//        private Text _textOldLv;
//        private Text _textNowLv;

//        private Text _textOldCoin;
//        private Text _textNowCoin;

//        private Text _textOldExploreAbility;
//        private Text _textNowExploreAbility;

//        private Text _textOldMatchAbility;
//        private Text _textNowMatchAbility;
//        private TweenAlph _tweenAlph;
//        private TweenPos _tweenPos;
//        private TweenRotate _tweenRota;
//        #endregion

//        #region Method

//        public void InitView()
//        {
//            _textOldLv = Find<Text>("Panel/Content (1)/Text");
//            _textNowLv = Find<Text>("Panel/Content (1)/Text (1)");
//            _textOldCoin = Find<Text>("Panel/Content (2)/Text");
//            _textNowCoin = Find<Text>("Panel/Content (2)/Text (1)");
//            _textOldExploreAbility = Find<Text>("Panel/Content (3)/Text");
//            _textNowExploreAbility = Find<Text>("Panel/Content (3)/Text (1)");
//            _textOldMatchAbility = Find<Text>("Panel/Content (4)/Text");
//            _textNowMatchAbility = Find<Text>("Panel/Content (4)/Text (1)");
//            _tweenAlph = Find<TweenAlph>("Panel");
//            _tweenPos = Find<TweenPos>("Panel");
//            _tweenRota = Find<TweenRotate>("Panel");
//        }

//        public void RefreshView()
//        {
//            _tweenAlph.enabled=true;
//            _tweenAlph.Reset(true);
//            StartCoroutine(CloseWindow());
//            _tweenPos.enabled = true;
//            _tweenPos.Reset(true);
//            _tweenRota.enabled = true;
//            _tweenRota.Reset(true);
//            _textOldLv.text = _catUpData.oldLv;
//            _textNowLv.text = _catUpData.nowLv;
//            _textOldCoin.text = _catUpData.oldCoin;
//            _textNowCoin.text = _catUpData.nowCoin;
//            _textOldExploreAbility.text = _catUpData.oldExploreAbility;
//            _textNowExploreAbility.text = _catUpData.nowExploreAbility;
//            _textOldMatchAbility.text = _catUpData.oldMatchAbility;
//            _textNowMatchAbility.text = _catUpData.nowMatchAbility;
//        }

//        private IEnumerator CloseWindow()
//        {
//            yield return new WaitForSeconds(4f);
//            CloseWindow(this);
//        }
//        #endregion
//    }
//}

