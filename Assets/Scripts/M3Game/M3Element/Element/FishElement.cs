/** 
*FileName:     FishElement.cs 
*Author:       HASEE 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2017-10-23 
*Description:    
*History: 
*/
using System;
using UnityEngine;

namespace Game.Match3
{
    /// <summary>
    /// 鱼
    /// </summary>
    public class FishElement : Element
    {

        public override void Init(int id, M3Item item)
        {
            base.Init(id, item);
            eName = M3ElementType.FishElement;
        }

        public override void OnLanded()
        {
            base.OnLanded();
            if (itemObtainer.isCrushing)
                return;
            CheckDropOut();
            SetFishAnim();
        }

        public override void OnArriveGrid()
        {
            base.OnArriveGrid();
            if (itemObtainer.isCrushing)
                return;
            CheckDropOut();
            SetFishAnim();
        }

        public void Absorb()
        {
            Int2 currentPos = itemObtainer.itemInfo.PosInt2;
            var nextPos = GetNextItem(currentPos);
            if (nextPos.x != -1 && nextPos.y != -1)
            {
                M3Item nextItem = M3ItemManager.Instance.gridItems[nextPos.x, nextPos.y];
                if (nextItem != null)
                {
                    ExchangeItem(itemObtainer, nextItem, nextPos.x <= M3GameManager.Instance.fishManager.zombiePos.x);
                }
            }
        }

        private Int2 GetNextItem(Int2 currentPos, bool isLast = false)
        {
            Int2 nextPos = new Int2(currentPos.x - 1, currentPos.y);
            if (M3GameManager.Instance.CheckValid(nextPos.x, nextPos.y))
            {
                M3Item item = M3ItemManager.Instance.gridItems[nextPos.x, nextPos.y];
                if (item == null || (item.itemInfo.GetElement() != null && item.itemInfo.GetElement().isObstacle))
                {
                    if (!isLast)
                        return GetNextItem(nextPos, true);
                    else
                        return new Int2(-1, -1);
                }
                else
                {
                    return nextPos;
                }
            }
            return new Int2(-1, -1);
        }

        private void ExchangeItem(M3Item firstItem, M3Item NextItem, bool collect = false)
        {
            Vector3 target1 = firstItem.position;
            Vector3 target2 = NextItem.position;

            M3GameManager.Instance.fishManager.zombie.SetZombiePos(delegate ()
            {
                M3GameManager.Instance.SwapItemPosition(firstItem, NextItem);
                view.PlayAnimation(data.config.Animations["Absorb"].ToString());
                M3GameManager.Instance.fishManager.zombie.PlayAbsorbAnimation();
                KTweenUtils.LocalMoveTo(firstItem.itemView.itemTransform, target2, M3Config.FishMoveTim3, () =>
                {
                    if (collect)
                    {
                        DoAbsorb();
                    }
                    else
                        SetFishAnim();
                    FrameScheduler.instance.Add(collect ? 100 : 20, delegate ()
                        {
                            M3GameManager.Instance.fishManager.zombie.SetZombiePos();
                        });
                });
                KTweenUtils.LocalMoveTo(NextItem.itemView.itemTransform, target1, M3Config.FishMoveTim3);
            });
        }

        private void SetFishAnim()
        {
            if (view != null)
            {
                if (M3GameManager.Instance.fishManager.hasZombie && itemObtainer.itemInfo.posX - M3GameManager.Instance.fishManager.zombiePos.x <= 2)
                {
                    view.PlayAnimation(data.config.Animations["Warning"].ToString());
                }
                else
                    view.PlayAnimation(data.config.IdleAnim);
            }
        }

        public void CheckDropOut()
        {
            if (itemObtainer.GetGrid().isFishExit)
            {
                DoCollect();
            }
        }

        private void DoAbsorb()
        {
            Action action = delegate
            {
                itemObtainer.itemInfo.EliminateElement(this);
                DestroyElement();
                if (itemObtainer.itemInfo.CheckEmpty())
                {
                    int x = itemObtainer.itemInfo.posX;
                    int y = itemObtainer.itemInfo.posY;
                    itemObtainer.RemoveFrom(x, y);
                    itemObtainer.ItemDestroy();
                }
                if (view != null)
                    M3GameManager.Instance.fishManager.zombie.PlayCollectAnimation();
            };
            if (view != null)
                M3FxManager.Instance.PlayFishAbsorbAnimation(view.transform, action);
        }

        private void DoCollect()
        {
            itemObtainer.isCrushing = true;
            Action action = delegate
            {
                itemObtainer.itemInfo.EliminateElement(this);
                DestroyElement();
                Debug.Log(FrameScheduler.instance.GetCurrentFrame());
                itemObtainer.isCrushing = false;
                if (itemObtainer.itemInfo.CheckEmpty())
                {
                    int x = itemObtainer.itemInfo.posX;
                    int y = itemObtainer.itemInfo.posY;
                    itemObtainer.RemoveFrom(x, y);
                    itemObtainer.ItemDestroy();
                }
            };
            M3GameManager.Instance.fishManager.OnOtherFishCollect();
            if (view == null || view.eleGameObject == null)
                return;
            else
            {
                M3FxManager.Instance.PlayFishCollectAnimation(view.transform);
            }
            FrameScheduler.instance.Add(M3Config.ElementDisapperFrame, action);
        }

        public override void Crush()
        {
            base.Crush();
            DoCollect();
        }

        public override void ProcessSpecialEliminate(ItemSpecial special, object[] args, bool ignoreEffect = false)
        {
        }

        public override Element Clone()
        {
            var bottleEle = new FishElement();
            return this.Clone(bottleEle);
        }

    }
}