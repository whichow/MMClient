using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public partial class SettingPopChangNameWindow
    {

        private Button _backBtn;
        private Button _buttonEnter;


        private Text _textName;



        public void InitView()
        {
            _backBtn = Find<Button>("Cancel");
            _backBtn.onClick.AddListener(this.OnBackBtnClick);
            _buttonEnter = Find<Button>("Confirm");
            _buttonEnter.onClick.AddListener(EnterBtnOnClick);
            _textName = Find<Text>("InputFieldName/Placeholder");


        }

        private void EnterBtnOnClick()
        {
         
            var name = _textName.text;
  
            if (name.Length > 8)
            {
                ToastBox.ShowText("昵称不可超过8个字符");
                return;
            }
            if (string.IsNullOrEmpty(name))
            {
                ToastBox.ShowText("昵称为空");
                return;
            }
            if (name == PlayerDataModel.Instance.mPlayerData.mName)
            {
                ToastBox.ShowText("昵称相同");
                return;
            }
            if (!BadWords.ContainsAny(name))
            {
                //KUser.ChangeName(name, ChangeNameCallBack);
                GameApp.Instance.GameServer.ChangeNameRequest(name);
            }
            else
            {
                ToastBox.ShowText("昵称不合法");
            }

        }
        //private void ChangeNameCallBack(int code, string message, object data)
        //{
        //    if (code == 0)
        //    {
        //        CloseWindow<SettingPopChangNameWindow>();
        //        KUIWindow.GetWindow<SettingWindow>().RefreshView();
        //    }
        //}


        public void RefreshView()
        {


        }
    }
}
