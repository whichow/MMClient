//// ***********************************************************************
//// Assembly         : Unity
//// Author           : Kimch
//// Created          : 
////
//// Last Modified By : Kimch
//// Last Modified On : 
//// ***********************************************************************
//// <copyright file= "CatLearnWindow" company=""></copyright>
//// <summary></summary>
//// ***********************************************************************
//using Spine.Unity;
//using System.Collections;
//using UnityEngine;
//using UnityEngine.UI;

//namespace Game.UI
//{
//    partial class CatLearnWindow
//    {
//        #region Field

//        Button _backBtn;
//        Button _maskBtn;
//        Button _tipsBtn;

//        Toggle _starToggle;
//        public Toggle _skillToggle;

//        Transform _skillUp;
//        Transform _starUp;
//        Button _okBtn;

//        private KUIGrid _layoutElementPool;
//        private Transform[] transContenCats;
//        public KCat[] catList;
//        private int haveNum;
//        private Image _imageCatIcon;
//        private KUIImage _imageFrame;
//        private KUIImage _imageFrag;
//        private Text _textGrad;
//        private KUIImage[] _starImages;
//        private SkeletonAnimation _starUpAnimation;

//        private Image _imageCatIconSkill;
//        private KUIImage _imageFrameSkill;
//        private KUIImage _imageFragSkill;
//        private Text _textGradSkill;
//        private KUIImage[] _starImagesSkill;
//        private CatLearnItem[] itemList;
//        private Text _textMaxGrade;
//        //升级技能
//        private KCat[] catSkillList;
//        private CatLearnItem[] itemSkillList;
//        private Transform[] transContenCatSkills;
//        public KCat _cat;
//        private Text _textConst;
//        private SkeletonAnimation _skillUpAnimation;
//        private Text _textChoice;
//        //特效方向


//        static Quaternion q1 = Quaternion.Euler(0f, 0f, 0f);
//        static Quaternion q2 = Quaternion.Euler(0f, 180f, 0f);
//        static Quaternion q3 = Quaternion.Euler(180f, 180f, 0f);
//        static Quaternion q4 = Quaternion.Euler(180f, 0f, 0f);
//        static Quaternion q5 = Quaternion.Euler(180, 0, 90);
//        static Quaternion q6 = Quaternion.Euler(180, 180, 0);
//        static Quaternion q7 = Quaternion.Euler(0, 180, 90);

//        static Quaternion[] quaternTwo = new Quaternion[] {
//            q1,
//            q2,
//            q4,
//            q3
//        };
//        static Quaternion[] quaternOne = new Quaternion[] {
//            q1,
//            q5,
//            q6,
//            q7
//        };
//        #endregion

//        #region Method

//        public void InitView()
//        {
//            _backBtn = Find<Button>("Back");
//            _backBtn.onClick.AddListener(this.OnBackBtnClick);

//            _tipsBtn = Find<Button>("Tip");
//            _tipsBtn.onClick.AddListener(this.OnTipsBtnClick);

//            _starToggle = Find<Toggle>("Tab View/ToggleGroup/Toggle1");
//            _starToggle.onValueChanged.AddListener(this.OnToggleValueChanged);
//            _skillToggle = Find<Toggle>("Tab View/ToggleGroup/Toggle2");
//            _skillToggle.onValueChanged.AddListener(this.OnToggleValueChanged);

//            _skillUp = Find<Transform>("R/SkillUp");
//            _starUp = Find<Transform>("R/StarUp");
//            _okBtn = Find<Button>("R/ButtonOK");

//            _okBtn.onClick.AddListener(OnOkClick);
//            _textMaxGrade = Find<Text>("R/StarUp/Tipimage/Text");
//            _layoutElementPool = Find<KUIGrid>("L/Scroll View");
//            if (_layoutElementPool)
//            {
//                _layoutElementPool.uiPool.itemTemplate.AddComponent<CatLearnItem>();

