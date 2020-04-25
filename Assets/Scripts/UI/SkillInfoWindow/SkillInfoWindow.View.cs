// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "SkillInfoWindow" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public partial class SkillInfoWindow
    {
        private Button _blackMaskBtn;
        private Image _iconImage;
        private Text _titleText;
        private Text _energyCostText;
        private Text _descriptionText;

        public void InitView()
        {
            _blackMaskBtn = Find<Button>("BlackMask");
            _blackMaskBtn.onClick.AddListener(OnBlackMaskClick);

            _iconImage = Find<Image>("Icon");
            _titleText = Find<Text>("Title");
            _energyCostText = Find<Text>("EnergyCost");
            _descriptionText = Find<Text>("Description");
        }

        public void RefreshView()
        {
            _iconImage.overrideSprite = GetIcon();
            _titleText.text = GetTitle();
            _energyCostText.text = GetEnergyCost();
            _descriptionText.text = GetDescription();
        }
    }
}

