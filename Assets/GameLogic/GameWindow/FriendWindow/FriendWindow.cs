using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    partial class FriendWindow : KUIWindow
    {
        private Button _closeBtn;
        private Toggle[] _toggles;
        public int _friendType { get; private set; }

        public FriendWindow() :
            base(UILayer.kNormal, UIMode.kSequenceHide)
        {
            uiPath = "FriendWindow";
            uiAnim = UIAnim.kAnim1;
            hasMask = true;
        }

        public override void Awake()
        {
            _closeBtn = Find<Button>("Close");
            _closeBtn.onClick.AddListener(OnCloseBtnClick);
            _toggles = new Toggle[2];
            for (int i = 0; i < 2; i++)
                _toggles[i] = Find<Toggle>("ToggleGroup/Tog" + (i + 1));
            foreach (Toggle tog in _toggles)
                tog.onValueChanged.AddListener((bool blSelect) => { if (blSelect) OnFriendTypeChange(tog); });
            _friendType = FriendTypeConst.Friend;
            InitView();
        }

        private void OnFriendTypeChange(Toggle tog)
        {
            switch (tog.name)
            {
                case "Tog1":
                    _friendType = FriendTypeConst.Friend;
                    break;
                case "Tog2":
                    _friendType = FriendTypeConst.AddFriend;
                    break;
            }
            GameApp.Instance.GameServer.ReqFriendData();
            //if (!FriendDataModel.Instance.isData)
            //    GameApp.Instance.GameServer.ReqFriendData();
            //else
            //    RefreshView(_friendType);
        }

        private void OnFriendData()
        {
            RefreshView(_friendType);
        }

        public void OnCloseBtnClick()
        {
            CloseWindow(this);
        }

        public override void OnEnable()
        {
            OnFriendTypeChange(_toggles[_friendType]);
        }
    }
}
