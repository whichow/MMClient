using Game.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Build
{
    /// <summary>
    /// Ê÷¶´
    /// </summary>
    class BuildingTree : BuildingFunction
    {
        #region Interface

        
        public override void OnIntoView()
        {
            KUIWindow.OpenWindow<MapSelectWindow>();
            //KUIWindow.OpenWindow<MapSelectWindow>();
        }



        #endregion
        #region method
        protected override void OnTap()
        {
            base.OnTap();

            //BuildingManager.Instance.setBuildingFocus(1104, null);
            //GameCamera.Block(transform.gameObject, GameCamera.Restrictions.All);
            //AreaManager.Instance.FocusArea(1524);
            //BuildingManager.Instance.setBuildingFocus(1001,null);
            // BuildingManager.Instance.setBuildingFocus(1101, (bl, ev) => { });
            //BuildingManager.Instance.ShieldBuildingFun(Category.kCatHouse,false);

            //IconFlyMgr.Instance.IconPathGroupStart(this.transform.position, 0.1f, new Int3[6] {
            //    new Int3(1,20,-1),
            //    new Int3(2,20,-1),
            //    new Int3(3,20,-1),
            //    new Int3(4,20,-1),
            //    new Int3(5,20,-1),
            //    new Int3(7,20,-1),
            //});
           // Vector3 pos, int iconId, float time, GiftType type, int count
           // IconFlyMgr.Instance.IconPathGroupStart(this.transform.position, (int)KItem.GiftType.kGold, 0.1f, IconFlyMgr.GiftType.KNone,10);

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
