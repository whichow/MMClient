using System;

namespace Game.Match3
{
    public class Hammer : PropBase
    {
        public Hammer(Action action) : base(action)
        {
            propType = PropType.Hammer;
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
            //        if (tmp != null && tmp.itemInfo.GetElement() != null&&
            //            tmp.itemInfo.GetElement().eName!=M3ElementType.FishElement
            //             &&tmp.itemInfo.GetElement().eName !=M3ElementType.VenomParentElement
            //            )
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
            if (clickItem != null && clickItem.itemInfo.GetElement() != null && clickItem.itemInfo.GetElement().data.CanPropEliminate())
            {
                var vec = M3Supporter.Instance.GetItemPositionByGrid(x, y);
                M3FxManager.Instance.PlayerHammerEffect(vec.x, vec.y);
                M3GameManager.Instance.propItem = null;
                FrameScheduler.instance.Add(30, delegate ()
                {
                    M3GameManager.Instance.ShakeGrid();
                    M3GameManager.Instance.propItem = null;
                    clickItem.OnSpecialCrush(ItemSpecial.Prop, null, true);
                    ((M3CheckBrownCoomState)M3GameManager.Instance.gameFsm.GetFSM().GetStateInstance(StateEnum.CheckBrownCoom)).haveChecked = false;
                    ((M3CheckCatteryState)M3GameManager.Instance.gameFsm.GetFSM().GetStateInstance(StateEnum.CheckCattery)).haveChecked = false;
                    ((M3CheckWoolBallState)M3GameManager.Instance.gameFsm.GetFSM().GetStateInstance(StateEnum.CheckWoolBall)).haveChecked = false;
                    OnItemUsed();
                });
            }
        }

    }
}