using System;
using System.Collections.Generic;

namespace Game.Match3
{
    /// <summary>
    /// 发射活力猫，直接引爆，并返还一定比例的能量
    /// </summary>
    public class MagicBomb : M3SkillBase
    {
        public MagicBomb()
        {
            skillType = (int)ESkillType.MagicBomb;
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
                        && (tmp.itemInfo.GetElement().data.IsBaseElement() || tmp.itemInfo.GetElement().data.IsSpecialElement())
                        )
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

            if (item.itemInfo.GetElement().data.IsBaseElement())
            {
                Action callBack = delegate ()
                {
                    EliminateManager.Instance.ProcessEliminate(ItemSpecial.fColor, item.itemInfo.posX, item.itemInfo.posY, item.itemInfo.GetElement().data.GetColor());
                    PlaySkillBoom(KFxBehaviour.skillBoom1, item);
                    AfterUseSkill();
                };
                PlaySkillEffect(KFxBehaviour.skillEffect1, vec1, vec2, callBack);
            }
            else if (item.itemInfo.GetElement().data.IsSpecialElement())
            {
                Action callBack = delegate ()
                {
                    var special = item.itemInfo.GetElement().GetSpecial();
                    item.itemInfo.GetElement().crushWithOutSpecial = true;
                    switch (special)
                    {
                        case ElementSpecial.None:
                            break;
                        case ElementSpecial.Row:
                            EliminateManager.Instance.ProcessSpecial2ColorEliminate(ItemSpecial.fLine2Color, -1, -1, item.itemInfo.posX, item.itemInfo.posY, 0, item.itemInfo.GetPartakeEliminateElement().GetColor());
                            break;
                        case ElementSpecial.Column:
                            EliminateManager.Instance.ProcessSpecial2ColorEliminate(ItemSpecial.fLine2Color, -1, -1, item.itemInfo.posX, item.itemInfo.posY, 0, item.itemInfo.GetPartakeEliminateElement().GetColor());
                            break;
                        case ElementSpecial.Area:
                            EliminateManager.Instance.ProcessSpecial2ColorEliminate(ItemSpecial.fArea2Color, -1, -1, item.itemInfo.posX, item.itemInfo.posY, 0, item.itemInfo.GetPartakeEliminateElement().GetColor());
                            break;
                        case ElementSpecial.Color:
                            EliminateManager.Instance.ProcessDoubleColorEliminate(ItemSpecial.fDoubleColor, -1, -1, item.itemInfo.posX, item.itemInfo.posY);
                            break;
                        default:
                            break;
                    }
                    PlaySkillBoom(KFxBehaviour.skillBoom1, item);
                };
                PlaySkillEffect(KFxBehaviour.skillEffect1, vec1, vec2, callBack);
                AfterUseSkill();
            }
        }

        public override void AfterUseSkill()
        {
            base.AfterUseSkill();
            M3GameManager.Instance.catManager.AddEnergy(m_cat.GetSkillParam());
        }

    }
}