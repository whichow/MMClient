namespace Game.Match3
{
    /// <summary>
    /// 特效元素，如红色横特效
    /// </summary>
    public class SpecialElement : NormalElement
    {
        public override void Init(int id, M3Item item)
        {
            base.Init(id, item);
            eName = M3ElementType.SpecialElement;
        }

        public override void OnDisappear()
        {
            base.OnDisappear();
            if (!crushWithOutSpecial)
            {
                ItemColor color = GetColor();
                ElementSpecial special = GetSpecial();
                itemObtainer.ProcessSpecial(special, color);
            }
            if (M3GameManager.Instance.soundManager != null)
            {
                ElementSpecial special = GetSpecial();
                switch (special)
                {
                    case ElementSpecial.None:
                        break;
                    case ElementSpecial.Row:
                        M3GameManager.Instance.soundManager.PlayEliminateLineSpecialAudio();
                        break;
                    case ElementSpecial.Column:
                        M3GameManager.Instance.soundManager.PlayEliminateLineSpecialAudio();

                        break;
                    case ElementSpecial.Area:
                        M3GameManager.Instance.soundManager.PlayEliminateWrapSpecialAudio();

                        break;
                    case ElementSpecial.Color:
                        M3GameManager.Instance.soundManager.PlayEliminateColorSpecialAudio();

                        break;
                    default:
                        break;
                }
            }
        }

        public override void ProcessColorCrush()
        {
            itemObtainer.isCrushing = true;
            FrameScheduler.instance.Add(60, delegate ()
            {
                itemObtainer.isCrushing = false;
                this.ProcessSpecialEliminate(ItemSpecial.fColor, null);
            });
        }

        public override void OnCreate()
        {
            base.OnCreate();
            if (!M3GameManager.Instance.isAutoAI)
                M3FxManager.Instance.Special(view.gameObject, GetSpecial());
        }

        public override Element Clone()
        {
            var specialEle = new SpecialElement();
            return this.Clone(specialEle);
        }

    }
}