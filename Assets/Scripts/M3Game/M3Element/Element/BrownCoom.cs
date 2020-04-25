using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Match3
{
    /// <summary>
    /// 褐怪物
    /// </summary>
    public class BrownCoom : Element
    {
        public bool needDivide = false;
        private bool isDividing = false;

        public override void Init(int id, M3Item item)
        {
            base.Init(id, item);
            eName = M3ElementType.BrownCoomElement;
            isObstacle = true;
        }

        public override void Crush()
        {
            base.Crush();
            if (needDivide)
                return;
            if (itemObtainer.isCrushing)
                return;
            if (eSpecial == ItemSpecial.fNormal)
                return;
            if (eSpecial == ItemSpecial.Prop)
            {
                Divide();
            }
             ((M3CheckBrownCoomState)M3GameManager.Instance.gameFsm.GetFSM().GetStateInstance(StateEnum.CheckBrownCoom)).haveChecked = false;
            needDivide = true;
        }

        public override void ProcessSpecialEliminate(ItemSpecial special, object[] args, bool ignoreEffect = false)
        {
            base.ProcessSpecialEliminate(special, args, ignoreEffect);
            eSpecial = special;
            Crush();
        }

        public override void ProcessNeighborEliminate(int sx, int sy)
        {
            if (data.CanScrapingEliminate())
                Crush();
            else
            {
                if (isDividing || itemObtainer.isCrushing)
                    return;
                if (view != null)
                {
                    view.PlayAnimation((string)data.config.Animations["Shield"], delegate ()
                 {
                     view.PlayAnimation(data.config.IdleAnim, true);
                 }, true);
                    if (M3GameManager.Instance.soundManager != null)
                        M3GameManager.Instance.soundManager.PlayEleminateCoin();
                }
            }
        }

        private void AddScore()
        {
            int score = M3GameManager.Instance.modeManager.ProcessComboScore(data.config.Point, ItemSpecial.fNormal, M3GameManager.Instance.comboManager.GetCombo(), false);
            M3GameManager.Instance.modeManager.AddScore(score, 1, M3Supporter.Instance.GetItemPositionByGrid(itemObtainer.itemInfo.posX, itemObtainer.itemInfo.posY));
        }

        public override void Jump()
        {
            if (isJumping || itemObtainer.isCrushing)
            {
                return;
            }
            List<Int2> list = new List<Int2>();
            M3Item item;
            for (int i = 0; i < 4; i++)
            {
                Int2 point = new Int2(itemObtainer.itemInfo.posX + M3Const.DirectionOffset[i].x, itemObtainer.itemInfo.posY + M3Const.DirectionOffset[i].y);

                if (M3GameManager.Instance.CheckValid(point.x, point.y))
                {
                    item = M3ItemManager.Instance.gridItems[point.x, point.y];
                    if (item != null && item.itemInfo.GetElement() != null && item.itemInfo.GetElement().eName != M3ElementType.GreyCoomElement && item.itemInfo.GetElement().eName != M3ElementType.BrownCoomElement && item.itemInfo.GetElement().data.IsBaseElement())
                        list.Add(point);
                }
            }

            if (list.Count > 0)
            {
                isJumping = true;
                Int2 point2 = list[M3Supporter.Instance.GetRandomInt(0, list.Count)];
                Int2 lastPos = new Int2(itemObtainer.itemInfo.posX, itemObtainer.itemInfo.posY);

                itemObtainer.itemInfo.RemoveHighestElement();
                itemObtainer = M3ItemManager.Instance.gridItems[point2.x, point2.y];
                itemObtainer.itemInfo.AddElement(this);
                if (view != null)
                {
                    view.PlayAnimation((string)data.config.Animations["Jump"], true);
                    view.eleTransform.SetParent(itemObtainer.itemView.elementRoot);
                    view.transform.localScale = Vector3.one;
                    Vector3 vec = new Vector3(0, 0, view.transform.localPosition.z);
                    M3FxManager.Instance.PlayCoomJump(view.eleTransform, vec, null);
                }

                FrameScheduler.instance.Add(M3Config.CoomJumpFrame, delegate ()
                {
                    if (view != null)
                    {
                        this.view.RefreshView(itemObtainer.itemView.itemTransform);
                        view.transform.localScale = Vector3.one;
                        view.PlayAnimation(data.config.IdleAnim, true);
                    }
                    isJumping = false;
                });
            }
        }

        public void Divide()
        {
            if (isDividing)
            {
                return;
            }
            List<Int2> list = new List<Int2>();
            M3Item item;
            for (int i = 0; i < 4; i++)
            {
                Int2 point = new Int2(itemObtainer.itemInfo.posX + M3Const.DirectionOffset[i].x, itemObtainer.itemInfo.posY + M3Const.DirectionOffset[i].y);

                if (M3GameManager.Instance.CheckValid(point.x, point.y))
                {
                    item = M3ItemManager.Instance.gridItems[point.x, point.y];
                    if (item != null && !item.isCrushing && item.itemInfo.GetElement() != null && item.itemInfo.GetElement().eName != M3ElementType.GreyCoomElement && item.itemInfo.GetElement().eName != M3ElementType.BrownCoomElement && item.itemInfo.GetElement().data.IsBaseElement())
                        list.Add(point);
                }
            }
            isDividing = true;
            itemObtainer.isCrushing = true;
            int transfromId = data.config.ClearTransforID;

            if (list.Count > 0)
            {
                Int2 point2 = list[M3Supporter.Instance.GetRandomInt(0, list.Count)];

                Int2 lastPos = new Int2(itemObtainer.itemInfo.posX, itemObtainer.itemInfo.posY);


                Action action = delegate ()
                {
                    var e = itemObtainer.itemInfo.GetElement();
                    itemObtainer.itemInfo.EliminateElement(this);
                    itemObtainer.AddElement(transfromId);
                    var obtainer2 = M3ItemManager.Instance.gridItems[point2.x, point2.y];
                    obtainer2.AddElement(transfromId);
                    itemObtainer.isCrushing = false;
                    isDividing = false;
                    e.DestroyElement();
                };
                if (view != null)
                {
                    if (lastPos.x == point2.x)
                    {
                        if (point2.y > lastPos.y)
                            PlayDivideAnimation(M3Direction.Rigth, null);
                        else
                            PlayDivideAnimation(M3Direction.Left, null);
                    }
                    else if (lastPos.y == point2.y)
                    {
                        if (point2.x > lastPos.x)
                            PlayDivideAnimation(M3Direction.Bottom, null);
                        else
                            PlayDivideAnimation(M3Direction.Top, null);
                    }
                }
                FrameScheduler.instance.Add(M3Config.brownCoomFenlieFrame, action);
            }
            else
            {
                Action action = delegate ()
                {
                    var e = itemObtainer.itemInfo.GetElement();
                    itemObtainer.itemInfo.EliminateElement(this);
                    itemObtainer.AddElement(transfromId);
                    itemObtainer.isCrushing = false;
                    isDividing = false;
                    e.DestroyElement();
                };
                action();
            }
            AddScore();
            if (M3GameManager.Instance.soundManager != null)
                M3GameManager.Instance.soundManager.PlayEleminateCoom();
        }

        public void PlayDivideAnimation(M3Direction dir, Action action)
        {
            switch (dir)
            {
                case M3Direction.Top:
                    view.PlayAnimation(data.GetAnimationsByKey(M3Const.CoomDivisionAnimationKeys[0]));
                    break;
                case M3Direction.Bottom:
                    view.PlayAnimation(data.GetAnimationsByKey(M3Const.CoomDivisionAnimationKeys[1]));
                    break;
                case M3Direction.Left:
                    view.PlayAnimation(data.GetAnimationsByKey(M3Const.CoomDivisionAnimationKeys[2]));
                    break;
                case M3Direction.Rigth:
                    view.PlayAnimation(data.GetAnimationsByKey(M3Const.CoomDivisionAnimationKeys[3]));
                    break;
                default:
                    break;
            }

        }

        public override Element Clone()
        {
            Element ele = new BrownCoom();
            return Clone(ele);
        }

    }
}