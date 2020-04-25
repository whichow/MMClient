// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 2017-11-11
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "ToastBox.View" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    partial class ToastBox : KUIWindow
    {
        #region Field

        private GameObject _panel;
        private Text _content;
        private TweenAlph _tweenAlph;
        private TweenPos _tweenPos;
        #endregion

        #region Method

        public void InitView()
        {

            _panel = Find("Panel");
            _content = Find<Text>("Panel/Content");
            _tweenAlph = _panel.GetComponent<TweenAlph>();
            _tweenPos = _panel.GetComponent<TweenPos>();
        }

        public void RefreshView()
        {
            //_canvasGroup.alpha = GetAlpha();
            _content.text = GetContent();
            _tweenAlph.PlayBack();
            _tweenPos.PlayBack();
        }

        #endregion
    }
}
