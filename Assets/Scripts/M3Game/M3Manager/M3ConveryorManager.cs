using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Match3
{
    /// <summary>
    /// 传送带管理器
    /// </summary>
    public class M3ConveyorManager
    {
        public bool haveMovedThisRound = true;
        public bool noConveyor = true;

        public void Init()
        {
            List<List<Int2>> conveyorList = M3GameManager.Instance.level.GetConveyorList();
            for (int i = 0; i < conveyorList.Count; i++)
            {
                for (int j = 0; j < conveyorList[i].Count; j++)
                {
                    int index = ((j + 1) != conveyorList[i].Count) ? j + 1 : 0;
                    Int2 from = conveyorList[i][j];
                    Int2 to = conveyorList[i][index];
                    bool flag = IsStraight(conveyorList[i], index);
                    if (!M3GameManager.Instance.isAutoAI)
                    {
                        M3GridManager.Instance.gridCells[to.x, to.y].CreateConveyor(GetRotation(from, to, conveyorList[i], index, flag), flag, !flag && IsNeedFlip(conveyorList[i], index));
                    }
                }
            }
            noConveyor = (conveyorList == null || conveyorList.Count == 0);
        }

        private bool IsNeedFlip(List<Int2> list, int index)
        {
            bool result = true;
            int index2 = index - 1;
            int index3 = index + 1;
            if (index == 0)
            {
                index2 = list.Count - 1;
            }
            if (index == list.Count - 1)
            {
                index3 = 0;
            }
            if ((list[index2].x > list[index].x && list[index].y > list[index3].y) || (list[index2].y > list[index].y && list[index].x < list[index3].x) || (list[index2].x < list[index].x && list[index].y < list[index3].y) || (list[index2].y < list[index].y && list[index].x > list[index3].x))
            {
                result = false;
            }
            return result;
        }

        private float GetRotation(Int2 from, Int2 to, List<Int2> list, int index, bool isStraight)
        {
            if (index == 0 && isStraight)
            {
                from = list[0];
                to = list[1];
            }
            if (from.x == to.x && this.IsSmaller(from.y, to.y))
            {
                return 90;
            }
            if (this.IsSmaller(from.x, to.x) && from.y == to.y)
            {
                return 0;
            }
            if (from.x == to.x && !this.IsSmaller(from.y, to.y))
            {
                return 270;
            }
            if (!this.IsSmaller(from.x, to.x) && from.y == to.y)
            {
                return 180;
            }
            return 0f;
        }

        private bool IsSmaller(int a, int b)
        {
            //if (Mathf.Abs(a - b) > 1)
            //{
            //    return a >= b;
            //}
            return a <= b;
        }

        private bool IsStraight(List<Int2> list, int index)
        {
            bool result = true;
            int index2 = index - 1;
            int index3 = index + 1;
            if (index == 0)
            {
                index2 = list.Count - 1;
            }
            if (index == list.Count - 1)
            {
                index3 = 0;
            }
            if (list[index2].x != list[index3].x && list[index2].y != list[index3].y)
            {
                result = false;
            }
            return result;
        }

        public void MoveConveyor()
        {
            if (haveMovedThisRound)
                return;
            haveMovedThisRound = true;
            List<List<Int2>> conveyorList = M3GameManager.Instance.level.GetConveyorList();
            for (int i = 0; i < conveyorList.Count; i++)
            {
                int lastItemX = conveyorList[i][conveyorList[i].Count - 1].x;
                int lastItemY = conveyorList[i][conveyorList[i].Count - 1].y;
                M3Item lastItem = M3ItemManager.Instance.gridItems[lastItemX, lastItemY];
                Element lastIce = M3GridManager.Instance.gridCells[lastItemX, lastItemY].gridInfo.floorElement;

                M3GridManager.Instance.gridCells[lastItemX, lastItemY].gridInfo.floorElement = null;
                M3ItemManager.Instance.gridItems[lastItemX, lastItemY] = null;

                for (int j = conveyorList[i].Count - 1; j >= 0; j--)
                {
                    int index = (j != 0) ? (j - 1) : (conveyorList[i].Count - 1);
                    Int2 point = conveyorList[i][index];//当前格子
                    Int2 point2 = conveyorList[i][j];//目标格子

                    M3Item from = M3ItemManager.Instance.gridItems[point.x, point.y];
                    Element fromIce = M3GridManager.Instance.gridCells[point.x, point.y].gridInfo.floorElement;
                    if (j == 0)
                    {
                        from = lastItem;
                        fromIce = lastIce;
                    }
                    if (from != null && !from.isCrushing)
                    {
                        MoveItem(from, point2);
                    }
                    if (fromIce != null && !fromIce.gridObtainer.isCrushing)
                    {
                        MoveGrid(fromIce, point2);
                    }
                    if (j != 0)
                    {
                        M3ItemManager.Instance.gridItems[point.x, point.y] = null;
                        M3GridManager.Instance.gridCells[point.x, point.y].gridInfo.floorElement = null;
                    }
                    if (!M3GameManager.Instance.isAutoAI)
                        M3GridManager.Instance.gridCells[point.x, point.y].MoveConveyor();
                }
            }
        }

        private void MoveItem(M3Item from, Int2 point2)
        {
            //M3Item from = M3ItemManager.Instance.gridItems[point.x, point.y];
            if (from != null)
            {
                bool flag = IsGridNear(from.itemInfo.PosInt2, point2);
                M3ItemManager.Instance.gridItems[point2.x, point2.y] = from;
                from.itemInfo.RefreshPos(point2.x, point2.y);
                from.isTweening = true;
                M3GridManager.Instance.gridCells[point2.x, point2.y].isDroping = true;
                if (flag)
                {
                    Action action = delegate ()
                      {
                          if (from != null)
                          {
                              if (from.itemView != null)
                                  from.itemView.itemTransform.localPosition = M3Supporter.Instance.GetItemPositionByGrid(point2.x, point2.y) + new Vector3(0, 0, from.itemView.itemTransform.position.z);
                              M3GridManager.Instance.gridCells[point2.x, point2.y].isDroping = false;
                              if (from.itemInfo.GetElement() != null && from.itemInfo.GetElement().eName == M3ElementType.FishElement)
                              {
                                  ((FishElement)from.itemInfo.GetElement()).CheckDropOut();
                              }
                          }
                          from.isTweening = false;
                      };
                    if (!M3GameManager.Instance.isAutoAI)
                        M3FxManager.Instance.PlayConveyorAnimation(from.itemView.itemTransform, M3Supporter.Instance.GetItemPositionByGrid(point2.x, point2.y) + new Vector3(0, 0, from.itemView.itemTransform.position.z), M3Config.ConveyorTime, action);
                    else
                    {
                        action();
                    }
                }
                else
                {
                    PiecePortalAnimation(from, point2);
                }
                from.itemInfo.RefreshPos(point2.x, point2.y);
            }
        }

        private void MoveGrid(Element from, Int2 point)
        {
            if (from != null)
            {
                bool flag = IsGridNear(new Int2(from.posX, from.posY), point);
                M3GridManager.Instance.gridCells[point.x, point.y].gridInfo.floorElement = from;
                from.gridObtainer = M3GridManager.Instance.gridCells[point.x, point.y];
                if (flag)
                {
                    Action action = delegate ()
                    {
                        if (from.view != null)
                            from.Refresh();
                        M3GridManager.Instance.gridCells[point.x, point.y].isDroping = false;
                    };
                    if (!M3GameManager.Instance.isAutoAI)
                    {
                        from.view.eleTransform.SetParent(M3GridManager.Instance.gridCells[point.x, point.y].gridView.gridTransform, true);
                        M3FxManager.Instance.PlayConveyorAnimation(from.view.eleTransform, new Vector3(0, 0, from.view.eleTransform.localPosition.z), M3Config.ConveyorTime, action);
                    }
                    else
                    {
                        action();
                    }
                }
                else
                {
                    M3GridManager.Instance.gridCells[point.x, point.y].gridInfo.floorElement = from;
                    Action action = delegate ()
                    {
                        if (from.view != null)
                        {
                            from.Refresh();
                        }
                        M3GridManager.Instance.gridCells[point.x, point.y].isDroping = false;
                    };
                    if (!M3GameManager.Instance.isAutoAI)
                    {
                        from.view.eleTransform.SetParent(M3GridManager.Instance.gridCells[point.x, point.y].gridView.gridTransform);
                        KTweenUtils.ScaleTo(from.view.eleTransform, Vector3.zero, M3Config.ConveyorTime / 2.0f, delegate ()
                        {
                            KTweenUtils.LocalMoveTo(from.view.eleTransform, new Vector3(0, 0, from.view.eleTransform.localPosition.z), 0);
                            KTweenUtils.ScaleTo(from.view.eleTransform, Vector3.one, M3Config.ConveyorTime / 2.0f, action);
                        });
                    }
                    else
                    {
                        action();
                    }
                }
                from.posX = point.x;
                from.posY = point.y;
            }
        }

        public void PiecePortalAnimation(M3Item piece, Int2 destination)
        {
            Action action = delegate ()
            {
                M3GridManager.Instance.gridCells[destination.x, destination.y].isDroping = false;
                if (piece.itemInfo.GetElement().eName == M3ElementType.FishElement)
                {
                    ((FishElement)piece.itemInfo.GetElement()).CheckDropOut();
                }
                piece.isTweening = false;
            };
            if (!M3GameManager.Instance.isAutoAI)
            {
                Vector3 a = piece.position;
                Vector3 piece2Position = M3Supporter.Instance.GetItemPositionByGrid(destination.x, destination.y) + new Vector3(0f, 0f, piece.itemView.itemTransform.position.z);
                KTweenUtils.ScaleTo(piece.itemView.itemTransform, Vector3.zero, M3Config.ConveyorTime / 2.0f, delegate ()
                {
                    KTweenUtils.LocalMoveTo(piece.itemView.itemTransform, piece2Position, 0);
                    KTweenUtils.ScaleTo(piece.itemView.itemTransform, Vector3.one, M3Config.ConveyorTime / 2.0f, action);
                });
            }
            else
            {
                action();
            }
        }

        //public Vector3 GetPortalOffset(M3Item item, Int2 destination)
        //{
        //    Vector3 result = default(Vector3);
        //    Int2 piece = item.itemInfo.PosInt2;
        //    if (piece.x == destination.x && piece.y < destination.y)
        //    {
        //        result.y = -Constants.gap / 2f;
        //    }
        //    if (piece.x == destination.x && piece.y > destination.y)
        //    {
        //        result.y = Constants.gap / 2f;
        //    }
        //    if (piece.y == destination.y && piece.x > destination.x)
        //    {
        //        result.x = Constants.gap / 2f;
        //    }
        //    if (piece.y == destination.y && piece.x < destination.x)
        //    {
        //        result.x = -Constants.gap / 2f;
        //    }
        //    return result;
        //}

        public bool IsGridNear(Int2 grid1, Int2 grid2)
        {
            return Math.Abs(grid1.x - grid2.x) + Math.Abs(grid1.y - grid2.y) == 1;
        }

        public void UnlockConveyor()
        {
            this.haveMovedThisRound = false;
        }

    }
}