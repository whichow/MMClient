
namespace Game.Match3
{
    /// <summary>
    /// 灰怪物
    /// </summary>
    public class M3CheckCoomState : State<GameFSM>
    {
        public bool haveChecked = true;
        private bool hold;

        public override void Enter(GameFSM entityType)
        {
            base.Enter(entityType);
            this.hold = false;
            if (this.haveChecked)
            {
                M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.CheckBrownCoom);
            }
        }

        public override void Execute(GameFSM entityType)
        {
            base.Execute(entityType);
            if (this.hold) return;

            if (this.haveChecked)
            {
                M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.CheckBrownCoom);
            }
            else
            {
                this.haveChecked = true;
                this.hold = true;
                
                bool flag = M3GameManager.Instance.CheckGreyCoom();
                //Debug.Log("Grey " + flag);

                FrameScheduler.Instance().Add((!flag) ? 0 : M3Const.StateWaitFrame_2, delegate
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
