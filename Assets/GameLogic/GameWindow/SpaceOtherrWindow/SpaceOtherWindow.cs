using Game.DataModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    partial class SpaceOtherWindow : KUIWindow
    {
        public SpaceOtherWindow() :
            base(UILayer.kNormal, UIMode.kSequenceHide)
        {
            uiPath = "SpaceOtherWindow";
            uiAnim = UIAnim.kAnim1;
            hasMask = true;
        }

        public override void Awake()
        {
            InitView();
        }

        public override void OnEnable()
        {
            _unitRenderTexture = UnitRenderTexture.Get(_rawImage);
            _unitRenderTexture.SetPlayer(SpaceDataModel.Instance.mSpaceOtherDataVO.mPlayerId);
            _avatar = _unitRenderTexture.GetPlayerAvatar();
            RefreshView();
            _isForm = false;
            _rawImage.gameObject.SetActive(false);
            _leftObj.SetActive(true);
            _formText.text = KLocalization.GetLocalString(54184);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            _unitRenderTexture.Dispose();
        }

        private void OnBackBtnClick()
        {
            CloseWindow(this);
        }

        private void OnCopyIdBtnClick()
        {
            TextEditor te = new TextEditor();
            te.text = _playerId.text;
            te.OnFocus();
            te.Copy();
            ToastBox.ShowText(KLocalization.GetLocalString(70028));
        }

        private void OnFollow()
        {
            if (SpaceDataModel.Instance.IsFocus(_spaceOtherDataVO.mPlayerId))
                GameApp.Instance.GameServer.ReqFocusCancal(_spaceOtherDataVO.mPlayerId);
            else
                GameApp.Instance.GameServer.ReqFocusPlayer(_spaceOtherDataVO.mPlayerId);
        }

        private void OnForm()
        {
            if (_isForm)
            {
                _leftObj.SetActive(true);
                _rawImage.gameObject.SetActive(false);
                _formText.text = KLocalization.GetLocalString(54184);
                _isForm = !_isForm;
            }
            else
            {
                _leftObj.SetActive(_spaceOtherDataVO.mGender == GenderConst.All);
                _rawImage.gameObject.SetActive(_spaceOtherDataVO.mGender > GenderConst.All);
                if (_spaceOtherDataVO.mGender == GenderConst.All)
                {
                    _formText.text = KLocalization.GetLocalString(54184);
                    ToastBox.ShowText(KLocalization.GetLocalString(54187));
                }
                else
                {
                    _formText.text = KLocalization.GetLocalString(58164);
                    _isForm = !_isForm;
                }
            }
        }
    }
}
