using System;

namespace Game.Match3
{
    /// <summary>
    /// 能量块
    /// </summary>
    public class EnergyBottle : NormalElement
    {
        public override void Init(int id, M3Item item)
        {
            base.Init(id, item);
            eName = M3ElementType.EnergyElement;
        }

        public override void Crush()
        {
            base.Crush();
            if (itemObtainer.isCrushing)
                return;

            itemObtainer.isCrushing = true;
            OnDisappear();
        }

        public override void ProcessSpecialEliminate(ItemSpecial special, object[] args, bool ignoreEffect = false)
        {
            Crush();
        }

        public override void OnDisappear()
        {
            if (itemObtainer.GetGrid() != null && itemObtainer.GetGrid().gridInfo.spawnPointType == DropPointType.SpawnPoint)
                itemObtainer.GetGrid().portDropSpeed = 0;
            Action action = delegate
            {
                SendEnergy();
                itemObtainer.isCrushing = false;
                itemObtainer.itemInfo.EliminateElement(this);

                if (itemObtainer.itemInfo.CheckEmpty())
                {
                    int x = itemObtainer.itemInfo.posX;
                    int y = itemObtainer.itemInfo.posY;
                    itemObtainer.RemoveFrom(x, y);
                    itemObtainer.ItemDestroy();
                }
                DestroyElement();
            };
            FrameScheduler.instance.Add(M3Config.ElementDisapperFrame, action);
            SendEnergy();

            if (view != null)
            {
                view.PlayAnimation(data.config.ClearAnim);
                M3FxManager.Instance.PlayBottleCrashEffect(itemObtainer.coordinate.x, itemObtainer.coordinate.y);
                KUIWindow.GetWindow<Game.UI.M3GameUIWindow>().PlayBottleLiziEffect(view.eleTransform.position, 1);

            }
            if (M3GameManager.Instance.soundManager != null)
                M3GameManager.Instance.soundManager.PlayEleminateBottle();
        }

        private void SendEnergy()
        {
            M3GameManager.Instance.catManager.AddEnergy(M3Const.EnergyBottleValue);
        }

        public override Element Clone()
        {
            var bottleEle = new EnergyBottle();
            return Clone(bottleEle);
        }

    }
}