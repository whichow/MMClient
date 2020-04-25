using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [AddComponentMenu("BuildingTeween/Other/TweenColorBuilding")]

    public class TweenColorBuilding : TweenBase
    {
        public Color from = new Color(1, 1, 1, 1);
        public Color to = new Color(1, 1, 1, 1);

        private Renderer _curRenderer;

        private Color _curColor;

        string _colorName = "_ColorOffSet";
        //private ShaderColor _shaderColor;

        private Color _originalColor;
        private MaterialPropertyBlock materialProperty;
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
                setColor(_valueColor);
                //if (_curRenderer)
                //{

                //    //_shaderColor.SetColor(_valueColor);
                //    //rawImage.color = colorClamp01(_valueColor);
                //}

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
        private void setColor(Color color)
        {
            if (_curRenderer)
            {
                materialProperty.SetColor(_colorName, color);
                _curRenderer.SetPropertyBlock(materialProperty);
            }
        }
        private void init()
        {

            _curRenderer = this.GetComponent<Renderer>();
            if (_curRenderer)
            {
                //_shaderColor = new ShaderColor(_curRenderer);
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
            materialProperty = new MaterialPropertyBlock();
            //    _originalColor = image.color;


            //        private Image image;
            //private Texture texture;
            //_originalPos = this.transform.localPosition;

        }
    }
}
