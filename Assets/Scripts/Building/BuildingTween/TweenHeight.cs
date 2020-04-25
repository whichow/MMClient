using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    [AddComponentMenu("BuildingTeween/TweenHeight")]
    public class TweenHeight : TweenBase
    {
        //[Range(0, 1)]
        public float from = 1f;
        //[Range(0, 1)]
        public float to = 1f;
        public bool isUsedFactor;


        //private Image image;
        //private RawImage rawImage;
        private RectTransform rectTransform;
        private float heitht = 0;
        private float _value;

        [HideInInspector]
        public float heightValue
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;

                if (isUsedFactor)
                    rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, heitht * _value);
                else
                    rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _value);
            }
        }

        private void init()
        {
            rectTransform = this.GetComponent<RectTransform>();
            heitht = rectTransform.rect.height;
        }

        protected override void TweenAwake()
        {
            base.TweenAwake();

            init();

        }

        protected override void TweenUpData(float time, bool isFinish)
        {
            heightValue = Mathf.Lerp(from, to, time);
        }
    }
}