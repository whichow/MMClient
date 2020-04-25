/** 
 *FileName:     ChooseCatItem.cs 
 *Author:       LiMuChen 
 *Version:      1.0 
 *UnityVersion：5.6.3f1
 *Date:         2017-10-23 
 *Description:    
 *History: 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    using K.Extension;
    using UnityEngine.EventSystems;

    public class ChooseCatItem : KUIItem, IPointerClickHandler
    {
        #region Field
        private Image catIcon;
        private KUIImage _imagFlag;
        private KUIImage _imagFram;
        private Text _textGrad;
        private CatDataVO _catItem;
        private Transform _transOk;
        private Text _textDiscovery;
        private Transform _tranBlack;
        private Text _textState;
        private Text _textCatName;
        #endregion

        #region Method

        public void ShowCat(CatDataVO cat, int type)
        {
            _catItem = cat;
            ShowCatIcon(catIcon, cat.mCatXDM.GetIconSprite());
            ShowRarity(_imagFlag, _imagFram, cat.mCatXDM.Rarity);
            _textGrad.text = "1";
            if (type == 1)
            {
                _textDiscovery.text = "产金能力：" + cat.mCatInfo.CoinAbility;
            }
            else if (type == 2)
            {
                _textDiscovery.text = "探索能力：" + cat.mCatInfo.ExploreAbility;
            }
            else
            {
                _textDiscovery.text = "消除能力：" + cat.mCatInfo.MatchAbility;
            }
            if (string.IsNullOrEmpty(cat.mNickName))
            {
                _textCatName.text = cat.mName;
            }
            else
            {
                _textCatName.text = cat.mNickName;
            }
    
            _transOk.gameObject.SetActive(false);
            RefreshCatState(cat.mCatInfo.State);
        }


        public void ShowCatIcon(Image icon, Sprite catIcon)
        {
            icon.overrideSprite = catIcon;
        }

        private void RefreshCatState(int state)
        {
            switch (state)
            {
                case 0:
                    _tranBlack.gameObject.SetActive(false);
                    break;
                case 1:
                    _tranBlack.gameObject.SetActive(true);
                    _textState.text = KLocalization.GetLocalString(54108); ;
                    break;
                case 2:
                    _tranBlack.gameObject.SetActive(true);
                    _textState.text = KLocalization.GetLocalString(54110); ;
                    break;
                case 3:
                    _tranBlack.gameObject.SetActive(true);
                    _textState.text = KLocalization.GetLocalString(54109);
                    break;
                default:
                    break;
            }
        }

        public void ShowRarity(KUIImage _specialFlagImage, KUIImage _specialFrameImage, int rarity)
        {
            if (rarity == 2)
            {
                _specialFlagImage.ShowSprite(1);
                _specialFrameImage.gameObject.SetActive(true);
                _specialFrameImage.ShowSprite(1);
            }
            else if (rarity == 3)
            {
                _specialFlagImage.ShowSprite(2);
                _specialFrameImage.gameObject.SetActive(true);
                _specialFrameImage.ShowSprite(2);
            }
            else if (rarity == 4)
            {
                _specialFlagImage.ShowSprite(3);
                _specialFrameImage.gameObject.SetActive(true);
                _specialFrameImage.ShowSprite(3);
            }
            else
            {
                _specialFlagImage.ShowSprite(0);
                _specialFrameImage.gameObject.SetActive(false);
                _specialFrameImage.ShowSprite(0);
            }
        }

        #endregion

        #region Action

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (_catItem.mCatInfo.State == 0)
            {
                _transOk.gameObject.SetActive(true);
                KUIWindow.GetWindow<ChooseCatWindow>()._cat = _catItem;
                KUIWindow.GetWindow<ChooseCatWindow>().OnConfirmBtnClick();
            }
            else
            {
                Debug.Log("当前猫正在其他任务中....");
            }

        }



        #endregion

        #region Unity

        private void Awake()
        {
            catIcon = Find<Image>("Item/CardSmall/Cat");
            _imagFlag = Find<KUIImage>("Item/CardSmall/Level");
            _imagFram = Find<KUIImage>("Item/CardSmall/Light");
            _textGrad = Find<Text>("Item/CardSmall/Level/Text");
            _transOk = Find<Transform>("Item/OK");
            _textDiscovery = Find<Text>("Item/Hide/Shu/Text");
            _tranBlack = Find<Transform>("Item/Black");
            _textState = Find<Text>("Item/Black/Item1/Text");
            _textCatName = Find<Text>("Item/Hide/Name");
        }
        void Update()
        {

        }

        #endregion
    }
}

