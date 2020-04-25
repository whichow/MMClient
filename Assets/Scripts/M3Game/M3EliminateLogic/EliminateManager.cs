/** 
 *FileName:     EliminateManager.cs 
 *Author:       HeJunJie 
 *Version:      1.0 
 *UnityVersion：5.6.2f1
 *Date:         2017-07-18 
 *Description:    
 *History: 
*/
using System.Collections.Generic;
using UnityEngine;

namespace Game.Match3
{
    /// <summary>
    /// 消除管理器
    /// </summary>
    public class EliminateManager : Singleton<EliminateManager>
    {
        private Dictionary<ItemSpecial, EliminateBase> elimainateDic;

        public void Init()
        {
            elimainateDic = new Dictionary<ItemSpecial, EliminateBase>();
            elimainateDic.Add(ItemSpecial.fRow, new RowEliminateLogic());
            elimainateDic.Add(ItemSpecial.fColumn, new ColEliminateLogic());
            elimainateDic.Add(ItemSpecial.fArea, new AreaEliminateLogic());
            elimainateDic.Add(ItemSpecial.fColor, new ColorEliminateLogic());
            elimainateDic.Add(ItemSpecial.fRowAndCol, new RowAndColElimainateLogic());
            elimainateDic.Add(ItemSpecial.fRow2Area, new Row2AreaEliminateLogic());
            elimainateDic.Add(ItemSpecial.fCol2Area, new Col2AreaEliminateLogic());
            elimainateDic.Add(ItemSpecial.fDoubleArea, new DoubleAreaEliminateLogic());
            elimainateDic.Add(ItemSpecial.fArea2Color, new RowAndCol2ColorEliminateLogic());
            elimainateDic.Add(ItemSpecial.fLine2Color, new RowAndCol2ColorEliminateLogic());
            elimainateDic.Add(ItemSpecial.fDoubleColor, new DoubleColorEliminateLogic());
            //双行双列走十字逻辑了
            //elimainateDic.Add(ItemSpecial.fDoubleCol, new DoubleColEliminateLogic());
            //elimainateDic.Add(ItemSpecial.fDoubleRow, new DoubleRowEliminateLogic());
        }

        public void ProcessEliminate(ItemSpecial type, int x, int y, ItemColor color)
        {
            if (elimainateDic.ContainsKey(type))
            {
                elimainateDic[type].ProcessEliminateLogic(x, y, color);
            }
        }

        public void ProcessColorEliminate(ItemSpecial type, int x1, int y1, int x2, int y2, ItemColor color)
        {
            if (elimainateDic.ContainsKey(type))
            {
                elimainateDic[type].ProcessEliminateLogic(x1, y1, x2, y2, color);
            }
        }

        public void ProcessDoubleColEliminate(ItemSpecial type, int x1, int y1, int x2, int y2, bool isTransverse)
        {
            if (elimainateDic.ContainsKey(type))
            {
                elimainateDic[type].ProcessEliminateLogic(x1, y1, x2, y2, isTransverse);
            }
        }

        public void ProcessRowAndColEliminate(ItemSpecial type, ItemSpecial item1Special, ItemSpecial item2Special, int x1, int y1, int x2, int y2, int exchangePos)
        {
            if (elimainateDic.ContainsKey(type))
            {
                elimainateDic[type].ProcessEliminateLogic(x1, y1, x2, y2, exchangePos);
            }
        }

        public void ProcessRow2AreaEliminate(ItemSpecial type, int x1, int y1, int x2, int y2, int isRow)
        {
            if (elimainateDic.ContainsKey(type))
            {
                elimainateDic[type].ProcessEliminateLogic(x1, y1, x2, y2, isRow);
            }
        }

        public void ProcessCol2AreaEliminate(ItemSpecial type, int x1, int y1, int x2, int y2, int isRow)
        {
            if (elimainateDic.ContainsKey(type))
            {
                elimainateDic[type].ProcessEliminateLogic(x1, y1, x2, y2, isRow);
            }
        }

