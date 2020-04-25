// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "CatInfoWindow.View" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using Framework;
using Game.DataModel;
using Msg.ClientMessage;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public partial class CatInfoWindow
    {
        private CatDatas _catDatas;
        private CatDataVO _catDataVO;
        private Text _combatValue;
        private Text _titleText;
        private Text _nameText;
        private Text _colorText;
        private Text _propertyValue1;
        private Text _propertyValue2;
        private Text _propertyValue3;
        private Image _skillImage;
        private KUIImage _rarityImage;
        private Transform _modelParent;
        private Button _changeNameBtn;
        private Button _skillBtn;
        private Button _moveBtn;
        private Button _leftBtn;
        private Button _rightBtn;
        private Button _exhibitionBtn;
        private Text _exhibitionText;
        private Image _picture;
        private GameObject _pictureObj;



        public void InitView()
        {
            _combatValue = Find<Text>("Crown/Text");
            _titleText = Find<Text>("Base/Title");
            _nameText = Find<Text>("Base/Name/Text");
            _colorText = Find<Text>("Base/Color");
            _propertyValue1 = Find<Text>("Property/P1/Value");
            _propertyValue2 = Find<Text>("Property/P2/Value");
            _propertyValue3 = Find<Text>("Property/P3/Value");
            _skillImage = Find<Image>("Text/Skill");
            _rarityImage = Find<KUIImage>("Rarity");
            _modelParent = Find<Transform>("Model");
            _changeNameBtn = Find<Button>("Base/Name/Change");
            _skillBtn = Find<Button>("Text/Skill");
            _moveBtn = Find<Button>("MoveBtn");
            _leftBtn = Find<Button>("Left");
            _rightBtn = Find<Button>("Right");
            _exhibitionBtn = Find<Button>("ExhibitionBtn");
            _exhibitionText = Find<Text>("ExhibitionBtn/Text");
            _picture = Find<Image>("Kuang/Img");
            _pictureObj = Find("Kuang");

            _exhibitionBtn.onClick.AddListener(OnExhibition);
            _changeNameBtn.onClick.AddListener(OnChangeNameClick);
            _skillBtn.onClick.AddListener(OnSkillBtnClick);
            _moveBtn.onClick.AddListener(OnMoveBtnClick);
            _leftBtn.onClick.AddListener(OnLeftBtnClick);
            _rightBtn.onClick.AddListener(OnRightBtnClick);
        }

        public override void AddEvents()
        {
            base.AddEvents();
            CatDataModel.Instance.AddEvent(CatEvent.CatNick, OnCatNick);
            SpaceDataModel.Instance.AddEvent(SpaceEvent.SetPicture, OnSetPicture);
        }

        public override void RemoveEvents()
        {
            base.RemoveEvents();
            CatDataModel.Instance.RemoveEvent(CatEvent.CatNick, OnCatNick);
            SpaceDataModel.Instance.RemoveEvent(SpaceEvent.SetPicture, OnSetPicture);
        }

        private void OnSetPicture()
        {
            if (SpaceDataModel.Instance.OnIsExhibition(_catDataVO.mCatInfo.Id))
                _exhibitionText.text = KLocalization.GetLocalString(54165);
            else
                _exhibitionText.text = KLocalization.GetLocalString(54164);
        }

        private void OnExhibition()
        {
            if (SpaceDataModel.Instance.mLstCatId.Count >= 9 && !SpaceDataModel.Instance.OnIsExhibition(_catDataVO.mCatInfo.Id))
            {
                ToastBox.ShowText(KLocalization.GetLocalString(54166));
            }
            else
            {
                if (SpaceDataModel.Instance.OnIsExhibition(_catDataVO.mCatInfo.Id))
                    GameApp.Instance.GameServer.ReqSetSpacePicture(_catDataVO.mCatInfo.Id, true);
                else
                    GameApp.Instance.GameServer.ReqSetSpacePicture(_catDataVO.mCatInfo.Id, false);
            }
        }

        private void OnCatNick()
        {
            _nameText.text = _catDataVO.mNickName;
        }

        public string GetCatColorText()
        {
            var sb = new System.Text.StringBuilder();
            var tmpColor = _catDataVO.mCatXDM.Color;
            if ((tmpColor & CatColorConst.Red) != 0)
                sb.Append("<color=#f93535>红色</color> ");
            if ((tmpColor & CatColorConst.Yellow) != 0)
                sb.Append("<color=#ffc823>黄色</color> ");
            if ((tmpColor & CatColorConst.Blue) != 0)
                sb.Append("<color=#4f4ffc>蓝色</color> ");
            if ((tmpColor & CatColorConst.Green) != 0)
                sb.Append("<color=#35dc61>绿色</color> ");
            if ((tmpColor & CatColorConst.Purple) != 0)
                sb.Append("<color=#d83eff>紫色</color> ");
            if ((tmpColor & CatColorConst.Brown) != 0)
                sb.Append("<color=#A0522D>棕色</color> ");
            return sb.ToString();
        }

        private void OnCatItem(CatDataVO vo)
        {
            _catDataVO = vo;
            OnSetPicture();

            string fileName = string.Format("{0}-{1}.jpg", PlayerDataModel.Instance.mPlayerData.mPlayerID, _catDataVO.mCatInfo.Id);
            ImageUtils.LoadNetImage(fileName, _picture, OnAction);

            RefreshMoveBtn();
            _rarityImage.ShowSprite(_catDataVO.mCatXDM.Rarity - 1);
            _combatValue.text = _catDataVO.mCatScore.ToString();
            _titleText.text = _catDataVO.mName;
            _nameText.text = _catDataVO.mNickName;
            _colorText.text = GetCatColorText();
            if (_catDataVO.mCatXDM.SkillId > 0)
                _skillImage.overrideSprite = KIconManager.Instance.GetSkillIcon(_catDataVO.mCatXDM.SkillId);
            _propertyValue1.text = _catDataVO.mCatInfo.CoinAbility.ToString();
            _propertyValue2.text = _catDataVO.mCatInfo.ExploreAbility.ToString();
            _propertyValue3.text = _catDataVO.mCatInfo.MatchAbility.ToString();
            ClearView();
            StartCoroutine(ShowCat());
            _leftBtn.gameObject.SetActive(_catDatas.mIndex > 0);
            _rightBtn.gameObject.SetActive(_catDatas.mIndex < _catDatas.mCatDataVOs.Count - 1);
        }

        private void OnAction(Texture2D texture2D)
        {
            _pictureObj.SetActive(texture2D != null);
        }

        public void RefreshView()
        {
            _catDatas = data as CatDatas;
            _exhibitionBtn.gameObject.SetActive(_catDatas.mOpenType == CatOpenType.Space);
            _moveBtn.gameObject.SetActive(_catDatas.mOpenType == CatOpenType.Normal);
            _changeNameBtn.gameObject.SetActive(_catDatas.mOpenType != CatOpenType.SpaceOther);
            OnCatItem(_catDatas.mCatDataVOs[_catDatas.mIndex]);
        }

        private void OnChangeNameClick()
        {
            ChangeNameData nameData = new ChangeNameData();
            nameData.data = _catDataVO;
            nameData.type = NmaeType.CatName;
            OpenWindow<ChangeName>(nameData);
        }

        private void OnSkillBtnClick()
        {
            if (_catDataVO.mCatXDM.SkillId > 0)
                OpenWindow<SkillInfoWindow>(_catDataVO.mCatXDM.SkillId);
        }

        private void OnMoveBtnClick()
        {
            KCattery.Instance.RemoveCat(0, _catDataVO.mCatInfo.Id, OnRemoveCallback);
        }

        private void OnRemoveCallback(int index,string str ,object value)
        {
            RefreshMoveBtn();
            ToastBox.ShowText(KLocalization.GetLocalString(54167));
        }

        private void RefreshMoveBtn()
        {
            if (_catDataVO.mCatInfo.State == CatStateConst.Cattery)
            {
                _moveBtn.interactable = true;
                ((KUIImage)_moveBtn.targetGraphic).ShowGray(false);
            }
            else
            {
                _moveBtn.interactable = false;
                ((KUIImage)_moveBtn.targetGraphic).ShowGray(true);
            }
        }


        private void OnLeftBtnClick()
        {
            if (_catDatas.mIndex > 0)
            {
                _catDatas.mIndex--;
                OnCatItem(_catDatas.mCatDataVOs[_catDatas.mIndex]);
            }
        }

        private void OnRightBtnClick()
        {
            if (_catDatas.mIndex < _catDatas.mCatDataVOs.Count - 1)
            {
                _catDatas.mIndex++;
                OnCatItem(_catDatas.mCatDataVOs[_catDatas.mIndex]);
            }
        }

        public IEnumerator ShowCat()
        {
            yield return null;
            CatUtils.GetUIModel(_catDataVO.mCatXDM.ID, (modelObj) =>
            {
                if (modelObj)
                    modelObj.transform.SetParent(_modelParent, false);
            });
        }

        public void ClearView()
        {
            if (_modelParent.childCount > 0)
                Object.Destroy(_modelParent.GetChild(0).gameObject);
        }
    }
}

