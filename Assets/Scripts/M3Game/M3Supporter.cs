using Game.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Match3
{
    public class M3Supporter : MonoBehaviour
    {
        #region Static

        public static M3Supporter Instance;

        private bool console = false;

        #endregion

        #region Field

        private M3Item[] _suggestItems;
        private List<Int2> hintPoint = new List<Int2>();
        private List<Int2> checkMoveList = new List<Int2>();
        private List<GameObject> effects = new List<GameObject>();

        #endregion

        #region Property

        #endregion

        #region Method

        /// <summary>
        /// 检测可移动交换状态，不能移动则刷新元素
        /// </summary>
        public void CheckMoveStateEnter()
        {
            ResetPiece();
            if (CheckNoMoves())
            {
                Debuger.Log("无可交换的元素 执行 刷新");
                if (M3GameManager.Instance.isReady)
                    M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.Refresh);
                if (!M3GameManager.Instance.isAutoAI && M3GameManager.Instance.isReady)
                    KUIWindow.GetWindow<M3GameUIWindow>().ShowGameRefreshTips(true);
                FrameScheduler.instance.Add((!M3GameManager.Instance.isAutoAI && M3GameManager.Instance.isReady) ? M3Config.refreshWaitFrame : 0, delegate ()
                {
                    RefreshAllPiece();
                    if (!M3GameManager.Instance.isAutoAI && M3GameManager.Instance.isReady)
                        KUIWindow.GetWindow<M3GameUIWindow>().ShowGameRefreshTips(false);
                });
            }
        }

        /// <summary>
        /// 刷新所有三消单元，换位置
        /// </summary>
        public void RefreshAllPiece()
        {
            M3GameManager.Instance.ScreenLock = true;
            if (M3GameManager.Instance.isReady)
                M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.Refresh);
            int delay = (!M3GameManager.Instance.isAutoAI && M3GameManager.Instance.isReady) ? 5 : 0;
            FrameScheduler.instance.Add(delay, delegate ()
            {
                List<Int2> list = GetAllRefreshItem();
                bool canRefresh = true;
                list = RefreshItemLoop(list, out canRefresh);
                if (canRefresh)
                {
                    FrameScheduler.instance.Add(delay, delegate ()
                    {
                        if (!M3GameManager.Instance.isAutoAI)
                            ResetAllPiecePosition(list);
                        if (M3GameManager.Instance.isReady)
                            M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.CheckAndCrush);
                    });
                }
            });
        }

        /// <summary>
        /// 缓动改变View坐标
        /// </summary>
        /// <param name="list"></param>
        private void ResetAllPiecePosition(List<Int2> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                M3Item item = M3ItemManager.Instance.gridItems[list[i].x, list[i].y];
                item.isTweening = true;
                Vector3 target = GetItemPositionByGrid(list[i].x, list[i].y);
                KTweenUtils.LocalMoveTo(item.itemView.itemTransform, target, M3GameManager.Instance.isReady ? M3Config.refreshTime : 0f, () => { item.isTweening = false; });
            }
        }

        /// <summary>
        /// 获取所有可刷新位置的基础元素
        /// </summary>
        /// <returns></returns>
        private List<Int2> GetAllRefreshItem()
        {
            List<Int2> list = new List<Int2>();
            for (int i = M3Config.GridHeight - 1; i >= 0; i--)
            {
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    M3Item item = M3ItemManager.Instance.gridItems[i, j];
                    if (item != null && 
                        item.itemInfo.GetElement() != null && 
                        item.itemInfo.GetElement() is NormalElement && 
                        item.itemInfo.GetElement().data.IsBaseElement()
                        )
                    {
                        list.Add(new Int2(i, j));
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 显示可交换提示
        /// </summary>
        public void ShowMoveHint()
        {
            if (M3GameManager.Instance.isAutoAI)
                return;
            if (checkMoveList.Count == 0)
            {
                CheckMoveStateEnter();
            }
            if (checkMoveList.Count > 1)
            {
                if (hintPoint.Count == 2)
                {
                    if (hintPoint[0].y == hintPoint[1].y)
                    {
                        effects.Add(M3FxManager.Instance.PlayMoveHint(M3DirectionType.Col, hintPoint[0], hintPoint[1]));
                    }
                    else
                    {
                        effects.Add(M3FxManager.Instance.PlayMoveHint(M3DirectionType.Row, hintPoint[0], hintPoint[1]));
                    }
                    if (M3GameManager.Instance.soundManager != null)
                        M3GameManager.Instance.soundManager.PlayAutoTips();
                }
            }
        }

        /// <summary>
        /// 刷新三消单元位置
        /// </summary>
        /// <param name="list"></param>
        /// <param name="canRefresh"></param>
        /// <returns></returns>
        private List<Int2> RefreshItemLoop(List<Int2> list, out bool canRefresh)
        {
            canRefresh = true;
            int num = 0;
            int num2 = 500;
            list = RefreshList(list);
            while (num < num2 && CheckNoMoves())
            {
                list = RefreshList(list);
                num++;
            }
            if (num == num2)
            {
                canRefresh = false;
                if (!M3GameManager.Instance.isAutoAI)
                    M3GameManager.Instance.modeManager.ShowGameOver();
            }
            Debuger.Log("刷新次数：" + num);
            return list;
        }

        /// <summary>
        /// 检测是否已经没有可移动交换的元素了
        /// </summary>
        /// <returns></returns>
        public bool CheckNoMoves()
        {
            bool flag = false;
            for (int i = 0; i < M3Config.GridHeight; i++)
            {
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    if (i >= 0 && i < M3Config.GridHeight && j >= 0 && j < M3Config.GridWidth)
                    {
                        M3Item item = M3ItemManager.Instance.gridItems[i, j];
                        if (item != null && item.itemInfo.GetElement() != null && !item.itemInfo.GetElement().isObstacle
                            && IsItemMovesCanEliminate(i, j))
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                if (flag)
                    break;
            }
            return !flag;
        }

        public List<List<Int2>> GetAllCanMoves()
        {
            M3ItemManager manager = M3ItemManager.Instance;
            List<List<Int2>> list = new List<List<Int2>>();
            for (int i = 0; i < M3Config.GridHeight; i++)
            {
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    if (i >= 0 && i < M3Config.GridHeight && j >= 0 && j < M3Config.GridWidth)
                    {
                        M3Item item = M3ItemManager.Instance.gridItems[i, j];
                        if (item != null && item.itemInfo.GetElement() != null && !item.itemInfo.GetElement().isObstacle)
                        {
                            for (int k = 0; k < 4; k++)
                            {
                                int num = i + M3Const.DirectionOffset[k].x;
                                int num2 = j + M3Const.DirectionOffset[k].y;
                                if (M3GameManager.Instance.CheckValid(num, num2)
                                    && (M3ItemManager.Instance.gridItems[num, num2] != null)
                                    && M3GameManager.Instance.CheckSwap(manager.gridItems[i, j], manager.gridItems[num, num2]))
                                {
                                    M3GameManager.Instance.specialHandler.GetSpecialType(manager.gridItems[i, j], manager.gridItems[num, num2]);
                                    if (M3GameManager.Instance.specialHandler.HaveSpecialInExchange())
                                    {
                                        list.Add(new List<Int2> { new Int2(i, j), new Int2(num, num2) });

                                    }
                                    else if (manager.gridItems[num, num2] != null && manager.gridItems[num, num2].itemInfo.GetElement() != null
                                             && !manager.gridItems[num, num2].itemInfo.GetElement().isObstacle)
                                    {

                                        M3GameManager.Instance.SwapItemPosition(manager.gridItems[i, j], manager.gridItems[num, num2]);
                                        if (IsItemCanEliminate(num, num2, true))
                                        {
                                            list.Add(new List<Int2> { new Int2(i, j), new Int2(num, num2) });
                                        }

                                        M3GameManager.Instance.SwapItemPosition(manager.gridItems[i, j], manager.gridItems[num, num2]);
                                    }
                                    M3GameManager.Instance.specialHandler.ResetVars();
                                }
                            }
                        }
                    }
                }

            }
            return list;
        }

        /// <summary>
        /// 是否可移动交换消除
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsItemMovesCanEliminate(int x, int y)
        {
            bool flag = false;
            M3ItemManager manager = M3ItemManager.Instance;
            checkMoveList.Clear();
            hintPoint.Clear();
            for (int i = 0; i < 4; i++)
            {
                checkMoveList.Add(new Int2(x, y));
                int num = x + M3Const.DirectionOffset[i].x;
                int num2 = y + M3Const.DirectionOffset[i].y;
                if (M3GameManager.Instance.CheckValid(num, num2) && (manager.gridItems[num, num2] != null) && M3GameManager.Instance.CheckSwap(manager.gridItems[x, y], manager.gridItems[num, num2]))
                {
                    M3GameManager.Instance.specialHandler.GetSpecialType(manager.gridItems[x, y], manager.gridItems[num, num2]);
                    if (M3GameManager.Instance.specialHandler.HaveSpecialInExchange())
                    {
                        flag = true;
                        checkMoveList.Add(new Int2(num, num2));
                        hintPoint.Add(new Int2(x, y));
                        hintPoint.Add(new Int2(num, num2));
                    }
                    else if (manager.gridItems[num, num2] != null && manager.gridItems[num, num2].itemInfo.GetElement() != null
                        && !manager.gridItems[num, num2].itemInfo.GetElement().isObstacle)
                    {
                        M3GameManager.Instance.SwapItemPosition(manager.gridItems[x, y], manager.gridItems[num, num2]);
                        if (IsItemCanEliminate(num, num2, true))
                        {
                            flag = true;
                        }
                        if (flag)
                        {
                            hintPoint.Add(new Int2(x, y));
                            hintPoint.Add(new Int2(num, num2));
                        }
                        M3GameManager.Instance.SwapItemPosition(manager.gridItems[x, y], manager.gridItems[num, num2]);
                    }
                    M3GameManager.Instance.specialHandler.ResetVars();
                    if (flag)
                    {
                        break;
                    }
                    checkMoveList.Clear();
                    hintPoint.Clear();
                }
            }
            if (!flag)
            {
                checkMoveList.Clear();
                hintPoint.Clear();
            }
            return flag;
        }

        public Vector2 WordToScenePoint(Vector3 worldPosition, RectTransform rect, Canvas canvas)
        {
            var screenPoint = Camera.main.WorldToScreenPoint(worldPosition);
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect as RectTransform, screenPoint, canvas.worldCamera, out pos);
            return pos;
        }

        private bool IsItemCanEliminate(int x, int y, bool v)
        {
            ConnectedGraphs connectedGraphs = new ConnectedGraphs(x, y, true);
            for (int i = 0; i < connectedGraphs.GetGraph().Count; i++)
            {
                if (i != 0)
                {
                    checkMoveList.Add(new Int2((int)connectedGraphs.GetGraph()[i].x, (int)connectedGraphs.GetGraph()[i].y));
                }
            }
            if (connectedGraphs.shape != string.Empty)
            {
            }
            return connectedGraphs.shape != string.Empty;
        }

        public void ResetPiece()
        {
            for (int i = 0; i < effects.Count; i++)
            {
                GameObject.Destroy(effects[i]);
            }
            effects.Clear();
            checkMoveList.Clear();
            hintPoint.Clear();
        }

        /// <summary>
        /// 随机互换列表中的三消单元
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<Int2> RefreshList(List<Int2> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int random = M3Supporter.Instance.GetRandomInt(0, list.Count);
                //互换两个位置后不能产生消除
                if (CheckItemLegal(list[random].x, list[random].y, M3ItemManager.Instance.gridItems[list[i].x, list[i].y].itemInfo.GetElement().GetColor())
                    && CheckItemLegal(list[i].x, list[i].y, M3ItemManager.Instance.gridItems[list[random].x, list[random].y].itemInfo.GetElement().GetColor())
                    )
                {
                    SwapItem(list[i].x, list[i].y, list[random].x, list[random].y);
                    var tmp = list[i];
                    list[i] = list[random];
                    list[random] = tmp;
                }
            }
            return list;
        }

        public void SwapItem(int x1, int y1, int x2, int y2)
        {
            M3GameManager.Instance.SwapItemPosition(M3ItemManager.Instance.gridItems[x1, y1], M3ItemManager.Instance.gridItems[x2, y2]);
        }

        /// <summary>
        /// 检测是否是空行
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="isRow"></param>
        /// <returns></returns>
        public bool CheckIsLineEmpty(int x, int y, bool isRow)
        {
            bool flag = false;
            if (isRow)
            {
                for (int i = 0; i < M3Config.GridWidth; i++)
                {
                    if (M3GridManager.Instance.gridCells[x, i] != null)
                    {
                        flag = true;
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < M3Config.GridHeight; i++)
                {
                    if (M3GridManager.Instance.gridCells[i, y] != null)
                    {
                        flag = true;
                        break;
                    }
                }
            }
            return flag;
        }

        /// <summary>
        /// 检测刷新坐标合法性，周围不能有同颜色可消除 三个同颜色相连可消除
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public bool CheckItemLegal(int x, int y, ItemColor color)
        {
            M3ItemManager manager = M3ItemManager.Instance;
            bool flag1 = true;
            bool flag2 = true;

            //上两格
            if (x > 1 && CheckSameColor(x - 1, y, color) && CheckSameColor(x - 2, y, color))
            {
                flag1 = false;
            }
            //下两格
            if (x < M3Config.GridHeight - 2 && CheckSameColor(x + 1, y, color) && CheckSameColor(x + 2, y, color))
            {
                flag1 = false;
            }
            //左两格
            if (y > 1 && CheckSameColor(x, y - 1, color) && CheckSameColor(x, y - 2, color))
            {
                flag2 = false;
            }
            //右两格
            if (y < M3Config.GridWidth - 2 && CheckSameColor(x, y + 1, color) && CheckSameColor(x, y + 2, color))
            {
                flag2 = false;
            }
            //上下两格
            if (x > 0 && x < M3Config.GridHeight - 1 && CheckSameColor(x - 1, y, color) && CheckSameColor(x + 1, y, color))
            {
                flag1 = false;
            }
            //左右两格
            if (y > 0 && y < M3Config.GridWidth - 1 && CheckSameColor(x, y - 1, color) && CheckSameColor(x, y + 1, color))
            {
                flag2 = false;
            }
            return flag2 & flag1;
        }

        /// <summary>
        /// 检测坐标元素颜色是否匹配
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        private bool CheckSameColor(int x, int y, ItemColor color)
        {
            M3Item item = M3ItemManager.Instance.gridItems[x, y];

            if (item == null || item.itemInfo.GetElement() == null)
                return false;

            ItemColor color2 = item.itemInfo.GetPartakeEliminateElement().GetColor();
            if (color2 == ItemColor.fNone)
                return false;

            if ((int)color2 < 7)
            {
                return color == color2;
            }
            return false;
        }

        public int GetRandomInt(int min, int max)
        {
            return M3RandomMgr.Instance.GetRandomInt(min, max);
            //return UnityEngine.Random.Range(min, max);
        }

        public Vector3 GetItemPositionByGrid(int x, int y)
        {
            return new Vector3(y * M3Config.DistancePerUnit, -x * M3Config.DistancePerUnit);
        }

        public Vector3 GetItemWorldPositon(int x, int y)
        {
            M3Item item = M3ItemManager.Instance.gridItems[x, y];
            return item.itemView.itemTransform.position;
        }

        public Int2 GetPosByDirection(int x, int y, M3Direction direction)
        {
            switch (direction)
            {
                case M3Direction.Top:
                    return new Int2(x - 1, y);
                case M3Direction.Bottom:
                    return new Int2(x + 1, y);
                case M3Direction.Left:
                    return new Int2(x - 1, y - 1);
                case M3Direction.Rigth:
                    return new Int2(x - 1, y + 1);
                default:
                    return new Int2(x, y);
            }
        }

        private List<Int2> GetListPos(int x, int y)
        {
            var cells = M3GridManager.Instance.gridCells;

            var tmpList = new List<Int2>(4);
            if (y < M3Config.GridWidth - 1 && cells[x, y + 1] != null)
            {
                tmpList.Add(new Int2(x, y + 1));
            }
            if (y - 1 >= 0 && cells[x, y - 1] != null)
            {
                tmpList.Add(new Int2(x, y - 1));
            }
            if (x < M3Config.GridHeight - 1 && cells[x + 1, y] != null)
            {
                tmpList.Add(new Int2(x + 1, y));
            }
            if (x - 1 >= 0 && cells[x - 1, y] != null)
            {
                tmpList.Add(new Int2(x - 1, y));
            }
            return tmpList;
        }

        public string TransformTimer(int time)
        {
            int min = time / 60;
            int sec = time % 60;
            return min + ":" + sec;
        }

        #endregion

        #region Unity

        private void Awake()
        {
            Instance = this;
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                console = !console;
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log(M3GameManager.Instance.gameFsm.GetFSM().GetCurrentStateEnum());
            }
        }

        private void OnGUI()
        {
            if (console)
            {
                GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                GUI.skin.label.normal.textColor = Color.red;
                GUILayout.Label("掉落速度:" + M3Config.DropInitialSpeed, new GUILayoutOption[0]);
                if (GUILayout.Button("+ 5%", new GUILayoutOption[] { GUILayout.Width(75), GUILayout.Height(55) }))
                {
                    M3Config.DropInitialSpeed += M3Config.PieceDropSpeedCycle;
                }
                if (GUILayout.Button("- 5%", new GUILayoutOption[] { GUILayout.Width(75), GUILayout.Height(55) }))
                {
                    M3Config.DropInitialSpeed -= M3Config.PieceDropSpeedCycle;
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                GUILayout.Label("掉落加速度:" + M3Config.DropAcceleratedSpeed, new GUILayoutOption[0]);
                if (GUILayout.Button("+ 5%", new GUILayoutOption[] { GUILayout.Width(75), GUILayout.Height(55) }))
                {
                    M3Config.DropAcceleratedSpeed += M3Config.PieceDropAccelerateCycle;
                }
                if (GUILayout.Button("- 5%", new GUILayoutOption[] { GUILayout.Width(75), GUILayout.Height(55) }))
                {
                    M3Config.DropAcceleratedSpeed -= M3Config.PieceDropAccelerateCycle;
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                GUILayout.Label("最大速度:" + M3Config.DropSpeedMax, new GUILayoutOption[0]);
                if (GUILayout.Button("+ 5%", new GUILayoutOption[] { GUILayout.Width(75), GUILayout.Height(55) }))
                {
                    M3Config.DropSpeedMax += M3Config.DropSpeedMaxCycle;
                }
                if (GUILayout.Button("- 5%", new GUILayoutOption[] { GUILayout.Width(75), GUILayout.Height(55) }))
                {
                    M3Config.DropSpeedMax -= M3Config.DropSpeedMaxCycle;
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                GUILayout.Label("斜落速度:" + M3Config.InclinedDropTime, new GUILayoutOption[0]);
                if (GUILayout.Button("+ 5%", new GUILayoutOption[] { GUILayout.Width(75), GUILayout.Height(55) }))
                {
                    M3Config.InclinedDropTime += M3Config.InclinedDropTimeCycle;
                }
                if (GUILayout.Button("- 5%", new GUILayoutOption[] { GUILayout.Width(75), GUILayout.Height(55) }))
                {
                    M3Config.InclinedDropTime -= M3Config.InclinedDropTimeCycle;
                }
                GUILayout.EndHorizontal();
            }
        }
#endif

        #endregion

    }
}