// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "Bubble" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using UnityEngine;
using UnityEngine.UI;

namespace Game.Build
{
    public class BubbleProgress : Bubble
    {
        /// <summary>
        /// 
        /// </summary>
        public Image icon;
        /// <summary>
        /// 
        /// </summary>
        public Image mask;

        public void Set(float progress, Sprite icon)
        {
            this.mask.fillAmount = progress;
            if (icon == null)
            {
                this.icon.enabled = false;
            }
            else
            {
                this.icon.enabled = true;
                this.icon.sprite = icon;
            }
        }

        public float curPorgress
        {
            get
            {
                return this.mask.fillAmount;
            }
            set
            {
                this.mask.fillAmount = value;
            }
        }
    }
}

