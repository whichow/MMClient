using DG.Tweening;
using UnityEngine;

namespace Game
{
    class ShaderColor
    {
        string _textureName = "_MainTex";
        string _colorName = "_Color";
        string _colorBrightness = "_Black";

        private Renderer _colorRenderer;
        private MaterialPropertyBlock materialProperty;
        private bool isSet;
        private Tweener tweener;

        public ShaderColor(Renderer colorRenderer)
        {
            _colorRenderer = colorRenderer;
            materialProperty = new MaterialPropertyBlock();
        }

        public void SetTexture()
        {

        }

        public void SetColor(Color color)
        {
            //if(_colorRenderer.material.(_colorName))
            if (_colorRenderer == null)
                return;
            SpriteRenderer spriteRenderer = _colorRenderer as SpriteRenderer;
            if (spriteRenderer)
            {
                spriteRenderer.color = color;
            }
            else
            {
                materialProperty.SetColor(_colorName, color);
                _colorRenderer.SetPropertyBlock(materialProperty);
            }
        }

        public void SetBrightness(bool isLoop = true)
        {
            Color from = new Color(1, 1, 1, 1);
            float r = 200.0f / 255;
            Color to = new Color(r, r, r, 1);
            Color color = new Color(0, 0, 0, 1);
            DOTween.Init(true, false, LogBehaviour.ErrorsOnly);
            DOTween.defaultLoopType = LoopType.Yoyo;

            //Tween
            tweener = DOTween.To(() => from,
               x =>
               {
                   from = x;
                   SetColor(from);
               },
               to, 0.75f);
            if (isLoop)
                tweener.SetLoops(-1);
            else
                tweener.SetLoops(2);
            tweener.SetId(this.GetHashCode());
        }

        public void ResetBrightness()
        {
            try
            {
                if (tweener != null)
                    DOTween.Kill(tweener.id);
                //tweener.Kill();
            }
            catch (System.Exception ex)
            {
                Debuger.LogWarning(ex);
            }

            SetColor(Color.white);
        }

        public void SetBrightness(Color color)
        {
            materialProperty.SetColor(_colorBrightness, color);
            _colorRenderer.SetPropertyBlock(materialProperty);
        }

    }
}