// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KLightFx" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using UnityEngine;
using System.Collections;

namespace Game
{
    public class KLightmapFx : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        public int lightmapIndex;
        /// <summary>
        /// 
        /// </summary>
        public Vector4 lightmapTilingOffset = new Vector4(1f, 1f, 0f, 0f);

        #region MONOBEHAVIOUR

        /// <summary>
        /// Use this for initialization
        /// </summary>
        private void Start()
        {
            if (this.GetComponent<Renderer>())
            {
                this.GetComponent<Renderer>().lightmapIndex = this.lightmapIndex;
                this.GetComponent<Renderer>().lightmapScaleOffset = this.lightmapTilingOffset;
            }
        }

        #endregion
    }
}