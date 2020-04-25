using UnityEngine;

namespace Game.Match3
{
    public class M3IdleState : State<GameFSM>
    {
        private int idleCount;

        public override void Enter(GameFSM entityType)
        {
            base.Enter(entityType);
            if (M3GameManager.Instance.catManager != null && M3GameManager.Instance.catManager.skillCallBack != null && M3GameManager.Instance.catManager.skillCallBack.Count > 0)
            {
                var list = M3GameManager.Instance.catManager.skillCallBack;
                for (int i = 0; i < list.Count; i++)
                {
                    list[i]();
                }
                M3GameManager.Instance.catManager.skillCallBack.Clear();
                M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.Skill);
            }
            else
            {
                if (!M3GameManager.Instance.modeManager.isGameEnd)
                {
                    M3Supporter.Instance.CheckMoveStateEnter();
                }

                if (M3GameManager.Instance.modeManager.mode == GameModeEnum.StepMode)
                {
                    if (M3GameManager.Instance.modeManager.IsStepModeLevelEnd() && !M3GameManager.Instance.modeManager.isGameEnd)
                    {
                        Debuger.Log("End To Bonus");
                        M3GameManager.Instance.modeManager.EndLevel();
                    }
                    if (M3GameManager.Instance.bonusManager != null && M3GameManager.Instance.bonusManager.needPreBoom)
                    {
                        M3GameManager.Instance.bonusManager.needPreBoom = false;
                        M3GameManager.Instance.bonusManager.ShowBonusAnimation();
                    }
                    if (M3GameManager.Instance.modeManager.isBonusEnd)
                    {
                        M3GameManager.Instance.modeManager.ShowGameOver();
                    }
                }
            }
            idleCount = 0;
            M3GameManager.Instance.isRoundComplete = true;
        }

        public override void Execute(GameFSM entityType)
        {
            base.Execute(entityType);
            idleCount++;
            if (idleCount <= 1) return;

            bool isAllStopMove = M3GridManager.Instance.IsAllElementStopTweening();
            if (isAllStopMove && !M3GridManager.Instance.dropLock)
            {
                if (M3GameManager.Instance.ScreenLock && (!M3GameManager.Instance.modeManager.isGameEnd || !M3GameManager.Instance.modeManager.IsStepModeLevelEnd()))
                {
                    M3GameManager.Instance.ScreenLock = false;
                }
            }
            if (M3GameManager.Instance.ScreenLock|| !isAllStopMove)
            {
                return;
            }
            if (M3GameManager.Instance.modeManager.mode == GameModeEnum.TimeMode)
            {
                if (M3GameManager.Instance.modeManager.IsTimeModeLevelEnd() && !M3GameManager.Instance.modeManager.isGameEnd)
                {
                    Debuger.Log("game over");
                    M3GameManager.Instance.modeManager.EndLevel();
                    if (!M3GameManager.Instance.isAutoAI)
                    {
                        M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.GameOver);
                    }
                }
            }
            if (idleCount == M3Const.HintMoveTime && !M3GameManager.Instance.modeManager.isGameEnd)
            {
                M3Supporter.Instance.ShowMoveHint();
            }
            if (idleCount < 300)
            {
                idleCount++;
            }
            else
            {
                idleCount += 2;
            }
        }

        public override void Exit(GameFSM entityType)
        {
            base.Exit(entityType);
            M3GameManager.Instance.ScreenLock = true;
        }

    }
}
