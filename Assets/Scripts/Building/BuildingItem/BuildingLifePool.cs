// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "BuildingLifePool" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Build
{
    /// <summary>
    /// 体力池
    /// </summary>
    public class BuildingLifePool : BuildingFunction//Building, IFunction
    {
        #region Interface

        public override bool supportInfomation
        {
            get { return true; }
        }



        #endregion
        #region method
        protected override void OnTap()
        {
            base.OnTap();

            //GameCamera.Instance.Show(this.entityView.centerNode.position);
        }

        #endregion
        #region Unity  

        // Use this for initialization
        private void Start()
        {
            entityView.ShowModel();
        }

        // Update is called once per frame
        private void Update()
        {

        }

        #endregion
    }
}
