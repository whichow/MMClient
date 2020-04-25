using System.Collections.Generic;
using UnityEngine;

namespace Game.Match3
{
    //public class MatchSpecialType
    //{
    //    public Int2 pos;
    //    public ElementSpecial special;
    //    public ItemColor color;

    //    public MatchSpecialType(Int2 p, ElementSpecial s, ItemColor c)
    //    {
    //        pos = p;
    //        special = s;
    //        color = c;
    //    }
    //}

    public class MatchSpecialType
    {
        public Vector2 position
        {
            get;
            set;
        }

        public string type
        {
            get;
            set;
        }

        public ItemColor color
        {
            get;
            set;
        }
        public bool isHide
        {
            get;
            set;
        }
    }

    public class M3Matcher
    {

        private List<List<Vector2>> eliminateList;

        private List<Int2> tmpList = new List<Int2>();

        private List<MatchSpecialType> specialList = new List<MatchSpecialType>();

        private bool[,] computeMap;

        private Vector2 pieceFrom;
        private Vector2 pieceTo;

        public M3Matcher()
        {
            eliminateList = new List<List<Vector2>>();
            specialList = new List<MatchSpecialType>();
        }

        private void ClearVars()
        {
            eliminateList.Clear();
            specialList.Clear();
        }

        /// <summary>
        /// 标记为已经计算过了
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void MarkComputed(int x, int y)
        {
            this.computeMap[x, y] = true;
        }

        private void InitComputeMap()
        {
            this.computeMap = new bool[M3Config.GridHeight, M3Config.GridWidth];
            for (int i = 0; i < M3Config.GridHeight; i++)
            {
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    this.computeMap[i, j] = false;
                }
            }
        }

        public List<MatchSpecialType> GetSpecialList()
        {
            return specialList;
        }

        public void GetSinglePieceResult(M3Item item)
        {
            this.ClearVars();
            this.InitComputeMap();
            this.FindConnectedGraph(item.itemInfo.posX, item.itemInfo.posY, true);
        }

        public void FindConnectedGraph(int x, int y, bool isSingle)
        {
            if (M3ItemManager.Instance.gridItems[x, y] == null || M3ItemManager.Instance.gridItems[x, y].IsNoNeedToCompute())
            {
                MarkComputed(x, y);
                return;
            }

            ConnectedGraphs connectedGraphs = new ConnectedGraphs(x, y, isSingle);
            List<Vector2> graph = connectedGraphs.GetGraph();
            if (graph.Count >= 3)
            {
                CreateSpecialItem(graph, connectedGraphs);
                //var middleItem = M3ItemManager.Instance.gridItems[(int)graph[graph.Count / 2].x, (int)graph[graph.Count / 2].y];
                for (int i = 0; i < graph.Count; i++)
                {
                    Vector2 vec = graph[i];

                    var item = M3ItemManager.Instance.gridItems[(int)vec.x, (int)vec.y];
                    if (item != null)
                    {
                        var ele = item.itemInfo.GetElement();
                        if (ele != null && (ele.eName == M3ElementType.NormalElement))
                        {
                            item.crushScore = ele.data.config.Point;
                        }
                    }

                    MarkComputed((int)vec.x, (int)vec.y);
                }

                //middleItem.crushScore = score;
                eliminateList.Add(graph);
            }
        }

        /// <summary>
        /// 标记要生成的特殊元素（如炸弹）
        /// </summary>
        /// <param name="elimlist"></param>
        /// <param name="graph"></param>
        private void CreateSpecialItem(List<Vector2> elimlist, ConnectedGraphs graph)
        {
            if (graph.shape == "3")
            {
                return;
            }

            //取特殊元素生成的坐标
            MatchSpecialType matchSpecialType = new MatchSpecialType();
            matchSpecialType.position = elimlist[0];
            if (this.IsManualCrush())
            {
                for (int i = 0; i < elimlist.Count; i++)
                {
                    if (IsVector2Same(elimlist[i], this.pieceTo))
                    {
                        matchSpecialType.position = elimlist[i];
                        break;
                    }
                    if (IsVector2Same(elimlist[i], this.pieceFrom))
                    {
                        //目标的为非参于消除元素，用起始拖动元素的坐标
                        matchSpecialType.position = elimlist[i];
                        break;
                    }
                }
            }
            else if (graph.GetCrossPosition().x != -1f)
            {
                Debuger.LogError("-------------->IsManualCrush()==false");
                matchSpecialType.position = graph.GetCrossPosition();
            }

            //设置坐标内元素为特殊元素类型
            M3Item m3Item = M3ItemManager.Instance.gridItems[(int)matchSpecialType.position.x, (int)matchSpecialType.position.y];
            if (m3Item.IsObstacle())
            {
                return;
            }
            NormalElement normalPiece = (NormalElement)m3Item.itemInfo.GetPartakeEliminateElement();
            //matchSpecialType.type = graph.shape;
            //matchSpecialType.color = m3Item.itemInfo.GetElement().GetColor();
            //matchSpecialType.isHide = false;
            //specialList.Add(matchSpecialType);
            normalPiece.specialType = graph.shape;
        }

        private bool IsManualCrush()
        {
            return !IsVector2Same(this.pieceTo, new Vector2(-1f, -1f));
        }

        private bool IsVector2Same(Vector2 v1, Vector2 v2)
        {
            return v1.x == v2.x && v1.y == v2.y;
        }

        private bool IsComputed(int x, int y)
        {
            return this.computeMap[x, y];
        }

        /// <summary>
        /// 相关消除元素列表
        /// </summary>
        /// <returns></returns>
        public List<List<Vector2>> GetEliminateList()
        {
            return eliminateList;
        }

        /// <summary>
        /// 获取交换产生的消除结果
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void GetNewResulet(M3Item from, M3Item to)
        {
            if (from != null && to != null)
            {
                pieceFrom = new Vector2(from.itemInfo.posX, from.itemInfo.posY);
                pieceTo = new Vector2(to.itemInfo.posX, to.itemInfo.posY);
            }
            ClearVars();
            InitComputeMap();
            Compute();
        }

        private void Compute()
        {
            for (int i = 0; i < M3Config.GridHeight; i++)
            {
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    if (!this.IsComputed(i, j))
                    {
                        this.FindConnectedGraph(i, j, false);
                    }
                }
            }
        }

    }
}