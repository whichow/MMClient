// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "BubbleTimeProgress" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Build
{
    public class BubbleTimeProgress : Bubble
    {
        public delegate void UpdateDelegate(out float progress, out string time);

        #region Field

        /// <summary>
        /// 
        /// </summary>
        public Image maskImage;
        /// <summary>
        /// 
        /// </summary>
        public Text timeText;

        private UpdateDelegate _updateDelegate;

        #endregion

        #region Property

        public float porgress
        {
            set
            {
                this.maskImage.fillAmount = value;
            }
        }

        public string time
        {
            set { timeText.text = value; }
        }

        #endregion

        public void SetUpdateDelegate(UpdateDelegate updateDelegate)
        {
            _updateDelegate = updateDelegate;
        }

        #region Unity  

        private void OnEnable()
        {
            _updateDelegate = null;
        }

        private void Update()
        {
            if (_updateDelegate != null)
            {
                float progress;
                string time;
                _updateDelegate(out progress, out time);
                this.porgress = progress;
                this.time = time;
            }
        }

        #endregion
    }
}