        public void ProcessDoubleAreaEliminate(ItemSpecial type, int x1, int y1, int x2, int y2, int isRow)
        {
            if (elimainateDic.ContainsKey(type))
            {
                elimainateDic[type].ProcessEliminateLogic(x1, y1, x2, y2, isRow);
            }
        }

        public void ProcessSpecial2ColorEliminate(ItemSpecial type, int x1, int y1, int x2, int y2, int isRow, ItemColor color)
        {
            if (elimainateDic.ContainsKey(type))
            {
                elimainateDic[type].ProcessEliminateLogic(x1, y1, x2, y2, isRow, color);
            }
        }

        public void ProcessDoubleColorEliminate(ItemSpecial type, int x1, int y1, int x2, int y2)
        {
            if (elimainateDic.ContainsKey(type))
            {
                elimainateDic[type].ProcessEliminateLogic(x1, y1, x2, y2);
            }
        }


        #region 草坪处理

        private List<Int2> _upList;
        private List<Int2> _rightList;
        private List<Int2> _downList;
        private List<Int2> _leftList;

        private List<Int2> UpList
        {
            get
            {
                if (_upList == null)
                {
                    _upList = new List<Int2>();
                }
                return _upList;
            }
        }

        public List<Int2> RightList
        {
            get
            {
                if (_rightList == null)
                {
                    _rightList = new List<Int2>();
                }
                return _rightList;
            }
        }

        public List<Int2> DownList
        {
            get
            {
                if (_downList == null)
                {
                    _downList = new List<Int2>();
                }
                return _downList;
            }
        }

        public List<Int2> LeftList
        {
            get
            {
                if (_leftList == null)
                {
                    _leftList = new List<Int2>();
                }
                return _leftList;
            }
        }

        /// <summary>
        /// 检测是否有草坪
        /// </summary>
        /// <returns></returns>
        public bool CheckHasLawn(M3Grid grid)
        {
            if (grid != null && grid.gridInfo.HaveLawn)
            {
                return true;
            }
            return false;
        }

        public bool CheckHasLawn(int x, int y)
        {
            M3Grid grid = M3GridManager.Instance.gridCells[x, y];
            return CheckHasLawn(grid);
        }

        public bool CheckHasLawn(Int2 pos)
        {
            return CheckHasLawn(pos.x, pos.y);
        }

