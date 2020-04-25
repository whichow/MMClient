
namespace Game.Match3
{
    public class M3GlobalState : State<GameFSM>
    {

        public override void Enter(GameFSM entityType)
        {
            base.Enter(entityType);
        }

        public override void Execute(GameFSM entityType)
        {
            base.Execute(entityType);

            FrameScheduler.Instance().Update();
            M3GridUpdate.Instance.RunUpdate();
            M3ItemUpdate.Instance.RunUpdate();
            M3GameManager.Instance.modeManager.Update();
        }

        public override void Exit(GameFSM entityType)
        {
            base.Exit(entityType);
        }

    }
}
