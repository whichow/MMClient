// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "CatBagItem" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using Game.DataModel;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI
{
    public class CatHandBookWindowItem : KUIItem, IPointerClickHandler
    {
        #region Field
        private Text _txt_quality;
        private KUIImage _img_icon;
        private Text _txt_infoName;
        private Text _txt_num;
        private KUIImage _img_quality;
        private Text _txt_shadowName;
        private GameObject _go_openPanel;
        private GameObject _go_unlockedPanel;

        private CatXDM _catData;
        private KHandBookManager.HandBookConfiger _hbData;
        [HideInInspector]
        public int _mailID { get; private set; }
        #endregion

        #region Unity
        private void Awake()
        {
            _txt_quality = Find<Text>("CardSmall/Level/Text");
            _img_quality = Find<KUIImage>("quality");
            _img_icon = Find<KUIImage>("CardSmall/img_icon");
            _txt_infoName = Find<Text>("Open/Name");
            _txt_num = Find<Text>("Open/Shu/Text");

            _txt_shadowName = Find<Text>("Unlocked/Name");
            _go_openPanel = Find<Transform>("Open").gameObject;
            _go_unlockedPanel = Find<Transform>("Unlocked").gameObject;
        }
        #endregion

        #region Method
        public void ShowCat(KHandBookManager.HandBookConfiger[] kcats, int index)
        {
            SetSelect(false);
            if (kcats == null || kcats.Length == 0)
            {
                return;
            }
            _hbData = kcats[index];
            _catData = XTable.CatXTable.GetByID(_hbData.id);
            SetQuality();
            _txt_infoName.text = _catData.Name;
            _txt_shadowName.text = _catData.Name;
            _img_icon.overrideSprite = KIconManager.Instance.GetCatIcon(_catData.Icon);
            ShowGetBtn(Convert.ToBoolean(_hbData.learned));
        }
        #endregion

        #region Interface
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            UI.ItemGetPath.ShowGetPath(_catData.ID, typeof(CatXDM));
        }
        //显示暗影和按键
        public void ShowGetBtn(bool showbtn)
        {
            _go_openPanel.SetActive(showbtn);
            _go_unlockedPanel.SetActive(!showbtn);
            if (showbtn)
            {
                _txt_num.text = "数量：" + KCatManager.Instance.GetCatCountByShopID(_hbData.id).ToString();
            }
        }
        private void SetQuality()
        {
            switch (_catData.Rarity)
            {
                case 1:
                    _txt_quality.text = "N";
                    _img_quality.ShowSprite(0);
                    _img_quality.gameObject.SetActive(false);
                    break;
                case 2:
                    _txt_quality.text = "S";
                    _img_quality.ShowSprite(0);
                    _img_quality.gameObject.SetActive(true);
                    break;
                case 3:
                    _txt_quality.text = "SR";
                    _img_quality.ShowSprite(1);
                    _img_quality.gameObject.SetActive(true);
                    break;
                case 4:
                    _txt_quality.text = "SSR";
                    _img_quality.ShowSprite(2);
                    _img_quality.gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }
        public void SetSelect(bool visable)
        {
            _go_openPanel.SetActive(visable);
            _go_unlockedPanel.SetActive(!visable);
        }
        private void RefreshRight()
        {

        }
        #endregion
    }
}

