using Game.DataModel;
using Msg.ClientMessage;
using System.Collections.Generic;

namespace Game
{
    public class BagDataModel : DataModelBase<BagDataModel>
    {
        private Dictionary<int, ItemInfo> _dictAllItems = new Dictionary<int, ItemInfo>();

        public void ExeUpdateItem(S2CItemsInfoUpdate value)
        {
            if (value.Items == null || value.Items.Count == 0)
                return;
            List<int> changeIds = new List<int>();
            ItemInfo info;
            for (int i = 0; i < value.Items.Count; i++)
            {
                info = value.Items[i];
                if (!changeIds.Contains(info.ItemCfgId))
                    changeIds.Add(info.ItemCfgId);
                if (_dictAllItems.ContainsKey(info.ItemCfgId))
                {
                    _dictAllItems[info.ItemCfgId].ItemNum = info.ItemNum;
                    if (_dictAllItems[info.ItemCfgId].ItemNum <= 0)
                        _dictAllItems.Remove(info.ItemCfgId);
                }
                else
                    _dictAllItems.Add(info.ItemCfgId, info);
            }
            DispatchEvent(BagEvent.BagDataRefresh);
        }

        public void ExeBagData(S2CGetItemInfos value)
        {
            _dictAllItems.Clear();
            for (int i = 0; i < value.Items.Count; i++)
            {
                if (value.Items[i].ItemNum > 0)
                    _dictAllItems.Add(value.Items[i].ItemCfgId, value.Items[i]);
            }
            DispatchEvent(BagEvent.BagAllDataRefresh);
        }

        public void ExeSellItem(S2CSellItemResult value)
        {
            DispatchEvent(BagEvent.BagSellItem);
        }

        public void ExeUseItem(S2CUseItem value)
        {
            EventData eventData = new EventData();
            eventData.Data = value.CostItem;
            DispatchEvent(BagEvent.BagUseItem, eventData);
        }

        public List<ItemInfo> GetFormByType(int type)
        {
            List<ItemInfo> lstItemInfo = new List<ItemInfo>();
            Dictionary<int, ItemInfo>.ValueCollection valColl = _dictAllItems.Values;
            ItemXDM itemXDM;
            foreach (ItemInfo info in valColl)
            {
                itemXDM = XTable.ItemXTable.GetByID(info.ItemCfgId);
                if (itemXDM.Tag == ItemTagConst.Attire && itemXDM.EquipType == type)
                    lstItemInfo.Add(info);
            }
            lstItemInfo.Sort((x, y) => x.ItemCfgId.CompareTo(y.ItemCfgId));
            return lstItemInfo;
        }

        public List<ItemInfo> GetBagItemDataByType(int tagType)
        {
            List<ItemInfo> lstItemInfo = new List<ItemInfo>();
            Dictionary<int, ItemInfo>.ValueCollection valColl = _dictAllItems.Values;
            ItemXDM itemXDM;
            if (tagType > 0)
            {
                foreach (ItemInfo info in valColl)
                {
                    itemXDM = XTable.ItemXTable.GetByID(info.ItemCfgId);
                    if (itemXDM.Tag == tagType && itemXDM.Type != ItemTypeConst.Resource && itemXDM.Type != ItemTypeConst.ThreeIntoProp)
                        lstItemInfo.Add(info);
                }
            }
            else
            {
                foreach (ItemInfo info in valColl)
                {
                    itemXDM = XTable.ItemXTable.GetByID(info.ItemCfgId);
                    if (itemXDM.Type != ItemTypeConst.Resource && itemXDM.Type != ItemTypeConst.ThreeIntoProp && itemXDM.Type != ItemTypeConst.Head && itemXDM.Type != ItemTypeConst.Attire)
                        lstItemInfo.Add(info);
                }
            }
            lstItemInfo.Sort(OnSortInfo);
            return lstItemInfo;
        }

        public int OnSortInfo(ItemInfo v1, ItemInfo v2)
        {
            ItemXDM xdm1 = XTable.ItemXTable.GetByID(v1.ItemCfgId);
            ItemXDM xdm2 = XTable.ItemXTable.GetByID(v2.ItemCfgId);
            int index = xdm2.Tag.CompareTo(xdm1.Tag);
            if (index != 0)
                return index;
            index = xdm2.Rarity.CompareTo(xdm1.Rarity);
            if (index != 0)
                return index;
            index = xdm1.ID.CompareTo(xdm2.ID);
            return index;
        }

        public int GetItemCountById(int itemCfgId)
        {
            if (_dictAllItems.ContainsKey(itemCfgId))
                return _dictAllItems[itemCfgId].ItemNum;
            return 0;
        }

        public ItemInfo GetItemById(int itemCfgId)
        {
            if (_dictAllItems.ContainsKey(itemCfgId))
                return _dictAllItems[itemCfgId];
            return null;
        }
    }

    public class ItemIDConst
    {
        /// <summary>
        /// 金币
        /// </summary>
        public const int Gold = 2;
        /// <summary>
        /// 钻石
        /// </summary>
        public const int Diamond = 3;
        /// <summary>
        /// 友情点
        /// </summary>
        public const int FriendPoint = 6;
        /// <summary>
        /// 魅力值
        /// </summary>
        public const int Charm = 7;
        /// <summary>
        /// 魂石
        /// </summary>
        public const int SoulStone = 9;
        /// <summary>
        /// 魅力徽章
        /// </summary>
        public const int CharmBadge = 10;
        /// <summary>
        /// 加速消耗货币
        /// </summary>
        public const int TimeProp = 11;
        /// <summary>
        /// 行动力
        /// </summary>
        public const int Vigour = 14;
        /// <summary>
        /// 低级抽卡卷
        /// </summary>
        public const int LowCard = 201;
        /// <summary>
        /// 中级抽卡卷
        /// </summary>
        public const int MidCard = 202;
        /// <summary>
        /// 高级抽卡卷
        /// </summary>
        public const int HighCard = 203;
    }

    public class ItemTypeConst
    {
        public const int Resource = 1;//资源
        public const int DrawCard = 2;//抽卡卷
        public const int RemoProp = 3;//清除道具
        public const int ObstacleProp = 4;//障碍物道具
        public const int OrnamentMaterial = 5;//装饰物材料
        public const int FosterCard = 6;//寄养卡
        public const int CatFragment = 7;//猫咪碎片
        public const int PhysicalProp = 8;//体力道具
        public const int PopProp = 9;//跳跳道具
        public const int ThreeIntoProp = 10;//三消道具
        public const int Head = 11;//头像
        public const int Attire = 12;//装扮
    }

    public class ItemRarityConst
    {
        public const int RarityOne = 1;
        public const int RarityTwo = 2;
        public const int RarityThree = 3;
        public const int RarityFour = 4;
        public const int RarityFive = 5;
    }

    public class ItemTagConst
    {
        public const int AllItem = 0;//全部
        public const int Fragment = 1;//碎片
        public const int Material = 2;//材料
        public const int Consumable = 3;//消耗品
        public const int Head = 4;//头像
        public const int Attire = 5;//装扮
    }

}
