///** 
// *作用：用于填充排行榜子物体界面的类
// *Author:       LiMuChen 
//*/
//using Game.Build;
//using Msg.ClientMessage;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;

//namespace Game.UI
//{
//    public class RankListItem : KUIItem, IPointerClickHandler
//    {
//        #region Filed
//        private Text _int_sequece;
//        private KUIImage _img_sequence;
//        private KUIImage _img_head;
//        private Text _txt_name;
//        private KUIImage _img_rankType;
//        private Text _txt_score;
//        private GameObject _go_btnPanel;

//        private Button _btn_add;
//        private Image _img_add;
//        private Button _btn_room;
//        private Button _btn_visite;
//        private Button _btn_zan;
//        private Image _img_zan;

//        private RankListWindow.RankType _crtRankType;
//        public RankingListItemInfo _data_rank { get; private set; }
//        #endregion
//        #region Unity 
//        // Use this for initialization
//        private void Awake()
//        {
//            _int_sequece = Find<Text>("Item/img_sequenceNum/txt_sequenceNum");
//            _img_sequence = Find<KUIImage>("Item/img_sequenceNum");
//            _img_head = Find<KUIImage>("Item/img_head/ImageMask/Iconhead");
//            _txt_name = Find<Text>("Item/txt_name");
//            _img_rankType = Find<KUIImage>("Item/img_type");
//            _txt_score = Find<Text>("Item/txt_score");
//            _go_btnPanel = Find<Transform>("Item/AddFriendsButton").gameObject;

//            _btn_add = Find<Button>("Item/AddFriendsButton/ButtonGrid/btn_add");
//            _img_add = Find<Image>("Item/AddFriendsButton/ButtonGrid/btn_add");
//            _btn_room = Find<Button>("Item/AddFriendsButton/ButtonGrid/btn_room");
//            _btn_visite = Find<Button>("Item/AddFriendsButton/ButtonGrid/btn_visite");
//            _btn_zan = Find<Button>("Item/AddFriendsButton/ButtonGrid/btn_zan");
//            _img_zan = Find<Image> ("Item/AddFriendsButton/ButtonGrid/btn_zan");
//        }
//        #endregion
//        #region Method
//        protected override void Refresh()
//        {
//            ShowItem(data as RankingListItemInfo);
//        }
//        public void ShowItem(RankingListItemInfo value)
//        {
//            if (value == null)
//            {
//                return;
//            }
//            _data_rank = value;
//            _crtRankType = KUIWindow.GetWindow<RankListWindow>().CurrentRankType;
//            KRankManager.Instance.DeleteTemplateRanking((int)_crtRankType, _data_rank.Rank);
//            if (_data_rank.Rank == KRankManager.Instance.Dict_allRankLst[(int)_crtRankType][KRankManager.Instance.Dict_allRankLst[(int)_crtRankType].Count - 1].Rank)
//            {
//                int startNum;
//                if (KRankManager.Instance.Dict_allRankLst[(int)_crtRankType].Count - 1 <= 0)
//                {
//                    startNum = 1;
//                }
//                else {
//                    startNum = KRankManager.Instance.Dict_allRankLst[(int)_crtRankType].Count - 1;
//                }

//                if ((int)_crtRankType == 4)
//                {
//                    int mycat = KRankManager.Instance.GetMyBestCatID;
//                    KRankManager.Instance.PullOneRankingList((int)_crtRankType, startNum, KRankManager.int_const_eachTimeGetNum, mycat, KUIWindow.GetWindow<RankListWindow>().AddRankDialog);
//                }
//                else
//                {
//                    KRankManager.Instance.PullOneRankingList((int)_crtRankType, startNum, KRankManager.int_const_eachTimeGetNum, 1, KUIWindow.GetWindow<RankListWindow>().AddRankDialog);
//                }

