/** 
 *FileName:     EliminateBase.cs 
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
    /// 消除逻辑
    /// </summary>
    public class EliminateBase
    {
        /// <summary>
        /// 消除逻辑
        /// </summary>
        /// <param name="args"></param>
        public virtual void ProcessEliminateLogic(params object[] args)
        {

        }

        public void ProcessSpecialList(List<Int2> list, ItemSpecial special, int originPosX, int originPosY, bool pointerTargetHaveLawn)
        {
            // pointerTargetHaveLawn 先检测操作目标点是否有草坪，有的话所有消除组都从各自触发点四方向铺草坪，遇阻挡停止, 单组false就行
            if (special == ItemSpecial.fArea || special == ItemSpecial.fDoubleArea)
            {
                EliminateManager.Instance.AreaLayingLawnForSpecialEliminate(list, originPosX, originPosY, pointerTargetHaveLawn);
            }
            else if (special == ItemSpecial.fColor)
            {
                EliminateManager.Instance.ColorLayingLawnForSpecialEliminate(list, pointerTargetHaveLawn);
            }
            else
            {
                // 从触发点四方向铺草坪，找到草坪开始向后铺，遇阻挡停止
                EliminateManager.Instance.LayingLawnForSpecialEliminate(list, originPosX, originPosY, pointerTargetHaveLawn);
            }

            M3Item item = null;
            foreach (var pos in list)
            {
                item = M3ItemManager.Instance.gridItems[pos.x, pos.y];
                if (item != null && !item.isCrushing)
                {
                    //var ele = itemTmp.itemInfo.GetElement();
                    //if (CheckContainSpecial(itemTmp)&& ele!=null&& (ele.eName==M3ElementType.NormalElement||ele.eName==M3ElementType.SpecialElement))
                    //{
                    //    if (((NormalElement)ele).specialType != string.Empty)
                    //    {
                    //        ((NormalElement)ele).isHide = true;
                    //    }
                    //}

                    //Debuger.Log("+++++++++++++++++++");
                    //Debuger.Log(item.itemInfo.posX + "++" + item.itemInfo.posY);

                    item.OnSpecialCrush(special, new object[] { originPosX, originPosY });
                }
            }
        }

        private bool CheckContainSpecial(M3Item item)
        {
            var list = M3GameManager.Instance.matcher.GetSpecialList();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].position.x == item.itemInfo.posX && list[i].position.y == item.itemInfo.posY)
                {
                    return true;
                }
            }
            return false;
        }

    }
}