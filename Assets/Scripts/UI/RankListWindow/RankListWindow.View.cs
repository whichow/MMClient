//using Msg.ClientMessage;
///** 
// *作用：排行榜的流程控制类
// *Author:       LiMuChen 
//*/
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//namespace Game.UI
//{
//    partial class RankListWindow
//    {
//        #region Field
//        Button _closeBtn;
//        Toggle _toggle1;
//        Toggle _toggle2;
//        Toggle _toggle3;
//        Toggle _toggle4;
//        private Toggle _toggleTop1;
//        private Toggle _toggleTop2;
//        private Text _textTopToggle1;
//        private Text _textTopToggle2;
//        private KUIImage _imageTitle;
//        private Button _btn_question;
//        private Text _txt_tips;

//        private KUIGrid _kgrd_content;
//        public IList<RankingListItemInfo> CurrentRankContentLst { get; private set; }

//        private Text _int_sequece;
//        private KUIImage _img_sequence;
//        private KUIImage _img_head;
//        private GameObject _go_myHead;
//        private Text _txt_name;
//        private KUIImage _img_rankType;
//        private Text _txt_score;
//        private RankingListItemInfo _data_currentTypeMyRank;
//        #endregion
//        #region Method
//        public void InitView()
//        {
//            Debug.Log(transform.name);
//            _closeBtn = Find<Button>("Panel/Quit");
//            _closeBtn.onClick.AddListener(OnCloseBtnClick);

//            _toggle1 = Find<Toggle>("Panel/LTabView/ToggleGroup/Toggle1");
//            _toggle1.onValueChanged.AddListener(OnToggleValueChanged);
//            _toggle2 = Find<Toggle>("Panel/LTabView/ToggleGroup/Toggle2");
//            _toggle2.onValueChanged.AddListener(OnToggleValueChanged);
//            _toggle3 = Find<Toggle>("Panel/LTabView/ToggleGroup/Toggle3");
//            _toggle3.onValueChanged.AddListener(OnToggleValueChanged);
//            _toggle4 = Find<Toggle>("Panel/LTabView/ToggleGroup/Toggle4");
//            _toggle4.onValueChanged.AddListener(OnToggleValueChanged);
//            _toggleTop1 = Find<Toggle>("Panel/TopTabView/ToggleGroup/Toggle1");
//            _toggleTop2 = Find<Toggle>("Panel/TopTabView/ToggleGroup/Toggle2");
//            _textTopToggle1 = Find<Text>("Panel/TopTabView/ToggleGroup/Toggle1/Background/Textshadow");
//            _textTopToggle2 = Find<Text>("Panel/TopTabView/ToggleGroup/Toggle2/Background/Textshadow");
//            _imageTitle = Find<KUIImage>("Panel/Back01/Back02");
//            _kgrd_content = Find<KUIGrid>("Panel/Scroll View");
//            if (_kgrd_content)
//            {
//                _kgrd_content.uiPool.itemTemplate.AddComponent<RankListItem>();
//            }
//            _btn_question = Find<Button>("Panel/Back01/BtnQues");
//            _btn_question.onClick.AddListener(this.OpenaDescritonPanel);
//            _txt_tips = Find<Text>("Panel/Back01/BtnQues/Text");

