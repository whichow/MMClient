using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    class ColorChange
    {
        private Shader _colorShader;
        public virtual Shader  ColorShader
        {
          get
            {
                if(!_colorShader)
                    _colorShader = Shader.Find("Custom/SkeletonColor");
                return _colorShader;
            }
        }
        private Material colorMaterial;
        public virtual Material ColorMaterial
        {
            get
            {
                if (!colorMaterial)
                {

                    colorMaterial = new Material(ColorShader);
                    if (currMarerial)
                    {
                        colorMaterial.SetTexture("_MainTex", currMarerial.mainTexture);
                        colorMaterial.SetColor("_Emission", new Color(255,0,0,255));
                        Debug.Log(colorMaterial.GetColor("_Emission"));
                        Debug.Log(colorMaterial.GetTexture("_MainTex"));
                    }
               
                }
                return colorMaterial;
            }
        }
        private Material currMarerial;
        private string textName;
        public virtual string TextName
        {
            get
            {
                if (string.IsNullOrEmpty(textName))
                    textName = "_MainTex";
                return textName;
            }
            set
            {
                textName = value;
            }
        }
        Renderer meshRenderer;
        public ColorChange(ref Renderer meshRenderer)
        {
            if (meshRenderer)
            {
                this.meshRenderer = meshRenderer;
                currMarerialSet(meshRenderer.material);
            }
        }

        private void currMarerialSet(Material material)
        {

            currMarerial = material;
        }
        public void ColorSet()
        {
            if (meshRenderer)
            {
                this.meshRenderer.material = ColorMaterial;
            }
        }
        public void ColorRecovery()
        {

            this.meshRenderer.material = currMarerial;
        }
        //Custom/SkeletonColor
    }
}