//            }
//            transContenCats = new Transform[4];
//            for (int i = 0; i < transContenCats.Length; i++)
//            {
//                transContenCats[i] = Find<Transform>("R/StarUp/ContentHeadIconBack/Item" + (i + 1));
//            }
//            transContenCatSkills = new Transform[8];
//            for (int i = 0; i < transContenCatSkills.Length; i++)
//            {
//                transContenCatSkills[i] = Find<Transform>("R/SkillUp/Item" + (i + 1));
//            }
//            _imageCatIcon = Find<Image>("R/StarUp/Item/Cat/Icon");
//            _imageFrame = Find<KUIImage>("R/StarUp/Item/Cat/Frame");
//            _imageFrag = Find<KUIImage>("R/StarUp/Item/Cat/Flag");
//            _textGrad = Find<Text>("R/StarUp/Item/Cat/Grade");
//            _starUpAnimation = Find<SkeletonAnimation>("R/StarUp/Item/Fx/Fx_StarUp_01");
//            _textConst = Find<Text>("R/ButtonOK/Text");
//            var fish = transform.Find("R/StarUp/Item/Cat/Fish");
//            _starImages = new KUIImage[fish.childCount];
//            for (int i = 0; i < _starImages.Length; i++)
//            {
//                _starImages[i] = fish.GetChild(i).GetComponent<KUIImage>();
//            }
//            _skillUpAnimation = Find<SkeletonAnimation>("R/SkillUp/Item9/Fx/Fx_StarUp_01");
//            _imageCatIconSkill = Find<Image>("R/SkillUp/Item9/Cat/Icon");
//            _imageFrameSkill = Find<KUIImage>("R/SkillUp/Item9/Cat/Frame");
//            _imageFragSkill = Find<KUIImage>("R/SkillUp/Item9/Cat/Flag");
//            _textGradSkill = Find<Text>("R/SkillUp/Item9/Cat/Grade");
//            var fishSkill = transform.Find("R/SkillUp/Item9/Cat/Fish");
//            _starImagesSkill = new KUIImage[fishSkill.childCount];

//            for (int i = 0; i < _starImagesSkill.Length; i++)
//            {
//                _starImagesSkill[i] = fishSkill.GetChild(i).GetComponent<KUIImage>();
//            }
//            _textChoice = Find<Text>("L/Choice");
//            RefreshView();
//        }
//        public void RefreshView()
//        {

//            Debug.Log("RefreshView");

//            StartCoroutine(FillElements());
//            _cat = data as KCat;
//            _textMaxGrade.text = string.Format(KLocalization.GetLocalString(54049), _cat.nextmaxGrade);
//            oldCatSkillGrade = _cat.skillGrade;
//            oldCatAddCoin = _cat.coinAbility;
//            oldCatExplore = _cat.exploreAbility;
//            oldCatMatch = _cat.matchAbility;
//            oldCatMaxGrad = _cat.maxGrade;
//            oldCatStart = _cat.star;
//            int skillLvl = _cat.GetSkillMaxGrade() - _cat.skillGrade;
//            if (skillLvl > 8)
//            {
//                catSkillList = new KCat[8];
//                itemSkillList = new CatLearnItem[8];
//            }
//            else
//            {
//                catSkillList = new KCat[skillLvl];
//                itemSkillList = new CatLearnItem[skillLvl];
//            }


//            catList = new KCat[_cat.star];
//            itemList = new CatLearnItem[_cat.star];
//            var catItem = KItemManager.Instance.GetCat(_cat.shopId);
//            if (_skillToggle.isOn)
//            {
//                if (_cat.GetSkillMaxGrade()>_cat.skillGrade)
//                {
          
//                    _okBtn.GetComponent<Image>().material = null;
//                }
//                else
//                {
//                    _okBtn.GetComponent<Image>().material = Resources.Load<Material>("Materials/UIGray");
//                }
//                _skillUp.gameObject.SetActive(true);
//                _starUp.gameObject.SetActive(false);
//                _textConst.text = catItem.GetUpskillCost(_cat.skillGrade).ToString();
//                RefreshCallBackStartView();
//                if (GetCatSkillInfos().Count > 0)
//                {
//                    _textChoice.gameObject.SetActive(false);
//                }
//                else
//                {
//                    _textChoice.gameObject.SetActive(true);
//                }
//            }
//            else
//            {
//                if (_cat.maxGrade > _cat.grade)
//                {
//                    _okBtn.GetComponent<Image>().material = Resources.Load<Material>("Materials/UIGray");
//                }
//                else
//                {
//                    _okBtn.GetComponent<Image>().material = null;
//                }
//                _skillUp.gameObject.SetActive(false);
//                _starUp.gameObject.SetActive(true);
//                _textConst.text = catItem.GetUpstarCost(_cat.skillGrade).ToString();

//                RefreshCallBackSkillView();
//                if (GetCatStartInfos().Count > 0)
//                {
//                    _textChoice.gameObject.SetActive(false);
//                }
//                else
//                {
//                    _textChoice.gameObject.SetActive(true);
//                }
//            }
//            RefreshViewStart();
//            RefreshViewSkill();

