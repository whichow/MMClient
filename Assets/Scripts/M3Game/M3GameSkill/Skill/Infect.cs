
using System;
using System.Collections.Generic;

namespace Game.Match3
{
    /// <summary>
    /// 随机向当前棋盘内发散N个相应颜色猫
    /// </summary>
    public class Infect : M3SkillBase
    {
        public Infect()
        {
            skillType = (int)ESkillType.Infect;
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
            ItemColor color = M3GameManager.Instance.catManager.GetCatColor();
            return M3GameManager.Instance.GetAllNormalElement(color);
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
                if (targetList[i].itemInfo.GetElement() is NormalElement)
                {
                    int index = i;
                    var vec1 = M3GameManager.Instance.catManager.GetSkillRootPosition();
                    var vec2 = targetList[index].itemView.itemTransform.position;

                    Action callBack = delegate ()
                    {
                        ItemColor color = M3GameManager.Instance.catManager.GetCatColor();
                        ((NormalElement)targetList[index].itemInfo.GetElement()).ChangeToOtherColor(color);
                        if (index == targetList.Count - 1)
                        {
                            FrameScheduler.instance.Add(20, AfterUseSkill);
                        }
                        PlaySkillBoom(KFxBehaviour.skillBoom1, targetList[index]);
                    };

                    PlaySkillEffect(KFxBehaviour.skillEffect1, vec1, vec2, callBack);

                    //M3FxManager.Instance.PlayerSkillInfectArrow(vec1, new Vector3(vec2.x, vec2.y, vec1.z), 0.4f, 
                    //    M3GameManager.Instance.catManager.skillRoot.transform,
                    //    delegate (){
                    //    }
                    //    );
                }
            }
        }

    }
}