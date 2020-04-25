using Game.DataModel;
using Msg.ClientMessage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    partial class SpaceOtherWindow
    {
        private SpaceOtherDataVO _spaceOtherDataVO;
        private Button _close;
        private Image _head;
        private Text _level;
        private Text _name;
        private Text _playerId;
        private Button _buttonCopyId;
        private Text _zanNum;
        private Text _followNum;
        private Text _charmValNum;
        private Button _followBtn;
        private Button _formBtn;
        private Text _formText;
        private Text _followText;
        private GameObject _leftObj;

        private RawImage _rawImage;
        private Button _rawBtn;
        private UnitRenderTexture _unitRenderTexture;
        private PlayerAvatar _avatar;

        private UIList _pictureList;
        private bool _isPicture;
        private Text _pictureName;
        private Button _pictureBtn;
        private GameObject _pictureObj;
        private Image _pictureIcon;
        private Button _pictureClose;
        private GameObject _hintObj;

        private bool _isForm;
        private int _curCatId = 0;

        private void InitView()
        {
            _close = Find<Button>("Left/Close");
            _close.onClick.AddListener(OnBackBtnClick);
            _head = Find<Image>("Left/LeftObj/ImageHead");
            _level = Find<Text>("Left/LeftObj/ImageHead/Image/Text");
            _name = Find<Text>("Left/LeftObj/NiC/ImageNameBack/name");
            _playerId = Find<Text>("Left/LeftObj/ID/IdNum");
            _buttonCopyId = Find<Button>("Left/LeftObj/ID/ChangeNameButton");
            _buttonCopyId.onClick.AddListener(OnCopyIdBtnClick);
            _zanNum = Find<Text>("Left/LeftObj/PraiseNumIcon/praiseNum/Num");
            _followNum = Find<Text>("Left/LeftObj/FollowNumIcon/FollowNum/Num");
            _charmValNum = Find<Text>("Left/LeftObj/StarNumIcon/starNum/Num");
            _followBtn = Find<Button>("Left/LeftObj/FollowBtn");
            _followBtn.onClick.AddListener(OnFollow);
            _formBtn = Find<Button>("Left/FormBtn");
            _formBtn.onClick.AddListener(OnForm);
            _formText = Find<Text>("Left/FormBtn/Text");
            _followText = Find<Text>("Left/LeftObj/FollowBtn/Text");
            _leftObj = Find("Left/LeftObj");

            _rawImage = Find<RawImage>("Left/RawImage");
            _rawBtn = Find<Button>("Left/RawImage");
            _rawBtn.onClick.AddListener(OnPlayAnimation);

            _pictureList = Find<UIList>("Right/List");
            _pictureList.CanScroll = false;
            _pictureList.SetRenderHandler(RenderHandler);
            _pictureList.SetPointerHandler(PointerHandler);
            _pictureName = Find<Text>("Right/Picture/name");
            _pictureBtn = Find<Button>("Right/Picture/Btn");
            _pictureBtn.onClick.AddListener(OnPictureBtn);
            _pictureObj = Find("Right/PictureIcon");
            _pictureIcon = Find<Image>("Right/PictureIcon/Img/Icon");
            _pictureClose = Find<Button>("Right/PictureIcon/Img/Close");
            _pictureClose.onClick.AddListener(OnClosePicture);
            _hintObj = Find("Right/Hint");
        }

        public override void AddEvents()
        {
            base.AddEvents();
            SpaceDataModel.Instance.AddEvent(SpaceEvent.Focus, OnFollowChange);
            SpaceDataModel.Instance.AddEvent(SpaceEvent.FocusCancel, OnFollowChange);
            SpaceDataModel.Instance.AddEvent(SpaceEvent.CatUnlock, OnCatUnlock);
        }

        public override void RemoveEvents()
        {
            base.RemoveEvents();
            SpaceDataModel.Instance.RemoveEvent(SpaceEvent.Focus, OnFollowChange);
            SpaceDataModel.Instance.RemoveEvent(SpaceEvent.FocusCancel, OnFollowChange);
            SpaceDataModel.Instance.RemoveEvent(SpaceEvent.CatUnlock, OnCatUnlock);
        }

        private void OnCatUnlock()
        {
            _spaceOtherDataVO = SpaceDataModel.Instance.mSpaceOtherDataVO;
            _pictureList.DataArray = _spaceOtherDataVO.mLstSpaceCat;
        }

        private void OnPlayAnimation()
        {
            int index = UnityEngine.Random.Range(0, 3);
            switch (index)
            {
                case 0:
                    _unitRenderTexture.PlayAnimation(PlayerAvatar.ROLE_COSTUME);
                    break;
                case 1:
                    _unitRenderTexture.PlayAnimation(PlayerAvatar.ROLE_APPLAUSE);
                    break;
                case 2:
                    _unitRenderTexture.PlayAnimation(PlayerAvatar.ROLE_RANKING);
                    break;
            }
        }

        private void PointerHandler(UIListItem item, int index)
        {
            SpaceCatData value = item.dataSource as SpaceCatData;
            if (value == null)
                return;
        }

        private void RenderHandler(UIListItem item, int index)
        {
            bool isOpen = false;
            SpaceCatData value = item.dataSource as SpaceCatData;
            Image catIcon = item.GetComp<Image>("Cat/Icon");
            Image pictureIcon = item.GetComp<Image>("Picture/Icon");
            if (value == null)
                return;
            item.GetComp<Button>("Cat").onClick.RemoveAllListeners();
            item.GetComp<Button>("Cat").onClick.AddListener(() => { OnCat(value.CatId, index); });
            item.GetComp<Button>("Picture/Icon").onClick.RemoveAllListeners();
            item.GetComp<Button>("Picture/Icon").onClick.AddListener(() => { OnPicture(pictureIcon.overrideSprite, isOpen); });
            item.GetComp<Button>("Picture/Img").onClick.RemoveAllListeners();
            item.GetComp<Button>("Picture/Img").onClick.AddListener(() => { OnPictureImg(value.CatId); });
            item.GetGameObject("Cat").SetActive(_isPicture);
            item.GetGameObject("Picture").SetActive(!_isPicture);
            CatXDM catXDM = XTable.CatXTable.GetByID(value.CatTableId);
            if (catXDM != null)
                catIcon.overrideSprite = KIconManager.Instance.GetCatIcon(catXDM.Icon);
            if (!_isPicture)
            {
                string fileName = string.Format("{0}-{1}.jpg", _spaceOtherDataVO.mPlayerId, value.CatId);
                ImageUtils.LoadNetImage(fileName, pictureIcon, (texture2D) => {
                    item.GetGameObject("Picture/Img").SetActive(texture2D != null && !value.IsUnlock);
                    if (texture2D != null)
                        isOpen = true;
                });
            }
        }

        private void RefreshView()
        {
            _isPicture = true;
            _spaceOtherDataVO = SpaceDataModel.Instance.mSpaceOtherDataVO;
            HeadIconUtils.SetHeadIcon(_spaceOtherDataVO.mPlayerHead, _spaceOtherDataVO.mPlayerId, _head);
            _level.text = _spaceOtherDataVO.mPlayerLevel.ToString();
            _name.text = _spaceOtherDataVO.mPlayerName;
            _playerId.text = _spaceOtherDataVO.mPlayerId.ToString();
            _zanNum.text = _spaceOtherDataVO.mZaned.ToString();
            _charmValNum.text = _spaceOtherDataVO.mCharm.ToString();
            SpacePicture();
            OnFollowChange();
        }

        private void OnFollowChange()
        {
            if (SpaceDataModel.Instance.IsFocus(_spaceOtherDataVO.mPlayerId))
                _followText.text = KLocalization.GetLocalString(54186);
            else
                _followText.text = KLocalization.GetLocalString(54185);
            _followNum.text = _spaceOtherDataVO.mFocusNum.ToString();
        }

        private void SpacePicture()
        {
            if (_isPicture)
                _pictureName.text = KLocalization.GetLocalString(54171);
            else
                _pictureName.text = KLocalization.GetLocalString(54172);
            _spaceOtherDataVO.mLstSpaceCat.Sort((x, y) => -x.CatTableId.CompareTo(y.CatTableId));
            _pictureList.DataArray = _spaceOtherDataVO.mLstSpaceCat;
            _hintObj.SetActive(_pictureList.DataArray.Count == 0);
        }

        private void OnPictureBtn()
        {
            _isPicture = !_isPicture;
            SpacePicture();
        }

        private void OnCat(int catId,int index)
        {
            List<CatDataVO> catDataVOs = new List<CatDataVO>();
            for (int i = 0; i < _spaceOtherDataVO.mLstSpaceCat.Count; i++)
            {
                CatDataVO vo = new CatDataVO();
                CatInfo info = new CatInfo();
                info.Id = _spaceOtherDataVO.mLstSpaceCat[i].CatId;
                info.CatCfgId = _spaceOtherDataVO.mLstSpaceCat[i].CatTableId;
                info.Nick = _spaceOtherDataVO.mLstSpaceCat[i].CatName;
                info.CoinAbility = _spaceOtherDataVO.mLstSpaceCat[i].CoinAbility;
                info.ExploreAbility = _spaceOtherDataVO.mLstSpaceCat[i].ExploreAbility;
                info.MatchAbility = _spaceOtherDataVO.mLstSpaceCat[i].MatchAbility;
                vo.OnInit(info);
                catDataVOs.Add(vo);
            }
            CatDatas catDatas = new CatDatas();
            catDatas.mCatDataVOs = catDataVOs;
            catDatas.mIndex = index;
            catDatas.mOpenType = CatOpenType.SpaceOther;
            KUIWindow.OpenWindow<CatInfoWindow>(catDatas);
        }

        private void OnPicture(Sprite sprite,bool isOpen)
        {
            if (isOpen)
            {
                _pictureObj.SetActive(true);
                _pictureIcon.overrideSprite = sprite;
            }
            else
            {
                ToastBox.ShowText(KLocalization.GetLocalString(54188));
            }
        }

        private void OnPictureImg(int id)
        {
            _curCatId = id;
            OpenWindow<MessageBox>(new MessageBox.Data()
            {
                onConfirm = CompCat,
                onCancel = OnCancel,
                content = KLocalization.GetLocalString(54192)
            });
        }

        private void CompCat()
        {
            GameApp.Instance.GameServer.ReqCatUnlock(_spaceOtherDataVO.mPlayerId, _curCatId);
        }

        private void OnCancel()
        {

        }

        private void OnClosePicture()
        {
            _pictureObj.SetActive(false);
        }
    }
}