//        }
//        public override void OnDisable()
//        {
//            RefreshCallBackSkillView();
//            RefreshCallBackStartView();
//        }
//        public void RefreshCallBackSkillView()
//        {
//            for (int i = 0; i < transContenCatSkills.Length; i++)
//            {
//                transContenCatSkills[i].Find("Cat").gameObject.SetActive(false);
//            }
//            for (int i = 0; i < catSkillList.Length; i++)
//            {
//                catSkillList[i] = null;
//            }
//        }
//        public void RefreshCallBackStartView()
//        {
//            for (int i = 0; i < transContenCats.Length; i++)
//            {
//                transContenCats[i].Find("Cat").gameObject.SetActive(false);
//            }
//            for (int i = 0; i < catList.Length; i++)
//            {
//                catList[i] = null;
//            }
//        }
//        private bool CatsStarIsHaveNull()
//        {
//            for (int i = 0; i < catList.Length; i++)
//            {
//                if (catList[i]==null)
//                {
//                    return true;
//                }
//            }
//            return false;
//        }
//        public void AddCat(KCat cat, CatLearnItem item)
//        {
//            if (cat.locked == true)
//            {
//                ToastBox.ShowText(string.Format(KLocalization.GetLocalString(54116), _cat.maxGrade));
//                return;
//            }
//            if (_cat.maxGrade > _cat.grade)
//            {
//                ToastBox.ShowText(string.Format(KLocalization.GetLocalString(54115), _cat.maxGrade));
//                return;
//            }
//            item.RefreshItem(true);
//            int indx = 0;
//            for (int i = 0; i < catList.Length; i++)
//            {
//                if (CatsStarIsHaveNull())
//                {
//                    if (catList[i] == null)
//                    {
//                        catList[i] = cat;
//                        itemList[i] = item;
//                        indx = i;
//                        break;
//                    }
//                    continue;
//                }
//                else
//                {
//                    itemList[itemList.Length-1].RefreshItem(false);
//                    catList[catList.Length-1] = cat;
//                    itemList[itemList.Length-1] = item;
//                    indx = catList.Length-1;
//                    break;
//                }

//            }
//            Image icon;
//            Text _textGrad;
//            KUIImage _specialFlagImage;
//            KUIImage _specialFrameImage;
//            KUIImage[] _starImages;
//            Transform trans = transContenCats[indx];
//            Transform transSkillImage;
//            icon = trans.Find("Cat/Icon").GetComponent<Image>();
//            icon.overrideSprite = cat.GetIconSprite();
//            _textGrad = trans.Find("Cat/Grade").GetComponent<Text>();
//            _textGrad.text = cat.grade.ToString();
//            _specialFlagImage = trans.Find("Cat/Flag").GetComponent<KUIImage>();
//            _specialFrameImage = trans.Find("Cat/Frame").GetComponent<KUIImage>();
//            transSkillImage = trans.Find("Cat/Skill").GetComponent<Transform>();
//            ShowRarity(_specialFlagImage, _specialFrameImage, cat.rarity);
//            if (_cat.shopId == cat.shopId)
//            {
//                transSkillImage.gameObject.SetActive(true);
//            }
//            else
//            {
//                transSkillImage.gameObject.SetActive(false);
//            }
//            var fish = trans.Find("Cat/Fish");
//            _starImages = new KUIImage[fish.childCount];
//            for (int i = 0; i < _starImages.Length; i++)
//            {
//                _starImages[i] = fish.GetChild(i).GetComponent<KUIImage>();
//            }
//            ShowStar(_starImages, cat.star);
//            trans.Find("Cat").gameObject.SetActive(true);
//            trans.Find("Empty").gameObject.SetActive(false);
//        }
//        private bool CatsSkillIsHaveNull()
//        {
//            for (int i = 0; i < catSkillList.Length; i++)
//            {
//                if (catSkillList[i] == null)
//                {
//                    return true;
//                }
//            }
//            return false;
//        }
//        public void AddSkillCat(KCat cat, CatLearnItem item)
//        {
//            if (_cat.GetSkillMaxGrade() > _cat.skillGrade)
//            {
//                item.RefreshItem(true);
//                int indx = 0;
//                for (int i = 0; i < catSkillList.Length; i++)
//                {
//                    if (CatsSkillIsHaveNull())
//                    {
//                        if (catSkillList[i] == null)
//                        {
//                            catSkillList[i] = cat;
//                            itemSkillList[i] = item;
//                            indx = i;
//                            break;
//                        }
//                        continue;
//                    }
//                    else
//                    {
//                        itemSkillList[itemSkillList.Length-1].RefreshItem(false);
//                        catSkillList[catSkillList.Length-1] = cat;
//                        itemSkillList[catSkillList.Length-1] = item;
//                        indx = catSkillList.Length-1;
//                        break;
//                    }
//                }
//                Image icon;
//                Text _textGrad;
//                KUIImage _specialFlagImage;
//                KUIImage _specialFrameImage;
//                KUIImage[] _starImages;
//                Transform trans = transContenCatSkills[indx];

//                icon = trans.Find("Cat/Icon").GetComponent<Image>();
//                icon.overrideSprite = cat.GetIconSprite();
//                _textGrad = trans.Find("Cat/Grade").GetComponent<Text>();
//                _textGrad.text = cat.grade.ToString();
//                _specialFlagImage = trans.Find("Cat/Flag").GetComponent<KUIImage>();
//                _specialFrameImage = trans.Find("Cat/Frame").GetComponent<KUIImage>();
//                ShowRarity(_specialFlagImage, _specialFrameImage, cat.rarity);

