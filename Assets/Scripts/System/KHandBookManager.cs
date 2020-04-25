// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KMailManager" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using UnityEngine;

namespace Game
{
    using Game.DataModel;
    using System.Collections.Generic;
    using System.Linq;
    using Callback = System.Action<int, string, object>;
    /// <summary>
    /// 邮件系统管理器
    /// </summary>
    public class KHandBookManager : SingletonUnity<KHandBookManager>
    {
        #region CONST
        public class HandBookConfiger
        {
            public int id;
            public string name;
            public int[] tag;
            public int[] getInformation;
            public int bloom;
            public int learned = 0;//0:尚未掌握 1：掌握
        }

        public class HandBookData {
            public HandBookConfiger CfgData;
        }

        public const int _int_LandeID_maozhen = 41001;
        public const int _int_LandeID_yangguangdao = 41002;
        public const int _int_LandeID_youleyuan = 41005;
        public const int _int_LandeID_xinyangdao = 41007;
        public const int _int_LandID_kongzhongfeidao = 41003;
        public const int _int_LandeID_meishidao = 41004;
        public const int _int_LandeID_liulangmaojidi = 41006;

        #endregion

        #region ENUM

        #endregion 

        #region FIELD

        private Dictionary<int, HandBookConfiger> _handBookConfigDictionary = new Dictionary<int, HandBookConfiger>();
        private List<int> intListAllFinishLande = new List<int>();

        #endregion

        public Dictionary<int, HandBookConfiger> HandBookConfigDictionary {
            get {
                return _handBookConfigDictionary;
           }
        }

        public List<int> AllFinishLande {
            get {
               return intListAllFinishLande;
            }
        }
        #region PROPERTY
        public HandBookConfiger[] GetCatDatas(UI.CatHandBookWindow.PageType pt) {
            var aLLHB = new HandBookConfiger[_handBookConfigDictionary.Count];
            _handBookConfigDictionary.Values.CopyTo(aLLHB, 0);
            List<HandBookConfiger> alldat = new List<HandBookConfiger>(aLLHB);
            List<HandBookConfiger> allcatdata = alldat.FindAll(x => x.tag.Contains(1));
            List<HandBookConfiger> catdata = new List<HandBookConfiger>();
            switch (pt)
            {
                case UI.CatHandBookWindow.PageType.allCat:
                    catdata = allcatdata;
                    break;
                case UI.CatHandBookWindow.PageType.NCat:
                    catdata = allcatdata.FindAll(Y => XTable.CatXTable.GetByID(Y.id).Rarity == 1);
                    break;
                case UI.CatHandBookWindow.PageType.RCat:
                    catdata = allcatdata.FindAll(Y => XTable.CatXTable.GetByID(Y.id).Rarity == 2);
                    break;
                case UI.CatHandBookWindow.PageType.SRCat:
                    catdata = allcatdata.FindAll(Y => XTable.CatXTable.GetByID(Y.id).Rarity == 3);
                    break;
                case UI.CatHandBookWindow.PageType.SSRCat:
                    catdata = allcatdata.FindAll(Y => XTable.CatXTable.GetByID(Y.id).Rarity == 4);
                    break;
            }
			catdata.Sort((x, y) =>
				{
					if (x.learned > y.learned)
					{
						return -1;
					}
					else if (x.learned < y.learned)
					{
						return 1;
					}
					else
					{/*
猫咪图鉴中，排序未按照“已拥有>稀有度>颜色>静态ID”序排序。装饰品也未进行“已拥有>未拥有”的排序
					*/
						var _cat_x = XTable.CatXTable.GetByID(x.id);
						var _cat_y = XTable.CatXTable.GetByID(y.id);
						if (_cat_x.Rarity > _cat_y.Rarity)
						{
							return -1;
						}
						else if (_cat_x.Rarity < _cat_y.Rarity)
						{
							return 1;
						}
						else
						{
							if (_cat_x.MainColor > _cat_y.MainColor)
							{
								return -1;
							}
							else if (_cat_x.MainColor < _cat_y.MainColor)
							{
								return 1;
							}
							else
							{
								return x.id.CompareTo(y.id);
							}
						}

					}
				});
            return catdata.ToArray();
        }

