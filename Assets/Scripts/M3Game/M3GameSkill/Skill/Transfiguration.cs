using System;
using System.Collections.Generic;

namespace Game.Match3
{
    /// <summary>
    /// 转变技能  使指定位置的普通元素变成相应猫的颜色，且变色了的猫拥有得分暴击能力（该猫被消除时，得分*N）
    /// </summary>
    public class Transfiguration : M3SkillBase
    {
        public Transfiguration()
        {
            skillType = (int)ESkillType.Transfiguration;
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
                        && tmp.itemInfo.GetElement().eName == M3ElementType.NormalElement)
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

            Action callBack = delegate ()
            {
                NormalElement ele = (NormalElement)item.itemInfo.GetElement();
                ele.ChangeToOtherColor(M3GameManager.Instance.catManager.GetCatColor());
                ele = (NormalElement)item.itemInfo.GetElement();
                ele.AddCrit(m_cat.GetSkillParam());
                if (M3GameManager.Instance.catManager.needView)
                {
                    PlaySkillBoom(KFxBehaviour.skillBoom1, item);
                }
                AfterUseSkill();
            };

            if (M3GameManager.Instance.catManager.needView)
            {
                var vec1 = M3GameManager.Instance.catManager.GetSkillRootPosition();
                var vec2 = item.itemView.itemTransform.position;
                PlaySkillEffect(KFxBehaviour.skillEffect1, vec1, vec2, callBack);
            }
            else
            {
                callBack();
            }
        }

        //public override List<Int2> GetOperationElementPos()
        //{
        //    M3Item tmp;
        //    operationList.Clear();
        //    for (int i = 0; i < M3Config.GridHeight; i++)
        //    {
        //        for (int j = 0; j < M3Config.GridWidth; j++)
        //        {
        //            tmp = M3ItemManager.Instance.gridItems[i, j];
        //            if (tmp != null && tmp.itemInfo.GetElement() != null
        //                && tmp.itemInfo.GetElement().eName == M3ElementType.NormalElement)
        //            {
        //                operationList.Add(new Int2(i, j));
        //            }
        //        }
        //    }
        //    return operationList;
        //}

    }
}