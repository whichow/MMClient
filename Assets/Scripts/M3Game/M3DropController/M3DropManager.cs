using UnityEngine;

namespace Game.Match3
{
    public class M3DropManager
    {
        /// <summary>
        /// 下落
        /// </summary>
        /// <param name="grid1"></param>
        /// <param name="grid2"></param>
        /// <param name="isFlickerPortal"></param>
        /// <param name="check"></param>
        /// <returns></returns>
        public static bool DropItemTo(M3Grid grid1, M3Grid grid2, bool isFlickerPortal, bool check = false)
        {
            M3Item item = grid1.GetItem();

            if (!item.CanDrop)
                return false;

            if ((grid1.gridInfo.posY != grid2.gridInfo.posY && grid1.gridInfo.passType != 1 && IsHaveHigherPriority(grid1, grid2)) || item == null)
                return false;

            if (grid1.gridInfo.posY == grid2.gridInfo.posY && grid2.gridInfo.passType == 2 && !isFlickerPortal)
                return false;

            if (!check)
            {
                if (!M3GameManager.Instance.isAutoAI)
                {
                    if (isFlickerPortal)
                    {
                        PortalDropPieceAnimation(grid1, grid2);
                    }
                    else
                    {
                        NormalDropPieceAnimation(item, grid2);
                    }
                }
                else
                {
                    if (isFlickerPortal)
                    {
                        Debug.LogWarning("Protal " + grid1.gridInfo.posX + "_" + grid1.gridInfo.posY + "   " + grid2.gridInfo.posX + "_" + grid2.gridInfo.posY + "    " + FrameScheduler.instance.GetCurrentFrame());
                    }
                }

                int x1 = item.itemInfo.posX;
                int y1 = item.itemInfo.posY;
                int x2 = grid2.gridInfo.posX;
                int y2 = grid2.gridInfo.posY;
                item.itemInfo.RefreshPos(x2, y2);
                M3ItemManager.Instance.gridItems[x1, y1] = null;
                M3ItemManager.Instance.gridItems[x2, y2] = item;
            }
            return true;
        }

        public static bool IsHaveHigherPriority(M3Grid grid1, M3Grid grid2)
        {
            return M3GridManager.Instance.gridCells[grid2.gridInfo.posX - 1, grid2.gridInfo.posY] != grid1 && IsHavePieceTop(grid2);
        }

        private static bool IsHavePieceTop(M3Grid grid)
        {
            if (grid.IsDropObstacle(RopeTypeEnum.Bottom) || grid.IsDropObstacle(RopeTypeEnum.Top))
            {
                return false;
            }
            int num = grid.gridInfo.posX;
            int num2 = grid.gridInfo.posY;

            if (grid.gridInfo.passType == 2)
            {
                Int2 tmpInt2 = grid.gridInfo.GetFlickerPortalIn();
                num = tmpInt2.x;
                num2 = tmpInt2.y;

            }
            while (num >= 0 && grid.CheckValid(num, num2))
            {
                M3Item highestPiece = M3ItemManager.Instance.gridItems[num, num2];

                if (highestPiece != null && (!highestPiece.CanDrop))
                {
                    return false;
                }
                if (grid.CheckValid(num - 1, num2) && M3GridManager.Instance.gridCells[num - 1, num2].gridInfo.passType == 1)
                    return false;

                if (M3GridManager.Instance.gridCells[num, num2].gridInfo.spawnPointType == DropPointType.SpawnPoint || highestPiece != null)
                {
                    return true;
                }

                if (M3GridManager.Instance.gridCells[num, num2] != null)
                    if (/*M3GridManager.Instance.gridCells[num, num2].IsDropObstacle(RopeTypeEnum.Bottom) ||*/ M3GridManager.Instance.gridCells[num, num2].IsDropObstacle(RopeTypeEnum.Top))
                        return false;

                if (grid.CheckValid(num - 1, num2) && M3GridManager.Instance.gridCells[num - 1, num2] != null)
                    if (M3GridManager.Instance.gridCells[num - 1, num2].IsDropObstacle(RopeTypeEnum.Bottom) /*|| M3GridManager.Instance.gridCells[num - 1, num2].IsDropObstacle(RopeTypeEnum.Top)*/)
                        return false;

                if (M3GridManager.Instance.gridCells[num, num2].gridInfo.passType == 2 /*&& gridManager.m_grids[num2, num].portalType != null*/)
                {
                    Int2 tmpInt2 = M3GridManager.Instance.gridCells[num, num2].gridInfo.GetFlickerPortalIn();
                    num = tmpInt2.x;
                    num2 = tmpInt2.y;
                }
                else
                {
                    num--;
                }
            }
            return false;
        }

