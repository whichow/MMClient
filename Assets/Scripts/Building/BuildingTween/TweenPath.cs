using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [AddComponentMenu("BuildingTeween/TweenPath")]
    public class TweenPath : TweenBase
    {
        float curValue = 1f;
        public float MoveUnitX = 1;
        public float MoveUnitY = 1;

        public AnimationCurve PathCurve = new AnimationCurve(new Keyframe(0, 0, 0, 1), new Keyframe(1, 1, 1, 0));
        //public float FromPos = 0;
        //public float ToPos = 1;
        //public float Speed = 1;

        private Vector2 _originalPos;
        private int length;
        private float _xMax
        {
            get
            {
                if (PathCurve.keys.Length > 0)
                    return PathCurve.keys[length - 1].time;
                else
                    return 0;
            }
        }
        private float _xMin
        {
            get
            {
                if (PathCurve.keys.Length > 0)
                    return PathCurve.keys[0].time;
                else
                    return 0;
            }
        }
        Vector2 _pos = new Vector2();
        Vector2 pos
        {

            get
            {
                _pos.x = value * MoveUnitX;
                _pos.y = PathCurve.Evaluate(value) * MoveUnitY;
                return _pos;
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
                transform.localPosition = _originalPos + pos;
            }
        }
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
        }


        //public float timeTest;
        //动画更行是调用
        protected override void TweenUpData(float time, bool isFinish)
        {
            //Debug.Log("time"+ time);
            if (value <= _xMax)
            {
                value = Mathf.Lerp(_xMin, _xMax, time/** Speed * Time.deltaTime*/);
                //transform.position = position + pos;
            }

        }


    }
}