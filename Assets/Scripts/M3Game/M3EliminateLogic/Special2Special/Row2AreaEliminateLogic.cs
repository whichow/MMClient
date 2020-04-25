/** 
 *FileName:     Row2AreaEliminateLogic.cs 
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
    /// 横消与炸弹
    /// </summary>
    public class Row2AreaEliminateLogic : EliminateBase
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

            int listX1;
            int listX2;
            int listX3;
            int listX4;
            if (targetY == sourceY)
            {
                if (targetX < sourceX)
                {
                    listX1 = targetX - 1;
                    listX2 = targetX;
                    listX3 = sourceX;
                    listX4 = sourceX + 1;
                }
                else
                {
                    listX1 = sourceX - 1;
                    listX2 = targetX;
                    listX3 = sourceX;
                    listX4 = targetX + 1;
                }
            }
            else
            {
                listX1 = targetX - 1;
                listX2 = targetX;
                listX3 = targetX + 1;
                listX4 = targetX + 2;
            }

            // 先检测触发点是否有草坪，有的话所有消除组都铺
            bool hasLawn = EliminateManager.Instance.CheckHasLawn(targetX, targetY);
            if (!hasLawn)
            {
                hasLawn = EliminateManager.Instance.CheckHasLawn(sourceX, sourceY);
            }
            List<List<Int2>> tmpList = new List<List<Int2>>();
            List<Int2> originPosList = new List<Int2>();

            if (listX1 >= 0 && listX1 < M3Config.GridHeight)
            {
                tmpList.Add(M3GameManager.Instance.GetLineAllItem(listX1, targetY, true, true));
                originPosList.Add(new Int2(listX1, targetY));
                if (!M3GameManager.Instance.isAutoAI)
                {
                    if (M3Supporter.Instance.CheckIsLineEmpty(listX1, targetY, false))
                        M3FxManager.Instance.FireArrow(M3Supporter.Instance.GetItemPositionByGrid(listX1, targetY), false);
                }
            }
            if (listX2 >= 0 && listX2 < M3Config.GridHeight)
            {
                tmpList.Add(M3GameManager.Instance.GetLineAllItem(listX2, targetY, true, true));
                originPosList.Add(new Int2(listX2, targetY));
                if (!M3GameManager.Instance.isAutoAI)
                {
                    if (M3Supporter.Instance.CheckIsLineEmpty(listX2, targetY, false))
                        M3FxManager.Instance.FireArrow(M3Supporter.Instance.GetItemPositionByGrid(targetX, targetY), false);
                }
            }
            if (listX3 >= 0 && listX3 < M3Config.GridHeight)
            {
                tmpList.Add(M3GameManager.Instance.GetLineAllItem(listX3, targetY, true, true));
                originPosList.Add(new Int2(listX3, targetY));
                if (!M3GameManager.Instance.isAutoAI)
                {
                    if (M3Supporter.Instance.CheckIsLineEmpty(listX3, targetY, false))
                        M3FxManager.Instance.FireArrow(M3Supporter.Instance.GetItemPositionByGrid(listX3, targetY), false);
                }
            }
            if (listX4 >= 0 && listX4 < M3Config.GridHeight)
            {
                tmpList.Add(M3GameManager.Instance.GetLineAllItem(listX4, targetY, true, true));
                originPosList.Add(new Int2(listX4, targetY));
                if (!M3GameManager.Instance.isAutoAI)
                {
                    if (M3Supporter.Instance.CheckIsLineEmpty(listX4, targetY, false))
                        M3FxManager.Instance.FireArrow(M3Supporter.Instance.GetItemPositionByGrid(listX4, targetY), false);
                }
            }

            //int x;
            //int y;
            //M3Item tempItem;
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
                //        tempItem.OnSpecialCrush(ItemSpecial.fRow2Area);
                //    }
                //}
            }

        }
    }
}