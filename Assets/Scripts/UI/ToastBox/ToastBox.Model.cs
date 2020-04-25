// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 2017-11-11
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "ToastBox.Model" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using UnityEngine;

namespace Game.UI
{
    partial class ToastBox : KUIWindow
    {
        #region Field

        private float _alpha;

        #endregion

        #region Method

        public void RefreshModel()
        {
            _alpha = 1f;
        }

        public float GetAlpha()
        {
            _alpha = Mathf.Max(0f, _alpha - Time.deltaTime);
            if (_alpha <= 0f)
            {
                CloseWindow(this);
            }
            return _alpha;
        }

        public string GetContent()
        {
            if (data is string)
            {
                return (string)data;
            }
            return "";
        }

        #endregion
    }
}