        public HandBookConfiger[] GetDecorateDatas(UI.DecorateHandBookWindow.PageType pt)
        {
            var aLLHB = new HandBookConfiger[_handBookConfigDictionary.Count];
            _handBookConfigDictionary.Values.CopyTo(aLLHB, 0);
            List<HandBookConfiger> alldat = new List<HandBookConfiger>(aLLHB);
			List<HandBookConfiger> allDecorateDatas = alldat.FindAll(x => x.tag.Contains(2));
			List<HandBookConfiger> decorateData = new List<HandBookConfiger>();
            switch (pt)
            {
                case UI.DecorateHandBookWindow.PageType.letter:
                    decorateData = allDecorateDatas.FindAll(Y => KItemManager.Instance.GetBuilding(Y.id).itemTag == 2);
                    break;
                case UI.DecorateHandBookWindow.PageType.infrastructure:
                    decorateData = allDecorateDatas.FindAll(Y => KItemManager.Instance.GetBuilding(Y.id).itemTag == 3);
                    break;
                case UI.DecorateHandBookWindow.PageType.trees:
                    decorateData = allDecorateDatas.FindAll(Y => KItemManager.Instance.GetBuilding(Y.id).itemTag == 4);
                    break;
                case UI.DecorateHandBookWindow.PageType.water:
                    decorateData = allDecorateDatas.FindAll(Y => KItemManager.Instance.GetBuilding(Y.id).itemTag == 5);
                    break;
                case UI.DecorateHandBookWindow.PageType.others:
                    decorateData = allDecorateDatas.FindAll(Y => KItemManager.Instance.GetBuilding(Y.id).itemTag == 6);
                    break;
                default:
                    Debug.LogError("异常");
                    break;
            }
			decorateData.Sort((x, y) =>
				{
					if (x.learned > y.learned)
					{
						return -1;
					}
					else if (x.learned < y.learned)
					{
						return 1;
					}
					else
					{
						return x.id.CompareTo(y.id);
					}
				});
            return decorateData.ToArray();
        }

        public HandBookConfiger[] GetSuiteDatas()
        {
            var aLLHB = new HandBookConfiger[_handBookConfigDictionary.Count];
            _handBookConfigDictionary.Values.CopyTo(aLLHB, 0);
            List<HandBookConfiger> alldat = new List<HandBookConfiger>(aLLHB);
            KItemSuit[] allsuite = KItemManager.Instance.GetSuits();
            KItemBuilding[] allbuildings = KItemManager.Instance.GetBuildings();
            Dictionary<int,HandBookConfiger> suiteDict = new Dictionary<int, HandBookConfiger>();   
            Dictionary<int, HandBookConfiger> newsuiteDict = new Dictionary<int, HandBookConfiger>();
            foreach (var item in _handBookConfigDictionary.Values)
            {
                if (item.tag.Contains(3))
                {
                    HandBookConfiger onenewsuitHB = new HandBookConfiger();
                    KItemBuilding onenewbuilding = KItemManager.Instance.GetBuilding(item.id);
                    KItemSuit onenewsuit = KItemManager.Instance.GetSuit(onenewbuilding.suitID);
                    onenewsuitHB.id = onenewsuit.itemID;
                    onenewsuitHB.name = onenewsuit.itemName;
                    onenewsuitHB.getInformation = onenewsuit.getInformation;
                    onenewsuitHB.tag =  new int[1] { onenewsuit.itemTag };
                    onenewsuitHB.bloom = 1;
                    if (!newsuiteDict.ContainsKey(onenewsuit.itemID))
                    {
                        if (item.learned == 1)
                        {
                            onenewsuitHB.learned = 1;
                        }
                        newsuiteDict.Add(onenewsuitHB.id, onenewsuitHB);
                    }
                }
            }
            List<HandBookConfiger> sortlist = new List<HandBookConfiger>(newsuiteDict.Values.ToList<HandBookConfiger>());
            sortlist.Sort((x, y) =>
            {
                if (x.learned > y.learned)
                {
                    return -1;
                }
                else if (x.learned < y.learned)
                {
                    return 1;
                }
                else
                {
                    return x.id.CompareTo(y.id);
                }
            });
            return sortlist.ToArray();
        }

