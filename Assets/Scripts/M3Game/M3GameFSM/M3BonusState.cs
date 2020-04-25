namespace Game.Match3
{
    /// <summary>
    /// 结算
    /// </summary>
    public class M3BonusState : State<GameFSM>
    {
        public override void Enter(GameFSM entityType)
        {
            base.Enter(entityType);
        }

        public override void Execute(GameFSM entityType)
        {
            base.Execute(entityType);
            if (M3GridManager.Instance.IsAllElementStopTweening() && M3GridManager.Instance.IsAllElementLanded() && !M3GridManager.Instance.dropLock)
            {
                if (M3GameManager.Instance.runningSpecialCount == 0)
                {
                    M3GameManager.Instance.modeManager.isBonusEnd = true;
                    M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.Idle);
                }
            }
        }

        public override void Exit(GameFSM entityType)
        {
            base.Exit(entityType);
        }

    }
}
