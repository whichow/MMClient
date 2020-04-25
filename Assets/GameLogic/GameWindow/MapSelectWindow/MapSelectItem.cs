using Game.DataModel;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class MapSelectItem : MonoBehaviour
    {
        private Text _levelNum;
        private GameObject _starRoot;
        private GameObject _userFlag;
        private RectTransform _flagRectTrans;
        private Image _userIcon;
        private KUIImage _curBtnIcon;
        private KUIImage _unCurBtnIcon;
        private KUIImage[] _stars = new KUIImage[3];
        private Button _clickBtn;

        private LevelXDM lvData;
        private bool flag = true;
        private bool animationLock;

        private void Awake()
        {
            _levelNum = transform.Find("btn/mapIndex").GetComponent<Text>();
            _starRoot = transform.Find("star").gameObject;
            _userFlag = transform.Find("userIcon").gameObject;
            _flagRectTrans = _userFlag.GetComponent<RectTransform>();

            Canvas canvas = _userFlag.AddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = _levelNum.canvas.sortingOrder + 1;

            _userIcon = _userFlag.transform.Find("icon").GetComponent<KUIImage>();
            _curBtnIcon = transform.Find("btn/icon_b").gameObject.GetComponent<KUIImage>();
            _unCurBtnIcon = transform.Find("btn/icon_y").gameObject.GetComponent<KUIImage>();

            for (int i = 0; i < _starRoot.transform.childCount; i++)
            {
                _stars[i] = _starRoot.transform.GetChild(i).GetComponent<KUIImage>();
            }

            _clickBtn = transform.Find("btn").GetComponent<Button>();
            if (_clickBtn != null)
                _clickBtn.onClick.AddListener(OnBtnClick);
        }

        private void Update()
        {
            if (!animationLock)
                PlayUserIconAnimation();
        }

        public void Init(LevelXDM level)
        {
            lvData = level;
            Refresh();
        }

        public void Refresh()
        {
            animationLock = false;
            HeadIconUtils.SetHeadIcon(PlayerDataModel.Instance.mPlayerData.mHead, PlayerDataModel.Instance.mPlayerData.mPlayerID, _userIcon);
            _levelNum.text = (lvData.LevelIndex).ToString();
            _userFlag.SetActive(false);
            if (lvData.Unlocked)
            {
                if (lvData.ID == LevelDataModel.Instance.CurrMaxLevelId)
                {
                    _curBtnIcon.gameObject.SetActive(true);
                    _unCurBtnIcon.gameObject.SetActive(false);
                }
                else
                {
                    _curBtnIcon.gameObject.SetActive(false);
                    _unCurBtnIcon.gameObject.SetActive(true);
                }
            }
            else
            {
                _curBtnIcon.gameObject.SetActive(false);
                _unCurBtnIcon.gameObject.SetActive(true);
                _unCurBtnIcon.ShowGray(true);
            }
            for (int i = 0; i < 3; i++)
            {
                _stars[i].ShowGray(!(lvData.CurrStar > i));
            }
        }

        public void ShowUser()
        {
            _userFlag.SetActive(true);
            LevelDataModel.Instance.userCurrentLevelID = lvData.ID;
        }

        public void PlayJumpNextLevel()
        {
            _userFlag.SetActive(true);
            animationLock = true;
            var next = KUIWindow.GetWindow<MapSelectWindow>().itemDic[lvData.NextLevelID];
            _userFlag.transform.SetParent(next.transform);
            KTweenUtils.LocalMoveTo(_userFlag.transform, Vector3.zero, 1f, delegate ()
            {
                if (this.isActiveAndEnabled)
                {
                    _userFlag.transform.SetParent(this.transform);
                    _userFlag.transform.localPosition = Vector3.zero;
                    _userFlag.SetActive(false);
                    next.Refresh();
                    next.ShowUser();
                    animationLock = false;
                }
            });
        }

        private void PlayUserIconAnimation()
        {
            while (flag)
            {
                flag = false;
                if (!this.isActiveAndEnabled)
                {
                    break;
                }
                KTweenUtils.DoRectTransformPosY(_flagRectTrans, 10, 0.5f, delegate ()
                {
                    if (this.isActiveAndEnabled)
                    {
                        KTweenUtils.DoRectTransformPosY(_flagRectTrans, 0, 0.5f, delegate ()
                        {
                            if (this.isActiveAndEnabled)
                            {
                                flag = true;
                            }
                        });
                    }
                });
            }
        }

        private void OnBtnClick()
        {
            if (lvData.Unlocked && !KUIWindow.GetWindow<MapSelectWindow>().itemLock)
            {
                KUIWindow.OpenWindow<LevelInfoWindow>(new LevelInfoData(lvData));
            }
        }

    }
}
