using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [AddComponentMenu("BuildingTeween/TweenPathTarget")]
    public class TweenPathTarget : TweenBase
    {
        public enum PathType
        {
            None,
            Vertacal,
            Horizontal,
            Mul,
            Sum,
        }
        float curValue = 1f;
        //public float MoveUnitX = 1;
        public float MoveUnitHeight = 1;

        //public bool isUsedWorldPos;


        //public Transform trans;
        public AnimationCurve PathCurve = new AnimationCurve(new Keyframe(0, 0, 0, 1), new Keyframe(1, 1, 1, 0));
        public Vector2 FromPos;
        public Vector2 ToPos;
        private Vector2 deltaMove;


        //public float Speed = 1;

        private Vector2 _originalPos;
        private int length;
        public PathType pathType;

        private Vector2 _lastKey;
        private Vector2 _LastKey
        {
            get
            {
                if (PathCurve.keys.Length > 0)
                {
                    _lastKey.x = PathCurve.keys[length - 1].time;
                    _lastKey.y = PathCurve.keys[length - 1].value;
                    return _lastKey;
                }

                else
                    return Vector2.zero;
            }
        }

        //private Vector2 _yLastDelta
        //{
        //    get
        //    {
        //        return 0 - _yLastKey;
        //    }
        //}
        private Vector2 _firstKey = new Vector2();
        private Vector2 _FirstKey
        {
            get
            {
                if (PathCurve.keys.Length > 0)
                {
                    _firstKey.x = PathCurve.keys[0].time;
                    _firstKey.y = PathCurve.keys[0].value;
                    return _firstKey;
                }

                else
                    return Vector2.zero;
            }
        }
        Vector2 _slopeLineDir = new Vector2();
        /// <summary>
        /// 曲线第一个点和最后一个点斜率
        /// </summary>
        private float _slopeLine
        {
            get
            {
                _slopeLineDir = _LastKey - _FirstKey;
                float slope = _slopeLineDir.y / _slopeLineDir.x;
                if (float.IsNaN(slope) || float.IsInfinity(slope))
                    return 0;
                return slope;
            }
        }
        Vector2 _curvePos = new Vector2();
        Vector2 curvePos
        {

            get
            {
                _curvePos.x = _time * MoveUnitHeight;

                _curvePos.y = PathCurve.Evaluate(_time) * MoveUnitHeight;

                return _curvePos;
            }
        }

        Vector2 _linePos = new Vector2();
        Vector2 _LinePos
        {

            get
            {
                if (_slopeLine != 0)
                {

                    _linePos.x = _time;

                    _linePos.y = (_slopeLine * (_time - _FirstKey.x) + _FirstKey.y) * MoveUnitHeight;
                }
                else
                {
                    _linePos = Vector2.zero;
                }

                return _linePos;
            }
        }

        float _value;
        float value
        {
            get
            {

                return _value;
            }
            set
            {
                _value = value;
                Vector2 vec = Vector2.zero;
                if (pathType == PathType.None || pathType == PathType.Vertacal)
                {
                    vec = _originalPos + (new Vector2(0, curvePos.y - _LinePos.y)) + deltaMove * _time;
                }
                else if (pathType == PathType.Mul)
                {
                    vec.x = curvePos.x * deltaMove.x;
                    vec.y = curvePos.y * deltaMove.y;
                    vec = _originalPos + vec + deltaMove * _time;
                }

                transform.localPosition = vec;
                //Instantiate(trans).transform.position = transform.position;
                //Instantiate(trans).transform.position = _originalPos + deltaMove * _time;
            }
        }
        float _time;
        /// <summary>
        /// 重置初始点
        /// </summary>
        public void ResetPos()
        {
            _originalPos = this.transform.localPosition;
        }

        protected override void TweenAwake()
        {
            base.TweenAwake();
            length = PathCurve.length;
            _originalPos = transform.localPosition;
            deltaMove = ToPos - FromPos;
            if (MoveUnitHeight == 0)
            {
                MoveUnitHeight = Mathf.Max(FromPos.x, ToPos.x);
            }
        }


        //public float timeTest;
        //动画更行是调用
        protected override void TweenUpData(float time, bool isFinish)
        {
            //Debug.Log("time"+ time);
            //if (value <= _xMax)
            //{
            _time = time;
            value = Mathf.Lerp(_FirstKey.x, _LastKey.x, time/* Speed * Time.deltaTime*/);
            //    //transform.position = position + pos;
            //}

        }


    }
}