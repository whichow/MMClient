using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [AddComponentMenu("BuildingTeween/TweenScl")]
    class TweenScl : TweenBase
    {
        public Vector2 from;
        public Vector2 to;

        private Vector3 _originalScale;

        private Vector2 _value;
        public Vector2 value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                this.transform.localScale = new Vector3(
                    _originalScale.x * (_value.x == 0 ? 1 : _value.x),
                    _originalScale.y * (_value.y == 0 ? 1 : _value.y),
                    1);
            }
        }

        protected override void TweenUpData(float time, bool isFinish)
        {
            value = Vector2.Lerp(from, to, time);
        }
        protected override void TweenAwake()
        {
            base.TweenAwake();
            _originalScale = this.transform.localScale;
        }
    }
}