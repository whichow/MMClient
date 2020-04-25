
namespace Game.Match3
{
    public class OperationBase
    {
        protected M3SkillBase skill;
        protected KCat cat;
        protected int mode = 1;

        public OperationBase(M3SkillBase s, KCat c)
        {
            skill = s;
            cat = c;
        }

        /// <summary>
        /// 开始使用技能
        /// </summary>
        public virtual void OnSkillStart()
        {
            M3GameManager.Instance.skillOperation = this;
            M3GameManager.Instance.skillLock = true;
            M3Supporter.Instance.ResetPiece();
            M3GameManager.Instance.catManager.CatBehaviour.SkillPre(false, delegate (KCatBehaviour.State currentState)
            {
                skill.OnUseSkill(new M3UseSkillArgs(cat));
            });
        }

        /// <summary>
        /// 技能使用完成
        /// </summary>
        public virtual void OnSkillUsed()
        {
            M3GameManager.Instance.skillOperation = null;
            M3GameManager.Instance.skillLock = false;
            M3GameManager.Instance.catManager.ClearEnergy();
            M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(Game.Match3.StateEnum.Skill);
        }

        /// <summary>
        /// 技能取消使用
        /// </summary>
        public virtual void OnSkillCancel()
        {
            skill.OnCancelUse();
        }

    }
}