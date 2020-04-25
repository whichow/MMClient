// ***********************************************************************
// Company          : 
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
namespace Game
{
    using Game.DataModel;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    using ShopCallback = System.Action<int, string, object>;

    /// <summary>
    /// KItemManager
    /// </summary>
    public class KItemManager : KGameModule
    {
        #region CONST F

        //private const int PLAYER_I = 0;
        //private const string PLAYER_K = "character";

        private const int SKILL_I = 1;
        private const string SKILL_K = "skill";

        private const int BUILDING_I = 2;
        private const string BUILDING_K = "building";

        private const int PROP_I = 3;
        private const string PROP_K = "prop";

        private const int PACK_I = 4;
        private const string PACK_K = "shop";

        private const int THING_I = 5;
        private const string THING_K = "things";

        private const int CROP_I = 6;
        private const string CROP_K = "crop";

        private const int OTHER_I = 7;
        private const string OTHER_K = "other";

        private const int FORMULA_I = 8;
        private const string FORMULA_K = "formula";

        private const int SUIT_I = 9;
        private const string SUIT_K = "Suit";

        //private const int FOSTERCARD_I = 10;
        //private const string FOSTERCARD_K = "foster";

        #endregion

        #region MODEL

        /// <summary>
        /// ItemGroup
        /// </summary>
        public class ItemGroup
        {
            public string name;
            public KItem[] items;
        }

        #endregion

        #region Field

        /// <summary>
        /// 
        /// </summary>
        private ItemGroup[] _allGroups = new ItemGroup[16];
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, KItem> _allItems = new Dictionary<int, KItem>(1024);

        #endregion

        #region GET 

        /// <summary>Get the item.</summary>
        /// <param name="id">The itemID.</param>
        /// <returns></returns>
        public KItem GetItem(int id)
        {
            KItem item;
            _allItems.TryGetValue(id, out item);
            return item;
        }

        ///// <summary>Gets the pack.</summary>
        ///// <param name="id">The identifier.</param>
        ///// <returns></returns>
        //public KItemCat GetCat(int id)
        //{
        //    KItem item;
        //    if (_allItems.TryGetValue(id, out item))
        //    {
        //        return (KItemCat)item;
        //    }
        //    var items = this.GetCats();
        //    return System.Array.Find(items, (it) => it.itemID == id);
        //}

        ///// <summary>Gets the packs.</summary>
        ///// <returns></returns>
        //public KItemCat[] GetCats()
        //{
        //    return (KItemCat[])_allGroups[PLAYER_I].items;
        //}

        /// <summary>Gets the pack.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public KItemSkill GetSkill(int id)
        {
            KItem item;
            if (_allItems.TryGetValue(id, out item))
            {
                return (KItemSkill)item;
            }
            var items = this.GetSkills();
            return System.Array.Find(items, (it) => it.itemID == id);
        }
        /// <summary>Gets the packs.</summary>
        /// <returns></returns>
        public KItemSkill[] GetSkills()
        {
            return (KItemSkill[])_allGroups[SKILL_I].items;
        }

        /// <summary>Gets the pack.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public KItemBuilding GetBuilding(int id)
        {
            KItem item;
            if (_allItems.TryGetValue(id, out item))
            {
                return (KItemBuilding)item;
            }
            var items = this.GetBuildings();
            return System.Array.Find(items, (it) => it.itemID == id);
        }
        /// <summary>Gets the packs.</summary>
        /// <returns></returns>
        public KItemBuilding[] GetBuildings()
        {
            return (KItemBuilding[])_allGroups[BUILDING_I].items;
        }

        /// <summary>Gets the pack.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public KItemPack GetPack(int id)
        {
            KItem item;
            if (_allItems.TryGetValue(id, out item))
            {
                return (KItemPack)item;
            }
            var packs = this.GetPacks();
            return System.Array.Find(packs, (p) => p.itemID == id);
        }
        /// <summary>Gets the packs.</summary>
        /// <returns></returns>
        public KItemPack[] GetPacks()
        {
            return (KItemPack[])_allGroups[PACK_I].items;
        }

        /// <summary>Gets the pack.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public KItemProp GetProp(int id)
        {
            KItem item;
            if (_allItems.TryGetValue(id, out item))
            {
                return (KItemProp)item;
            }
            var items = this.GetProps();
            return System.Array.Find(items, (it) => it.itemID == id);
        }
        /// <summary>Gets the packs.</summary>
        /// <returns></returns>
        public KItemProp[] GetProps()
        {
            return (KItemProp[])_allGroups[PROP_I].items;
        }

