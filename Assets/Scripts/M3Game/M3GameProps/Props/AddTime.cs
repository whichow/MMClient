using System;

namespace Game.Match3
{
    public class AddTime : PropBase
    {
        public AddTime(Action action) : base(action)
        {
            propType = PropType.Time;
        }

        public override void OnPropClick()
        {
            base.OnPropClick();
            M3GameManager.Instance.modeManager.GameModeCtrl.AddTime(5);
            OnItemUsed();
        }

    }
}