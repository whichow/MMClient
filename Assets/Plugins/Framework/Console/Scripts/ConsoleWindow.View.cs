// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "ConsoleWindow.View" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace K.Console
{
    partial class ConsoleWindow
    {
        #region Field

        /// <summary>
        /// 输出
        /// </summary>
        private ScrollRect _outputRect;
        /// <summary>
        /// 
        /// </summary>
        private InputField _inputField;
        /// <summary>
        /// 
        /// </summary>
        private Text _outputText;

        #endregion

        #region Method

        public void InitView()
        {
            _inputField = transform.Find("InputField").GetComponent<InputField>();
            _inputField.onEndEdit.AddListener(this.OnSubmit);
            _outputRect = transform.Find("Scroll View").GetComponent<ScrollRect>();
            _outputText = transform.Find("Scroll View/Viewport/Content").GetComponent<Text>();
            transform.Find("Close").GetComponent<Button>().onClick.AddListener(Console.Hide);
        }

        public void RefreshView()
        {
            //if (_inputField)
            //{
            //    _inputField.ActivateInputField();
            //}

            if (_outputText)
            {
                _outputText.text = _textBuffer.ToString();
            }

            if (_outputRect)
            {
                _outputRect.verticalNormalizedPosition = 0;
            }
        }

        #endregion

    }
}

