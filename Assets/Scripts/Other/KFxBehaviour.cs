
/** 
*FileName:     KFxBehaviour.cs 
*Author:       Hejunjie 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2017-11-08 
*Description:    
*History: 
*/
using Spine.Unity;
using System;
using UnityEngine;

namespace Game
{
    public class KFxBehaviour : MonoBehaviour
    {

        public static readonly string skillEffect1 = "Effect1";
        public static readonly string skillEffect2 = "Effect2";

        public static readonly string skillBoom1 = "Boom1";
        public static readonly string skillBoom2 = "Boom2";

        public static readonly string skillTime1 = "SkillTime1";
        public static readonly string skillTime2 = "SkillTime2";

        private SkeletonAnimation skeletonAnimation;

        Action destroyAction;

        public void PlayStraightEffect(Vector3 target, Vector3 to, Vector3 from, float speed, Action action)
        {
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            transform.right = to - from;
            if (skeletonAnimation != null)
            {
                skeletonAnimation.AnimationName = "animation";
                skeletonAnimation.loop = true;

            }

            target = new Vector3(target.x, target.y, transform.position.z);
            float time = Vector2.Distance(from, to) / speed;
            KTweenUtils.MoveTo(transform, target, time, delegate ()
            {
                GameObject.Destroy(gameObject);
                if (action != null)
                    action();
            });
        }

        public void PlaySelfDestroyEffect()
        {
            destroyAction = delegate () { GameObject.Destroy(this.gameObject); };

        }


        private void FinishDestroyDelegate(Spine.TrackEntry trackEntry)
        {
            if (destroyAction != null)
            {
                destroyAction();
            }
        }

        public void Awake()
        {
            skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
            if (skeletonAnimation != null)
                skeletonAnimation.AnimationState.Complete += FinishDestroyDelegate;

        }
    }
}