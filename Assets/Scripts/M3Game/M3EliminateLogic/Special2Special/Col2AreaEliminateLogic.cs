/** 
 *FileName:     Col2AreaEliminateLogic.cs 
 *Author:       HeJunJie 
 *Version:      1.0 
 *UnityVersion：5.6.2f1
 *Date:         2017-08-03 
 *Description:    
 *History: 
*/
using System.Collections.Generic;

namespace Game.Match3
{
    /// <summary>
    /// 竖消与炸弹
    /// </summary>
    public class Col2AreaEliminateLogic : EliminateBase
    {

        public override void ProcessEliminateLogic(params object[] args)
        {
            base.ProcessEliminateLogic(args);
            int targetX = (int)args[0];
            int targetY = (int)args[1];
            int sourceX = (int)args[2];
            int sourceY = (int)args[3];
            int isRow = (int)args[4];
            M3ItemManager.Instance.gridItems[targetX, targetY].itemInfo.GetElement().crushWithOutSpecial = true;
            M3ItemManager.Instance.gridItems[sourceX, sourceY].itemInfo.GetElement().crushWithOutSpecial = true;
            M3ItemManager.Instance.gridItems[targetX, targetY].crushScore = SpecialEliminateScore.BoomAndLine;

            int listY1;
            int listY2;
            int listY3;
            int listY4;
            if (targetX == sourceX)
            {
                if (targetY < sourceY)
                {
                    listY1 = targetY - 1;
                    listY2 = targetY;
                    listY3 = sourceY;
                    listY4 = sourceY + 1;
                }
                else
                {
                    listY1 = sourceY - 1;
                    listY2 = sourceY;
                    listY3 = targetY;
                    listY4 = targetY + 1;
                }
            }
            else
            {
                listY1 = targetY - 2;
                listY2 = targetY - 1;
                listY3 = targetY;
                listY4 = targetY + 1;
            }

            // 先检测触发点是否有草坪，有的话所有消除组都铺
            bool hasLawn = EliminateManager.Instance.CheckHasLawn(targetX, targetY);
            if (!hasLawn)
            {
                hasLawn = EliminateManager.Instance.CheckHasLawn(sourceX, sourceY);
            }

            List<List<Int2>> tmpList = new List<List<Int2>>();
            List<Int2> originPosList = new List<Int2>();

            if (listY1 >= 0)
            {
                tmpList.Add(M3GameManager.Instance.GetLineAllItem(targetX, listY1, false, true));
                originPosList.Add(new Int2 ( targetX, listY1 ));
                if (!M3GameManager.Instance.isAutoAI)
                {
                    if (M3Supporter.Instance.CheckIsLineEmpty(targetX, listY1, false))
                        M3FxManager.Instance.FireArrow(M3Supporter.Instance.GetItemPositionByGrid(targetX, listY1), true);
                }
            }

            if (listY2 >= 0)
            {
                tmpList.Add(M3GameManager.Instance.GetLineAllItem(targetX, listY2, false, true));
                originPosList.Add(new Int2(targetX, listY2));
                if (!M3GameManager.Instance.isAutoAI)
                {
                    if (M3Supporter.Instance.CheckIsLineEmpty(targetX, listY2, false))
                        M3FxManager.Instance.FireArrow(M3Supporter.Instance.GetItemPositionByGrid(targetX, listY2), true);
                }
            }

            tmpList.Add(M3GameManager.Instance.GetLineAllItem(targetX, listY3, false, true));
            originPosList.Add(new Int2(targetX, listY3));
            if (!M3GameManager.Instance.isAutoAI)
            {
                if (M3Supporter.Instance.CheckIsLineEmpty(targetX, listY3, false))
                    M3FxManager.Instance.FireArrow(M3Supporter.Instance.GetItemPositionByGrid(targetX, listY3), true);
            }

            if (listY4 < M3Config.GridWidth)
            {
                tmpList.Add(M3GameManager.Instance.GetLineAllItem(targetX, listY4, false, true));
                originPosList.Add(new Int2(targetX, listY4));
                if (!M3GameManager.Instance.isAutoAI)
                {
                    if (M3Supporter.Instance.CheckIsLineEmpty(targetX, listY4, false))
                        M3FxManager.Instance.FireArrow(M3Supporter.Instance.GetItemPositionByGrid(targetX, listY4), true);
                }
            }

            //int x;
            //int y;
            //M3Item tempItem;
            //int count = 0;
            for (int i = 0; i < tmpList.Count; i++)
            {
                ProcessSpecialList(tmpList[i], ItemSpecial.fCol2Area, originPosList[i].x, originPosList[i].y, hasLawn);
                //for (int j = 0; j < tmpList[i].Count; j++)
                //{
                //    x = tmpList[i][j].x;
                //    y = tmpList[i][j].y;
                //    tempItem = M3ItemManager.Instance.gridItems[x, y];
                //    if (tempItem != null && !tempItem.isCrushing)
                //    {
                //        tempItem.OnSpecialCrush(ItemSpecial.fCol2Area);
                //    }
                //    count++;
                //}
            }

        }


    }
}