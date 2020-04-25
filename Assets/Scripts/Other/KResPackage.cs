// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KResPackage" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using UnityEngine;

namespace Game
{
    public class KResPackage : ScriptableObject
    {
        #region Field

        public Object[] objects;

        #endregion

        #region Unity

        private void Awake()
        {
            Debug.Log("KResPackage.Awake");
        }

        private void OnDestroy()
        {
            Debug.Log("KResPackage.OnDestroy");
        }

        #endregion
    }
}