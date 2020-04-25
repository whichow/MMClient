/** 
 *FileName:     M3CheckFishState.cs 
 *Author:       HASEE 
 *Version:      1.0 
 *UnityVersion：5.6.2f1
 *Date:         2017-10-23 
 *Description:    
 *History: 
*/

namespace Game.Match3
{
    public class M3CheckFishState : State<GameFSM>
    {
        public bool haveChecked = true;
        private bool hold;

        public override void Enter(GameFSM entityType)
        {
            base.Enter(entityType);
            this.hold = false;
            if (this.haveChecked)
            {
                M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.CheckVenom);
            }
        }

        public override void Execute(GameFSM entityType)
        {
            base.Execute(entityType);
            if (this.hold) return;

            if (this.haveChecked)
            {
                M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.CheckVenom);
            }
            else
            {
                this.haveChecked = true;
                this.hold = true;
                bool flag = M3GameManager.Instance.fishManager.CheckFish();

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