        public HandBookConfiger[] GetLandeDatas(int landeID)
        {
            var aLLHB = new HandBookConfiger[_handBookConfigDictionary.Count];
            _handBookConfigDictionary.Values.CopyTo(aLLHB, 0);
            List<HandBookConfiger> alldat = new List<HandBookConfiger>(aLLHB);
            KItemSuit[] allsuite = KItemManager.Instance.GetSuits();
            KItemBuilding[] allbuildings = KItemManager.Instance.GetBuildings();
            Dictionary<int, HandBookConfiger> suiteDict = new Dictionary<int, HandBookConfiger>();
            Dictionary<int, HandBookConfiger> newsuiteDict = new Dictionary<int, HandBookConfiger>();
            foreach (var item in _handBookConfigDictionary.Values)
            {
                if (item.tag.Contains(4) && item.tag.Length == 1)
                {
                    HandBookConfiger onenewsuitHB = new HandBookConfiger();
                    Debug.Log("区域装饰物ID：" + item.id);
                    KItemBuilding onenewbuilding = KItemManager.Instance.GetBuilding(item.id);
                    KItemSuit onenewsuit = KItemManager.Instance.GetSuit(onenewbuilding.suitID);
                    onenewsuitHB.id = onenewsuit.itemID;
                    onenewsuitHB.name = onenewsuit.itemName;
                    onenewsuitHB.getInformation = onenewsuit.getInformation;
                    onenewsuitHB.tag =  new int[1] { onenewsuit.itemTag };
                    onenewsuitHB.bloom = 1;
                    if (!newsuiteDict.ContainsKey(onenewsuit.itemID))
                    {
                        if (item.learned == 1)
                        {
                            onenewsuitHB.learned = 1;
                        }
                        newsuiteDict.Add(onenewsuitHB.id, onenewsuitHB);
                    }
                }
            }
            Dictionary<int, HandBookConfiger> targetLandeDict = new Dictionary<int, HandBookConfiger>();          
            for (int i = 0; i < allbuildings.Length; i++)
            {
                if (allbuildings[i].suitID == landeID)
                {
                    targetLandeDict.Add(_handBookConfigDictionary[allbuildings[i].itemID].id, _handBookConfigDictionary[allbuildings[i].itemID]);
                }
            }
            return targetLandeDict.Values.ToArray();
        }
        #endregion
        #region 用于左侧列表toggle功能
        public UI.CatHandBookWindowItem selectedCat { get; private set; }
        public void ClearOtherSelect(Game.UI.CatHandBookWindowItem NEWselectItem)
        {
            if (selectedCat == null)
            {
                selectedCat = NEWselectItem;
                return;
            }
                selectedCat.SetSelect(false);
            if (NEWselectItem != selectedCat)
            {
                selectedCat = NEWselectItem;
            }
            else {
                selectedCat = null;
            }
        }
        #endregion 

        #region METHOD

        /// <summary>
        /// 客户端向服务器拉取所有的图鉴数据
        /// </summary>
        /// <param name="callback"></param>
        public void GetHandBooks(Callback callback)
        {          
            KUser.HandBookGets((code, message, data) =>
            {
                var protoDatas = data as ArrayList;
                if (protoDatas != null)
                {
                    for (int i = 0; i < protoDatas.Count; i++)
                    {
                        if (protoDatas[0] is Msg.ClientMessage.S2CGetHandbookResult)
                        {
                            var bookDatas = (Msg.ClientMessage.S2CGetHandbookResult)protoDatas[0];
                            for (int j = 0; j < bookDatas.Items.Count; j++)
                            {
                                if (!_handBookConfigDictionary.ContainsKey(bookDatas.Items[j]))
                                {
                                    Debug.Log("异常数据：" + bookDatas.Items[j]);
                                }
                                if (_handBookConfigDictionary.ContainsKey(bookDatas.Items[j]) && _handBookConfigDictionary[bookDatas.Items[j]].id == bookDatas.Items[j])
                                {
                                    _handBookConfigDictionary[bookDatas.Items[j]].learned = 1;
                                }
                            }
                        }
                        if (protoDatas.Count > 1)
                        {
                            if (protoDatas[1] is Msg.ClientMessage.S2CGetHandbookResult)
                            {
                                var bookDatas = (Msg.ClientMessage.S2CGetHandbookResult)protoDatas[1];
                                for (int j = 0; j < bookDatas.Items.Count; j++)
                                {
                                    if (!intListAllFinishLande.Contains(bookDatas.Items[j]))
                                    {
                                        intListAllFinishLande.Add(bookDatas.Items[j]);
                                    }
                                }
                            }
                        }
                    }
                    if (callback != null)
                    {
                        callback(code, message, data);
                    }
                }
            });
        }
        public void Process(object data)
        {
            if (data is Msg.ClientMessage.S2CNewHandbookItemNotify)
            {
                var originData = (Msg.ClientMessage.S2CNewHandbookItemNotify)data;
              //响应其他状况的触发
            }
        }
        #endregion

