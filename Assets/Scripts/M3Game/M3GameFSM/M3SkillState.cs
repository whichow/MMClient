
namespace Game.Match3
{
    public class M3SkillState : State<GameFSM>
    {

        public override void Enter(GameFSM entityType)
        {
            base.Enter(entityType);
            M3GameManager.Instance.ScreenLock = true;
        }

        public override void Execute(GameFSM entityType)
        {
            base.Execute(entityType);
        }

        public override void Exit(GameFSM entityType)
        {
            base.Exit(entityType);
        }

    }
}
