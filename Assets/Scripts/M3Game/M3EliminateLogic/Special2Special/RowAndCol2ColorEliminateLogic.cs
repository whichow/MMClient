/** 
 *FileName:     RowAndCol2ColorEliminateLogic.cs 
 *Author:       HeJunJie 
 *Version:      1.0 
 *UnityVersion：5.6.2f1
 *Date:         2017-08-04 
 *Description:    
 *History: 
*/

namespace Game.Match3
{
    /// <summary>
    /// 横/竖消与颜色 / 炸弹与颜色
    /// </summary>
    public class RowAndCol2ColorEliminateLogic : EliminateBase
    {
        private int x1;
        private int y1;
        private int x2;
        private int y2;
        private int isArea;
        private ItemColor color;

        public override void ProcessEliminateLogic(params object[] args)
        {
            base.ProcessEliminateLogic(args);
            x1 = (int)args[0];
            y1 = (int)args[1];
            x2 = (int)args[2];
            y2 = (int)args[3];
            isArea = (int)args[4];
            color = (ItemColor)args[5];

            if (isArea == 1)
                WaitToProcessArea();
            else
                WaitToProcessColAndRow();
        }

        private void WaitToProcessColAndRow()
        {
            // 先检测触发点是否有草坪，有的话所有消除组都铺
            bool hasLawn = EliminateManager.Instance.CheckHasLawn(x1, y1);
            if (!hasLawn)
            {
                hasLawn = EliminateManager.Instance.CheckHasLawn(x2, y2);
            }
            // 设置草坪
            if (hasLawn)
            {
                M3ItemManager.Instance.gridItems[x1, y1].itemInfo.GetElement().needCreateLawn = true;
                M3ItemManager.Instance.gridItems[x2, y2].itemInfo.GetElement().needCreateLawn = true;
            }

            M3Item colorItem = null;
            if (M3GameManager.Instance.CheckValid(x1, y1))
            {
                colorItem = M3ItemManager.Instance.gridItems[x1, y1];
                Element cEle = colorItem.itemInfo.GetElement();
                if (cEle.view != null)
                    cEle.view.PlayAnimation(cEle.data.config.ClearAnim);
            }
            var itemList = M3GameManager.Instance.GetSameBaseColorItem(color);

            int tmp;
            //Debug.Log(itemList.Count);
            foreach (var item in itemList)
            {
                if (item.itemInfo.GetElement().data.GetSpecial() > 0)
                    continue;

                tmp = M3Supporter.Instance.GetRandomInt(0, 2);
                if (tmp == 0)
                {
                    ((NormalElement)item.itemInfo.GetPartakeEliminateElement()).ChangeToSpecial(ElementSpecial.Row);
                }
                else
                {
                    ((NormalElement)item.itemInfo.GetPartakeEliminateElement()).ChangeToSpecial(ElementSpecial.Column);
                }
            }
            M3GridManager.Instance.dropLock = true;
            FrameScheduler.instance.Add(M3Config.LineTOColorWaitFrame, delegate ()
            {
                if (colorItem != null)
                {
                    colorItem.crushScore = SpecialEliminateScore.MagicAndLine;
                    colorItem.itemInfo.GetPartakeEliminateElement().crushWithOutSpecial = true;
                    colorItem.OnSpecialCrush(ItemSpecial.fNormal, null, false);
                }
                foreach (var item in itemList)
                {
                    // 设置草坪
                    if (hasLawn)
                    {
                        item.ElementCrushFlag = true;
                        item.itemInfo.GetElement().needCreateLawn = true;
                    }

                    if (M3GridManager.Instance.dropLock)
                        M3GridManager.Instance.dropLock = false;
                    item.OnSpecialCrush(ItemSpecial.fLine2Color);
                }
            });

        }

        private void WaitToProcessArea()
        {
            // 先检测触发点是否有草坪，有的话所有消除组都铺
            bool hasLawn = EliminateManager.Instance.CheckHasLawn(x1, y1);
            if (!hasLawn)
            {
                hasLawn = EliminateManager.Instance.CheckHasLawn(x2, y2);
            }
            // 设置草坪
            if (hasLawn)
            {
                M3ItemManager.Instance.gridItems[x1, y1].itemInfo.GetElement().needCreateLawn = true;
                M3ItemManager.Instance.gridItems[x2, y2].itemInfo.GetElement().needCreateLawn = true;
            }

            M3Item colorItem = null;
            if (M3GameManager.Instance.CheckValid(x1, y1))
            {
                colorItem = M3ItemManager.Instance.gridItems[x1, y1];
                Element cEle = colorItem.itemInfo.GetElement();
                if (cEle.view != null)
                    cEle.view.PlayAnimation(cEle.data.config.ClearAnim);
            }
            var itemList = M3GameManager.Instance.GetSameBaseColorItem(color);
            foreach (var item in itemList)
            {
                if (item.itemInfo.GetElement().GetSpecial() > 0)
                    continue;

                ((NormalElement)item.itemInfo.GetPartakeEliminateElement()).ChangeToSpecial(ElementSpecial.Area);
            }
            M3GridManager.Instance.dropLock = true;
            FrameScheduler.instance.Add(M3Config.LineTOColorWaitFrame, delegate ()
            {
                if (colorItem != null)
                {
                    colorItem.crushScore = SpecialEliminateScore.MagicAndBoom;
                    colorItem.itemInfo.GetPartakeEliminateElement().crushWithOutSpecial = true;
                    colorItem.OnSpecialCrush(ItemSpecial.fNormal);
                }
                foreach (var item in itemList)
                {
                    // 设置草坪
                    if (hasLawn)
                    {
                        item.ElementCrushFlag = true;
                        item.itemInfo.GetElement().needCreateLawn = true;
                    }

                    if (M3GridManager.Instance.dropLock)
                        M3GridManager.Instance.dropLock = false;
                    item.OnSpecialCrush(ItemSpecial.fArea2Color);
                }
            });
        }

    }
}