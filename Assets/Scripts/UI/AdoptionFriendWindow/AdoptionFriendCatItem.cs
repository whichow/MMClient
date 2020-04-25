//// ***********************************************************************
//// Assembly         : Unity
//// Author           : LiMuChen
//// Created          : #DATA#
////
//// Last Modified By : LiMuChen
//// Last Modified On : 
//// ***********************************************************************
//// <copyright file= "AdoptionMyCatItem" company="moyu"></copyright>
//// <summary></summary>
//// ***********************************************************************
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;
//namespace Game.UI
//{
//    public class AdoptionFriendCatItem : KUIItem, IPointerClickHandler
//    {
//        #region Field
//        private Image _iconImage;
//        //private Image _lockImage;
//        private Text _gradeText;
//        private KUIImage _specialFlagImage;
//        private KUIImage _specialFrameImage;
//        private KUIImage _colorImage;
//        private KUIImage[] _starImages;

//        private int _catIndex;
//        private KCat[] _catArray;
//        private Image _imageGrade;
//        private Transform _transBlack;
//        private Text _backText;
//        private Transform _transOk;
//        private Transform _transText;
//        private KCat cat;
//        private Text _textCatName;
//        #endregion

//        #region Interface
//        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
//        {
//            if (_transBlack.gameObject.activeSelf == false)
//            {
//                KUIWindow.GetWindow<AdoptionFriendWindow>().AddCat(cat);
//            }
//            //else
//            //{
//            //    KUIWindow.GetWindow<AdoptionCatWindow>().RemoveCat(cat, this);
//            //}
//        }
//        #endregion

//        #region Method

//        public void ShowCat(KCat[] cats, int index)
//        {
//            if (cats == null || index < 0 || index >= cats.Length)
//            {
//                return;
//            }


//            cat = cats[index];


//            if (cat != null)
//            {
//                ShowColor(cat.mainColor);
//                ShowIcon(cat.photo);
//                //ShowLock(cat.locked);
//                ShowRarity(cat.rarity);
//                ShowStar(cat.star);
//                ShowGrade(cat.grade);
//                _imageGrade.fillAmount = Mathf.Clamp01((float)cat.exp / (float)cat.maxExp);
//                ShowState(cat.state);
//                if ( string.IsNullOrEmpty( cat.nickName))
//                {
//                    _textCatName.text = cat.nickName;
//                }
//                else
//                {
//                    _textCatName.text = cat.name;
//                }
     
//            }
//        }
//        public void RefreshItem(bool isActivity)
//        {
//            _transBlack.gameObject.SetActive(isActivity);
//        }
//        protected override void Refresh()
//        {
//            //var realData = data as CatInfoWindow.WindowData;
//            //if (realData != null)
//            //{
//            //    ShowCat(realData.catArray, realData.catIndex);
//            //}
//        }
//        public void ShowIcon(string icon)
//        {
//            _iconImage.overrideSprite = KIconManager.Instance.GetCatFull(icon);
//        }

//        //public void ShowLock(bool locked)
//        //{
//        //    _lockImage.enabled = locked;
//        //}
//        public void ShowState(int state)
//        {
//            switch (state)
//            {
//                case 0:
//                    _transBlack.gameObject.SetActive(false);
//                    break;
//                case 1:
//                    _transBlack.gameObject.SetActive(true);
//                    _transOk.gameObject.SetActive(false);
//                    _backText.text = KLocalization.GetLocalString(54108);
//                    _transText.gameObject.SetActive(true);
//                    break;
//                case 2:
//                    _transBlack.gameObject.SetActive(true);
//                    _transOk.gameObject.SetActive(false);
//                    _backText.text = KLocalization.GetLocalString(54110);
//                    _transText.gameObject.SetActive(true);
//                    break;
//                case 3:
//                    _transBlack.gameObject.SetActive(true);
//                    _transOk.gameObject.SetActive(true);
//                    _transText.gameObject.SetActive(false);
//                    break;
//                default:
//                    break;
//            }


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

//        #endregion

  


//        #region Unity
//        void Awake()
//        {
//            _iconImage = Find<Image>("Item/Cat");
//            _gradeText = Find<Text>("Item/Level/Text");
//            _specialFlagImage = Find<KUIImage>("Item/Level");
//            _specialFrameImage = Find<KUIImage>("Item/Light");
//            _colorImage = Find<KUIImage>("Item/Fish");
//            _transBlack = Find<Transform>("Item/Black");
//            _backText = Find<Text>("Item/Black/Item1/Text");
//            _transText = Find<Transform>("Item/Black/Item1");
//            _transOk = Find<Transform>("Item/Black/OK");
//            var fish =Find<Transform>("Item/Fish");
//            _textCatName = Find<Text>("Item/Name");
//            _starImages = new KUIImage[fish.childCount];
//            for (int i = 0; i < _starImages.Length; i++)
//            {
//                _starImages[i] = fish.GetChild(i).GetComponent<KUIImage>();
//            }
//            _imageGrade = Find<Image>("Item/ProgressImageBack/Image");
//        }


//        void Update()
//        {

//        }
//        #endregion
//    }


//}

