namespace Game.Match3
{
    public class M3GameTimeMode : M3GameMode
    {
        public int totalTime = 0;
        public int nextTime = 1;
        public float nextTimer = 0;
        public float signTime = 20;
        private float superTime = -1;

        public override void Init(M3GameModeManager m, M3LevelData data, GameTargetEnum target)
        {
            base.Init(m, data, target);
            superTime = -1;
            totalTime = data.time;
            nextTimer = nextTime;
        }

        public override void AddTime(int num)
        {
            base.AddTime(num);
            if (num > 0)
            {
                totalTime += num;
                manager.isGameEnd = false;
                M3GameEvent.DispatchEvent(M3FightEnum.DoTimer, M3Supporter.Instance.TransformTimer(totalTime));
                M3GameManager.Instance.ScreenLock = false;
            }
        }

        public override void ProcessSteps(int step)
        {
            base.ProcessSteps(step);
        }

        public override void OnEnterSuperTime()
        {
            base.OnEnterSuperTime();
            superTime = 5.0f;
            isSuperTime = true;
        }

        public override void Execute(float time)
        {
            base.Execute(time);
            if (manager.timeOut)
                return;
            nextTimer -= time;
            if (superTime >= 0)
            {
                superTime -= time;
                if (superTime < 0)
                {
                    isSuperTime = false;
                    superTime = -1;
                }
            }

            if (nextTimer <= 0)
            {
                totalTime -= nextTime;
                if (totalTime < 0)
                {
                    if (!manager.timeOut)
                    {
                        manager.timeOut = true;
                    }
                }
                else
                {
                    M3GameEvent.DispatchEvent(M3FightEnum.DoTimer, M3Supporter.Instance.TransformTimer(totalTime));
                    nextTimer = nextTime;
                }
            }
        }

    }
}