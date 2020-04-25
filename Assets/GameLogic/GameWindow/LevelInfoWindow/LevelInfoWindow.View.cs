using Game.DataModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public partial class LevelInfoWindow
    {
        private GameObject  _collectGoalGO;
        private GameObject  _scoreGoalGO;
        private UIList      _collectGoalList;
        private Text        _scoreGoalText;

        private GameObject  _modelParent;
        private GameObject  _noCatGO;
        private GameObject  _catInfoGO;
        private Text        _catLvText;
        private Text        _goldBonusText;
        private Text        _scoreBonusText;

        private GameObject  _skillInfoGO;
        private Image       _skillIcon;
        private Text        _skillName;

        private Text        _titleTxt;
        private UIList      _catList;
        private GameObject  _noCatTipsGO;
        private GameObject  _catTipsGO;
        private Button      _closeBtn;
        private Button      _startBtn;
        private Text        _energyCostText;

        private RectTransform _energyFly;
        private RectTransform _energyStartPoint;
        private RectTransform _energyEndPoint;

        private KUIImage[]  _stars = new KUIImage[3];

        private bool        isStart;
        private GameObject  catModel;


        public void InitView()
        {
            _collectGoalGO      = Find("Content/L/CollectModePanel/Top/Goal");
            _scoreGoalGO        = Find("Content/L/CollectModePanel/Top/Score");
            _collectGoalList    = Find<UIList>("Content/L/CollectModePanel/Top/Goal/List");
            _scoreGoalText      = Find<Text>("Content/L/CollectModePanel/Top/Score/Text");

            _modelParent        = Find("Content/L/CatPanel/CatModel");
            _noCatGO            = Find("Content/L/CatPanel/NoCat");
            _catInfoGO          = Find("Content/L/CatPanel/CatInfo");
            _catLvText          = Find<Text>("Content/L/CatPanel/CatInfo/Level/Text");
            _goldBonusText      = Find<Text>("Content/L/CatPanel/CatInfo/Gold/Text");
            _scoreBonusText     = Find<Text>("Content/L/CatPanel/CatInfo/Score/Text");

            _skillInfoGO        = Find("Content/L/SkillPanel");
            _skillIcon          = Find<Image>("Content/L/SkillPanel/SkillIcon");
            _skillName          = Find<Text>("Content/L/SkillPanel/ImageBack/Text");

            _titleTxt           = Find<Text>("Content/R/Title/Text");
            _catList            = Find<UIList>("Content/R/CatList");
            _noCatTipsGO        = Find("Content/R/NoCatTips");
            _catTipsGO          = Find("Content/R/CatTips");
            _closeBtn           = Find<Button>("Content/R/CloseBtn");
            _startBtn           = Find<Button>("Content/R/StartBtn");
            _energyCostText     = Find<Text>("Content/R/StartBtn/Start/Image/Text");

            _energyFly          = Find<RectTransform>("Content/R/energyFly");
            _energyStartPoint   = Find<RectTransform>("Content/R/energyFlyStart");
            _energyEndPoint     = Find<RectTransform>("Content/R/energyFlyEnd");
            _energyFly.gameObject.SetActive(false);

            _startBtn.onClick.AddListener(OnStartBtnClick);
            _closeBtn.onClick.AddListener(OnCloseBtnCLick);

            _collectGoalList.SetRenderHandler(GoalListRenderHandler);
            _catList.SetRenderHandler(CatListRenderHandler);
            _catList.SetSelectHandler(CatListSelectHandler);

            for (int i = 0; i < 3; i++)
            {
                _stars[i] = transform.Find("Content/R/star/star").GetChild(i).GetComponent<KUIImage>();
            }

            isStart = false;
        }

        public void RefreshView()
        {
            UpdateTargetWindow();
            UpdateCat();
            ShowTips();
            ShowStars();
            //ShowProps();
        }

        public void UpdateTargetWindow()
        {
            if (m3Data.gameTarget == 1)
            {
                _collectGoalGO.SetActive(true);
                _scoreGoalGO.SetActive(false);

                List<ElementXDM> goalList = new List<ElementXDM>();
                if (levelData != null && _collectGoalList != null)
                {
                    if (m3Data != null)
                    {
                        foreach (var id in m3Data.LevelTaskElementDic.Keys)
                        {
                            goalList.Add(XTable.ElementXTable.GetByID(id));
                        }
                    }
                }
                _collectGoalList.DataArray = goalList;
            }
            else
            {
                _collectGoalGO.SetActive(false);
                _scoreGoalGO.SetActive(true);
                _scoreGoalText.text = m3Data.score.ToString();
            }
            _titleTxt.text = levelData.Name;
        }

        private void UpdateCat()
        {
            List<CatDataVO> catList = new List<CatDataVO>();
            List<CatDataVO> allCats = CatDataModel.Instance.GetAllCatDataVO();
            foreach (var item in allCats)
            {
                if (levelData.IgnoreCatID.Contains(item.mCatXDM.ID))
                    continue;
                catList.Add(item);
            }
            catList.Sort(CatDataModel.Instance.MatchSortItem);

            _catList.DataArray = catList;

            int index = 0;
            int lastSelectCatID = PlayerPrefs.GetInt("LastSelectCatID" + PlayerDataModel.Instance.mPlayerData.mPlayerID, -1);
            if (lastSelectCatID > -1)
            {
                for (int i = 0; i < catList.Count; i++)
                {
                    if (catList[i].mCatInfo.Id == lastSelectCatID)
                    {
                        index = i;
                        break;
                    }
                }
            }
            _catList.SelectedIndex = index;

            bool hasCat = (allCats.Count != 0);

            _skillInfoGO.SetActive(hasCat);
            _catInfoGO.SetActive(hasCat);
            _noCatGO.SetActive(!hasCat);
            _noCatTipsGO.SetActive(!hasCat);
            _catTipsGO.SetActive(!hasCat);
        }

        private void ShowStars()
        {
            for (int i = 0; i < _stars.Length; i++)
            {
                _stars[i].ShowGray(!(levelData.CurrStar > i));
            }
        }

        private void ShowTips()
        {
            _energyCostText.text = "-" + levelData.NeedPower;
        }

        //private void ShowProps()
        //{
        //    _propItemPool.Clear();
        //    selectPropList.Clear();
        //    for (int i = 0; i < propList.Length; i++)
        //    {
        //        var ele = _propItemPool.SpawnElement();
        //        var item = ele.GetComponent<SelectPropItem>();

        //        if (item != null)
        //        {
        //            selectPropList.Add(item);
        //            item.Show(propList[i], propShopList[i]);
        //        }
        //    }
        //}

        private void GoalListRenderHandler(UIListItem item, int index)
        {
            ElementXDM vo = item.dataSource as ElementXDM;
            if (vo != null)
            {
                item.GetComp<Image>("Icon").overrideSprite = KIconManager.Instance.GetMatch3ElementIcon(vo.Icon);
            }
        }

        private void CatListRenderHandler(UIListItem item, int index)
        {
            CatDataVO vo = item.dataSource as CatDataVO;
            if (vo != null)
            {
                item.GetComp<Image>("Icon").overrideSprite = vo.mCatXDM.GetIconSprite();
                item.GetComp<KUIImage>("Level").ShowSprite(vo.mCatXDM.Rarity - 1);
                item.GetComp<KUIImage>("Light").ShowSprite(vo.mCatXDM.Rarity - 1);
                item.GetGameObject("Light").SetActive(vo.mCatXDM.Rarity > 1 && vo.mCatXDM.Rarity < 5);
            }
        }

        private void CatListSelectHandler(UIListItem item, int index)
        {
            if (item == null) return;

            CatDataVO vo = item.dataSource as CatDataVO;
            currentSelect = vo;
            if (vo != null)
            {
                _catLvText.text = vo.mCatInfo.Level.ToString();
                _goldBonusText.text = "0";
                _scoreBonusText.text = vo.mCatInfo.MatchAbility.ToString();

                if (vo.mCatXDM.SkillId > 0)
                {
                    SkillXDM skill = XTable.SkillXTable.GetByID(vo.mCatXDM.SkillId);
                    _skillInfoGO.SetActive(true);
                    _skillIcon.overrideSprite = KIconManager.Instance.GetSkillIcon(skill.ID);
                    _skillName.text = skill.Name;
                    //_skillName.text = KLocalization.GetLocalString(skill.NameID);
                }
            }

            _catInfoGO.SetActive(vo != null);
            _skillInfoGO.SetActive(vo != null && vo.mCatXDM.SkillId > 0);

            StartCoroutine(RefreshViewCat(vo));
        }

        private IEnumerator RefreshViewCat(CatDataVO cat)
        {
            yield return null;

            if (catModel != null)
            {
                GameObject.Destroy(catModel);
            }

            GameObject obj = CatUtils.GetUIModel(cat.mCatXDM.ID);
            if (obj)
            {
                catModel = obj;
                obj.gameObject.layer = _modelParent.gameObject.layer;
                obj.transform.SetParent(_modelParent.transform, false);
            }
        }

        public void PlayEnergyFlyAnimation(Action action)
        {
            _energyFly.SetParent(_energyStartPoint, false);
            _energyFly.gameObject.SetActive(true);
            _energyFly.localScale = Vector3.one;
            _energyFly.localPosition = Vector3.zero;
            _energyFly.SetParent(_energyEndPoint,true);
            KTweenUtils.ScaleTo(_energyFly, new Vector3(0.5f, 0.5f, 0.5f), 0.6f);
            KTweenUtils.DoRectTransformPos(_energyFly, Vector2.zero, 0.6f, delegate() {

                KTweenUtils.ScaleTo(_startBtn.transform, new Vector3(1.2f, 1.2f, 1.2f), 0.1f, delegate ()
                {
                    KTweenUtils.ScaleTo(_startBtn.transform, new Vector3(0.9f, 0.9f, 0.9f), 0.07f, delegate ()
                    {
                        _startBtn.transform.localScale = Vector3.one;
                        action?.Invoke();
                    });
                });
                KTweenUtils.ScaleTo(_energyFly, new Vector3(1.2f, 1.2f, 1.2f), 0.1f, delegate ()
                {
                    KTweenUtils.ScaleTo(_energyFly, new Vector3(0.9f, 0.9f, 0.9f), 0.07f, delegate ()
                    {
                        _energyFly.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    });
                });
            });
        }

    }
}
