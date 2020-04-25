using System;

namespace Game.Match3
{
    /// <summary>
    /// 魔力猫
    /// </summary>
    public class MagicCatElement : NormalElement
    {
        private ItemColor crushColor = ItemColor.fNone;
        private  int x1;
        private  int y1;
        private  int x2;
        private  int y2;

        public override void Init(int id, M3Item item)
        {
            base.Init(id, item);
            eName = M3ElementType.MagicCatElement;
        }

        public override void ProcessSpecialEliminate(ItemSpecial special, object[] args, bool ignoreEffect = false)
        {
            if (itemObtainer.isCrushing)
                return;

            if (args != null && args.Length > 4)
            {
                crushColor = (ItemColor)args[0];
                x1 = (int)args[1];
                y1 = (int)args[2];
                x2 = (int)args[3];
                y2 = (int)args[4];
            }
            else
            {
                crushColor = ItemColor.fRandom;
            }
            int x = itemObtainer.itemInfo.posX;
            int y = itemObtainer.itemInfo.posY;
            eSpecial = special;

            Action action = delegate
            {
                if (eSpecial == ItemSpecial.fColor || eSpecial == ItemSpecial.fNormal || eSpecial == ItemSpecial.Prop || eSpecial == ItemSpecial.Skill)
                {
                    M3GameManager.Instance.AffectBoomGrid(x, y);
                    M3GameManager.Instance.AffectBoomItem(x, y);
                }
                else if (needEffectNeighour)
                {
                    M3GameManager.Instance.AffectBoomGrid(x, y);
                    M3GameManager.Instance.AffectBoomItem(x, y);
                    needEffectNeighour = false;
                }

                itemObtainer.itemInfo.EliminateElement(this);
                if (itemObtainer.itemInfo.CheckEmpty())
                {
                    itemObtainer.RemoveFrom(x, y);
                    itemObtainer.ItemDestroy();
                }
                DestroyElement();
                itemObtainer.isCrushing = false;

                AddScore();
                if (haveEnergy)
                {
                    M3GameManager.Instance.catManager.AddEnergy(energyValue);
                }
                M3GameManager.Instance.runningSpecialCount--;
                if (eSpecial != ItemSpecial.fDoubleCol && eSpecial != ItemSpecial.fDoubleRow)
                {
                    M3GridManager.Instance.gridCells[x, y].Boom();
                }
            };
            DOMagicCat(action);

            base.ProcessSpecialEliminate(special, args, ignoreEffect);
        }

        private void DOMagicCat(Action action)
        {
            if (itemObtainer.isCrushing)
                return;

            int x = itemObtainer.itemInfo.posX;
            int y = itemObtainer.itemInfo.posY;
            M3GameManager.Instance.runningSpecialCount++;

            itemObtainer.isCrushing = true;
            if (itemObtainer.GetGrid() != null && itemObtainer.GetGrid().gridInfo.spawnPointType == DropPointType.SpawnPoint)
                itemObtainer.GetGrid().portDropSpeed = 0;

            if (crushWithOutSpecial)
            {
                FrameScheduler.instance.Add(M3Config.NormalElementDisapperFrame, delegate ()
                {
                    action();
                });
            }
            else
            {
                M3GridManager.Instance.dropLock = true;
                //var itemList = M3GameManager.Instance.GetAllSameColorItem(crushColor = (crushColor == ItemColor.fRandom ? M3GameManager.Instance.GetCurrentGridRandomColors() : crushColor));
                //for (int i = 0; i < itemList.Count; i++)
                //{
                //    if (itemList[i] != null)
                //    {
                //        itemList[i].OnColorEliminateCrush();
                //    }
                //}
                FrameScheduler.instance.Add(M3Config.MagicCatWaitFrame, delegate ()
                {
                    M3GridManager.Instance.dropLock = false;
                    EliminateManager.Instance.ProcessColorEliminate(ItemSpecial.fColor, x1, y1, x2, y2, crushColor);
                    crushWithOutSpecial = true;
                    FrameScheduler.instance.Add(M3Config.NormalElementDisapperFrame, delegate ()
                    {
                        action();
                    });
                });
            }

            if (view != null)
            {
                view.PlayAnimation(data.config.ClearAnim, true);
                if (M3GameManager.Instance.soundManager != null)
                    M3GameManager.Instance.soundManager.PlayEliminateColorSpecialAudio();
            }
        }

        public override void AddScore()
        {
            M3GameManager.Instance.modeManager.AddScore(data.config.Point, 1, M3Supporter.Instance.GetItemPositionByGrid(itemObtainer.itemInfo.posX, itemObtainer.itemInfo.posY));
            itemObtainer.specialScore = 0;
            itemObtainer.crushScore = 0;
        }

        public override void AddEnergy(int value)
        {
            base.AddEnergy(value);
            haveEnergy = true;
            energyValue = value;
            if (view != null)
                M3FxManager.Instance.PlayEnergyCornerEffect(itemObtainer.itemInfo.posX, itemObtainer.itemInfo.posY, view.eleTransform);
        }

        public override Element Clone()
        {
            var normalEle = new MagicCatElement();
            normalEle.isCrit = this.isCrit;
            normalEle.haveEnergy = this.haveEnergy;
            normalEle.multipleRatio = this.multipleRatio;
            return Clone(normalEle);
        }

    }
}