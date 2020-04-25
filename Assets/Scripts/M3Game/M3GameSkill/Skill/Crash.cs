using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Match3
{
    /// <summary>
    /// 消除所有当前棋盘的相应颜色基础猫，并有一定几率返还随机炸弹（点头、摇头、13消、活力猫）
    /// </summary>
    public class Crash : M3SkillBase
    {
        public Crash()
        {
            skillType = (int)ESkillType.Crash;
            skillOperationType = SkillOperationType.None;
        }

        public override bool CheckCanUseSkill()
        {
            return itemList.Count > 0;
        }

        protected override List<M3Item> GetSkillAffectItemList()
        {
            return M3GameManager.Instance.GetSameBaseColorItem(M3GameManager.Instance.catManager.GetCatColor());
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

                Action callBack = delegate ()
                {
                    targetList[index].OnSpecialCrush(ItemSpecial.fNormal, null, true);
                    if (index == targetList.Count - 1)
                    {
                        FrameScheduler.instance.Add(20, AfterUseSkill);
                    }
                    PlaySkillBoom(KFxBehaviour.skillBoom1, targetList[index]);
                };

                FrameScheduler.instance.Add(5 * index, delegate ()
                {
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
            int r = M3Supporter.Instance.GetRandomInt(0, 100);
            if (r <= M3GameManager.Instance.catManager.GameCat.GetSkillParam())
            {
                M3GameManager.Instance.catManager.skillCallBack.Add(delegate ()
                {
                    var cList = M3GameManager.Instance.GetAllNormalElement();
                    int tmpIndex = M3Supporter.Instance.GetRandomInt(0, cList.Count);
                    ElementSpecial s;
                    int tmpIndex2 = M3Supporter.Instance.GetRandomInt(0, 4);
                    switch (tmpIndex2)
                    {
                        case 0:
                            s = ElementSpecial.Row;
                            break;
                        case 1:
                            s = ElementSpecial.Column;
                            break;
                        case 2:
                            s = ElementSpecial.Area;
                            break;
                        case 3:
                            s = ElementSpecial.Color;
                            break;
                        default:
                            s = ElementSpecial.Row;
                            break;
                    }
                    if (cList[tmpIndex] != null)
                    {
                        var vec1 = M3GameManager.Instance.catManager.GetSkillRootPosition();
                        var vec2 = cList[tmpIndex].itemView.itemTransform.position;
                        M3GameManager.Instance.catManager.EffectLauncher.DoEffect(new object[] { m_cat.GetSkillEffectName(KFxBehaviour.skillEffect2),
                                new Vector3(vec2.x, vec2.y, vec1.z), vec2, vec1 }, delegate ()
                                {
                                    ((NormalElement)cList[tmpIndex].itemInfo.GetPartakeEliminateElement()).ChangeToSpecial(s);
                                    M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.CheckAndCrush);
                                }
                        );
                    }
                    else
                    {
                        M3GameManager.Instance.gameFsm.GetFSM().ChangeGameState(StateEnum.CheckAndCrush);
                    }
                });
            }
        }

    }
}