using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [AddComponentMenu("BuildingTeween/Other/TweenSclOther")]
    class TweenSclOther : TweenBase
    {
        public Vector3 from;
        public Vector3 to;
        public Transform transPos;
        public bool isLuanchBounds;

        private Vector3 _originalScale;

        private Vector3 _value;
        private Vector3 vect;
        private Vector3 _originalPos;
        private Renderer _renderer;
        private MeshFilter _meshFilter;
        private Collider2D _collider2D;
        /// <summary>
        /// 上下边缘距离中心点的比例系数
        /// </summary>
        private struct ratioCenter
        {
            /// <summary> 标记是否初始化</summary>
            private bool isNew;
            public float Left;
            public float Right;
            public float Bottom;
            public float Top;

            public ratioCenter(
            float left,
            float right,
            float bottom,
            float top)
            {
                this.isNew = true;
                this.Left = left;
                this.Right = right;
                this.Bottom = bottom;
                this.Top = top;
            }
            public override string ToString()
            {
                return string.Format("calss{0} Left:{1},Right:{2},Bottom:{3},Top:{4}", base.ToString(), Left, Right, Bottom, Top);

            }
        }
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
        private ratioCenter _ratioCenter;
        public AlignmentType _alignmentType;
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

                if (_alignmentType == AlignmentType.Center || _alignmentType == AlignmentType.Default)
                {

                    return;
                }

                Vector3 delta = scale - _originalScale;
                Vector3 pos = Vector3.zero;
                if (_alignmentType == AlignmentType.Left)
                {
                    pos = _originalPos + new Vector3(delta.x * _ratioCenter.Left, 0, delta.z);
                }
                else if (_alignmentType == AlignmentType.Right)
                {
                    pos = _originalPos + new Vector3(-delta.x * _ratioCenter.Right, 0, delta.z);
                }
                else if (_alignmentType == AlignmentType.Top)
                {
                    pos = _originalPos + new Vector3(0, -delta.y * _ratioCenter.Top, delta.z);
                }
                else if (_alignmentType == AlignmentType.Bottom)
                {
                    pos = _originalPos + new Vector3(0, delta.y * _ratioCenter.Bottom, delta.z);
                }
                else if (_alignmentType == AlignmentType.LeftTop)
                {
                    pos = _originalPos + new Vector3(delta.x * _ratioCenter.Left, -delta.y * _ratioCenter.Top, delta.z);
                }
                else if (_alignmentType == AlignmentType.LeftBottom)
                {
                    pos = _originalPos + new Vector3(delta.x * _ratioCenter.Left, delta.y * _ratioCenter.Bottom, delta.z);
                }
                else if (_alignmentType == AlignmentType.RightTop)
                {
                    pos = _originalPos + new Vector3(-delta.x * _ratioCenter.Right, -delta.y * _ratioCenter.Top, delta.z);
                }
                else if (_alignmentType == AlignmentType.RightBottom)
                {
                    pos = _originalPos + new Vector3(-delta.x * _ratioCenter.Right, delta.y * _ratioCenter.Bottom, delta.z);
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

        private void calculateRatioCenter()
        {

            if (_collider2D)
            {
                Bounds bounds = _collider2D.bounds;
                _ratioCenter.Left = (transform.position - bounds.min).x / bounds.size.x;
                _ratioCenter.Right = (bounds.max - transform.position).x / bounds.size.x;
                _ratioCenter.Bottom = (transform.position - bounds.min).y / bounds.size.y;
                _ratioCenter.Top = (bounds.max - transform.position).y / bounds.size.y;
            }
            else if (_meshFilter)
            {
                Bounds bounds = _meshFilter.mesh.bounds;
                _ratioCenter.Left = (transform.position - bounds.min).x / bounds.size.x;
                _ratioCenter.Right = (bounds.max - transform.position).x / bounds.size.x;
                _ratioCenter.Bottom = (transform.position - bounds.min).y / bounds.size.y;
                _ratioCenter.Top = (bounds.max - transform.position).y / bounds.size.y;
            }
            else if (_renderer)
            {
                Bounds bounds = _renderer.bounds;
                _ratioCenter.Left = (transform.position - bounds.min).x / bounds.size.x;
                _ratioCenter.Right = (bounds.max - transform.position).x / bounds.size.x;
                _ratioCenter.Bottom = (transform.position - bounds.min).y / bounds.size.y;
                _ratioCenter.Top = (bounds.max - transform.position).y / bounds.size.y;

            }
            else
            {
                _ratioCenter = new ratioCenter(0.5f, 0.5f, 0.5f, 0.5f);
            }
            Debug.Log("边界" + _ratioCenter.ToString());
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
            _renderer = this.GetComponent<Renderer>();
            _meshFilter = this.GetComponent<MeshFilter>();
            _collider2D = this.GetComponent<Collider2D>();
            if (isLuanchBounds)
                calculateRatioCenter();
        }
    }
}
