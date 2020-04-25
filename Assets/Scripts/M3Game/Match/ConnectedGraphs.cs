using System.Collections.Generic;
using UnityEngine;

namespace Game.Match3
{
    public class ConnectedGraphs
    {
        /// <summary>
        /// 方向 0左 1下 2右 3 上  坐标系是x向下，y向右
        /// </summary>
        private Vector2[] directionOffset = new Vector2[]
        {
            new Vector2(0f, -1f), //0左
            new Vector2(1f, 0f),  //1下
            new Vector2(0f, 1f),  //2右
            new Vector2(-1f, 0f)  //3上
        };

        //private Vector2[] m_squareOffset = new Vector2[]
        //{
        //        new Vector2(-1f, 0f),
        //        new Vector2(-1f, 1f),
        //        new Vector2(0f, 2f),
        //        new Vector2(1f, 2f),
        //        new Vector2(2f, 1f),
        //        new Vector2(2f, 0f),
        //        new Vector2(0f, -1f),
        //        new Vector2(1f, -1f)
        //};

        private M3Matcher match;

        /// <summary>
        /// 关连元素的坐标列表
        /// </summary>
        private List<Vector2> graph;

        /// <summary>
        /// 寻找到的相连元素列表（每方向一个列表）
        /// </summary>
        private List<List<Vector2>> lines;

        /// <summary>
        /// 当前棋盘坐标点
        /// </summary>
        private Vector2 origin;

        private Vector2 crossPosition = new Vector2(-1f, -1f);

        /// <summary>
        /// 单个块检测
        /// </summary>
        private bool isSingle;

        public string shape = string.Empty;

        public ConnectedGraphs(int _x, int _y, bool single)
        {
            //Debuger.LogFormat("------->ConnectedGraphs({0},{1},{2})", _x,  _y,  single);
            this.origin = new Vector2((float)_x, (float)_y);
            this.isSingle = single;
            this.InitArrayList();
            this.PutToGraph(new Vector2((float)_x, (float)_y));
            this.GetLines();
            this.Find();
        }

        private void InitArrayList()
        {
            this.graph = new List<Vector2>();
            this.lines = new List<List<Vector2>>();
        }

        private void Find()
        {
            this.Find_3();
            this.Find_4();
            this.Find_5();
            this.Find_5Cross();
            this.Find_5T();
            this.Find_5L();
            //this.find_4Square();
            for (int i = 0; i < 2; i++)
            {
                if (lines[i].Count >= 3)
                    PutLineToGraph(lines[i]);
            }
        }

        private void GetLines()
        {
            if (this.isSingle)
            {
                //单个块，需要 左下右上 四方向寻找相连元素
                for (int i = 0; i <= 3; i++)
                {
                    this.lines.Add(this.FindLineForDirection(this.origin, i));
                }
                if (this.lines[2].Count > 0)
                    this.lines[2].RemoveAt(0);
                this.lines[0].AddRange(this.lines[2]);
                if (this.lines[3].Count > 0)
                    this.lines[3].RemoveAt(0);
                this.lines[1].AddRange(this.lines[3]);
                this.lines.RemoveAt(2);
                this.lines.RemoveAt(2);

                this.FixOrigin();
            }
            else
            {
                //向下右方寻找相连元素
                for (int j = 1; j <= 2; j++)
                {
                    this.lines.Add(this.FindLineForDirection(this.origin, j));
                }
            }
        }

