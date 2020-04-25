// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "CropWindow" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using Game.Build;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class CropItem : KUIItem
    {
        #region Field

        /// <summary>
        /// 
        /// </summary>
        private Text _titleText;
        /// <summary>
        /// 
        /// </summary>
        private Text _costText;
        /// <summary>
        /// 
        /// </summary>
        private Text _timeText;
        /// <summary>
        /// 
        /// </summary>
        private Text _produceText;
        /// <summary>
        /// 
        /// </summary>
        private Image _iconImage;
        /// <summary>
        /// 
        /// </summary>
        private Button _buyButton;
        /// <summary>
        /// 
        /// </summary>
        private Text _unlockHint;

        /// <summary>
        /// 
        /// </summary>
        private bool _needUpdate;
        /// <summary>
        /// 
        /// </summary>
        private KItemCrop _shopCrop;

        #endregion

        #region Method

        public void Show(KItemCrop shopCrop)
        {
            _shopCrop = shopCrop;
            _needUpdate = true;
        }

        public Sprite GetSprite(string name)
        {
            return KIconManager.Instance.GetItemIcon(name);
        }

        private void OnBuyBtnClick()
        {
            var farms = BuildingManager.Instance.GetEntities<BuildingFarmland>();
            if (farms != null && farms.Count > 0)
            {
                foreach (var farm in farms)
                {
                    if (farm.canSow)
                    {
                        farm.Sowing(_shopCrop);
                        break;
                    }
                }
            }
        }

        #endregion

        #region Unity  

        // Use this for initialization
        private void Start()
        {
            _titleText = Find<Text>("Item/Title");
            _iconImage = Find<Image>("Item/Icon");
            _buyButton = Find<Button>("Item/Buy");
            _costText = Find<Text>("Item/Buy/Cost");
            _timeText = Find<Text>("Item/Data/Time");
            _produceText = Find<Text>("Item/Data/Produce");
            _unlockHint = Find<Text>("Item/Unlock");

            _buyButton.onClick.AddListener(this.OnBuyBtnClick);
        }

        // Update is called once per frame
        private void Update()
        {
            if (_needUpdate && _shopCrop != null)
            {
                var second = _shopCrop.produceTimes[_shopCrop.produceTimes.Length - 1];
                var ts = K.Extension.TimeExtension.ToTimeString(second);

                _titleText.text = _shopCrop.itemName;
                _costText.text = _shopCrop.Cost.ToString("N0");
                _timeText.text = ts;
                _produceText.text = _shopCrop.Exe.ToString("N0");

                var sprite = GetSprite(_shopCrop.iconName);

                if (_shopCrop.unlockGrade <= PlayerDataModel.Instance.mPlayerData.mLevel)
                {
                    _iconImage.overrideSprite = sprite;
                    _buyButton.gameObject.SetActive(true);
                    _unlockHint.gameObject.SetActive(false);
                }
                else
                {
                    _buyButton.gameObject.SetActive(false);
                    _unlockHint.text = string.Format("第{0}级解锁", _shopCrop.unlockGrade);
                    _unlockHint.gameObject.SetActive(true);
                }

                _needUpdate = false;
            }
        }

        #endregion
    }
}
