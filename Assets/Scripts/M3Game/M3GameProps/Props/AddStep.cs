using System;

namespace Game.Match3
{
    public class AddStep : PropBase
    {
        public static int step = 5;

        public AddStep(Action action) : base(action)
        {
            propType = PropType.Step;
        }

        public override void OnPropClick()
        {
            base.OnPropClick();
            M3GameManager.Instance.modeManager.GameModeCtrl.AddStep(step);
            OnItemUsed();
        }

    }
}