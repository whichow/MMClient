using UnityEngine;

namespace Game.Match3
{
    public class M3Grid
    {
        #region Field

        //private SpriteRenderer _bgSpriteRenderer;
        //private SpriteRenderer _spriteRenderer;
        //private bool needDrop;
        //private int dropCheckCount;
        public float portDropSpeed;
        public bool isFishExit = false;
        public bool isFishPort = false;

        private Int2[] checkDropPos = new Int2[3];
        private bool[] bottomBlank = new bool[3];
        private bool isConveyorStraight;
        private Conveyor conveyorPiece;

        //public bool needUpdate;
        public bool isCrushing = false;

        #endregion

        #region Property

        public M3GridInfo gridInfo { get; private set; }

        public M3GridView gridView { get; private set; }

        public bool isDroping { get; set; }

        #endregion

        #region Method

        public M3Item GetItem()
        {
            return M3ItemManager.Instance.gridItems[gridInfo.posX, gridInfo.posY];
        }

        public bool CanSwap(M3Direction direction)
        {
            var ropeDir = (M3Direction)gridInfo.ropeType;
            return (ropeDir & direction) == 0;
        }

        #endregion

        #region 爆炸

        /// <summary>
        /// 爆炸
        /// </summary>
        public void Boom()
        {
            FloorElementProcess();
        }

        private void FloorElementProcess()
        {
            var ele = gridInfo.floorElement;
            if (ele != null)
            {
                ele.ProcessSpecialEliminate(ItemSpecial.fNormal, null);
            }
        }

        /// <summary>
        /// 特效爆炸
        /// </summary>
        public void SpecialBoom()
        {
            FloorElementProcess();
        }

        /// <summary>
        /// 爆炸影响（如周围积雪会被消除一层，周围能量块会被收集）
        /// </summary>
        public void BoomAffect()
        {

        }

        /// <summary>
        /// 特效爆炸影响
        /// </summary>
        public void SpecialAffect()
        {

        }

        #endregion

        #region 传送带

        public void MoveConveyor()
        {
            conveyorPiece.PlayAnimation();
        }

        GameObject CreateConveyor(int x, int y, bool isStraight)
        {
            GameObject tmp = null;
            KAssetManager.Instance.TryGetMatchPrefab("converyor", out tmp);
            GameObject go = GameObject.Instantiate(tmp);
            go.transform.SetParent(M3GridManager.Instance.gridCells[x, y].gridView.transform, false);
            go.transform.localPosition = new Vector3(0, 0, 0.7f);
            go.transform.localScale = Vector3.one;
            go.transform.localEulerAngles = Vector3.zero;
            Game.TransformUtils.SetLayer(go, LayerMask.NameToLayer("Default"));
            return go;
        }

        public void CreateConveyor(float zRotation, bool isStraight, bool isFlip)
        {
            isConveyorStraight = isStraight;
            this.conveyorPiece = CreateConveyor(gridInfo.posX, gridInfo.posY, isStraight).AddComponent<Conveyor>();
            conveyorPiece.Create(gridInfo.posX, gridInfo.posY, isStraight);
            this.conveyorPiece.transform.Rotate(0f, 0f, zRotation);

            if (!isStraight)
                this.conveyorPiece.transform.Rotate(0f, 0, 90);
            if (isFlip)
            {
                this.conveyorPiece.transform.localScale = new Vector3(-1f, 1f, 1f);
                this.conveyorPiece.transform.Rotate(0f, 0, -180);
            }
            //if (!isStraight)
            //{
            //    this.conveyorPiece.transform.Rotate(0f, 0, 90);
            //}
            //else
            //{
            //    //this.conveyorPiece.transform.Rotate(0f, 0f, 180f);
            //}
        }

        #endregion

        #region 下落

        /// <summary>
        /// 底下左中右三格是否可以下落
        /// </summary>
        public void InitBlankInfo()
        {
            //this.dropCheckCount = 3;
            for (int i = 0; i < 3; i++)
            {
                int x = gridInfo.posX + 1;
                int y = gridInfo.posY - M3Config.gridBottomBlankOffset[i];
                this.bottomBlank[i] = CheckValid(x, y) && M3GridManager.Instance.gridCells[x, y].gridInfo.spawnPointType != DropPointType.SpawnPoint;
                this.checkDropPos[i] = new Int2(x, y);
            }
        }

        /// <summary>
        /// 传送门
        /// </summary>
        public void CreateFlickerPortal()
        {
            if (gridInfo.passType == 1)
            {
                this.bottomBlank[0] = false;
                this.bottomBlank[1] = true;
                this.bottomBlank[2] = false;
                Int2 tmp = gridInfo.GetFlickerPortalOut();
                this.checkDropPos[1].x = tmp.x;
                this.checkDropPos[1].y = tmp.y;
            }
        }

