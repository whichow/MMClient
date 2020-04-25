using System;
using System.Collections.Generic;

namespace Game.Match3
{
    public class MagicBroom : PropBase
    {
        private int lineCount = 2;

        public MagicBroom(Action action) : base(action)
        {
            propType = PropType.MagicBroom;
        }

        public override void OnPropClick()
        {
            base.OnPropClick();
            GetBottomLineClear();
            OnItemUsed();
        }

        public void GetBottomLineClear()
        {
            List<M3Item> eliminateList = new List<M3Item>();
            M3Item tmp;
            for (int i = M3Config.GridHeight - 1; i > M3Config.GridHeight - 1 - lineCount; i--)
            {
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    tmp = M3ItemManager.Instance.gridItems[i, j];
                    if (tmp != null)
                    {
                        eliminateList.Add(tmp);
                    }
                }
            }
            for (int i = 0; i < eliminateList.Count; i++)
            {
                eliminateList[i].OnSpecialCrush(ItemSpecial.fNormal, null, true);
            }
        }

    }
}