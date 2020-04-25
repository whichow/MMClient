using System.Collections.Generic;

namespace Game.Match3
{
    /// <summary>
    /// 颜色
    /// </summary>
    public enum ItemColor
    {
        fNone = 0,
        fRed = 1,
        fYellow = 2,
        fBlue = 3,
        fGreen = 4,
        fPurple = 5,
        fBrown = 6,
        fEnergy = 7,
        fCoin = 8,
        fRandom = 99,
    }

    /// <summary>
    /// 消除特效
    /// </summary>
    public enum ItemSpecial
    {
        fNormal = 0,
        /// <summary>
        /// 一行
        /// </summary>
        fRow = 1,
        /// <summary>
        /// 一列
        /// </summary>
        fColumn = 2,
        /// <summary>
        /// 区域（炸弹13消）
        /// </summary>
        fArea = 3,
        /// <summary>
        /// 颜色
        /// </summary>
        fColor = 4,
        /// <summary>
        /// 双列
        /// </summary>
        fDoubleCol = 10,
        /// <summary>
        /// 双行
        /// </summary>
        fDoubleRow = 11,
        /// <summary>
        /// 行加列
        /// </summary>
        fRowAndCol = 12,
        /// <summary>
        /// 行和区域
        /// </summary>
        fRow2Area = 13,
        /// <summary>
        /// 列和区域
        /// </summary>
        fCol2Area = 14,
        /// <summary>
        /// 双区域
        /// </summary>
        fDoubleArea = 15,
        /// <summary>
        /// 行或列和颜色
        /// </summary>
        fLine2Color = 16,
        /// <summary>
        /// 区域和颜色
        /// </summary>
        fArea2Color = 17,
        /// <summary>
        /// 双颜色
        /// </summary>
        fDoubleColor = 18,

        /// <summary>
        /// 技能
        /// </summary>
        Skill = 97,
        /// <summary>
        /// 道具
        /// </summary>
        Prop = 98,
        /// <summary>
        /// 其它
        /// </summary>
        other = 99,
    }


    /// <summary>
    /// 三消单元内元素管理
    /// </summary>
    [System.Serializable]
    public class M3ItemInfo
    {
        public int posX { get; private set; }
        public int posY { get; private set; }
        public List<Element> allElementList;

        public Int2 PosInt2
        {
            get { return new Int2(posX, posY); }
        }

        /// <summary>
        /// 是否可以下落
        /// </summary>
        public bool CanDrop
        {
            get
            {
                var ele = GetElement();
                if (ele != null && ele.data.config.CanDrop)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// 检测该单元内是否没有元素了
        /// </summary>
        /// <returns></returns>
        public bool CheckEmpty()
        {
            return allElementList == null || allElementList.Count == 0;
        }


        public void Init(int x, int y, List<Element> eleList, M3Item item)
        {
            posX = x;
            posY = y;
            allElementList = new List<Element>();
            for (int i = 0; i < eleList.Count; i++)
            {
                AddElement(eleList[i]);
            }
        }

        public void AddElement(Element ele)
        {
            if (!allElementList.Contains(ele))
            {
                int index = 0;
                for (int i = 0; i < allElementList.Count; i++)
                {
                    if (allElementList[i].data.config.Level < ele.data.config.Level)
                    {
                        index++;
                        continue;
                    }
                }
                allElementList.Insert(index, ele);
            }
            else
            {
                Debuger.LogError("[M3ItemInfo.AddElement] 添加重复元素 位置:" + posX + "|" + posY);
            }
        }

        /// <summary>
        /// 从列表中移除元素 添加到收集目标
        /// </summary>
        /// <param name="ele"></param>
        public void EliminateElement(Element ele)
        {
            if (RemoveElement(ele))
            {
                M3GameManager.Instance.modeManager.GameModeCtrl.AddTarget(ele.data.GetTargetId(), (ele.view != null) ? ele.itemObtainer.itemView.transform.position : UnityEngine.Vector3.zero, ele);
            }
            //if (allElementList.Contains(ele))
            //{
            //    M3GameManager.Instance.modeManager.GameModeCtrl.AddTarget(ele.data.GetTargetId(), (ele.view != null) ? ele.itemObtainer.itemView.transform.position : UnityEngine.Vector3.zero, ele);
            //    allElementList.Remove(ele);
            //}
            //else
            //{
            //    Debuger.LogError("[M3ItemInfo.EliminateElement] 移除错误 位置：" + posX + "|" + posY);
            //}
        }

        /// <summary>
        /// 从列表中移除最高层级的元素
        /// </summary>
        public void RemoveHighestElement()
        {
            RemoveElement(GetElement());
        }

        /// <summary>
        /// 消毁最高层级的元素
        /// </summary>
        public void DestroyHighestElement()
        {
            var ele = GetElement();
            RemoveElement(ele);
            ele.DestroyElement();
        }

        /// <summary>
        /// 消毁参与消除的元素
        /// </summary>
        public void DestroyPartakeElement()
        {
            var ele = GetPartakeEliminateElement();
            RemoveElement(ele);
            ele.DestroyElement();
        }

        /// <summary>
        /// 从列表中移除元素
        /// </summary>
        /// <param name="ele"></param>
        public bool RemoveElement(Element ele)
        {
            if (allElementList.Contains(ele))
            {
                allElementList.Remove(ele);
                return true;
            }
            else
            {
                Debuger.Log("[M3ItemInfo.RemoveElement] 移除错误 位置：" + posX + "|" + posY);
                return false;
            }
        }

        /// <summary>
        /// 获取最高层级的元素
        /// </summary>
        /// <returns></returns>
        public Element GetElement()
        {
            if (allElementList.Count > 0)
                return allElementList[allElementList.Count - 1];
            return null;
        }

        public Element GetElementByIndex(int index)
        {
            if (allElementList.Count > 0 && allElementList.Count >= index)
                return allElementList[allElementList.Count - index];
            return null;
        }

        /// <summary>
        /// 获取参与匹配消除的元素
        /// </summary>
        /// <returns></returns>
        public Element GetPartakeEliminateElement()
        {
            Element ele = GetElement();
            if (ele == null)
                return null;
            if (ele.eName == M3ElementType.LockElement)
                ele = GetElementByIndex(2);
            return ele;
        }

        /// <summary>
        /// 获取非覆盖类型元素
        /// </summary>
        /// <returns></returns>
        public Element GetWithOutCoverEliminateElement()
        {
            Element ele = GetElement();
            if (ele == null)
                return null;
            if (ele.eName == M3ElementType.LockElement)
                ele = GetElementByIndex(2);
            if (ele.eName == M3ElementType.GreyCoomElement || ele.eName == M3ElementType.BrownCoomElement)
                ele = GetElementByIndex(2);
            return ele;
        }

        /// <summary>
        /// 更新所在格子坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void RefreshPos(int x, int y)
        {
            if (x >= 0 && x < M3Config.GridHeight && y >= 0 && y < M3Config.GridWidth)
            {
                posX = x;
                posY = y;
            }
            else
            {
                Debuger.LogError("[M3ItemInfo.RefreshPos] 超出棋盘坐标 " + x + ":" + y);
            }
        }

        /// <summary>
        /// 是否有该类型的元素
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public bool HasType(List<int> types)
        {
            bool b = false;
            foreach (var item in allElementList)
            {
                if (types.Contains(item.data.config.Type))
                {
                    b = true;
                    break;
                }
            }
            return b;
        }

    }
}