        /// <summary>Gets the pack.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public KItemThing GetThing(int id)
        {
            KItem item;
            if (_allItems.TryGetValue(id, out item))
            {
                return (KItemThing)item;
            }
            var items = this.GetThings();
            return System.Array.Find(items, (it) => it.itemID == id);
        }
        /// <summary>Gets the packs.</summary>
        /// <returns></returns>
        public KItemThing[] GetThings()
        {
            return (KItemThing[])_allGroups[THING_I].items;
        }

        /// <summary>Gets the pack.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public KItemCrop GetCrop(int id)
        {
            KItem item;
            if (_allItems.TryGetValue(id, out item))
            {
                return (KItemCrop)item;
            }
            var items = this.GetCrops();
            return System.Array.Find(items, (it) => it.itemID == id);
        }
        /// <summary>Gets the packs.</summary>
        /// <returns></returns>
        public KItemCrop[] GetCrops()
        {
            return (KItemCrop[])_allGroups[CROP_I].items;
        }

        /// <summary>Gets the pack.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public KItemFormula GetFormula(int id)
        {
            KItem item;
            if (_allItems.TryGetValue(id, out item))
            {
                return (KItemFormula)item;
            }
            var items = this.GetFormulas();
            return System.Array.Find(items, (it) => it.itemID == id);
        }
        /// <summary>Gets the packs.</summary>
        /// <returns></returns>
        public KItemFormula[] GetFormulas()
        {
            return (KItemFormula[])_allGroups[FORMULA_I].items;
        }

        /// <summary>Gets the pack.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public KItemSuit GetSuit(int id)
        {
            KItem item;
            if (_allItems.TryGetValue(id, out item))
            {
                return (KItemSuit)item;
            }
            var items = this.GetSuits();
            return System.Array.Find(items, (it) => it.itemID == id);
        }
        /// <summary>Gets the packs.</summary>
        /// <returns></returns>
        public KItemSuit[] GetSuits()
        {
            return (KItemSuit[])_allGroups[SUIT_I].items;
        }

        ///// <summary>Gets the item.</summary>
        ///// <param name="id">The identifier.</param>
        ///// <returns></returns>
        //public KItemFosterCard GetFosterCard(int id)
        //{
        //    KItem item;
        //    if (_allItems.TryGetValue(id, out item))
        //    {
        //        return (KItemFosterCard)item;
        //    }
        //    var items = this.GetFosterCards();
        //    return System.Array.Find(items, (it) => it.itemID == id);
        //}
        ///// <summary>Gets the item.</summary>
        ///// <returns></returns>
        //public KItemFosterCard[] GetFosterCards()
        //{
        //    return (KItemFosterCard[])_allGroups[FOSTERCARD_I].items;
        //}
        //public KItemFosterCard GetFosterCardbyBindId(int id)
        //{
        //    var cardList = GetFosterCards();
        //    return System.Array.Find(cardList, (it) => it.bindItemId == id);
        //}
        #endregion

        //#region OPER

        ///// <summary>Buys the item.</summary>
        ///// <param name="item">The item.</param>
        ///// <param name="callback">The callback.</param>
        //public bool BuyItem(KItem item, ShopCallback callback)
        //{
        //    if (item != null)
        //    {
        //        //switch (item.moneyType)
        //        //{
        //        //    case KShopItem.MoneyType.Coin:
        //        //        if (KUser.coin < item.moneyCost)
        //        //        {
        //        //            if (callback != null)
        //        //            {
        //        //                callback(21, "金币不足");
        //        //            }
        //        //            return false;
        //        //        }
        //        //        break;
        //        //    case KShopItem.MoneyType.Stone:
        //        //        if (KUser.stone < item.moneyCost)
        //        //        {
        //        //            if (callback != null)
        //        //            {
        //        //                callback(22, "钻石不足");
        //        //            }
        //        //            return false;
        //        //        }
        //        //        break;
        //        //}
        //    }
        //    return true;
        //}
        ///// <summary>Upgrade the item.</summary>
        ///// <param name="item">The item.</param>
        ///// <param name="callback">The callback.</param>
        //public void UpgradeItem(KItem item, ShopCallback callback)
        //{
        //}

        //#endregion

        //#region BILLING

        ///// <summary>
        ///// Called when [billing supported].
        ///// </summary>
        ///// <param name="supported">if set to <c>true</c> [supported].</param>
        //public void OnBillingSupported(bool supported)
        //{

