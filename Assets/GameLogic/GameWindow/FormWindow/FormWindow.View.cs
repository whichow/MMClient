using Game.DataModel;
using Msg.ClientMessage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    partial class FormWindow
    {
        private UIList _formList;
        private Button _close;
        private Button _fashionReset;
        private Button _fashionSave;
        private Button _fashionShop;
        private RawImage _rawImage;
        private GameObject _textObj;

        private UnitRenderTexture _unitRenderTexture;
        private PlayerAvatar _avatar;
        private Dictionary<int, ItemXDM> _curForm;
        private Button _avatarBtn;

        private void InitView()
        {
            _formList = Find<UIList>("Right/List");
            _formList.SetRenderHandler(RenderHandler);
            _formList.SetPointerHandler(PointerHandler);

            _rawImage = Find<RawImage>("Left/RawImage");
            _avatarBtn = Find<Button>("Left/RawImage");
            _avatarBtn.onClick.AddListener(OnPlayAnimation);

            _close = Find<Button>("Close");
            _close.onClick.AddListener(OnBackBtnClick);
            _fashionReset = Find<Button>("Left/FashionReset");
            _fashionReset.onClick.AddListener(OnFashionReset);
            _fashionSave = Find<Button>("Left/FashionSave");
            _fashionSave.onClick.AddListener(OnFashionSave);
            _fashionShop = Find<Button>("Left/FashionShop");
            _fashionShop.onClick.AddListener(OnFashionShop);
            _textObj = Find("Right/Text");
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
            ItemInfo vo = item.dataSource as ItemInfo;
            if (vo == null)
                return;
            ItemXDM itemXDM = XTable.ItemXTable.GetByID(vo.ItemCfgId);
            item.GetComp<TweenScl>("ItemObj").PlayBack();
            if (!IsFashionById(vo.ItemCfgId))
            {
                if (itemXDM.RoleType == SpaceDataModel.Instance.mGender)
                {
                    if (_curForm.ContainsKey(itemXDM.EquipType))
                        _curForm[itemXDM.EquipType] = itemXDM;
                    else
                        _curForm.Add(itemXDM.EquipType, itemXDM);
                    _avatar.ChangePart(itemXDM.Model);
                    RefreshView(_formType);
                }
                else
                {
                    ToastBox.ShowText(KLocalization.GetLocalString(53097));
                }
            }
            else
            {
                _avatar.SetDefaultPart((EAvatarPart)itemXDM.EquipType);
                _curForm.Remove(itemXDM.EquipType);
                RefreshView(_formType);
                //µ¥¼þÍÑ×°
            }
        }

        private void RenderHandler(UIListItem item, int index)
        {
            ItemInfo vo = item.dataSource as ItemInfo;
            if (vo == null)
                return;
            item.GetComp<Image>("ItemObj/Icon").overrideSprite = KIconManager.Instance.GetItemIcon(vo.ItemCfgId);
            item.GetGameObject("ItemObj/Img").SetActive(IsFashionById(vo.ItemCfgId));
        }

        private bool IsFashionById(int id)
        {
            foreach (var item in _curForm)
            {
                if (item.Value.ID == id)
                    return true;
            }
            return false;
        }

        private void RefreshView(int type)
        {
            _formList.DataArray = BagDataModel.Instance.GetFormByType(type);
            _textObj.SetActive(_formList.DataArray.Count == 0);
        }

        private void OnFashionReset()
        {
            _avatar.SetDefaultPart(EAvatarPart.none);
            _curForm.Clear();
            RefreshView(_formType);
        }

        private void OnFashionSave()
        {
            List<int> lstId = new List<int>();
            foreach (var item in _curForm)
                lstId.Add(item.Value.ID);
            GameApp.Instance.GameServer.ReqFashionSace(lstId);
        }

        private void OnFashionShop()
        {
            KUIWindow.OpenWindow<ShopWindow>(ShopIDConst.Attire);
        }
    }
}
