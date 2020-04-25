using UnityEngine;

namespace Game.Match3
{
    /// <summary>
    /// 特殊效果消除处理器
    /// </summary>
    public class M3SpecialHandler
    {
        /// <summary>
        /// 附带特殊效果的元素数量
        /// </summary>
        public int specialCount;

        public ElementSpecial special1 = ElementSpecial.None;
        public ElementSpecial special2 = ElementSpecial.None;
        public M3Item SpecialItem1 = null;
        public M3Item SpecialItem2 = null;

        /// <summary>
        /// 重置
        /// </summary>
        public void ResetVars()
        {
            specialCount = 0;
            special1 = ElementSpecial.None;
            special2 = ElementSpecial.None;
            //SpecialItem1 = null;
            //SpecialItem2 = null;
        }

        /// <summary>
        /// 检测是否是特殊效果交换可直接消除（如一个横消和竖消或魔力猫）
        /// </summary>
        /// <param name="item1">from</param>
        /// <param name="item2">to</param>
        public void GetSpecialType(M3Item item1, M3Item item2)
        {
            ResetVars();
            int num = 0;
            if (item1 == null || item2 == null)
            {
                Debug.Log("GetSpecialType Item is Null");
                return;
            }
            if (item1.itemInfo.GetElement() == null || item2.itemInfo.GetElement() == null)
                return;

            SpecialItem1 = item1;
            SpecialItem2 = item2;
            if (item1.itemInfo.GetElement().data.GetSpecial() > 0)
            {
                specialCount += 1;
                special1 = item1.itemInfo.GetElement().GetSpecial();
                if (item1.itemInfo.GetElement().eName == M3ElementType.MagicCatElement
                    && M3Const.MagicEliminateContains.Contains(item2.itemInfo.GetElement().eName))
                    num += 1;
            }
            if (item2.itemInfo.GetElement().data.GetSpecial() > 0)
            {
                specialCount += 1;
                special2 = item2.itemInfo.GetElement().GetSpecial();
                if (item2.itemInfo.GetElement().eName == M3ElementType.MagicCatElement
                    && M3Const.MagicEliminateContains.Contains(item1.itemInfo.GetElement().eName))
                    num += 1;
            }

            // 只有两个都是特殊效果或是魔力猫可消除的才会产生特殊消除
            if (specialCount == 1 && num == 0)
            {
                ResetVars();
            }
        }

        /// <summary>
        /// 是否是特殊效果交换
        /// </summary>
        /// <returns></returns>
        public bool HaveSpecialInExchange()
        {
            return this.specialCount > 0;
        }