        //}
        ///// <summary>
        ///// Called when [purchase cancelled].
        ///// </summary>
        ///// <param name="itemID">Name of the item.</param>
        //public void OnPurchaseCancelled(int itemID)
        //{
        //}
        ///// <summary>
        ///// Called when [purchase failed].
        ///// </summary>
        ///// <param name="itemName">Name of the item.</param>
        ///// <param name="developerPayload">The developer payload.</param>
        //public void OnPurchaseFailed(int itemID)
        //{
        //}
        ///// <summary>
        ///// Called when [purchase succeeded].
        ///// </summary>
        ///// <param name="itemName">Name of the item.</param>
        ///// <param name="developerPayload">The developer payload.</param>
        //public void OnPurchaseSucceeded(int itemID)
        //{

        //}

        //#endregion

        #region Load  

        public void Load(Hashtable shopTable)
        {
            _allGroups[BUILDING_I] = LoadItem<KItemBuilding>(shopTable, BUILDING_K);
            //_allGroups[PLAYER_I] = LoadItem<KItemCat>(shopTable, PLAYER_K);
            _allGroups[SKILL_I] = LoadItem<KItemSkill>(shopTable, SKILL_K);
            _allGroups[PROP_I] = LoadItem<KItemProp>(shopTable, PROP_K);
            _allGroups[PACK_I] = LoadItem<KItemPack>(shopTable, PACK_K);
            _allGroups[THING_I] = LoadItem<KItemThing>(shopTable, "ThingsXDM");
            _allGroups[CROP_I] = LoadItem<KItemCrop>(shopTable, CROP_K);
            _allGroups[OTHER_I] = LoadItem<KItem>(shopTable, OTHER_K);
            _allGroups[FORMULA_I] = LoadItem<KItemFormula>(shopTable, FORMULA_K);
            _allGroups[SUIT_I] = LoadItem<KItemSuit>(shopTable, SUIT_K);
            //_allGroups[FOSTERCARD_I] = LoadItem<KItemFosterCard>(shopTable, FOSTERCARD_K);

            System.Array.ForEach(_allGroups, group =>
            {
#if UNITY_EDITOR
                try
                {
#endif
                    if (group != null)
                    {
                        foreach (var item in group.items)
                        {
                            if (!_allItems.ContainsKey(item.itemID))
                            {
                                _allItems.Add(item.itemID, item);
                            }
                            else
                            {
                                Debuger.LogError("ID重复:" + item.itemName + item.itemID);
                                Debuger.LogError("已有ID:" + _allItems[item.itemID].itemName );
                            }
                        }
                    }
#if UNITY_EDITOR
                }
                catch(System.Exception e)
                {
                    Debug.LogError(group.name + " id repeat! " + e);
                }
#endif
            });

            foreach (var item in _allItems)
            {
                item.Value.LoadComplete();
            }
        }

        #endregion

        #region UNITY 

        public static KItemManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        public override void Load()
        {
            var table = new Hashtable();

            var assets = new string[] { /*PLAYER_K,*/ BUILDING_K, SKILL_K, PROP_K, THING_K, PACK_K, CROP_K, OTHER_K, FORMULA_K, SUIT_K/*, FOSTERCARD_K*/ };
            foreach (var asset in assets)
            {
                TextAsset tmpText;
                if (KAssetManager.Instance.TryGetExcelAsset(asset, out tmpText))
                {
                    if (tmpText)
                    {
                        var tmpTable = tmpText.bytes.ToJsonTable();
                        foreach (DictionaryEntry entry in tmpTable)
                        {
                            table.Add(entry.Key, entry.Value);
                        }
                    }
                }
            }
            Load(table);
        }

        #endregion

        #region STATIC 

        private static ItemGroup LoadItem<T>(Hashtable table, string key) where T : KItem, new()
        {
            var itemList = table.GetArrayList(key);
            if (itemList != null && itemList.Count > 0)
            {
                var tmpT = new Hashtable();
                var items = new T[itemList.Count - 1];
                for (int i = 0; i < items.Length; i++)
                {
                    var tmpL0 = (ArrayList)itemList[0];
                    var tmpLi = (ArrayList)itemList[i + 1];
                    for (int j = 0; j < tmpL0.Count; j++)
                    {
                        tmpT[tmpL0[j]] = tmpLi[j];
                    }
                    items[i] = new T();
                    items[i].Load(tmpT);
                }
                return new ItemGroup() { name = key, items = items };
            }

            return new ItemGroup() { name = key, items = new KItem[0] };
        }

        #endregion
    }
}
