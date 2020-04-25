using Game.DataModel;
using UnityEngine;

namespace Game.Match3
{
    public class M3AnimationConst
    {
        public const string SHAKEHEAD = "ShakeHead";
    }

    public class M3Item
    {
        #region Field

        private Collider2D _collider2D;
        //private bool _checked;
        private bool isDroping;

        //public bool touchLock;
        public bool isCrushing = false;
        public int crushScore;
        public int specialScore;
        public float dropSpeed = 0;
        public bool isTweening = false;
        public float targetPosX;
        public float targetPosY;
        /// <summary>
        /// 是否已落地 下落完成
        /// </summary>
        public bool isLanded = true;
        public float disappearPosX = -999;
        public float disappearPosY;
        public Vector3 shownPos;
        public bool isLandToCrush = false;
        public bool isTargetByCattery = false;
        public bool isTargetByJump = false;
 
        #endregion

        /// <summary>
        /// 是否可以下落
        /// </summary>
        public bool CanDrop
        {
            get { return itemInfo.CanDrop; }
        }

        /// <summary>
        /// 是否可以选中移动
        /// </summary>
        public bool CanSelect
        {
            get { return CheckSelectable(); }
        }

        private bool CheckSelectable()
        {
            return itemInfo.GetElement() != null && itemInfo.GetElement().data.config.CanMove;
        }

        /// <summary>
        /// 设置选中状态 播放动画
        /// </summary>
        public bool Selected
        {
            set
            {
                //if (itemInfo.CheckDestroy()) return;
                if (itemInfo.GetElement() == null) return;

                if (value)
                {
                    itemInfo.GetElement().view.PlayAnimation(itemInfo.GetElement().data.config.SelectAnim, true);
                }
                else
                {
                    itemInfo.GetElement().view.PlayAnimation(itemInfo.GetElement().data.config.IdleAnim, true);
                }
            }
        }

        /// <summary>
        /// 是否不需要参与消除计算
        /// </summary>
        /// <returns></returns>
        public bool IsNoNeedToCompute()
        {
            var ele = itemInfo.GetElement();
            if (ele == null || ele.eName == M3ElementType.CatteryElement) return true;
            return (itemInfo.GetPartakeEliminateElement() != null && (itemInfo.GetPartakeEliminateElement().GetColor() == 0)) || isCrushing;
        }

        /// <summary>
        /// 落地处理 已经下落完成
        /// </summary>
        public void OnLanded()
        {
            var elementList = itemInfo.allElementList;
            for (int i = 0; i < elementList.Count; i++)
            {
                elementList[i].OnLandedAnimation();
            }

            dropSpeed = 0;
            disappearPosX = -999;
            IsDroping = false;
            isLanded = true;
            if (isLandToCrush)
            {
                OnSpecialCrush(ItemSpecial.fNormal);
                isLandToCrush = false;
            }
            else
            {
                if (itemInfo.GetElement() != null)
                {
                    itemInfo.GetElement().OnLanded();
                }
                //if (M3GameManager.Instance.isAutoAI)
                //Debug.Log(itemInfo.posX + " _ " + itemInfo.posY + " landed  " + FrameScheduler.instance.GetCurrentFrame());
                RuleChecker();
            }
            if (M3GameManager.Instance.soundManager != null)
                M3GameManager.Instance.soundManager.PlayerElementLandedAudio();
        }

        public void OnArriveGrid()
        {
            if (itemInfo.GetElement() != null)
            {
                itemInfo.GetElement().OnArriveGrid();
            }
        }

        public M3Grid GetGrid()
        {
            return M3GridManager.Instance.gridCells[itemInfo.posX, itemInfo.posY];
        }

        public void SetPosition(float targetPosX, float targetPosY)
        {
            Vector3 local = itemView.itemTransform.localPosition;
            //itemView.itemTransform.localPosition = new Vector3(targetPosX, targetPosY, local.z);
            KTweenUtils.LocalMoveTo(itemView.itemTransform, new Vector3(targetPosX, targetPosY, local.z), 0);
        }

        /// <summary>
        /// 是否是障碍物类型元素
        /// </summary>
        /// <returns></returns>
        public bool IsObstacle()
        {
            return itemInfo.GetElement() != null && itemInfo.GetElement().isObstacle;
        }

        #region Property

        public M3ItemInfo itemInfo { get; private set; }
        public M3ItemView itemView { get; private set; }

        //public bool active
        //{
        //    set { _collider2D.enabled = value; }
        //}

        /// <summary>
        /// 坐标换算出来的位置
        /// </summary>
        public Vector3 position
        {
            get { return new Vector3(itemInfo.posY * M3Config.DistancePerUnit, -itemInfo.posX * M3Config.DistancePerUnit); }
        }

        /// <summary>
        /// 坐标
        /// </summary>
        public Int2 coordinate
        {
            get { return new Int2(itemInfo.posY, -itemInfo.posX); }
            set
            {
                if (itemView != null && itemView.itemTransform != null)
                    itemView.itemTransform.localPosition = new Vector3(value.y * M3Config.DistancePerUnit, -value.x * M3Config.DistancePerUnit);
            }
        }

