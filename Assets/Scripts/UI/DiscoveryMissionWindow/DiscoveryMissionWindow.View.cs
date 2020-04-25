
using System;
/** 
*FileName:     DiscoveryMissionWindow.View.cs 
*Author:       LiMuChen 
*Version:      1.0 
*UnityVersion：5.6.3f1
*Date:         2017-10-23 
*Description:    
*History: 
*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public partial class DiscoveryMissionWindow
    {
        #region Field  

        private Button _closeButton;

        private List<CatDataVO> catList;

        private Text _textSuccesRate;
        private Text _textBonusTimes;
        private Transform[] _transCats;

        private GameObject _textConditionTopGO;
        private GameObject _textConditionBtmGO;
        private Text _textConditionTop;
        private Text _textConditionBtm;
        private Button _startButton;
        private Button[] _buttons;
        private KExplore.Task task;
        private Transform _transevent1;
        private Transform _transevent2;
        #endregion

        #region Method

        public void InitView()
        {
            catList = new List<CatDataVO>();
            _closeButton = Find<Button>("MissionBack/Close");
            _closeButton.onClick.AddListener(this.OnCloseBtnClick);
            _textSuccesRate = Find<Text>("MissionBack/MissionLeft/Text1/Text");
            _textBonusTimes = Find<Text>("MissionBack/MissionLeft/Text2/Text");
            _textConditionTopGO = Find<Transform>("MissionBack/MissionRight/Item1").gameObject;
            _textConditionBtmGO = Find<Transform>("MissionBack/MissionRight/Item2").gameObject;
            _textConditionTop = Find<Text>("MissionBack/MissionRight/Item1/Label");
            _textConditionBtm = Find<Text>("MissionBack/MissionRight/Item2/Label");
            _transCats = new Transform[3];
            _buttons = new Button[3];
            for (int i = 0; i < _transCats.Length; i++)
            {
                catList.Add(null);
                _transCats[i] = Find<Transform>("MissionBack/CatGrid/Item" + (i + 1));
                _buttons[i] = Find<Button>("MissionBack/CatGrid/Item" + (i + 1));
            }
            _buttons[0].onClick.AddListener(OnButton1);
            _buttons[1].onClick.AddListener(OnButton2);
            _buttons[2].onClick.AddListener(OnButton3);
            _startButton = Find<Button>("MissionBack/Button");
            _startButton.onClick.AddListener(OnStartBtnClick);
            _transevent1 = Find<Transform>("MissionBack/MissionRight/Item1/Background/Checkmark");
            _transevent2 = Find<Transform>("MissionBack/MissionRight/Item2/Background/Checkmark");
            RefreshView();

        }

        public void RefreshView()
        {
            _transevent1.gameObject.SetActive(false);
            _transevent2.gameObject.SetActive(false);
            task = data as KExplore.Task;
            var cats = KCatManager.Instance.allCats;
            //OpenWindow<ChooseCatWindow>(GetCat(cats, task));
            var cat = task.catIds;
            _textConditionTopGO.SetActive(task.catConditions.Count > 0);
            _textConditionBtmGO.SetActive(task.catConditions.Count > 1);
            if (task.catConditions.Count > 0)
            {
                RefreshText(_textConditionTop, task.catConditions[0]);
            }
            if (task.catConditions.Count > 1)
            {
                RefreshText(_textConditionBtm, task.catConditions[1]);
            }
            RefreshCats();
        }

        private List<CatDataVO> GetCat(List<CatDataVO> allCats, List<KExplore.Condition> conditions, int idx)
        {
            List<CatDataVO> tempCatList;
            List<CatDataVO> allCatList = new List<CatDataVO>();
            allCatList.AddRange(allCats);
            for (int i = 0; i < conditions.Count; i++)
            {
                int value = Convert.ToInt32(conditions[i].values[0]);
                switch (conditions[i].type)
                {
                    case 1:
                        tempCatList = new List<CatDataVO>();
                        for (int j = 0; j < allCatList.Count; j++)
                        {
                            if (allCatList[j].mCatInfo.Level >= value)
                            {
                                tempCatList.Add(allCatList[j]);
                            }
                        }
                        allCatList = tempCatList;
                        break;
                    case 2:
                        tempCatList = new List<CatDataVO>();
                        for (int j = 0; j < allCatList.Count; j++)
                        {
                            if (allCatList[j].mCatXDM.Rarity >= value)
                            {
                                tempCatList.Add(allCatList[j]);
                            }

                        }
                        allCatList = tempCatList;
                        break;
                    case 3:
                        tempCatList = new List<CatDataVO>();
                        for (int j = 0; j < allCatList.Count; j++)
                        {
                            if (allCatList[j].mCatInfo.Star>= value)
                            {
                                tempCatList.Add(allCatList[j]);
                            }
                        }
                        allCatList = tempCatList;
                        break;
                    case 4:
                        tempCatList = new List<CatDataVO>();
                        for (int j = 0; j < allCatList.Count; j++)
                        {
                            for (int k = 0; k < conditions[i].values.Count; k++)
                            {
                                if (allCatList[j].mCatXDM.MainColor == Convert.ToInt32(conditions[i].values[k]) || allCatList[j].mCatXDM.ContainColor(Convert.ToInt32(conditions[i].values[k])))
                                {
                                    if (!tempCatList.Contains(allCatList[j]))
                                    {
                                        tempCatList.Add(allCatList[j]);
                                    }
                                }
                            }
                        }
                        allCatList = tempCatList;
                        break;
                    case 5:
                        break;
                    default:
                        break;
                }
            }
            for (int j = 0; j < catList.Count; j++)
            {
                if (catList[j] != null)
                {
                    allCatList.Remove(catList[j]);
                }
            }
            allCatList.Sort(CatDataModel.Instance.ExploreSortItem);
            return allCatList;
        }

        private void RemoveCat(int idx)
        {
            catList[idx] = null;
            RefreshCats();
            _transCats[idx].Find("CardBig/Cat/Del").GetComponent<Button>().onClick.RemoveAllListeners();
        }
        private void RefreshCats()
        {
            KUIImage _imageFlag;
            KUIImage _imageFram;
            KUIImage[] _starImages;
            KUIImage _imageColor;
            for (int i = 0; i < _transCats.Length; i++)
            {
                if (catList[i] != null)
                {
                    _transCats[i].Find("CardBig/Empty").gameObject.SetActive(false);
                    _transCats[i].Find("CardBig/Cat").GetComponent<Image>().overrideSprite = catList[i].mCatXDM.GetPhotoSprite();
                    _imageFlag = _transCats[i].Find("CardBig/Cat/Level").GetComponent<KUIImage>();
                    _imageFram = _transCats[i].Find("CardBig/Cat/Light").GetComponent<KUIImage>();
                    _transCats[i].Find("CardBig/Cat/Level/Text").GetComponent<Text>().text = catList[i].mCatInfo.Level.ToString();
                    _imageColor = _transCats[i].Find("CardBig/Cat/Fish").GetComponent<KUIImage>();
                    ShowColor(_imageColor, catList[i].mCatXDM.MainColor);
                    var fish = _transCats[i].Find("CardBig/Cat/Fish");
                    _starImages = new KUIImage[fish.childCount];
                    if (string.IsNullOrEmpty(catList[i].mNickName))
                    {
                        _transCats[i].Find("CardBig/Cat/Name").GetComponent<Text>().text = catList[i].mName;
                    }
                    else
                    {
                        _transCats[i].Find("CardBig/Cat/Name").GetComponent<Text>().text = catList[i].mNickName;
                    }
                    _transCats[i].Find("CardBig/Cat/Item/Text").GetComponent<Text>().text = catList[i].mCatInfo.ExploreAbility.ToString();
                    for (int j = 0; j < _starImages.Length; j++)
                    {
                        _starImages[j] = fish.GetChild(j).GetComponent<KUIImage>();
                    }
                    ShowStar(_starImages, catList[i].mCatInfo.Star);
                    ShowRarity(_imageFlag, _imageFram, catList[i].mCatXDM.Rarity);

                    _transCats[i].Find("CardBig/Cat").gameObject.SetActive(true);
                }
                else
                {
                    _transCats[i].Find("CardBig/Cat").gameObject.SetActive(false);
                    _transCats[i].Find("CardBig/Empty").gameObject.SetActive(true);
                }
            }
            _textSuccesRate.text = task.CalcTaskSuccessChance(catList).ToString();
            _textBonusTimes.text = task.CalcTaskRewardMultiple(catList).ToString() + "%";
        }

        public void RefreshText(Text text, KExplore.Condition condition)
        {
            if (condition.values.Count == 0 && condition.values == null)
            {
                return;
            }
            switch (condition.type)
            {
                case 1:
                    text.text = string.Format(condition.description, condition.values[0]);
                    break;
                case 2:
                    text.text = string.Format(condition.description, condition.values[0]);
                    break;
                case 3:
                    text.text = string.Format(condition.description, condition.values[0]);
                    break;
                case 4:
                    string retStr = string.Empty; ;
                    //for (int i = 0; i < condition.values.Count; i++)
                    //{
                    //    retStr += ret += string.Format(condition[i].description, _task.catConditions[i].GetFormatValues()) + "\n";
                    //}
                    text.text = /*"颜色" +*/ string.Format(condition.description, condition.GetFormatValues());
                    break;
                case 5:
                    text.text = "猫咪>=" + Convert.ToInt32(condition.values[0]);
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
        public void ShowStar(KUIImage[] _starImages, int star)
        {
            for (int i = 0; i < _starImages.Length; i++)
            {
                _starImages[i].ShowGray(i >= star);
            }
        }
        public void ShowColor(KUIImage _colorImage, int color)
        {
            if (color == (int)KCat.Color.fRed)
            {
                _colorImage.ShowSprite(0);
            }
            else if (color == (int)KCat.Color.fYellow)
            {
                _colorImage.ShowSprite(1);
            }
            else if (color == (int)KCat.Color.fBlue)
            {
                _colorImage.ShowSprite(3);
            }
            else if (color == (int)KCat.Color.fGreen)
            {
                _colorImage.ShowSprite(2);
            }
            else if (color == (int)KCat.Color.fPurple)
            {
                _colorImage.ShowSprite(5);
            }
            else if (color == (int)KCat.Color.fBrown)
            {
                _colorImage.ShowSprite(4);
            }
        }
        public override void OnDisable()
        {
            for (int i = 0; i < catList.Count; i++)
            {
                catList[i] = null;
            }
            RefreshCats();
        }
        #endregion
    }
}

