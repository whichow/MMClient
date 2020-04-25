using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    [AddComponentMenu("BuildingTeween/TweenCol")]
    public class TweenCol : TweenBase
    {
        public Color from = new Color(1, 1, 1, 1);
        public Color to = new Color(1, 1, 1, 1);

        private Graphic _graphic;
        private Renderer _curRenderer;

        private Color _curColor;

        private ShaderColor _shaderColor;

        private Color _originalColor;

        private Color _valueColor;
        private Color _ValueColor
        {
            get
            {
                return _valueColor;
            }
            set
            {

                _valueColor = value;
                if (_graphic)
                {

                    _graphic.color = colorClamp01(_valueColor);
                    return;
                }
                if (_curRenderer)
                {
                    _shaderColor.SetColor(_valueColor);
                    //rawImage.color = colorClamp01(_valueColor);
                }

            }
        }
        private Color colorClamp01(Color color)
        {
            _curColor.r = Mathf.Clamp01(color.r);
            _curColor.g = Mathf.Clamp01(color.g);
            _curColor.b = Mathf.Clamp01(color.b);
            _curColor.a = Mathf.Clamp01(color.a);
            return _curColor;

        }
        private void init()
        {
            _graphic = this.GetComponent<Image>();
            if (_graphic)
            {
                _originalColor = _graphic.color;
                return;
            }

            _curRenderer = this.GetComponent<Renderer>();
            if (_curRenderer)
            {
                _shaderColor = new ShaderColor(_curRenderer);
                return;
            }

        }

        protected override void TweenUpData(float time, bool isFinish)
        {
            _ValueColor = Color.Lerp(from, to, time);
        }
        protected override void TweenAwake()
        {
            base.TweenAwake();
            init();
            //    _originalColor = image.color;


            //        private Image image;
            //private Texture texture;
            //_originalPos = this.transform.localPosition;

        }
    }
}