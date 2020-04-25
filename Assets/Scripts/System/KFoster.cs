//// ***********************************************************************
//// Assembly         : Unity
//// Author           : Kimch
//// Created          : 
////
//// Last Modified By : Kimch
//// Last Modified On : 
//// ***********************************************************************
//// <copyright file= "KFoster" company=""></copyright>
//// <summary></summary>
//// ***********************************************************************
//namespace Game
//{
//    using System.Collections;
//    using System.Collections.Generic;
//    using Callback = System.Action<int, string, object>;

//    /// <summary>
//    /// 寄养数据
//    /// </summary>
//    public class KFoster : KGameModule
//    {
//        #region Struct

//        public class Slot
//        {
//            private int _remainTimestamp;
//            /// <summary>
//            /// 放入猫时的寄养卡
//            /// </summary>
//            public int startCardId
//            {
//                get;
//                set;
//            }
//            public int startTimestamp
//            {
//                get;
//                set;
//            }
//            public int remainTime
//            {
//                get { return _remainTimestamp - KLaunch.Timestamp; }
//                set { _remainTimestamp = value + KLaunch.Timestamp; }
//            }

//            public int incomeExp;
//            public KItem.ItemInfo[] incomeItems;
//            public KCat cat;
//            public KFriend catOwner;
//            public KFriend slotOwner;
//        }

//        #endregion

//        private int _cardRemainTimestamp;

//        private readonly List<Slot> _selfSlots = new List<Slot>(6);
//        private readonly List<Slot> _mixSlots = new List<Slot>(6);


//        /// <summary>
//        /// 寄养所建筑Id
//        /// </summary>
//        public int buildingId
//        {
//            get;
//            private set;
//        }

//        /// <summary>
//        /// 卡片Id
//        /// </summary>
//        public int cardId
//        {
//            get;
//            private set;
//        }

//        /// <summary>
//        /// 自己的
//        /// </summary>
//        public int selfSlotCount
//        {
//            get;
//            private set;
//        }

//        /// <summary>
//        /// 好友在我这寄养
//        /// </summary>
//        public int friendInSelfSlotCount
//        {
//            get;
//            private set;
//        }

//        /// <summary>
//        /// 我在好友那寄养
//        /// </summary>
//        public int selfInFriendSlotCount
//        {
//            get;
//            private set;
//        }

//        /// <summary>
//        /// 卡片剩余时间
//        /// </summary>
//        public int cardRemainTime
//        {
//            get { return _cardRemainTimestamp - KLaunch.Timestamp; }
//            set { _cardRemainTimestamp = KLaunch.Timestamp + value; }
//        }

//        /// <summary>
//        /// 自己的位置
//        /// </summary>
//        public List<Slot> selfSlots
//        {
//            get
//            {
//                _selfSlots.Sort((s1, s2) => s1.startTimestamp.CompareTo(s2.startTimestamp));
//                return _selfSlots;
//            }
//        }

//        /// <summary>
//        /// 混合的位置
//        /// </summary>
//        public List<Slot> mixSlots
//        {
//            get { return _mixSlots; }
//        }

//        ///////////以下是好友


//        private int _friendCardRemainTimestamp;
//        private readonly List<Slot> _friendSlots = new List<Slot>(6);
//        private readonly List<Slot> _friendMixSlots = new List<Slot>(3);

//        public int friendId
//        {
//            get;
//            private set;
//        }
//        /// <summary>
//        /// 卡片Id
//        /// </summary>
//        public int friendCardId
//        {
//            get;
//            private set;
//        }
//        /// <summary>
//        /// 卡片剩余时间
//        /// </summary>
//        public int friendCardRemainTime
//        {
//            get { return _cardRemainTimestamp - KLaunch.Timestamp; }
//            set { _cardRemainTimestamp = KLaunch.Timestamp + value; }
//        }
//        /// <summary>
//        /// 好友被寄养
//        /// </summary>
//        public int friendFosteredSlotCount
//        {
//            get;
//            private set;
//        }
//        /// <summary>
//        /// 好友的位置
//        /// </summary>
//        public List<Slot> friendSlots
//        {
//            get { return _friendSlots; }
//        }
//        /// <summary>
//        /// 好友的好友寄养位置
//        /// </summary>
//        public List<Slot> friendMixSlots
//        {
//            get { return _friendMixSlots; }
//        }

//        #region Method

//        /// <summary>
//        /// 获取自己的数据
//        /// </summary>
//        /// <param name="settle">是否结算</param>
//        /// <param name="callback"></param>
//        public void GetSelfInfos(bool settle, Callback callback)
//        {
//            KServer.Instance.FosterGets(settle, (code, message, data) =>
//             {
//                 if (code == 0)
//                 {
//                     OnGetInfosCallback(code, message, data);
//                 }

//                 if (callback != null)
//                 {
//                     callback(code, message, data);
//                 }
//             });
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="callback"></param>
//        public void GetMixInfos(Callback callback)
//        {
//            KServer.Instance.FosterGets2((code, message, data) =>
//            {
//                if (code == 0)
//                {
//                    OnGetInfosCallback(code, message, data);
//                }

