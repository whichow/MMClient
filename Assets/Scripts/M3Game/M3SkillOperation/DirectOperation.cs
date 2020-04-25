
namespace Game.Match3
{
    public class DirectOperation : OperationBase
    {
        public DirectOperation(M3SkillBase s, KCat c) : base(s, c)
        {
        }

        public override void OnSkillStart()
        {
            base.OnSkillStart();
            //skill.OnUseSkill(new M3UseSkillArgs(cat));

            //OnSkillUsed();
            //M3GameManager.Instance.catManager.catBehaviour.Skill(false, delegate (KCatBehaviour.State currentState) {
            //    M3GameManager.Instance.catManager.catBehaviour.Idle();
            //});
        }

        public override void OnSkillUsed()
        {
            base.OnSkillUsed();
        }

        public override void OnSkillCancel()
        {
            base.OnSkillCancel();
        }

    }
}