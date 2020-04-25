using Game.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game.Build
{
    /// <summary>
    /// 探索地带
    /// </summary>
    class BuildingExpeditionZone : BuildingFunction
    {
        #region Interface

        public override void OnIntoView()
        {
            KExplore.Instance.GetAllTask((int code, string message, object data)=>
            {
                if (code == 0)
                    KUIWindow.OpenWindow<DiscoveryWindow>();
                else
                    Debug.Log("探索地带进入失败");
            });
            Debug.Log("探索地带");
            
                

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
