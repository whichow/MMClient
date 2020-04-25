using Game.DataModel;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Match3
{
    public class M3ItemManager : Singleton<M3ItemManager>
    {
        #region Static

        private const float BaseDistance = 1f;

        #endregion

        #region Field

        /// <summary>
        /// 
        /// </summary>
        public GameObject itemParent;
        /// <summary>
        /// 
        /// </summary>
        public GameObject itemPrefab;

        public GameObject CacheObj;

        public GameObject zombieParent;

        /// <summary>
        /// 
        /// </summary>
        private M3Item[,] _gridItems;

        #endregion

        #region Property

        /// <summary>
        /// 三消单元集合
        /// </summary>
        public M3Item[,] gridItems
        {
            get { return _gridItems; }
            //set { _gridItems = value; }
        }

        #endregion

        #region Method

        /// <summary>
        /// 初始化棋盘
        /// </summary>
        /// <param name="map"></param>
        public void CreateMap(M3CellData[,] map)
        {
            _gridItems = new M3Item[M3Config.GridHeight, M3Config.GridWidth];
            for (int x = 0; x < M3Config.GridHeight; x++)
            {
                for (int y = 0; y < M3Config.GridWidth; y++)
                {
                    if (map[x, y].isActive)
                    {
                        if (/*(map[x, y].randomElement != 1) &&*/ (!map[x, y].isEmpty))
                        {
                            List<int> list = new List<int>();
                            for (int i = 0; i < map[x, y].elementsList.Count; i++)
                            {
                                var ele = XTable.ElementXTable.GetByID(map[x, y].elementsList[i]);
                                if (ele != null && M3Const.ITEM_TYPE.Contains(ele.Type))
                                    list.Add(ele.ID);
                            }
                            CreateItem(x, y, list, true);
                        }
                    }
                }
            }
            for (int x = 0; x < M3Config.GridHeight; x++)
            {
                for (int y = 0; y < M3Config.GridWidth; y++)
                {
                    if (map[x, y].isActive)
                    {
                        if (map[x, y].isRandomElement && !map[x, y].isEmpty)
                        {
                            int id = RandomGenElementID(x, y);
                            var ele = CreateElement(id);
                            ele.Init(id, _gridItems[x, y]);
                            ele.OnCreate();
                            _gridItems[x, y].itemInfo.AddElement(ele);
                        }
                    }
                }
            }
        }

        public void ReloadMap(List<Element>[,] map)
        {
            var items = new M3Item[M3Config.GridHeight, M3Config.GridWidth];
            for (int x = 0; x < M3Config.GridHeight; x++)
            {
                for (int y = 0; y < M3Config.GridWidth; y++)
                {
                    if (map[x, y] != null)
                    {
                        CreateItem(x, y, map[x, y]);
                    }
                }
            }
        }

        public int RandomGenElementID(int x, int y)
        {
            var list = M3GameManager.Instance.level.GetSpawnList();
            while (list.Count > 0)
            {
                var e = GetRandomSpawnList(list);
                if (e == null)
                    return -1;
                ItemColor color = (ItemColor)(e.elementID - 1000);
                if (M3GameManager.Instance.FindColorMatchCount(x, y, color) < 3)
                {
                    return e.elementID;
                }
                else
                {
                    list.Remove(e);
                }
            }
            Debuger.LogError("GenElementError pos: " + x + "|" + y);
            return (int)ItemColor.fRed + M3Config.BaseElement;
        }

        public M3Item CreateItem(int x, int y, List<int> eleList, bool isFirst = false)
        {
            var item = new M3Item();
            if (itemParent != null)
            {
                var tmpGo = KPool.Spawn(itemPrefab);
                item.InitView(tmpGo);
            }
            item.InitData();
            List<Element> list = new List<Element>();
            Element ele = null;
            for (int i = 0; i < eleList.Count; i++)
            {
                ele = CreateElement(eleList[i]);
                ele.Init(eleList[i], item);
                if (ele.view != null)
                    ele.OnCreate();
                list.Add(ele);
            }
            item.itemInfo.Init(x, y, list, item);
            gridItems[x, y] = item;
            if (itemParent != null)
            {
                item.itemView.itemTransform.SetParent(itemParent.transform, false);
                item.itemView.itemTransform.name = x + "_" + y;
                //item.itemView.cacheLocalPosition = M3Supporter.Instance.GetItemPositionByGrid(x, y);
                item.coordinate = new Int2(x, y);
                item.itemView.itemTransform.localScale = Vector3.one;
            }
            if (isFirst && item.itemView != null && item.itemView.itemTransform != null)
            {
                item.itemView.itemTransform.localScale = Vector3.zero;
                KTweenUtils.ScaleTo(item.itemView.itemTransform, Vector3.one, 0.3f);
            }
            return item;
        }

        public M3Item CreateItem(int x, int y, List<Element> eleList)
        {
            var item = new M3Item();
            if (itemParent != null)
            {
                var tmpGo = KPool.Spawn(itemPrefab);
                item.InitView(tmpGo);
            }
            item.InitData();
            List<Element> list = new List<Element>();
            Element ele = null;
            for (int i = 0; i < eleList.Count; i++)
            {
                ele = eleList[i].Clone();
                ele.Init(eleList[i].data.config.ID, item);
                ele.InitClone(eleList[i], null);
                if (ele.view != null)
                    ele.OnCreate();
                list.Add(ele);
            }
            item.itemInfo.Init(x, y, list, item);
            gridItems[x, y] = item;
            if (itemParent != null)
            {
                item.itemView.itemTransform.SetParent(itemParent.transform, false);
                item.itemView.itemTransform.name = x + "_" + y;
                item.coordinate = new Int2(x, y);
                item.itemView.itemTransform.localScale = Vector3.one;
            }
            if (item.itemView != null && item.itemView.itemTransform != null)
            {
                item.itemView.itemTransform.localScale = Vector3.zero;
                KTweenUtils.ScaleTo(item.itemView.itemTransform, Vector3.one, 0.3f);
            }
            return item;
        }

        public Element CreateElement(int id)
        {
            var xdm = XTable.ElementXTable.GetByID(id);
            Element ele;
            switch (xdm.Type)
            {
                case M3ElementType.NormalElement:
                    ele = new NormalElement();
                    break;
                case M3ElementType.SpecialElement:
                    ele = new SpecialElement();
                    break;

                case M3ElementType.CatteryElement:
                    ele = new CatteryElement();
                    break;
                case M3ElementType.MagicBookElement:
                    ele = new BookElement();
                    break;
                case M3ElementType.LockElement:
                    ele = new CoverElement();
                    break;

                case M3ElementType.CrystalElement:
                    ele = new CrystalElement();
                    break;
                case M3ElementType.GiftElement:
                    ele = new GiftElement();
                    break;
                case M3ElementType.EnergyElement:
                    ele = new EnergyBottle();
                    break;
                case M3ElementType.MagicCatElement:
                    ele = new MagicCatElement();
                    break;
                case M3ElementType.GreyCoomElement:
                    ele = new GreyCoom();
                    break;
                case M3ElementType.BrownCoomElement:
                    ele = new BrownCoom();
                    break;
                case M3ElementType.VenomElement:
                    ele = new VenomElement();
                    break;
                case M3ElementType.VenomParentElement:
                    ele = new VenomParentElement();
                    break;
                case M3ElementType.CurtainElement:
                    ele = new CurtainElement();
                    break;

                case M3ElementType.BellElement:
                    ele = new BellElement();
                    break;
                case M3ElementType.WoolBall:
                    ele = new WoolBallElement();
                    break;
                    
                case M3ElementType.CoinElement:
                    ele = new CoinElement();
                    break;
                case M3ElementType.FishElement:
                    ele = new FishElement();
                    break;
                default:
                    ele = new Element();
                    break;
            }
            return ele;
        }

        public int GetSpecialElementID(ItemColor color, ElementSpecial special)
        {
            switch (special)
            {
                case ElementSpecial.None:
                    break;
                case ElementSpecial.Row:
                    return (M3Config.SpecialElement + (int)color);
                case ElementSpecial.Column:
                    return M3Config.SpecialElement + (int)color + M3Config.ColorCount;
                case ElementSpecial.Area:
                    return M3Config.SpecialElement + (int)color + M3Config.ColorCount * 2;
                case ElementSpecial.Color:
                    return 2019;
                default:
                    return -1;
            }
            return -1;
        }

        public int GetNormalElementID(ItemColor color)
        {
            switch (color)
            {
                case ItemColor.fNone:
                    break;
                case ItemColor.fRed:
                    return M3Const.Red_NormalElement;
                case ItemColor.fYellow:
                    return M3Const.Yellow_NormalElement;
                case ItemColor.fBlue:
                    return M3Const.Blue_NormalElement;
                case ItemColor.fGreen:
                    return M3Const.Green_NormalElement;
                case ItemColor.fPurple:
                    return M3Const.Purple_NormalElement;
                case ItemColor.fBrown:
                    return M3Const.Brown_NormalElement;
                case ItemColor.fEnergy:
                case ItemColor.fRandom:
                    break;
                default:
                    break;
            }
            return M3Const.Red_NormalElement;
        }

        public int GetCrystalElementID(ItemColor color)
        {
            switch (color)
            {
                case ItemColor.fNone:
                    break;
                case ItemColor.fRed:
                    return M3Const.Red_CrystalElement;
                case ItemColor.fYellow:
                    return M3Const.Yellow_CrystalElement;
                case ItemColor.fBlue:
                    return M3Const.Blue_CrystalElement;
                case ItemColor.fGreen:
                    return M3Const.Green_CrystalElement;
                case ItemColor.fPurple:
                    return M3Const.Purple_CrystalElement;
                case ItemColor.fBrown:
                    return M3Const.Brown_CrystalElement;
                case ItemColor.fEnergy:
                case ItemColor.fRandom:
                    break;
                default:
                    break;
            }
            return M3Const.Red_NormalElement;
        }

        public int GetGiftElementID(ItemColor color)
        {
            switch (color)
            {
                case ItemColor.fNone:
                    break;
                case ItemColor.fRed:
                    return M3Const.Red_GiftElement;
                case ItemColor.fYellow:
                    return M3Const.Yellow_GiftElement;
                case ItemColor.fBlue:
                    return M3Const.Blue_GiftElement;
                case ItemColor.fGreen:
                    return M3Const.Green_GiftElement;
                case ItemColor.fPurple:
                    return M3Const.Purple_GiftElement;
                case ItemColor.fBrown:
                    return M3Const.Brown_GiftElement;
                default:
                    return M3Const.Red_GiftElement;
            }
            return M3Const.Red_GiftElement;
        }


        /// <summary>
        /// 生成消除后产生的特殊元素（如炸弹）
        /// </summary>
        /// <param name="color"></param>
        /// <param name="special"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public M3Item SpawnSpecialItem(ItemColor color, ElementSpecial special, int x, int y)
        {
            if (gridItems[x, y] != null && !gridItems[x, y].isCrushing)
            {
                Debug.LogError("生成出错！改棋格已经有其他元素 " + x + "|" + y + gridItems[x, y].itemInfo.GetElement().GetSpecial());
                //Destroy(gridItems[x, y].gameObject);
            }
            M3Item tmpItem;
            if (special == ElementSpecial.Color)
                tmpItem = CreateItem(x, y, new List<int>() { 2019 });
            else if (special == ElementSpecial.Row)
                tmpItem = CreateItem(x, y, new List<int>() { (M3Config.SpecialElement + (int)color) });
            else if (special == ElementSpecial.Column)
                tmpItem = CreateItem(x, y, new List<int>() { (M3Config.SpecialElement + (int)color) + M3Config.ColorCount });
            else if (special == ElementSpecial.Area)
                tmpItem = CreateItem(x, y, new List<int>() { (M3Config.SpecialElement + (int)color) + M3Config.ColorCount * 2 });
            else
                tmpItem = CreateItem(x, y, new List<int>() { (M3Config.BaseElement + (int)color) });

            gridItems[x, y] = tmpItem;
            if (itemParent != null)
            {
                tmpItem.itemView.itemTransform.SetParent(itemParent.transform, false);
                tmpItem.itemView.itemTransform.localPosition = new Vector3(y, -x);
            }
            tmpItem.itemInfo.RefreshPos(x, y);

            return tmpItem;
        }

        public M3Item CreateItemById(int x, int y, int id)
        {
            M3Item item = M3ItemManager.Instance.CreateItem(x, y, new List<int>() { id });
            return item;
        }

        public void ChangeElement(M3Item item, int id)
        {
            item.itemInfo.DestroyPartakeElement();
            item.AddElement(id);
        }

        public void CacheItem(GameObject obj, bool stay)
        {
            obj.transform.SetParent(CacheObj.transform);
        }

        public RuleCode GetRandomSpawnList(List<RuleCode> list)
        {
            int totalWeight = 0;
            if (list == null)
            {
                Debug.LogError("List is NUll");
                return null;
            }
            for (int i = 0; i < list.Count; i++)
            {
                totalWeight += list[i].weight;
            }
            if (totalWeight <= 0)
                return null;
            int weight = M3Supporter.Instance.GetRandomInt(0, totalWeight);
            int tmp = 0;
            for (int i = 0; i < list.Count; i++)
            {
                tmp += list[i].weight;
                if (tmp > weight)
                {
                    return list[i];
                }
            }
            return null;
        }

        public void ReloadAllItem(List<Element>[,] map)
        {
            for (int i = 0; i < M3Config.GridHeight; i++)
            {
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    if (gridItems[i, j] != null)
                        gridItems[i, j].ItemDestroy();
                    gridItems[i, j] = null;
                }
            }
            ReloadMap(map);
        }

        #endregion

        #region Unity

        public void InitView()
        {
            zombieParent = GameObject.Find("Zombie");
            itemParent = M3GameManager.Instance.gameScreen.transform.Find("Board/Item").gameObject;
            GameObject go;
            KAssetManager.Instance.TryGetMatchPrefab("Item", out go);
            itemPrefab = go;
            CacheObj = M3GameManager.Instance.gameScreen.transform.Find("Board/CacheObj").gameObject;
        }

        #endregion

    }
}