namespace Game.Match3
{
    /// <summary>
    /// 检测消除
    /// </summary>
    public class M3CheckAndCrushState : State<GameFSM>
    {
        public bool needToCrush = true;
        private int idleCount;

        public override void Enter(GameFSM entityType)
        {
            base.Enter(entityType);

            needToCrush = M3GameManager.Instance.CheckNeedToCrush();
            if (needToCrush)
            {
                //Debug.Log("from  " + M3GameManager.Instance.gameFsm.GetFSM().PreviousState().ToString());
                M3GameManager.Instance.GoCrush();
                M3GameManager.Instance.fishManager.CheckPieceSwitchedDrop();
            }

            idleCount = 0;
        }

        public override void Execute(GameFSM entityType)
        {
            base.Execute(entityType);

            if (M3GridManager.Instance.IsAllElementStopTweening() && M3GridManager.Instance.IsAllElementLanded() && !M3GridManager.Instance.dropLock)
            {
                if (needToCrush)
                {
                    M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.CheckAndCrush);
                }
                else
                {
                    idleCount++;
                    if (idleCount >= 0)
                    {
                        M3GameManager.Instance.GetMatchResult();
                        if (M3GameManager.Instance.CheckNeedToCrush())
                        {
                            M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.CheckAndCrush);
                        }
                        else
                        {
                            M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.RoundLogic);
                            if (!M3GameManager.Instance.comboManager.comboLock)
                            {
                                M3GameManager.Instance.comboManager.CommitCombo();
                                M3GameManager.Instance.fishManager.CheckDelayFish();
                            }
                        }
                    }
                }
            }
        }

        public override void Exit(GameFSM entityType)
        {
            base.Exit(entityType);
            M3GameManager.Instance.ClearSelectedItem();
            if (M3GameManager.Instance.hiddenManager != null)
                M3GameManager.Instance.hiddenManager.CrushHiddens();
        }

    }
}