        private bool CheckNeedDrop()
        {
            if (M3ItemManager.Instance.gridItems[gridInfo.posX, gridInfo.posY] == null)
                return true;
            return false;
        }

        /// <summary>
        /// 检测格子是否为空
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool CheckGridIsBlank(int x, int y)
        {
            return M3GridManager.Instance.gridCells[x, y].GetItem() == null && !M3GridManager.Instance.gridCells[x, y].isDroping;
        }

        /// <summary>
        /// 检测下落
        /// </summary>
        /// <param name="check"></param>
        /// <param name="type">1为三个全部检查，2为只检查垂直</param>
        /// <returns></returns>
        public bool CheckDrop(bool check = false, int type = 1)
        {
            bool flag = false;
            int dropCheckCount = 3;
            M3Grid grid;
            for (int i = 0; i < dropCheckCount; i++)
            {
                if (type == 2 && i != 0)
                    continue;
                int x = checkDropPos[i].x;
                int y = checkDropPos[i].y;
                if (bottomBlank[i] && CheckGridIsBlank(x, y))
                {
                    grid = M3GridManager.Instance.gridCells[x, y];
                    if (grid == null)
                        continue;

                    if ((gridInfo.passType != 1 || grid.gridInfo.passType != 2) && CheckHasRope(this, grid))
                        continue;

                    if (M3DropManager.DropItemTo(this, grid, gridInfo.passType == 1, check))
                    {
                        flag = true;
                        break;
                    }
                }
            }
            return flag;
            //bool flag = false;
            //int dropCheckCount = 3;
            //List<M3Grid> gridList = new List<M3Grid>();
            //for (int i = 0; i < dropCheckCount; i++)
            //{
            //    int x = checkDropPos[i].x;
            //    int y = checkDropPos[i].y;
            //    M3Grid grid;
            //    if (bottomBlank[i] && checkGridIsBlank(x, y))
            //    {
            //        grid = M3GridManager.Instance.gridCells[x, y];
            //        if (grid == null)
            //            continue;
            //        if (CheckHasRope(this, grid))
            //            continue;
            //        if (M3DropManager.DropItemTo(this, grid, gridInfo.passType == 1, true))
            //        {
            //            gridList.Add(grid);
            //        }
            //    }
            //}
            //if (gridList.Count > 0)
            //{
            //    var t = gridList[M3Supporter.Instance.GetRandomInt(0, gridList.Count)];
            //    M3DropManager.DropItemTo(this, t, gridInfo.passType == 1, check);
            //    flag = true;
            //}
            //return flag;
        }

