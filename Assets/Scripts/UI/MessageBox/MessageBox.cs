// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "MessageBox" company=""></copyright>
// <summary></summary>
// ***********************************************************************

namespace Game.UI
{
    public partial class MessageBox : KUIWindow
    {
        #region Static

        public static void ShowMessage(string title, string content)
        {
            DefaultData.title = title;
            DefaultData.content = content;
            DefaultData.onConfirm = null;
            DefaultData.onCancel = null;
            OpenWindow<MessageBox>(DefaultData);
        }

        public static void ShowMessage(string title, string content, System.Action onConfirm)
        {
            DefaultData.title = title;
            DefaultData.content = content;
            DefaultData.onConfirm = onConfirm;
            DefaultData.onCancel = null;
            OpenWindow<MessageBox>(DefaultData);
        }

        public static void ShowMessage(string title, string content, System.Action onConfirm, System.Action onCancel)
        {
            DefaultData.title = title;
            DefaultData.content = content;
            DefaultData.onConfirm = onConfirm;
            DefaultData.onCancel = onCancel;
            OpenWindow<MessageBox>(DefaultData);
        }

        public static void HideMessage()
        {
            CloseWindow<MessageBox>();
        }

        #endregion

        #region Constructor

        public MessageBox()
                    : base(UILayer.kPop, UIMode.kNone)
        {
            uiPath = "MessageBox";
        }

        #endregion

        #region Action

        private void OnConfirmClick()
        {
            if (_messageData.onConfirm != null)
            {
                K.Events.EventInvoker.Invoke(_messageData.onConfirm);
            }
            CloseWindow(this);
        }

        private void OnCancelClick()
        {
            if (_messageData.onCancel != null)
            {
                K.Events.EventInvoker.Invoke(_messageData.onCancel);
            }
            CloseWindow(this);
        }

        #endregion

        #region Unity  

        // Use this for initialization
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

        #endregion
    }
}

