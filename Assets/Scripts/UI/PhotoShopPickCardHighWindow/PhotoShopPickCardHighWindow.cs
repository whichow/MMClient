using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.UI
{
    public  partial class PhotoShopPickCardHighWindow :KUIWindow
    {

        public PhotoShopPickCardHighWindow() :
            base(UILayer.kNormal, UIMode.kSequenceHide)
        {
            uiPath = "PhotoShopPickCardHigh";
        }

        private void OnCloseBtnClick()
        {
            CloseWindow(this);
        }
        public override void Awake()
        {
            //InitModel();
            InitView();
        }

        public override void OnEnable()
        {
            RefreshView();
        }

        // Update is called once per frame
        public override void Update()
        {
        }

    }
}