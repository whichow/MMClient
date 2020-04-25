//// ***********************************************************************
//// Assembly         : Unity
//// Author           : LiMuChen
//// Created          : #DATA#
////
//// Last Modified By : LiMuChen
//// Last Modified On : 
//// ***********************************************************************
//// <copyright file= "AdoptionCatWindow.View" company="moyu"></copyright>
//// <summary></summary>
//// ***********************************************************************
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;

//namespace Game.UI
//{
//    partial class AdoptionCatWindow
//    {
//        #region Field

    
//        private Button _btnClose;
//        private Dropdown _sortDropdown;
//        private List<Toggle> _rarityToggles;
//        private KUIGrid _layoutElementPool;
//        public KCat[] _catsList;
//        private Transform[] _transCats;
  
//        #endregion

//        #region Method

//        public void InitView()
//        {
//            _sortDropdown = Find<Dropdown>("Panel/Myself/Myself/Dropdown");
//            _sortDropdown.onValueChanged.AddListener(this.OnSortTypeChange);
//            _btnClose = Find<Button>("Panel/Close");
//            _btnClose.onClick.AddListener(this.OnCancelClick);
//            var toggleGroup = Find<ToggleGroup>("Panel/Myself/Myself/Tab View/ToggleGroup");
//            _rarityToggles = new List<Toggle>(toggleGroup.GetComponentsInChildren<Toggle>());
//            for (int i = 0; i < _rarityToggles.Count; i++)
//            {
//                _rarityToggles[i].onValueChanged.AddListener(this.OnPageChange);
//            }
//            _layoutElementPool = Find<KUIGrid>("Panel/Myself/Myself/Scroll View");
//            if (_layoutElementPool )
//            {
//                _layoutElementPool.uiPool.itemTemplate.AddComponent<AdoptionMyCatItem>();
//            }
//            var catsTrans = Find<Transform>("Panel/Myself/CardGroup");
//            _catsList = new KCat[catsTrans.childCount];
//            _transCats = new Transform[catsTrans.childCount];
    
//            for (int i = 0; i < _transCats.Length; i++)
//            {
//                _transCats[i] = Find<Transform>("Panel/Myself/CardGroup/Cat"+(i+1));
//            }
 
//        }
//        public void RefreshTransCats()
//        {
//            InitCatList();

//            for (int i = 0; i < _catsList.Length; i++)
//            {
//                if (_catsList[i] != null)
//                {
//                    ShowCat(_transCats[i], _catsList[i]);
//                    _transCats[i].Find("Cat").gameObject.SetActive(true);
//                    _transCats[i].Find("Empty").gameObject.SetActive(false);
//                    _transCats[i].Find("Cat/Del").GetComponent<Button>().onClick.AddListener(OnePointColliderObject);
//                }
//                else
//                {
//                    _transCats[i].Find("Cat").gameObject.SetActive(false);
//                    _transCats[i].Find("Empty").gameObject.SetActive(true);
//                }

//            }
//        }
//        /// <summary>//        /// 点击对象//        /// </summary>//        /// <returns>点击对象Tag</returns>//        public void OnePointColliderObject()
//        {//            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);//            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