        private void FixOrigin()
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < this.lines[i].Count; j++)
                {
                    if (this.lines[i][j].x <= this.origin.x && this.lines[i][j].y <= this.origin.y)
                    {
                        this.origin = this.lines[i][j];
                    }
                    else if (this.lines[i][j].x < this.origin.x)
                    {
                        this.origin = this.lines[i][j];
                    }
                }
            }
        }

        /// <summary>
        /// 查找三消
        /// </summary>
        private void Find_3()
        {
            for (int i = 0; i < this.lines.Count; i++)
            {
                List<Vector2> list = this.lines[i];
                if (list.Count >= 3)
                {
                    for (int j = 0; j < list.Count; j++)
                    {
                        this.PutToGraph(list[j]);
                    }
                    this.shape = "3";
                    break;
                }
            }
        }

        /// <summary>
        /// 查找四消
        /// </summary>
        private void Find_4()
        {
            if (this.graph.Count >= 3)
            {
                for (int i = 0; i < this.lines.Count; i++)
                {
                    List<Vector2> list = this.lines[i];
                    if (list.Count >= 4)
                    {
                        this.PutToGraph(list[3]);
                        bool graphIsVertical = this.GetGraphIsVertical();
                        this.shape = ((!graphIsVertical) ? "4V" : "4H");
                        this.crossPosition = ((!graphIsVertical) ? this.graph[0] : this.graph[3]);
                        break;
                    }
                }
            }
        }


        /// <summary>
        /// 查找五消 十字型
        /// </summary>
        private void Find_5Cross()
        {
            if (this.shape == "3" || this.shape == "4H" || this.shape == "4V")
            {
                List<Vector2> tmp = new List<Vector2>();
                for (int i = 0; i < graph.Count; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        List<Vector2> list = this.FindLineForDirection(this.graph[i], (!this.GetGraphIsVertical()) ? (j * 2 + 1) : (j * 2));
                        if (list.Count >= 2)
                        {
                            for (int k = 0; k < list.Count; k++)
                            {
                                if (!tmp.Contains(list[k]))
                                    tmp.Add(list[k]);
                            }
                        }
                    }
                    if (tmp.Count >= 3)
                    {
                        this.PutLineToGraph(tmp);
                        if (tmp.Count == 5)
                        {
                            //五连优先级最高
                            this.shape = "5";
                        }
                        else
                        {
                            this.shape = "5T";
                        }
                        this.crossPosition = tmp[0];
                        break;
                    }
                    tmp.Clear();
                }

                //Debug.Log(tmp.Count);
                //Debug.Log(m_graph.Count);
                //if (this.m_graph.Count >= 5)
                //{
                //    break;
                //}
            }
        }

        /// <summary>
        /// 查找五消 L型
        /// </summary>
        private void Find_5L()
        {
            if (this.shape == "3")
            {
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        int num = (!this.GetGraphIsVertical()) ? (j * 2 + 1) : (j * 2);
                        List<Vector2> list = this.FindLineForDirection(this.graph[i * 2], num);
                        List<Vector2> list2 = this.FindLineForDirection(this.graph[i * 2], (!this.GetGraphIsVertical()) ? (4 - num) : (2 - num));
                        if (list.Count == 3 && list2.Count < 3)
                        {
                            this.PutLineToGraph(list);
                            this.shape = "5L";
                            this.crossPosition = list[0];
                            break;
                        }
                    }
                    //if (this.graph.Count >= 5)
                    //{
                    //    break;
                    //}
                }
            }
        }

        /// <summary>
        /// 查找五消 T型
        /// </summary>
        private void Find_5T()
        {
            if (this.shape == "3" || this.shape == "4H" || this.shape == "4V")
            {
                for (int i = 1; i < this.graph.Count - 1; i++)
                {
                    List<Vector2> tmpList = new List<Vector2>();
                    for (int j = 0; j < 2; j++)
                    {
                        List<Vector2> list = this.FindLineForDirection(this.graph[i], (!this.GetGraphIsVertical()) ? (j * 2 + 1) : (j * 2));
                        if (j != 0)
                            list.RemoveAt(0);
                        if (list.Count > 0)
                            tmpList.AddRange(list);
                    }
                    if (tmpList.Count >= 3)
                    {
                        this.PutLineToGraph(tmpList);
                        this.shape = "5T";
                        this.crossPosition = tmpList[0];
                        break;
                    }
                    //if (this.graph.Count >= 5)
                    //{
                    //    break;
                    //}
                }
            }
        }

        /// <summary>
        /// 查找五消 一字型 优先级最高
        /// </summary>
        private void Find_5()
        {
            //始终检测5连
            //if (this.shape == "4H" || this.shape == "4V")
            {
                for (int i = 0; i < this.lines.Count; i++)
                {
                    List<Vector2> list = this.lines[i];
                    if (list.Count == 5)
                    {
                        this.PutToGraph(list[4]);
                        this.shape = "5";
                        this.crossPosition = this.graph[2];
                        break;
                    }
                }
                if (shape == "5")
                {
                    for (int i = 1; i < this.graph.Count - 1; i++)
                    {
                        List<Vector2> tmpList = new List<Vector2>();
                        for (int j = 0; j < 2; j++)
                        {
                            List<Vector2> list = this.FindLineForDirection(this.graph[i], (!this.GetGraphIsVertical()) ? (j * 2 + 1) : (j * 2));
                            if (j != 0)
                                list.RemoveAt(0);
                            if (list.Count > 0)
                                tmpList.AddRange(list);
                        }
                        if (tmpList.Count >= 3)
                        {
                            this.PutLineToGraph(tmpList);
                        }
                    }

                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void Find_6()
        {
            if (this.shape == "5")
            {
                for (int i = 0; i < 2; i++)
                {
                    List<Vector2> list = this.FindLineForDirection(this.graph[2], (!this.GetGraphIsVertical()) ? (i * 2 + 1) : (i * 2));
                    if (list.Count >= 2)
                    {
                        this.PutLineToGraph(list);
                        this.shape = "6";
                        this.crossPosition = this.graph[2];
                    }
                }
            }
        }

        /// <summary>
        /// 按照方向来寻找可相连的元素
        /// </summary>
        /// <param name="start"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        private List<Vector2> FindLineForDirection(Vector2 start, int direction)
        {
            List<Vector2> list = new List<Vector2>();
            Vector2 vector = start;
            while (this.IsPieceValid(vector))
            {
                if (!this.IsPieceNormalAndSame(vector))
                {
                    break;
                }
                list.Add(vector);
                vector = this.GetNextPiece(vector, direction);
            }
            return list;
        }

        private Vector2 GetNextPiece(Vector2 origin, int direction)
        {
            return origin + this.directionOffset[direction];
        }

        /// <summary>
        /// 坐标是否有效
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        private bool IsPieceValid(Vector2 pos)
        {
            return pos.x >= 0f && pos.x < M3Config.GridHeight && pos.y >= 0f && pos.y < M3Config.GridWidth;
        }

        /// <summary>
        /// 检测是否是可以相连的相同元素
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        private bool IsPieceNormalAndSame(Vector2 pos)
        {
            if (!this.IsPieceValid(pos))
            {
                return false;
            }
            M3Grid grid = M3GridManager.Instance.gridCells[(int)this.origin.x, (int)this.origin.y];
            M3Grid grid2 = M3GridManager.Instance.gridCells[(int)pos.x, (int)pos.y];
            if (grid == null || grid2 == null)
                return false;

            M3Item item = grid.GetItem();
            M3Item item2 = grid2.GetItem();
            if (item == null || item2 == null)
                return false;
            if (item.itemInfo.GetPartakeEliminateElement() == null || item2.itemInfo.GetPartakeEliminateElement() == null)
                return false;
            if (!item.itemInfo.GetPartakeEliminateElement().data.CanNormalEliminate() || !item2.itemInfo.GetPartakeEliminateElement().data.CanNormalEliminate())
                return false;
            return
                grid == grid2
                || (item2.itemInfo.GetElement() != null
                && !item2.isCrushing
                /*&& item2.itemInfo.GetPartakeEliminateElement() is NormalElement*/
                //&& item2.itemInfo.GetPartakeEliminateElement().data.GetColor()!= ItemColor.fNone
                //&& item.itemInfo.GetPartakeEliminateElement().data.GetColor()!= ItemColor.fNone
                && item2.itemInfo.GetPartakeEliminateElement().data.CanNormalEliminate()
                && item.itemInfo.GetPartakeEliminateElement().data.CanNormalEliminate()
                && item2.itemInfo.GetPartakeEliminateElement().data.GetColor() == item.itemInfo.GetPartakeEliminateElement().data.GetColor()
                && !item2.GetGrid().CheckDrop(true));

        }
        private void PutToGraph(Vector2 pos)
        {
            if (!this.graph.Contains(pos))
            {
                this.graph.Add(pos);
            }
        }

        private void PutLineToGraph(List<Vector2> line)
        {
            for (int i = 0; i < line.Count; i++)
            {
                this.PutToGraph(line[i]);
            }
        }

        private bool GetGraphIsVertical()
        {
            Vector2 vector = this.graph[0];
            Vector2 vector2 = this.graph[this.graph.Count - 1];
            return vector.y == vector2.y;
        }
        private bool IsVerticalLine(Vector2 v1, Vector2 v2)
        {
            return (v1.x != v2.x) && (v1.y == v2.y);
        }
        private bool IsHorizontalLine(Vector2 v1, Vector2 v2)
        {
            return (v1.x == v2.x) && (v1.y != v2.y);
        }

        /// <summary>
        /// 获取关连元素的坐标列表
        /// </summary>
        /// <returns></returns>
        public List<Vector2> GetGraph()
        {
            return this.graph;
        }

        public Vector2 GetCrossPosition()
        {
            return this.crossPosition;
        }
    }
}