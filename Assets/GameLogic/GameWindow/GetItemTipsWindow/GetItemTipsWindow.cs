using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public partial class GetItemTipsWindow : KUIWindow
    {
        private Button _buttOk;
        private Button _closeBtn;
        private Button _panelBtn;

        public GetItemTipsWindow()
            : base(UILayer.kPop, UIMode.kSequence)
        {
            uiPath = "GetItemTipsWindow";
        }

        public void OnCloseBtnClick()
        {
            CloseWindow(this);
        }

        public override void Awake()
        {
            _buttOk = Find<Button>("Panel/Confirm");
            _buttOk.onClick.AddListener(OnCloseBtnClick);
            _closeBtn = Find<Button>("Panel/Close");
            _closeBtn.onClick.AddListener(OnCloseBtnClick);
            _panelBtn = Find<Button>("BlackMask");
            _panelBtn.onClick.AddListener(OnCloseBtnClick);
            InitView();
        }

        public override void OnEnable()
        {
            RefreshView();
        }
    }
}