        public void GoSpecialCrush()
        {
            #region Special
            if (SpecialItem1 == null || SpecialItem2 == null)
            {
                Debug.Log("SpecialHandler Item Is Null");
                ResetVars();
                return;
            }
            if (SpecialItem1.itemInfo.GetElement() == null || SpecialItem2.itemInfo.GetElement() == null)
            {
                Debug.Log("SpecialHandler Item Is Null");
                ResetVars();
                return;
            }
            M3ItemInfo info1 = SpecialItem1.itemInfo;
            M3ItemInfo info2 = SpecialItem2.itemInfo;
            if (specialCount == 1)
            {
                if (special1 == ElementSpecial.Color)
                {
                    SpecialItem1.OnSpecialCrush(ItemSpecial.fColor, new object[] { info2.GetPartakeEliminateElement().GetColor(), info1.posX, info1.posY, info2.posX, info2.posY });
                }
                else if (special2 == ElementSpecial.Color)
                {
                    SpecialItem2.OnSpecialCrush(ItemSpecial.fColor, new object[] { info1.GetPartakeEliminateElement().GetColor(), info2.posX, info2.posY, info1.posX, info1.posY });
                }
            }
            else
            {
                //两个点头特效
                if (special1 == ElementSpecial.Column && special2 == ElementSpecial.Column)
                {
                    ////SwapItemPosition(item1, item2);
                    ////横向
                    //if (info1.posX == info2.posX)
                    //{
                    //    EliminateManager.Instance.ProcessDoubleColEliminate(ItemSpecial.fDoubleCol, info2.posX, info2.posY, info1.posX, info1.posY, true);
                    //}
                    ////纵向
                    //else if (info1.posY == info2.posY)
                    //{
                    //    EliminateManager.Instance.ProcessDoubleColEliminate(ItemSpecial.fDoubleCol, info2.posX, info2.posY, info1.posX, info1.posY, false);
                    //}

                    // 点头/点头交换 和 摇头/摇头交换 改为以结束点为中心进行十字消除（原点头/摇头交换效果）
                    EliminateManager.Instance.ProcessRowAndColEliminate(ItemSpecial.fRowAndCol, ItemSpecial.fColumn, ItemSpecial.fRow, info1.posX, info1.posY, info2.posX, info2.posY, 1);
                    if (M3GameManager.Instance.soundManager != null)
                    {
                        M3GameManager.Instance.soundManager.PlayLineToLineAudio();
                    }
                }

                //两个摇头特效
                if (special1 == ElementSpecial.Row && special2 == ElementSpecial.Row)
                {
                    ////SwapItemPosition(item1, item2);
                    ////横向
                    //if (info1.posX == info2.posX)
                    //{
                    //    EliminateManager.Instance.ProcessDoubleColEliminate(ItemSpecial.fDoubleRow, info2.posX, info2.posY, info1.posX, info1.posY, true);
                    //}
                    ////纵向
                    //else if (info1.posY == info2.posY)
                    //{
                    //    EliminateManager.Instance.ProcessDoubleColEliminate(ItemSpecial.fDoubleRow, info2.posX, info2.posY, info1.posX, info1.posY, false);
                    //}

                    // 点头/点头交换 和 摇头/摇头交换 改为以结束点为中心进行十字消除（原点头/摇头交换效果）
                    EliminateManager.Instance.ProcessRowAndColEliminate(ItemSpecial.fRowAndCol, ItemSpecial.fColumn, ItemSpecial.fRow, info1.posX, info1.posY, info2.posX, info2.posY, 1);
                    if (M3GameManager.Instance.soundManager != null)
                    {
                        M3GameManager.Instance.soundManager.PlayLineToLineAudio();
                    }
                }

                //第一个为 点头，第二个为 摇头
                if (special1 == ElementSpecial.Column && special2 == ElementSpecial.Row)
                {
                    //SwapItemPosition(item1, item2);
                    EliminateManager.Instance.ProcessRowAndColEliminate(ItemSpecial.fRowAndCol, ItemSpecial.fColumn, ItemSpecial.fRow, info2.posX, info2.posY, info1.posX, info1.posY, 2);
                    if (M3GameManager.Instance.soundManager != null)
                    {
                        M3GameManager.Instance.soundManager.PlayLineToLineAudio();
                    }
                }

                //第一个为 摇头，第二个为 点头
                if (special1 == ElementSpecial.Row && special2 == ElementSpecial.Column)
                {
                    //SwapItemPosition(item1, item2);
                    EliminateManager.Instance.ProcessRowAndColEliminate(ItemSpecial.fRowAndCol, ItemSpecial.fColumn, ItemSpecial.fRow, info1.posX, info1.posY, info2.posX, info2.posY, 1);
                    if (M3GameManager.Instance.soundManager != null)
                    {
                        M3GameManager.Instance.soundManager.PlayLineToLineAudio();
                    }
                }

                // 点头与炸弹
                if (special1 == ElementSpecial.Column && special2 == ElementSpecial.Area || special2 == ElementSpecial.Column && special1 == ElementSpecial.Area)
                {
                    ////SwapItemPosition(item1, item2);
                    if (info1.posX != info2.posX && info1.posY == info2.posY)//纵向交换
                    {
                        if (special1 == ElementSpecial.Column)
                            EliminateManager.Instance.ProcessCol2AreaEliminate(ItemSpecial.fCol2Area, info1.posX, info1.posY, info2.posX, info2.posY, 0);
                        else
                            EliminateManager.Instance.ProcessCol2AreaEliminate(ItemSpecial.fCol2Area, info2.posX, info2.posY, info1.posX, info1.posY, 0);
                    }
                    else /*if (info1.posY != info2.posY && info1.posX == info2.posX)*///横向交换
                    {
                        if (special1 == ElementSpecial.Column)
                            EliminateManager.Instance.ProcessCol2AreaEliminate(ItemSpecial.fCol2Area, info1.posX, info1.posY, info2.posX, info2.posY, 1);
                        else
                            EliminateManager.Instance.ProcessCol2AreaEliminate(ItemSpecial.fCol2Area, info2.posX, info2.posY, info1.posX, info1.posY, 1);
                    }

                    if (M3GameManager.Instance.soundManager != null)
                    {
                        M3GameManager.Instance.soundManager.PlayLineToWrapAudio();
                    }
                }

                // 摇头与炸弹
                if (special1 == ElementSpecial.Row && special2 == ElementSpecial.Area || special2 == ElementSpecial.Row && special1 == ElementSpecial.Area)
                {
                    //SwapItemPosition(item1, item2);
                    if (info1.posX != info2.posX && info1.posY == info2.posY)//纵向交换
                    {
                        EliminateManager.Instance.ProcessRow2AreaEliminate(ItemSpecial.fRow2Area, info1.posX, info1.posY, info2.posX, info2.posY, 0);
                    }
                    else /*if (info1.posY != info2.posY && info1.posX == info2.posX)*///横向交换
                    {
                        if (special1 == ElementSpecial.Row)
                            EliminateManager.Instance.ProcessRow2AreaEliminate(ItemSpecial.fRow2Area, info1.posX, info1.posY, info2.posX, info2.posY, 1);
                        else
                            EliminateManager.Instance.ProcessRow2AreaEliminate(ItemSpecial.fRow2Area, info2.posX, info2.posY, info1.posX, info1.posY, 1);
                    }
                    if (M3GameManager.Instance.soundManager != null)
                    {
                        M3GameManager.Instance.soundManager.PlayLineToWrapAudio();
                    }
                }

                // 两个炸弹
                if (special1 == ElementSpecial.Area && special2 == ElementSpecial.Area)
                {
                    //SwapItemPosition(item1, item2);
                    if ((info1.posX == info2.posX) && (info2.posY != info1.posY))//横向
                    {
                        EliminateManager.Instance.ProcessDoubleAreaEliminate(ItemSpecial.fDoubleArea, info2.posX, info2.posY, info1.posX, info1.posY, 1);
                    }
                    else
                    {
                        EliminateManager.Instance.ProcessDoubleAreaEliminate(ItemSpecial.fDoubleArea, info2.posX, info2.posY, info1.posX, info1.posY, 0);
                    }
                    if (M3GameManager.Instance.soundManager != null)
                    {
                        M3GameManager.Instance.soundManager.PlayWrapToWrapAudio();
                    }
                }

                //横竖特效与魔力猫
                if (special1 == ElementSpecial.Color && (special2 == ElementSpecial.Row || special2 == ElementSpecial.Column))
                {
                    //SwapItemPosition(item1, item2);
                    if (special2 == ElementSpecial.Row)
                        EliminateManager.Instance.ProcessSpecial2ColorEliminate(ItemSpecial.fLine2Color, info1.posX, info1.posY, info2.posX, info2.posY, 0, info2.GetPartakeEliminateElement().GetColor());
                    else
                        EliminateManager.Instance.ProcessSpecial2ColorEliminate(ItemSpecial.fLine2Color, info1.posX, info1.posY, info2.posX, info2.posY, 0, info2.GetPartakeEliminateElement().GetColor());
                    if (M3GameManager.Instance.soundManager != null)
                    {
                        M3GameManager.Instance.soundManager.PlayLineToColorAudio();
                    }
                }
                if (special2 == ElementSpecial.Color && (special1 == ElementSpecial.Row || special1 == ElementSpecial.Column))
                {

                    //SwapItemPosition(item1, item2);
                    if (special1 == ElementSpecial.Row)
                        EliminateManager.Instance.ProcessSpecial2ColorEliminate(ItemSpecial.fLine2Color, info2.posX, info2.posY, info1.posX, info1.posY, 0, info1.GetPartakeEliminateElement().GetColor());
                    else
                        EliminateManager.Instance.ProcessSpecial2ColorEliminate(ItemSpecial.fLine2Color, info2.posX, info2.posY, info1.posX, info1.posY, 0, info1.GetPartakeEliminateElement().GetColor());
                    if (M3GameManager.Instance.soundManager != null)
                    {
                        M3GameManager.Instance.soundManager.PlayLineToColorAudio();
                    }
                }

                //13消特效与魔力猫
                if (special1 == ElementSpecial.Color && special2 == ElementSpecial.Area)
                {
                    //SwapItemPosition(item1, item2);
                    if (special2 == ElementSpecial.Area)
                        EliminateManager.Instance.ProcessSpecial2ColorEliminate(ItemSpecial.fArea2Color, info1.posX, info1.posY, info2.posX, info2.posY, 1, info2.GetPartakeEliminateElement().GetColor());
                    if (M3GameManager.Instance.soundManager != null)
                    {
                        M3GameManager.Instance.soundManager.PlayColorToWrapAudio();
                    }
                }
                if (special2 == ElementSpecial.Color && special1 == ElementSpecial.Area)
                {

                    //SwapItemPosition(item1, item2);
                    if (special1 == ElementSpecial.Area)
                        EliminateManager.Instance.ProcessSpecial2ColorEliminate(ItemSpecial.fArea2Color, info2.posX, info2.posY, info1.posX, info1.posY, 1, info1.GetPartakeEliminateElement().GetColor());
                    if (M3GameManager.Instance.soundManager != null)
                    {
                        M3GameManager.Instance.soundManager.PlayColorToWrapAudio();
                    }
                }

                //魔力猫和魔力猫
                if (special1 == ElementSpecial.Color && special2 == ElementSpecial.Color)
                {
                    //SwapItemPosition(item1, item2);
                    EliminateManager.Instance.ProcessDoubleColorEliminate(ItemSpecial.fDoubleColor, info1.posX, info1.posY, info2.posX, info2.posY);
                    if (M3GameManager.Instance.soundManager != null)
                    {
                        M3GameManager.Instance.soundManager.PlayColorToColorAudio();
                    }
                }
            }
            #endregion

            ResetVars();
        }
    }
}