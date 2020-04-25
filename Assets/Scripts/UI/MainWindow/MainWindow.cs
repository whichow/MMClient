// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "MainWindow" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using Game.Build;
using UnityEngine;

namespace Game.UI
{
    partial class MainWindow : KUIWindow
    {

        #region Constructor

        public MainWindow() :
            base(UILayer.kBackground, UIMode.kSequenceHide)
        {
            uiPath = "MainWindow";
        }

        #endregion

        #region Action       
        public bool isOpen
        {
            get
            {
                return _showMajor;
            }
        }
        public System.Action buildingInit;
        /// <summary>
        /// 供外部使用 （展开或者关闭菜单）
        /// </summary>
        public void MajorBtnClick(bool isShow = true)
        {
            //_hLayoutGroup.gameObject.SetActive(isShow);
            //_vLayoutGroup.gameObject.SetActive(isShow);
            if (_showTime > 0)
            {
                return;
            }
            if (!isShow && _showMajor)
            {
                _showTime = _showDuration;
                //_showMajor = isShow;
            }

            if (isShow && !_showMajor)
            {
                _showTime = _showDuration;
                _hLayoutGroup.gameObject.SetActive(true);
                _vLayoutGroup.gameObject.SetActive(true);
            }
        }
        //bool bl = true;
        private void OnMajorBtnClick()
        {
            KUIWindow.CloseWindow<FunctionWindow>();

            if (_showTime > 0)
            {
                return;
            }
            _showTime = _showDuration;

            if (_showMajor)
            {

            }
            else
            {
                _hLayoutGroup.gameObject.SetActive(true);
                _vLayoutGroup.gameObject.SetActive(true);
            }
        }

        private void OnMajorHBtn1Click()
        {
            KUIWindow.OpenWindow<FriendWindow>();
        }

        private void OnMajorHBtn2Click()
        {
            KUIWindow.OpenWindow<ShopWindow>(ShopIDConst.Special);
        }

        private void OnMajorHBtn3Click()
        {
            OpenWindow<BagWindow>();
        }

        private void OnMajorHBtn4Click()
        {
            if (Map.Instance.CurControlMode != Map.ControlMode.kEditor && Map.Instance.CurrMapObject == null)
            {
                KUIWindow.OpenWindow<BuildingShopWindow>();
            }
        }

        private void OnMajorHBtn5Click()
        {
            OpenWindow<HandBookWindow>();
        }

        private void OnMajorVBtn1Click()
        {
            OpenWindow<TaskWindow>();
        }

        private void OnMajorVBtn2Click()
        {
            KUIWindow.OpenWindow<CatWindow>(CatOpenType.Normal);
        }

        private void OnChatBtnClick()
        {
            //OpenWindow<ChatWindow>();
        }

        private void OnHomeBtnClick()
        {
            togglePlayerView(false);
        }

        private void OnMinorBtn1Click()
        {

        }
        private void AddCoinClick()
        {
            KUIWindow.OpenWindow<ShopWindow>(ShopIDConst.Special);
        }
        private void AddGemClick()
        {
            KUIWindow.OpenWindow<ShopWindow>(ShopIDConst.Diamond);
        }
        private void AddFeedClick()
        {
            KUIWindow.OpenWindow<MapSelectWindow>();
            //KUIWindow.OpenWindow<ShopWindow>(ShopIDConst.Special);
        }

        private void OnMinorBtn2Click()
        {
            OpenWindow<RankWindow>();
        }

        private void OnMinorBtn3Click()
        {
            KUIWindow.OpenWindow<MailWindow>();
        }

        private void OnMinorBtn4Click()
        {
            OpenWindow<ActivityWindow>();
        }

        private void OnHeadBtnClick()
        {
            KUIWindow.OpenWindow<SpaceWindow>();
        }

        private void OnFriendCallBack(int code, string message, object data)
        {
            OpenWindow<DiscoveryWindow>();
        }