//                if (callback != null)
//                {
//                    callback(code, message, data);
//                }
//            });
//        }

//        /// <summary>
//        /// 获取好友的数据
//        /// </summary>
//        /// <param name="callback"></param>
//        public void GetFriendInfos(int playerId, Callback callback)
//        {
//            KServer.Instance.FosterGetsFriend(playerId, (code, message, data) =>
//             {
//                 if (code == 0)
//                 {
//                     OnGetInfosCallback(code, message, data);
//                 }

//                 if (callback != null)
//                 {
//                     callback(code, message, data);
//                 }
//             });
//        }

//        /// <summary>
//        /// 增加自己的猫
//        /// </summary>
//        /// <param name="catId"></param>
//        /// <param name="callback"></param>
//        public void AddSelfCat(int catId, Callback callback)
//        {
//            KServer.Instance.FosterAddCat(buildingId, catId, (code, message, data) =>
//             {
//                 if (code == 0)
//                 {
//                     OnGetInfosCallback(code, message, data);
//                 }

//                 if (callback != null)
//                 {
//                     callback(code, message, data);
//                 }
//             });
//        }

//        /// <summary>
//        /// 移除自己的猫
//        /// </summary>
//        /// <param name="catId"></param>
//        /// <param name="callback"></param>
//        public void RemoveSelfCat(int catId, Callback callback)
//        {
//            KServer.Instance.FosterRemoveCat(buildingId, catId, (code, message, data) =>
//            {
//                if (code == 0)
//                {
//                    OnGetInfosCallback(code, message, data);
//                }

//                if (callback != null)
//                {
//                    callback(code, message, data);
//                }
//            });
//        }


//        /// <summary>
//        /// 增加自己的猫到好友家
//        /// </summary>
//        /// <param name="catId"></param>
//        /// <param name="callback"></param>
//        public void AddSelfCatInFriend(int friendId, int catId, Callback callback)
//        {
//            KServer.Instance.FosterAddCatInFriend(friendId, catId, (code, message, data) =>
//            {
//                if (code == 0)
//                {
//                    OnGetInfosCallback(code, message, data);
//                }

//                if (callback != null)
//                {
//                    callback(code, message, data);
//                }
//            });
//        }

//        /// <summary>
//        /// 装备寄养卡
//        /// </summary>
//        /// <param name="buildingId"></param>
//        /// <param name="cardId">bingid</param>
//        /// <param name="callback"></param>
//        public void FosterEquipCard(int buildingId, int bindCardId, Callback callback)
//        {
//            KServer.Instance.FosterEquipCard(buildingId, bindCardId, (code, message, data) =>
//            {
//                if (code == 0)
//                {
//                    OnGetInfosCallback(code, message, data);
//                }

//                if (callback != null)
//                {
//                    callback(code, message, data);
//                }
//            });
//        }

//        /// <summary>
//        /// 卸载寄养卡
//        /// </summary>
//        /// <param name="buildingId"></param>
//        /// <param name="cardId"></param>
//        /// <param name="callback"></param>
//        public void FosterUnequipCard(int buildingId, Callback callback)
//        {
//            KServer.Instance.FosterUnequipCard(buildingId, (code, message, data) =>
//            {
//                if (code == 0)
//                {
//                    OnGetInfosCallback(code, message, data);
//                }

//                if (callback != null)
//                {
//                    callback(code, message, data);
//                }
//            });
//        }

//        /// <summary>
//        /// 合成寄养卡
//        /// </summary>
//        /// <param name="bindCardIds"></param>
//        /// <param name="callback"></param>
//        public void FosterComposeCard(int[] bindCardIds, Callback callback)
//        {
//            KServer.Instance.FosterComposeCard(bindCardIds, (code, message, data) =>
//            {
//                if (code == 0)
//                {
//                    OnGetInfosCallback(code, message, data);
//                }

//                if (callback != null)
//                {
//                    callback(code, message, data);
//                }
//            });
//        }

//        private void OnGetInfosCallback(int code, string message, object data)
//        {
//            var protoDatas = data as ArrayList;
//            if (protoDatas != null)
//            {
//                for (int i = 0; i < protoDatas.Count; i++)
//                {
//                    var protoData = protoDatas[i];

//                    if (protoData is Msg.ClientMessage.S2CPullFosterDataResult)
//                    {
//                        var originData = (Msg.ClientMessage.S2CPullFosterDataResult)protoData;
//                        buildingId = originData.BuildingId;
//                        cardId = originData.CardId;
//                        cardRemainTime = originData.CardRemainSeconds;
//                        selfSlotCount = originData.SelfSlotNum;
//                        _selfSlots.Clear();

