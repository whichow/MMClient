
namespace Game.Match3
{
    public class M3CheckConveyorState : State<GameFSM>
    {
        private bool hold;

        public override void Enter(GameFSM entityType)
        {
            base.Enter(entityType);
            hold = false;
            if (M3GameManager.Instance.conveyorManager.noConveyor || M3GameManager.Instance.conveyorManager.haveMovedThisRound)
            {
                M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.CheckCoom);
            }
        }

        public override void Execute(GameFSM entityType)
        {
            base.Execute(entityType);

            if (this.hold) return;

            if (M3GameManager.Instance.conveyorManager.noConveyor || M3GameManager.Instance.conveyorManager.haveMovedThisRound)
            {
                M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.CheckCoom);
            }
            else
            {
                if (!M3GameManager.Instance.modeManager.isGameEnd)
                {
                    M3GameManager.Instance.conveyorManager.MoveConveyor();
                }
                this.hold = true;
                FrameScheduler.Instance().Add(M3Const.StateWaitFrame_1, delegate
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