        /// <summary>
        /// 传送
        /// </summary>
        /// <param name="grid1"></param>
        /// <param name="grid2"></param>
        public static void PortalDropPieceAnimation(M3Grid grid1, M3Grid grid2)
        {
            int x1 = grid1.gridInfo.posX;
            int y1 = grid1.gridInfo.posY;
            int x2 = grid2.gridInfo.posX;
            int y2 = grid2.gridInfo.posY;

            Vector3 piecePosition = M3Supporter.Instance.GetItemPositionByGrid(x1, y1);
            Vector3 piecePosition2 = M3Supporter.Instance.GetItemPositionByGrid(x2, y2);

            Vector3 vector = new Vector3(piecePosition.x, piecePosition.y, piecePosition.z);
            Vector3 shownPos = new Vector3(piecePosition2.x, piecePosition2.y, piecePosition2.z);
            M3Item highestPiece = grid1.GetItem();
            highestPiece.disappearPosX = vector.x;
            highestPiece.disappearPosY = vector.y;
            highestPiece.shownPos = shownPos;
            highestPiece.targetPosX = piecePosition2.x;
            highestPiece.targetPosY = piecePosition2.y;
            highestPiece.IsDroping = true;
            if (highestPiece.dropSpeed == 0f)
            {
                highestPiece.dropSpeed = M3Config.DropInitialSpeed;
            }
            highestPiece.isLanded = false;
        }

        /// <summary>
        /// 下落动画
        /// </summary>
        /// <param name="item"></param>
        /// <param name="grid"></param>
        public static void NormalDropPieceAnimation(M3Item item, M3Grid grid)
        {
            item.IsDroping = true;
            var vec = M3Supporter.Instance.GetItemPositionByGrid(grid.gridInfo.posX, grid.gridInfo.posY);
            if (item.itemInfo.posY != grid.gridInfo.posY)
            {
                item.isTweening = true;
                item.dropSpeed = M3Config.DropInitialSpeed + M3Config.DropAcceleratedSpeed * Time.fixedDeltaTime;
                grid.isDroping = true;
                KTweenUtils.LocalMoveTo(item.itemView.itemTransform, vec, M3Config.InclinedDropTime,
                    delegate ()
                    {
                        item.isTweening = false;
                        grid.isDroping = false;
                        item.IsDroping = false;
                    }
                    );
            }

            if (item.dropSpeed == 0)
            {
                item.dropSpeed = M3Config.DropInitialSpeed;
            }
            item.targetPosX = vec.x;
            item.targetPosY = vec.y;
            item.isLanded = false;
        }

        /// <summary>
        /// 获取掉落口生成的元素ID
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public static int GetSpawnItemID(M3Grid grid)
        {
            var list = grid.gridInfo.spawnRule;
            int totalWeight = 0;
            if (list == null)
            {
                Debug.LogError("掉落口" + grid.gridInfo.posX + "_" + grid.gridInfo.posY + "没有配置");
                return -1;
            }
            for (int i = 0; i < list.Count; i++)
            {
                totalWeight += list[i].weight;
            }
            int weight = M3Supporter.Instance.GetRandomInt(0, totalWeight);
            int tmp = 0;
            for (int i = 0; i < list.Count; i++)
            {
                tmp += list[i].weight;
                if (tmp > weight)
                {
                    return list[i].elementID;
                }
            }
            return -1;
            //return 1000 + UnityEngine.Random.Range(1, 6);
        }

        /// <summary>
        /// 从掉落口生成新的三消单元
        /// </summary>
        /// <param name="grid"></param>
        public static void CreatePieceFromPort(M3Grid grid)
        {
            grid.isDroping = true;
            int x = grid.gridInfo.posX;
            int y = grid.gridInfo.posY;

            int id = 0;
            if (grid.isFishPort)
            {
                int collectType = M3GameManager.Instance.fishManager.GetCollectType(x, y);
                if (collectType > 0)
                {
                    id = M3Const.FishElementID;
                }
            }
            if (id == 0)
            {
                id = GetSpawnItemID(grid);
                if (id == -1)
                {
                    Debug.LogError("ID Error");
                    return;
                }
            }

            var item = M3ItemManager.Instance.CreateItemById(grid.gridInfo.posX, grid.gridInfo.posY, id);
            item.coordinate = new Int2(x - 1, y);
            M3ItemManager.Instance.gridItems[x, y] = item;

            if (M3GameManager.Instance.isAutoAI)
                Debug.Log("Spawn Item ID : " + id + "   " + grid.gridInfo.posX + " _ " + grid.gridInfo.posY + "    " + FrameScheduler.instance.GetCurrentFrame());

            if (grid.portDropSpeed == 0)
                item.dropSpeed = M3Config.DropInitialSpeed /*+ grid.portDropSpeed*/;
            else
                item.dropSpeed = grid.portDropSpeed + M3Config.DropAcceleratedSpeed * Time.fixedDeltaTime;
            item.isLanded = false;
            NormalDropPieceAnimation(item, grid);
        }

    }
}