// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KLocalizationText" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class KLocalizationText : MonoBehaviour
    {
        public int textId;

        private Text _text;

        private void Awake()
        {
            _text = GetComponent<Text>();
        }

        private void OnEnable()
        {
            if (KLocalization.IsDefaultLanguage())
            {
                return;
            }

            if (textId == 0)
            {
                textId = KLocalization.GetStringId(_text.text);
            }
            if (textId != 0)
            {
                _text.text = KLocalization.GetLocalString(textId);
            }
        }
    }
}