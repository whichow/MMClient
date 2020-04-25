using System;

namespace Game.Match3
{
    /// <summary>
    /// 银币  会阻挡消除特效，比如会挡住消一排的特效
    /// </summary>
    public class CoinElement : Element
    {
        public override void Init(int id, M3Item item)
        {
            base.Init(id, item);
            eName = M3ElementType.CoinElement;
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
            FrameScheduler.instance.Add(M3Config.ElementDisapperFrame, action);
            if (view != null)
                view.PlayAnimation(data.config.ClearAnim);
            AddScore();
            if (M3GameManager.Instance.soundManager != null)
                M3GameManager.Instance.soundManager.PlayEleminateCoin();
        }

        private void AddScore()
        {
            int score = M3GameManager.Instance.modeManager.ProcessComboScore(data.config.Point, eSpecial, M3GameManager.Instance.comboManager.GetCombo(), false);
            M3GameManager.Instance.modeManager.AddScore(score, 1, M3Supporter.Instance.GetItemPositionByGrid(itemObtainer.itemInfo.posX, itemObtainer.itemInfo.posY));
        }

        public override Element Clone()
        {
            CoinElement ele = new CoinElement();
            return Clone(ele);
        }

    }
}