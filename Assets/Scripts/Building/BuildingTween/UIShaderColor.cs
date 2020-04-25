using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    class UIShaderColor
    {

        string _textureName = "_MainTex";
        string _colorName = "_ColorOffSet";
        string _colorBrightness = "_ColorBrightness";
        private Graphic _colorGraphics;
        private bool isSet;

        private Image image;
        private RawImage rawImage;
        public UIShaderColor(Graphic colorGraphics)
        {
            _colorMaterial = new Material(_colorShader);
            _colorGraphics = colorGraphics;
            _originalMaterial = _colorGraphics.material;
            SetTexture();

        }

        Shader _colorShader
        {
            get
            {
                return Shader.Find("Custom/SkeletonColor");
            }
        }
        Material _colorMaterial;
        //{
        //    get
        //    {
        //        return new Material(_colorShader);
        //    }
        //}
        Material _originalMaterial;
        //_ColorOffSet
        //    _MainTex
        public void SetTexture()
        {
            Texture texture = _colorGraphics.mainTexture;
            _colorMaterial.SetTexture(_textureName, texture);

            //if (image)
            //{
            //    Texture texture = image.overrideSprite.texture;
            //    if (texture)
            //    {
            //        _colorMaterial.SetTexture(_textureName, texture);
            //    }
            //}
            //rawImage = _colorGraphics as RawImage;
            //if(rawImage)
            //{
            //    Texture texture = rawImage.mainTexture;
            //    _colorMaterial.SetTexture(_textureName, texture);
            //}


        }
        public void SetColor(Color color)
        {
            _colorMaterial.SetColor(_colorName, color);
            _colorGraphics.material = _colorMaterial;
            //if (image)
            //    image.material = _colorMaterial;
            //if(rawImage)
            //  rawImage.material = _colorMaterial;

            //if (isSet)
            //{
            //    isSet = true;
            //    _colorRenderer.material = _colorMaterial;
            //}
        }
        public void SetBrightness(Color color)
        {
            _colorMaterial.SetColor(_colorBrightness, color);

            _colorGraphics.material = _colorMaterial;
            //if (image)
            //    image.material = _colorMaterial;
            //if (rawImage)
            //    rawImage.material = _colorMaterial;


            //if (isSet)
            //{
            //    isSet = true;
            //    _colorRenderer.material = _colorMaterial;
            //}
        }
        public void reset()
        {
            isSet = false;
            _colorGraphics.material = _originalMaterial;
        }
    }
}