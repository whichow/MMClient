using UnityEngine;
using System.Collections;

namespace Game
{
    public class MultiResolution : MonoBehaviour
    {
        private static float baseWidth = 1334f;
        private static float baseHeight = 750f;

        private float _baseRatio;
        private float _percentScale;

        /// <summary>
        /// scale tranform by width and high of scene
        /// </summary>
        private void AdjustScale()
        {
#if UNITY_ANDROID || UNITY_IPHONE
            var _tranform = transform;
            _baseRatio = baseWidth / baseHeight * Screen.height;
            _percentScale = Screen.width / _baseRatio;
            _tranform.localScale = new Vector3(_tranform.localScale.x * _percentScale, _tranform.localScale.y, 1f);
#endif
        }
        #region UNITY

        private void Start()
        {
            AdjustScale();
        }

        #endregion
    }
}