/** 
*FileName:     M3Zombie.cs 
*Author:       HASEE 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2017-10-24 
*Description:    
*History: 
*/
using System;
using UnityEngine;

namespace Game.Match3
{
    public class M3Zombie
    {
        public GameObject gameObject;
        public Animator animator;

        private GameObject parent;

        private bool isStun;
        private int StunCount;

        private int currentPos = -99;


        public void Init()
        {
            parent = M3ItemManager.Instance.zombieParent;
            gameObject = M3FxManager.Instance.PlayM3CommonEffect((int)MatchEffectType.Zombie, parent);
            animator = gameObject.GetComponent<Animator>();
            SetZombiePos();

            isStun = false;
        }

        public void SetZombiePos(Action action = null)
        {
            var topFishPos = GetTopFish();
            var time = topFishPos.y == currentPos ? 0 : M3Config.ZombieMoveTime;
            currentPos = topFishPos.y;
            var pos = M3Supporter.Instance.GetItemPositionByGrid(0, topFishPos.y);

            KTweenUtils.LocalMoveTo(gameObject.transform, new Vector3(pos.x/*-2f*/, pos.y + 1f * M3Config.DistancePerUnit), time, action);
        }
        private Int2 GetTopFish()
        {
            for (int i = 0; i < M3Config.GridHeight; i++)
            {
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    M3Item item = M3ItemManager.Instance.gridItems[i, j];
                    if (item != null && item.itemInfo.GetElement() != null && item.itemInfo.GetElement().eName == M3ElementType.FishElement)
                    {
                        return item.itemInfo.PosInt2;
                    }
                }
            }
            return new Int2(0, 0);
        }

        public bool CheckStun()
        {
            return isStun;
        }

        public void StunZombie(int roundCount)
        {
            isStun = true;
            StunCount = roundCount;
        }

        public void Refresh()
        {
            if (StunCount > 0)
            {
                StunCount--;
            }
            else
            {
                isStun = false;
                StunCount = 0;
            }
        }
        public void PlayAbsorbAnimation()
        {
            Action action = delegate ()
            {
                M3FxManager.Instance.PlayAnimator(animator, M3Effect.GetEffectConfig((int)MatchEffectType.Zombie).animations["Idle"].ToString(), null);
            };
            M3FxManager.Instance.PlayZombieLine(new Vector3(currentPos, 0.5f, 0));
            M3FxManager.Instance.PlayAnimator(animator, M3Effect.GetEffectConfig((int)MatchEffectType.Zombie).animations["Absorb"].ToString(), action);

        }
        public void PlayCollectAnimation()
        {
            Action action = delegate () { M3FxManager.Instance.PlayAnimator(animator, M3Effect.GetEffectConfig((int)MatchEffectType.Zombie).animations["Idle"].ToString(), null); };
            M3FxManager.Instance.PlayAnimator(animator, M3Effect.GetEffectConfig((int)MatchEffectType.Zombie).animations["GetFish"].ToString(), action);
        }
    }
}