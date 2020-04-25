/** 
 *FileName:     RowAndColElimainateLogic.cs 
 *Author:       HeJunJie 
 *Version:      1.0 
 *UnityVersion：5.6.2f1
 *Date:         2017-08-01 
 *Description:    
 *History: 
*/
using System.Collections.Generic;
using UnityEngine;

namespace Game.Match3
{
    /// <summary>
    /// 行与列消除
    /// </summary>
    public class RowAndColElimainateLogic : EliminateBase
    {
        public override void ProcessEliminateLogic(params object[] args)
        {
            base.ProcessEliminateLogic(args);
            int x1 = (int)args[0];
            int y1 = (int)args[1];
            int x2 = (int)args[2];
            int y2 = (int)args[3];
            int e = (int)args[4];

            M3Item item1 = M3ItemManager.Instance.gridItems[x1, y1];
            M3Item item2 = M3ItemManager.Instance.gridItems[x2, y2];
            if (item1 != null && item2 != null)
            {
                item1.itemInfo.GetElement().crushWithOutSpecial = true;
                item2.itemInfo.GetElement().crushWithOutSpecial = true;

                // 管你摇头点头呢都是以手指结束点为十字交叉点消除
                int crossX = e == 2 ? x2 : x1;
                int crossY = e == 2 ? y2 : y1;

                // 先检测触发点是否有草坪，有的话所有消除组都铺
                bool hasLawn = EliminateManager.Instance.CheckHasLawn(x1, y1);
                if (!hasLawn)
                {
                    hasLawn = EliminateManager.Instance.CheckHasLawn(x2, y2);
                }

                List<Int2> itemList = M3GameManager.Instance.GetLineAllItem(crossX, crossY, false, true);
                ProcessSpecialList(itemList, ItemSpecial.fColumn, crossX, crossY, hasLawn);

                List<Int2> itemList2 = M3GameManager.Instance.GetLineAllItem(crossX, crossY, true, true);
                ProcessSpecialList(itemList2, ItemSpecial.fRow, crossX, crossY, hasLawn);

                Vector3 tmp = M3Supporter.Instance.GetItemPositionByGrid(crossX, crossY);
                if (!M3GameManager.Instance.isAutoAI)
                    M3FxManager.Instance.FireArrow(tmp, true);
                if (!M3GameManager.Instance.isAutoAI)
                    M3FxManager.Instance.FireArrow(tmp, false);
            }

            //if (e == 2) //第一个为 点头，第二个为 摇头
            //{
            //    //Vector3 vec1 = M3ItemManager.Instance.gridItems[x2, y2].itemView.itemTransform.position;
            //    //Vector3 vec2 = M3ItemManager.Instance.gridItems[x1, y1].itemView.itemTransform.position;
            //    //if (M3ItemManager.Instance.gridItems[x1, y1] != null)
            //    //    M3ItemManager.Instance.gridItems[x1, y1].OnSpecialCrush(ItemSpecial.fRowAndCol);
            //    //if (M3ItemManager.Instance.gridItems[x2, y2] != null)
            //    //    M3ItemManager.Instance.gridItems[x2, y2].OnSpecialCrush(ItemSpecial.fRowAndCol);
            //    M3Item item1 = M3ItemManager.Instance.gridItems[x1, y2];
            //    M3Item item2 = M3ItemManager.Instance.gridItems[x2, y2];
            //    if (item1 != null && item2 != null)
            //    {
            //        item1.itemInfo.GetElement().crushWithOutSpecial = true;
            //        item2.itemInfo.GetElement().crushWithOutSpecial = true;

            //        Vector3 tmp = M3Supporter.Instance.GetItemPositionByGrid(x2, y2);
            //        if (!M3GameManager.Instance.isAutoAI)
            //            M3FxManager.Instance.FireArrow(tmp, false);
            //        List<Int2> itemList = M3GameManager.Instance.GetLineAllItem(x2, y2, true, true);
            //        ProcessSpecialList(itemList, ItemSpecial.fRow);

            //        Vector3 tmp2 = M3Supporter.Instance.GetItemPositionByGrid(x1, y1);
            //        if (!M3GameManager.Instance.isAutoAI)
            //            M3FxManager.Instance.FireArrow(tmp2, true);
            //        List<Int2> itemList2 = M3GameManager.Instance.GetLineAllItem(x1, y1, false, true);
            //        ProcessSpecialList(itemList2, ItemSpecial.fColumn);
            //    }

            //}
            //else //第一个为 摇头，第二个为 点头
            //{
            //    //if (M3ItemManager.Instance.gridItems[x1, y1] != null)
            //    //    M3ItemManager.Instance.gridItems[x1, y1].OnSpecialCrush(ItemSpecial.fRowAndCol);
            //    //if (M3ItemManager.Instance.gridItems[x2, y2] != null)
            //    //    M3ItemManager.Instance.gridItems[x2, y2].OnSpecialCrush(ItemSpecial.fRowAndCol);
            //    M3Item item1 = M3ItemManager.Instance.gridItems[x1, y2];
            //    M3Item item2 = M3ItemManager.Instance.gridItems[x2, y2];
            //    if (item1 != null && item2 != null)
            //    {
            //        item1.itemInfo.GetElement().crushWithOutSpecial = true;
            //        item2.itemInfo.GetElement().crushWithOutSpecial = true;
            //        Vector3 tmp = M3Supporter.Instance.GetItemPositionByGrid(x2, y2);

            //        List<Int2> itemList = M3GameManager.Instance.GetLineAllItem(x2, y2, false, true);
            //        ProcessSpecialList(itemList, ItemSpecial.fColumn);

            //        Vector3 tmp2 = M3Supporter.Instance.GetItemPositionByGrid(x1, y1);

            //        List<Int2> itemList2 = M3GameManager.Instance.GetLineAllItem(x1, y1, true, true);
            //        ProcessSpecialList(itemList2, ItemSpecial.fRow);
            //        if (!M3GameManager.Instance.isAutoAI)
            //            M3FxManager.Instance.FireArrow(tmp, true);
            //        if (!M3GameManager.Instance.isAutoAI)
            //            M3FxManager.Instance.FireArrow(tmp2, false);
            //    }
            //}

        }


    }
}