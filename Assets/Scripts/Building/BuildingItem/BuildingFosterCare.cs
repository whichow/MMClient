using Game.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game.Build
{
    /// <summary>
    /// 寄养所
    /// </summary>
    class BuildingFosterCare : BuildingFunction
    {
        #region Interface


        public override void OnIntoView()
        {
            Debug.Log("寄养所");
            //AdoptionWindow.OpenAdoption(KUser.SelfPlayer.id);
        }
     
        #endregion
        #region method
        protected override void OnTap()
        {
            //base.OnTap();

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
