using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Build
{
    class TimeProgress :Bubble
    {

        GameObject _parentNode;
        Transform _targetTrans;
        RectTransform _parentNodeRect;
        public BuildingTimeCounDown buildingTimeCounDown { get; private set; }

        #region Unity 
        protected override void Start()
        {

        }
        //public void Refurbish()
        //{
        //    buildingTimeCounDown.porgress =
        //}
        protected override void Awake()
        {
            base.Awake();
            _parentNode = this.gameObject;
            _parentNodeRect = this.GetComponent<RectTransform>();
            buildingTimeCounDown = this.GetComponent<BuildingTimeCounDown>();
        }
        public override void FollowTarget(Transform target)
        {
            _targetTrans = target;
            LateUpdate();
        }

        protected override void LateUpdate()
        {
            if (_targetTrans)
            {
                var anchoredPosition = BubbleManager.Instance.ScreenPointToLocalPointInRectangle(_targetTrans.position);
                _parentNodeRect.anchoredPosition = anchoredPosition;
            }
        }
        #endregion
    }
}
