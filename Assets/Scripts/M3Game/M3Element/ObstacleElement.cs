using Game.DataModel;
using System;

namespace Game.Match3
{
    /// <summary>
    /// 障碍物类型元素
    /// </summary>
    public class ObstacleElement : Element
    {
        public override void Init(int id, M3Item item)
        {
            base.Init(id, item);
            isObstacle = true;
        }

        public override void Crush()
        {
            base.Crush();
            if (itemObtainer.isCrushing)
                return;
            if (data.config.ClearTransforID != 0)
            {
                DoLogic();
            }
            else
            {
                OnDisappear();
            }
            AddScore();
        }

        public override void ProcessSpecialEliminate(ItemSpecial special, object[] args, bool ignoreEffect = false)
        {
            base.ProcessSpecialEliminate(special, args, ignoreEffect);
            Crush();
        }

        public override void ProcessNeighborEliminate(int sx, int sy)
        {
            base.ProcessNeighborEliminate(sx, sy);
        }

        public virtual void DoLogic()
        {
            var tarElement = XTable.ElementXTable.GetByID(data.config.ClearTransforID);
            if (view != null)
                view.PlayTweenAnim(data.config.ClearAnim, tarElement.IdleAnim);
            TransformElement(tarElement.ID);
        }

        public override void OnDisappear()
        {
            base.OnDisappear();
            itemObtainer.isCrushing = true;
            Action action = delegate
            {
                itemObtainer.itemInfo.EliminateElement(this);
                if (itemObtainer.itemInfo.CheckEmpty())
                {
                    int x = itemObtainer.itemInfo.posX;
                    int y = itemObtainer.itemInfo.posY;
                    itemObtainer.RemoveFrom(x, y);
                    itemObtainer.ItemDestroy();
                }
                itemObtainer.isCrushing = false;
                DestroyElement();
            };
            if (view != null)
                view.PlayAnimation(data.config.ClearAnim);
            if (eSpecial == ItemSpecial.other)
            {
                action();
            }
            else
            {
                FrameScheduler.instance.Add(M3Config.ElementDisapperFrame, action);
            }
        }

        protected virtual void AddScore()
        {
            int score = M3GameManager.Instance.modeManager.ProcessComboScore(data.config.Point, ItemSpecial.fNormal, M3GameManager.Instance.comboManager.GetCombo(), false);
            M3GameManager.Instance.modeManager.AddScore(score, 1, M3Supporter.Instance.GetItemPositionByGrid(itemObtainer.itemInfo.posX, itemObtainer.itemInfo.posY));
        }

    }
}