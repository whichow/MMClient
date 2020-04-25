using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [AddComponentMenu("BuildingTeween/TweenPosTarget")]
    public class TweenPosTarget : TweenBase
    {
        private Vector3 _targetPos;
        public Vector3 targetPos
        {
            get
            {
                return _targetPos;
            }
            set
            {
                _targetPos = value;
                _delta = targetPos - _originalPos;
            }
        }

        private Vector3 curPos;
        private Vector3 _valuePos;
        private Vector3 _originalPos;

        private Vector3 _delta;
        private Vector3 _startPos = Vector3.zero;
        private Vector3 _ValuePos
        {
            get
            {
                return _valuePos;
            }
            set
            {

                _valuePos = value;
                this.transform.localPosition = _originalPos + _valuePos;
            }
        }

        protected override void TweenUpData(float time, bool isFinish)
        {
            // _delta = targetPos - _originalPos;
            _ValuePos = Vector3.Lerp(_startPos, _delta, time);//Vector3Clamp(from, to, time);
        }
        private Vector3 Vector3Clamp(Vector3 from, Vector3 to, float time)
        {
            curPos.x = Mathf.Lerp(from.x, to.x, time);
            curPos.y = Mathf.Lerp(from.y, to.y, time);
            curPos.z = Mathf.Lerp(from.z, to.z, time);
            return curPos;
        }
        /// <summary>
        /// 重新设置起始位置
        /// </summary>
        /// <param name="beginPos"></param>
        public void SetBeginPos(Vector3 beginPos)
        {
            _originalPos = beginPos;
            this.transform.localPosition = _originalPos;
            _delta = targetPos - _originalPos;
        }
        //public void SetTargetPos(Vector3 targetPos)
        //{
        //    this.targetPos = targetPos;
        //    _delta = targetPos - _originalPos;
        //}
        /// <summary>
        /// 重置位置
        /// </summary>
        public void ResetPos()
        {
            this.transform.localPosition = _originalPos;
            _delta = targetPos - _originalPos;
        }
        /// <summary>
        /// 刷新位置到当前位置
        /// </summary>
        public void RefurbishPos()
        {
            _originalPos = this.transform.localPosition;
        }
        protected override void TweenAwake()
        {
            base.TweenAwake();
            _originalPos = this.transform.localPosition;
            _delta = targetPos - _originalPos;

        }
    }
}