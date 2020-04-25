using System;
using UnityEngine;

namespace Game.Match3
{
    /// <summary>
    /// 礼物盒
    /// </summary>
    public class GiftElement : NormalElement
    {

        public override void Init(int id, M3Item item)
        {
            base.Init(id, item);
            eName = M3ElementType.GiftElement;
        }

        public override void Crush()
        {
            if (itemObtainer.isCrushing)
                return;
            itemObtainer.isCrushing = true;
            if (itemObtainer.GetGrid() != null && itemObtainer.GetGrid().gridInfo.spawnPointType == DropPointType.SpawnPoint)
                itemObtainer.GetGrid().portDropSpeed = 0;
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
                //Debug.Log("获取礼物");
                DestroyElement();
            };
            if (view != null)
                view.PlayAnimation(data.config.ClearAnim);
            FrameScheduler.instance.Add(M3Config.ElementDisapperFrame, action);
            if (M3GameManager.Instance.soundManager != null)
                M3GameManager.Instance.soundManager.PlayEleminateGift();
        }

        public override void ProcessSpecialEliminate(ItemSpecial special, object[] args, bool ignoreEffect = false)
        {
            Crush();
        }

        public override Element Clone()
        {
            var ele = new GiftElement();
            return Clone(ele);
        }

    }
}