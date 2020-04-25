/** 
*FileName:     M3CheckCatSkillState.cs 
*Author:        
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2017-12-25 
*Description:    
*History: 
*/

namespace Game.Match3
{
    /// <summary>
    /// 猫释放技能
    /// </summary>
    public class M3CheckCatSkillState : State<GameFSM>
    {
        bool isHold = false;

        public override void Enter(GameFSM entityType)
        {
            base.Enter(entityType);
            isHold = false;
        }

        public override void Execute(GameFSM entityType)
        {
            base.Execute(entityType);
            if (isHold) return;

            if (M3GameManager.Instance.catManager.hasCat && !M3GameManager.Instance.modeManager.IsStepModeLevelEnd())
            {
                isHold = true;
                if (M3GameManager.Instance.catManager.OnTryToUseSkill(null))
                {
                    //连放技能时（放完技能消除元素能量又长满了可以再次放技能）防止从CheckEnergy切换到Idle状态导致不能再次放技能
                    //((M3AddEnergyState)M3GameManager.Instance.gameFsm.GetFSM().GetStateInstance(StateEnum.CheckEnergy)).haveChecked = false;
                }
                else
                {
                    M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.CheckEnergy);
                }
            }
            else
            {
                M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.CheckEnergy);
            }
        }

        public override void Exit(GameFSM entityType)
        {
            base.Exit(entityType);
        }

    }
}