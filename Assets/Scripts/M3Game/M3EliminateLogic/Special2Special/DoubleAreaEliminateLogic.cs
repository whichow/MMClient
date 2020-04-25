/** 
 *FileName:     DoubleAreaEliminateLogic.cs 
 *Author:       HeJunJie 
 *Version:      1.0 
 *UnityVersion：5.6.2f1
 *Date:         2017-08-03 
 *Description:    
 *History: 
*/
using System.Collections.Generic;
using UnityEngine;

namespace Game.Match3
{
    /// <summary>
    /// 两个炸弹
    /// </summary>
    public class DoubleAreaEliminateLogic : EliminateBase
    {

        public override void ProcessEliminateLogic(params object[] args)
        {
            base.ProcessEliminateLogic(args);
            int x1 = (int)args[0];
            int y1 = (int)args[1];
            int x2 = (int)args[2];
            int y2 = (int)args[3];
            int isRow = (int)args[4];
            M3ItemManager.Instance.gridItems[x2, y2].itemInfo.GetElement().crushWithOutSpecial = true;
            M3ItemManager.Instance.gridItems[x1, y1].itemInfo.GetElement().crushWithOutSpecial = true;
            M3ItemManager.Instance.gridItems[x2, y2].crushScore = SpecialEliminateScore.DoubleBoom;

            // 先检测触发点是否有草坪，有的话所有消除组都铺
            bool hasLawn = EliminateManager.Instance.CheckHasLawn(x1, y1);
            if (!hasLawn)
            {
                hasLawn = EliminateManager.Instance.CheckHasLawn(x2, y2);
            }
            // 设置草坪
            if (hasLawn)
            {
                M3ItemManager.Instance.gridItems[x1, y1].itemInfo.GetElement().needCreateLawn = true;
                M3ItemManager.Instance.gridItems[x2, y2].itemInfo.GetElement().needCreateLawn = true;
            }

            //M3ItemManager.Instance.gridItems[x1, y1].OnSpecialCrush(ItemSpecial.fDoubleArea);
            //M3ItemManager.Instance.gridItems[x2, y2].OnSpecialCrush(ItemSpecial.fDoubleArea);

            List<Int2> list = new List<Int2>();
            if (isRow == 0)
            {
                if (x1 > x2)
                    list = GetDoubleAreaItem(x1, y1, x2, y2);
                else
                    list = GetDoubleAreaItem(x2, y2, x1, y1);
            }
            else
            {
                if (y1 < y2)
                    list = GetDoubleAreaItem(x1, y1, x2, y2);
                else
                    list = GetDoubleAreaItem(x2, y2, x1, y1);
            }

            ProcessSpecialList(list, ItemSpecial.fDoubleArea, x1, y1, hasLawn);
            //for (int i = 0; i < list.Count; i++)
            //{
            //    if (list[i] != null)
            //    {
            //        list[i].OnSpecialCrush(ItemSpecial.fDoubleArea);
            //    }
            //}
        }

        private List<Int2> GetDoubleAreaItem(int x1, int y1, int x2, int y2)
        {
            List<Int2> itemList = new List<Int2>();
            M3Item tmp;
            if (Mathf.Abs(x1 - x2) > 0)//纵向
            {
                for (int i = 1; i < 5; i++)
                {
                    for (int j = 0; j < 5 - i; j++)
                    {
                        if (y2 - i >= 0 && x2 - j >= 0)
                        {
                            tmp = M3ItemManager.Instance.gridItems[x2 - j, y2 - i];
                            if (tmp != null && !tmp.isCrushing)
                                itemList.Add(new Int2(tmp.itemInfo.posX, tmp.itemInfo.posY));
                        }
                        if (y2 + i < M3Config.GridWidth && x2 - j >= 0)
                        {
                            tmp = M3ItemManager.Instance.gridItems[x2 - j, y2 + i];
                            if (tmp != null && !tmp.isCrushing)
                                itemList.Add(new Int2(tmp.itemInfo.posX, tmp.itemInfo.posY));

                        }
                        if (y1 - i >= 0 && x1 + j < M3Config.GridHeight)
                        {
                            tmp = M3ItemManager.Instance.gridItems[x1 + j, y1 - i];
                            if (tmp != null && !tmp.isCrushing)
                                itemList.Add(new Int2(tmp.itemInfo.posX, tmp.itemInfo.posY));

                        }
                        if (y1 + i < M3Config.GridWidth && x1 + j < M3Config.GridHeight)
                        {
                            tmp = M3ItemManager.Instance.gridItems[x1 + j, y1 + i];
                            if (tmp != null && !tmp.isCrushing)
                                itemList.Add(new Int2(tmp.itemInfo.posX, tmp.itemInfo.posY));
                        }
                    }
                }
                for (int i = 0; i < 5; i++)
                {
                    if (x2 - i >= 0)
                    {
                        tmp = M3ItemManager.Instance.gridItems[x2 - i, y2];
                        if (tmp != null && !tmp.isCrushing)
                            itemList.Add(new Int2(tmp.itemInfo.posX, tmp.itemInfo.posY));
                    }
                    if (x1 + i < M3Config.GridHeight)
                    {
                        tmp = M3ItemManager.Instance.gridItems[x1 + i, y1];
                        if (tmp != null && !tmp.isCrushing)
                            itemList.Add(new Int2(tmp.itemInfo.posX, tmp.itemInfo.posY));
                    }
                }
            }
            else//横向
            {
                for (int i = 1; i < 5; i++)
                {
                    for (int j = 0; j < 5 - i; j++)
                    {
                        if (x2 - i >= 0 && y2 + j < M3Config.GridWidth)
                        {
                            tmp = M3ItemManager.Instance.gridItems[x2 - i, y2 + j];
                            if (tmp != null && !tmp.isCrushing)
                                itemList.Add(new Int2(tmp.itemInfo.posX, tmp.itemInfo.posY));
                        }
                        if (x2 + i < M3Config.GridHeight && y2 + j < M3Config.GridWidth)
                        {
                            tmp = M3ItemManager.Instance.gridItems[x2 + i, y2 + j];
                            if (tmp != null && !tmp.isCrushing)
                                itemList.Add(new Int2(tmp.itemInfo.posX, tmp.itemInfo.posY));
                        }
                        if (x1 - i >= 0 && y1 - j >= 0)
                        {
                            tmp = M3ItemManager.Instance.gridItems[x1 - i, y1 - j];
                            if (tmp != null && !tmp.isCrushing)
                                itemList.Add(new Int2(tmp.itemInfo.posX, tmp.itemInfo.posY));
                        }
                        if (x1 + i < M3Config.GridHeight && y1 - j >= 0)
                        {
                            tmp = M3ItemManager.Instance.gridItems[x1 + i, y1 - j];
                            if (tmp != null && !tmp.isCrushing)
                                itemList.Add(new Int2(tmp.itemInfo.posX, tmp.itemInfo.posY));
                        }
                    }
                }
                for (int i = 0; i < 5; i++)
                {
                    if (y1 - i >= 0)
                    {
                        tmp = M3ItemManager.Instance.gridItems[x1, y1 - i];
                        if (tmp != null && !tmp.isCrushing)
                            itemList.Add(new Int2(tmp.itemInfo.posX, tmp.itemInfo.posY));
                    }
                    if (y2 + i < M3Config.GridWidth)
                    {
                        tmp = M3ItemManager.Instance.gridItems[x2, y2 + i];
                        if (tmp != null && !tmp.isCrushing)
                            itemList.Add(new Int2(tmp.itemInfo.posX, tmp.itemInfo.posY));
                    }
                }
            }
            return itemList;
        }

    }
}