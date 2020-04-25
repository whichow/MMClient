///*
// 作用：[排行榜]的Mgr类，用于排行榜数据的存储和处理，也用于与服务器的交互
// 作者：wsy
// */
//namespace Game
//{
//    using UnityEngine;
//    using System.Collections;
//    using System.Collections.Generic;
//    using Callback = System.Action<int, string, object>;
//    using Msg.ClientMessage;
//    using System.Linq;
//    using System;
//    using Game.UI;

//    /// <summary>
//    /// KFriendManager
//    /// </summary>
//    public class KRankManager : SingletonUnity<KRankManager>
//    {
//        /// <summary>
//        /// 每次跟服务器拉取的数量
//        /// </summary>
//        public const int int_const_eachTimeGetNum = 20;

//        public const int int_const_eachTimeGetGameoverNum = 10;
//        /// <summary>
//        /// 排行榜排序的极限，高于此数值之后不会排序，会有对应的显示形式
//        /// </summary>
//        public const int int_const_maxShowSequence = 9999;
//        /// <summary>
//        /// 全部字典数据
//        /// </summary>
//        private Dictionary<int, IList<RankingListItemInfo>> _dict_allRankLst = new Dictionary<int, IList<RankingListItemInfo>>();
//        public Dictionary<int, IList<RankingListItemInfo>> Dict_allRankLst
//        {
//            get
//            {
//                return _dict_allRankLst;
//            }
//        }
//        /// <summary>
//        /// 自己在各个榜中的排名
//        /// </summary>
//        private Dictionary<int, MySelfDataInRankUpdateByServer> _dict_myRankIDInEveryRank = new Dictionary<int, MySelfDataInRankUpdateByServer>();
//        public Dictionary<int, MySelfDataInRankUpdateByServer> Dict_myRankIDInEveryRank
//        {
//            get
//            {
//                return _dict_myRankIDInEveryRank;
//            }
//        }
//        private Dictionary<int, IList<RankingListItemInfo>> _dict_newRankLst = new Dictionary<int, IList<RankingListItemInfo>>();
//        /// <summary>
//        /// 最新获取的排行榜信息
//        /// </summary>
//        public Dictionary<int, IList<RankingListItemInfo>> CurrentRankNewAddData
//        {
//            get
//            {
//                return _dict_newRankLst;
//            }
//        }
//        /// <summary>
//        /// List参数的重载用于切换页签时，部分未生成的新信息的移除
//        /// </summary>
//        /// <param name="rankType"></param>
//        /// <param name="lst_sequences"></param>
//        public void DeleteTemplateRanking(int rankType, List<int> lst_sequences)
//        {
//            for (int i = 0; i < lst_sequences.Count; i++)
//            {
//                DeleteTemplateRanking(rankType, lst_sequences[i]);
//            }
//        }
//        /// <summary>
//        /// int参数的重载用于已经被子物体使用之后，移除出列表防止重复数据的生成
//        /// </summary>
//        /// <param name="rankType"></param>
//        /// <param name="lst_sequences"></param>
//        public void DeleteTemplateRanking(int rankType, int sequence)
//        {
//            if (_dict_newRankLst.ContainsKey(rankType))
//            {
//                _dict_newRankLst[rankType].Remove(_dict_newRankLst[rankType].Find(x => x.Rank == sequence));
//            }
//        }
//        /// <summary>
//        /// 滚动之前执行一次去除重复项
//        /// </summary>
//        public void DistinctSameData(int rankType)
//        {
//            _dict_newRankLst[rankType].Distinct().ToList();
//        }
//        private RankListItem _rankListItem_select = new RankListItem();
//        /// <summary>
//        /// 点击选中的排行榜子物体
//        /// </summary>
//        public RankListItem RankListItem_select
//        {
//            get
//            {
//                return _rankListItem_select;
//            }
//        }
//        private RankingListItemInfo _rankListItemData_select = new RankingListItemInfo();
//        /// <summary>
//        /// 被选中的排行榜子物体的数据
//        /// </summary>
//        public RankingListItemInfo RankListItemData_select
//        {
//            get
//            {
//                return _rankListItemData_select;
//            }
//        }
//        private SelectCatForShowAddAttribute _data_catAddAttribute = new SelectCatForShowAddAttribute();
//        /// <summary>
//        /// 排行榜被查看的猫咪的补充属性
//        /// </summary>
//        public SelectCatForShowAddAttribute Data_catAddAttribute
//        {
//            get
//            {
//                return _data_catAddAttribute;
//            }
//        }
//        /// <summary>
//        /// 移除当前选中的排行榜子物体
//        /// </summary>
//        public void SelectOrClearItem(RankListItem newSelected)
//        {
//            if (_rankListItem_select != null)
//            {
//                _rankListItem_select.SelectNewItem(false);
//            }
//            _rankListItem_select = newSelected;
//            if (_rankListItem_select != null)
//            {
//                _rankListItem_select.SelectNewItem(true);
//                _rankListItemData_select = newSelected._data_rank;
//            }
//            else
//            {
//                _rankListItemData_select = null;
//            }
//        }
//        /// <summary>
//        /// 每次打开界面的时候移除一次所有数据，保证数据最新
//        /// </summary>
//        public void ClearAllRankData()
//        {
//            SelectOrClearItem(null);
//            _dict_allRankLst.Clear();
//            _dict_myRankIDInEveryRank.Clear();
//            _dict_newRankLst.Clear();
//        }
//        /// <summary>
//        /// 获取当前背包内欧气值最高的猫咪的ID
//        /// </summary>
//        /// <returns></returns>
//        public void GetMyBestCatIDOperation()
//        {
//            List<KCat> lst_cats = new List<KCat>();
//            for (int i = 0; i < KCatManager.Instance.allCats.Length; i++)
//            {
//                lst_cats.Add(KCatManager.Instance.allCats[i]);
//            }
//            lst_cats.Sort((x, y) => -x.GetCombatValue().CompareTo(y.GetCombatValue()));
//            if (lst_cats.Count > 0 && lst_cats[0] != null)
//            {
//                GetMyBestCatID = lst_cats[0].catId;
//            }
//            else
//            {
//                GetMyBestCatID = 0;
//            }
//        }
//        public int GetMyBestCatID { get; private set; }
//        #region 协议相关
//        /// <summary>
//        /// 客户端向服务器拉取好友列表
//        /// </summary>
//        /// <param name="rankType"> 排行榜类型 </param>
//        /// <param name="startRank"> 向服务器拉取时的角标 </param>
//        /// <param name="rankNum"> 本次向服务器拉取的数量 </param>
//        /// <param name="param"> 只在关卡排行榜时用于获取关卡ID </param>
//        /// <param name="callback"></param>
//        public void PullOneRankingList(int rankType, int startRank, int rankNum, int param, Callback callback)
//        {
//            KUser.C2SPullRankingList(rankType, startRank, rankNum, param, (code, message, data) =>
//         {
//             var protoDatas = data as ArrayList;
//             if (protoDatas != null)
//             {
//                 for (int i = 0; i < protoDatas.Count; i++)
//                 {
//                     if (protoDatas[i] is S2CPullRankingListResult)
//                     {
//                         var rankDatas = (S2CPullRankingListResult)protoDatas[i];
//                         int thisranktype = rankDatas.RankType;
//                         int thisstartid = rankDatas.StartRank;
//                         IList<RankingListItemInfo> serverFriendList = new List<RankingListItemInfo>();
//                         if (rankDatas.ItemList is IList<RankingListItemInfo>)
//                         {
//                             serverFriendList = rankDatas.ItemList;
//                             if (_dict_allRankLst.ContainsKey(thisranktype))
//                             {
//                                 for (int K = serverFriendList.Count - 1; K >= 0; K--)
//                                 {
//                                     for (int J = 0; J < _dict_allRankLst[thisranktype].Count; J++)
//                                     {
//                                         if (_dict_allRankLst[thisranktype][J].Rank == serverFriendList[K].Rank)
//                                         {
//                                             serverFriendList.Remove(serverFriendList.Find(x => x.Rank == serverFriendList[K].Rank));
//                                             break;
//                                         }
//                                     }
//                                 }
//                                 if (_dict_newRankLst.ContainsKey(thisranktype))
//                                 {
//                                     _dict_newRankLst[thisranktype].AddRange(serverFriendList);
//                                     _dict_newRankLst[thisranktype].Distinct().ToList();
//                                 }
//                                 else
//                                 {
//                                     _dict_newRankLst.Add(thisranktype, serverFriendList);
//                                 }
//                                 _dict_allRankLst[thisranktype].AddRange(serverFriendList);
//                             }
//                             else
//                             {
//                                 _dict_allRankLst.Add(thisranktype, serverFriendList);
//                             }
//                         }
//                         int thisrankMyID = rankDatas.SelfRank;
//                         MySelfDataInRankUpdateByServer _myrankdata = new MySelfDataInRankUpdateByServer
//                         {
//                             SelfRank = thisrankMyID,
//                             SelfValue1 = rankDatas.SelfValue1,
//                             SelfValue2 = rankDatas.SelfValue2
//                         };
//                         if (_dict_myRankIDInEveryRank.ContainsKey(thisranktype))
//                         {
//                             _dict_myRankIDInEveryRank[thisranktype].SelfRank = thisrankMyID;
//                             _dict_myRankIDInEveryRank[thisranktype].SelfValue1 = rankDatas.SelfValue1;
//                             _dict_myRankIDInEveryRank[thisranktype].SelfValue2 = rankDatas.SelfValue2;
//                         }
//                         else
//                         {
//                             _dict_myRankIDInEveryRank.Add(thisranktype, _myrankdata);
//                         }
//                         Debug.Log("-><color=#9400D3>" + "[日志] [KRankManager] [PullOneRankingList]获取排行榜信息，索取类型ID：" + rankType + "返回的ID：" + thisranktype + "起始ID：" + startRank + "索取的长度：" + rankNum + "返回的长度：" + rankDatas.ItemList.Count + "发送的参数param：" + param + "返回的参数1：" + rankDatas.SelfValue1 + "返回的参数2：" + rankDatas.SelfValue2 + "自己的排序：" + rankDatas.SelfRank + "</color>");
//                     }
//                 }
//                 if (callback != null)// && (int)KUIWindow.GetWindow<RankListWindow>().CurrentRankType == rankType)
//                 {
//                     callback(code, message, data);
//                 }
//             }
//         });
//        }
//        /// <summary>
//        /// 向服务器拉取猫咪的具体信息供打开猫咪信息面板时使用
//        /// </summary>
//        /// <param name="playerid"></param>
//        /// <param name="catid"></param>
//        /// <param name="callback"></param>
//        public void C2SPlayerCatInfo(int playerid, int catid, Callback callback)
//        {
//            _data_catAddAttribute = null;
//            KUser.C2SPlayerCatInfo(playerid, catid, (code, message, data) =>
//            {
//                var protoDatas = data as ArrayList;
//                if (protoDatas != null)
//                {
//                    for (int i = 0; i < protoDatas.Count; i++)
//                    {
//                        if (protoDatas[0] is S2CPlayerCatInfoResult)
//                        {
//                            var rankDatas = (S2CPlayerCatInfoResult)protoDatas[0];
//                            SelectCatForShowAddAttribute addInfo = new SelectCatForShowAddAttribute();
//                            addInfo.playerId = rankDatas.PlayerId;
//                            addInfo.catId = rankDatas.CatId;
//                            addInfo.catLv = rankDatas.CatLevel;
//                            addInfo.catExp = rankDatas.CatExp;
//                            addInfo.catStar = rankDatas.CatStar;
//                            addInfo.catSkillLv = rankDatas.CatSkillLevel;
//                            addInfo.catAddCoin = rankDatas.CatAddCoin;
//                            addInfo.catAddMatch = rankDatas.CatAddMatch;
//                            addInfo.catAddExplore = rankDatas.CatAddExplore;
//                            _data_catAddAttribute = addInfo;
//                        }
//                    }
//                    if (callback != null)
//                    {
//                        callback(code, message, data);
//                    }
//                }
//            });
//        }
//        #endregion  
//    }
//    /// <summary>
//    /// 排行榜中我自己的信息
//    /// </summary>
//    public class MySelfDataInRankUpdateByServer
//    {
//        public int SelfRank;
//        public int SelfValue1; // 类型1 总分  2 积分  3 魅力值  4 猫ID
//        public int SelfValue2; // 类型4时为 欧气值
//    }
//    /// <summary>
//    /// 排行榜中的猫查看详情之后从服务器获取的补充信息
//    /// </summary>
//    public class SelectCatForShowAddAttribute
//    {
//        public int playerId;
//        public int catId;
//        public int catLv;
//        public int catExp;
//        public int catStar;
//        public int catSkillLv;
//        public int catAddCoin;
//        public int catAddMatch;
//        public int catAddExplore;
//    }
//}
