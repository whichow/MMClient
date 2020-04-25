using System.Collections.Generic;

namespace Game.Match3
{
    public class M3SkillManager
    {
        private Dictionary<ESkillType, M3SkillBase> skillDic;

        public M3SkillManager()
        {
            Init();
        }

        public void Init()
        {
            skillDic = new Dictionary<ESkillType, M3SkillBase>();
            RegisteredSkill();
        }

        public void RegisteredSkill()
        {
            skillDic.Add(ESkillType.Transfiguration, new Transfiguration());
            skillDic.Add(ESkillType.Infect, new Infect());
            skillDic.Add(ESkillType.Crash, new Crash());
            skillDic.Add(ESkillType.DetonateBombs, new DetonateBombs());
            skillDic.Add(ESkillType.SendGift, new SendGift());
            skillDic.Add(ESkillType.SuperTime, new SuperTime());
            skillDic.Add(ESkillType.AddStepAndTime, new AddStepAndTime());
            skillDic.Add(ESkillType.WitchMagic, new WitchMagic());
            skillDic.Add(ESkillType.Cross, new Cross());
            skillDic.Add(ESkillType.AreaBomb, new AreaBomb());
            skillDic.Add(ESkillType.MagicBomb, new MagicBomb());
            skillDic.Add(ESkillType.ShockWave, new ShockWave());
            skillDic.Add(ESkillType.Missile, new Missile());
            skillDic.Add(ESkillType.ChangeSpecial, new SkillChangeSpecial());
        }

        public M3SkillBase GetSkillEntity(ESkillType id)
        {
            if (skillDic.ContainsKey(id))
            {
                return skillDic[id];
            }
            else
            {
                Debuger.Log("没有注册技能ID :" + id);
                return null;
            }
        }

    }
}