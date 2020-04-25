using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Build
{
    class BuildingBubble : MonoBehaviour
    {
        #region Field


        /// <summary>
        /// 
        /// </summary>
        private Transform _target;
        /// <summary>
        /// 
        /// </summary>
        private RectTransform _rectTrans;

        #endregion

        #region Method

        protected virtual void Init()
        {

        }

        public virtual void Show()
        {
            this.gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            this.gameObject.SetActive(false);
            _target = null;
        }
        //设置UI跟随的目标，并刷新位置
        public virtual void FollowTarget(Transform target)
        {
            _target = target;
            LateUpdate();
        }

        #endregion

        #region Unity

        protected virtual void Awake()
        {
            _rectTrans = GetComponent<RectTransform>();
        }

        protected virtual void Start()
        {
 
        }

        protected virtual void LateUpdate()
        {
            if (_target)
            {
                var anchoredPosition = BubbleManager.Instance.ScreenPointToLocalPointInRectangle(_target.position);
                _rectTrans.anchoredPosition = anchoredPosition;
            }
        }

        #endregion
    }
}
