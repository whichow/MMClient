using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Build
{
    [AddComponentMenu("BuildingTeween/Other/IdleToTouch")]
    class IdleToTouch : MonoBehaviour, SceneClickEventBase
    {
        private SkeletonAnimation skeletonAnimation;
        private Spine.AnimationState animationState;
        private List<Spine.Animation> animLst;
        public void OnFocus(bool focus)
        {
        }
        private void Play(int animIndex, bool isLoop = false)
        {
            if (skeletonAnimation)
            {
                skeletonAnimation.loop = isLoop;
                //skeletonAnimation.Reset();
                if (animIndex < animLst.Count)
                    skeletonAnimation.AnimationName = animLst[animIndex].Name;
            }
        }
        private void animComplete(Spine.TrackEntry trackEntry)
        {
            if (trackEntry.Loop)
            {
                return;
            }
            Play(0, true);
        }
        public void OnLongPress()
        {
        }

        public void OnTap()
        {
            Play(1);
        }

        private void Start()
        {
            skeletonAnimation = GetComponent<SkeletonAnimation>();
            if (skeletonAnimation)
            {
                animationState = skeletonAnimation.AnimationState;
                animationState.Complete += animComplete;
                animLst = new List<Spine.Animation>(animationState.Data.SkeletonData.Animations);
                Play(0, true);
            }
            this.gameObject.layer = LayerManager.ClickerLayer;
        }
    }
}
