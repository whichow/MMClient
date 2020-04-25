// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "ConsoleWindow" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace K.Console
{
    /// <summary>
    /// 
    /// </summary>
    [DisallowMultipleComponent]
    public partial class ConsoleWindow : MonoBehaviour
    {
        #region Field  

        /// <summary>
        /// 
        /// </summary>
        private List<string> _historyInputs = new List<string>(10);
        /// <summary>
        /// 
        /// </summary>
        private int _maxHistory = 10;
        /// <summary>
        /// 
        /// </summary>
        private int _currIndex;

        #endregion

        #region Method

        /// <summary>
        /// Displays the given message as a new entry in the console output.
        /// </summary>
        public void AppendLine(string text)
        {
            if (_textBuffer.Length > 5120)
            {
                _textBuffer.Length = 0;
            }

            _textBuffer.AppendLine(text);
            RefreshView();
        }

        public void SelectInputHistory(bool up)
        {
            string navigatedToInput = SelectHistory(up);
            SetInputText(navigatedToInput);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        private void ExecCommand(string input)
        {
            var split = input.Split(' ');
            if (split != null && split.Length > 0)
            {
                AddHistory(input);

                var cmd = split[0];
                var args = new string[split.Length - 1];
                Array.Copy(split, 1, args, 0, args.Length);

                _textBuffer.AppendLine(">:" + input);
                _textBuffer.AppendLine(ConsoleCommand.ExecuteCommand(cmd, args));
                RefreshView();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        private void AddHistory(string input)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(input.Trim()))
            {
                return;
            }

            // Don't add the same input twice in a row
            if (_historyInputs.Count > 0 && input.Equals(_historyInputs[0], StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            // If we went over capacity, remove the oldest input entry to make room for a new one
            if (_historyInputs.Count == _maxHistory)
            {
                _historyInputs.RemoveAt(_maxHistory - 1);
            }

            // Insert the new input entry
            _historyInputs.Insert(0, input);

            // If the go-to input entry was removed for capacity reasons, then the new input entry becomes go-to input entry
            if (_currIndex == _maxHistory - 1)
            {
                _currIndex = 0;
            }
            // Otherwise make sure the go-to input entry remains the same by shifting the index
            // Note that if there was no input entry before, then the go-to input entry index remains 0 which is the new input entry
            else
            {
                _currIndex = Mathf.Clamp(++_currIndex, 0, _historyInputs.Count - 1);
            }

            // If the new input entry is different than the go-to input entry, then it becomes the go-to input entry
            if (!input.Equals(_historyInputs[_currIndex], StringComparison.OrdinalIgnoreCase))
            {
                _currIndex = 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearHistory()
        {
            _historyInputs.Clear();
            _currIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="upward"></param>
        /// <returns></returns>
        private string SelectHistory(bool upward)
        {
            if (_historyInputs.Count == 0)
            {
                return "";
            }

            if (upward)
            {
                _currIndex++;
            }
            else
            {
                _currIndex--;
            }

            _currIndex = Mathf.Clamp(_currIndex, 0, _historyInputs.Count - 1);

            return _historyInputs[_currIndex];
        }

        /// <summary>
        /// Clears the console output.
        /// </summary>
        public void ClearOutput()
        {
            _textBuffer.Length = 0;
            ClearHistory();
            RefreshView();
        }

        /// <summary>
        /// Clears the console input.
        /// </summary>
        public void ClearInput()
        {
            SetInputText(string.Empty);
        }

        /// <summary>
        /// Writes the given string into the console input, ready to be user submitted.
        /// </summary>
        private void SetInputText(string input)
        {
            _inputField.MoveTextStart(false);
            _inputField.text = input;
            _inputField.MoveTextEnd(false);
        }

        /// <summary>
        /// What to do when the user wants to submit a command.
        /// </summary>
        private void OnSubmit(string input)
        {
            if (EventSystem.current.alreadySelecting)
            {
                return;
            }

            ClearInput();
            //_inputField.ActivateInputField();

            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(input.Trim()))
            {
                return;
            }

            ExecCommand(input);

            if (_outputRect.verticalScrollbar)
            {
                _outputRect.verticalScrollbar.value = 0;
            }
        }

        #endregion 

        #region Unity

        private void Awake()
        {
            InitView();
        }

        private void OnEnable()
        {
            RefreshView();
        }

        #endregion
    }
}