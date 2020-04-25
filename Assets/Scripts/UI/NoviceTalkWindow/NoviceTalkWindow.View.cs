// ***********************************************************************
// Assembly         : Unity
// Author           : LiMuChen
// Created          : 
//
// Last Modified By : LiMuChen
// Last Modified On : 
// ***********************************************************************
// <copyright file= "NoviceTalkWindow.View" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /// <summary>
    /// 引导类型2 黑底文本叙述
    /// </summary>
    partial class NoviceTalkWindow
    {
        #region Field

        private Text _textContent;
        private Button _btnBlack;

        private Button _button1;
        private Text _textBtn1;
        private Button _button2;
        private Text _textBtn2;

        private int talkNum;
        private int talkIndx;

        #endregion

        #region Method

        public void InitView()
        {
            _textContent = Find<Text>("Text");
            _btnBlack = Find<Button>("BackGround");

            _button1 = Find<Button>("Button1");
            _button1.onClick.AddListener(OnBtnClick);
            _textBtn1 = Find<Text>("Button1/Text");
            _button2 = Find<Button>("Button2");
            _button2.onClick.AddListener(OnBtnClick);
            _textBtn2 = Find<Text>("Button2/Text");
        }
        private void OnBtnClick()
        {
            if ((talkIndx + 3) / 3 < talkNum)
            {
                _textContent.color = new Color(1f, 1f, 1f, 0f);
                _button1.gameObject.SetActive(false);
                _button2.gameObject.SetActive(false);
                talkIndx += 3;
                RefrshText();
                RefrshBtn();
            }
        }
        public void RefreshView()
        {
            List<int> talking = _noviceTalkData.action.Dialogs;
            if (talking.Count % 3 == 0)
            {
                talkNum = talking.Count / 3;
                talkIndx = 0;
            }
            else
            {
                Debug.Log("语言ID配置数量不正确");
                return;
            }
            RefrshBtn();
            StartCoroutine(DoText());
        }

        private void RefrshText()
        {
            StartCoroutine(DoText());
        }

        private IEnumerator DoText()
        {
            List<int> talking = _noviceTalkData.action.Dialogs;
            _textContent.text = KLocalization.GetLocalString(talking[talkIndx]);
            KTweenUtils.DoTextColor(_textContent, 1f, 1f, 1f, 1f, 3f);
            yield return new WaitForSeconds(1.5f);
            //_textContent.color = new Color(1f, 1f, 1f, 1f / 255f);
            if (talking[talkIndx + 1] != 0 && talking[talkIndx + 2] != 0)
            {
                _textBtn1.text = KLocalization.GetLocalString(talking[talkIndx + 1]);
                _textBtn2.text = KLocalization.GetLocalString(talking[talkIndx + 2]);
                _button1.gameObject.SetActive(true);
                _button2.gameObject.SetActive(true);
            }
            else
            {
                _button1.gameObject.SetActive(false);
                _button2.gameObject.SetActive(false);
            }
            yield return new WaitForSeconds(2f);
            if ((talkIndx + 3) / 3 == talkNum)
            {
                GuideManager.Instance.CompleteStep();
                CloseWindow(this);
            }
            yield return null;
        }

        private void RefrshBtn()
        {
            List<int> talking = _noviceTalkData.action.Dialogs;
            if (talking[talkIndx + 1] != 0 && talking[talkIndx + 2] != 0)
            {
                _btnBlack.onClick.RemoveAllListeners();
            }
            else
            {
                _btnBlack.onClick.AddListener(OnBtnClick);
            }
        }

        #endregion

    }
}

