using System;

namespace Game.Match3
{
    public class MagicRow : PropBase
    {
        public MagicRow(Action action) : base(action)
        {
            propType = PropType.MagicRow;
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
            //        if (tmp != null && tmp.itemInfo.GetElement() != null && tmp.itemInfo.GetElement().data.IsBaseElement() && tmp.itemInfo.GetElement() is NormalElement)
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
            if (clickItem != null && clickItem.itemInfo.GetElement() != null && clickItem.itemInfo.GetElement().data.IsBaseElement() && clickItem.itemInfo.GetElement() is NormalElement)
            {
                M3GameManager.Instance.propItem = null;
                ((NormalElement)clickItem.itemInfo.GetPartakeEliminateElement()).ChangeToSpecial(ElementSpecial.Row);
                OnItemUsed();
            }
        }

    }
}