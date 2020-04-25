namespace Game.Match3
{
    /// <summary>
    /// 覆盖元素，如牢笼
    /// </summary>
    public class CoverElement : Element
    {

        public override void Init(int id, M3Item item)
        {
            base.Init(id, item);
            isObstacle = true;
            eName = M3ElementType.LockElement;
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
            AddScore();
            //obtainer.isCrushing = true;
            itemObtainer.itemInfo.EliminateElement(this);
            if (view != null)
            {
                M3ItemManager.Instance.CacheItem(view.gameObject, true);
                view.PlayAnimation(data.config.ClearAnim);
            }
            FrameScheduler.instance.Add(M3Config.ElementDisapperFrame, () => { DestroyElement(); });
            if (M3GameManager.Instance.soundManager != null)
                M3GameManager.Instance.soundManager.PlayEleminateLock();
        }

        private void AddScore()
        {
            int score = M3GameManager.Instance.modeManager.ProcessComboScore(data.config.Point, ItemSpecial.fNormal, M3GameManager.Instance.comboManager.GetCombo(), false);
            M3GameManager.Instance.modeManager.AddScore(score, 1, M3Supporter.Instance.GetItemPositionByGrid(itemObtainer.itemInfo.posX, itemObtainer.itemInfo.posY));
        }

        public override Element Clone()
        {
            CoverElement ele = new CoverElement();
            return Clone(ele);
        }

    }
}