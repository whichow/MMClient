/** 
 *FileName:     DoubleColEliminateLogic.cs 
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
    /// 双列特效消除
    /// </summary>
    public class DoubleColEliminateLogic : EliminateBase
    {

        public override void ProcessEliminateLogic(params object[] args)
        {
            base.ProcessEliminateLogic(args);
            int x1 = (int)args[0];
            int y1 = (int)args[1];
            int x2 = (int)args[2];
            int y2 = (int)args[3];
            bool isTransverse = (bool)args[4];
            if (isTransverse)
            {
                List<M3Item> item1List = M3GameManager.Instance.GetLineItem(x1, y1, false, true);
                List<M3Item> item2List = M3GameManager.Instance.GetLineItem(x2, y2, false, true);

                M3Item item1 = M3ItemManager.Instance.gridItems[x1, y1];
                M3Item item2 = M3ItemManager.Instance.gridItems[x2, y2];
                item1.itemInfo.GetElement().crushWithOutSpecial = true;
                item2.itemInfo.GetElement().crushWithOutSpecial = true;
                item1.crushScore = SpecialEliminateScore.DoubleLine;
                Vector3 tmp = M3Supporter.Instance.GetItemPositionByGrid(x1, y1);
                Vector3 tmp2 = M3Supporter.Instance.GetItemPositionByGrid(x2, y2);
                if (!M3GameManager.Instance.isAutoAI)
                {
                    M3FxManager.Instance.FireArrow(tmp, true);
                    M3FxManager.Instance.FireArrow(tmp2, true);
                }
                for (int i = 0; i < item1List.Count; i++)
                {
                    item1List[i].OnSpecialCrush(ItemSpecial.fDoubleRow);
                    if (item1List[i].itemInfo.GetElement() is NormalElement)
                        item1List[i].GetGrid().Boom();
                }
                for (int i = 0; i < item2List.Count; i++)
                {
                    item2List[i].OnSpecialCrush(ItemSpecial.fDoubleRow);
                    if (item2List[i].itemInfo.GetElement() is NormalElement)
                        item2List[i].GetGrid().Boom();
                }
            }
            else
            {
                List<M3Item> item1List = M3GameManager.Instance.GetLineItem(x1, y1, false, true);
                List<M3Item> item2List = M3GameManager.Instance.GetLineItem(x2, y2, false, true);

                M3Item item1 = M3ItemManager.Instance.gridItems[x1, y1];
                M3Item item2 = M3ItemManager.Instance.gridItems[x2, y2];
                item1.itemInfo.GetElement().crushWithOutSpecial = true;
                item2.itemInfo.GetElement().crushWithOutSpecial = true;
                item1.crushScore = SpecialEliminateScore.DoubleLine;
                Vector3 tmp = M3Supporter.Instance.GetItemPositionByGrid(x1, y1);
                if (!M3GameManager.Instance.isAutoAI)
                {
                    M3FxManager.Instance.FireArrow(tmp, true);
                }

                for (int i = 0; i < item1List.Count; i++)
                {
                    item1List[i].OnSpecialCrush(ItemSpecial.fDoubleRow);
                    if (item1List[i].itemInfo.GetElement() is NormalElement)
                        item1List[i].GetGrid().Boom();
                }
                for (int i = 0; i < item2List.Count; i++)
                {
                    item2List[i].OnSpecialCrush(ItemSpecial.fDoubleRow);
                    if (item2List[i].itemInfo.GetElement() is NormalElement)
                        item2List[i].GetGrid().Boom();
                }
            }
        }

    }
}