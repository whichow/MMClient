using Msg.ClientMessage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    partial class SpaceWindow
    {
        private PlayerDataVO _playerDataVO;
        private Button _close;
        private Image _head;
        private Text _level;
        private Text _name;
        private Button _changeNameBtn;
        private Text _playerId;
        private Button _buttonCopyId;
        private Text _zanNum;
        private Text _followNum;
        private Text _charmValNum;
        private Button _changeHeadBtn;
        private Button _formBtn;
        private Button _settingBtn;
        
        private UIList _pictureList;
        private bool _isPicture;
        private Text _pictureName;
        private Button _pictureBtn;
        private GameObject _pictureObj;
        private Image _pictureIcon;
        private Button _pictureClose;

        private GameObject _formObj;
        private Button _formClose;
        private Button _girlBtn;
        private Button _manBtn;

        private void InitView()
        {
            _close = Find<Button>("Left/Close");
            _close.onClick.AddListener(OnBackBtnClick);
            _head = Find<Image>("Left/ImageHead");
            _level = Find<Text>("Left/ImageHead/Image/Text");
            _name = Find<Text>("Left/NiC/ImageNameBack/name");
            _changeNameBtn = Find<Button>("Left/NiC/ChangeNameButton");
            _changeNameBtn.onClick.AddListener(OnChangeName);
            _playerId = Find<Text>("Left/ID/IdNum");
            _buttonCopyId = Find<Button>("Left/ID/ChangeNameButton");
            _buttonCopyId.onClick.AddListener(OnCopyIdBtnClick);
            _zanNum = Find<Text>("Left/PraiseNumIcon/praiseNum/Num");
            _followNum = Find<Text>("Left/FollowNumIcon/FollowNum/Num");
            _charmValNum = Find<Text>("Left/StarNumIcon/starNum/Num");
            _changeHeadBtn = Find<Button>("Left/ChangeHead");
            _changeHeadBtn.onClick.AddListener(OnChangeHead);
            _formBtn = Find<Button>("Left/FormBtn");
            _formBtn.onClick.AddListener(OnForm);
            _settingBtn = Find<Button>("Left/SettingBtn");
            _settingBtn.onClick.AddListener(OnSetting);

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

            _formObj = Find("Form");
            _formClose = Find<Button>("Form/Panel/Close");
            _formClose.onClick.AddListener(OnFormClose);
            _girlBtn = Find<Button>("Form/Panel/ButtonGroup/Girl");
            _girlBtn.onClick.AddListener(OnGirl);
            _manBtn = Find<Button>("Form/Panel/ButtonGroup/Man");
            _manBtn.onClick.AddListener(OnMan);
        }

        public override void AddEvents()
        {
            base.AddEvents();
            PlayerDataModel.Instance.AddEvent(PlayerEvent.ChangeName, ChangeName);
            PlayerDataModel.Instance.AddEvent(PlayerEvent.ChangeHead, ChangeHead);
            SpaceDataModel.Instance.AddEvent(SpaceEvent.FashionData, OnFashionData);
            SpaceDataModel.Instance.AddEvent(SpaceEvent.SetGender, OnSetGender);
        }

        public override void RemoveEvents()
        {
            base.RemoveEvents();
            PlayerDataModel.Instance.RemoveEvent(PlayerEvent.ChangeName, ChangeName);
            PlayerDataModel.Instance.RemoveEvent(PlayerEvent.ChangeHead, ChangeHead);
            SpaceDataModel.Instance.RemoveEvent(SpaceEvent.FashionData, OnFashionData);
            SpaceDataModel.Instance.RemoveEvent(SpaceEvent.SetGender, OnSetGender);
        }

        private void PointerHandler(UIListItem item, int index)
        {
            int value;
            if (item.dataSource == null)
                value = 0;
            else
                value = int.Parse(item.dataSource.ToString());
        }

        private void RenderHandler(UIListItem item, int index)
        {
            bool isOpen = false;
            item.gameObject.SetActive(true);
            int value;
            if (item.dataSource == null)
                value = 0;
            else
                value = int.Parse(item.dataSource.ToString());
            Image catIcon = item.GetComp<Image>("Cat/Icon");
            Image pictureIcon = item.GetComp<Image>("Picture/Icon");
            item.GetComp<Button>("Cat").onClick.RemoveAllListeners();
            item.GetComp<Button>("Cat").onClick.AddListener(() => { OnCat(value, index); });
            item.GetComp<Button>("Picture").onClick.RemoveAllListeners();
            item.GetComp<Button>("Picture").onClick.AddListener(() => { OnPicture(pictureIcon.overrideSprite, isOpen); });
            item.GetComp<Button>("Bg").onClick.RemoveAllListeners();
            item.GetComp<Button>("Bg").onClick.AddListener(() => { OnBg(); });
            item.GetGameObject("Cat").SetActive(value > 0 && _isPicture);
            item.GetGameObject("Picture").SetActive(value > 0 && !_isPicture);
            item.GetGameObject("Bg").SetActive(value == 0);
            CatDataVO cat = CatDataModel.Instance.GetCatDataVOById(value);
            if (cat != null)
                catIcon.overrideSprite = KIconManager.Instance.GetCatIcon(cat.mCatXDM.Icon);
            if (value > 0 && !_isPicture)
            {
                string fileName = string.Format("{0}-{1}.jpg", PlayerDataModel.Instance.mPlayerData.mPlayerID, value);
                ImageUtils.LoadNetImage(fileName, pictureIcon, (texture2D)=> {
                    if (texture2D != null)
                        isOpen = true;
                });
            }
            else
            {
                pictureIcon.overrideSprite = null;
            }
        }

        private void ChangeName()
        {
            _name.text = _playerDataVO.mName;
        }

        private void ChangeHead()
        {
            HeadIconUtils.SetHeadIcon(_playerDataVO.mHead, PlayerDataModel.Instance.mPlayerData.mPlayerID, _head);
        }

        private void RefreshView()
        {
            _formObj.SetActive(false);
            _isPicture = true;
            _playerDataVO = PlayerDataModel.Instance.mPlayerData;
            HeadIconUtils.SetHeadIcon(_playerDataVO.mHead, PlayerDataModel.Instance.mPlayerData.mPlayerID, _head);
            _level.text = _playerDataVO.mLevel.ToString();
            _name.text = _playerDataVO.mName;
            _playerId.text = _playerDataVO.mPlayerID.ToString();
            _zanNum.text = _playerDataVO.mZan.ToString();
            _charmValNum.text = _playerDataVO.mCharmVal.ToString();
            _followNum.text = SpaceDataModel.Instance.mFocusNum.ToString();
            SpacePicture();
        }

        private void SpacePicture()
        {
            if (_isPicture)
                _pictureName.text = KLocalization.GetLocalString(54171);
            else
                _pictureName.text = KLocalization.GetLocalString(54172);
            SpaceDataModel.Instance.mLstCatId.Sort(OnSort);
            _pictureList.DataArray = SpaceDataModel.Instance.mLstCatId;
        }

        private int OnSort(int index1,int index2)
        {
            CatDataVO cat1 = CatDataModel.Instance.GetCatDataVOById(index1);
            CatDataVO cat2 = CatDataModel.Instance.GetCatDataVOById(index2);
            return cat1.mCatInfo.CatCfgId > cat2.mCatInfo.CatCfgId ? -1 : 1;
        }

        private void OnPictureBtn()
        {
            _isPicture = !_isPicture;
            SpacePicture();
        }

        private void OnCat(int catId,int index)
        {
            List<CatDataVO> catDataVOs = new List<CatDataVO>();
            catDataVOs = CatDataModel.Instance.GetLstCatDataById(SpaceDataModel.Instance.mLstCatId);
            CatDatas catDatas = new CatDatas();
            catDatas.mCatDataVOs = catDataVOs;
            catDatas.mIndex = index;
            catDatas.mOpenType = CatOpenType.Space;
            KUIWindow.OpenWindow<CatInfoWindow>(catDatas);
        }

        private void OnPicture(Sprite sprite, bool isOpen)
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

        private void OnBg()
        {
            KUIWindow.OpenWindow<CatWindow>(CatOpenType.Space);
        }

        private void OnClosePicture()
        {
            _pictureObj.SetActive(false);
        }
    }
}