        #region UNITY
        //Fieldguide
        public void Start()
        {
            TextAsset textAsset;
            KAssetManager.Instance.TryGetExcelAsset("Fieldguide", out textAsset);
            if (textAsset)
            {
                var table = textAsset.bytes.ToJsonTable();
                if (table != null)
                {
                    Load(table);
                }
            }
        }

        private void Load(Hashtable table)
        {
            var handbookList = table.GetArrayList("Fieldguide");
            if (handbookList != null && handbookList.Count > 0)
            {
                _handBookConfigDictionary.Clear();

                var tmpT = new Hashtable();
                var tmpL0 = (ArrayList)handbookList[0];
                for (int i = 1; i < handbookList.Count; i++)
                {
                    var tmpLi = (ArrayList)handbookList[i];
                    for (int j = 0; j < tmpL0.Count; j++)
                    {
                        tmpT[tmpL0[j]] = tmpLi[j];
                    }
                    var HBConfig = new HandBookConfiger
                    {
                        id = tmpT.GetInt("Id"),
                        name = tmpT.GetString("Name"),
                        tag = tmpT.GetArray<int>("Tage"),
                        bloom = tmpT.GetInt("Turnon"),
                    };
                    _handBookConfigDictionary.Add(HBConfig.id, HBConfig);
                }
            }
        }
        /// <summary>
        /// 区域套装集齐后领取该功能
        /// </summary>
        /// <param name="suitID"></param>
        public void GetLandeAward(int suitID, Callback callback) {
            CurrentAwardArry = new Msg.ClientMessage.ItemInfo[] { };
            List<Msg.ClientMessage.ItemInfo> lsitAward = new List<Msg.ClientMessage.ItemInfo>();
            KUser.GetLandeAward(suitID, (code, message, AnimData) => {
            var protoDatas = AnimData as ArrayList;
                if (protoDatas != null)
                {
                    for (int i = 0; i < protoDatas.Count; i++)
                    {
                        if (protoDatas[i] is Msg.ClientMessage.S2CGetSuitHandbookRewardResult)
                        {
                            var bookDatas = (Msg.ClientMessage.S2CGetSuitHandbookRewardResult)protoDatas[i];
                            for (int j = 0; j < bookDatas.Rewards.Count; j++)
                            {
                                lsitAward.Add(bookDatas.Rewards[j]);
                            }
                            if (!intListAllFinishLande.Contains(suitID))
                            {
                                intListAllFinishLande.Add(suitID);
                            }
                            CurrentAwardArry = lsitAward.ToArray();
                            for (int k = 0; k < CurrentAwardArry.Length; k++)
                            {
                                Debug.Log("罗列：" + CurrentAwardArry[i].ItemCfgId);
                            }
                            Debug.Log("-><color=#9400D3>" + "[日志] [KHandBookManager] [GetLandeAward] 向服务器发送领取区域奖励，回馈：" + bookDatas.Rewards.Count + "</color>");
                        }
                    }
                    if (callback != null)
                    {
                        callback(code, message, AnimData);
                    }
                }
            });
        }
        /// <summary>
        /// 区域奖励领取到的物品列表
        /// </summary>
        public Msg.ClientMessage.ItemInfo[] CurrentAwardArry { private set; get; }
        #endregion

        #region STATIC

        #endregion
    }
}
