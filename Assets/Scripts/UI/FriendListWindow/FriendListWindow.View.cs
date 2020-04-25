
//using Msg.ClientMessage;
///** 
//*FileName:     FriendListWindow.View.cs 
//*Author:       LiMuChen 
//*Version:      1.0 
//*UnityVersion：5.6.3f1
//*Date:         2017-10-23 
//*Description:    
//*History: 
//*/
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//namespace Game.UI
//{
//    public partial class FriendListWindow
//    {
//        #region Field  


//        private KUIGrid _itemPool;

//        private Button _closeButton;

//        public KCat _cat;
//        private Transform _transCat;
//        private Button _chooseCatBtn;
//        private Button _btnFoster;
//        private FriendListItem _item;
//        private FriendInfo _firendData;
//        private GameObject _goNone;
//        #endregion

//        #region Method

//        public void InitView()
//        {
//            _closeButton = Find<Button>("Close");
//            _closeButton.onClick.AddListener(this.OnCloseBtnClick);
//            _transCat = Find<Transform>("ImageBack/ChooseCat");
//            _itemPool = Find<KUIGrid>("Scroll View");
//            _chooseCatBtn = Find<Button>("ImageBack/ChooseCat");
//            _chooseCatBtn.onClick.AddListener(OnChooseCatBtnClick);
//            _btnFoster = Find<Button>("ImageBack/Button");
//            _btnFoster.onClick.AddListener(OnFosterBtnClick);
//            if (_itemPool)
//            {
//                _itemPool.uiPool.itemTemplate.AddComponent<FriendListItem>();
//            }
//            _goNone = Find("ImageBack/None");

//        }
//        public override void OnDisable()
//        {
//            RefreshItem();
   
//        }




//        public void RefreshView()
//        {
//            //RefreshItem();
//            if (lstFrdData.Count>0)
//            {
//                _goNone.gameObject.SetActive(true);
//            }
//            else
//            {
//                _goNone.gameObject.SetActive(false);
//            }
//            _itemPool.ClearItems();
//            _itemPool.uiPool.SetItemDatas(lstFrdData);
//            _itemPool.RefillItems(0);
//            RefreshCats();
//        }

//        private void RefreshItem()
//        {
//            _cat = null;
//            if (_item!=null)
//            {
            
//                _item.RefreshBlack(false);
//                _item = null;
//                _firendData = null;
//            }
//        }
//        public void ChooseFriend(FriendInfo firendData,FriendListItem item)
//        {
//            if (_item != null)
//            {
//                _item.RefreshBlack(false);
//                _item = item;
//                _item.RefreshBlack(true);
//            }
//            else
//            {
//                _item = item;
//                _item.RefreshBlack(true);
//            }
//            if (firendData!=null)
//            {
//                _firendData = firendData;
//            }
      
//        }
//        private void RefreshCats()
//        {
//            KUIImage _imageFlag;
//            KUIImage _imageFram;
//            KUIImage[] _starImages;
//            KUIImage _imageColor;

//            if (_cat != null)
//            {
//                _transCat.Find("CardBig/Empty").gameObject.SetActive(false);
//                _transCat.Find("CardBig/Cat").GetComponent<Image>().overrideSprite = KIconManager.Instance.GetCatFull(_cat.photo);
//                _imageFlag = _transCat.Find("CardBig/Cat/Level").GetComponent<KUIImage>();
//                _imageFram = _transCat.Find("CardBig/Cat/Light").GetComponent<KUIImage>();
//                _transCat.Find("CardBig/Cat/Level/Text").GetComponent<Text>().text = _cat.grade.ToString();
//                _imageColor = _transCat.Find("CardBig/Cat/Fish").GetComponent<KUIImage>();
//                ShowColor(_imageColor, _cat.mainColor);
//                var fish = _transCat.Find("CardBig/Cat/Fish");
//                _starImages = new KUIImage[fish.childCount];
//                if (string.IsNullOrEmpty(_cat.nickName))
//                {
//                    _transCat.Find("CardBig/Cat/Name").GetComponent<Text>().text = _cat.name;
//                }
//                else
//                {
//                    _transCat.Find("CardBig/Cat/Name").GetComponent<Text>().text = _cat.nickName;
//                }
//                //_transCats[i].Find("CardBig/Cat/Item/Text").GetComponent<Text>().text = _cat.exploreAbility.ToString();
//                for (int j = 0; j < _starImages.Length; j++)
//                {
//                    _starImages[j] = fish.GetChild(j).GetComponent<KUIImage>();
//                }
//                ShowStar(_starImages, _cat.star);
//                ShowRarity(_imageFlag, _imageFram, _cat.rarity);

//                _transCat.Find("CardBig/Cat").gameObject.SetActive(true);
//                _transCat.Find("CardBig/Cat/Del").GetComponent<Button>().onClick.AddListener(RemoverCat);
//            }
//            else
//            {
//                _transCat.Find("CardBig/Cat").gameObject.SetActive(false);
//                _transCat.Find("CardBig/Empty").gameObject.SetActive(true);
//            }
//        }
//       private void RemoverCat()
//        {
//            _cat = null;
//            RefreshView();
//        }
//        public void ShowColor(KUIImage _colorImage, int color)
//        {
//            if (color == (int)KCat.Color.fRed)
//            {
//                _colorImage.ShowSprite(0);
//            }
//            else if (color == (int)KCat.Color.fYellow)
//            {
//                _colorImage.ShowSprite(1);
//            }
//            else if (color == (int)KCat.Color.fBlue)
//            {
//                _colorImage.ShowSprite(3);
//            }
//            else if (color == (int)KCat.Color.fGreen)
//            {
//                _colorImage.ShowSprite(2);
//            }
//            else if (color == (int)KCat.Color.fPurple)
//            {
//                _colorImage.ShowSprite(5);
//            }
//            else if (color == (int)KCat.Color.fBrown)
//            {
//                _colorImage.ShowSprite(4);
//            }
//        }
//        public void ShowRarity(KUIImage _specialFlagImage, KUIImage _specialFrameImage, int rarity)
//        {
//            if (rarity == 2)
//            {
//                _specialFlagImage.ShowSprite(1);
//                _specialFrameImage.gameObject.SetActive(true);
//                _specialFrameImage.ShowSprite(1);
//            }
//            else if (rarity == 3)
//            {
//                _specialFlagImage.ShowSprite(2);
//                _specialFrameImage.gameObject.SetActive(true);
//                _specialFrameImage.ShowSprite(2);
//            }
//            else if (rarity == 4)
//            {
//                _specialFlagImage.ShowSprite(3);
//                _specialFrameImage.gameObject.SetActive(true);
//                _specialFrameImage.ShowSprite(3);
//            }
//            else
//            {
//                _specialFlagImage.ShowSprite(0);
//                _specialFrameImage.gameObject.SetActive(false);
//                _specialFrameImage.ShowSprite(0);
//            }
//        }
//        public void ShowStar(KUIImage[] _starImages, int star)
//        {
//            for (int i = 0; i < _starImages.Length; i++)
//            {
//                _starImages[i].ShowGray(i >= star);
//            }
//        }
//    }


//    #endregion
//}


