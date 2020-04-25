using UnityEngine;

namespace Game.Match3
{
    /// <summary>
    /// 三消步数模式
    /// </summary>
    public class M3GameStepMode : M3GameMode
    {
        private int signSetps = 5;
        private int superTimeStep = -1;

        public override void Init(M3GameModeManager m, M3LevelData data, GameTargetEnum target)
        {
            base.Init(m, data, target);
            superTimeStep = -1;
            totalSetps = data.steps + M3GameManager.Instance.propManager.GetPrepareStep();
        }

        public override void OnEnterSuperTime()
        {
            base.OnEnterSuperTime();
            isSuperTime = true;
            superTimeStep = 3;
        }

        public override void ProcessSteps(int step)
        {
            base.ProcessSteps(step);

            if (superTimeStep >= 0)
            {
                superTimeStep -= step;
                if (superTimeStep <= 0)
                {
                    isSuperTime = false;
                    if (!M3GameManager.Instance.isAutoAI)
                    {
                        M3FxManager.Instance.PlaySuperTimeEffect("SuperTime", false);
                    }
                }
            }

            totalSetps -= step;
            M3GameEvent.DispatchEvent(M3FightEnum.DoStep, totalSetps);
            if (totalSetps <= signSetps)
            {
            }

            totalSetps = Mathf.Max(0, totalSetps);
            if (totalSetps <= 0)
            {
                if (M3GameManager.Instance.isAutoAI)
                {
                    isEnd = false;
                    return;
                }

                Debuger.Log("步数用完");
                if (!isEnd)
                    isEnd = true;
            }
        }

        public override void AddStep(int num)
        {
            base.AddStep(num);
            if (num > 0)
            {
                totalSetps += num;
                manager.isGameEnd = false;
                M3GameEvent.DispatchEvent(M3FightEnum.DoStep, totalSetps);
                M3GameManager.Instance.ScreenLock = false;
            }
        }

        public override void Execute(float time)
        {
            base.Execute(time);
        }

        public override void ResetVars()
        {
            base.ResetVars();
        }

    }
}