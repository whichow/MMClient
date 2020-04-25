/** 
*FileName:     ElementView.cs 
*Author:       HeJunJie 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2017-08-14 
*Description:    
*History: 
*/
using Spine.Unity;
using System;
using UnityEngine;

namespace Game.Match3
{
    /// <summary>
    /// 元素显示对象
    /// </summary>
    public class ElementView : MonoBehaviour
    {
        public Transform eleTransform;
        public GameObject eleGameObject;
        public Animation anima;
        public Animator animator;
        public SkeletonAnimation skeletonAnimation;
        public Element element;
        public ElementCorner corner;
        public int sortLayer;
        private System.Action onFinish;

        public virtual void Init(Element e, Transform trans)
        {
            element = e;
            eleTransform = trans;
            eleGameObject = trans.gameObject;
            animator = eleGameObject.GetComponentInChildren<Animator>();
            skeletonAnimation = eleGameObject.GetComponentInChildren<SkeletonAnimation>();
            sortLayer = e.data.config.Level;
            if (skeletonAnimation != null)
            {
                if (!string.IsNullOrEmpty(e.data.config.Skin))
                {
                    //skeletonAnimation.initialSkinName = e.data.config.skin;
                    skeletonAnimation.skeleton.SetSkin(e.data.config.Skin);
                    //skeletonAnimation.Reset();
                }
                skeletonAnimation.AnimationState.Complete += CompleteDelegate;
            }

            Vector3 vec = eleTransform.localPosition;
            eleTransform.localEulerAngles = Vector3.zero;
            eleTransform.localPosition = new Vector3(vec.x, vec.y, -0.001f * sortLayer);
            PlayAnimation(e.data.config.IdleAnim, true);
            corner = M3FxManager.Instance.PlayM3CommonEffect((int)MatchEffectType.ElementCorner, eleGameObject).AddComponent<ElementCorner>();
            corner.transform.localPosition = new Vector3(0, 0, -0.1f);
        }

        private void CompleteDelegate(Spine.TrackEntry trackEntry)
        {
            OnComplete();
        }

        private void OnComplete()
        {
            if (this.onFinish != null)
            {
                this.onFinish();
                this.onFinish = null;
            }
        }

        public virtual void RefreshView(Transform parent = null, bool center = true)
        {
            Vector3 vec;
            if (!center)
                vec = eleTransform.localPosition;
            else
                vec = Vector3.zero;
            //transform.localPosition = new Vector3(vec.x, vec.y, -0.001f * sortLayer);
            KTweenUtils.LocalMoveTo(transform, new Vector3(vec.x, vec.y, -0.001f * sortLayer), 0);
        }

        public void ChangeViewSkin(string skinName)
        {
            if (skeletonAnimation != null)
            {
                skeletonAnimation.skeleton.SetSkin(skinName);
            }
        }

        public void PlayAnimation(string animation, bool loop = false)
        {
            if (skeletonAnimation != null && skeletonAnimation.state.Data.SkeletonData.FindAnimation(animation) != null)
            {
                skeletonAnimation.AnimationName = null;
                skeletonAnimation.loop = loop;
                skeletonAnimation.AnimationName = animation;
            }
            else if (animator != null)
            {
                animator.Play(animation, -1, 0);
            }
        }

        public void PlayAnimation(string animation, Action action, bool loop = false)
        {
            if (skeletonAnimation != null && skeletonAnimation.state.Data.SkeletonData.FindAnimation(animation) != null)
            {
                skeletonAnimation.AnimationName = null;
                skeletonAnimation.loop = loop;
                skeletonAnimation.AnimationName = animation;
                this.onFinish = action;
            }
        }

        public virtual void PlayTweenAnim(string currentName, string nextName, Action callBack = null)
        {
            //PlayAnimator(currentName, () => { PlayAnimation(nextName); if (callBack != null) callBack(); });
            PlayAnimation(currentName);
            Action action = delegate ()
            {
                PlayAnimation(nextName);
                if (callBack != null)
                    callBack();
            };
            this.onFinish = action;
        }

        public void DestroyAllEffect()
        {
            int count = eleTransform.childCount;
            for (int i = count - 1; i >= 0; i--)
            {
                GameObject.Destroy(eleTransform.GetChild(i).gameObject);
            }
        }

        public void PlayEliminateFx()
        {
            M3FxManager.Instance.PlayCashStarEffect(element.itemObtainer.coordinate.x, element.itemObtainer.coordinate.y);
        }

        public virtual void Dispose()
        {

        }

    }
}