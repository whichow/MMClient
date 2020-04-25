//// ***********************************************************************
//// Assembly         : Unity
//// Author           : Kimch
//// Created          : 
////
//// Last Modified By : Kimch
//// Last Modified On : 
//// ***********************************************************************
//// <copyright file= "CatBagItem" company=""></copyright>
//// <summary></summary>
//// ***********************************************************************
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;

//namespace Game.UI
//{
//    public class CatBagItem : KUIItem
//    {
//        #region Field

//        private Image _iconImage;
//        private Image _lockImage;
//        private Text _gradeText;
//        private KUIImage _specialFlagImage;
//        private KUIImage _specialFrameImage;
//        private KUIImage _colorImage;
//        private KUIImage[] _starImages;

//        private GameObject _stateObject;
//        private Text _stateText;
//        private Button _itemBtn;

//        #endregion

//        #region Method

//        public void ShowCat(KCat[] cats, int index)
//        {
//            if (cats == null || index < 0 || index >= cats.Length)
//            {
//                return;
//            }

//            var cat = cats[index];
//            if (cat != null)
//            {
//                ShowColor(cat.mainColor);
//                ShowIcon(cat.GetIconSprite());
//                ShowLock(cat.locked);
//                ShowRarity(cat.rarity);
//                ShowStar(cat.star);
//                ShowGrade(cat.grade);
//                ShowState(cat.state);
//            }
//        }

//        public void ShowIcon(Sprite icon)
//        {
//            _iconImage.overrideSprite = icon;
//        }

//        public void ShowLock(bool locked)
//        {
//            _lockImage.enabled = locked;
//        }

//        public void ShowColor(int color)
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

//        public void ShowRarity(int rarity)
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

//        public void ShowStar(int star)
//        {
//            for (int i = 0; i < _starImages.Length; i++)
//            {
//                _starImages[i].ShowGray(i >= star);
//            }
//        }

//        public void ShowGrade(int grade)
//        {
//            _gradeText.text = grade.ToString();
//        }

//        public void ShowState(int state)
//        {

//            switch (state)
//            {
//                case 0:
//                    _stateObject.gameObject.SetActive(false);
//                    break;
//                case 1:
//                    _stateObject.gameObject.SetActive(true);
//                    _stateText.text = KLocalization.GetLocalString(54108);
//                    break;
//                case 2:
//                    _stateObject.gameObject.SetActive(true);
//                    _stateText.text = KLocalization.GetLocalString(54110);
//                    break;
//                case 3:
//                    _stateObject.gameObject.SetActive(true);
//                    _stateText.text = KLocalization.GetLocalString(54109);
//                    break;
//                default:
//                    break;
//            }

//        }

//        #endregion

//        #region Interface

//        //void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
//        //{
           
//        //}
//        private void OnItemBtnClick()
//        {
//            KUIWindow.OpenWindow<CatInfoWindow>(windowData: data);
//        }
//        #endregion

//        protected override void Refresh()
//        {
//            //var realData = data as CatInfoWindow.WindowData;
//            //if (realData != null)
//            //{
//            //    ShowCat(realData.catArray, realData.catIndex);
//            //}
//        }

//        #region Unity

//        private void Awake()
//        {
//            _iconImage = Find<Image>("Item/Icon");
//            _lockImage = Find<Image>("Item/Lock");
//            _gradeText = Find<Text>("Item/Grade");
//            _specialFlagImage = Find<KUIImage>("Item/Flag");
//            _specialFrameImage = Find<KUIImage>("Item/Frame");

//            _stateObject = Find("Item/State");
//            _stateText = Find<Text>("Item/State/Text");

//            _colorImage = Find<KUIImage>("Item/Fish");
//            var fish = transform.Find("Item/Fish");
//            _starImages = new KUIImage[fish.childCount];
//            for (int i = 0; i < _starImages.Length; i++)
//            {
//                _starImages[i] = fish.GetChild(i).GetComponent<KUIImage>();
//            }
//            _itemBtn = Find<Button>("Item");
//            _itemBtn.onClick.AddListener(OnItemBtnClick);
//        }

//        #endregion
//    }
//}

