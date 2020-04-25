//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;
//using System;

//namespace Game.UI
//{
//    public class CatLearnItem : KUIItem, IPointerClickHandler
//    {
//        #region Field

//        private Image _iconImage;
//        private Image _lockImage;
//        private Text _gradeText;
//        private KUIImage _specialFlagImage;
//        private KUIImage _specialFrameImage;
//        private KUIImage[] _starImages;

//        private int _catIndex;
//        private KCat[] _catArray;
//        private Transform _goOk;
//        private Transform _goBlack;
//        private KCat cat;
//        private Transform _stateObject;
//        private Text _stateText;
//        private bool isOn;
//        #endregion

//        #region Method

//        public void ShowCat(KCat[] cats, int index)
//        {
//            if (cats == null || cats.Length == 0)
//            {
//                return;
//            }
//            _catArray = cats;
//            _catIndex = index;

//            cat = cats[index];
//            if (cat != null)
//            {
//                ShowIcon(cat.GetIconSprite());
//                ShowLock(cat.locked);
//                ShowRarity(cat.rarity);
//                ShowStar(cat.star);
//                ShowGrade(cat.grade);
//                RefreshItem(false);
//                ShowState(cat.state);
//            }
//        }
//        protected override void Refresh()
//        {
//            //var realData = data as CatInfoWindow.WindowData;
//            //if (realData != null)
//            //{
//            //    ShowCat(realData.catArray, realData.catIndex);
//            //}
//        }
//        public void ShowIcon(Sprite icon)
//        {
//            _iconImage.overrideSprite = icon;
//        }

//        public void ShowLock(bool locked)
//        {
//            _lockImage.gameObject.SetActive(locked);
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
//        public void RefreshItem(bool isOn)
//        {
//            _goOk.gameObject.SetActive(isOn);
//            _goBlack.gameObject.SetActive(isOn);

//        }
//        #endregion

//        #region Interface

//        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
//        {
//            if (cat.locked || cat.state != 0)
//            {
//                Debug.Log("已上锁");
//                return;
//            }
//            else
//            {

//                if (!KUIWindow.GetWindow<CatLearnWindow>()._skillToggle.isOn)
//                {
//                    if (_goOk.gameObject.activeSelf == false)
//                    {
//                        KUIWindow.GetWindow<CatLearnWindow>().AddCat(cat, this);
//                    }
//                    else
//                    {
//                        KUIWindow.GetWindow<CatLearnWindow>().RemoveCat(cat, this);
//                    }

//                }
//                else
//                {
//                    var _cat = KUIWindow.GetWindow<CatLearnWindow>()._cat;
//                    if (_cat.GetSkillMaxGrade()>_cat.skillGrade)
//                    {
//                        if (_goOk.gameObject.activeSelf == false)
//                        {
//                            KUIWindow.GetWindow<CatLearnWindow>().AddSkillCat(cat, this);
//                        }
//                        else
//                        {
//                            KUIWindow.GetWindow<CatLearnWindow>().RemvoeCatSkill(cat, this);
//                        }
//                    }
              
//                }

//            }

//        }

//        #endregion    

//        #region Unity

//        private void Awake()
//        {
//            _iconImage = Find<Image>("Item/Icon");
//            _lockImage = Find<Image>("Item/Lock");
//            _gradeText = Find<Text>("Item/Grade");
//            _specialFlagImage = Find<KUIImage>("Item/Flag");
//            _specialFrameImage = Find<KUIImage>("Item/Frame");
//            _goOk = Find<Transform>("Item/OK");
//            _goBlack = Find<Transform>("Item/Black");
//            _stateObject = Find<Transform>("Item/State");
//            _stateText = Find<Text>("Item/State/Text");
//            var fish = transform.Find("Item/Fish");
//            _starImages = new KUIImage[fish.childCount];
//            for (int i = 0; i < _starImages.Length; i++)
//            {
//                _starImages[i] = fish.GetChild(i).GetComponent<KUIImage>();
//            }
//        }

//        #endregion
//    }


//}
