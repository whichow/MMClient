

using System.Collections.Generic;
/** 
*FileName:     ColorEliminateLogic.cs 
*Author:       HeJunJie 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2017-08-05 
*Description:    
*History: 
*/
namespace Game.Match3
{
    public class ColorEliminateLogic : EliminateBase
    {
        private int x1;
        private int y1;
        private int x2;
        private int y2;
        private ItemColor color;

        public override void ProcessEliminateLogic(params object[] args)
        {
            base.ProcessEliminateLogic(args);
            x1 = (int)args[0];
            y1 = (int)args[1];
            x2 = (int)args[2];
            y2 = (int)args[3];
            color = (ItemColor)args[4];

            if (color == ItemColor.fRandom)
            {
                var fcolor = M3GameManager.Instance.GetCurrentGridRandomColors();
                if (fcolor != ItemColor.fNone)
                {
                    ProcessColorEliminate(fcolor);
                }
            }
            else
            {
                ProcessColorEliminate(color);
            }
        }

        private void ProcessColorEliminate(ItemColor color)
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

            List<Int2> tmpList = new List<Int2>();
            List<M3Item> itemList = M3GameManager.Instance.GetAllSameColorItem(color);
            for (int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i] != null)
                {
                    tmpList.Add(new Int2(itemList[i].itemInfo.posX, itemList[i].itemInfo.posY));
                    //itemList[i].OnSpecialCrush(ItemSpecial.fColor);
                }
            }
            ProcessSpecialList(tmpList, ItemSpecial.fColor, x1, y1, hasLawn);
        }

    }
}