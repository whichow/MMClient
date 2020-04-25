using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Game.Match3
{
    public class M3ItemView : MonoBehaviour
    {
        #region Field

        /// <summary>
        /// 表现层位移动画
        /// </summary>
        private Animation _rAnimation;
        /// <summary>
        /// 表现层表现动画
        /// </summary>
        private Animator _rAnimator;
        /// <summary>
        /// 
        /// </summary>
        private Transform _rootTransform;

        public M3Item obtainer;
        public Transform elementRoot;

        public Transform itemTransform;

        public GameObject itemGameobject;

        public Vector3 cacheLocalPosition;
        public void Init(GameObject obj, M3Item item)
        {
            itemTransform = obj.transform;
            itemGameobject = obj;
            elementRoot = itemTransform.Find("Root");
            obtainer = item;
        }

        public void AnimationBack(Vector3 target)
        {
            StartCoroutine(_AnimationBack(target));
        }

        private IEnumerator _AnimationBack(Vector3 target)
        {
            obtainer.isTweening = true;
            var tweener = _rootTransform.DOLocalMove(target, 0.2f);
            tweener.OnComplete(delegate () { obtainer.isTweening = false; });
            tweener.SetLoops(2, LoopType.Yoyo);
            yield return tweener.WaitForCompletion();
            _rootTransform.localPosition = Vector3.zero;
        }

        #endregion

        #region 处理下落


        #endregion

        #region Unity

        public void Awake()
        {
            _rootTransform = transform.Find("Root");
            _rAnimation = _rootTransform.GetComponent<Animation>();
            _rAnimator = _rootTransform.GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            //if (transform.localPosition != cacheLocalPosition)
            //{
            //    transform.localPosition = cacheLocalPosition;
            //}
        }
        #endregion
    }
}