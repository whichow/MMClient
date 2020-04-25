using Game.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Build
{
    /// <summary>
    /// 拍照小摊
    /// </summary>
    class BuildingTakePhotos : BuildingFunction
    {
        #region Interface

        public override void OnIntoView()
        {
            KUIWindow.OpenWindow<PhotoShopWindow>();
            Debug.Log("拍照小摊");
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
