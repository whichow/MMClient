/** 
 *FileName:     RowEliminateLogic.cs 
 *Author:       HeJunJie 
 *Version:      1.0 
 *UnityVersion：5.6.2f1
 *Date:         2017-07-18 
 *Description:    
 *History: 
*/
using System.Collections.Generic;
using UnityEngine;

namespace Game.Match3
{
    public class RowEliminateLogic : EliminateBase
    {
        public override void ProcessEliminateLogic(object[] args)
        {
            base.ProcessEliminateLogic(args);
            int x = int.Parse(args[0].ToString());
            int y = int.Parse(args[1].ToString());

            Vector3 tmp = M3Supporter.Instance.GetItemPositionByGrid(x, y);
            if (!M3GameManager.Instance.isAutoAI)
                M3FxManager.Instance.FireArrow(tmp, false);

            List<Int2> itemList = M3GameManager.Instance.GetLineAllItem(x, y, true);
            ProcessSpecialList(itemList, ItemSpecial.fRow, x, y, false);
        }

    }
}