//            }
//            if (_data_rank.Rank <= 3 && _data_rank.Rank >= 1)
//            {
//                _img_sequence.enabled = true;
//                _img_sequence.overrideSprite = _img_sequence.sprites[_data_rank.Rank - 1];
//                _int_sequece.gameObject.SetActive(false);
//            }
//            else if (_data_rank.Rank >= KRankManager.int_const_maxShowSequence || _data_rank.Rank == 0)
//            {
//                _img_sequence.enabled = true;
//                _img_sequence.overrideSprite = _img_sequence.sprites[3];
//                _int_sequece.gameObject.SetActive(true);
//                _int_sequece.text = "--";
//                _int_sequece.gameObject.SetActive(false);
//            }
//            else
//            {
//                //_img_sequence.overrideSprite = _img_sequence.sprites[3];
//                _img_sequence.enabled = false;
//                _int_sequece.gameObject.SetActive(true);
//                _int_sequece.text = _data_rank.Rank.ToString();
//            }
//            switch (_crtRankType)
//            {
//                case RankListWindow.RankType.TotalScore:
//                    _txt_name.text = _data_rank.PlayerName;
//                    //Debug.Log("头像：" + _data_rank.PlayerHead);
//                    _img_head.overrideSprite = KIconManager.Instance.GetHeadIcon(_data_rank.PlayerHead);
//                    _img_rankType.gameObject.SetActive(false);
//                    //Debug.LogWarning("当前页签：" + _crtRankType + "1  名称：" + _img_rankType.overrideSprite.name);
//                    _txt_score.text = _data_rank.PlayerStageTotalScore.ToString();
//                    break;
//                case RankListWindow.RankType.kCharm:
//                    _txt_name.text = _data_rank.PlayerName;
//                    _img_head.overrideSprite = KIconManager.Instance.GetHeadIcon(_data_rank.PlayerHead);
//                    _img_rankType.gameObject.SetActive(true);
//                    _img_rankType.overrideSprite = _img_rankType.sprites[1];
//                    _txt_score.text = _data_rank.PlayerCharm.ToString();
//                    break;
//                case RankListWindow.RankType.kOuQi:
//                    KItemCat _catItem = KItemManager.Instance.GetCat(_data_rank.CatTableId);
//                    _txt_name.text = _data_rank.PlayerName;
//                    _img_head.overrideSprite = KIconManager.Instance.GetHeadIcon(_catItem.iconName);
//                    _img_rankType.gameObject.SetActive(true);
//                    _img_rankType.overrideSprite = _img_rankType.sprites[0];
//                    _txt_score.text = _data_rank.CatOuqi.ToString();
//                    break;
//                case RankListWindow.RankType.kZan:
//                    _txt_name.text = _data_rank.PlayerName;
//                    _img_head.overrideSprite = KIconManager.Instance.GetHeadIcon(_data_rank.PlayerHead);
//                    _txt_score.text = _data_rank.PlayerZaned.ToString();
//                    _img_rankType.gameObject.SetActive(true);
//                    _img_rankType.overrideSprite = _img_rankType.sprites[2];
//                    break;
//            }
//            _go_btnPanel.SetActive(false);
//            if (KRankManager.Instance.RankListItemData_select != null && KRankManager.Instance.RankListItemData_select.PlayerId == _data_rank.PlayerId)
//            {
//                if (_crtRankType == RankListWindow.RankType.TotalScore || _crtRankType == RankListWindow.RankType.kCharm || _crtRankType == RankListWindow.RankType.kZan)
//                {
//                    SelectNewItem(true);
//                }
//            }
//            else {
//                SelectNewItem(false);
//            }
//        }
//        #endregion
//        #region Interface
//        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
//        {
//            switch (_crtRankType)
//            {
//                case RankListWindow.RankType.TotalScore:
//                case RankListWindow.RankType.kCharm:
//                case RankListWindow.RankType.kZan:
//                     KRankManager.Instance.SelectOrClearItem(this);
//                    break;
//                case RankListWindow.RankType.kOuQi:
//                    KRankManager.Instance.C2SPlayerCatInfo(_data_rank.PlayerId, _data_rank.CatId, GetCatDataInRankCallBack);
//                    break;
//            }
//        }
//        /// <summary>
//        /// 点击选中该条目
//        /// </summary>
//        public void SelectNewItem(bool value) {
//            if (_data_rank.PlayerId != KUser.SelfPlayer.id)
//            {
//                switch (_crtRankType)
//                {
//                    case RankListWindow.RankType.TotalScore:
//                        _img_rankType.gameObject.SetActive(false);
//                        _txt_score.gameObject.SetActive(!value);
//                        _go_btnPanel.SetActive(value);
//                        InitializationBtns();
//                        break;
//                    case RankListWindow.RankType.kCharm:
//                    case RankListWindow.RankType.kZan:
//                        _img_rankType.gameObject.SetActive(!value);
//                        _txt_score.gameObject.SetActive(!value);
//                        _go_btnPanel.SetActive(value);
//                        InitializationBtns();
//                        break;
//                }
//            }
//        }
//        /// <summary>
//        /// 通过数据初始化按键面板
//        /// </summary>
//        public void InitializationBtns() {
//            if (_data_rank.PlayerId == KUser.SelfPlayer.id)
//            {
//                _btn_add.gameObject.SetActive(false);
//                _btn_room.gameObject.SetActive(false);
//                _btn_visite.gameObject.SetActive(false);
//                _btn_zan.gameObject.SetActive(false);
//            }
//            else {
//                _btn_add.gameObject.SetActive(true);
//                _btn_room.gameObject.SetActive(false);
//                _btn_visite.gameObject.SetActive(true);
//                _btn_zan.gameObject.SetActive(true);
//            }
//            _btn_add.onClick.RemoveAllListeners();
//            _btn_room.onClick.RemoveAllListeners();
//            _btn_visite.onClick.RemoveAllListeners();
//            _btn_zan.onClick.RemoveAllListeners();
//            _btn_room.onClick.AddListener(()=> {
//                ToastBox.ShowText("进入对方空间");
//            });
//            _btn_visite.onClick.AddListener(()=> {
//                BuildingManager.Instance.VisitPlayer(_data_rank.PlayerId, Convert.ToBoolean(_data_rank.IsFriend));
//                KUIWindow.CloseWindow<RankListWindow>();                
//            });
//            if (_data_rank.IsFriend){
//                _img_add.material = Resources.Load<Material>("Materials/UIGray");
//                _btn_add.onClick.AddListener(()=> {
//                    ToastBox.ShowText(KLocalization.GetLocalString(55001));
//                });
//            }else {
//                _img_add.material = null;
//                _btn_add.onClick.AddListener(() => {
//                    KFriendManager.Instance.AddFriend(_data_rank.PlayerId, AddFriendCallBack);
//                });
//            }
//            if (KRankManager.Instance.Dict_allRankLst[(int)_crtRankType].Find(x=>x.PlayerId ==_data_rank.PlayerId).IsZaned){
//                _img_zan.material = Resources.Load<Material>("Materials/UIGray");
//                _btn_zan.onClick.AddListener(() => {
//                    ToastBox.ShowText(KLocalization.GetLocalString(55002));
//                });
//            }else {
//                _img_zan.material = null;
//                _btn_zan.onClick.AddListener(()=> {
//                    //Debug.Log("执行点赞动作：" + _data_rank.PlayerId);
//                    KFriendManager.Instance.C2SZanPlayer(_data_rank.PlayerId, ZanPlayerCallBack);
//                });
//            }
//        }
//        /// <summary>
//        /// 添加好友的反馈
//        /// </summary>
//        /// <param name="code"></param>
//        /// <param name="str"></param>
//        /// <param name="obj"></param>
//        private void AddFriendCallBack(int code,string str,object obj) {
//            ToastBox.ShowText(KLocalization.GetLocalString(53043));
//            InitializationBtns();
//        }
//        /// <summary>
//        /// 点赞的反馈
//        /// </summary>
//        /// <param name="code"></param>
//        /// <param name="str"></param>
//        /// <param name="obj"></param>
//        private void ZanPlayerCallBack(int code, string str, object obj) {
//            //ToastBox.ShowText("已点赞");
//            InitializationBtns();
//        }
//        /// <summary>
//        /// 向服务器索取所点击的猫咪的详细信息
//        /// </summary>
//        /// <param name="code"></param>
//        /// <param name="str"></param>
//        /// <param name="obj"></param>
//        private void GetCatDataInRankCallBack(int code,string str,object obj) {
//            KItemCat cfgcat = KItemManager.Instance.GetCat(_data_rank.CatTableId);
//            SelectCatForShowAddAttribute addData = KRankManager.Instance.Data_catAddAttribute;
//            if (addData == null)
//            {
//                ToastBox.ShowText("[RankListItem] 异常：从服务器获取的猫补充信息为空");
//                return;
//            }
//            KCat selectedCat = new KCat
//            {
//                shopId = _data_rank.CatTableId,
//                nickName = _data_rank.CatNick,
//                mainColor = cfgcat.mainColor,

//                catId = addData.catId,
//                grade = addData.catLv,
//                exp = addData.catExp,
//                star = addData.catStar,
//                skillGrade = addData.catSkillLv,
//                initCoinAbility = addData.catAddCoin,
//                initMatchAbility = addData.catAddMatch,
//                initExploreAbility = addData.catAddExplore,
//            };
//            KUIWindow.OpenWindow<CatInfoWindow>(windowData: new CatInfoWindow.WindowData
//            {
//                catSource = 1,
//                carDataFromSource = selectedCat,
//            });
//        }
//        #endregion
//    }
//}

