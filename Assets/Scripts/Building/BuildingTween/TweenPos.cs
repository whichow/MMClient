using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [AddComponentMenu("BuildingTeween/TweenPos")]
    public class TweenPos : TweenBase
    {
        public Vector3 from;
        public Vector3 to;

        private Vector3 curPos;
        private Vector3 _valuePos;
        private Vector3 _originalPos;
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
            _ValuePos = Vector3Clamp(from, to, time);
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
        }
        /// <summary>
        /// 重置位置
        /// </summary>
        public void ResetPos()
        {
            this.transform.localPosition = _originalPos;
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

        }
    }
}