using UnityEngine;
using System.Collections;

namespace Game
{
    public class Resolution : MonoBehaviour
    {
        #region Field

        public Camera bgCamera;
        public float baseWidth = 1920f;
        public float baseHeight = 1080f;
        public float pixelsPerUnit = 115f;

        private float _baseRatio;
        private float _percentScale;

        #endregion

        #region Method

        private void AdjustCamera()
        {
            Camera.main.orthographic = true;
            Camera.main.orthographicSize = baseHeight * 0.5f / pixelsPerUnit;
            bgCamera.orthographic = true;
            bgCamera.orthographicSize = baseHeight * 0.5f / pixelsPerUnit;
        }

        private void AdjustScale()
        {
            var _tranform = this.transform;

            _baseRatio = baseWidth / baseHeight * Screen.height;

            _percentScale = Screen.width / _baseRatio;

            if (_percentScale < 1f)
            {
                _tranform.localScale = new Vector3(_tranform.localScale.x * _percentScale, _tranform.localScale.y * _percentScale, 1f);
            }

            //var theCamera = Camera.main;
            //float halfFOV = (theCamera.fieldOfView * 0.5f) * Mathf.Deg2Rad;
            //float aspect = theCamera.aspect;

            //float height = 20 * Mathf.Tan(halfFOV);
            //float width = height * aspect;

            //_tranform.localPosition = new Vector3(-width * 0.25f, -height * 0.25f, 0);

            //Debug.Log("w:" + width + " h:" + height);
        }

        #endregion

        #region Unity

        private void Start()
        {
            AdjustCamera();
            AdjustScale();
        }

        #endregion
    }
}