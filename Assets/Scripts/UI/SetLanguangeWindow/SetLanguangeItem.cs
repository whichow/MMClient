// ***********************************************************************
// Assembly         : Unity
// Author           : Hejunjie
// Created          : #DATA#
//
// Last Modified By : Hejunjie
// Last Modified On : 
// ***********************************************************************
// <copyright file= "SetLanguangeItem" company="moyu"></copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI
{
    public class SetLanguangeItem : KUIItem, IPointerClickHandler
    {
        private Image languangeIcon;

        private Text _textLanguangeName;


        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            KLanguageManager.Instance.SetLanguage((data as KLanguage).name);
        }

        private void ShowLanguange(KLanguage language)
        {

            languangeIcon.overrideSprite = KIconManager.Instance.GetItemIcon(language.iconName);
            _textLanguangeName.text = language.name;
        }
        protected override void Refresh()
        {
      
            ShowLanguange(data as KLanguage);
        }



        void Awake()
        {
            languangeIcon = Find<Image>("Item/Image");
            _textLanguangeName = Find<Text>("Item/Text");
        }


        void Update()
        {

        }
    }
}