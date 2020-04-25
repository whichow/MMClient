/** 
 *FileName:     M3AddEnergyState.cs 
 *Author:       HASEE 
 *Version:      1.0 
 *UnityVersion：5.6.2f1
 *Date:         2017-10-28 
 *Description:    
 *History: 
*/

namespace Game.Match3
{
    /// <summary>
    /// 能量符散发处理
    /// </summary>
    public class M3AddEnergyState : State<GameFSM>
    {
        public bool haveChecked = true;
        private bool hold;

        public override void Enter(GameFSM entityType)
        {
            base.Enter(entityType);
            if (this.haveChecked)
            {
                M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.Idle);
            }
            this.hold = false;
        }

        public override void Execute(GameFSM entityType)
        {
            base.Execute(entityType);
            if (this.hold)
            {
                return;
            }
            if (this.haveChecked)
            {
                M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.Idle);
            }
            else
            {
                this.haveChecked = true;
                this.hold = true;
                int flag = M3GameManager.Instance.modeManager.GameModeCtrl.CheckSendEnergyElement();
                M3GameManager.Instance.comboManager.comboLock = false;
                FrameScheduler.Instance().Add((flag == 0) ? 0 : M3Const.StateWaitFrame_1, delegate
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
