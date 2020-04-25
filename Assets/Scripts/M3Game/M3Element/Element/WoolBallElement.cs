/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/6/12 11:55:56
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using Game.DataModel;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Match3
{
    /// <summary>
    /// 毛线球
    /// </summary>
    public class WoolBallElement : Element
    {
        /// <summary>
        /// 方向 0左 1下 2右 3 上  坐标系是x向下，y向右
        /// </summary>
        private enum EDirection
        {
            left = 0,
            down = 1,
            right = 2,
            up = 3
        }

        private struct SArgs
        {
            public ItemSpecial special;
            public int originPosX;
            public int originPosY;

            public SArgs(ItemSpecial special, int originPosX, int originPosY)
            {
                this.special = special;
                this.originPosX = originPosX;
                this.originPosY = originPosY;
            }
        }

        private bool m_roolFalg;
        /// <summary>
        /// 被触发滚动消除的参数，有多次触发在猫窝处理后执行一次
        /// </summary>
        private List<SArgs> m_waitRollArgsList = new List<SArgs>();


        public override void Init(int id, M3Item item)
        {
            base.Init(id, item);
            eName = M3ElementType.WoolBall;
        }

        public override void ProcessNeighborEliminate(int sx, int sy)
        {
            //Debug.Log("相邻消除-------------");
            m_waitRollArgsList.Add(new SArgs(ItemSpecial.fNormal, sx, sy));
        }

        /// <summary>
        /// 处理滚动碾压 ,选最后一次触发的
        /// </summary>
        /// <returns></returns>
        public bool ProcessRoll()
        {
            //Debug.Log("处理滚动碾压-------------" + m_waitRollArgsList.Count);
            bool b = false;
            if (m_waitRollArgsList.Count > 0)
            {
                b = true;
                var item = m_waitRollArgsList[m_waitRollArgsList.Count - 1];
                EDirection dir = GetDirection(item.originPosX, item.originPosY);
                ProcessRollByDir(dir);

                m_waitRollArgsList.Clear();
            }
            return b;
        }

        private EDirection GetDirection(int sx, int sy)
        {
            int x = itemObtainer.itemInfo.posX;
            int y = itemObtainer.itemInfo.posY;
            EDirection dir = EDirection.down;
            //优先级为下>右>左>上
            if (sx < x)
            {
                dir = EDirection.down;
                if (x == M3Config.GridHeight - 1)
                {
                    dir = EDirection.up;
                }
            }
            else if (sy < y)
            {
                dir = EDirection.right;
                if (y == M3Config.GridWidth - 1)
                {
                    dir = EDirection.left;
                }
            }
            else if (sy > y)
            {
                dir = EDirection.left;
                if (y == 0)
                {
                    dir = EDirection.right;
                }
            }
            else if (sx > x)
            {
                dir = EDirection.up;
                if (x == 0)
                {
                    dir = EDirection.down;
                }
            }
            return dir;
        }

        /// <summary>
        /// 处理滚动碾压
        /// </summary>
        private void ProcessRollByDir(EDirection dir)
        {
            m_roolFalg = true;
            //Debug.Log("处理滚动碾压方向-------------" + dir);
            var list = GetRollTarget(dir, itemObtainer.itemInfo.posX, itemObtainer.itemInfo.posY);
            if (list.Count > 0)
            {
                Vector3[] path = new Vector3[list.Count];
                for (int i = 0; i < list.Count; i++)
                {
                    path[i] = list[i].itemView.itemTransform.position + new Vector3(0, 0, -0.2f);
                }
                M3Item targetItem = list[list.Count - 1];
                RollToGrid(targetItem.itemInfo.posX, targetItem.itemInfo.posY, path, () =>
                 {
                     for (int i = 0; i < list.Count - 1; i++)
                     {
                         var pItem = list[i];
                         pItem.OnSpecialCrush(ItemSpecial.other);
                     }
                 });
            }
        }

        private void RollToGrid(int toX, int toY, Vector3[] path, Action callback)
        {
            M3Item lastItem = itemObtainer;
            M3Item targetItem = M3ItemManager.Instance.gridItems[toX, toY];
            if (view != null)
            {
                M3GridManager.Instance.dropLock = true;

                Vector3 sPos = view.eleTransform.position;
                view.eleTransform.position = sPos + new Vector3(0, 0, -0.2f);
                M3FxManager.Instance.PlayRollAnimation(view.eleTransform, path, 0.5f, delegate ()
                {
                    view.eleTransform.SetParent(targetItem.itemView.itemTransform);
                    view.eleTransform.localScale = Vector3.one;
                    view.eleTransform.localPosition = view.eleTransform.localPosition + new Vector3(0, 0, -1);

                    lastItem.itemInfo.RemoveHighestElement();
                    if (lastItem.itemInfo.CheckEmpty())
                    {
                        int x = lastItem.itemInfo.posX;
                        int y = lastItem.itemInfo.posY;
                        lastItem.RemoveFrom(x, y);
                        lastItem.ItemDestroy();
                    }

                    //var a = targetItem.itemInfo.allElementList;
                    //Debuger.Log("------------------");
                    //foreach (var item in a)
                    //{
                    //    Debuger.Log(item.data.config.Name);
                    //}

                    // 消除所有，包括多层的，如三层窗帘
                    var eleList = targetItem.itemInfo.allElementList;
                    for (int i = eleList.Count - 1; i >= 0; i--)
                    {
                        var item = eleList[i];
                        //Debuger.Log("X: " + item.data.config.Name);
                        var transforID = item.data.config.ClearTransforID;
                        if (transforID == 0)
                        {
                            if (item != this)
                            {
                                itemObtainer.isCrushing = false;
                                targetItem.OnSpecialCrush(ItemSpecial.fNormal);
                            }
                        }
                        else
                        {
                            itemObtainer.isCrushing = false;
                            targetItem.OnSpecialCrush(ItemSpecial.other);
                            while (transforID > 0)
                            {
                                var tarxdm = XTable.ElementXTable.GetByID(transforID);
                                transforID = tarxdm.ClearTransforID;
                                //Debuger.Log("Xx: " + tarxdm.Name);
                                itemObtainer.isCrushing = false;
                                targetItem.OnSpecialCrush(ItemSpecial.other);
                            }
                        }
                    }

                    itemObtainer = targetItem;
                    view.RefreshView();

                    m_roolFalg = false;
                    M3GridManager.Instance.dropLock = false;
                    callback?.Invoke();
                });
                targetItem.itemInfo.AddElement(this);
                //Debuger.Log("AddElement----: " + data.config.Name);
            }
        }

        private List<M3Item> GetRollTarget(EDirection dir, int x, int y, int index = 0)
        {
            //Debug.Log($"x:{posX} - y{posY}: itemX:{itemObtainer.itemInfo.posX} - itemY:{itemObtainer.itemInfo.posY}");
            List<M3Item> itemList = new List<M3Item>();
            for (int i = index; i < 3; i++)
            {
                switch (dir)
                {
                    case EDirection.left:
                        y--;
                        break;
                    case EDirection.down:
                        x++;
                        break;
                    case EDirection.right:
                        y++;
                        break;
                    case EDirection.up:
                        x--;
                        break;
                }

                M3Item item = GetItem(x, y);
                if (item != null && !item.isCrushing && item.itemInfo.GetElement() != null)
                {
                    if (item.itemInfo.HasType(M3Const.WoolBallResistType))
                    {
                        if (index != 0) break;

                        if (i == 0)
                        {
                            // 当边缘处理，反弹
                            int d = (int)dir;
                            d = (d - 2) < 0 ? Math.Abs(-d - 2) : (d - 2);
                            dir = (EDirection)d;
                            return GetRollTarget(dir, itemObtainer.itemInfo.posX, itemObtainer.itemInfo.posY);
                        }
                        else
                        {
                            // 当边缘处理，拐弯，向边缘远的地方去
                            itemList.AddRange(GetRollTurnTarget(dir, x, y, i));
                            return itemList;
                        }
                    }
                    itemList.Add(item);
                    if (item.itemInfo.HasType(M3Const.WoolBallObstacleType))
                    {
                        break;
                    }
                }
                else
                {
                    if (index != 0) break;

                    if (x > M3Config.GridHeight - 1 || y > M3Config.GridWidth - 1 || x < 0 || y < 0)
                    {
                        itemList.AddRange(GetRollTurnTarget(dir, x, y, i));
                        return itemList;
                    }
                    continue;
                }
            }
            return itemList;
        }

        /// <summary>
        /// 拐弯，向边缘远的地方去
        /// </summary>
        private List<M3Item> GetRollTurnTarget(EDirection dir, int x, int y, int len)
        {
            //Debuger.Log("拐弯---" + dir);
            switch (dir)
            {
                case EDirection.left:
                    y++;
                    break;
                case EDirection.down:
                    x--;
                    break;
                case EDirection.right:
                    y--;
                    break;
                case EDirection.up:
                    x++;
                    break;
                default:
                    break;
            }

            if (dir == EDirection.left || dir == EDirection.right)
            {
                if (x <= M3Config.GridHeight * 0.5)
                {
                    dir = EDirection.down;
                }
                else
                {
                    dir = EDirection.up;
                }
            }
            else
            {
                if (y <= M3Config.GridWidth * 0.5)
                {
                    dir = EDirection.right;
                }
                else
                {
                    dir = EDirection.left;
                }
            }
            return GetRollTarget(dir, x, y, len);
        }

        private M3Item GetItem(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < M3Config.GridHeight && y < M3Config.GridWidth)
            {
                return M3ItemManager.Instance.gridItems[x, y];
            }
            return null;
        }

        public override void ProcessSpecialEliminate(ItemSpecial special, object[] args, bool ignoreEffect = false)
        {
            base.ProcessSpecialEliminate(special, args, ignoreEffect);
            //Debuger.Log("特殊消除----------" + special + this.GetHashCode() + m_roolFalg);
            if (!m_roolFalg)
            {
                m_waitRollArgsList.Add(new SArgs(special, int.Parse(args[0].ToString()), int.Parse(args[1].ToString())));
            }
        }

    }
}
