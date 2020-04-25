// ***********************************************************************
// Assembly         : Unity
// Author           : LiMuChen
// Created          : 
//
// Last Modified By : LiMuChen
// Last Modified On : 
// ***********************************************************************
// <copyright file= "GiveNameWindow.View" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using UnityEngine.UI;

namespace Game.UI
{
    partial class NoviceChangeName
    {
        #region Field
        private Button _btnRandName;
        private Text _textName;
        private Text _talkName;
        private Button _btnOkName;
        private Text _textTalk;
        private InputField _field;
        #endregion

        #region Method

        public void InitView()
        {
            _btnRandName = Find<Button>("Nickname/Random");
            _btnRandName.onClick.AddListener(OnRanNameBtnClick);
            _textName = Find<Text>("Nickname/Image/Text/Text");
            _talkName = Find<Text>("Talk/Image/Name");
            _btnOkName = Find<Button>("Nickname/Button");
            _btnOkName.onClick.AddListener(OnOkBtnClick);
            _textTalk = Find<Text>("Talk/Text");
            _field = Find<InputField>("Nickname/Image/Text");
        }

        public void RefreshView()
        {
            if (_changeNameData != null)
            {
                _talkName.text = "我";
                _textTalk.text = KLocalization.GetLocalString(_changeNameData.action.Dialogs[0]);
            }
        }

        #endregion
    }
}

