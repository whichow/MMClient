using System;

namespace Game.Match3
{
    public class Refresh : PropBase
    {
        public Refresh(Action action) : base(action)
        {
            propType = PropType.Refresh;
        }

        public override void OnPropClick()
        {
            base.OnPropClick();
            M3Supporter.Instance.RefreshAllPiece();
            OnItemUsed();
        }

    }
}