//            List<RaycastResult> results = new List<RaycastResult>();//            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
//            switch (results[0].gameObject.transform.parent.transform.parent.name)
//            {
//                case "Cat1":
//                    RemoveCat(_catsList[0]);
//                    break;
//                case "Cat2":
//                    RemoveCat(_catsList[1]);
//                    break;
//                case "Cat3":
//                    RemoveCat(_catsList[2]);
//                    break;
//                case "Cat4":
//                    RemoveCat(_catsList[3]);
//                    break;
//                case "Cat5":
//                    RemoveCat(_catsList[4]);
//                    break;
//                case "Cat6":
//                    RemoveCat(_catsList[5]);
//                    break;
//                default:
//                    break;
//            }
//        }
//        public void InitCatList()
//        {
//            for (int i = 0; i < _catsList.Length; i++)
//            {
//                if (i < KFoster.Instance.selfSlots.Count)
//                {
//                    _catsList[i] = KFoster.Instance.selfSlots[i].cat;
//                }
//                else
//                {
//                    _catsList[i] = null;
//                }
//            }
//        }
//        public void ShowCat(Transform trans, KCat cat)
//        {
//            Image _imageIcon;
//            Text _textGrad;
//            KUIImage _imageFrame;
//            KUIImage _imageFlag;
//            KUIImage[] _starImages;
//            Text _textStne;
//            KUIImage _imageColor;
//            Text _textName;
//            Image _imageCatExp;
//            var fish = trans.Find("Cat/Fish");
//            _starImages = new KUIImage[fish.childCount];
//            for (int i = 0; i < _starImages.Length; i++)
//            {
//                _starImages[i] = fish.GetChild(i).GetComponent<KUIImage>();
//            }
//            _imageCatExp = trans.Find("Cat/ProgressImageBack/Image").GetComponent<Image>();
//            _textName = trans.Find("Cat/Name").GetComponent<Text>();
//            _imageIcon = trans.Find("Cat").GetComponent<Image>();
//            _textGrad = trans.Find("Cat/Level/Text").GetComponent<Text>();
//            _imageFlag = trans.Find("Cat/Level").GetComponent<KUIImage>();
//            _imageFrame = trans.Find("Cat/Light").GetComponent<KUIImage>();
//            _imageColor = trans.Find("Cat/Fish").GetComponent<KUIImage>();
//            _textStne = trans.Find("Cat/Item/Text").GetComponent<Text>();
//            ShowRarity(_imageFlag, _imageFrame, cat.rarity);
//            _textName.text = cat.name;
//            _imageCatExp.fillAmount = Mathf.Clamp01((float)cat.exp / (float)cat.maxExp);
//            _textGrad.text = cat.grade.ToString();
//            _textStne.text = cat.soulStone.ToString();
//            _imageIcon.overrideSprite = KIconManager.Instance.GetCatFull(cat.photo);
//            ShowStar(_starImages, cat.star);
//            ShowColor(_imageColor, cat.mainColor);
//        }
//        public void AddCat(KCat cat)
//        {
//            KFoster.Instance.AddSelfCat(cat.catId,AddSelfCatCallBack);
         
//        }
//        private void AddSelfCatCallBack(int code,string message,object data)
//        {
//            if (code==0)
//            {
//                RefreshView();
//            }
//        }
//        public void RemoveCat(KCat cat)
//        {
//            KFoster.Instance.RemoveSelfCat(cat.catId, RemoveSelfCatCallBack);
//        }
//        private void RemoveSelfCatCallBack(int code,string message,object data)
//        {
//            if (code == 0)
//            {
//                RefreshView();
//            }
//        }
//        //public bool isHaveNull()
//        //{
//        //    for (int i = 0; i < _catsList.Length; i++)
//        //    {
//        //        if (_catsList[i] == null)
//        //        {
//        //            return true;
//        //        }
//        //    }
//        //    return false;
//        //}
//        public void ShowStar(KUIImage[] _starImages, int star)
//        {
//            for (int i = 0; i < _starImages.Length; i++)
//            {
//                _starImages[i].ShowGray(i >= star);
//            }
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
//        public void RefreshView()
//        {
    
//            RefreshTransCats();
//            _sortDropdown.value = (int)sortType;
    

//            StartCoroutine(FillElements());

             
//        }
//        private IEnumerator FillElements()
//        {
//            _layoutElementPool.ClearItems();
//            var cats = GetCatInfos();
//            var datas = new ArrayList(cats.Length);
//            for (int i = 0; i < cats.Length; i++)
//            {
//                //datas.Add(new CatInfoWindow.WindowData
//                //{
//                //    catIndex = i,
//                //    catArray = cats,
//                //});
//            }
//            _layoutElementPool.uiPool.SetItemDatas(datas);
//            _layoutElementPool.RefillItems(0);
//            yield return null;
//        }
//        private string GetOnToggle()
//        {
//            foreach (var toggle in _rarityToggles)
//            {
//                if (toggle.isOn)
//                {
//                    return toggle.name;
//                }
//            }
//            return null;
//        }




     
//        #endregion
//    }


//}

