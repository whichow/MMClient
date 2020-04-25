// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 2017-11-02
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "IndicatorBox" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public partial class IndicatorBox
    {
        #region Field

        private Transform _indicator;

        #endregion

        #region Method

        public void InitView()
        {
            _indicator = Find<Transform>("Indicator");
        }

        public void RefreshView()
        {
            _indicator.Rotate(Vector3.forward, GetRotateSpeed(), Space.Self);
        }

        #endregion
    }
}