        /// <summary>
        /// 检测是否有绳索
        /// </summary>
        /// <param name="grid1"></param>
        /// <param name="grid2"></param>
        /// <returns></returns>
        public bool CheckHasRope(M3Grid grid1, M3Grid grid2)
        {
            if (grid1.gridInfo.passType == 1 && grid2.gridInfo.passType == 2)
            {
                return M3GameManager.Instance.CheckRopeType(grid1, RopeTypeEnum.Bottom) || M3GameManager.Instance.CheckRopeType(grid2, RopeTypeEnum.Top);
            }

            if (grid1.gridInfo.posY == grid2.gridInfo.posY)
            {
                return M3GameManager.Instance.CheckRopeType(grid1, RopeTypeEnum.Bottom) || M3GameManager.Instance.CheckRopeType(grid2, RopeTypeEnum.Top);
            }
            else if (grid1.gridInfo.posY < grid2.gridInfo.posY)
            {
                return (((M3GameManager.Instance.CheckRopeType(grid1, RopeTypeEnum.Rigth) || M3GameManager.Instance.CheckRopeType(grid1.gridInfo.posX, grid1.gridInfo.posY + 1, RopeTypeEnum.Left)) &&
                    (M3GameManager.Instance.CheckRopeType(grid2, RopeTypeEnum.Left) || M3GameManager.Instance.CheckRopeType(grid2.gridInfo.posX, grid2.gridInfo.posY - 1, RopeTypeEnum.Rigth)))
                    || ((M3GameManager.Instance.CheckRopeType(grid2, RopeTypeEnum.Top) || M3GameManager.Instance.CheckRopeType(grid2.gridInfo.posX - 1, grid2.gridInfo.posY, RopeTypeEnum.Bottom)) &&
                    (M3GameManager.Instance.CheckRopeType(grid2, RopeTypeEnum.Left) || M3GameManager.Instance.CheckRopeType(grid2.gridInfo.posX, grid2.gridInfo.posY - 1, RopeTypeEnum.Rigth)))
                    || ((M3GameManager.Instance.CheckRopeType(grid1, RopeTypeEnum.Rigth) || M3GameManager.Instance.CheckRopeType(grid1.gridInfo.posX, grid1.gridInfo.posY + 1, RopeTypeEnum.Left)) &&
                    (M3GameManager.Instance.CheckRopeType(grid1, RopeTypeEnum.Bottom) || M3GameManager.Instance.CheckRopeType(grid1.gridInfo.posX + 1, grid1.gridInfo.posY, RopeTypeEnum.Top)))
                   || ((M3GameManager.Instance.CheckRopeType(grid1, RopeTypeEnum.Bottom) || M3GameManager.Instance.CheckRopeType(grid1.gridInfo.posX + 1, grid1.gridInfo.posY, RopeTypeEnum.Top)) &&
                   (M3GameManager.Instance.CheckRopeType(grid2, RopeTypeEnum.Top) || M3GameManager.Instance.CheckRopeType(grid2.gridInfo.posX - 1, grid2.gridInfo.posY, RopeTypeEnum.Bottom)))
                    );
            }
            else
            {
                return (((M3GameManager.Instance.CheckRopeType(grid1, RopeTypeEnum.Left) || M3GameManager.Instance.CheckRopeType(grid1.gridInfo.posX, grid1.gridInfo.posY - 1, RopeTypeEnum.Rigth)) &&
                       (M3GameManager.Instance.CheckRopeType(grid2, RopeTypeEnum.Rigth) || M3GameManager.Instance.CheckRopeType(grid2.gridInfo.posX, grid2.gridInfo.posY + 1, RopeTypeEnum.Left)))
                    || ((M3GameManager.Instance.CheckRopeType(grid2, RopeTypeEnum.Top) || M3GameManager.Instance.CheckRopeType(grid2.gridInfo.posX - 1, grid2.gridInfo.posY, RopeTypeEnum.Bottom)) &&
                       (M3GameManager.Instance.CheckRopeType(grid2, RopeTypeEnum.Rigth) || M3GameManager.Instance.CheckRopeType(grid2.gridInfo.posX, grid2.gridInfo.posY + 1, RopeTypeEnum.Left)))
                    || ((M3GameManager.Instance.CheckRopeType(grid1, RopeTypeEnum.Left) || M3GameManager.Instance.CheckRopeType(grid1.gridInfo.posX, grid1.gridInfo.posY - 1, RopeTypeEnum.Rigth)) &&
                       (M3GameManager.Instance.CheckRopeType(grid1, RopeTypeEnum.Bottom) || M3GameManager.Instance.CheckRopeType(grid1.gridInfo.posX + 1, grid1.gridInfo.posY, RopeTypeEnum.Top)))
                    || ((M3GameManager.Instance.CheckRopeType(grid1, RopeTypeEnum.Bottom) || M3GameManager.Instance.CheckRopeType(grid1.gridInfo.posX + 1, grid1.gridInfo.posY, RopeTypeEnum.Top)) &&
                       (M3GameManager.Instance.CheckRopeType(grid2, RopeTypeEnum.Top) || M3GameManager.Instance.CheckRopeType(grid2.gridInfo.posX - 1, grid2.gridInfo.posY, RopeTypeEnum.Bottom)))
                    );
            }
        }

        /// <summary>
        /// 检测坐标是否有效
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool CheckValid(int x, int y)
        {
            return x >= 0 && x < M3Config.GridHeight && y >= 0 && y < M3Config.GridWidth && (M3GridManager.Instance.gridCells[x, y] != null);
        }

        /// <summary>
        /// 是否下落有绳索阻挡
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsDropObstacle(RopeTypeEnum type)
        {
            return M3GameManager.Instance.CheckRopeType(gridInfo.posX, gridInfo.posY, type);
        }

        public void GridDestroy()
        {
            if (gridInfo.floorElement != null)
            {
                gridInfo.floorElement.DestroyElement();
                gridInfo.floorElement = null;
            }
        }

        #endregion

        #region Init

        public void InitView(Transform tf)
        {
            //_spriteRenderer = tf.GetComponent<SpriteRenderer>();
            //_bgSpriteRenderer = tf.Find("BgSprite").GetComponent<SpriteRenderer>();
            gridView = tf.transform.GetComponentInChildren<M3GridView>();
            gridView.Init(this, tf);
        }

        public void Init(M3CellData data)
        {
            //needDrop = false;
            gridInfo = new M3GridInfo();
            gridInfo.Init(data.gridX, data.gridY, this);
        }

        #endregion

    }
}