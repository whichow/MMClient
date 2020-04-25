// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "BuildingDefault" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Build
{
    /// <summary>
    /// 普通建筑
    /// </summary>
    public class BuildingDefault : Building 
    {
        protected override void OnTap()
        {
            base.OnTap();
            //GameCamera.Instance.Show(this.entityView.centerNode.position);
        }


        #region Unity  



        private void Start()
        {
            //viewTransform.localScale = Vector3.one * 0.348f;

            entityView.ShowModel();
        }

        #endregion
    }
}

