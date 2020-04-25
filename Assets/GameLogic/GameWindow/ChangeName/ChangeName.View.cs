// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "ChangeNameBox" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public partial class ChangeName
    {
        private Image _iconImage;
        private InputField _inputField;
        private Button _backBtn;
        private Button _confirmBtn;
        private ChangeNameData _dataVO;

        public void InitView()
        {
            _iconImage = Find<Image>("HeadBack/HeadIcon");
            _inputField = Find<InputField>("InputField");
            _inputField.onValidateInput = OnValidateInput;
            _backBtn = Find<Button>("Cancel");
            _backBtn.onClick.AddListener(OnBackBtnClick);
            _confirmBtn = Find<Button>("Confirm");
            _confirmBtn.onClick.AddListener(OnConfirmBtnClick);
        }

        public override void AddEvents()
        {
            base.AddEvents();
            CatDataModel.Instance.AddEvent(CatEvent.CatNick, OnChangeName);
            PlayerDataModel.Instance.AddEvent(PlayerEvent.ChangeName, OnChangeName);
        }

        public override void RemoveEvents()
        {
            base.RemoveEvents();
            CatDataModel.Instance.RemoveEvent(CatEvent.CatNick, OnChangeName);
            PlayerDataModel.Instance.RemoveEvent(PlayerEvent.ChangeName, OnChangeName);
        }

        public int GetStringLength(string str)
        {
            if (str.Equals(string.Empty))
                return 0;
            int strlen = 0;
            ASCIIEncoding strData = new ASCIIEncoding(); //将字符串转换为ASCII编码的字节数字
            byte[] strBytes = strData.GetBytes(str);
            for (int i = 0; i <= strBytes.Length - 1; i++)
            {
                if (strBytes[i] == 63)  //中文都将编码为ASCII编码63,即"?"号
                    strlen++;
                strlen++;
            }
            return strlen;
        }

        public char OnValidateInput(string text, int charInput, char addedChar)
        {
            if (GetStringLength((text + addedChar).ToString()) > 14)
                return '\0';
            return addedChar;
        }

        private void OnConfirmBtnClick()
        {
            if (string.IsNullOrEmpty(_inputField.text) || BadWords.ContainsAny(_inputField.text))
            {
                ToastBox.ShowText(KLocalization.GetLocalString(54169));
                return;
            }
            if (_dataVO.type == NmaeType.CatName)
                GameApp.Instance.GameServer.ReqCatNick((_dataVO.data as CatDataVO).mCatInfo.Id, _inputField.text);
            else
                GameApp.Instance.GameServer.ReqChangeName(_inputField.text);
        }

        private void OnChangeName()
        {
            ToastBox.ShowText(KLocalization.GetLocalString(54170));
            CloseWindow(this);
        }

        public void RefreshView()
        {
            _inputField.text = "";
            _dataVO = data as ChangeNameData;
            if (_dataVO.type == NmaeType.CatName)
                _iconImage.overrideSprite = KIconManager.Instance.GetCatIcon((_dataVO.data as CatDataVO).mCatXDM.Icon);
            else
                _iconImage.overrideSprite = KIconManager.Instance.GetHeadIcon((_dataVO.data as PlayerDataVO).mHead);
        }
    }
}
