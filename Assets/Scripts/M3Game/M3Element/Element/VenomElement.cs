namespace Game.Match3
{
    /// <summary>
    /// 毒液
    /// </summary>
    public class VenomElement : ObstacleElement
    {

        public override void Init(int id, M3Item item)
        {
            base.Init(id, item);
            eName = M3ElementType.VenomElement;
        }

        public override void OnDisappear()
        {
            base.OnDisappear();
            M3GameManager.Instance.venomManager.haveVenomCrush = true;
            if (M3GameManager.Instance.soundManager != null)
                M3GameManager.Instance.soundManager.PlayEleminateVenom();
        }

        public void PlayFenLieAnimation(string animaName)
        {
            if (view == null)
                return;
            animaName = (string)data.config.Animations[animaName];
            view.PlayAnimation(animaName, false);
            FrameScheduler.instance.Add(M3Config.VenomFenlieFrame, delegate ()
            {
                view.PlayAnimation(data.config.IdleAnim, true);
            });
        }

        public override Element Clone()
        {
            Element ele = new VenomElement();
            return Clone(ele);
        }

    }
}