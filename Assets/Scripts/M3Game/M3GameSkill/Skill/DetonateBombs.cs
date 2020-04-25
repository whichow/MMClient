using System;
using System.Collections.Generic;

namespace Game.Match3
{
    /// <summary>
    /// 引爆棋盘中所有炸弹，并返还N个直线炸弹
    /// </summary>
    public class DetonateBombs : M3SkillBase
    {
        public DetonateBombs()
        {
            skillType = (int)ESkillType.DetonateBombs;
            skillOperationType = SkillOperationType.None;
        }

        public override bool CheckCanUseSkill()
        {
            return itemList.Count > 0;
        }

        protected override List<M3Item> GetSkillAffectItemList()
        {
            return M3GameManager.Instance.GetAllSpecialElement();
        }

        protected override List<M3Item> GetSkillTargetItemList()
        {
            return itemList;
        }

        public override void OnUseSkill(M3UseSkillArgs args)
        {
            base.OnUseSkill(args);

            M3GridManager.Instance.dropLock = true;

            var targetList = itemList;
            for (int i = 0; i < targetList.Count; i++)
            {
                int index = i;
                var vec1 = M3GameManager.Instance.catManager.GetSkillRootPosition();
                var vec2 = targetList[index].itemView.itemTransform.position;

                FrameScheduler.instance.Add(5 * index, delegate ()
                {
                    Action callBack = delegate ()
                    {
                        if (targetList[index].itemInfo.GetElement() is MagicCatElement)
                            targetList[index].OnSpecialCrush(ItemSpecial.other);
                        else
                            targetList[index].OnSpecialCrush(ItemSpecial.fNormal, null, true);
                        if (index == targetList.Count - 1)
                        {
                            FrameScheduler.instance.Add(20, AfterUseSkill);
                        }
                        PlaySkillBoom(KFxBehaviour.skillBoom1, targetList[index]);
                    };
                    PlaySkillEffect(KFxBehaviour.skillEffect1, vec1, vec2, callBack);
                });
            }

            Reward();
        }

        public override void AfterUseSkill()
        {
            base.AfterUseSkill();
            M3GridManager.Instance.dropLock = false;
        }

        private void Reward()
        {
            int count = m_cat.GetSkillParam();
            M3GameManager.Instance.catManager.skillCallBack.Add(delegate ()
            {
                var cList = M3GameManager.Instance.GetAllNormalElement();

                for (int i = 0; i < count; i++)
                {
                    int tmpIndex = M3Supporter.Instance.GetRandomInt(0, cList.Count);
                    ElementSpecial s;
                    int tmpIndex2 = M3Supporter.Instance.GetRandomInt(0, 2);
                    switch (tmpIndex2)
                    {
                        case 0:
                            s = ElementSpecial.Row;
                            break;
                        case 1:
                            s = ElementSpecial.Column;
                            break;
                        default:
                            s = ElementSpecial.Row;
                            break;
                    }
                    if (cList[tmpIndex] != null)
                    {
                        int tempI = i;
                        int index = tmpIndex;
                        var itemTmp = cList[index];
                        var vec1 = M3GameManager.Instance.catManager.GetSkillRootPosition();
                        var vec2 = cList[index].itemView.itemTransform.position;

                        Action callBack = delegate ()
                        {
                            ((NormalElement)itemTmp.itemInfo.GetPartakeEliminateElement()).ChangeToSpecial(s);
                            if (tempI == count - 1)
                            {
                                M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.CheckAndCrush);
                            }
                        };
                        PlaySkillEffect(KFxBehaviour.skillEffect2, -vec1, vec2, callBack);

                        //M3FxManager.Instance.PlayerSkillInfectArrow(vec1, new Vector3(vec2.x, vec2.y, vec1.z), 0.2f,
                        //   M3GameManager.Instance.catManager.skillRoot.transform,
                        //   delegate ()
                        //   {
                        //   }
                        //   );
                        cList.RemoveAt(index);
                    }
                    else
                    {
                        M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.CheckAndCrush);
                    }
                }
            });
        }

    }
}