using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    [AddComponentMenu("BuildingTeween/TweenAlph")]
    //[RequireComponent(typeof(Graphic), typeof(Renderer))]
    //[RequireComponent(typeof(Graphic))]
    class TweenAlph : TweenBase
    {
        [Range(0, 1)]
        public float from = 1f;
        [Range(0, 1)]
        public float to = 1f;
        public bool isCtrlChild;

        //private Image image;
        //private RawImage rawImage;
        private Graphic _graphic;
        private Graphic[] _graphicList;
        private Color[] _originalColorList;

        private Renderer curRenderer;

        private float _originalAlpha;
        private Color _originalColor;

        private float _value;

        [HideInInspector]
        public float AlphaValue
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;

                if (_graphic)
                {
                    _originalColor.a = _value;
                    _graphic.color = _originalColor;
                }
                if (isCtrlChild)
                {
                    for (int i = 0; i < _graphicList.Length; i++)
                    {
                        _originalColorList[i].a = _value;
                        _graphicList[i].color = _originalColorList[i];
                    }
                }
                //if (rawImage)
                //{
                //    _originalColor.a =Mathf.Clamp01( _value);
                //    rawImage.color = _originalColor;
                //}


            }
        }

        private void init()
        {
            _graphic = this.GetComponent<Graphic>();
            if (_graphic)
            {
                _originalAlpha = _graphic.color.a;
                _originalColor = _graphic.color;

            }
            if (isCtrlChild)
            {
                _graphicList = GetComponentsInChildren<Graphic>();

                int length = _graphicList.Length;
                _originalColorList = new Color[length];
                for (int i = 0; i < length; i++)
                {
                    _originalColorList[i] = _graphicList[i].color;
                }
            }
            //rawImage = this.GetComponent<RawImage>();
            //if (rawImage)
            //{
            //    _originalAlpha = rawImage.color.a;
            //    _originalColor = rawImage.color;
            //    return;
            //}
            curRenderer = this.GetComponent<Renderer>();
            if (curRenderer)
            {
                return;
            }

        }

        protected override void TweenAwake()
        {
            base.TweenAwake();

            init();

        }

        protected override void TweenUpData(float time, bool isFinish)
        {
            AlphaValue = Mathf.Lerp(from, to, time);
        }
    }
}