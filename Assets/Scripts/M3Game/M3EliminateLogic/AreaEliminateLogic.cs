/** 
 *FileName:     AreaEliminateLogic.cs 
 *Author:       HeJunJie 
 *Version:      1.0 
 *UnityVersion：5.6.2f1
 *Date:         2017-07-18 
 *Description:    
 *History: 
*/
using System.Collections.Generic;

namespace Game.Match3
{
    /// <summary>
    /// 区域消除逻辑 （炸弹 13消）
    /// </summary>
    public class AreaEliminateLogic : EliminateBase
    {
        Int2[] boomArray = new Int2[]{
            new Int2(-1,0),
            new Int2(0,-1),
            new Int2(0,1),
            new Int2(1,0),
            new Int2(-1,-1),
            new Int2(-1,1),
            new Int2(1,-1),
            new Int2(1,1),
            new Int2(-2,0),
            new Int2(0,-2),
            new Int2(0,2),
            new Int2(2,0),
        };

        public override void ProcessEliminateLogic(params object[] args)
        {
            base.ProcessEliminateLogic(args);
            int x = int.Parse(args[0].ToString());
            int y = int.Parse(args[1].ToString());
            if (M3ItemManager.Instance.gridItems[x, y] == null)
                return;

            // 先检测触发点是否有草坪，有的话所有消除组都铺
            bool hasLawn = EliminateManager.Instance.CheckHasLawn(x, y);

            List<Int2> list = new List<Int2>();
            M3ItemInfo info = M3ItemManager.Instance.gridItems[x, y].itemInfo;
            list.Add(new Int2(info.posX, info.posY));
            for (int i = 0; i < boomArray.Length; i++)
            {
                int xx = x + boomArray[i].x;
                int yy = y + boomArray[i].y;
                if (M3GameManager.Instance.CheckValid(xx, yy))
                {
                    list.Add(new Int2(xx, yy));
                }
            }
            ProcessSpecialList(list, ItemSpecial.fArea, x, y, hasLawn);
        }
    }
}