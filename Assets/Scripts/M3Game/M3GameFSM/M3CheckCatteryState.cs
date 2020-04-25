
namespace Game.Match3
{
    /// <summary>
    /// 猫窝
    /// </summary>
    public class M3CheckCatteryState : State<GameFSM>
    {
        public bool haveChecked = true;
        private bool hold;

        public override void Enter(GameFSM entityType)
        {
            base.Enter(entityType);
            this.hold = false;
            if (this.haveChecked)
            {
                M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.CheckWoolBall);
            }
        }

        public override void Execute(GameFSM entityType)
        {
            base.Execute(entityType);
            if (this.hold) return;

            if (this.haveChecked)
            {
                M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.CheckWoolBall);
            }
            else
            {
                this.haveChecked = true;
                this.hold = true;
                bool flag = M3GameManager.Instance.CheckCattery();

                FrameScheduler.Instance().Add((!flag) ? 0 : M3Const.StateWaitFrame_1, delegate
                {
                    M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.CheckAndCrush);
                });
            }
        }

        public override void Exit(GameFSM entityType)
        {
            base.Exit(entityType);
        }

    }
}
