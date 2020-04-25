using System;
using System.Collections.Generic;

namespace Game.Match3
{
    public class PropBase
    {
        public PropType propType;
        protected List<M3Item> hightList;
        public Action callBack;

        public PropBase(Action action)
        {
            hightList = new List<M3Item>();
            callBack = action;
        }

        public virtual void OnPropClick()
        {
            M3GameManager.Instance.propItemLock = true;
            M3GameManager.Instance.propItem = this;
        }

        public virtual void OnItemClick(int x, int y)
        {
            M3Supporter.Instance.ResetPiece();

        }

        public virtual void OnItemUsed()
        {
            M3GameManager.Instance.propItemLock = false;
            M3GameManager.Instance.propItem = null;
            M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.CheckAndCrush);
            if (callBack != null)
                callBack();
            //for (int i = 0; i < hightList.Count; i++)
            //{
            //    Game.TransformUtils.SetLayer(hightList[i].itemView.itemGameobject, LayerMask.NameToLayer("Default"));
            //}
            //hightList.Clear();
            OnItemStart();
        }

        public virtual void OnCancelUse()
        {
            M3GameManager.Instance.propManager.CancelUseProp();
            //for (int i = 0; i < hightList.Count; i++)
            //{
            //    Game.TransformUtils.SetLayer(hightList[i].itemView.itemGameobject, LayerMask.NameToLayer("Default"));
            //}
            //hightList.Clear();
        }

        public virtual void OnItemStart()
        {

        }

    }
}