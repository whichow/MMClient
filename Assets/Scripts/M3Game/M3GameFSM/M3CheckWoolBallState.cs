/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/6/17 14:13:43
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/

namespace Game.Match3
{
    /// <summary>
    /// 毛线球处理
    /// </summary>
    public class M3CheckWoolBallState : State<GameFSM>
    {
        public bool haveChecked = true;
        private bool hold;

        public override void Enter(GameFSM entityType)
        {
            base.Enter(entityType);
            hold = false;
            if (haveChecked)
            {
                M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.CheckCrystal);
            }
        }

        public override void Execute(GameFSM entityType)
        {
            base.Execute(entityType);

            if (hold) return;

            if (haveChecked)
            {
                M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.CheckCrystal);
            }
            else
            {
                haveChecked = true;
                hold = true;
                bool flag = M3GameManager.Instance.CheckWoolBall();

                FrameScheduler.Instance().Add((!flag) ? 0 : M3Const.StateWaitFrame_1, delegate
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