        private void OnShortcut1Click()
        {
            KUIWindow.OpenWindow<PhotoShopWindow>();
        }
        private void OnShortcut2Click()
        {
            KUIWindow.OpenWindow<MapSelectWindow>();
        }

        #endregion

        #region Method
        /// <summary>
        /// 收起主界面菜单列表
        /// </summary>
        public void FoldMajor()
        {
            if (_showMajor)
            {
                if (_showTime > 0)
                {
                    return;
                }
                _showTime = _showDuration;
            }
        }

        public override void Awake()
        {
            InitView();

            _showMajor = true;
        }

        public override void OnEnable()
        {
            RefreshView();
        }

        public override void Update()
        {
            if (_showTime > 0f)
            {
                _showTime = Mathf.Max(0f, _showTime - Time.deltaTime);
                float progress = _showTime / _showDuration;

                var curve = AnimationCurve.EaseInOut(0, 0, 1f, 1f);
                if (_showMajor)
                {
                    progress = curve.Evaluate(progress);
                    _hLayoutGroup.padding.right = (int)Mathf.Lerp(10, _hSourcePadding, progress);
                    _hLayoutGroup.spacing = new Vector2(Mathf.Lerp(_hTargetSpacing, _hSourceSpacing, progress), 0f);

                    _vLayoutGroup.padding.bottom = (int)Mathf.Lerp(10, _vSourcePadding, progress);
                    _vLayoutGroup.spacing = new Vector2(0, Mathf.Lerp(_vTargetSpacing, _vSourceSpacing, progress));
                    if (_showTime <= 0f)
                    {
                        _hLayoutGroup.padding.right = 10;
                        _hLayoutGroup.spacing = new Vector2(_hTargetSpacing, 0f);

                        _vLayoutGroup.padding.bottom = 10;
                        _vLayoutGroup.spacing = new Vector2(0f, _vTargetSpacing);

                        _hLayoutGroup.gameObject.SetActive(false);
                        _vLayoutGroup.gameObject.SetActive(false);
                        _showMajor = false;
                    }
                }
                else
                {
                    progress = 1f - curve.Evaluate(progress);

                    _hLayoutGroup.padding.right = (int)Mathf.Lerp(10, _hSourcePadding, progress);
                    _hLayoutGroup.spacing = new Vector2(Mathf.Lerp(_hTargetSpacing, _hSourceSpacing, progress), 0f);

                    _vLayoutGroup.padding.bottom = (int)Mathf.Lerp(10, _vSourcePadding, progress);
                    _vLayoutGroup.spacing = new Vector2(0f, Mathf.Lerp(_vTargetSpacing, _vSourceSpacing, progress));

                    if (_showTime <= 0f)
                    {
                        _hLayoutGroup.padding.right = _hSourcePadding;
                        _hLayoutGroup.spacing = new Vector2(_hSourceSpacing, 0f);

                        _vLayoutGroup.padding.bottom = _vSourcePadding;
                        _vLayoutGroup.spacing = new Vector2(0f, _vSourceSpacing);
                        _showMajor = true;
                    }
                }
            }
        }

        public void togglePlayer(bool isPlayer, object data = null)
        {
            GetPlayerData();
            togglePlayerView(isPlayer, data);
        }

        public override void UpdatePerSecond()
        {
            //CheckFunction();

            //RefreshView();
            //Debug.Log("-><color=#9400D3>" + "队列消息数量：" + KChatManager.Instance.LstAddRockSystemAnnouncementDatas.Count + "</color>");
            //if (_enum_scrolTextStatus == ScrolTextStatus.Idle && KChatManager.Instance.LstAddRockSystemAnnouncementDatas.Count > 0)
            //{
            //    _data_systemAccouncement = KChatManager.Instance.CarryOffAnnouncementItemByMainViewRock();
            //    if (_data_systemAccouncement != null)
            //    {
            //        NormalizationScrolText();
            //    }
            //}
        }
        #endregion
    }

    public enum ScrolTextStatus
    {
        Idle = 1,
        MoveToScreen = 2,
        PauseToShow = 3,
        MoveOutScreen = 4,
        Running = 5,
    }
}
