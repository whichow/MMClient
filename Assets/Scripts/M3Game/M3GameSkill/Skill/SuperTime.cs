
namespace Game.Match3
{
    /// <summary>
    /// 释放技能后3个回合内/5秒内进入Super Time，得分变为N倍
    /// </summary>
    public class SuperTime : M3SkillBase
    {
        public SuperTime()
        {
            skillType = (int)ESkillType.SuperTime;
            skillOperationType = SkillOperationType.None;
        }

        public override void OnUseSkill(M3UseSkillArgs args)
        {
            base.OnUseSkill(args);
            M3GameManager.Instance.modeManager.EnterSuperTime(m_cat.GetSkillParam());
            M3FxManager.Instance.PlaySuperTimeEffect("SuperTime", true);
            AfterUseSkill();
        }
    }
}