        /// <summary>
        /// 设置消除标记，被标记为消除了则不参与草坪铺设，当阻挡处理
        /// </summary>
        public bool ElementCrushFlag
        {
            get
            {
                bool b = false;
                var ele = itemInfo.GetElement();
                if (ele != null)
                {
                    b = ele.crushFlag;
                }
                return b;
            }
            set
            {
                var ele = itemInfo.GetElement();
                if (ele != null)
                {
                    ele.crushFlag = value;
                }
            }
        }

        /// <summary>
        /// 是否下落中
        /// </summary>
        public bool IsDroping
        {
            get { return isDroping; }
            set
            {
                isDroping = value;
                if (_collider2D != null)
                    _collider2D.enabled = !value;
            }
        }

        #endregion

        #region Method

        public void InitView(GameObject go)
        {
            _collider2D = go.GetComponent<Collider2D>();
            itemView = go.GetComponent<M3ItemView>();
            itemView.Init(go, this);
        }

        public void InitData()
        {
            itemInfo = new M3ItemInfo();
        }

        /// <summary>
        /// bomb
        /// </summary>
        /// <param name="special"></param>
        public void ProcessSpecial(ElementSpecial special, ItemColor color)
        {
            Vector3 pos = M3Supporter.Instance.GetItemPositionByGrid(itemInfo.posX, itemInfo.posY);
            switch (special)
            {
                case ElementSpecial.Row:
                    //M3FxManager.Instance.FireArrow(pos, false);
                    EliminateManager.Instance.ProcessEliminate(ItemSpecial.fRow, itemInfo.posX, itemInfo.posY, color);
                    break;
                case ElementSpecial.Column:
                    //M3FxManager.Instance.FireArrow(pos, true);
                    EliminateManager.Instance.ProcessEliminate(ItemSpecial.fColumn, itemInfo.posX, itemInfo.posY, color);
                    break;
                case ElementSpecial.Area:
                    EliminateManager.Instance.ProcessEliminate(ItemSpecial.fArea, itemInfo.posX, itemInfo.posY, color);
                    break;
                case ElementSpecial.Color:
                    EliminateManager.Instance.ProcessColorEliminate(ItemSpecial.fColor, itemInfo.posX, itemInfo.posY, -1, -1, ItemColor.fRandom);
                    break;
            }
        }

        /// <summary>
        /// 改变所在格子坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void ChangeItemPos(int x, int y)
        {
            if (x >= 0 && x < M3Config.GridHeight && y >= 0 && y < M3Config.GridWidth)
            {
                itemInfo.RefreshPos(x, y);
            }
        }

        public void OnColorEliminateCrush()
        {
            var ele = itemInfo.GetElement();
            if (ele != null)
            {
                ele.ProcessColorCrush();
            }
        }

        public Element AddElement(int id)
        {
            var config = XTable.ElementXTable.GetByID(id);
            Element e = null;
            switch (config.Type)
            {
                case M3ElementType.NormalElement:
                    e = new NormalElement();
                    break;
                case M3ElementType.SpecialElement:
                    e = new SpecialElement();
                    break;
                case M3ElementType.MagicBookElement:
                    e = new BookElement();
                    break;
                case M3ElementType.MagicCatElement:
                    e = new MagicCatElement();
                    break;
                case M3ElementType.GiftElement:
                    e = new GiftElement();
                    break;
                case M3ElementType.GreyCoomElement:
                    e = new GreyCoom();
                    break;
                case M3ElementType.VenomElement:
                    e = new VenomElement();
                    break;
                default:
                    e = new Element();
                    break;
            }
            e.Init(id, this);
            e.OnCreate();
            itemInfo.AddElement(e);
            return e;
        }

        /// <summary>
        /// 特殊消除 最高层元素
        /// </summary>
        /// <param name="special"></param>
        /// <param name="args"></param>
        /// <param name="isEffect"></param>
        public void OnSpecialCrush(ItemSpecial special, object[] args = null, bool isEffect = false)
        {
            var ele = itemInfo.GetElement();
            if (ele != null)
            {
                ele.ProcessSpecialEliminate(special, args, isEffect);
            }
        }

        public bool CheckValid(int x, int y)
        {
            return x >= 0 && x < M3Config.GridHeight && y >= 0 && y < M3Config.GridWidth && (M3ItemManager.Instance.gridItems[x, y] != null);
        }

        //public int FindSelfMatchCount()
        //{
        //    return FindAllMatch(this.itemInfo.posX, this.itemInfo.posY, this.itemInfo.GetElement().data.GetColor()).Count;
        //}

        ///// <summary>
        ///// 获取四方向颜色匹配的元素
        ///// </summary>
        ///// <param name="x"></param>
        ///// <param name="y"></param>
        ///// <param name="color"></param>
        ///// <returns></returns>
        //public List<M3Item> FindAllMatch(int x, int y, ItemColor color)
        //{
        //    var retList = new List<M3Item>();
        //    if (color == ItemColor.fNone)
        //        return retList;

