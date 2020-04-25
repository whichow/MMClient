// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "MainWindow.View" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using Game.Build;
using Msg.ClientMessage;

using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    partial class MainWindow
    {
        public enum ScaleAnimType
        {
            kNone = -1,
            kGoldNode,
            kDiamontNode,
            kFoodNode,
            kBuildingBagNode,
            kMajorBtn,
            kShopBagNode,
            kCharmNode,
            kGradeNode,
            kCatNode,

        }
        #region Field

        private Button _majorButton;

        private Button _majorHFriend;
        private Button _majorHShop;
        private Button _majorHBag;
        private Button _majorHBuilding;
        private Button _majorHPicture;

        private Button _majorVTask;
        private Button _majorVCat;
        private Button _majorVButton3;

        private Button _minorButton1;
        private Button _minorButton2;
        private Button _minorButton3;
        private Button _minorButton4;

        private Button _headButton;

        private Button _home;

        private Image _playerHeadImage;

        private Text _coinText;
        private Text _stoneText;
        private Text _foodText;
        private Text _gradeText;
        private Text _charmText;
        private Image _expImage;
        private Text _nameText;


        private Button _chatButton;

        private GridLayoutGroup _hLayoutGroup;
        private GridLayoutGroup _vLayoutGroup;

        private Button _shortcut1;
        private Button _shortcut2;

        private GameObject _minor;
        private GameObject _major;
        private GameObject _shortcut;
        private GameObject _otherPlayer;

        public Transform goldNode { get; private set; }
        public Transform diamontNode { get; private set; }
        public Transform foodNode { get; private set; }
        public Transform buildingBagNode { get; private set; }
        public Transform majorBtn { get; private set; }
        public Transform shopBagNode { get; private set; }
        public Transform charmNode { get; private set; }
        public Transform catNode { get; private set; }
        public Transform gradeNode { get; private set; }
        private TweenScl[] tweenSclList;


        private bool _showMajor;
        public bool showMajor
        {
            get
            {
                return _showMajor;
            }
        }

        private float _showTime = 0f;
        private float _showDuration = 0.3f;

        private int _hSourcePadding;
        private int _vSourcePadding;

        private float _hSourceSpacing;
        private float _vSourceSpacing;

        private float _hTargetSpacing;
        private float _vTargetSpacing;

        private bool _isFriend;
        private Button _btnCoinAdd;
        private Button _btnGemAdd;
        private Button _btnFeedAdd;

        //系统通告滚动字幕控制
        private ScrolTextStatus _enum_scrolTextStatus = ScrolTextStatus.Idle;
        private GameObject _go_scrolText;
        private Text _txt_scrolText;
        private GameObject _go_scrolTextParent;
        private AnouncementItem _data_systemAccouncement;
        private TweenPos _twnPs_scrolText;
        #endregion

        #region Method

        private void InitView()
        {
            _majorButton = Find<Button>("Major/Button");
            _majorButton.onClick.AddListener(this.OnMajorBtnClick);

            _majorVTask = Find<Button>("Major/H/Task");
            _majorVTask.onClick.AddListener(this.OnMajorVBtn1Click);
            _majorVCat = Find<Button>("Major/H/Cat");
            _majorVCat.onClick.AddListener(this.OnMajorVBtn2Click);
            _majorHShop = Find<Button>("Major/H/Shop");
            _majorHShop.onClick.AddListener(this.OnMajorHBtn2Click);
            _majorHBag = Find<Button>("Major/H/Bag");
            _majorHBag.onClick.AddListener(this.OnMajorHBtn3Click);
            _majorHBuilding = Find<Button>("Major/H/Building");
            _majorHBuilding.onClick.AddListener(this.OnMajorHBtn4Click);



            _majorHFriend = Find<Button>("Major/V/Friend");
            _majorHFriend.onClick.AddListener(this.OnMajorHBtn1Click);
            _majorHPicture = Find<Button>("Major/V/Picture");
            _majorHPicture.onClick.AddListener(this.OnMajorHBtn5Click);



            _minorButton1 = Find<Button>("Minor/Button1");
            _minorButton1.onClick.AddListener(this.OnMinorBtn1Click);
            _minorButton2 = Find<Button>("Minor/Button2");
            _minorButton2.onClick.AddListener(this.OnMinorBtn2Click);
            _minorButton3 = Find<Button>("Minor/Button3");
            _minorButton3.onClick.AddListener(this.OnMinorBtn3Click);
            _minorButton4 = Find<Button>("Minor/Button4");
            _minorButton4.onClick.AddListener(this.OnMinorBtn4Click);

            _shortcut1 = Find<Button>("Shortcut/Button1");
            _shortcut1.onClick.AddListener(this.OnShortcut1Click);
            _shortcut2 = Find<Button>("Shortcut/Button2");
            _shortcut2.onClick.AddListener(this.OnShortcut2Click);

            _hLayoutGroup = Find<GridLayoutGroup>("Major/H");
            _vLayoutGroup = Find<GridLayoutGroup>("Major/V");

            _hSourcePadding = _hLayoutGroup.padding.right;
            _vSourcePadding = _vLayoutGroup.padding.bottom;
            _hSourceSpacing = _hLayoutGroup.spacing.x;
            _vSourceSpacing = _vLayoutGroup.spacing.y;
            _hTargetSpacing = -_hLayoutGroup.cellSize.x;
            _vTargetSpacing = -_vLayoutGroup.cellSize.y;

            _headButton = Find<Button>("Player/Head");
            _headButton.onClick.AddListener(this.OnHeadBtnClick);

            _playerHeadImage = Find<Image>("Player/Head/Icon");
            _coinText = Find<Text>("Coin/Text");
            _stoneText = Find<Text>("Stone/Text");
            _foodText = Find<Text>("Food/Text");
            _gradeText = Find<Text>("Player/Grade/Text");
            _expImage = Find<Image>("Player/Exp/Mask");
            _nameText = Find<Text>("Player/Name");

            _chatButton = Find<Button>("Chat");
            _chatButton.onClick.AddListener(this.OnChatBtnClick);

            _charmText = Find<Text>("Player/Charm/Text");
            
            _home = Find<Button>("OtherPlayer/Home");
            _home.onClick.AddListener(OnHomeBtnClick);

            _minor = Find("Minor");
            _major = Find("Major");
            _shortcut = Find("Shortcut");
            _otherPlayer = Find("OtherPlayer");

            goldNode = Find<Transform>("Coin/CoinIcon");
            diamontNode = Find<Transform>("Stone/DaimontIcon");
            foodNode = Find<Transform>("Food/FoodIcon");
            buildingBagNode = Find<Transform>("Major/H/Building/Image");

            majorBtn = _majorButton.transform;
            shopBagNode = Find<Transform>("Major/H/Bag/Image");
            gradeNode = Find<Transform>("Player/Grade");
            charmNode = Find<Transform>("Player/Charm");
            catNode = Find<Transform>("Major/H/Cat/Image");
            _btnCoinAdd = Find<Button>("Coin/Button");
            _btnCoinAdd.onClick.AddListener(AddCoinClick);
            _btnGemAdd = Find<Button>("Stone/Button");
            _btnGemAdd.onClick.AddListener(AddGemClick);
            _btnFeedAdd = Find<Button>("Food/Button");
            _btnFeedAdd.onClick.AddListener(AddFeedClick);
            initTweenSclData();

            //系统通告滚动字幕控制
            _enum_scrolTextStatus = ScrolTextStatus.Idle;
            _go_scrolText = Find("SystemAccouncement/Image/Text");
            _txt_scrolText = Find<Text>("SystemAccouncement/Image/Text");
            _twnPs_scrolText = Find<TweenPos>("SystemAccouncement/Image/Text");
            
        }

        public override void AddEvents()
        {
            base.AddEvents();
            PlayerDataModel.Instance.AddEvent(PlayerEvent.ChangeName, OnChangeName);
            PlayerDataModel.Instance.AddEvent(PlayerEvent.ChangeHead, OnChangeHead);
            PlayerDataModel.Instance.AddEvent(PlayerEvent.PlayerDataRefresh, RefreshView);
            BagDataModel.Instance.AddEvent(BagEvent.BagDataRefresh, RefreshView);
        }

        public override void RemoveEvents()
        {
            base.RemoveEvents();
            PlayerDataModel.Instance.RemoveEvent(PlayerEvent.ChangeName, OnChangeName);
            PlayerDataModel.Instance.RemoveEvent(PlayerEvent.ChangeHead, OnChangeHead);
            PlayerDataModel.Instance.RemoveEvent(PlayerEvent.PlayerDataRefresh, RefreshView);
            BagDataModel.Instance.RemoveEvent(BagEvent.BagDataRefresh, RefreshView);
        }

        public List<GameObject> GetFriendBtn()
        {
            var list = new List<GameObject>();
            list.Add(_home.gameObject);
            list.Add(KUIRoot.Instance._popupLayer.gameObject);
            return list;
        }

        private void CheckFunction()
        {
            PlayerDataModel.Instance.mPlayerData.UpdateFunction();

            bool state;
            state = PlayerDataModel.Instance.mPlayerData.GetFunction(FunctionType.kRanking);
            _minorButton2.gameObject.SetActive(state);
            state = PlayerDataModel.Instance.mPlayerData.GetFunction(FunctionType.kMail);
            _minorButton3.gameObject.SetActive(state);
            state = PlayerDataModel.Instance.mPlayerData.GetFunction(FunctionType.kActivity);
            _minorButton4.gameObject.SetActive(state);

            state = PlayerDataModel.Instance.mPlayerData.GetFunction(FunctionType.kCat);
            _majorVCat.gameObject.SetActive(state);
            state = PlayerDataModel.Instance.mPlayerData.GetFunction(FunctionType.kShop);
            _majorHShop.gameObject.SetActive(state);
            state = PlayerDataModel.Instance.mPlayerData.GetFunction(FunctionType.kBag);
            _majorHBag.gameObject.SetActive(state);

            state = PlayerDataModel.Instance.mPlayerData.GetFunction(FunctionType.kBuilding);
            _majorHBuilding.gameObject.SetActive(state);
            state = PlayerDataModel.Instance.mPlayerData.GetFunction(FunctionType.kFriend);
            _majorHFriend.gameObject.SetActive(state);
            state = PlayerDataModel.Instance.mPlayerData.GetFunction(FunctionType.kHandbook);
            _majorHPicture.gameObject.SetActive(state);
            state = PlayerDataModel.Instance.mPlayerData.GetFunction(FunctionType.kMission);
            _majorVTask.gameObject.SetActive(state);

            state = PlayerDataModel.Instance.mPlayerData.GetFunction(FunctionType.kDrawCard);
            _shortcut1.gameObject.SetActive(state);
            state = PlayerDataModel.Instance.mPlayerData.GetFunction(FunctionType.kMatch3);
            _shortcut2.gameObject.SetActive(state);
        }

        private void initTweenSclData()
        {
            tweenSclList = new TweenScl[9];
            tweenSclList[(int)ScaleAnimType.kGoldNode] = goldNode.GetComponent<TweenScl>();
            tweenSclList[(int)ScaleAnimType.kDiamontNode] = diamontNode.GetComponent<TweenScl>();
            tweenSclList[(int)ScaleAnimType.kFoodNode] = foodNode.GetComponent<TweenScl>();
            //tweenSclList[(int)ScaleAnimType.kBuildingBagNode] = buildingBagNode.GetComponent<TweenScl>();
            //tweenSclList[(int)ScaleAnimType.kMajorBtn] = majorBtn.GetComponent<TweenScl>();
            //tweenSclList[(int)ScaleAnimType.kShopBagNode] = shopBagNode.GetComponent<TweenScl>();
            //tweenSclList[(int)ScaleAnimType.kCharmNode] = charmNode.GetComponent<TweenScl>();
            //tweenSclList[(int)ScaleAnimType.kCatNode] = catNode.GetComponent<TweenScl>();
            //tweenSclList[(int)ScaleAnimType.kGradeNode] = gradeNode.GetComponent<TweenScl>();
        }
        TweenScl tweenScl;
        public void playTweenSclAnim(ScaleAnimType scaleAnimType)
        {
            if (scaleAnimType == ScaleAnimType.kNone)
                return;
            tweenScl = tweenSclList[(int)scaleAnimType];
            if (tweenScl)
            {
                tweenScl.PlayBack();
            }
            else
            {

            }
        }
        private void RefreshView()
        {
            if (_isFriend)
                return;
            _coinText.text = PlayerDataModel.Instance.mPlayerData.mGold.ToString("N0");
            _stoneText.text = PlayerDataModel.Instance.mPlayerData.mDiamond.ToString("N0");
            //_foodText.text = GetFood();
            _foodText.text = BagDataModel.Instance.GetItemCountById(14).ToString("N0");
            _gradeText.text = PlayerDataModel.Instance.mPlayerData.mLevel.ToString("N0");
            _charmText.text = PlayerDataModel.Instance.mPlayerData.mCharmVal.ToString("N0");
            _expImage.fillAmount = GetExpProgress();
            OnChangeHead();
            _nameText.text = GetName();
        }

        private void OnChangeName()
        {
            _nameText.text = GetName();
        }

        private void OnChangeHead()
        {
            HeadIconUtils.SetHeadIcon(PlayerDataModel.Instance.mPlayerData.mHead, PlayerDataModel.Instance.mPlayerData.mPlayerID, _playerHeadImage);
        }

        /// <summary>
        /// 是否切换玩家界面
        /// </summary>
        /// <param name="isFriend"></param>
        private void togglePlayerView(bool isFriend, object data = null)
        {
            _isFriend = isFriend;
            _minor.SetActive(!isFriend);
            _major.SetActive(!isFriend);
            _shortcut.SetActive(!isFriend);
            _otherPlayer.SetActive(isFriend);

            if (isFriend)
            {
                // _coinText.text = otherPlayerInfo.PlayerGold.ToString();// GetCoin();
                // _stoneText.text = otherPlayerInfo.PlayerDiamond.ToString();// GetStone();
                //_foodText.text = "0";//GetFood();
                _gradeText.text = otherPlayerInfo.PlayerLevel.ToString();//GetGrade();
                _charmText.text = otherPlayerInfo.PlayerCharm.ToString();// GetCharm();
                _expImage.fillAmount = 1;
                OnChangeHead();
                _nameText.text = otherPlayerInfo.PlayerName;
            }
            else
            {
                BuildingSurfaceManager.Instance.OnShowSurface();
                Build.BuildingManager.Instance.ShowEntityAll(true);
                //if (buildingInit != null)
                //{
                //    buildingInit();
                //}
                RefreshView();
            }
        }
        #endregion

        #region SystemAccouncementMovingControl
        /*
        _enum_scrolTextStatus = ScrolTextStatus.Idle;
        _go_scrolText = Find("SystemAccouncement/Text");
        _txt_scrolText = Find<Text>("SystemAccouncement/Text");
        _go_scrolTextParent = Find("SystemAccouncement/Text");
        */
        //_data_systemAccouncement
        private void NormalizationScrolText()
        {
            //Debug.Log("-><color=#9400D3>" + "[日志] [MainWindow.View.cs] [NormalizationScrolText] 通过[UpdatePerSecond]的每秒判断后，正常化一次滚动字幕,时间：" + Time.time + "</color>");
            //string _str_acnmtContent = KLocalization.GetLocalString(KChatManager.Instance.Dict_SystemAccouncementCfg[_data_systemAccouncement.MsgType].mainViewDescriptionID);
            //_str_acnmtContent = _str_acnmtContent.Replace(" ", "");
            //string subjectName = "<color=#02ac88>" + _data_systemAccouncement.PlayerName + "</color>";
            //string objectName = "<color=#ff5c01>" + KChatManager.FillobjectName(_data_systemAccouncement) + "</color>";
            //_txt_scrolText.text = string.Format(_str_acnmtContent, subjectName, objectName);
            //_enum_scrolTextStatus = ScrolTextStatus.Running;
            //_twnPs_scrolText.PlayBack();
            //_twnPs_scrolText.onEndTweenSet(() =>
            //{
            //    _enum_scrolTextStatus = ScrolTextStatus.Idle;
            //    _data_systemAccouncement = null;
            //});
        }
        #endregion
    }
}

