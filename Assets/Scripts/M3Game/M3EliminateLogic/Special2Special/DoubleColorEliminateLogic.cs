

using System.Collections.Generic;
/** 
*FileName:     DoubleColorEliminateLogic.cs 
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
    /// 两个颜色消
    /// </summary>
    public class DoubleColorEliminateLogic : EliminateBase
    {
        private int x1;
        private int y1;
        private int x2;
        private int y2;

        public override void ProcessEliminateLogic(params object[] args)
        {
            base.ProcessEliminateLogic(args);
            base.ProcessEliminateLogic(args);
            x1 = (int)args[0];
            y1 = (int)args[1];
            x2 = (int)args[2];
            y2 = (int)args[3];
            WaitProcessDoubleColor();
        }

        private void WaitProcessDoubleColor()
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

            M3Item colorItem1 = null;
            if (M3GameManager.Instance.CheckValid(x1, y1))
            {
                colorItem1 = M3ItemManager.Instance.gridItems[x1, y1];
                Element cEle = colorItem1.itemInfo.GetElement();
                if (cEle.view != null)
                    cEle.view.PlayAnimation(cEle.data.config.ClearAnim);
            }

            M3Item colorItem2 = M3ItemManager.Instance.gridItems[x2, y2];
            Element cEle2 = colorItem2.itemInfo.GetElement();
            if (cEle2.view != null)
                cEle2.view.PlayAnimation(cEle2.data.config.ClearAnim);

            List<M3Item> list = M3GameManager.Instance.GetAllItem();
            M3GridManager.Instance.dropLock = true;
            FrameScheduler.instance.Add(M3Config.LineTOColorWaitFrame, delegate ()
            {
                if (colorItem1 != null)
                {
                    colorItem1.crushScore = SpecialEliminateScore.DoubleMagic;
                    colorItem1.itemInfo.GetPartakeEliminateElement().crushWithOutSpecial = true;
                    colorItem1.OnSpecialCrush(ItemSpecial.fDoubleColor, null, false);
                }
                colorItem2.crushScore = SpecialEliminateScore.DoubleMagic;
                colorItem2.itemInfo.GetPartakeEliminateElement().crushWithOutSpecial = true;
                colorItem2.OnSpecialCrush(ItemSpecial.fDoubleColor, null, false);
                foreach (var item in list)
                {
                    if (M3GridManager.Instance.dropLock)
                        M3GridManager.Instance.dropLock = false;
                    if (item != null)
                    {
                        // 设置草坪
                        if (hasLawn && EliminateManager.Instance.CanLayingLawn(item))
                        {
                            var ele = item.itemInfo.GetElement();
                            if (ele != null)
                            {
                                item.ElementCrushFlag = true;
                                ele.needCreateLawn = hasLawn;
                            }
                        }

                        item.OnSpecialCrush(ItemSpecial.fDoubleColor);
                    }
                }

            });
            FrameScheduler.instance.Add(M3Config.LineTOColorWaitFrame, delegate ()
            {
                M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(Game.Match3.StateEnum.CheckAndCrush);
            });
        }

    }
}