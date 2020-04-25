/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/4/18 16:09:40
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using Game.Match3;
using System;
using System.Collections.Generic;

namespace Game
{
    /// <summary>
    /// 散发特效元素
    /// </summary>
    public class SkillChangeSpecial : M3SkillBase
    {
        #region C&D
        public SkillChangeSpecial()
        {
            skillType = (int)ESkillType.ChangeSpecial;
            skillOperationType = SkillOperationType.None;
        }
        #endregion

        public override bool CheckCanUseSkill()
        {
            bool b = true;
            int skillParam = m_cat.GetSkillParam();
            if (itemList.Count < skillParam)
            {
                //条件不满足
                b = false;
            }
            return b;
        }

        protected override List<M3Item> GetSkillAffectItemList()
        {
            ItemColor itemColor;
            KItemSkill skill = KItemManager.Instance.GetSkill(m_cat.skillId);
            switch (skill.SkillColorType)
            {
                case ESkillColorType.Green:
                    itemColor = ItemColor.fGreen;
                    break;
                case ESkillColorType.Blue:
                    itemColor = ItemColor.fBlue;
                    break;
                case ESkillColorType.Purple:
                    itemColor = ItemColor.fPurple;
                    break;
                case ESkillColorType.Brown:
                    itemColor = ItemColor.fBrown;
                    break;
                case ESkillColorType.Red:
                    itemColor = ItemColor.fRed;
                    break;
                case ESkillColorType.Yellow:
                    itemColor = ItemColor.fYellow;
                    break;
                case ESkillColorType.Energy:
                    itemColor = ItemColor.fEnergy;
                    break;
                case ESkillColorType.Coin:
                    itemColor = ItemColor.fCoin;
                    break;
                case ESkillColorType.Random:
                    int r = M3Supporter.Instance.GetRandomInt(1, 7);
                    itemColor = (ItemColor)r;
                    break;
                default:
                    itemColor = M3GameManager.Instance.catManager.GetCatColor();
                    break;
            }

            return M3GameManager.Instance.GetSameNormalColorItem(itemColor);
        }

        protected override List<M3Item> GetSkillTargetItemList()
        {
            int skillParam = m_cat.GetSkillParam();
            List<M3Item> changeList = new List<M3Item>();
            for (int i = 0; i < skillParam; i++)
            {
                var item = itemList[M3Supporter.Instance.GetRandomInt(0, itemList.Count)];
                itemList.Remove(item);
                changeList.Add(item);
            }
            return changeList;
        }

        public override void OnUseSkill(M3UseSkillArgs args)
        {
            base.OnUseSkill(args);

            KItemSkill skill = KItemManager.Instance.GetSkill(m_cat.skillId);
            List<M3Item> targetList = GetSkillTargetItemList();

            M3GridManager.Instance.dropLock = true;

            for (int i = 0; i < targetList.Count; i++)
            {
                int index = i;
                var vec1 = M3GameManager.Instance.catManager.GetSkillRootPosition();
                var vec2 = targetList[index].itemView.itemTransform.position;

                Action callBack = delegate ()
                {
                    int r = M3Supporter.Instance.GetRandomInt(0, skill.EleSpecial.Length);
                    ((NormalElement)targetList[index].itemInfo.GetPartakeEliminateElement()).ChangeToSpecial(skill.EleSpecial[r]);
                    if (index == targetList.Count - 1)
                    {
                        FrameScheduler.instance.Add(20, AfterUseSkill);
                    }
                    PlaySkillBoom(KFxBehaviour.skillBoom1, targetList[index]);
                };

                PlaySkillEffect(KFxBehaviour.skillEffect1, vec1, vec2, callBack);
            }
        }

        public override void AfterUseSkill()
        {
            base.AfterUseSkill();
            M3GridManager.Instance.dropLock = false;
        }

    }
}
