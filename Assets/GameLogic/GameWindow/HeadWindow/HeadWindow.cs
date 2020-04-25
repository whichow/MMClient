using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    partial class HeadWindow : KUIWindow
    {
        public HeadWindow() :
            base(UILayer.kNormal, UIMode.kSequence)
        {
            uiPath = "HeadWindow";
            uiAnim = UIAnim.kAnim1;
            hasMask = true;
        }

        public override void Awake()
        {
            InitView();
        }

        public override void OnEnable()
        {
            RefreshView();
        }

        private void OnBackBtnClick()
        {
            CloseWindow(this);
        }
    }
}
