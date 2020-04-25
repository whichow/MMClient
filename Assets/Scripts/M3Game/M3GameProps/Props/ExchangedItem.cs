using System;
using UnityEngine;

namespace Game.Match3
{
    public class ExchangedItem : PropBase
    {
        private M3Item firstItem;
        private GameObject firstFlag;

        public ExchangedItem(Action action) : base(action)
        {
            propType = PropType.Exchanged;
        }

        public override void OnPropClick()
        {
            base.OnPropClick();
            //hightList.Clear();
            //M3Item tmp;
            //for (int i = 0; i < M3Config.GridHeight; i++)
            //{
            //    for (int j = 0; j < M3Config.GridWidth; j++)
            //    {
            //        tmp = M3ItemManager.Instance.gridItems[i, j];
            //        if (tmp != null && tmp.itemInfo.GetElement() != null && tmp.itemInfo.GetElement() is NormalElement)
            //        {
            //            hightList.Add(tmp);
            //        }
            //    }
            //}
            //for (int i = 0; i < hightList.Count; i++)
            //{
            //    Game.TransformUtils.SetLayer(hightList[i].itemView.itemGameobject, LayerMask.NameToLayer("HightLight"));
            //}
        }

        public override void OnItemClick(int x, int y)
        {
            base.OnItemClick(x, y);
            M3Item clickItem = M3ItemManager.Instance.gridItems[x, y];
            if (firstItem == null && clickItem.itemInfo.GetElement() != null && clickItem.itemInfo.GetElement() is NormalElement)
            {
                firstItem = M3ItemManager.Instance.gridItems[x, y];
                M3GameManager.Instance.ActiveSelector(firstItem.itemView.itemTransform.position);
            }
            if (firstItem != null && clickItem.itemInfo.GetElement() != null && clickItem != firstItem && clickItem.itemInfo.GetElement() is NormalElement)
            {
                M3GameManager.Instance.HideSelector();
                ExchangeItem(firstItem, clickItem);
            }
        }

        public override void OnItemUsed()
        {
            base.OnItemUsed();
        }

        public override void OnItemStart()
        {
            base.OnItemStart();
        }

        private void ExchangeItem(M3Item firstItem, M3Item clickItem)
        {
            if (Mathf.Abs(firstItem.itemInfo.posX - clickItem.itemInfo.posX) + Mathf.Abs(firstItem.itemInfo.posY - clickItem.itemInfo.posY) > 1)
            {
                OnCancelUse();

                return;
            }
            Debug.Log("使用强制交换道具");

            M3GameManager.Instance.SwapItemPosition(firstItem, clickItem);
            M3GameManager.Instance.propItem = null;
            Vector3 target1 = firstItem.itemView.itemTransform.localPosition;
            Vector3 target2 = clickItem.itemView.itemTransform.localPosition;
            KTweenUtils.LocalMoveTo(firstItem.itemView.itemTransform, target2, M3Config.ItemMoveTime);
            KTweenUtils.LocalMoveTo(clickItem.itemView.itemTransform, target1, M3Config.ItemMoveTime, () => { this.OnItemUsed(); });
        }

    }
}