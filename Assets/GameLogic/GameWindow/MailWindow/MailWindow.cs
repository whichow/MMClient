using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    partial class MailWindow : KUIWindow
    {
        public MailWindow() :
            base(UILayer.kNormal, UIMode.kSequenceRemove)
        {
            uiPath = "MailWindow";
            uiAnim = UIAnim.kAnim1;
            hasMask = true;
        }

        public override void Awake()
        {
            InitView();
        }

        public override void OnEnable()
        {
            GameApp.Instance.GameServer.ReqMailData();
        }

        private void OnQuitBtnClick()
        {
            CloseWindow(this);
        }
    }
}
