// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "Area" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using UnityEngine;

namespace Game.Build
{
    public class AreaBorder : MonoBehaviour
    {
        #region Field

        public Area[] dependencies;

        /// <summary>
        /// 
        /// </summary>
        private PolygonCollider2D _polygonCollider;

        #endregion

        #region Method

        /// <summary>
        /// 设置路径
        /// </summary>
        /// <param name="index"></param>
        /// <param name="points"></param>
        public void SetPath(int index, Vector2[] points)
        {
            if (!_polygonCollider)
            {
                _polygonCollider = gameObject.AddComponent<PolygonCollider2D>();
            }
            _polygonCollider.SetPath(index, points);
        }

        /// <summary>
        /// 包含
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool IsInside(Vector3 position)
        {
            return _polygonCollider && _polygonCollider.OverlapPoint(position);
        }

        private void Check()
        {
            if (this.dependencies.Length == 1)
            {
            }
            else
            {
            }
        }

        private void Check(bool unlock)
        {
            this.Check();
        }

        #endregion

        #region Unity

        private void Awake()
        {
            if (this.dependencies != null)
            {

            }
        }

        private void OnDestroy()
        {
            if (this.dependencies != null)
            {
            }
        }

        #endregion
    }
}
