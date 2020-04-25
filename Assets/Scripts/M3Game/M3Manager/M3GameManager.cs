using Game.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Match3
{
    //public enum GameState
    //{
    //    None,
    //    Prepare,
    //    Playing,
    //    Pause,
    //    Win,
    //    Lose
    //}

    public enum ExitGameType
    {
        None,
        NextLevel,
        Reload,
        Editor,
    }

    public class M3BackDate
    {
        public List<Element>[,] items;
        public List<Element>[,] grids;
        public Dictionary<int, int> target;
        public int score;
        public int step;
        public int currentStep;
        public float energy;
    }

    /// <summary>
    /// 坐标系是x向下，y向右 （坐标体系是反的，要特别注意）
    /// </summary>
    public class M3GameManager : MonoBehaviour
    {
        public bool isAutoAI = false;
        public M3BackDate lastBackData = new M3BackDate();
        public M3BackDate firstBackData = new M3BackDate();
        public M3Grid[,] lastGrid;
        public GameObject gameScreen;

        #region Static

        public static M3GameManager Instance;

        #endregion

        #region Field

        public bool isRoundComplete = false;
        public bool isPause = false;
        public bool isGoalSignFinish = false;
        public bool isGameoverNeedTutorial = false;
        public bool isGameOver = false;
        /// <summary>
        /// 
        /// </summary>
        public int cellNotEmpty;

        /// <summary>
        /// 关卡数据
        /// </summary>
        public M3LevelData level;

        /// <summary>
        /// 选中框
        /// </summary>
        public GameObject selector;
        //private GameObject lockArea;

        /// <summary>
        /// 选中第一个
        /// </summary>
        private M3Item _selectedItem;
        /// <summary>
        /// 选中第二个
        /// </summary>
        private M3Item _selectedItem2;

        private Int2[] lockItemsArr;
        private Action lockItemsAction;
        private bool _isTouchHold;

        public PropBase propItem;
        public OperationBase skillOperation;
        public bool propItemLock;
        public bool skillLock;
        public int runningSpecialCount;
        public bool isReady = false;

        #region manager

        public GameFSM gameFsm;

        public M3ComboManager comboManager;

        public M3SpecialHandler specialHandler;

        public M3BonusManager bonusManager;

        public M3Matcher matcher;

        public M3GameModeManager modeManager;

        public M3VenomManager venomManager;

        public M3GameUIManager gameUI;

        public M3PropManager propManager;

        public M3CatManager catManager;

        public M3SkillManager skillManager;

        public M3ConveyorManager conveyorManager;

        public M3FishManager fishManager;

        public M3HiddenManager hiddenManager;

        public M3SoundManager soundManager;

        #endregion

        #endregion

        #region Property

        public bool ScreenLock { get; set; }

        //public int curMoves { get; set; }

        public M3Item SelectedItem
        {
            get { return _selectedItem; }
        }

        public M3Item SelectedItem2
        {
            get { return _selectedItem2; }
        }

        #endregion

        #region Method

        /// <summary>
        /// 选择一个元素
        /// </summary>
        public void SelectItem()
        {
            if (ScreenLock)
                return;
            if (!propItemLock && !skillLock)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _isTouchHold = true;
                    if (_selectedItem == null)
                    {
                        _selectedItem = CheckTouchItem(Input.mousePosition);
                        if (_selectedItem != null && (!_selectedItem.CanSelect))
                        {
                            _selectedItem = null;
                        }
                    }
                }
                else if (_isTouchHold && Input.GetMouseButton(0))
                {
                    if (_selectedItem != null)
                    {
                        _selectedItem.Selected = true;
                        ActiveSelector(_selectedItem.itemView.itemTransform.position);

                        _selectedItem2 = CheckTouchItem(Input.mousePosition);
                        if (_selectedItem2 != null && (!_selectedItem2.CanSelect))
                            _selectedItem2 = null;
                        if (_selectedItem2 != null && _selectedItem2 != _selectedItem)
                        {
                            if (CheckSwap(_selectedItem, _selectedItem2))
                            {
                                CheckLogic(_selectedItem, _selectedItem2);
                                _selectedItem.Selected = false;
                                selector.SetActive(false);
                            }
                            else
                            {
                                _selectedItem.Selected = false;
                                _selectedItem = _selectedItem2;
                                _selectedItem2 = null;
                                ActiveSelector(_selectedItem.itemView.itemTransform.position);
                            }
                        }
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    _isTouchHold = false;
                }
            }
            else
            {
                if (_selectedItem != null || _selectedItem2 != null)
                {
                    _selectedItem = null;
                    _selectedItem2 = null;
                    selector.SetActive(false);
                }

                if (Input.GetMouseButtonDown(0))
                {
                    if (propItemLock)
                    {
                        if (propItem != null)
                        {
                            var touchItem = CheckTouchItem(Input.mousePosition);
                            if (touchItem != null)
                            {
                                propItem.OnItemClick(touchItem.itemInfo.posX, touchItem.itemInfo.posY);
                            }
                            else
                            {
                                propItem.OnCancelUse();
                            }
                        }
                    }
                    //if (skillLock)
                    //{
                    //    if (skillOperation != null)
                    //    {
                    //        var touchItem = CheckTouchItem(Input.mousePosition);
                    //        if (touchItem != null)
                    //        {
                    //            //skillOperation.OnSelectItem(touchItem.itemInfo.posX, touchItem.itemInfo.posY);
                    //        }
                    //        else
                    //        {
                    //            skillOperation.OnSkillCancel();
                    //        }
                    //    }
                    //}
                }
            }
        }

        /// <summary>
        /// 清除交换的元素
        /// </summary>
        public void ClearSwitchItem()
        {
            _selectedItem = null;
            _selectedItem2 = null;
        }

        /// <summary>
        /// 检测交换元素后的逻辑
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        public bool CheckLogic(M3Item item1, M3Item item2)
        {
            bool flag = false;
            _selectedItem = item1;
            _selectedItem2 = item2;

            SwapItemPosition(item1, item2);
            ItemMove(item1, item2);

            GetMatchResult();
            ScreenLock = true;
            venomManager.ResetVars();
            comboManager.ResetCombo();
            modeManager.ResetVars();
            catManager.ResetVars();

            Action action = delegate ()
            {
                if (CheckNeedToCrush())
                {
                    SwapItemPosition(item1, item2);
                    SetCurrentBackDate();
                    SwapItemPosition(item1, item2);
                    flag = true;
                    modeManager.GameModeCtrl.ProcessSteps(1);
                    conveyorManager.UnlockConveyor();

                    ((M3RoundLogicState)gameFsm.GetFSM().GetStateInstance(StateEnum.RoundLogic)).haveChecked = false;
                    ((M3CheckCoomState)gameFsm.GetFSM().GetStateInstance(StateEnum.CheckCoom)).haveChecked = false;
                    ((M3CheckBrownCoomState)gameFsm.GetFSM().GetStateInstance(StateEnum.CheckBrownCoom)).haveChecked = false;
                    ((M3CheckFishState)gameFsm.GetFSM().GetStateInstance(StateEnum.CheckFish)).haveChecked = false;
                    ((M3CheckVenomState)gameFsm.GetFSM().GetStateInstance(StateEnum.CheckVenom)).haveChecked = false;
                    ((M3CheckJumpState)gameFsm.GetFSM().GetStateInstance(StateEnum.CheckJump)).haveChecked = false;
                    ((M3CheckCatteryState)gameFsm.GetFSM().GetStateInstance(StateEnum.CheckCattery)).haveChecked = false;
                    ((M3CheckWoolBallState)gameFsm.GetFSM().GetStateInstance(StateEnum.CheckWoolBall)).haveChecked = false;
                    ((M3CrystalState)gameFsm.GetFSM().GetStateInstance(StateEnum.CheckCrystal)).haveChecked = false;
                    ((M3AddEnergyState)gameFsm.GetFSM().GetStateInstance(StateEnum.CheckEnergy)).haveChecked = false;

                    fishManager.Refresh();
                    gameFsm.GetFSM().ChangeGameState(StateEnum.CheckAndCrush);
                    if (lockItemsAction != null)
                    {
                        lockItemsAction();
                        lockItemsAction = null;
                    }
                }
                else
                {
                    //交换无效，还原两个块
                    SwapItemPosition(item1, item2);
                    ItemMove(item1, item2);
                    ClearSelectedItem();

                    //为了在交换无效的情况下再次显示提示走下流程
                    gameFsm.GetFSM().ChangeGameState(StateEnum.CheckAndCrush);
                }
            };
            if (isAutoAI)
                action();
            else
                KTweenUtils.ScaleTo(item1.itemView.itemTransform, Vector3.one, M3Config.ItemMoveTime, action);

            if (soundManager != null)
                soundManager.PlayExchangeElement();

            return flag;
        }

        public void GetMatchResult()
        {
            if (_selectedItem != null && _selectedItem2 != null)
            {
                specialHandler.GetSpecialType(_selectedItem, _selectedItem2);
            }
            else
            {
                specialHandler.ResetVars();
            }

            if (specialHandler.HaveSpecialInExchange())
            {
                matcher.GetEliminateList().Clear();
            }
            else
            {
                matcher.GetNewResulet(_selectedItem, _selectedItem2);
            }
        }

        /// <summary>
        /// 交换两个块的位置（显示对象，缓动效果）
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        public void ItemMove(M3Item item1, M3Item item2)
        {
            if (item1.itemView != null && item2.itemView != null)
            {
                item1.isTweening = true;
                item2.isTweening = true;
                Vector3 target1 = M3Supporter.Instance.GetItemPositionByGrid(item2.itemInfo.posX, item2.itemInfo.posY);
                Vector3 target2 = M3Supporter.Instance.GetItemPositionByGrid(item1.itemInfo.posX, item1.itemInfo.posY);
                KTweenUtils.LocalMoveTo(item1.itemView.itemTransform, target2, M3Config.ItemMoveTime, () => { item1.isTweening = false; });
                KTweenUtils.LocalMoveTo(item2.itemView.itemTransform, target1, M3Config.ItemMoveTime, () => { item2.isTweening = false; });
            }
        }

        /// <summary>
        /// 执行消除
        /// </summary>
        public void GoCrush()
        {
            if (specialHandler.HaveSpecialInExchange())
            {
                comboManager.AddCombo(1);
                specialHandler.GoSpecialCrush();
            }
            else
            {
                for (int i = 0; i < matcher.GetEliminateList().Count; i++)
                {
                    comboManager.AddCombo(1);
                    List<Vector2> list = matcher.GetEliminateList()[i];

                    //for (int k = 0; k < list.Count; k++)
                    //{
                    //    if (M3GameManager.Instance.isAutoAI)
                    //        Debug.Log("消除 : " + list[k].x + " | " + list[k].y + "   " + FrameScheduler.instance.GetCurrentFrame());
                    //}

                    //Debuger.Log("============");
                    //List<M3Item> normal = new List<M3Item>();
                    bool hasLawn = EliminateManager.Instance.CheckNormalEliminateHasLawn(list);

                    //foreach (var pos in list)
                    //{
                    //    M3ItemManager.Instance.gridItems[(int)pos.x, (int)pos.y].ElementCrushFlag = true;
                    //}

                    for (int k = 0; k < list.Count; k++)
                    {
                        M3Item item = M3ItemManager.Instance.gridItems[(int)list[k].x, (int)list[k].y];
                        if (item != null && item.itemInfo.GetElement() != null)
                        {
                            item.itemInfo.GetElement().needEffectNeighour = true;

                            //Debuger.Log(item.itemInfo.posX + "--" + item.itemInfo.posY);
                            // 设置草坪
                            if (hasLawn && EliminateManager.Instance.CanLayingLawn(item))
                            {
                                item.itemInfo.GetElement().needCreateLawn = hasLawn;
                            }
                            item.ElementCrushFlag = true;

                            //下面注释sn部分移上来的，逻辑是一样的，不需要多次循环了，normal也没有用到 by coamy 2019.4.9
                            item.OnSpecialCrush(ItemSpecial.fNormal);
                            //if (item.itemInfo.GetElement().eName != M3ElementType.SpecialElement)
                            //{
                            //    normal.Add(item);
                            //}
                            //--
                        }
                    }
                    //--sn--
                    //for (int j = 0; j < list.Count; j++)
                    //{
                    //    M3Item item = M3ItemManager.Instance.gridItems[(int)list[j].x, (int)list[j].y];
                    //    if (item != null && item.itemInfo.GetElement() != null && item.itemInfo.GetElement().eName == M3ElementType.SpecialElement)
                    //        item.OnSpecialCrush(ItemSpecial.fNormal);
                    //    else if (item != null && item.itemInfo.GetElement() != null)
                    //        normal.Add(item);
                    //}
                    //for (int j = 0; j < normal.Count; j++)
                    //{
                    //    normal[j].OnSpecialCrush(ItemSpecial.fNormal);
                    //}
                    //--sn--
                }
                matcher.GetEliminateList().Clear();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public bool CheckSwap(M3Item item1, M3Item item2)
        {
            int x1 = item1.itemInfo.posX;
            int y1 = item1.itemInfo.posY;
            int x2 = item2.itemInfo.posX;
            int y2 = item2.itemInfo.posY;
            //if (item1.empty || item2.empty)
            //    return false;
            if (Mathf.Abs(x1 - x2) + Mathf.Abs(y1 - y2) > 1)
            {
                return false;
            }
            return !CheckRopeBetweenItems(x1, y1, x2, y2);
        }

        //public void CrashCross(Vector2 vector2, string specialType, ItemColor itemColor)
        //{
        //    ItemSpecial special = ItemSpecial.fNormal;
        //    switch (specialType)
        //    {
        //        case "4V":
        //            special = ItemSpecial.fColumn;
        //            break;
        //        case "4H":
        //            special = ItemSpecial.fRow;
        //            break;
        //        case "5L":
        //            special = ItemSpecial.fArea;
        //            break;
        //        case "5T":
        //            special = ItemSpecial.fArea;
        //            break;
        //        case "5":
        //            special = ItemSpecial.fColor;
        //            break;
        //    }
        //    EliminateManager.Instance.ProcessEliminate(special, (int)vector2.x, (int)vector2.y, special == ItemSpecial.fColor ? ItemColor.fRandom : itemColor);
        //}

        /// <summary>
        /// 生成消除后产生的特殊元素（如炸弹）
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="type"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public M3Item CreateSpecialPiece(Vector2 pos, string type, ItemColor color)
        {
            var basePiece = CreateSpecialItem(pos, type, color);
            if (basePiece != null)
            {
                if (M3GameManager.Instance.modeManager.mode == GameModeEnum.StepMode)
                {
                    if (M3GameManager.Instance.modeManager.isGameEnd && !M3GameManager.Instance.isAutoAI)
                    {
                        if (basePiece.GetGrid().CheckDrop(true))
                            basePiece.isLandToCrush = true;
                        else
                            basePiece.OnSpecialCrush(ItemSpecial.fNormal);
                    }
                    if (basePiece != null && basePiece.itemInfo.GetElement() != null && basePiece.itemInfo.GetElement().GetSpecial() > 0 && M3GameManager.Instance.modeManager.IsLevelFinish()
                        && M3GameManager.Instance.bonusManager != null && M3GameManager.Instance.bonusManager.needPreBoom
                        && !M3GameManager.Instance.isAutoAI)
                    {
                        basePiece.OnSpecialCrush(ItemSpecial.fNormal);
                    }
                }
                basePiece.RuleChecker();
            }
            return basePiece;
        }

        /// <summary>
        /// 生成消除后产生的特殊元素（如炸弹）
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="type"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public M3Item CreateSpecialItem(Vector2 pos, string type, ItemColor color)
        {
            int _x = (int)pos.x;
            int _y = (int)pos.y;
            M3Item basePiece = null;
            ElementSpecial special = ElementSpecial.None;
            switch (type)
            {
                case "4V":
                    special = ElementSpecial.Column;
                    if (M3GameManager.Instance.soundManager != null)
                        M3GameManager.Instance.soundManager.PlayCreateLineSpecialAudio();
                    break;
                case "4H":
                    special = ElementSpecial.Row;
                    if (M3GameManager.Instance.soundManager != null)
                        M3GameManager.Instance.soundManager.PlayCreateLineSpecialAudio();
                    break;
                case "5":
                    special = ElementSpecial.Color;
                    if (M3GameManager.Instance.soundManager != null)
                        M3GameManager.Instance.soundManager.PlayCreateColorSpecialAudio();
                    break;
                case "5L":
                    special = ElementSpecial.Area;
                    if (M3GameManager.Instance.soundManager != null)
                        M3GameManager.Instance.soundManager.PlayCreateWrapSpecialAudio();
                    break;
                case "5T":
                    special = ElementSpecial.Area;
                    if (M3GameManager.Instance.soundManager != null)
                        M3GameManager.Instance.soundManager.PlayCreateWrapSpecialAudio();
                    break;
                case "6":
                    special = ElementSpecial.Color;
                    if (M3GameManager.Instance.soundManager != null)
                        M3GameManager.Instance.soundManager.PlayCreateColorSpecialAudio();
                    break;
            }
            basePiece = M3ItemManager.Instance.SpawnSpecialItem(color, special, _x, _y);
            return basePiece;
        }

        private M3Item CheckTouchItem(Vector3 mousePosition)
        {
            var worldPoint = Camera.main.ScreenToWorldPoint(mousePosition);

            var collider = Physics2D.OverlapPoint(new Vector2(worldPoint.x, worldPoint.y));
            if (collider)
            {
                var tmp = collider.GetComponent<M3ItemView>();
                if (tmp)
                {
                    M3Supporter.Instance.ResetPiece();
                    Int2 pos = new Int2(tmp.obtainer.itemInfo.posX, tmp.obtainer.itemInfo.posY);
                    if (lockItemsArr == null)
                        return tmp.obtainer;
                    if (lockItemsArr.Length == 0)
                        return null;
                    if (!Array.Exists(lockItemsArr, (p) => p.x + M3Config.RealFirstRow == pos.x && p.y + M3Config.RealFirsCol == pos.y))
                        return null;
                    else
                    {
                        return tmp.obtainer;
                    }
                }
            }
            return null;
        }

        public void LockM3Item(Int2[] items, Action action)
        {
            //int tmpX;
            //int tmpY;
            //M3Item item;
            //for (int i = 0; i < M3Config.GridHeight; i++)
            //{
            //    for (int j = 0; j < M3Config.GridWidth; j++)
            //    {
            //        if (CheckValid(i, j))
            //        {
            //            item = M3ItemManager.Instance.gridItems[i, j];
            //            if (item != null)
            //            {
            //                item.touchLock = true;

            //            }

            //        }
            //    }
            //}
            //for (int k = 0; k < items.Length; k++)
            //{
            //    Debug.Log(items[k].x + M3Config.RealFirstRow);
            //    Debug.Log(items[k].y + M3Config.RealFirsCol);
            //    if (CheckValid(items[k].x + M3Config.RealFirstRow, items[k].y + M3Config.RealFirsCol))
            //    {
            //        item = M3ItemManager.Instance.gridItems[items[k].x + M3Config.RealFirstRow, items[k].y + M3Config.RealFirsCol];
            //        if (item != null)
            //        {
            //            item.touchLock = false;

            //        }

            //    }
            //}
            lockItemsArr = items;
            lockItemsAction = action;
        }

        /// <summary>
        /// 交换两个块的位置（数据）
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        public void SwapItemPosition(M3Item item1, M3Item item2)
        {

            int item1X = item1.itemInfo.posX;
            int item1Y = item1.itemInfo.posY;
            int item2X = item2.itemInfo.posX;
            int item2Y = item2.itemInfo.posY;
            item1.ChangeItemPos(item2X, item2Y);
            item2.ChangeItemPos(item1X, item1Y);

            M3ItemManager.Instance.gridItems[item1X, item1Y] = item2;
            M3ItemManager.Instance.gridItems[item2X, item2Y] = item1;

        }

        public ItemColor GetCurrentGridRandomColors()
        {

            var colorList = GetCurrentGridColorPool();
            if (colorList.Count > 0)
            {

                ItemColor tmpColor;
                tmpColor = colorList[M3Supporter.Instance.GetRandomInt(0, colorList.Count)];
                return tmpColor;
            }
            else
            {
                return ItemColor.fNone;
            }

        }

        public List<ItemColor> GetCurrentGridColorPool()
        {
            List<ItemColor> colorList = new List<ItemColor>();
            for (int i = 0; i < M3Config.GridHeight; i++)
            {
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    M3Item item = M3ItemManager.Instance.gridItems[i, j];
                    if (item != null
                        && item.itemInfo.GetElement() != null && !item.isCrushing
                        && item.itemInfo.GetElement().data.IsBaseAndeSpecialElement()
                        )
                    {
                        ItemColor color = item.itemInfo.GetElement().GetColor();
                        if ((color != ItemColor.fNone && color != ItemColor.fRandom && color != ItemColor.fEnergy) && !colorList.Contains(color))
                        {
                            colorList.Add(color);
                        }
                    }
                }
            }
            return colorList;
        }
        
        public bool CheckGreyCoom()
        {
            bool result = false;
            M3Item item = null;
            List<GreyCoom> tmp = new List<GreyCoom>();
            List<BrownCoom> tmp2 = new List<BrownCoom>();
            for (int i = 0; i < M3Config.GridHeight; i++)
            {
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    item = M3ItemManager.Instance.gridItems[i, j];
                    if (item != null && item.itemInfo.GetElement() != null)
                    {
                        if (item.itemInfo.GetElement() != null && item.itemInfo.GetElement().eName == M3ElementType.GreyCoomElement)
                        {
                            tmp.Add(((GreyCoom)item.itemInfo.GetElement()));
                            result = true;
                        }
                        if (item.itemInfo.GetElement() != null && item.itemInfo.GetElement().eName == M3ElementType.BrownCoomElement)
                        {
                            BrownCoom bCoom = (BrownCoom)(item.itemInfo.GetElement());
                            tmp2.Add(bCoom);


                        }
                    }
                }
            }
            for (int i = 0; i < tmp.Count; i++)
            {
                tmp[i].Jump();
            }
            for (int i = 0; i < tmp2.Count; i++)
            {
                if (!tmp2[i].needDivide)
                {
                    result = true;
                    tmp2[i].Jump();
                }
            }
            return result;
        }

        public bool CheckJump()
        {
            bool result = false;
            M3Item item = null;
            List<Element> tmp2 = new List<Element>();
            for (int i = 0; i < M3Config.GridHeight; i++)
            {
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    item = M3ItemManager.Instance.gridItems[i, j];
                    Element ele = null;
                    if (item != null)
                        ele = item.itemInfo.GetElement();
                    if (ele != null && ele.data.config.JumpType && ele.data.config.JumpSpace > 0)
                    {
                        tmp2.Add(ele);
                        if (!result)
                            result = true;
                    }
                }
            }

            for (int i = 0; i < tmp2.Count; i++)
            {
                tmp2[i].Jump();
            }
            return result;
        }

        public bool CheckBrownCoom()
        {
            bool result = false;
            M3Item item = null;
            for (int i = 0; i < M3Config.GridHeight; i++)
            {
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    item = M3ItemManager.Instance.gridItems[i, j];
                    if (item != null && item.itemInfo.GetElement() != null)
                    {
                        if (item.itemInfo.GetElement().eName == M3ElementType.BrownCoomElement)
                        {
                            BrownCoom bCoom = ((BrownCoom)item.itemInfo.GetElement());
                            if (bCoom.needDivide)
                            {
                                ((BrownCoom)item.itemInfo.GetElement()).Divide();
                                result = true;
                            }
                        }
                    }
                }
            }
            return result;
        }

        public bool CheckCystal()
        {
            bool result = false;
            M3Item item = null;
            for (int i = 0; i < M3Config.GridHeight; i++)
            {
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    item = M3ItemManager.Instance.gridItems[i, j];
                    if (item != null && item.itemInfo.GetPartakeEliminateElement() != null)
                    {
                        if (item.itemInfo.GetPartakeEliminateElement().eName == M3ElementType.CrystalElement)
                        {
                            CrystalElement crystal = ((CrystalElement)item.itemInfo.GetPartakeEliminateElement());
                            crystal.ChangeCrystalColor(SearchCrystalColor(i, j, crystal.data.GetColor()));
                            if (!result)
                                result = true;
                        }

                    }
                }
            }
            return result;
        }

        public ItemColor SearchCrystalColor(int x, int y, ItemColor selfColor)
        {
            var list = this.GetCurrentGridColorPool();
            int count = 50;
            ItemColor color = ItemColor.fRed;
            while (count > 0)
            {
                color = list[M3Supporter.Instance.GetRandomInt(0, list.Count)];
                if ((color != selfColor) && (M3GameManager.Instance.FindColorMatchCount(x, y, color) < 3))
                {
                    return color;
                }
                count--;
            }
            return color = ItemColor.fRed;
        }

        public bool CheckCattery()
        {
            bool result = false;
            M3Item item = null;
            for (int i = 0; i < M3Config.GridHeight; i++)
            {
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    item = M3ItemManager.Instance.gridItems[i, j];
                    if (item != null && item.itemInfo.GetElement() != null)
                    {
                        if (item.itemInfo.GetElement().eName == M3ElementType.CatteryElement)
                        {
                            CatteryElement bCoom = ((CatteryElement)item.itemInfo.GetElement());
                            if (!result)
                                result = bCoom.Dye();
                            else
                            {
                                bCoom.Dye();
                            }
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 检测毛线球
        /// </summary>
        /// <returns></returns>
        public bool CheckWoolBall()
        {
            bool result = false;
            M3Item item = null;
            for (int i = 0; i < M3Config.GridHeight; i++)
            {
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    item = M3ItemManager.Instance.gridItems[i, j];
                    if (item != null && item.itemInfo.GetElement() != null)
                    {
                        if (item.itemInfo.GetElement().eName == M3ElementType.WoolBall)
                        {
                            WoolBallElement woolBall = ((WoolBallElement)item.itemInfo.GetElement());
                            if (!result)
                                result = woolBall.ProcessRoll();
                            else
                            {
                                woolBall.ProcessRoll();
                            }
                        }
                    }
                }
            }
            return result;
        }

        public int FindColorMatchCount(int x, int y, ItemColor color)
        {
            var items = M3ItemManager.Instance.gridItems;
            var cells = M3GridManager.Instance.gridCells;

            int rowCount = 0;
            int totalCount = 0;
            for (int ly = y - 1; ly >= 0; ly--)
            {
                if (items[x, ly] != null
                    && items[x, ly].itemInfo.GetPartakeEliminateElement() != null
                    && items[x, ly].itemInfo.GetPartakeEliminateElement().GetColor() == color)
                {
                    rowCount++;
                }
                else
                {
                    break;
                }
            }
            for (int ry = y + 1; ry < M3Config.GridWidth; ry++)
            {
                if (items[x, ry] != null
                        && items[x, ry].itemInfo.GetPartakeEliminateElement() != null
                    && items[x, ry].itemInfo.GetPartakeEliminateElement().GetColor() == color)
                {
                    rowCount++;
                }
                else
                {
                    break;
                }
            }

            if (rowCount < 2)
            {
                rowCount = 0;
            }

            int colCount = 0;
            for (int tx = x + 1; tx < M3Config.GridHeight; tx++)
            {
                if (
                    items[tx, y] != null
                    && items[tx, y].itemInfo.GetPartakeEliminateElement() != null
                    && items[tx, y].itemInfo.GetPartakeEliminateElement().GetColor() == color)
                {
                    colCount++;
                }
                else
                {
                    break;
                }
            }
            for (int bx = x - 1; bx >= 0; bx--)
            {
                if (
                    items[bx, y] != null
                    && items[bx, y].itemInfo.GetPartakeEliminateElement() != null
                    && items[bx, y].itemInfo.GetPartakeEliminateElement().GetColor() == color)
                {
                    colCount++;
                }
                else
                {
                    break;
                }
            }

            rowCount += 1;
            totalCount += rowCount;
            if (colCount >= 2)
            {
                totalCount = rowCount + colCount;
            }

            if (totalCount < 3)
            {
                totalCount = 0;
            }
            return totalCount;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void AffectBoomGrid(int x, int y)
        {
            var cells = M3GridManager.Instance.gridCells;

            if (x - 1 >= 0 && cells[x - 1, y] != null)
            {
                cells[x - 1, y].BoomAffect();
            }

            if (x + 1 < M3Config.GridHeight && cells[x + 1, y] != null)
            {
                cells[x + 1, y].BoomAffect();
            }

            if (y - 1 >= 0 && cells[x, y - 1] != null)
            {
                cells[x, y - 1].BoomAffect();
            }

            if (y + 1 < M3Config.GridWidth && cells[x, y + 1] != null)
            {
                cells[x, y + 1].BoomAffect();
            }
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void AffectBoomItem(int x, int y)
        {
            var items = M3ItemManager.Instance.gridItems;
            if (x - 1 >= 0 && items[x - 1, y] != null && items[x - 1, y].itemInfo.GetElement() != null)
            {
                items[x - 1, y].itemInfo.GetElement().ProcessNeighborEliminate(x, y);
            }

            if (x + 1 < M3Config.GridHeight && items[x + 1, y] != null && items[x + 1, y].itemInfo.GetElement() != null)
            {
                items[x + 1, y].itemInfo.GetElement().ProcessNeighborEliminate(x, y);
            }

            if (y - 1 >= 0 && items[x, y - 1] != null && items[x, y - 1].itemInfo.GetElement() != null)
            {
                items[x, y - 1].itemInfo.GetElement().ProcessNeighborEliminate(x, y);
            }

            if (y + 1 < M3Config.GridWidth && items[x, y + 1] != null && items[x, y + 1].itemInfo.GetElement() != null)
            {
                items[x, y + 1].itemInfo.GetElement().ProcessNeighborEliminate(x, y);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void AffectSpecialGrid(int x, int y)
        {
            var cells = M3GridManager.Instance.gridCells;

            if (x - 1 >= 0 && cells[x - 1, y] != null)
            {
                cells[x - 1, y].SpecialAffect();
            }

            if (x + 1 < M3Config.GridWidth && cells[x + 1, y] != null)
            {
                cells[x + 1, y].SpecialAffect();
            }

            if (y - 1 >= 0 && cells[x, y - 1] != null)
            {
                cells[x, y - 1].SpecialAffect();
            }

            if (y + 1 < M3Config.GridHeight && cells[x, y + 1] != null)
            {
                cells[x, y + 1].SpecialAffect();
            }
        }

        /// <summary>
        /// 检测是否能够产生消除
        /// </summary>
        /// <returns></returns>
        public bool CheckNeedToCrush()
        {
            return matcher.GetEliminateList().Count > 0 || specialHandler.HaveSpecialInExchange();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        public void ActiveSelector(Vector3 pos)
        {

            float sourceZ = selector.transform.position.z;
            selector.transform.position = new Vector3(pos.x, pos.y, sourceZ);
            if (!selector.activeSelf)
                soundManager.PlaySelectElement();
            selector.SetActive(true);
        }

        public void HideSelector()
        {
            if (selector != null)
                selector.SetActive(false);
        }

        public void ClearSelectedItem()
        {
            _selectedItem = null;
            _selectedItem2 = null;
            HideSelector();
        }

        public List<M3Item> GetAllItem()
        {
            List<M3Item> itemList = new List<M3Item>();

            for (int i = 0; i < M3Config.GridHeight; i++)
            {
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    M3Item item = M3ItemManager.Instance.gridItems[i, j];
                    if (item != null && !item.isCrushing /*&& (item.itemInfo.GetElement().data.IsBaseAndeSpecialElement()|| item.itemInfo.GetElement().eName == M3ElementType.LockElement)*/)
                        itemList.Add(item);
                }
            }

            return itemList;
        }

        /// <summary>
        /// 获取相同颜色的所有元素
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public List<M3Item> GetAllSameColorItem(ItemColor color)
        {
            List<M3Item> itemList = new List<M3Item>();

            for (int i = 0; i < M3Config.GridHeight; i++)
            {
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    M3Item item = M3ItemManager.Instance.gridItems[i, j];
                    if (item != null && !item.isCrushing
                        && item.itemInfo.GetWithOutCoverEliminateElement() != null
                        && item.itemInfo.GetWithOutCoverEliminateElement().GetColor() == color
                        /*&& (item.itemInfo.GetPartakeEliminateElement().eName == M3ElementType.NormalElement || item.itemInfo.GetPartakeEliminateElement().eName == M3ElementType.SpecialElement || item.itemInfo.GetPartakeEliminateElement().eName == M3ElementType.CrystalElement)*/)
                        itemList.Add(item);
                }
            }
            return itemList;
        }

        /// <summary>
        /// 获取相同颜色的可消除元素，包括特效元素
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public List<M3Item> GetSameBaseColorItem(ItemColor color)
        {
            List<M3Item> itemList = new List<M3Item>();

            for (int i = 0; i < M3Config.GridHeight; i++)
            {
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    M3Item item = M3ItemManager.Instance.gridItems[i, j];
                    if (item != null && !item.isCrushing
                        && item.itemInfo.GetPartakeEliminateElement() != null
                        && item.itemInfo.GetPartakeEliminateElement().GetColor() == color
                        && (item.itemInfo.GetElement().eName == M3ElementType.NormalElement || item.itemInfo.GetElement().eName == M3ElementType.SpecialElement))
                        itemList.Add(item);
                }
            }
            return itemList;
        }

        /// <summary>
        /// 获取相同颜色的可消除普通元素，不包括特效元素
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public List<M3Item> GetSameNormalColorItem(ItemColor color)
        {
            List<M3Item> itemList = new List<M3Item>();

            for (int i = 0; i < M3Config.GridHeight; i++)
            {
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    M3Item item = M3ItemManager.Instance.gridItems[i, j];
                    if (item != null && !item.isCrushing
                        && item.itemInfo.GetPartakeEliminateElement() != null
                        && item.itemInfo.GetPartakeEliminateElement().GetColor() == color
                        && item.itemInfo.GetElement().eName == M3ElementType.NormalElement)
                        itemList.Add(item);
                }
            }
            return itemList;
        }

        public List<M3Item> GetAllNormalElement(ItemColor ignoreColor = ItemColor.fNone)
        {
            List<M3Item> itemList = new List<M3Item>();
            for (int i = 0; i < M3Config.GridHeight; i++)
            {
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    M3Item item = M3ItemManager.Instance.gridItems[i, j];
                    if (item != null && !item.isCrushing && item.itemInfo.GetElement().data.IsBaseElement() && item.itemInfo.GetElement() is NormalElement)
                    {
                        if (ignoreColor == ItemColor.fNone)
                            itemList.Add(item);
                        else if (item.itemInfo.GetElement().GetColor() != ignoreColor)
                        {
                            itemList.Add(item);
                        }
                    }
                }
            }
            return itemList;
        }

        public List<M3Item> GetAllSpecialElement()
        {
            List<M3Item> itemList = new List<M3Item>();
            for (int i = 0; i < M3Config.GridHeight; i++)
            {
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    M3Item item = M3ItemManager.Instance.gridItems[i, j];
                    if (item != null && !item.isCrushing && item.itemInfo.GetElement().data.IsSpecialElement() && (item.itemInfo.GetElement() is SpecialElement || item.itemInfo.GetElement() is MagicCatElement))
                    {
                        itemList.Add(item);
                    }
                }
            }
            return itemList;
        }

        public List<M3Item> GetAllBaseElementWithoutEnergy()
        {
            List<M3Item> itemList = new List<M3Item>();
            for (int i = 0; i < M3Config.GridHeight; i++)
            {
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    M3Item item = M3ItemManager.Instance.gridItems[i, j];
                    if (item != null && !item.isCrushing
                        && item.itemInfo.GetElement() != null
                        && (item.itemInfo.GetElement().eName == M3ElementType.NormalElement || item.itemInfo.GetElement().eName == M3ElementType.SpecialElement || item.itemInfo.GetElement().eName == M3ElementType.MagicCatElement)
                        && !item.itemInfo.GetElement().haveEnergy)
                    {
                        itemList.Add(item);
                    }
                }
            }
            return itemList;
        }

        public List<M3Item> GetElementByID(int[] idList)
        {
            List<M3Item> itemList = new List<M3Item>();
            if (idList != null)
                for (int i = 0; i < M3Config.GridHeight; i++)
                {
                    for (int j = 0; j < M3Config.GridWidth; j++)
                    {
                        M3Item item = M3ItemManager.Instance.gridItems[i, j];
                        if (item != null
                            && !item.isCrushing
                            && item.itemInfo.GetElement() != null
                            && (((IList)idList).Contains(item.itemInfo.GetElement().data.config.ID)))
                        {
                            itemList.Add(item);
                        }
                    }
                }
            return itemList;
        }

        public List<Int2> GetCrossItem(int x, int y)
        {
            var list1 = GetLineAllItem(x, y, true, true);
            var list2 = GetLineAllItem(x, y, false, false);
            list2.InsertRange(0, list1);
            return list2;
        }

        public List<Int2> GetLineAllItem(int px, int py, bool isRow, bool containSelf = false, bool containResist = false)
        {
            List<Int2> itemList = new List<Int2>();
            Int2 tmp;
            if (isRow)
            {
                for (int yy = py; yy < M3Config.GridWidth; yy++)
                {
                    if (yy == py)
                        continue;
                    tmp = new Int2(px, yy);
                    if (!containResist && M3ItemManager.Instance.gridItems[px, yy] != null)
                    {
                        var ele = M3ItemManager.Instance.gridItems[px, yy].itemInfo.GetElement();
                        if (ele != null && ele.data.config.CanResist)
                        {
                            itemList.Add(tmp);
                            break;
                        }
                    }
                    itemList.Add(tmp);
                }
                for (int yy = py; yy >= 0; yy--)
                {
                    tmp = new Int2(px, yy);
                    if (M3ItemManager.Instance.gridItems[px, yy] != null)
                    {
                        var ele = M3ItemManager.Instance.gridItems[px, yy].itemInfo.GetElement();
                        if (ele != null && ele.data.config.CanResist)
                        {
                            itemList.Add(tmp);
                            break;
                        }
                    }
                    itemList.Add(tmp);
                }
            }
            else
            {
                for (int xx = px; xx < M3Config.GridHeight; xx++)
                {
                    if (xx == px)
                        continue;
                    tmp = new Int2(xx, py);
                    if (!containResist && M3ItemManager.Instance.gridItems[xx, py] != null)
                    {
                        var ele = M3ItemManager.Instance.gridItems[xx, py].itemInfo.GetElement();
                        if (ele != null && ele.data.config.CanResist)
                        {
                            itemList.Add(tmp);
                            break;
                        }
                    }
                    itemList.Add(tmp);
                }
                for (int xx = px; xx >= 0; xx--)
                {

                    tmp = new Int2(xx, py);
                    if (!containResist && M3ItemManager.Instance.gridItems[xx, py] != null)
                    {
                        var ele = M3ItemManager.Instance.gridItems[xx, py].itemInfo.GetElement();
                        if (ele != null && ele.data.config.CanResist)
                        {
                            itemList.Add(tmp);
                            break;
                        }
                    }
                    itemList.Add(tmp);
                }
            }

            if (!containSelf)
                itemList.Insert(0, new Int2(px, py));
            return itemList;
        }

        public List<M3Item> GetLineItem(int px, int py, bool isRow, bool contain = false)
        {
            List<M3Item> itemList = new List<M3Item>();
            M3Item tmp;
            if (isRow)
            {

                for (int yy = py; yy < M3Config.GridWidth; yy++)
                {
                    if (!contain && yy == py)
                        continue;
                    tmp = M3ItemManager.Instance.gridItems[px, yy];
                    if (tmp != null/*&&!tmp.isCrushing*/)
                    {
                        itemList.Add(tmp);
                        var ele = tmp.itemInfo.GetElement();
                        if (ele != null && ele.data.config.CanResist)
                            break;
                    }
                }
                for (int yy = py; yy >= 0; yy--)
                {

                    tmp = M3ItemManager.Instance.gridItems[px, yy];
                    if (tmp != null /*&&!tmp.isCrushing*/)
                    {
                        itemList.Add(tmp);
                        var ele = tmp.itemInfo.GetElement();
                        if (ele != null && ele.data.config.CanResist)
                            break;
                    }
                }
            }
            else
            {
                for (int xx = px; xx < M3Config.GridHeight; xx++)
                {
                    if (!contain && xx == px)
                        continue;
                    tmp = M3ItemManager.Instance.gridItems[xx, py];
                    if (tmp != null /*&& !tmp.isCrushing*/)
                    {
                        itemList.Add(tmp);
                        var ele = tmp.itemInfo.GetElement();
                        if (ele != null && ele.data.config.CanResist)
                            break;
                    }
                }
                for (int xx = px; xx >= 0; xx--)
                {

                    tmp = M3ItemManager.Instance.gridItems[xx, py];
                    if (tmp != null/* && !tmp.isCrushing*/)
                    {
                        itemList.Add(tmp);
                        var ele = tmp.itemInfo.GetElement();
                        if (ele != null && ele.data.config.CanResist)
                            break;
                    }
                }
            }
            return itemList;
        }

        public void LockItem(Int2[] arr)
        {
            //if (arr == null)
            //    return;
            //for (int i = 0; i < M3Config.GridHeight; i++)
            //{
            //    for (int j = 0; j < M3Config.GridWidth; j++)
            //    {
            //        M3Item item = M3ItemManager.Instance.gridItems[i, j];
            //        if (item != null)
            //        {
            //            item.touchLock = true;
            //        }
            //    }
            //}
            //for (int i = 0; i < arr.Length; i++)
            //{
            //    if (CheckValid(arr[i].x, arr[i].y))
            //    {
            //        M3Item item = M3ItemManager.Instance.gridItems[arr[i].x, arr[i].y];
            //        if (item != null)
            //        {
            //            item.touchLock = false;
            //        }
            //    }

            //}
        }
        
        #endregion

        #region Special 

        public bool CheckSteady()
        {
            return M3GridManager.Instance.IsAllElementStopTweening() && M3GridManager.Instance.IsAllElementLanded() && !M3GridManager.Instance.dropLock;
        }

        public bool CheckValid(int x, int y)
        {
            return x >= 0 && x < M3Config.GridHeight && y >= 0 && y < M3Config.GridWidth;
        }

        public bool CheckGridValid(int x, int y)
        {
            return CheckValid(x, y) && M3GridManager.Instance.gridCells[x, y] != null;
        }

        /// <summary>
        /// 检测格子是否有绳索
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool CheckRopeType(int x, int y, RopeTypeEnum type)
        {
            if (!M3GameManager.Instance.CheckValid(x, y))
            {
                return false;
            }
            var info = GetGridInfo(x, y);
            if (info != null)
                return (info.ropeTypeEnum & type) == type;
            return false;
        }

        public bool CheckRopeType(M3Grid grid, RopeTypeEnum type)
        {
            if (grid == null)
                return false;
            var info = grid.gridInfo;
            if (info != null)
                return (info.ropeTypeEnum & type) == type;
            return false;
        }

        /// <summary>
        /// 检测连个格子间是否有绳索阻隔
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public bool CheckRopeBetweenItems(int x1, int y1, int x2, int y2)
        {
            var info1 = GetGridInfo(x1, y1);
            var info2 = GetGridInfo(x2, y2);
            //上下
            if (x1 > x2 && y1 == y2)
            {
                if (info1 != null && info2 != null)
                {
                    if (((info1.ropeTypeEnum & RopeTypeEnum.Top) == RopeTypeEnum.Top) ||
                        ((info2.ropeTypeEnum & RopeTypeEnum.Bottom) == RopeTypeEnum.Bottom))
                    {
                        return true;
                    }
                }
            }
            if (x1 < x2 && y1 == y2)
            {
                if (info1 != null && info2 != null)
                {
                    if (((info1.ropeTypeEnum & RopeTypeEnum.Bottom) == RopeTypeEnum.Bottom) ||
                        ((info2.ropeTypeEnum & RopeTypeEnum.Top) == RopeTypeEnum.Top))
                    {
                        return true;
                    }
                }
            }
            //左右
            if (x1 == x2 && y1 > y2)
            {
                if (info1 != null && info2 != null)
                {
                    if (((info1.ropeTypeEnum & RopeTypeEnum.Left) == RopeTypeEnum.Left) ||
                        ((info2.ropeTypeEnum & RopeTypeEnum.Rigth) == RopeTypeEnum.Rigth))
                    {
                        return true;
                    }
                }
            }
            if (x1 == x2 && y1 < y2)
            {
                if (info1 != null && info2 != null)
                {
                    if (((info1.ropeTypeEnum & RopeTypeEnum.Rigth) == RopeTypeEnum.Rigth) ||
                        ((info2.ropeTypeEnum & RopeTypeEnum.Left) == RopeTypeEnum.Left))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private M3GridInfo GetGridInfo(int x, int y)
        {
            try
            {
                if (M3GridManager.Instance.gridCells[x, y] != null)
                    return M3GridManager.Instance.gridCells[x, y].gridInfo;
                return null;
            }
            catch (System.Exception)
            {
                Debug.Log(x + "|" + y);
                throw;
            }

        }

        #endregion

        #region Unity

        private void Awake()
        {
            Instance = this;
            if (isAutoAI) return;
            gameScreen = GameObject.Find("GameScreen");
            KLoading.LoadCompleted();
        }

        private void Start()
        {
            FrameScheduler.Instance().Clear();
            gameFsm = new GameFSM();
            gameFsm.Init();
        }

        private void Update()
        {
            if (!isAutoAI)
            {
                if (isPause) return;
                SelectItem();
            }
        }

        private void FixedUpdate()
        {
            if (!isAutoAI)
            {
                if (isPause) return;
                GMUpdate();
            }
        }

        public void GMUpdate()
        {
            gameFsm.GetFSM().SMUpdate();
        }

        public void RunGame()
        {
            gameFsm.GetFSM().ChangeGameState(StateEnum.CheckAndCrush);
            gameFsm.GetFSM().SetGlobalStateState(StateEnum.Global);
        }

        public void ShakeGrid()
        {
            float y = M3GridManager.Instance.gridBoard.transform.position.y;
            float offset1 = 0.15f;
            float offset2 = 0.1f;
            float time1 = 0.08f;
            float time2 = 0.04f;
            KTweenUtils.MoveY(M3GridManager.Instance.gridBoard.transform, y + offset1, time2, delegate ()
            {
                KTweenUtils.MoveY(M3GridManager.Instance.gridBoard.transform, y - offset1, time1, delegate ()
                {
                    KTweenUtils.MoveY(M3GridManager.Instance.gridBoard.transform, y + offset1, time1, delegate ()
                    {
                        KTweenUtils.MoveY(M3GridManager.Instance.gridBoard.transform, y, offset2, delegate ()
                        {

                        });
                    });
                });
            });
        }

        #region BackDate
        public void SetCurrentBackDate()
        {
            lastGrid = M3GridManager.Instance.gridCells;
            List<Element>[,] tmp = new List<Element>[M3Config.GridHeight, M3Config.GridWidth];
            List<Element>[,] gridTmp = new List<Element>[M3Config.GridHeight, M3Config.GridWidth];
            for (int i = 0; i < M3Config.GridHeight; i++)
            {
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    if (M3ItemManager.Instance.gridItems[i, j] != null)
                    {
                        //if (M3ItemManager.Instance.gridItems[i, j].itemInfo.allElementList.Count > 1)
                        //{
                        //    Debug.Log("COunt " + i+"_"+j+M3ItemManager.Instance.gridItems[i, j].itemInfo.allElementList.Count);
                        //}
                        tmp[i, j] = new List<Element>();
                        for (int k = 0; k < M3ItemManager.Instance.gridItems[i, j].itemInfo.allElementList.Count; k++)
                        {
                            tmp[i, j].Add(M3ItemManager.Instance.gridItems[i, j].itemInfo.allElementList[k].Clone());
                        }
                    }
                    if (M3GridManager.Instance.gridCells[i, j] != null)
                    {
                        M3Grid grid = M3GridManager.Instance.gridCells[i, j];
                        if (grid.gridInfo.floorElement != null)
                        {
                            gridTmp[i, j] = new List<Element>();
                            var ele = grid.gridInfo.floorElement.Clone();
                            gridTmp[i, j].Add(ele);
                        }
                    }
                }
            }
            lastBackData.items = tmp;
            lastBackData.grids = gridTmp;
            lastBackData.target = modeManager.CloneTarget();
            lastBackData.score = modeManager.score;
            lastBackData.step = modeManager.GameModeCtrl.totalSetps;
            lastBackData.currentStep = modeManager.GameModeCtrl.CurrentEnergyStepCount;
            lastBackData.energy = catManager.GetCurrentEnergy();
        }
        public void SetFirstBackDate()
        {
            lastGrid = M3GridManager.Instance.gridCells;
            List<Element>[,] tmp = new List<Element>[M3Config.GridHeight, M3Config.GridWidth];
            List<Element>[,] gridTmp = new List<Element>[M3Config.GridHeight, M3Config.GridWidth];
            for (int i = 0; i < M3Config.GridHeight; i++)
            {
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    if (M3ItemManager.Instance.gridItems[i, j] != null)
                    {
                        //if (M3ItemManager.Instance.gridItems[i, j].itemInfo.allElementList.Count > 1)
                        //{
                        //    Debug.Log("COunt " + i+"_"+j+M3ItemManager.Instance.gridItems[i, j].itemInfo.allElementList.Count);
                        //}
                        tmp[i, j] = new List<Element>();
                        for (int k = 0; k < M3ItemManager.Instance.gridItems[i, j].itemInfo.allElementList.Count; k++)
                        {
                            tmp[i, j].Add(M3ItemManager.Instance.gridItems[i, j].itemInfo.allElementList[k].Clone());
                        }
                    }
                    if (M3GridManager.Instance.gridCells[i, j] != null)
                    {
                        M3Grid grid = M3GridManager.Instance.gridCells[i, j];
                        if (grid.gridInfo.floorElement != null)
                        {
                            gridTmp[i, j] = new List<Element>();
                            var ele = grid.gridInfo.floorElement.Clone();
                            gridTmp[i, j].Add(ele);
                        }
                    }

                }
            }
            firstBackData.items = tmp;
            firstBackData.grids = gridTmp;
            firstBackData.target = modeManager.CloneTarget();
            firstBackData.score = modeManager.score;
            firstBackData.step = modeManager.GameModeCtrl.totalSetps;
            firstBackData.currentStep = modeManager.GameModeCtrl.CurrentEnergyStepCount;
            firstBackData.energy = catManager.GetCurrentEnergy();
        }
        public void BackDate(M3BackDate data = null)
        {
            //M3ItemManager.Instance.ReloadAllItem(lastItem);
            //if (lastTarget != null)
            //{
            //    foreach (var item in lastTarget)
            //    {
            //        Debug.Log(item.Key + "_" + item.Value);
            //    }

            //}
            //modeManager.GameModeCtrl.SetTargetDic(lastTarget);
            //modeManager.score = lastScore;
            //modeManager.GameModeCtrl.totalSetps = lastStep;
            if (data == null)
                data = lastBackData;
            M3ItemManager.Instance.ReloadAllItem(data.items);

            M3GridManager.Instance.ReloadAllGrid(data.grids);

            modeManager.GameModeCtrl.SetTargetDic(data.target);
            modeManager.score = data.score;
            modeManager.GameModeCtrl.totalSetps = data.step;
            catManager.SetCurrentEnergy(data.energy);
        }
        #endregion

        //public void OnGUI()
        //{
        //    if (GUILayout.Button("KKKKKK"))
        //    {
        //        BackDate();
        //    }
        //    if (GUILayout.Button("qqqqqqq"))
        //    {
        //        BackDate(firstBackData);
        //    }
        //    if (GUILayout.Button("SSSSSSSSSSSS"))
        //    {
        //        bonusManager.PlayFinishQuanAnimation();
        //        FrameScheduler.instance.Add(20, delegate ()
        //        {
        //            bonusManager.PlayFinishAnimation();
        //        });
        //    }
        //    if (GUILayout.Button("KKKKKSADSDASADK"))
        //    {
        //        Debug.Log(gameFsm.GetFSM().GetCurrentStateEnum().ToString());
        //    }
        //    GUILayout.TextField("总内存：" + ByteToM(UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong()) + "M");
        //    GUILayout.TextField("堆内存：" + ByteToM(UnityEngine.Profiling.Profiler.GetMonoUsedSizeLong()) + "M");
        //    if (GUILayout.Button("KKKKKK"))
        //    {
        //        var skill = new ShockWave();
        //        skill.FindTarget();
        //    }
        //    //if (M3GridManager.Instance == null)
        //    //    return;
        //    for (int i = 0; i < M3Config.GridHeight; i++)
        //    {
        //        for (int j = 0; j < M3Config.GridWidth; j++)
        //        {
        //            if (M3GridManager.Instance.gridCells[i, j] == null)
        //                continue;
        //            //if (M3ItemManager.Instance.gridItems[i, j] != null)
        //            //{
        //            //    if (M3ItemManager.Instance.gridItems[i, j].dropSpeed == M3Config.DropSpeedMax)
        //            //        Debug.LogError(i + "|" + j);
        //            //}
        //            if (M3GridManager.Instance.gridCells[i, j].isDroping)
        //            {
        //                var t = M3GridManager.Instance.gridCells[i, j].transform.Find("test");
        //                t.gameObject.SetActive(true);
        //                t.GetComponent<SpriteRenderer>().color = Color.red;
        //            }
        //            else
        //            {
        //                var t = M3GridManager.Instance.gridCells[i, j].transform.Find("test");
        //                t.gameObject.SetActive(false);
        //            }
        //        }
        //    }
        //}
        //static float ByteToM(long byteCount)
        //{
        //    return (float)(byteCount / (1024.0f * 1024.0f));
        //}

        #endregion

        public void OnExitGame(ExitGameType type)
        {
            switch (type)
            {
                case ExitGameType.None:
                    ReturnToCJ();
                    break;
                case ExitGameType.NextLevel:
                    ReSelectLevel();
                    break;
                case ExitGameType.Reload:
                    ReLoadLevelScene();
                    break;
                case ExitGameType.Editor:
                    ReturnEditor();
                    break;
                default:
                    break;
            }
            isGameOver = false;
        }
        private void ReturnToCJ()
        {
            RemoveFightEvent();
            LevelDataModel.Instance.ExitLevelScene(null);
        }
        private void ReSelectLevel()
        {
            RemoveFightEvent();
            //KLevelManager.Instance.FinishLevel(new MapSelectWindow.MapSelectData(KLevelManager.Instance.currLevel));
            //KUIWindow.OpenWindow<MapSelectWindow>(
            //    new MapSelectWindow.MapSelectData(KLevelManager.Instance.currLevel,delegate() { KLevelManager.Instance.FinishLevel(null); })
            //    );
            //KUIWindow.OpenWindow<MapSelectWindow>();
            KUIWindow.OpenWindow<MapSelectWindow>(
        new MapSelectWindow.MapSelectData(LevelDataModel.Instance.CurrLevel.ID, LevelDataModel.Instance.CurrLevel.NextLevelID, true)
        );

        }
        private void ReLoadLevelScene()
        {
            RemoveFightEvent();
            if (M3Config.isEditor)
            {
                KLaunch.LoadLevel("matchScene");

            }
            else
            {
                M3GameManager.Instance.isPause = true;
                KUIWindow.OpenWindow<MapSelectWindow>(
                    new MapSelectWindow.MapSelectData(LevelDataModel.Instance.CurrLevel, true)
                    );
            }

        }
        private void ReturnEditor()
        {
            RemoveFightEvent();
            KLaunch.LoadLevel("M3EditorScene");
        }
        public void ExitLevelScene(string SceneName)
        {

        }
        private void RemoveFightEvent()
        {
            foreach (M3FightEnum item in Enum.GetValues(typeof(M3FightEnum)))
            {
                M3GameEvent.RemoveEvent(item);
            }
        }

    }
}