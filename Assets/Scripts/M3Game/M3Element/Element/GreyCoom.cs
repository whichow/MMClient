using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Match3
{
    /// <summary>
    /// 灰怪物
    /// </summary>
    public class GreyCoom : Element
    {

        public override void Init(int id, M3Item item)
        {
            base.Init(id, item);
            isObstacle = true;
            eName = M3ElementType.GreyCoomElement;
        }

        public override void Crush()
        {
            base.Crush();
            DoLogic();
        }

        public override void ProcessSpecialEliminate(ItemSpecial special, object[] args, bool ignoreEffect = false)
        {
            base.ProcessSpecialEliminate(special, args, ignoreEffect);
            Crush();
        }

        private void DoLogic()
        {
            if (itemObtainer.isCrushing)
                return;
            itemObtainer.isCrushing = true;
            Action action = delegate
            {
                itemObtainer.isCrushing = false;
                itemObtainer.itemInfo.EliminateElement(this);
                if (itemObtainer.itemInfo.CheckEmpty())
                {
                    int x = itemObtainer.itemInfo.posX;
                    int y = itemObtainer.itemInfo.posY;
                    itemObtainer.RemoveFrom(x, y);
                    itemObtainer.ItemDestroy();
                }
            };
            if (view != null)
                view.PlayAnimation(data.config.ClearAnim, delegate ()
                {
                    DestroyElement();
                });
            AddScore();
            FrameScheduler.instance.Add(M3Config.ElementDisapperFrame, action);
            if (M3GameManager.Instance.soundManager != null)
                M3GameManager.Instance.soundManager.PlayEleminateCoom();
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
                    view.PlayAnimation((string)data.config.Animations["Jump"]/*, data.config.idleAnim*/);
                    view.eleTransform.SetParent(itemObtainer.itemView.itemTransform);
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

        public override Element Clone()
        {
            var ele = new GreyCoom();
            return Clone(ele);
        }

    }
}