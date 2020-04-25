using Game.DataModel;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Match3
{
    public enum M3Direction
    {
        Top = 1,
        Bottom = 2,
        Left = 4,
        Rigth = 8,
    }
    public enum M3DirectionType
    {
        None = 0,
        Row = 1,
        Col = 2,
    }
    public class M3GridManager : Singleton<M3GridManager>
    {
        #region Field

        /// <summary>
        /// 
        /// </summary>
        public GameObject gridParent;
        /// <summary>
        /// 
        /// </summary>
        public GameObject gridPrefab;
        /// <summary>
        /// 
        /// </summary>
        public GameObject borderParent;

        public GameObject gridBoard;
        /// <summary>
        /// 
        /// </summary>
        public GameObject[] borderPrefabs = new GameObject[6];

        public GameObject maskObj;

        private M3CellData[,] _map;

        private M3Grid[,] _gridCells;

        public bool dropLock;

        public int SpecialBoomCount;

        #endregion

        #region Property

        public M3Grid[,] gridCells
        {
            get { return _gridCells; }
            set { _gridCells = value; }
        }

        public M3CellData[,] map
        {
            get { return _map; }
            set { _map = value; }
        }

        #endregion

        #region Method

        private void InstantiateCell(int x, int y, M3CellData data)
        {
            var grid = new M3Grid();
            grid.Init(data);
            if (!M3GameManager.Instance.isAutoAI)
            {
                var tmpObj = GameObject.Instantiate(gridPrefab);
                grid.InitView(tmpObj.transform);
            }
            List<ElementXDM> list = new List<ElementXDM>();
            for (int i = 0; i < data.elementsList.Count; i++)
            {
                var ele = XTable.ElementXTable.GetByID(data.elementsList[i]);
                if (ele != null)
                    list.Add(ele);
            }
            grid.gridInfo.AddElement(list);

            if (!M3GameManager.Instance.isAutoAI)
                grid.gridView.UpdateView(grid.gridInfo);
            gridCells[x, y] = grid;
        }

        public bool CheckSpawnGrid(int x, int y)
        {
            return !(gridCells[x, y].gridInfo.spawnPointType == DropPointType.None);
        }

        #endregion

        #region Unity

        private void Awake()
        {
        }
        private void Start()
        {
        }
        #endregion

        public void Init()
        {
            gridParent = M3GameManager.Instance.gameScreen.transform.Find("Board/Grid").gameObject;
            GameObject go;
            KAssetManager.Instance.TryGetMatchPrefab("Grid", out go);
            gridPrefab = go;
            borderParent = M3GameManager.Instance.gameScreen.transform.Find("Board/Border").gameObject;
            gridBoard = M3GameManager.Instance.gameScreen.transform.Find("Board").gameObject;
            maskObj = M3GameManager.Instance.gameScreen.transform.Find("Board/Mask").gameObject;
        }

        /// <summary>
        /// Create Grid map
        /// </summary>
        /// <param name="mapName">name of map</param>
        /// <returns></returns>
        public void CreateGridMap()
        {
            InitModel();
            InitView();
        }

        public void InitModel()
        {
            var levelData = M3GameManager.Instance.level;
            M3Config.GridHeight = levelData.lvMapHeight;
            M3Config.GridWidth = levelData.lvMapWidth;
            M3Config.GridHeightIndex = levelData.lvMapHeight - 1;
            M3Config.GridWidthIndex = levelData.lvMapWidth - 1;

            map = new M3CellData[levelData.lvMapHeight, levelData.lvMapWidth];
            for (int i = 0; i < map.Length; i++)
            {
                map[levelData.celldata[i].gridX, levelData.celldata[i].gridY] = levelData.celldata[i];
            }

            CreateGrid(map);
            InitPort(M3GameManager.Instance.level);//初始化掉落口
            InitPortal(M3GameManager.Instance.level);//初始化传送门
            InitBlankInfo();//初始化下落格
            M3ItemManager.Instance.CreateMap(map);//初始化棋盘
            M3GameManager.Instance.conveyorManager.Init();//初始化传送带
            M3GameManager.Instance.hiddenManager.Init();
        }

        public void InitView()
        {
            for (int i = 0; i < 6; i++)
            {
                KAssetManager.Instance.TryGetMatchPrefab("Frame_" + i, out borderPrefabs[i]);
            }
            CreateBorder(map);
            var board = new GridBoarder();
            board.InitBorders();
            GenMask();
            AdjustBoard();
        }

        private void AdjustBoard()
        {
            var vec = gridBoard.transform.localPosition;
            //gridBoard.transform.localPosition = new Vector3(vec.x, (M3Config.GridHeight%2==0?M3Config.GridHeight-1:M3Config.GridHeight) / 2.0f, vec.z);
            gridBoard.transform.localPosition = new Vector3(1 - (M3Config.RealLastCol - M3Config.RealFirsCol) / 2.0f - M3Config.RealFirsCol, (M3Config.RealLastRow - M3Config.RealFirstRow) / 2.0f + M3Config.RealFirstRow, vec.z);
        }

        /// <summary>
        /// 初始化下落格
        /// </summary>
        public void InitBlankInfo()
        {
            for (int x = 0; x < M3Config.GridHeight; x++)
            {
                for (int y = 0; y < M3Config.GridWidth; y++)
                {
                    if (gridCells[x, y] != null)
                    {
                        gridCells[x, y].InitBlankInfo();
                        gridCells[x, y].CreateFlickerPortal();
                    }
                }
            }
        }

        /// <summary>
        /// 初始化传送门
        /// </summary>
        /// <param name="levelData"></param>
        public void InitPortal(M3LevelData levelData)
        {
            for (int i = 0; i < levelData.FlickerPortalList.Count; i++)
            {
                int inX = levelData.FlickerPortalList[i].in_x;
                int inY = levelData.FlickerPortalList[i].in_y;

                int outX = levelData.FlickerPortalList[i].out_x;
                int outY = levelData.FlickerPortalList[i].out_y;
                M3Grid gridIn;
                M3Grid gridOut;
                if (M3GameManager.Instance.CheckValid(inX, inY))
                {
                    gridIn = gridCells[inX, inY];
                    gridIn.gridInfo.passType = 1;
                    gridIn.gridInfo.flickerPortal = levelData.FlickerPortalList[i];
                    if (gridIn.gridView != null)
                        gridIn.gridView.CreatePortal(true);
                }
                if (M3GameManager.Instance.CheckValid(outX, outY))
                {
                    gridOut = gridCells[outX, outY];
                    gridOut.gridInfo.passType = 2;
                    gridOut.gridInfo.flickerPortal = levelData.FlickerPortalList[i];
                    if (gridOut.gridView != null)
                        gridOut.gridView.CreatePortal(false);
                }
            }
        }

        /// <summary>
        /// 初始化掉落口
        /// </summary>
        /// <param name="data"></param>
        public void InitPort(M3LevelData data)
        {
            M3Grid grid;
            var list = data.portTilesList;
            for (int i = 0; i < list.Count; i++)
            {
                grid = gridCells[list[i].pos.x, list[i].pos.y];
                if (grid != null)
                {
                    grid.gridInfo.spawnPointType = DropPointType.SpawnPoint;
                    grid.gridInfo.spawnRule = data.GetRule(list[i].rule);
                }
            }
        }


        private void GenMask()
        {
            int num = 2;
            GameObject maskPrefab = null;
            KAssetManager.Instance.TryGetMatchPrefab("MaskObj", out maskPrefab);
            if (maskPrefab == null)
            {
                Debug.Log("MaksNull");
                return;
            }
            for (int i = 0; i < M3Config.GridWidth; i++)
            {
                for (int j = 1; j < 3; j++)
                {
                    CreateMask(maskPrefab, new Vector3(i, j));
                    CreateMask(maskPrefab, new Vector3(i, -j - M3Config.GridHeight + 1));
                }

            }
            for (int i = 0; i < M3Config.GridHeight; i++)
            {
                for (int j = 1; j < 3; j++)
                {
                    CreateMask(maskPrefab, new Vector3(-j, -i));
                    CreateMask(maskPrefab, new Vector3(j + M3Config.GridWidth - 1, -i));
                }
            }
        }

        private void CreateMask(GameObject obj, Vector3 localPos)
        {
            var go = GameObject.Instantiate(obj);
            go.transform.SetParent(maskObj.transform);
            go.transform.localScale = new Vector3(M3Config.MaskRateX, M3Config.MaskRateY, M3Config.MaskRateZ);
            go.transform.localPosition = localPos;
        }

        public void CreateGrid(M3CellData[,] map)
        {
            M3GameManager.Instance.cellNotEmpty = 0;
            gridCells = new M3Grid[M3Config.GridHeight, M3Config.GridWidth];
            for (int x = 0; x < M3Config.GridHeight; x++)
            {
                for (int y = 0; y < M3Config.GridWidth; y++)
                {
                    if (map[x, y].isActive)
                    {
                        M3GameManager.Instance.cellNotEmpty++;
                        InstantiateCell(x, y, map[x, y]);
                    }
                }
            }
            for (int i = 0; i < M3Config.GridHeight; i++)
            {
                if (!isLineEmpty(i, true))
                {
                    M3Config.RealFirstRow = i;
                    break;
                }
            }
            for (int i = M3Config.GridHeight - 1; i >= 0; i--)
            {
                if (!isLineEmpty(i, true))
                {
                    M3Config.RealLastRow = i;
                    break;

                }
            }
            for (int i = 0; i < M3Config.GridWidth; i++)
            {
                if (!isLineEmpty(i, false))
                {
                    M3Config.RealFirsCol = i;
                    break;

                }
            }
            for (int i = M3Config.GridWidth - 1; i >= 0; i--)
            {
                if (!isLineEmpty(i, false))
                {
                    M3Config.RealLastCol = i;
                    break;

                }
            }
            M3Config.RealWidth = M3Config.RealLastCol - M3Config.RealFirsCol + 1;
            M3Config.RealHeight = M3Config.RealLastRow - M3Config.RealFirstRow + 1;
        }

        private bool isLineEmpty(int num, bool isRow)
        {
            if (isRow)
            {
                int count = 0;
                for (int i = 0; i < M3Config.GridWidth; i++)
                {
                    if (gridCells[num, i] != null)
                    {
                        count++;
                    }
                }
                if (count == 0)
                    return true;
            }
            else
            {
                int count = 0;
                for (int i = 0; i < M3Config.GridHeight; i++)
                {
                    if (gridCells[i, num] != null)
                    {
                        count++;
                    }
                }
                if (count == 0)
                    return true;
            }
            return false;
        }

        public bool IsAllElementStopTweening()
        {

            for (int i = 0; i < M3Config.GridHeight; i++)
            {
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    M3Item item = M3ItemManager.Instance.gridItems[i, j];

                    if (item != null && ((item.IsDroping || item.dropSpeed > 0) || item.isTweening))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool IsAllElementLanded()
        {
            for (int i = 0; i < M3Config.GridHeight; i++)
            {
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    M3Item item = M3ItemManager.Instance.gridItems[i, j];
                    M3Grid grid = M3GridManager.Instance.gridCells[i, j];
                    if (item != null && (!item.isLanded || item.isCrushing || (grid != null && grid.isCrushing)))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        #region Border

        private void CreateBorder(M3CellData[,] map)
        {
            for (int x = 0; x < M3Config.GridHeight; x++)
            {
                for (int y = 0; y < M3Config.GridWidth; y++)
                {
                    if (map[x, y].isActive)
                    {
                        InstantiateBorder(x, y);
                    }
                }
            }
        }

        public void InstantiateBorder(int x, int y)
        {
            M3Direction dir = 0;

            if (IsTop(x, y))
            {
                dir |= M3Direction.Top;
            }
            if (IsBottom(x, y))
            {
                dir |= M3Direction.Bottom;
            }
            if (IsLeft(x, y))
            {
                dir |= M3Direction.Left;
            }
            if (IsRight(x, y))
            {
                dir |= M3Direction.Rigth;
            }

            Vector3 tmpPos = new Vector3(y, -x, 2);
            GameObject tmpObj = null;
            switch (dir)
            {
                case M3Direction.Left:
                    tmpObj = GameObject.Instantiate(borderPrefabs[1]);
                    tmpObj.transform.SetParent(borderParent.transform, false);
                    tmpObj.transform.localPosition = tmpPos;
                    tmpObj.transform.localEulerAngles = new Vector3(0f, 0f, -90f);
                    break;
                case M3Direction.Rigth:
                    tmpObj = GameObject.Instantiate(borderPrefabs[1]);
                    tmpObj.transform.SetParent(borderParent.transform, false);
                    tmpObj.transform.localPosition = tmpPos;
                    tmpObj.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
                    break;
                case M3Direction.Top:
                    tmpObj = GameObject.Instantiate(borderPrefabs[1]);
                    tmpObj.transform.SetParent(borderParent.transform, false);
                    tmpObj.transform.localPosition = tmpPos;
                    tmpObj.transform.localEulerAngles = new Vector3(0f, 0f, 180f);
                    break;
                case M3Direction.Bottom:
                    tmpObj = GameObject.Instantiate(borderPrefabs[1]);
                    tmpObj.transform.SetParent(borderParent.transform, false);
                    tmpObj.transform.localPosition = tmpPos;
                    //tmpObj.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                    break;
                case M3Direction.Left | M3Direction.Top:
                    tmpObj = GameObject.Instantiate(borderPrefabs[2]);
                    tmpObj.transform.SetParent(borderParent.transform, false);
                    tmpObj.transform.localPosition = tmpPos;
                    tmpObj.transform.localEulerAngles = new Vector3(0f, 0f, 180f);
                    break;
                case M3Direction.Rigth | M3Direction.Top:
                    tmpObj = GameObject.Instantiate(borderPrefabs[2]);
                    tmpObj.transform.SetParent(borderParent.transform, false);
                    tmpObj.transform.localPosition = tmpPos;
                    tmpObj.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
                    break;
                case M3Direction.Rigth | M3Direction.Bottom:
                    tmpObj = GameObject.Instantiate(borderPrefabs[2]);
                    tmpObj.transform.SetParent(borderParent.transform, false);
                    tmpObj.transform.localPosition = tmpPos;
                    //tmpObj.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                    break;
                case M3Direction.Left | M3Direction.Bottom:
                    tmpObj = GameObject.Instantiate(borderPrefabs[2]);
                    tmpObj.transform.SetParent(borderParent.transform, false);
                    tmpObj.transform.localPosition = tmpPos;
                    tmpObj.transform.localEulerAngles = new Vector3(0f, 0f, -90f);
                    break;
                case M3Direction.Left | M3Direction.Rigth:
                    tmpObj = GameObject.Instantiate(borderPrefabs[5]);
                    tmpObj.transform.SetParent(borderParent.transform, false);
                    tmpObj.transform.localPosition = tmpPos;
                    tmpObj.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
                    break;
                case M3Direction.Top | M3Direction.Bottom:
                    tmpObj = GameObject.Instantiate(borderPrefabs[5]);
                    tmpObj.transform.SetParent(borderParent.transform, false);
                    tmpObj.transform.localPosition = tmpPos;
                    tmpObj.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                    break;
                case M3Direction.Left | M3Direction.Top | M3Direction.Bottom:
                    tmpObj = GameObject.Instantiate(borderPrefabs[3]);
                    tmpObj.transform.SetParent(borderParent.transform, false);
                    tmpObj.transform.localPosition = tmpPos;
                    tmpObj.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                    break;
                case M3Direction.Rigth | M3Direction.Top | M3Direction.Bottom:
                    tmpObj = GameObject.Instantiate(borderPrefabs[3]);
                    tmpObj.transform.SetParent(borderParent.transform, false);
                    tmpObj.transform.localPosition = tmpPos;
                    tmpObj.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
                    break;
                case M3Direction.Left | M3Direction.Top | M3Direction.Rigth:
                    tmpObj = GameObject.Instantiate(borderPrefabs[3]);
                    tmpObj.transform.SetParent(borderParent.transform, false);
                    tmpObj.transform.localPosition = tmpPos;
                    tmpObj.transform.localEulerAngles = new Vector3(0f, 0f, -90f);
                    break;
                case M3Direction.Left | M3Direction.Bottom | M3Direction.Rigth:
                    tmpObj = GameObject.Instantiate(borderPrefabs[3]);
                    tmpObj.transform.SetParent(borderParent.transform, false);
                    tmpObj.transform.localPosition = tmpPos;
                    tmpObj.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
                    break;
                case M3Direction.Left | M3Direction.Bottom | M3Direction.Rigth | M3Direction.Top:
                    tmpObj = GameObject.Instantiate(borderPrefabs[4]);
                    tmpObj.transform.SetParent(borderParent.transform, false);
                    tmpObj.transform.localPosition = tmpPos;
                    tmpObj.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
                    break;
                default:
                    tmpObj = GameObject.Instantiate(borderPrefabs[0]);
                    tmpObj.transform.SetParent(borderParent.transform, false);
                    tmpObj.transform.localPosition = new Vector3(y, -x, 2);
                    break;
            }
            //float per = 115.0f / 116.0f;
            //tmpObj.transform.localScale = new Vector3(per, per, per);
        }

        private bool IsLeft(int x, int y)
        {
            if (y <= 0)
            {
                return true;
            }
            else if (!map[x, y - 1].isActive)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsRight(int x, int y)
        {
            if (y >= M3Config.GridWidthIndex)
            {
                return true;
            }
            else if (!map[x, y + 1].isActive)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsBottom(int x, int y)
        {
            if (x >= M3Config.GridHeightIndex)
            {
                return true;
            }
            else if (!map[x + 1, y].isActive)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsTop(int x, int y)
        {
            if (x <= 0)
            {
                return true;
            }
            else if (!map[x - 1, y].isActive)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ReloadAllGrid(List<Element>[,] grids)
        {
            for (int i = 0; i < M3Config.GridHeight; i++)
            {
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    if (gridCells[i, j] != null)
                        gridCells[i, j].GridDestroy();
                }
            }

            ReloadMap(grids);
        }

        private void ReloadMap(List<Element>[,] map)
        {
            var items = new M3Item[M3Config.GridHeight, M3Config.GridWidth];
            for (int x = 0; x < M3Config.GridHeight; x++)
            {
                for (int y = 0; y < M3Config.GridWidth; y++)
                {
                    if (map[x, y] != null && gridCells[x, y] != null)
                    {
                        for (int i = 0; i < map[x, y].Count; i++)
                        {
                            gridCells[x, y].gridInfo.AddElement(map[x, y][i]);
                        }
                    }
                }
            }
        }

        #endregion
    }
}