//                        foreach (var catInfo in originData.SelfCats)
//                        {
//                            var slot = new Slot
//                            {
//                                startCardId = cardId,
//                                startTimestamp = catInfo.StartTime,
//                                incomeExp = catInfo.CatExp,
//                                cat = KCatManager.Instance.GetCat(catInfo.CatId)
//                            };
//                            slot.incomeItems = new KItem.ItemInfo[catInfo.Items.Count];
//                            for (int j = 0; j < catInfo.Items.Count; j++)
//                            {
//                                slot.incomeItems[j] = new KItem.ItemInfo
//                                {
//                                    itemID = catInfo.Items[j].ItemCfgId,
//                                    itemCount = catInfo.Items[j].ItemNum,
//                                };
//                            }
//                            _selfSlots.Add(slot);
//                        }
//                    }
//                    else if (protoData is Msg.ClientMessage.S2CPullFosterCatsWithFriendResult)
//                    {
//                        var originData = (Msg.ClientMessage.S2CPullFosterCatsWithFriendResult)protoData;
//                        friendInSelfSlotCount = originData.FosterFriendSlotNum;
//                        selfInFriendSlotCount = originData.FriendFosteredSlotNum;

//                        _mixSlots.Clear();
//                        //我寄养在好友的猫
//                        foreach (var item in originData.CatsInFriend)
//                        {
//                            var slot = new Slot
//                            {
//                                startCardId = item.StartCardId,
//                                incomeExp = item.CatExp,
//                                remainTime = item.RemainSeconds,
//                                cat = KCatManager.Instance.GetCat(item.CatId),
//                                slotOwner = new KFriend
//                                {
//                                    playerId = item.FriendId,
//                                    nickName = item.FriendName,
//                                    headURL = item.FriendHead.ToString(),
//                                    grade = item.FriendLevel,
//                                },
//                                incomeItems = new KItem.ItemInfo[item.Items.Count]
//                            };
//                            for (int j = 0; j < item.Items.Count; j++)
//                            {
//                                slot.incomeItems[j] = new KItem.ItemInfo
//                                {
//                                    itemID = item.Items[j].ItemCfgId,
//                                    itemCount = item.Items[j].ItemNum,
//                                };
//                            }

//                            _mixSlots.Add(slot);
//                        }
//                        //好友寄养在我的猫
//                        foreach (var item in originData.FriendCats)
//                        {
//                            var slot = new Slot
//                            {
//                                startCardId = item.StartCardId,
//                                remainTime = item.RemainSeconds,
//                                cat = new KCat
//                                {
//                                    catId = int.MaxValue,
//                                    shopId = item.CatTableId,
//                                    nickName = item.CatNick,
//                                    grade = item.CatLevel,
//                                    star = item.CatStar,
//                                },
//                                catOwner = new KFriend
//                                {
//                                    playerId = item.FriendId,
//                                    nickName = item.FriendName,
//                                    headURL = item.FriendHead.ToString(),
//                                    grade = item.FriendLevel,
//                                },
//                            };

//                            _mixSlots.Add(slot);
//                        }
//                    }
//                    else if (protoData is Msg.ClientMessage.S2CGetPlayerFosterCatsResult)
//                    {
//                        var originData = (Msg.ClientMessage.S2CGetPlayerFosterCatsResult)protoData;
//                        friendId = originData.PlayerId;
//                        friendCardRemainTime = originData.CardRemainSeconds;
//                        friendFosteredSlotCount = originData.FosteredSlotNum;
//                        friendCardId = originData.FosterCardId;
//                        _friendSlots.Clear();
//                        foreach (var item in originData.Cats)
//                        {
//                            var slot = new Slot
//                            {
//                                cat = new KCat
//                                {
//                                    catId = int.MaxValue,
//                                    shopId = item.CatTableId,
//                                    grade = item.CatLevel,
//                                    star = item.CatStar,
//                                },
//                            };
//                            _friendSlots.Add(slot);
//                        }

//                        _friendMixSlots.Clear();
//                        foreach (var item in originData.FriendCats)
//                        {
//                            var slot = new Slot
//                            {
//                                startCardId = item.StartCardId,
//                                remainTime = item.RemainSeconds,
//                                cat = new KCat
//                                {
//                                    catId = int.MaxValue,
//                                    shopId = item.CatTableId,
//                                    nickName = item.CatNick,
//                                    grade = item.CatLevel,
//                                    star = item.CatStar,
//                                },
//                                catOwner = new KFriend
//                                {
//                                    playerId = item.FriendId,
//                                    nickName = item.FriendName,
//                                    headURL = item.FriendHead.ToString(),
//                                    grade = item.FriendLevel,
//                                },
//                            };
//                            _friendMixSlots.Add(slot);
//                        }
//                    }
//                    else if (protoData is Msg.ClientMessage.S2CFosterEquipCardResult)
//                    {
//                        var originData = (Msg.ClientMessage.S2CFosterEquipCardResult)protoData;
//                        //buildingId = originData.BuildingId;
//                        cardId = originData.CardId;
//                        cardRemainTime = originData.CardRemainSeconds;
//                    }
//                }
//            }
//        }

//        #endregion

//        #region Unity

//        public static KFoster Instance;

//        private void Awake()
//        {
//            Instance = this;
//        }

//        #endregion
//    }
//}
