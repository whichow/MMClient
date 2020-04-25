using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public partial class PhotoShopPickCardLowItem : KUIItem
    {
        private Image _iconImage;
        private KUIImage _imageFlag;
        private KUIImage _imageFram;
        private GameObject _goCat;
        private GameObject _goBlackCat;
        private KUIImage[] _starImages;
        private KUIImage _colorImage;
        private Text _textRaity;
        private SkeletonAnimation _fxItem;
        private PhotoShopPickCardLowWindow.PickCardData _item;

        public void ShowCatTextur(PhotoShopPickCardLowWindow.PickCardData item)
        {
            if (_fxItem)
            {
                _fxItem.GetComponent<Renderer>().sortingOrder = _textRaity.canvas.sortingOrder + 1;
                _fxItem.loop = false;
                _fxItem.AnimationName = "start";
            }

            _goBlackCat.SetActive(true);
            _goCat.SetActive(false);

            if (item == null) return;

            _item = item;
            ShowIcon();
            ShowRarity();
        }

        public void ShowIcon()
        {
            if (_item.Type == PhotoShopPickCardLowWindow.EPickCardType.Cat)
            {
                _iconImage.overrideSprite = KIconManager.Instance.GetCatIcon(_item.Icon);
            }
            else if (_item.Type == PhotoShopPickCardLowWindow.EPickCardType.Building)
            {
                _iconImage.overrideSprite = KIconManager.Instance.GetBuildingIcon(_item.Icon);
            }
            else
            {
                _iconImage.overrideSprite = KIconManager.Instance.GetItemIcon(_item.Icon);
            }
        }

        public void ShowRarity()
        {
            if (_item.Rarity == 2)
            {
                _textRaity.text = "R";
                _imageFlag.ShowSprite(1);
                _imageFram.gameObject.SetActive(true);
                _imageFram.ShowSprite(1);
            }
            else if (_item.Rarity == 3)
            {
                _textRaity.text = "SR";
                _imageFlag.ShowSprite(2);
                _imageFram.gameObject.SetActive(true);
                _imageFram.ShowSprite(2);
            }
            else if (_item.Rarity == 4)
            {
                _textRaity.text = "SSR";
                _imageFlag.ShowSprite(3);
                _imageFram.gameObject.SetActive(true);
                _imageFram.ShowSprite(3);
            }
            else
            {
                _textRaity.text = "N";
                _imageFlag.ShowSprite(0);
                _imageFram.gameObject.SetActive(false);
                _imageFram.ShowSprite(0);
            }
        }

        public void StartRewardAnimontor()
        {
            if (_fxItem && _item != null)
            {
                if (_item.Rarity == 1)
                {
                    _fxItem.loop = false;
                    _fxItem.AnimationName = "animation";
                }
                else
                {
                    _fxItem.loop = false;
                    _fxItem.AnimationName = "animation2";
                }
            }
            StartCoroutine(RefreshReward());
        }

        public void RefreshReward(bool isAcitive)
        {
            _goBlackCat.SetActive(!isAcitive);
            _goCat.SetActive(isAcitive);
        }

        private IEnumerator RefreshReward()
        {
            yield return new WaitForSeconds(1.1f);
            RefreshReward(true);
            KUIWindow.GetWindow<PhotoShopPickCardLowWindow>().RefreshBtn();
        }

        public bool isRefreshReward()
        {
            bool isOpenReardfalse;
            if (_goBlackCat.activeSelf == false && _goCat.activeSelf == true)
            {
                isOpenReardfalse = true;
            }
            else
            {
                isOpenReardfalse = false;
            }
            return isOpenReardfalse;
        }

        public void Awake()
        {
            _fxItem = Find<SkeletonAnimation>("Item/CardBack/Fx_Card_01");
            _iconImage = Find<Image>("Item/CardBack/Receveri/HeadPic");
            _imageFlag = Find<KUIImage>("Item/CardBack/Receveri/Image");
            _imageFram = Find<KUIImage>("Item/CardBack/Receveri/CardBackIll");
            _textRaity = Find<Text>("Item/CardBack/Receveri/Level");
            _goCat = gameObject.transform.Find("Item/CardBack/Receveri").gameObject;
            _goBlackCat = gameObject.transform.Find("Item/CardBack/Back02").gameObject;
        }

    }
}
