/** 
 *FileName:     M3CheckJumpState.cs 
 *Author:       HeJunJie 
 *Version:      1.0 
 *UnityVersion：5.6.2f1
 *Date:         2018-01-04 
 *Description:    
 *History: 
*/

namespace Game.Match3
{
    /// <summary>
    /// 元素跳跃
    /// </summary>
    public class M3CheckJumpState : State<GameFSM>
    {
        public bool haveChecked = true;
        private bool hold;

        public override void Enter(GameFSM entityType)
        {
            base.Enter(entityType);
            this.hold = false;
            if (this.haveChecked)
            {
                M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.CheckCattery);
            }
        }

        public override void Execute(GameFSM entityType)
        {
            base.Execute(entityType);
            if (this.hold) return;

            if (this.haveChecked)
            {
                M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.CheckCattery);
            }
            else
            {
                this.haveChecked = true;
                this.hold = true;
                bool flag = M3GameManager.Instance.CheckJump();
                FrameScheduler.Instance().Add((!flag) ? 0 : M3Const.StateWaitFrame_3, delegate
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
