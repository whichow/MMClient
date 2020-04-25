
using Game.DataModel;
using System;
using System.Collections.Generic;

namespace Game.Match3
{
    /// <summary>
    /// 发射L型/十字特效，直接引爆，并返还一定比例的能量
    /// </summary>
    public class AreaBomb : M3SkillBase
    {
        public AreaBomb()
        {
            skillType = (int)ESkillType.AreaBomb;
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
                    EliminateManager.Instance.ProcessEliminate(ItemSpecial.fArea, item.itemInfo.posX, item.itemInfo.posY, ItemColor.fNone);
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
                    switch (special)
                    {
                        case ElementSpecial.None:
                            break;
                        case ElementSpecial.Row:
                            DoLine(item.itemInfo.posX, item.itemInfo.posY, true);
                            break;
                        case ElementSpecial.Column:
                            DoLine(item.itemInfo.posX, item.itemInfo.posY, false);
                            break;
                        case ElementSpecial.Area:
                            DoArea(item.itemInfo.posX, item.itemInfo.posY);
                            break;
                        case ElementSpecial.Color:
                            DoColor(item.itemInfo.posX, item.itemInfo.posY);
                            break;
                        default:
                            break;
                    }
                    PlaySkillBoom(KFxBehaviour.skillBoom1, item);
                    AfterUseSkill();
                };
                PlaySkillEffect(KFxBehaviour.skillEffect1, vec1, vec2, callBack);
            }
        }

        public override void AfterUseSkill()
        {
            base.AfterUseSkill();
            M3GameManager.Instance.catManager.AddEnergy(m_cat.GetSkillParam());
        }

        private void DoLine(int x, int y, bool isRow)
        {
            int tmpX = 2;
            int tmpY = 2;
            List<List<Int2>> list = new List<List<Int2>>();
            if (isRow)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (M3GameManager.Instance.CheckValid(x - tmpX, y))
                    {
                        list.Add(M3GameManager.Instance.GetLineAllItem(x - tmpX, y, true, true));
                    }
                    tmpX -= 1;
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    if (M3GameManager.Instance.CheckValid(tmpX, y - tmpY))
                    {
                        list.Add(M3GameManager.Instance.GetLineAllItem(tmpX, y - tmpY, false, true));
                    }
                    tmpY -= 1;
                }
            }
            M3Item item;
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list[i].Count; j++)
                {
                    item = M3ItemManager.Instance.gridItems[list[i][j].x, list[i][j].y];
                    if (item != null && item.itemInfo.GetElement() != null)
                        item.OnSpecialCrush(ItemSpecial.other);
                }
            }
        }

        private void DoArea(int x, int y)
        {
            List<Int2> list = new List<Int2>();
            M3Item itemTmp;
            for (int i = 0; i < M3Const.Boom41Array.Length; i++)
            {
                int xx = x + M3Const.Boom41Array[i].x;
                int yy = y + M3Const.Boom41Array[i].y;
                if (M3GameManager.Instance.CheckValid(xx, yy))
                {
                    list.Add(new Int2(xx, yy));
                }
            }

            for (int i = 0; i < list.Count; i++)
            {
                itemTmp = M3ItemManager.Instance.gridItems[list[i].x, list[i].y];
                if (itemTmp != null)
                {
                    itemTmp.OnSpecialCrush(ItemSpecial.other, null, true);
                }
            }
        }

        private void DoColor(int x, int y)
        {
            var fcolor = M3GameManager.Instance.GetCurrentGridRandomColors();
            M3Item colorItem = M3ItemManager.Instance.gridItems[x, y];
            Element cEle = colorItem.itemInfo.GetElement();
            cEle.view.PlayAnimation(cEle.data.config.ClearAnim);
            var itemList = M3GameManager.Instance.GetAllSameColorItem(fcolor);
            foreach (var item in itemList)
            {
                if (item.itemInfo.GetElement().GetSpecial() > 0)
                    continue;
                ((NormalElement)item.itemInfo.GetPartakeEliminateElement()).ChangeToSpecial(ElementSpecial.Area);
            }
            FrameScheduler.instance.Add(100, delegate ()
            {
                colorItem.OnSpecialCrush(ItemSpecial.fNormal, null, true);
                foreach (var item in itemList)
                {
                    item.OnSpecialCrush(ItemSpecial.other);
                }
                AfterUseSkill();
            });
        }

    }
}