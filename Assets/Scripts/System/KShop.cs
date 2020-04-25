// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 2017-10-30
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KShop" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;

namespace Game
{
    using Callback = System.Action<int, string, object>;

    /// <summary>
    /// 商店数据
    /// </summary>
    public class KShop : KGameModule
    {
        #region Field

        KItemPack[] _specialPacks;
        KItemPack[] _friendPacks;
        KItemPack[] _charmPacks;
        KItemPack[] _rmbPacks;
        KItemPack[] _stonePacks;

        #endregion

        #region Method

        ////1-特殊商店
        ////2-友情点商店
        ////3-魅力勋章商店
        ////4-人民币商店
        ////5-魂石商店
        public KItemPack[] GetPacks(int type)
        {
            switch (type)
            {
                case 1:
                    if (_specialPacks == null)
                    {
                        var packs = KItemManager.Instance.GetPacks();
                        _specialPacks = System.Array.FindAll(packs, p => p.itemTag == 1);
                    }
                    System.Array.Sort(_specialPacks, (p1, p2) => p1.itemID.CompareTo(p2.itemID));
                    return _specialPacks;
                case 2:
                    if (_friendPacks == null)
                    {
                        var packs = KItemManager.Instance.GetPacks();
                        _friendPacks = System.Array.FindAll(packs, p => p.itemTag == 2);
                    }
                    System.Array.Sort(_friendPacks, (p1, p2) => p1.itemID.CompareTo(p2.itemID));
                    return _friendPacks;
                case 3:
                    if (_charmPacks == null)
                    {
                        var packs = KItemManager.Instance.GetPacks();
                        _charmPacks = System.Array.FindAll(packs, p => p.itemTag == 3);
                    }
                    System.Array.Sort(_charmPacks, (p1, p2) => p1.itemID.CompareTo(p2.itemID));
                    return _charmPacks;
                case 4:
                    if (_rmbPacks == null)
                    {
                        var packs = KItemManager.Instance.GetPacks();
                        _rmbPacks = System.Array.FindAll(packs, p => p.itemTag == 4);
                    }
                    System.Array.Sort(_rmbPacks, (p1, p2) => p1.itemID.CompareTo(p2.itemID));
                    return _rmbPacks;
                case 5:
                    if (_stonePacks == null)
                    {
                        var packs = KItemManager.Instance.GetPacks();
                        _stonePacks = System.Array.FindAll(packs, p => p.itemTag == 5);
                    }
                    System.Array.Sort(_stonePacks, (p1, p2) => p1.itemID.CompareTo(p2.itemID));
                    return _stonePacks;
            }
            return null;
        }

        public KItemPack[] GetRmbItems()
        {
            var packs = KItemManager.Instance.GetPacks();
            return System.Array.FindAll(packs, p => !string.IsNullOrEmpty(p.bundleId));
        }

        /// <summary>
        /// 每次进商店
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="callback"></param>
        public void GetItems(int shopId, Callback callback)
        {
            KUser.ShopGetItems(shopId, (code, message, data) =>
             {
                 if (code == 0)
                 {
                     OnGetItems(code, message, data);
                 }
                 if (callback != null)
                 {
                     callback(code, message, data);
                 }
             });
        }

        public void BuyItem(int id, int count, Callback callback)
        {
            KUser.BuyItem(id, count, callback);
        }

        private void OnGetItems(int code, string message, object data)
        {
            var protoDatas = data as ArrayList;
            if (protoDatas != null)
            {
                for (int i = 0; i < protoDatas.Count; i++)
                {
                    var protoData = protoDatas[i];

                    if (protoData is Msg.ClientMessage.S2CShopItemsResult)
                    {
                        var originData = (Msg.ClientMessage.S2CShopItemsResult)protoData;

                        var shopId = originData.ShopId;
                        var items = originData.Items;

                        var list = new List<KItemPack>();

                        foreach (var item in items)
                        {
                            var pack = KItemManager.Instance.GetPack(item.ItemId);
                            if (pack != null)
                            {
                                pack.remainCount = item.LeftNum;
                                list.Add(pack);
                            }
                        }

                        switch (shopId)
                        {
                            case 1:
                                _specialPacks = list.ToArray();
                                break;
                            case 2:
                                _friendPacks = list.ToArray();
                                break;
                            case 3:
                                _charmPacks = list.ToArray();
                                break;
                            case 4:
                                _rmbPacks = list.ToArray();
                                break;
                            case 5:
                                _stonePacks = list.ToArray();
                                break;
                        }
                    }
                }
            }
        }

        #endregion

        #region Unity    

        public static KShop Instance;

        private void Awake()
        {
            Instance = this;
        }

        #endregion
    }
}