//                var fish = trans.Find("Cat/Fish");
//                _starImages = new KUIImage[fish.childCount];
//                for (int i = 0; i < _starImages.Length; i++)
//                {
//                    _starImages[i] = fish.GetChild(i).GetComponent<KUIImage>();
//                }
//                ShowStar(_starImages, cat.star);
//                trans.Find("Cat").gameObject.SetActive(true);
//                trans.Find("Empty").gameObject.SetActive(false);

//            }
//        }

//        public void RemvoeCatSkill(KCat cat, CatLearnItem item)
//        {
//            item.RefreshItem(false);
//            int indx = 0;
//            for (int i = 0; i < catSkillList.Length; i++)
//            {
//                if (catSkillList[i] != null)
//                {
//                    if (cat.catId == catSkillList[i].catId)
//                    {
//                        indx = i;
//                        itemSkillList[i] = null;
//                        catSkillList[i] = null;
//                    }
//                }
//            }
//            transContenCatSkills[indx].Find("Cat").gameObject.SetActive(false);
//            transContenCatSkills[indx].Find("Empty").gameObject.SetActive(true);


//        }
//        public void ShowStar(KUIImage[] _starImages, int star)
//        {
//            for (int i = 0; i < _starImages.Length; i++)
//            {
//                _starImages[i].ShowGray(i >= star);
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

//        public void RemoveCat(KCat cat, CatLearnItem item)
//        {
//            item.RefreshItem(false);
//            int indx = 0;
//            for (int i = 0; i < catList.Length; i++)
//            {
//                if (catList[i] != null)
//                {
//                    if (cat.catId == catList[i].catId)
//                    {
//                        indx = i;
//                        itemList[i] = null;
//                        catList[i] = null;
//                    }
//                }
//            }
//            transContenCats[indx].Find("Cat").gameObject.SetActive(false);
//            transContenCats[indx].Find("Empty").gameObject.SetActive(true);
//        }
//        public bool IsHaveCat(Transform trans)
//        {
//            return trans.Find("Cat").gameObject.activeSelf; ;
//        }
//        public void RefreshViewStart()
//        {
//            _cat = data as KCat;

//            for (int i = 0; i < transContenCats.Length; i++)
//            {
//                if (i > _cat.star - 1)
//                {
//                    transContenCats[i].Find("Black").gameObject.SetActive(true);
//                    transContenCats[i].Find("Empty").gameObject.SetActive(true);
//                }
//                else
//                {
//                    transContenCats[i].Find("Black").gameObject.SetActive(false);
//                    transContenCats[i].Find("Empty").gameObject.SetActive(true);
//                }
//            }
//            _imageCatIcon.overrideSprite = _cat.GetIconSprite();
//            ShowRarity(_imageFrag, _imageFrame, _cat.rarity);
//            ShowStar(_starImages, _cat.star);
//            _textGrad.text = _cat.grade.ToString();
//        }
//        private void RefreshViewSkill()
//        {
//            var cat = data as KCat;
//            for (int i = 0; i < transContenCatSkills.Length; i++)
//            {
//                if (i > itemSkillList.Length - 1)
//                {
//                    transContenCatSkills[i].Find("Black").gameObject.SetActive(true);
//                    transContenCatSkills[i].Find("Empty").gameObject.SetActive(true);
//                }
//                else
//                {
//                    transContenCatSkills[i].Find("Black").gameObject.SetActive(false);
//                    transContenCatSkills[i].Find("Empty").gameObject.SetActive(true);
//                }
//            }
//            _imageCatIconSkill.overrideSprite = cat.GetIconSprite();
//            ShowRarity(_imageFragSkill, _imageFrameSkill, cat.rarity);
//            ShowStar(_starImagesSkill, cat.star);
//            _textGradSkill.text = cat.grade.ToString();
//        }
//        private IEnumerator FillElements()
//        {
//            _layoutElementPool.ClearItems();
//            if (_skillToggle.isOn)
//            {
//                cats = GetCatSkillInfos();
//            }
//            else
//            {
//                cats = GetCatStartInfos();
//            }
//            var datas = new ArrayList(cats.Count);
//            for (int i = 0; i < cats.Count; i++)
//            {
//                //datas.Add(new CatInfoWindow.WindowData
//                //{
//                //    catIndex = i,
//                //    catArray = cats.ToArray(),
//                //});
//            }
//            _layoutElementPool.uiPool.SetItemDatas(datas);
//            _layoutElementPool.RefillItems(0);
//            yield return null;
//        }

//        #endregion
//    }
//}

