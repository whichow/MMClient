using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    partial class SpaceWindow : KUIWindow
    {
        private int _formType;

        public SpaceWindow() :
            base(UILayer.kNormal, UIMode.kSequenceHide)
        {
            uiPath = "SpaceWindow";
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

        private void OnChangeName()
        {
            ChangeNameData nameData = new ChangeNameData();
            nameData.data = _playerDataVO;
            nameData.type = NmaeType.PlayerName;
            OpenWindow<ChangeName>(nameData);
        }

        private void OnCopyIdBtnClick()
        {
            TextEditor te = new TextEditor();
            te.text = _playerId.text;
            te.OnFocus();
            te.Copy();
            ToastBox.ShowText(KLocalization.GetLocalString(54191));
        }

        private void OnChangeHead()
        {
            KUIWindow.OpenWindow<HeadWindow>();
        }

        private void OnFashionData()
        {
            if (SpaceDataModel.Instance.mGender == GenderConst.All)
                _formObj.SetActive(true);
            else
                KUIWindow.OpenWindow<FormWindow>();
        }

        private void OnSetGender()
        {
            KUIWindow.OpenWindow<FormWindow>();
        }

        private void OnFormClose()
        {
            _formObj.SetActive(false);
        }

        private void OnForm()
        {
            GameApp.Instance.GameServer.ReqFashionData();
        }

        private void OnGirl()
        {
            _formType = GenderConst.Girl;
            OnBox();
        }

        private void OnMan()
        {
            _formType = GenderConst.Man;
            OnBox();
        }

        private void OnBox()
        {
            OpenWindow<MessageBox>(new MessageBox.Data()
            {
                onConfirm = CompCat,
                onCancel = OnCancel,
                content = KLocalization.GetLocalString(53098)
            });
        }

        private void CompCat()
        {
            _formObj.SetActive(false);
            GameApp.Instance.GameServer.ReqSetGender(_formType);
        }

        private void OnCancel()
        {

        }

        private void OnSetting()
        {
            KUIWindow.OpenWindow<SettingWindow>();
        }
    }
}
