
namespace Game.Match3
{
    /// <summary>
    /// 步数增加N步/时间增加N秒
    /// </summary>
    public class AddStepAndTime : M3SkillBase
    {
        public AddStepAndTime()
        {
            skillType = (int)ESkillType.AddStepAndTime;
            skillOperationType = SkillOperationType.None;
        }

        public override void OnUseSkill(M3UseSkillArgs args)
        {
            base.OnUseSkill(args);
            if (M3GameManager.Instance.modeManager.mode == GameModeEnum.StepMode)
            {
                M3GameManager.Instance.modeManager.GameModeCtrl.AddStep(m_cat.GetSkillParam());
            }
            else if (M3GameManager.Instance.modeManager.mode == GameModeEnum.TimeMode)
            {
                M3GameManager.Instance.modeManager.GameModeCtrl.AddTime(m_cat.GetSkillParam());
            }
            AfterUseSkill();
        }

    }
}