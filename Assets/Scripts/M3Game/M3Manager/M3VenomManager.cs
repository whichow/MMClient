using System;
using System.Collections.Generic;

namespace Game.Match3
{
    public class M3VenomManager
    {
        private bool haveVenom = false;
        private bool haveVenomParent = false;
        public bool haveVenomCrush = false;

        public M3VenomManager()
        {
            Init();
        }

        public void ResetVars()
        {
            haveVenomCrush = false;
        }

        private void Init()
        {
            M3Item item;
            haveVenomCrush = false;
            item = CheckHaveVenom();
        }

        private M3Item CheckHaveVenom()
        {
            M3Item item = null;
            haveVenom = false;
            haveVenomParent = false;
            for (int i = 0; i < M3Config.GridHeight; i++)
            {
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    item = M3ItemManager.Instance.gridItems[i, j];
                    if (item != null && item.itemInfo.GetElement() != null
                        && (item.itemInfo.GetElement().eName == M3ElementType.VenomElement || item.itemInfo.GetElement().eName == M3ElementType.VenomParentElement))
                    {
                        haveVenom = true;
                        if (item.itemInfo.GetElement().eName == M3ElementType.VenomParentElement)
                            haveVenomParent = true;
                    }
                }
            }
            return item;
        }

        public bool CheckVenom()
        {
            bool flag = false;
            CheckHaveVenom();
            if (haveVenom && !haveVenomCrush)
            {
                if (haveVenomParent)
                {
                    M3Item item;
                    List<M3Item> list = new List<M3Item>();
                    for (int i = 0; i < M3Config.GridHeight; i++)
                    {
                        for (int j = 0; j < M3Config.GridWidth; j++)
                        {
                            item = M3ItemManager.Instance.gridItems[i, j];
                            if (item != null && item.itemInfo.GetElement() != null && item.itemInfo.GetElement().eName == M3ElementType.VenomParentElement)
                            {
                                list.Add(item);
                            }
                        }
                    }
                    var normalItems = GetInjectItem(list, ElementSpecial.None);

                    var rowSpecialItems = GetInjectItem(list, ElementSpecial.Row);
                    var colSpecialItems = GetInjectItem(list, ElementSpecial.Column);
                    var boomSpecialItems = GetInjectItem(list, ElementSpecial.Area);
                    var magicSpecialItems = GetInjectItem(list, ElementSpecial.Color);
                    flag = ApplyVenomToRandomGrid(normalItems, rowSpecialItems, colSpecialItems, boomSpecialItems, magicSpecialItems, true);
                    haveVenomCrush = true;
                }
                if (!flag)
                {
                    M3Item item;
                    List<M3Item> list = new List<M3Item>();
                    for (int i = 0; i < M3Config.GridHeight; i++)
                    {
                        for (int j = 0; j < M3Config.GridWidth; j++)
                        {
                            item = M3ItemManager.Instance.gridItems[i, j];
                            if (item != null && item.itemInfo.GetElement() != null && item.itemInfo.GetElement().eName == M3ElementType.VenomElement)
                            {
                                list.Add(item);
                            }
                        }
                    }
                    var normalItems = GetInjectItem(list, ElementSpecial.None);

                    var rowSpecialItems = GetInjectItem(list, ElementSpecial.Row);
                    var colSpecialItems = GetInjectItem(list, ElementSpecial.Column);
                    var boomSpecialItems = GetInjectItem(list, ElementSpecial.Area);
                    var magicSpecialItems = GetInjectItem(list, ElementSpecial.Color);
                    flag = ApplyVenomToRandomGrid(normalItems, rowSpecialItems, colSpecialItems, boomSpecialItems, magicSpecialItems);
                    haveVenomCrush = true;
                }
                return flag;
            }
            else
                return flag;
        }

