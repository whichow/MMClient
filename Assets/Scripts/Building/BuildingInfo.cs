// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "EntityInfo" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Build
{
    /// <summary>
    /// 联网数据
    /// </summary>
    public class EntityInfo : MonoBehaviour
    {
        /// <summary>
        /// 实例id
        /// </summary>
        public int enityID
        {
            get;
            private set;
        }

        /// <summary>
        /// 本地数据id
        /// </summary>
        public int enityDataID
        {
            get;
            private set;
        }

        /// <summary>
        /// 地图网格
        /// </summary>
        public Int2 mapGrid
        {
            get;
            private set;
        }

    }
}

