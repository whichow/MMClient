
using System;
using System.Collections.Generic;

namespace Game.Match3
{
    /// <summary>
    /// 发射交叉特效，直接引爆，并返还一定能量
    /// </summary>
    public class Cross : M3SkillBase
    {
        public Cross()
        {
            skillType = (int)ESkillType.Cross;
            skillOperationType = SkillOperationType.PointTouch;
        }

        public override bool CheckCanUseSkill()
        {
            return itemList.Count > 0;
        }

        protected override List<M3Item> GetSkillAffectItemList()
        {
            List<M3Item> list = new List<M3Item>();
            M3Item tmp;
            for (int i = 0; i < M3Config.GridHeight; i++)
            {
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    tmp = M3ItemManager.Instance.gridItems[i, j];
                    if (tmp != null && tmp.itemInfo.GetElement() != null
                        && tmp.itemInfo.GetElement().eName != M3ElementType.FishElement
                        && tmp.itemInfo.GetElement().isObstacle == false
                        && tmp.itemInfo.GetElement().data.IsBaseAndeSpecialElement())
                    {
                        list.Add(tmp);
                    }
                }
            }
            return list;
        }

        public override void OnUseSkill(M3UseSkillArgs args)
        {
            base.OnUseSkill(args);

            M3Item item = GetRandomItem();
            var vec1 = M3GameManager.Instance.catManager.GetSkillRootPosition();
            var vec2 = item.itemView.itemTransform.position;

            Action callBack = delegate ()
            {
                var list = M3GameManager.Instance.GetCrossItem(item.itemInfo.posX, item.itemInfo.posY);
                for (int i = 0; i < list.Count; i++)
                {
                    M3Item itemTmp = M3ItemManager.Instance.gridItems[list[i].x, list[i].y];
                    if (itemTmp != null)
                    {
                        var ele = itemTmp.itemInfo.GetElement();
                        itemTmp.OnSpecialCrush(ItemSpecial.Skill, null, true);
                    }
                }
                PlaySkillBoom(KFxBehaviour.skillBoom1, item);
                AfterUseSkill();
            };

            PlaySkillEffect(KFxBehaviour.skillEffect1, vec1, vec2, callBack);
        }

        public override void AfterUseSkill()
        {
            base.AfterUseSkill();
            M3GameManager.Instance.catManager.AddEnergy(m_cat.GetSkillParam());
        }

    }
}