        private bool ApplyVenomToRandomGrid(Dictionary<M3Item, int> normalItems, Dictionary<M3Item, int> rowSpecialItems, Dictionary<M3Item, int> colSpecialItems, Dictionary<M3Item, int> boomSpecialItems, Dictionary<M3Item, int> magicSpecialItems, bool isParent = false)
        {
            M3Item target = null;
            int num = 0;
            if (normalItems.Count > 0)
            {
                GetRandomItem(normalItems, ref target, ref num);
            }

            if (target == null && rowSpecialItems.Count > 0)
            {
                GetRandomItem(rowSpecialItems, ref target, ref num);
            }
            if (target == null && colSpecialItems.Count > 0)
            {
                GetRandomItem(colSpecialItems, ref target, ref num);
            }
            if (target == null && boomSpecialItems.Count > 0)
            {
                GetRandomItem(boomSpecialItems, ref target, ref num);
            }
            if (target == null && magicSpecialItems.Count > 0)
            {
                GetRandomItem(magicSpecialItems, ref target, ref num);
            }

            if (target == null)
                return false;

            var from = M3ItemManager.Instance.gridItems[target.itemInfo.posX - M3Const.DirectionOffset[num].x, target.itemInfo.posY - M3Const.DirectionOffset[num].y];
            int transfromId = from.itemInfo.GetElement().data.config.LinkID;
            if (!isParent)
            {
                Action cAction = delegate ()
                {
                    target.itemInfo.DestroyHighestElement();
                    target.AddElement(transfromId);
                };
                if (from.itemInfo.posX > target.itemInfo.posX)
                {
                    ((VenomElement)(from.itemInfo.GetElement())).PlayFenLieAnimation(M3Const.VenomAnimationKeys[0]);
                }
                else if (from.itemInfo.posX < target.itemInfo.posX)
                {
                    ((VenomElement)(from.itemInfo.GetElement())).PlayFenLieAnimation(M3Const.VenomAnimationKeys[1]);
                }
                else if (from.itemInfo.posY < target.itemInfo.posY)
                {
                    ((VenomElement)(from.itemInfo.GetElement())).PlayFenLieAnimation(M3Const.VenomAnimationKeys[3]);
                }
                else if (from.itemInfo.posY > target.itemInfo.posY)
                {
                    ((VenomElement)(from.itemInfo.GetElement())).PlayFenLieAnimation(M3Const.VenomAnimationKeys[2]);
                }

                FrameScheduler.instance.Add(M3Config.VenomFenlieFrame, cAction);
            }
            else
            {
                target.itemInfo.DestroyHighestElement();
                target.AddElement(transfromId);
            }

            return true;
        }

        private static void GetRandomItem(Dictionary<M3Item, int> normalItems, ref M3Item target, ref int num)
        {
            int count = normalItems.Count;
            int counter = 0;
            count = M3Supporter.Instance.GetRandomInt(0, count);

            foreach (var item in normalItems)
            {
                if (counter == count)
                {
                    target = item.Key;
                    num = item.Value;
                    break;
                }
                else
                {
                    counter++;
                }
            }
        }

        private Dictionary<M3Item, int> GetInjectItem(List<M3Item> list, ElementSpecial special)
        {
            Dictionary<M3Item, int> itemDictionary = new Dictionary<M3Item, int>();
            M3Item item;
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < M3Const.DirectionOffset.Length; j++)
                {
                    Int2 point = new Int2(list[i].itemInfo.posX + M3Const.DirectionOffset[j].x, list[i].itemInfo.posY + M3Const.DirectionOffset[j].y);
                    if (M3GameManager.Instance.CheckValid(point.x, point.y))
                    {
                        if (M3GameManager.Instance.CheckRopeBetweenItems(list[i].itemInfo.posX, list[i].itemInfo.posY, point.x, point.y))
                        {
                            continue;
                        }
                        item = M3ItemManager.Instance.gridItems[point.x, point.y];
                        if (item != null && item.itemInfo.GetElement() != null && !itemDictionary.ContainsKey(item))
                        {
                            switch (special)
                            {
                                case ElementSpecial.None:
                                    if (item.itemInfo.GetElement().eName == M3ElementType.NormalElement)
                                        itemDictionary.Add(item, j);
                                    break;
                                case ElementSpecial.Row:
                                    if (item.itemInfo.GetElement().eName == M3ElementType.SpecialElement)
                                        itemDictionary.Add(item, j);
                                    break;
                                case ElementSpecial.Column:
                                    if (item.itemInfo.GetElement().eName == M3ElementType.SpecialElement)
                                        itemDictionary.Add(item, j);
                                    break;
                                case ElementSpecial.Area:
                                    if (item.itemInfo.GetElement().eName == M3ElementType.SpecialElement
                                        && item.itemInfo.GetElement().GetSpecial() == ElementSpecial.Area)
                                        itemDictionary.Add(item, j);
                                    break;
                                case ElementSpecial.Color:
                                    if (item.itemInfo.GetElement().eName == M3ElementType.MagicCatElement)
                                        itemDictionary.Add(item, j);
                                    break;
                                default:
                                    break;
                            }

                        }
                    }
                }
            }
            return itemDictionary;
        }


    }
}