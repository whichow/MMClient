using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [AddComponentMenu("BuildingTeween/TweenSclVec3")]
    class TweenSclVec3 : TweenBase
    {
        public Vector3 from;
        public Vector3 to;
        public Transform transPos;

        private Vector3 _originalScale;

        private Vector3 _value;
        private Vector3 vect;
        private Vector3 _originalPos;
        public enum AlignmentType
        {
            Default = 0,
            Left,
            Right,
            Center,
            Bottom,
            Top,
            LeftTop,
            LeftBottom,
            RightTop,
            RightBottom,

        }
        public AlignmentType alignmentType;
        public Vector3 value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                Vector3 scale = Vector3MUL(_originalScale, _value);

                this.transform.localScale = scale;

                if (alignmentType == AlignmentType.Center || alignmentType == AlignmentType.Default)
                {

                    return;
                }
                Vector3 delta = scale - _originalScale;
                Vector3 pos = Vector3.zero;
                if (alignmentType == AlignmentType.Left)
                {
                    pos = _originalPos + new Vector3(delta.x, 0, delta.z) * 0.5f;
                }
                else if (alignmentType == AlignmentType.Right)
                {
                    pos = _originalPos + new Vector3(-delta.x, 0, delta.z) * 0.5f;
                }
                else if (alignmentType == AlignmentType.Top)
                {
                    pos = _originalPos + new Vector3(0, -delta.y, delta.z) * 0.5f;
                }
                else if (alignmentType == AlignmentType.Bottom)
                {
                    pos = _originalPos + new Vector3(0, delta.y, delta.z) * 0.5f;
                }
                else if (alignmentType == AlignmentType.LeftTop)
                {
                    pos = _originalPos + new Vector3(delta.x, -delta.y, delta.z) * 0.5f;
                }
                else if (alignmentType == AlignmentType.LeftBottom)
                {
                    pos = _originalPos + new Vector3(delta.x, delta.y, delta.z) * 0.5f;
                }
                else if (alignmentType == AlignmentType.RightTop)
                {
                    pos = _originalPos + new Vector3(-delta.x, -delta.y, delta.z) * 0.5f;
                }
                else if (alignmentType == AlignmentType.RightBottom)
                {
                    pos = _originalPos + new Vector3(-delta.x, delta.y, delta.z) * 0.5f;
                }
                this.transform.localPosition = pos;
            }
        }
        private Vector3 Vector3MUL(Vector3 vector1, Vector3 vector2)
        {
            vector1.x *= vect.x == 0 ? 1 : vector2.x;
            vector1.y *= vect.y == 0 ? 1 : vector2.y;
            vector1.z *= vect.z == 0 ? 1 : vector2.z;
            return vector1;
        }
        private void LateUpdate()
        {

        }
        protected override void TweenUpData(float time, bool isFinish)
        {
            value = Vector3.Lerp(from, to, time);
        }
        protected override void TweenAwake()
        {
            base.TweenAwake();
            _originalScale = this.transform.localScale;
            _originalPos = this.transform.localPosition;
            vect = from + to;
        }
    }
}