        /// <summary>
        /// 是否可以铺草坪
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool CanLayingLawn(M3Item item)
        {
            if (!item.ElementCrushFlag && !item.GetGrid().gridInfo.HaveIce && !item.itemInfo.HasType(M3Const.ResistLawnObstacleType))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 检测常规消除元素底下是否有草坪
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool CheckNormalEliminateHasLawn(List<Vector2> list)
        {
            M3Grid grid = null;
            foreach (var vec in list)
            {
                grid = M3GridManager.Instance.gridCells[(int)vec.x, (int)vec.y];
                if (grid != null && grid.gridInfo.HaveLawn)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 设置可铺草坪 ， 从触发点四方向铺草坪，找到草坪开始向后铺，遇阻挡停止（改为遇障碍物可以繼續了）
        /// </summary>
        /// <param name="list"></param>
        /// <param name="originPosX"></param>
        /// <param name="originPosY"></param>
        /// <param name="posHaveLawn">起点是否铺草坪</param>
        /// <returns></returns>
        public void LayingLawnForSpecialEliminate(List<Int2> list, int originPosX, int originPosY, bool posHaveLawn)
        {
            bool originHasLawn = false;
            M3Item item = M3ItemManager.Instance.gridItems[originPosX, originPosY];
            if (item != null)
            {
                originHasLawn = CheckHasLawn(item.GetGrid());
                //Debuger.LogError(string.Format("起点是否是草坪：{0} {1}-{2} 真有 {3}", posHaveLawn, originPosX, originPosY, originHasLawn));
                if (posHaveLawn && !originHasLawn && CanLayingLawn(item))
                {
                    originHasLawn = true;
                    item.itemInfo.GetElement().needCreateLawn = true; // 设置草坪
                }
                item.ElementCrushFlag = true;
            }
            else
            {
                originHasLawn = posHaveLawn;
            }

            //放到四方向列表中，原点开始放射状铺草坪
            foreach (var pos in list)
            {
                //非与原点相连忽略，最多是十字状放射
                if (pos.x == originPosX)
                {
                    //横向
                    if (pos.y > originPosY)
                    {
                        RightList.Add(pos);
                    }
                    else if (pos.y < originPosY)
                    {
                        LeftList.Add(pos);
                    }
                }
                else if (pos.y == originPosY)
                {
                    //竖向
                    if (pos.x < originPosX)
                    {
                        UpList.Add(pos);
                    }
                    else if (pos.x > originPosX)
                    {
                        DownList.Add(pos);
                    }
                }
            }

            UpList.Sort((Int2 a, Int2 b) => { return b.x.CompareTo(a.x); });
            RightList.Sort((Int2 a, Int2 b) => { return a.y.CompareTo(b.y); });
            DownList.Sort((Int2 a, Int2 b) => { return a.x.CompareTo(b.x); });
            LeftList.Sort((Int2 a, Int2 b) => { return b.y.CompareTo(a.y); });

            RadiateLayingLawn(UpList, originHasLawn);
            RadiateLayingLawn(RightList, originHasLawn);
            RadiateLayingLawn(DownList, originHasLawn);
            RadiateLayingLawn(LeftList, originHasLawn);

            UpList.Clear();
            RightList.Clear();
            DownList.Clear();
            LeftList.Clear();
        }

        /// <summary>
        /// 设置可铺草坪 炸弹 原点为草坪则全铺（没障碍的话）
        /// </summary>
        /// <param name="list"></param>
        /// <param name="originPosX"></param>
        /// <param name="originPosY"></param>
        /// <param name="posHaveLawn"></param>
        public void AreaLayingLawnForSpecialEliminate(List<Int2> list, int originPosX, int originPosY, bool posHaveLawn)
        {
            if (posHaveLawn)
            {
                M3Item item;
                foreach (var pos in list)
                {
                    item = M3ItemManager.Instance.gridItems[pos.x, pos.y];
                    if (item != null && CanLayingLawn(item))
                    {
                        item.ElementCrushFlag = true;
                        item.itemInfo.GetElement().needCreateLawn = true; // 设置草坪
                    }
                }
            }
            else
            {
                LayingLawnForSpecialEliminate(list, originPosX, originPosY, posHaveLawn);
            }
        }

        /// <summary>
        /// 设置可铺草坪 颜色活力猫
        /// </summary>
        /// <param name="list"></param>
        public void ColorLayingLawnForSpecialEliminate(List<Int2> list, bool posHaveLawn)
        {
            if (posHaveLawn)
            {
                M3Item item;
                foreach (var pos in list)
                {
                    item = M3ItemManager.Instance.gridItems[pos.x, pos.y];
                    if (item != null && CanLayingLawn(item))
                    {
                        item.ElementCrushFlag = true;
                        item.itemInfo.GetElement().needCreateLawn = true; // 设置草坪
                    }
                }
            }
        }

        /// <summary>
        /// 放射状铺草坪
        /// </summary>
        private void RadiateLayingLawn(List<Int2> list, bool originHasLawn)
        {
            bool lastHasLawn = originHasLawn;
            bool curHasLawn;
            bool canLayingLawn;
            M3Item item;
            foreach (var pos in list)
            {
                item = M3ItemManager.Instance.gridItems[pos.x, pos.y];
                if (item == null)
                {
                    continue;
                }

                curHasLawn = CheckHasLawn(item.GetGrid());
                canLayingLawn = CanLayingLawn(item);
                item.ElementCrushFlag = true;

                if (curHasLawn)
                {
                    lastHasLawn = true;
                    continue;
                }
                if (!lastHasLawn && !curHasLawn)
                {
                    continue;
                }
                if (lastHasLawn && !canLayingLawn)
                {
                    continue; //改为遇障碍物可以繼續了
                    //break;
                }

                lastHasLawn = true;
                item.itemInfo.GetElement().needCreateLawn = true; // 设置草坪
            }
        }

        #endregion

    }
}