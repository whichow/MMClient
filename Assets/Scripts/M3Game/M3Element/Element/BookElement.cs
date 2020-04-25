namespace Game.Match3
{
    /// <summary>
    /// 障碍元素，如雪块，书，气球等
    /// </summary>
    public class BookElement : ObstacleElement
    {
        public override void Init(int id, M3Item item)
        {
            base.Init(id, item);
            eName = M3ElementType.MagicBookElement;
        }

        public override void OnDisappear()
        {
            base.OnDisappear();
            if (M3GameManager.Instance.soundManager != null)
                M3GameManager.Instance.soundManager.PlayEleminateBook();
        }

        protected override void AddScore()
        {
            int score = M3GameManager.Instance.modeManager.ProcessComboScore(data.config.Point, ItemSpecial.fNormal, M3GameManager.Instance.comboManager.GetCombo(), false);
            M3GameManager.Instance.modeManager.AddScore(score, 1, M3Supporter.Instance.GetItemPositionByGrid(itemObtainer.itemInfo.posX, itemObtainer.itemInfo.posY));
        }

        public override Element Clone()
        {
            var bookElement = new BookElement();
            return Clone(bookElement);
        }

    }
}