//            _int_sequece = Find<Text>("Panel/MySelfInfo/img_sequenceNum/txt_sequenceNum");
//            _img_sequence = Find<KUIImage>("Panel/MySelfInfo/img_sequenceNum");
//            _img_head = Find<KUIImage>("Panel/MySelfInfo/img_head/ImageMask/Iconhead");
//            _go_myHead = Find<Transform>("Panel/MySelfInfo/img_head").gameObject;
//            _txt_name = Find<Text>("Panel/MySelfInfo/txt_name");
//            _img_rankType = Find<KUIImage>("Panel/MySelfInfo/img_type");
//            _txt_score = Find<Text>("Panel/MySelfInfo/txt_score");
//        }
//        /// <summary>
//        /// 流程：刷新界面顶层函数
//        /// </summary>
//        public void RefreshView()
//        {
//            KRankManager.Instance.SelectOrClearItem(null);
//            RefreshTipsLabel();
//            InitializationRightView();
//            InitializationContentView();
//        }
//        private void RefreshTipsLabel()
//        {
//            switch (_rankType)
//            {
//                case RankType.TotalScore:
//                    _txt_tips.text = KLocalization.GetLocalString(55010);
//                    break;
//                case RankType.kCharm:
//                    _txt_tips.text = KLocalization.GetLocalString(55012);
//                    break;
//                case RankType.kOuQi:
//                    _txt_tips.text = KLocalization.GetLocalString(55013);
//                    break;
//                case RankType.kZan:
//                    _txt_tips.text = KLocalization.GetLocalString(55015);
//                    break;
//                default:
//                    break;
//            }
//        }
//        private void InitializationRightView()
//        {
//            switch (_rankType)
//            {
//                case RankType.TotalScore:
//                    _textTopToggle1.text = KLocalization.GetLocalString(55016);
//                    _textTopToggle2.text = "";
//                    _toggleTop1.isOn = true;
//                    _toggleTop2.isOn = false;
//                    _toggleTop1.gameObject.SetActive(true);
//                    _toggleTop2.gameObject.SetActive(false);
//                    _imageTitle.ShowSprite(0);
//                    break;
//                case RankType.kCharm:
//                    _textTopToggle1.text = KLocalization.GetLocalString(55018);
//                    _textTopToggle2.text = "";
//                    _toggleTop1.isOn = true;
//                    _toggleTop2.isOn = false;
//                    _toggleTop1.gameObject.SetActive(true);
//                    _toggleTop2.gameObject.SetActive(false);
//                    _imageTitle.ShowSprite(1);
//                    break;
//                case RankType.kOuQi:
//                    _textTopToggle1.text = KLocalization.GetLocalString(55019);
//                    _textTopToggle2.text = "";
//                    _toggleTop1.isOn = true;
//                    _toggleTop2.isOn = false;
//                    _toggleTop1.gameObject.SetActive(true);
//                    _toggleTop2.gameObject.SetActive(false);
//                    _imageTitle.ShowSprite(2);
//                    break;
//                case RankType.kZan:
//                    _textTopToggle1.text = KLocalization.GetLocalString(55021);
//                    _textTopToggle2.text = "";
//                    _toggleTop1.isOn = true;
//                    _toggleTop2.isOn = false;
//                    _toggleTop1.gameObject.SetActive(true);
//                    _toggleTop2.gameObject.SetActive(false);
//                    _imageTitle.ShowSprite(3);
//                    break;
//                default:
//                    break;
//            }
//        }
//        /// <summary>
//        /// 初始化内容的列表
//        /// </summary>
//        public void InitializationContentView()
//        {
//            if (KRankManager.Instance.Dict_allRankLst.ContainsKey((int)_rankType))
//            {
//                CreateRankDialog();
//            }
//            else
//            {
//                if ((int)_rankType == 4)
//                {
//                    int mycat = KRankManager.Instance.GetMyBestCatID;
//                    KRankManager.Instance.PullOneRankingList((int)_rankType, 1, KRankManager.int_const_eachTimeGetNum, mycat, GetOldAndNewRankDataCallBack);
//                }
//                else
//                {
//                    KRankManager.Instance.PullOneRankingList((int)_rankType, 1, KRankManager.int_const_eachTimeGetNum, 1, GetOldAndNewRankDataCallBack);
//                }
//            }
//        }
//        /// <summary>
//        /// 初始化自己的条目
//        /// </summary>
//        public void InitializationMyItem()
//        {
//            _data_currentTypeMyRank.Rank = KRankManager.Instance.Dict_myRankIDInEveryRank[(int)_rankType].SelfRank;
//            if (_rankType == RankType.TotalScore || _rankType == RankType.kCharm || _rankType == RankType.kZan)
//            {
//                _data_currentTypeMyRank.PlayerId = KUser.SelfPlayer.id;
//                _data_currentTypeMyRank.PlayerName = KUser.SelfPlayer.nickName;
//                _data_currentTypeMyRank.PlayerLevel = KUser.SelfPlayer.grade;
//                _data_currentTypeMyRank.PlayerHead = KUser.SelfPlayer.headURL;
//                _data_currentTypeMyRank.PlayerStageTotalScore = KRankManager.Instance.Dict_myRankIDInEveryRank[(int)_rankType].SelfValue1;
//                _data_currentTypeMyRank.PlayerCharm = KUser.SelfPlayer.charm;
//                _data_currentTypeMyRank.PlayerZaned = KUser.SelfPlayer.praise;
//            }
//            else
//            {
//                KCat myRankCat = KCatManager.Instance.GetCat(KRankManager.Instance.Dict_myRankIDInEveryRank[(int)_rankType].SelfValue1);
//                if (myRankCat == null)
//                {
//                    Debug.Log("-><color=#9400D3>" + "[警告] [RankListWindow] [InitializationMyItem] 养成模块排行榜，该ID：" + KRankManager.Instance.Dict_myRankIDInEveryRank[(int)_rankType].SelfValue1 + "不能用从服务器索取的数据获取到自己的猫咪。原因：1.服务器没有发送一个正确的数值。2.自己没有猫咪" + "</color>");
//                    _data_currentTypeMyRank.CatId = 0;
//                    _data_currentTypeMyRank.CatTableId = 0;
//                    _data_currentTypeMyRank.CatNick = string.Empty;
//                    _data_currentTypeMyRank.CatOuqi = 0;
//                    _data_currentTypeMyRank.CatLevel = 0;
//                    _data_currentTypeMyRank.CatStar = 0;
//                }
//                else
//                {
//                    _data_currentTypeMyRank.CatId = KRankManager.Instance.Dict_myRankIDInEveryRank[(int)_rankType].SelfValue1;
//                    _data_currentTypeMyRank.CatTableId = myRankCat.shopId;
//                    _data_currentTypeMyRank.CatNick = myRankCat.nickName;
//                    _data_currentTypeMyRank.CatOuqi = KRankManager.Instance.Dict_myRankIDInEveryRank[(int)_rankType].SelfValue2;
//                    _data_currentTypeMyRank.CatLevel = myRankCat.grade;
//                    _data_currentTypeMyRank.CatStar = myRankCat.star;
//                }
//            }
//            if (_data_currentTypeMyRank.Rank <= 3 && _data_currentTypeMyRank.Rank >= 1)
//            {
//                _img_sequence.enabled = true;
//                _img_sequence.overrideSprite = _img_sequence.sprites[_data_currentTypeMyRank.Rank - 1];
//                _int_sequece.gameObject.SetActive(false);
//            }
//            else if (_data_currentTypeMyRank.Rank >= KRankManager.int_const_maxShowSequence || _data_currentTypeMyRank.Rank == 0)
//            {
//                _img_sequence.enabled = false;
//                _int_sequece.gameObject.SetActive(true);
//                _int_sequece.text = "--";
//                _int_sequece.gameObject.SetActive(true);
//            }
//            else
//            {
//                _img_sequence.enabled = false;
//                _int_sequece.gameObject.SetActive(true);
//                _int_sequece.text = _data_currentTypeMyRank.Rank.ToString();
//            }
//            _go_myHead.SetActive(true);
//            switch (_rankType)
//            {
//                case RankType.TotalScore:
//                    _txt_name.text = _data_currentTypeMyRank.PlayerName;
//                    //Debug.Log("头像：" + _data_currentTypeMyRank.PlayerHead);
//                    _img_head.gameObject.SetActive(true);
//                    _img_head.overrideSprite = KIconManager.Instance.GetHeadIcon(_data_currentTypeMyRank.PlayerHead);
//                    _txt_score.text = _data_currentTypeMyRank.PlayerStageTotalScore.ToString();
//                    _img_rankType.gameObject.SetActive(false);
//                    break;
//                case RankType.kCharm:
//                    _txt_name.text = _data_currentTypeMyRank.PlayerName;
//                    _img_head.gameObject.SetActive(true);
//                    _img_head.overrideSprite = KIconManager.Instance.GetHeadIcon(_data_currentTypeMyRank.PlayerHead);
//                    _txt_score.text = _data_currentTypeMyRank.PlayerCharm.ToString();
//                    _img_rankType.gameObject.SetActive(true);
//                    _img_rankType.overrideSprite = _img_rankType.sprites[1];
//                    break;
//                case RankType.kOuQi:
//                    if (_data_currentTypeMyRank.CatId != 0)
//                    {
//                        KItemCat _catItem = KItemManager.Instance.GetCat(_data_currentTypeMyRank.CatTableId);
//                        _txt_name.text = KUser.SelfPlayer.nickName;//_data_currentTypeMyRank.PlayerName;
//                        _img_head.gameObject.SetActive(true);
//                        _img_head.overrideSprite = KIconManager.Instance.GetHeadIcon(_catItem.iconName);
//                        _txt_score.text = _data_currentTypeMyRank.CatOuqi.ToString();
//                    }
//                    else
//                    {
//                        _txt_name.text = "--";
//                        _img_head.gameObject.SetActive(false);
//                        _go_myHead.SetActive(false);
//                        _txt_score.text = "--";
//                    }
//                    _img_rankType.gameObject.SetActive(true);
//                    _img_rankType.overrideSprite = _img_rankType.sprites[0];
//                    break;
//                case RankType.kZan:
//                    _txt_name.text = _data_currentTypeMyRank.PlayerName;
//                    _img_head.gameObject.SetActive(true);
//                    _img_head.overrideSprite = KIconManager.Instance.GetHeadIcon(_data_currentTypeMyRank.PlayerHead);
//                    _txt_score.text = _data_currentTypeMyRank.PlayerZaned.ToString();
//                    _img_rankType.gameObject.SetActive(true);
//                    _img_rankType.overrideSprite = _img_rankType.sprites[2];
//                    break;
//            }
//        }
//        public void GetOldAndNewRankDataCallBack(int code, string str, object obj)
//        {
//            CreateRankDialog();
//        }
//        /// <summary>
//        /// 使用Mgr数据生成排行榜列表
//        /// </summary>
//        private void CreateRankDialog()
//        {
//            _kgrd_content.ClearItems();
//            CurrentRankContentLst = new List<RankingListItemInfo>();
//            if (KRankManager.Instance.Dict_allRankLst.ContainsKey((int)_rankType))
//            {
//                CurrentRankContentLst = KRankManager.Instance.Dict_allRankLst[(int)_rankType];
//            }
//            ConcreteCreateRankItem(CurrentRankContentLst);
//            _data_currentTypeMyRank = new RankingListItemInfo();
//            InitializationMyItem();
//        }
//        public void AddRankDialog(int code, string str, object obj)
//        {
//            IList<RankingListItemInfo> Lstdialog = new List<RankingListItemInfo>();
//            if (KRankManager.Instance.CurrentRankNewAddData.ContainsKey((int)_rankType))
//            {
//                Lstdialog = KRankManager.Instance.CurrentRankNewAddData[(int)_rankType];
//            }
//            if (Lstdialog != null && Lstdialog.Count != 0)
//            {
//                ConcreteAddRankItem(Lstdialog);
//            }
//        }
//        /// <summary>
//        /// 生成首批条目的具体实现
//        /// </summary>
//        /// <returns></returns>
//        private void ConcreteCreateRankItem(IList<RankingListItemInfo> value)
//        {
//            RankingListItemInfo[] _rankArray = value.ToArray();
//            _kgrd_content.uiPool.SetItemDatas(_rankArray);
//            _kgrd_content.RefillItems();
//        }
//        /// <summary>
//        /// 后续补充条目的具体实现
//        /// </summary>
//        /// <returns></returns>
//        private void ConcreteAddRankItem(IList<RankingListItemInfo> value)
//        {
//            RankingListItemInfo[] _rankArray = value.ToArray();
//            _kgrd_content.uiPool.AddItemDatas(_rankArray);
//            int count = _kgrd_content.uiPool.itemCount;
//        }
//        /// <summary>
//        /// 点击小问号弹出玩法介绍面板
//        /// </summary>
//        private void OpenaDescritonPanel()
//        {

//        }
//        #endregion
//    }
//}

