// ***********************************************************************
// Assembly         : Unity
// Author           : LiMuChen
// Created          : 
//
// Last Modified By : LiMuChen
// Last Modified On : 
// ***********************************************************************
// <copyright file= "OpeningWindow.View" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace Game.UI
{
    partial class OpeningWindow
    {
        #region Field
        private Animator[] _animatorList;
        private Text _textDescribe;
        private Button _btnClose;
        private Image _imagBlack;
        #endregion

        #region Method

        public void InitView()
        {
            _imagBlack = Find<Image>("Black");
            var _goGroup = Find<Transform>("Group");
            _animatorList = new Animator[_goGroup.childCount];
            for (int i = 0; i < _animatorList.Length; i++)
            {
                _animatorList[i] = Find("Group/Open_" + (i + 1)).GetComponent<Animator>();
            }
            _btnClose = Find<Button>("Button");
            _btnClose.onClick.AddListener(OnCloseBtnClick);
            _textDescribe = Find<Text>("Text");
        }

        public void RefreshView()
        {
            if (_windowData != null)
            {
                if (_windowData.describeId!=null)
                {
                    StartCoroutine(StartAnmiton());
                }
            }
        }
        private IEnumerator StartAnmiton()
        {
            yield return null;
            while (!_animatorList[_animatorList.Length - 1].gameObject.activeSelf)
            {
                for (int i = 0; i < _animatorList.Length; i++)
                {
                    _animatorList[i].gameObject.SetActive(true);
                    KTweenUtils.DoImageColor(_imagBlack, new Color(0, 0, 0, 0), 0.8f);
                    yield return new WaitForSeconds(0.8f);
                    float anTime = _animatorList[i].runtimeAnimatorController.animationClips[0].length;
                    KTweenUtils.DoText(_textDescribe, KLocalization.GetLocalString(_windowData.describeId[i]), anTime - 0.5f);
                    yield return new WaitForSeconds(anTime);
                    if (i!=_animatorList.Length-1)
                    {
                        KTweenUtils.DoImageColor(_imagBlack, new Color(0, 0, 0, 1), 0.8f);
                        yield return new WaitForSeconds(0.8f);
                        _textDescribe.text = string.Empty;
                    }
                }
            }
            float anTime1 = _animatorList[_animatorList.Length - 1].runtimeAnimatorController.animationClips[0].length;
            //KTweenUtils.DoText(_textDescribe, KLocalization.GetLocalString(_windowData.describeId[_animatorList.Length - 1]), anTime1 - 0.5f);
            yield return new WaitForSeconds(anTime1);
            GuideManager.Instance.CompleteStep();
            CloseWindow(this);
        }
        public override void OnDisable()
        {

        }
        #endregion
    }
}

