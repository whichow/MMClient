using Game.DataModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public partial class MapSelectWindow
    {
        public GameObject _enterBtnGO { get; private set; }
        private GameObject _mapRootGO;
        private ScrollRect _scrollRect;

        private Text _goldText;
        private Text _diamondText;
        private Text _energyText;
        private Text _timeAdd;

        private Button _addGoldBtn;
        private Button _addDiamondBtn;
        private Button _addEnergyBtn;
        private Button _backBtn;

        private Button _cloudUnlockBtn;
        private GameObject _cloudGO;


        public Dictionary<int, MapSelectItem> itemDic;
        public List<GameObject> tmpCloud = new List<GameObject>();

        private float lockPer = 0;
        private SgTimer _sgTimer;
        private int _time;


        public void InitView()
        {
            _enterBtnGO     = Find("MapScroll/stageBtn");
            _mapRootGO      = Find("MapScroll/Viewport/Content");
            _scrollRect     = Find<ScrollRect>("MapScroll");

            _goldText       = Find<Text>("TopRight/Coin/Text");
            _diamondText    = Find<Text>("TopRight/Stone/Text");
            _energyText     = Find<Text>("TopRight/Food/Text");
            _timeAdd        = Find<Text>("TopRight/Food/TimeToAdd");
            _addGoldBtn     = Find<Button>("TopRight/Coin/Button");
            _addDiamondBtn  = Find<Button>("TopRight/Stone/Button");
            _addEnergyBtn   = Find<Button>("TopRight/Food/Button");
            _backBtn        = Find<Button>("BackButton");
            _cloudUnlockBtn = Find<Button>("cloud/ImageCloud/btn");
            _cloudGO        = Find("cloud");

            _addGoldBtn.onClick.AddListener(OnAddGlodClick);
            _addDiamondBtn.onClick.AddListener(OnAddDiamondClick);
            _addEnergyBtn.onClick.AddListener(OnEnergyClick);
            _backBtn.onClick.AddListener(OnBackClick);
            _cloudUnlockBtn.onClick.AddListener(OnUnlockClick);
            _cloudGO.SetActive(false);
        }

        private void RefreshView()
        {
            _timeAdd.gameObject.SetActive(false);
            GenMap();
            ShowUserInfo();
            AdjustScroll();
            SetCloud();
            StartCoroutine(OpenLevelInfoWindow());

            OnPlayerRefresh();
            if (PlayerDataModel.Instance.mPlayerData.mSpirit < KUser.SelfPlayer.maxPower)
            {
                _timeAdd.text = Utils.GetCountTimes(_time);
                _timeAdd.gameObject.SetActive(true);
                if (_sgTimer == null)
                    _sgTimer = SNTimer.SetTimeInterval(OnTime, 1000);
            }
            else
            {
                _timeAdd.gameObject.SetActive(false);
                if (_sgTimer != null)
                {
                    _sgTimer.Stop();
                    _sgTimer = null;
                }
            }
        }

        private void OnPlayerRefresh()
        {
            _goldText.text = PlayerDataModel.Instance.mPlayerData.mGold.ToString();
            _diamondText.text = PlayerDataModel.Instance.mPlayerData.mDiamond.ToString();
            _energyText.text = PlayerDataModel.Instance.mPlayerData.mSpirit + "/" + KUser.SelfPlayer.maxPower;
            _time = PlayerDataModel.Instance.mPlayerData.NextStaminaRemainSecs;
        }

        private void OnTime(object args, uint count)
        {
            if (_time > 0)
            {
                _time--;
                _timeAdd.text = Utils.GetCountTimes(_time);
            }
            else
            {
                GameApp.Instance.GameServer.ReqPlayerInfo();
            }
        }

        private void AdjustScroll()
        {
            if (mapData.currentLevel == null && mapData.nextLevel == null)
            {
                lockPer = 1;
            }
            else if (mapData.currentLevel != null && mapData.nextLevel == null)
            {
                int count = chapters.Count;
                int index = -1;

                for (int i = 0; i < count; i++)
                {
                    if (chapters[i].ID <= LevelDataModel.Instance.GetChapterByLevelID(mapData.currentLevel.ID).ID)
                    {
                        index = i;
                    }
                }
                lockPer = (float)index / (count - 1);
            }
            else if (mapData.currentLevel != null && mapData.nextLevel != null)
            {
                if (LevelDataModel.Instance.GetChapterByLevelID(mapData.nextLevel.ID).unlocked)
                {
                    int count = chapters.Count;
                    int index = -1;

                    for (int i = 0; i < count; i++)
                    {
                        if (chapters[i] == LevelDataModel.Instance.GetChapterByLevelID(mapData.nextLevel.ID))
                        {
                            index = i;
                        }
                    }
                    lockPer = (float)index / (count - 1);
                }
            }
            else
            {
                lockPer = 1;
            }
            _scrollRect.horizontalNormalizedPosition = lockPer;
        }

        private void DoScrollMove(float from, float to, float duation)
        {
            _scrollRect.horizontal = false;
            itemLock = true;
            _scrollRect.horizontalNormalizedPosition = from;
            KTweenUtils.DoFloat(from, to, duation, delegate (float value)
            {
                if (this.active)
                {
                    _scrollRect.horizontalNormalizedPosition = value;
                    if (value == to)
                    {
                        itemLock = false;
                        _scrollRect.horizontal = true;
                    }
                }
            });
        }

        private void SetCloud()
        {
            if (_cloudGO)
            {
                int id = LevelDataModel.Instance.GetChapterByLevelID(LevelDataModel.Instance.CurrUnlockMaxLevelId).ID;

                if (mapDic.ContainsKey(id))
                {
                    GameObject obj = GameObject.Instantiate(_cloudGO);
                    _cloudUnlockBtn = obj.transform.Find("ImageCloud/btn").GetComponent<Button>();
                    _cloudUnlockBtn.onClick.AddListener(OnUnlockClick);
                    tmpCloud.Add(obj);
                    obj.transform.SetParent(mapDic[id].transform, false);
                    obj.SetActive(true);
                }
            }
        }

        private void GenMap()
        {
            int count = chapters.Count;
            for (int i = 0; i < count; i++)
            {
                int index = i;
                LoadMapPrefab(chapters[index].ID);
            }
            _scrollRect.content.anchoredPosition = Vector2.zero;
        }

        private void GenLvBtnItem(MapItem item, int chapterID)
        {
            var levels = XTable.LevelXTable.GetAllLevelsBytChapterID(chapterID);
            if (levels != null)
            {
                for (int i = 0; i < levels.Count; i++)
                {
                    item.AddBtnObj(levels[i]);
                    itemDic.Add(levels[i].ID, item.item);
                }
            }
        }

        private void LoadMapPrefab(int chapterID)
        {
            GameObject prefab;
            GameObject tmp;
            ChapterUnlockXDM chapter = XTable.ChapterUnlockXTable.GetByID(chapterID);
            if (KAssetManager.Instance.TryGetUIPrefab("MapPrefab/" + chapter.UIPrefabName, out prefab))
            {
                if (prefab)
                {
                    MapItem item;
                    tmp = GameObject.Instantiate((GameObject)prefab);
                    (tmp).transform.SetParent(_mapRootGO.transform, false);
                    item = tmp.AddComponent<MapItem>();
                    mapDic.Add(chapter.ID, item);
                    item.Init(chapter);
                    GenLvBtnItem(item, chapter.ID);
                }
                else
                {
                    Debug.Log("找不到章节资源 " + chapter.UIPrefabName);
                }
            };
        }

        private void ShowUserInfo()
        {
            LevelXDM level = null;
            foreach (var item in itemDic)
            {
                level = XTable.LevelXTable.GetByID(item.Key);
                if (mapData.currentLevel != null && mapData.nextLevel != null)
                {
                    if (level.ID == mapData.currentLevel.ID
                        && LevelDataModel.Instance.GetChapterByLevelID(mapData.nextLevel.ID).unlocked
                        && mapData.nextLevel.ID == LevelDataModel.Instance.CurrMaxLevelId)
                    {
                        if (currentChapter.ID == LevelDataModel.Instance.GetChapterByLevelID(level.NextLevelID).ID)
                        {
                            if (LevelDataModel.Instance.userCurrentLevelID != mapData.nextLevel.ID)
                            {
                                item.Value.ShowUser();
                                item.Value.PlayJumpNextLevel();
                            }
                            else
                            {
                                itemDic[mapData.nextLevel.ID].ShowUser();
                            }
                        }
                        else
                        {
                            item.Value.ShowUser();
                        }
                    }
                    else if (level.ID == LevelDataModel.Instance.CurrMaxLevelId)
                    {
                        item.Value.ShowUser();
                    }
                }
                else if (level.ID == LevelDataModel.Instance.CurrMaxLevelId)
                {
                    item.Value.ShowUser();
                }
            }
        }

        IEnumerator OpenLevelInfoWindow()
        {
            float time = (mapData.currentLevel != null && mapData.nextLevel != null && LevelDataModel.Instance.userCurrentLevelID != mapData.nextLevel.ID ? 1.5f : 0f);

            yield return new WaitForSeconds(time);
            if (mapData != null)
            {
                if (mapData != null && mapData.currentLevel != null && mapData.showInfoWindow)
                {
                    LevelXDM level = mapData.nextLevel != null ? mapData.nextLevel : mapData.currentLevel;

                    //if (currentChapter.chapterID == level.chapterID)
                    if (XTable.ChapterUnlockXTable.GetByID(level.ChapterID).unlocked)
                    {
                        if (mapData.currentLevel != null && mapData.nextLevel == null)
                        {
                            OpenWindow<LevelInfoWindow>(new LevelInfoData(level));
                        }
                        else if (mapData.nextLevel != null && mapData.currentLevel != null)
                        {
                            OpenWindow<LevelInfoWindow>(new LevelInfoData(level));
                        }
                    }
                    else
                    {
                        OpenWindow<MessageBox>(new MessageBox.Data()
                        {
                            content = "请先解锁",
                            onConfirm = () => openBox(),
                        });
                    }
                }
            }
        }

        private void openBox()
        {

        }

    }
}
