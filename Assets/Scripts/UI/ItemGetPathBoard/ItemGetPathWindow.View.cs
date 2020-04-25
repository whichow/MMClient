// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "InfomationBox.View" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using Game.DataModel;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    partial class ItemGetPath
    {
        #region Field

        private Button _blackback;
        private Button _btn_close;

        private KUIImage _img_icon;
        private Text _txt_name;
        private Text _txt_num;
        private Text _txt_quality;
        private GameObject _go_learned;
        private KUIImage _img_quality;
        private Text _txt_titleOne;
        private Text _txt_titleTwo;
        private Text _txt_titleThree;
        private Text _txt_titleFour;
        private Text _txt_attributeOne;
        private Text _txt_attributeTwo;
        private Text _txt_attributeThree;
        private Text _txt_attributeFour;

        private KUIItemPool _layoutElementPool;
        #endregion

        #region Method

        public void InitView()
        {
            _blackback = Find<Button>("blackback");
            _blackback.onClick.AddListener(this.UIClose);
            _btn_close = Find<Button>("Panel/close");
            _btn_close.onClick.AddListener(this.UIClose);

            _img_icon = Find<KUIImage>("Panel/Left/iconBack/img_icon");
            _txt_name = Find<Text>("Panel/Left/txt_name");
            _txt_num = Find<Text>("Panel/Left/numBack/Text");
            _txt_quality = Find<Text>("Panel/Left/iconBack/qualityBack/Text");
            _go_learned = Find<Transform>("Panel/Left/img_unlocked").gameObject;

            _txt_titleOne = Find<Text>("Panel/Left/txt_title_1");
            _txt_titleTwo = Find<Text>("Panel/Left/txt_title_2");
            _txt_titleThree = Find<Text>("Panel/Left/txt_title_3");
            _txt_titleFour = Find<Text>("Panel/Left/txt_title_4");

            _txt_attributeOne = Find<Text>("Panel/Left/txt_title_1/txt_attribute_1");
            _txt_attributeTwo = Find<Text>("Panel/Left/txt_title_2/txt_attribute_2");
            _txt_attributeThree = Find<Text>("Panel/Left/txt_title_3/txt_attribute_3");
            _txt_attributeFour = Find<Text>("Panel/Left/txt_title_4/txt_attribute_4");

            _img_quality = Find<KUIImage>("Panel/Left/quality");
            _layoutElementPool = Find<KUIItemPool>("Panel/Right/Scroll View");
            if (_layoutElementPool && _layoutElementPool.elementTemplate)
            {
                _layoutElementPool.elementTemplate.gameObject.AddComponent<ItemGetPathWindowItem>();
            }
        }

        public void RefreshView()
        {
            _img_icon.overrideSprite = accurateData._spt_icon;
            _txt_name.text = accurateData.itemName;
            _txt_num.text = "数量：" + accurateData.itemNum;
            SetQuality();
            RefreshDescription();
            _go_learned.SetActive(!Convert.ToBoolean(accurateData.learned));
            ShowGetPaths();
        }

        private void SetQuality()
        {
            switch (accurateData.itemQuality)
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

        private void RefreshDescription() {
            _txt_titleOne.gameObject.SetActive(accurateData._str_className == typeof(CatXDM).FullName || accurateData._str_className == typeof(KItemBuilding).FullName || accurateData._str_className == typeof(KItemFormula).FullName);
            _txt_titleTwo.gameObject.SetActive(accurateData._str_className == typeof(CatXDM).FullName || accurateData._str_className == typeof(KItemBuilding).FullName || accurateData._str_className == typeof(KItemFormula).FullName);
            _txt_titleThree.gameObject.SetActive(accurateData._str_className == typeof(CatXDM).FullName || accurateData._str_className == typeof(KItemFormula).FullName);
            _txt_titleFour.gameObject.SetActive(accurateData._str_className == typeof(KItemThing).FullName);

            if (accurateData._str_className == typeof(CatXDM).FullName)
            {
                _txt_titleOne.text = accurateData._str_titleOne;
                _txt_titleTwo.text = accurateData._str_titleTwo;
                _txt_titleThree.text = accurateData._str_titleThree;
                _txt_attributeOne.text = accurateData._str_attributeOne;
                _txt_attributeTwo.text = accurateData._str_attributeTwo;
                _txt_attributeThree.text = accurateData._str_attributeThree;
            }
            else if (accurateData._str_className == typeof(KItemBuilding).FullName)
            {
                _txt_titleOne.text = accurateData._str_titleOne;
                _txt_titleTwo.text = accurateData._str_titleTwo;
                _txt_attributeOne.text = accurateData._str_attributeOne;
                _txt_attributeTwo.text = accurateData._str_attributeTwo;
            }
            else if (accurateData._str_className == typeof(KItemFormula).FullName)
            {
                _txt_titleOne.text = accurateData._str_titleOne;
                _txt_titleTwo.text = accurateData._str_titleTwo;
                _txt_titleThree.text = accurateData._str_titleThree;
                _txt_attributeOne.text = accurateData._str_attributeOne;
                _txt_attributeTwo.text = accurateData._str_attributeTwo;
                _txt_attributeThree.text = accurateData._str_attributeThree;

            }
            else if (accurateData._str_className == typeof(KItemThing).FullName)
            {
                _txt_titleFour.text = accurateData._str_titleFour;
                _txt_attributeFour.text = accurateData._str_attributeFour;
            }
        }

        private void ShowGetPaths() {

            StartCoroutine(FillElements());
        }

        private IEnumerator FillElements()
        {
            _layoutElementPool.Clear();
            var pathdata = GetShowGetPathsData();
            for (int i = 0; i < pathdata.Length; i++)
            {
                var element = _layoutElementPool.SpawnElement();
                var pathItem = element.GetComponent<ItemGetPathWindowItem>();
                pathItem.ShowGetPath(pathdata, i);
            }
            yield return null;
        }

        #endregion
    }
}
