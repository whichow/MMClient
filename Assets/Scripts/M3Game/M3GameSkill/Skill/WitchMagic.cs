
using System.Collections.Generic;

namespace Game.Match3
{
    /// <summary>
    /// 魔力扫把（N行）
    /// </summary>
    public class WitchMagic : M3SkillBase
    {
        public WitchMagic()
        {
            skillType = (int)ESkillType.WitchMagic;
            skillOperationType = SkillOperationType.None;
        }

        public override void OnUseSkill(M3UseSkillArgs args)
        {
            base.OnUseSkill(args);
            GetBottomLineClear(m_cat.GetSkillParam());
            AfterUseSkill();
        }

        public void GetBottomLineClear(int rowCount)
        {
            List<M3Item> eliminateList = new List<M3Item>();
            M3Item tmp;
            for (int i = M3Config.GridHeight - 1; i > M3Config.GridHeight - 1 - rowCount; i--)
            {
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    tmp = M3ItemManager.Instance.gridItems[i, j];
                    if (tmp != null)
                    {
                        eliminateList.Add(tmp);
                    }
                }
            }
            for (int i = 0; i < eliminateList.Count; i++)
            {
                eliminateList[i].OnSpecialCrush(ItemSpecial.fNormal, null, true);
            }
        }

    }
}