// ***********************************************************************
// Assembly         : Unity
// Author           : LiMuChen
// Created          : 
//
// Last Modified By : LiMuChen
// Last Modified On : 
// ***********************************************************************
// <copyright file= "NoviceChangeName" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Text;

namespace Game.UI
{
    /// <summary>
    /// 引导起名
    /// </summary>
    public partial class NoviceChangeName : KUIWindow
    {
        #region Static
        public bool isHaveName = false;
        #endregion

        #region Constructor

        public NoviceChangeName() : base(UILayer.kPop, UIMode.kNone)
        {
            uiPath = "ChangeName";
        }

        #endregion

        #region Action
        private void OnRanNameBtnClick()
        {
            _field.text = KPlayer.GetRandomName();

        }

        private void OnOkBtnClick()
        {
            var name = _textName.text;
            if (string.IsNullOrEmpty(name))
            {
                ToastBox.ShowText("昵称不能为空");
                return;
            }
            if (GetStringLength(name) > 18)
            {
                ToastBox.ShowText("昵称不可超过18个字节");
                return;
            }
            if (!BadWords.ContainsAny(name))
            {
                //KUser.ChangeName(name, ChangeNameCallBack);
                GameApp.Instance.GameServer.ChangeNameRequest(name);
            }
            else
            {
                ToastBox.ShowText("名字不合法");
            }
        }

        //private void ChangeNameCallBack(int code, string message, object data)
        //{
        //    if (code == 0)
        //    {
        //        GuideManager.Instance.CompleteStep();
        //        isHaveName = true;
        //        CloseWindow(this);
        //    }
        //}

        #endregion

        public int GetStringLength(string str)
        {
            if (str.Equals(string.Empty))
                return 0;
            int strlen = 0;
            ASCIIEncoding strData = new ASCIIEncoding();
            //将字符串转换为ASCII编码的字节数字
            byte[] strBytes = strData.GetBytes(str);
            for (int i = 0; i <= strBytes.Length - 1; i++)
            {
                if (strBytes[i] == 63)  //中文都将编码为ASCII编码63,即"?"号
                    strlen++;
                strlen++;
            }
            return strlen;
        }

        private void OnChangeNameHandler(IEventData args)
        {
            GuideManager.Instance.CompleteStep();
            isHaveName = true;
            CloseWindow(this);
        }

        #region Unity  

        public override void Awake()
        {
            InitModel();
            InitView();
        }

        public override void OnEnable()
        {
            RefreshModel();
            RefreshView();
        }

        public override void AddEvents()
        {
            base.AddEvents();
            PlayerDataModel.Instance.AddEvent(PlayerEvent.ChangeName, OnChangeNameHandler);
        }

        public override void RemoveEvents()
        {
            base.RemoveEvents();
            PlayerDataModel.Instance.RemoveEvent(PlayerEvent.ChangeName, OnChangeNameHandler);
        }

        #endregion
    }
}

