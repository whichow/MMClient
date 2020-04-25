
using System;
using System.Collections.Generic;

namespace Game.Match3
{
    /// <summary>
    /// 主动向棋盘中发散N个随机颜色的礼物盒
    /// </summary>
    public class SendGift : M3SkillBase
    {
        public SendGift()
        {
            skillType = (int)ESkillType.SendGift;
            skillOperationType = SkillOperationType.None;
        }

        public override bool CheckCanUseSkill()
        {
            bool b = true;
            int skillParam = m_cat.GetSkillParam();
            if (itemList.Count < skillParam)
            {
                b = false;
            }
            return b;
        }

        protected override List<M3Item> GetSkillAffectItemList()
        {
            return M3GameManager.Instance.GetAllNormalElement();
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

            List<M3Item> targetList = GetSkillTargetItemList();
            for (int i = 0; i < targetList.Count; i++)
            {
                int index = i;
                var vec1 = M3GameManager.Instance.catManager.GetSkillRootPosition();
                var vec2 = targetList[index].itemView.itemTransform.position;

                Action callBack = delegate ()
                {
                    ((NormalElement)targetList[index].itemInfo.GetElement()).ChangeToGift();
                    if (index == targetList.Count - 1)
                    {
                        FrameScheduler.instance.Add(20, AfterUseSkill);
                    }
                    PlaySkillBoom(KFxBehaviour.skillBoom1, targetList[index]);
                };

                PlaySkillEffect(KFxBehaviour.skillEffect1, vec1, vec2, callBack);
            }
        }

    }
}