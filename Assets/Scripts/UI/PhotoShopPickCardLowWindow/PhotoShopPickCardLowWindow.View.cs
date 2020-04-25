using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public partial class PhotoShopPickCardLowWindow
    {
        private Button _buttonConfirm;
        private Button _buttonAgain;

        private Text _textBlue;
        private Text _blueBackText;
        private Text _purpleBackText;
        private Text _diamondBackText;
        private KUIItemPool _layoutElementPool;
        private PhotoShopPickCardLowItem[] items;
        private List<PickCardData> itemData;

        public void InitView()
        {
            _buttonConfirm = Find<Button>("Back/ButtonConfirm");
            _buttonAgain = Find<Button>("Back/ButtonAgain");
            _textBlue = Find<Text>("Back/Image/BlueNum");

            _blueBackText = Find<Text>("Back/BlueBack/Text");
            _purpleBackText = Find<Text>("Back/PurpleBack/Text");
            _diamondBackText = Find<Text>("Back/DiamondBack/Text");
            _layoutElementPool = Find<KUIItemPool>("Back/Scroll View");
            if (_layoutElementPool && _layoutElementPool.elementTemplate)
            {
                _layoutElementPool.elementTemplate.gameObject.AddComponent<PhotoShopPickCardLowItem>();
            }
        }

        public void RefreshView()
        {
            _textBlue.text = GetBlueBlackNum();
            _blueBackText.text = GetBlueBlack();
            _purpleBackText.text = GetPurpoleBlack();
            _diamondBackText.text = GetDiamBlack();
            StartCoroutine(FillElements());
            RefreshBtn();
        }

        public void RefreshBtn()
        {
            var parents = Find<Transform>("Back/Scroll View/Viewport/Content");
            items = parents.GetComponentsInChildren<PhotoShopPickCardLowItem>();
            int num = 0;
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].isRefreshReward())
                {
                    num++;
                }
            }
            if (num == items.Length)
            {
                _buttonAgain.onClick.RemoveAllListeners();
                _buttonConfirm.onClick.RemoveAllListeners();
                _buttonAgain.onClick.AddListener(this.OnAgainBtnClick);
                _buttonConfirm.onClick.AddListener(this.OnCloseBtnClick);
                _buttonAgain.GetComponent<Image>().material = null;
                _buttonConfirm.GetComponent<Image>().material = null;
            }
            else
            {
                _buttonAgain.onClick.RemoveAllListeners();
                _buttonConfirm.onClick.RemoveAllListeners();
                _buttonAgain.onClick.AddListener(this.OnBtnNoClick);
                _buttonConfirm.onClick.AddListener(this.OnBtnNoClick);
                _buttonAgain.GetComponent<Image>().material = Resources.Load<Material>("Materials/UIGray");
                _buttonConfirm.GetComponent<Image>().material = Resources.Load<Material>("Materials/UIGray");
            }
        }

        private IEnumerator FillElements()
        {
            _layoutElementPool.Clear();
            if (itemData == null)
            {
                itemData = data as List<PickCardData>;
            }
            for (int i = 0; i < itemData.Count; i++)
            {
                var elementHead = _layoutElementPool.SpawnElement();
                var catItemHead = elementHead.GetComponent<PhotoShopPickCardLowItem>();
                catItemHead.ShowCatTextur(itemData[i]);
            }
            var grad = Find<Transform>("Back/Scroll View/Viewport/Content");
            var photoShopLowItem = grad.GetComponentsInChildren<PhotoShopPickCardLowItem>();
            for (int i = 0; i < photoShopLowItem.Length; i++)
            {
                photoShopLowItem[i].StartRewardAnimontor();
                yield return new WaitForSeconds(0.4f);
            }
            yield return null;
        }

    }
}