        //    var items = M3ItemManager.Instance.gridItems;
        //    var cells = M3GridManager.Instance.gridCells;

        //    for (int ly = y - 1; ly >= 0; ly--)
        //    {
        //        if (ly != itemInfo.posY &&
        //            items[x, ly] != null &&
        //            items[x, ly].itemInfo.GetPartakeEliminateElement().data.GetColor() == color
        //            && !items[x, ly].isCrushing
        //            )
        //        {
        //            retList.Add(items[x, ly]);
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }
        //    for (int ry = y + 1; ry < M3Config.GridWidth; ry++)
        //    {
        //        if (ry != itemInfo.posY &&
        //            items[x, ry] != null &&
        //            items[x, ry].itemInfo.GetPartakeEliminateElement().data.GetColor() == color
        //                 && !items[x, ry].isCrushing)
        //        {
        //            retList.Add(items[x, ry]);
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }

        //    if (retList.Count < 2)
        //    {
        //        retList.Clear();
        //    }

        //    var colList = new List<M3Item>(4);
        //    for (int tx = x + 1; tx < M3Config.GridHeight; tx++)
        //    {
        //        if (tx != itemInfo.posX &&
        //            items[tx, y] != null &&
        //            items[tx, y].itemInfo.GetPartakeEliminateElement().data.GetColor() == color
        //            && !items[tx, y].isCrushing)
        //        {
        //            colList.Add(items[tx, y]);
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }
        //    for (int bx = x - 1; bx >= 0; bx--)
        //    {
        //        if (bx != itemInfo.posX &&
        //            items[bx, y] != null &&
        //            items[bx, y].itemInfo.GetPartakeEliminateElement().data.GetColor() == color
        //            && !items[bx, y].isCrushing)
        //        {
        //            colList.Add(items[bx, y]);
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }

        //    retList.Insert(0, this);
        //    if (colList.Count >= 2)
        //    {
        //        retList.AddRange(colList);
        //    }

        //    if (retList.Count < 3)
        //    {
        //        retList.Clear();
        //    }

        //    return retList;
        //}

        //public int FindAllMatchCount(int x, int y, ItemColor color)
        //{
        //    return FindAllMatch(x, y, color).Count;
        //}

        //private int GetMatchCount()
        //{
        //    return FindAllMatchCount(itemInfo.posX, itemInfo.posY, itemInfo.GetElement().data.GetColor());
        //}

        //private List<M3Item> GetMatchList()
        //{
        //    return FindAllMatch(itemInfo.posX, itemInfo.posY, itemInfo.GetElement().data.GetColor());
        //}

        public void RuleChecker()
        {
            M3GameManager.Instance.matcher.GetSinglePieceResult(this);
            if (M3GameManager.Instance.CheckNeedToCrush())
            {
                M3GameManager.Instance.GoCrush();
            }
        }

        /// <summary>
        /// 随机生成一个摇头或点头元素，删除原普通元素
        /// </summary>
        public void MakeBaseElementSpecial()
        {
            Element ele = itemInfo.GetElement();
            if (ele != null && ele.data.IsBaseElement())
            {
                int tmpType = M3Supporter.Instance.GetRandomInt(0, 2);
                if (tmpType == 0)
                {
                    int id = M3ItemManager.Instance.GetSpecialElementID(itemInfo.GetElement().GetColor(), ElementSpecial.Row);
                    M3ItemManager.Instance.ChangeElement(this, id);
                }
                else
                {
                    int id = M3ItemManager.Instance.GetSpecialElementID(itemInfo.GetElement().GetColor(), ElementSpecial.Column);
                    M3ItemManager.Instance.ChangeElement(this, id);
                }
            }
        }

        /// <summary>
        /// 从管理器列表中移除
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void RemoveFrom(int x, int y)
        {
            M3ItemManager.Instance.gridItems[x, y] = null;
        }

        //public void SpecialBoomAffect()
        //{
        //}

        //public void ItemBounce()
        //{
        //}

        //public void ItemEnable()
        //{
        //}

        //public void ItemDisable()
        //{
        //}

        //public void ItemSuggestion()
        //{
        //}

        //public void ItemStopSuggestion()
        //{
        //}

        //public void AnimationBack(M3Item item)
        //{
        //    if (!M3Supporter.Instance.isNothingMove)
        //    {
        //        _itemView.AnimationBack(new Vector3(item.itemInfo.posY - itemInfo.posY, -item.itemInfo.posX + itemInfo.posX));
        //    }
        //}

        /// <summary>
        /// 直接消毁
        /// </summary>
        public void ItemDestroy()
        {
            foreach (var item in itemInfo.allElementList)
            {
                item.DestroyElement();
            }
            if (itemView != null)
                KPool.Despawn(this.itemView.itemGameobject);
        }

        public void RemoveAllElement()
        {
            foreach (var item in itemInfo.allElementList)
            {
                item.DestroyElement();
            }
            itemInfo.allElementList = null;
        }

        #endregion

    }
}